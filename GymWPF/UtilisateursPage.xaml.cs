using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Media.Effects;
using System.Windows.Media.Animation;

namespace GymWPF
{
    /// <summary>
    /// Logique d'interaction pour UtilisateursPage.xaml
    /// </summary>
    public partial class UtilisateursPage : Page
    {
        //Declaration -------------------------//
        SqlConnection cn = new SqlConnection("Data Source=.;Initial Catalog=NSS_Salle_Application;Integrated Security=True");
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter("", "Data Source=.;Initial Catalog=NSS_Salle_Application;Integrated Security=True");
        DataSet ds = new DataSet();
        SqlDataReader dr;
        //------------------------------------//

        MainApp dade;
        string iduser;
        public UtilisateursPage(MainApp dade, string iduser)
        {
            this.iduser = iduser;
            InitializeComponent();
            this.dade = dade;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            loaded();
            da.SelectCommand.CommandText = "select * from Utilisateur";
            da.Fill(ds, "users");
        }

        public void loaded()
        {
            cn.Open();
            cmd.Connection = cn;
            cmd.CommandText = "select s.nom_Salle,t.IdType,t.nom_Type from Salle s join SportSalle ss on s.IdSalle=ss.IdSalle join  Type_Sport t on ss.IdType=t.IdType";
            dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dt.Columns.Add("name", typeof(string)).Expression = "nom_Salle +' - '+nom_Type";
            SportsComboBox.ItemsSource = dt.DefaultView;
            SportsComboBox.DisplayMemberPath = "name";
            SportsComboBox.SelectedValuePath = "IdType";
            cn.Close();

            cn.Open();
            cmd.Connection = cn;
            cmd.CommandText = "select u.IdUser as IdUser,u.Nom as Nom,u.Prenom as Prenom,u.UserName as UserName,u.Password_User as Password_User,s.nom_Salle+' '+t.nom_Type as Sport from Utilisateur u join UtilisateurSportSalle uss on u.IdUser=uss.IdUser join Salle s on uss.IdSalle=s.IdSalle join Type_Sport t on uss.IdType=t.IdType ";
            dr = cmd.ExecuteReader();
            DataTable dt2 = new DataTable();
            dt2.Load(dr);
            ListViewUtilisateurs.DataContext = dt2;
            cn.Close();
        }

        
        private void BtnAjouter_Click(object sender, RoutedEventArgs e)
        {
            if (BtnAjouter.Content.ToString() == "Nouveau")
            {
                BtnAjouter.Content = "Ajouter";
                NomTextBox.Text = null;
                PrenomTextBox.Text = null;
                UserNameTextBox.Text = null;
                PassTextBox.Text = null;
                SportsComboBox.SelectedIndex = -1;
                ch1.IsChecked = false;
                ch2.IsChecked = false;
                ListViewUtilisateurs.UnselectAll();
            }
            else if (BtnAjouter.Content.ToString() == "Ajouter")
            
                {
                if (NomTextBox.Text == "" || UserNameTextBox.Text == "" || PrenomTextBox.Text == "" || PassTextBox.Text == "" || SportsComboBox.SelectedIndex == -1 || ch1.IsChecked == false && ch2.IsChecked == false)
                {
                    messageContent.Text = "Merci de remplire tout les champs";
                    animateBorder(borderMessage);
                }
                else
                {
                     
                    for (int i = 0; i < ds.Tables["users"].Rows.Count; i++)
                    {
                        if (ds.Tables["users"].Rows[i][3].ToString() == UserNameTextBox.Text)
                        {
                            messageContent.Text = "le pseudo est deja exist";
                            animateBorder(borderMessage);
                            return;
                        }
                    }
                    try
                    {
                       
                        cn.Open();
                        cmd.Connection = cn;

                        if (ch1.IsChecked == true)
                        {
                            cmd.CommandText = "insert into Utilisateur values ('" + NomTextBox.Text + "','" + PrenomTextBox.Text + "','" + UserNameTextBox.Text + "','" + PassTextBox.Text + "','" + true + "')";
                            cmd.ExecuteNonQuery();

                        }
                        else if (ch1.IsChecked == false)
                        {
                            cmd.CommandText = "insert into Utilisateur values ('" + NomTextBox.Text + "','" + PrenomTextBox.Text + "','" + UserNameTextBox.Text + "','" + PassTextBox.Text + "','" + false + "')";
                            cmd.ExecuteNonQuery();
                        }

                        cmd.CommandText = "select MAX(IdUser) from Utilisateur";
                        int IdUser = int.Parse(cmd.ExecuteScalar().ToString());

                        cmd.CommandText = "select IdSalle from SportSalle where IdType='" + SportsComboBox.SelectedValue + "'";
                        int IdSalle = int.Parse(cmd.ExecuteScalar().ToString());

                        cmd.CommandText = "insert into UtilisateurSportSalle values ('" + IdSalle + "','" + SportsComboBox.SelectedValue + "','" + IdUser + "')";


                        cmd.ExecuteNonQuery();

                        messageContent.Text = "Bien ajouté";
                        animateBorder(borderMessage);

                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                        MessageForm m = new MessageForm(msg);
                        m.ShowDialog();
                    }
                    finally
                    {
                        cn.Close();

                        NomTextBox.Text = null;
                        PrenomTextBox.Text = null;
                        UserNameTextBox.Text = null;
                        PassTextBox.Text = null;
                        SportsComboBox.SelectedIndex = -1;
                        ch1.IsChecked = false;
                        ch2.IsChecked = false;
                        loaded();
                    }
                }              

            }
               

        }

        private void ListViewUtilisateurs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListViewUtilisateurs.SelectedIndex != -1)
            {
                int index = ListViewUtilisateurs.SelectedIndex;
                DataRowView row = ListViewUtilisateurs.Items.GetItemAt(index) as DataRowView;

                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = "select t.IdType from Utilisateur u join UtilisateurSportSalle uss on u.IdUser=uss.IdUser join Salle s on uss.IdSalle=s.IdSalle join Type_Sport t on uss.IdType=t.IdType where uss.IdUser ='" + row.Row[0].ToString()+"'";
                int x = int.Parse(cmd.ExecuteScalar().ToString());
                cmd.CommandText = "select Valide from Utilisateur where IdUser = '" + row.Row[0].ToString() + "'";
                bool valide = Convert.ToBoolean(cmd.ExecuteScalar().ToString());
                cn.Close();
                BtnAjouter.Content = "Nouveau";
                NomTextBox.Text = row.Row[1].ToString();
                PrenomTextBox.Text = row.Row[2].ToString();
                UserNameTextBox.Text = row.Row[3].ToString();
                PassTextBox.Text = row.Row[4].ToString();
                SportsComboBox.SelectedValue = x;
                if (valide == true)                
                    ch1.IsChecked = true;                
                else                
                    ch2.IsChecked = true;                
            }           

        }

       
        private void BtnModifier_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewUtilisateurs.SelectedIndex == -1)
            {
                messageContent.Text = "veuillez selectioner une ligne";
                animateBorder(borderMessage);
            }
            else
            { if (NomTextBox.Text == "" || UserNameTextBox.Text == "" || PrenomTextBox.Text == "" || PassTextBox.Text == "" || SportsComboBox.SelectedIndex == -1 || ch1.IsChecked == false && ch2.IsChecked == false)
            {
                messageContent.Text = "Merci de remplire tout les champs";
                animateBorder(borderMessage);
            }
            else
            {
                for (int i = 0; i < ds.Tables["users"].Rows.Count; i++)
                {
                    int index = ListViewUtilisateurs.SelectedIndex;
                    DataRowView row = ListViewUtilisateurs.Items.GetItemAt(index) as DataRowView;
                    string psd = row.Row[3].ToString();

                    if (ds.Tables["users"].Rows[i][3].ToString() == UserNameTextBox.Text && psd != UserNameTextBox.Text)
                    {
                        messageContent.Text = "le pseudo est deja exist";
                        animateBorder(borderMessage);
                        return;
                    }
                }
                try
                {
                    int index = ListViewUtilisateurs.SelectedIndex;
                    DataRowView row = ListViewUtilisateurs.Items.GetItemAt(index) as DataRowView;
                    int id = int.Parse(row.Row[0].ToString());

                    cn.Open();
                    cmd.Connection = cn;
                    if (ch1.IsChecked == true)
                    {
                        cmd.CommandText = "update Utilisateur set Nom = '" + NomTextBox.Text + "' , Prenom = '" + PrenomTextBox.Text + "' , UserName = '" + UserNameTextBox.Text + "',Password_User = '" + PassTextBox.Text + "', Valide = '"+true+"' where IdUser = '" + id + "'";
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        cmd.CommandText = "update Utilisateur set Nom = '" + NomTextBox.Text + "' , Prenom = '" + PrenomTextBox.Text + "' , UserName = '" + UserNameTextBox.Text + "',Password_User = '" + PassTextBox.Text + "', Valide = '" + false + "' where IdUser = '" + id + "'";
                        cmd.ExecuteNonQuery();
                    }

                    cmd.CommandText = "select IdSalle from SportSalle where IdType='" + SportsComboBox.SelectedValue + "'";
                    int IdSalle = int.Parse(cmd.ExecuteScalar().ToString());

                    cmd.CommandText = "update UtilisateurSportSalle set IdSalle = '" + IdSalle + "', IdType = '" + SportsComboBox.SelectedValue + "' where IdUser = '" + id + "'";
                    cmd.ExecuteNonQuery();

                    messageContent.Text = "Bien modifié";
                    animateBorder(borderMessage);
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    MessageForm m = new MessageForm(msg);
                    m.ShowDialog();
                }
                finally
                {
                    cn.Close();
                    BtnAjouter.Content = "Ajouter";
                    NomTextBox.Text = null;
                    PrenomTextBox.Text = null;
                    UserNameTextBox.Text = null;
                    PassTextBox.Text = null;
                    SportsComboBox.SelectedIndex = -1;
                    ch1.IsChecked = false;
                    ch2.IsChecked = false;
                    ListViewUtilisateurs.UnselectAll();
                    loaded();
                }
            }   

            }

                   
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as FrameworkElement).DataContext;
            int index = ListViewUtilisateurs.Items.IndexOf(item);
            DataRowView row = ListViewUtilisateurs.Items.GetItemAt(index) as DataRowView;

           
            ConfirmForm c = new ConfirmForm("voulez vous vraiment supprimer ?");
            c.Owner = dade;
            dade.Opacity = 0.5;
            dade.Effect = new BlurEffect();
            if (iduser == row.Row[0].ToString())
            {
                messageContent.Text = "Vous ne pauvez pas supprimer cet utilisateur";
                animateBorder(borderMessage);
            }
            else
            {
                if ((bool)c.ShowDialog())
                {
                    cn.Open();
                    cmd.Connection = cn;
                    cmd.CommandText = "delete from Utilisateur where IdUser = '" + row.Row[0].ToString() + "'";
                    cmd.ExecuteNonQuery();
                    cn.Close();

                    messageContent.Text = "Bien supprimé";
                    animateBorder(borderMessage);

                    BtnAjouter.Content = "Ajouter";
                    NomTextBox.Text = null;
                    PrenomTextBox.Text = null;
                    UserNameTextBox.Text = null;
                    PassTextBox.Text = null;
                    SportsComboBox.SelectedIndex = -1;
                    ch1.IsChecked = false;
                    ch2.IsChecked = false;
                    ListViewUtilisateurs.UnselectAll();
                    loaded();

                }
                
            }
            dade.Opacity = 1;
            dade.Effect = null;

        }
        public void animateBorder(Border c)
        {
            ((Storyboard)GridGlobal.Resources["animate"]).Begin(c);
        }
    }
}
