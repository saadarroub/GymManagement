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
using System.Text.RegularExpressions;

namespace GymWPF
{
    /// <summary>
    /// Logique d'interaction pour SportsPage.xaml
    /// </summary>
    public partial class SportsPage : Page
    {
        //Declaration -------------------------//
        SqlConnection cn = new SqlConnection("Data Source=.;Initial Catalog=NSS_Salle_Application;Integrated Security=True");
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter("", "Data Source=.;Initial Catalog=NSS_Salle_Application;Integrated Security=True");
        DataSet ds = new DataSet();
        SqlDataReader dr;
        //------------------------------------//

        MainApp dade;
        string ConnectedSport;
        public SportsPage(MainApp dade, string ConnectedSport)
        {
            this.ConnectedSport = ConnectedSport;
            InitializeComponent();
            this.dade = dade;
        }

        
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            loaded();
        }
        public void loaded()
        {
            cn.Open();
            cmd.Connection = cn;
            cmd.CommandText = "select * from Salle";
            dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            SallesComboBox.ItemsSource = dt.DefaultView;
            SallesComboBox.DisplayMemberPath = "nom_Salle";
            SallesComboBox.SelectedValuePath = "IdSalle";
            cn.Close();

            cn.Open();
            cmd.Connection = cn;
            cmd.CommandText = "select t.IdType as IdType,s.nom_Salle as nom_Salle,t.nom_Type as nom_Type,ss.prix as prix,s.IdSalle as IdSalle from Salle s join SportSalle ss on s.IdSalle=ss.IdSalle join  Type_Sport t on ss.IdType=t.IdType";
            dr = cmd.ExecuteReader();
            DataTable dt2 = new DataTable();
            dt2.Load(dr);
            ListViewSports.DataContext = dt2;
            cn.Close();
        }

        private void BtnAjouter_Click(object sender, RoutedEventArgs e)
        {
            if (BtnAjouter.Content.ToString() == "Nouveau")
            {
                BtnAjouter.Content = "Ajouter";
                SportName.Text = null;
                SportPrix.Text = null;
                SallesComboBox.SelectedIndex = -1;
                ListViewSports.UnselectAll();
            }
            else if (BtnAjouter.Content.ToString() == "Ajouter")
            {
                if (SportName.Text == "" || SportPrix.Text == "" ||  SallesComboBox.SelectedIndex == -1 )
                {
                    messageContent.Text = "Merci de remplire tout les champs";
                    animateBorder(borderMessage);
                }
                else
                {
                    try
                    {
                        
                            cn.Open();
                            cmd.Connection = cn;
                            cmd.CommandText = "insert into Type_Sport values ('" + SportName.Text.Replace("'","''") + "')";
                            cmd.ExecuteNonQuery();

                            cmd.CommandText = "select MAX(IdType) from Type_Sport";
                            int id = int.Parse(cmd.ExecuteScalar().ToString());

                            cmd.CommandText = "insert into SportSalle values ('" + SallesComboBox.SelectedValue + "','" + id + "','" +double.Parse(SportPrix.Text) + "')";
                            cmd.ExecuteNonQuery();

                            cn.Close();
                            loaded();
                            SportName.Text = null;
                            SportPrix.Text = null;
                            SallesComboBox.SelectedIndex = -1;


                        messageContent.Text = "Bien ajouté";
                        animateBorder(borderMessage);



                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                        MessageForm m = new MessageForm(msg);
                        m.ShowDialog();
                    }
                }               
            }

               
        }

        private void ListViewSports_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListViewSports.SelectedIndex != -1)
            {
                int index = ListViewSports.SelectedIndex;
                DataRowView row = ListViewSports.Items.GetItemAt(index) as DataRowView;

                BtnAjouter.Content = "Nouveau";
                
                SallesComboBox.SelectedValue = row.Row[4].ToString();
                SportName.Text = row.Row[2].ToString();
                SportPrix.Text = row.Row[3].ToString();
            }
            
        }

        private void BtnModifier_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewSports.SelectedIndex == -1)
            {
                messageContent.Text = "veuillez selectioner une ligne";
                animateBorder(borderMessage);
            }
            else
            {
                 if (SportName.Text == "" || SportPrix.Text == "" || SallesComboBox.SelectedIndex == -1)
            {
                messageContent.Text = "Merci de remplire tout les champs";
                animateBorder(borderMessage);
            }
            else
            {
                try
                {
                    int index = ListViewSports.SelectedIndex;
                    DataRowView row = ListViewSports.Items.GetItemAt(index) as DataRowView;
                    int id = int.Parse(row.Row[0].ToString());

                    cn.Open();
                    cmd.Connection = cn;
                    cmd.CommandText = "update Type_Sport set nom_Type = '" + SportName.Text.Replace("'","''") + "' where IdType ='" + id + "'";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "update  SportSalle set IdSalle ='" + SallesComboBox.SelectedValue + "', prix ='" +double.Parse(SportPrix.Text) + "' where IdType ='" + id + "'";
                    cmd.ExecuteNonQuery();

                    cn.Close();
                    BtnAjouter.Content = "Ajouter";
                    SportName.Text = null;
                    SportPrix.Text = null;
                    SallesComboBox.SelectedIndex = -1;
                    ListViewSports.UnselectAll();

                    loaded();


                    messageContent.Text = "Bien modifié";
                    animateBorder(borderMessage);


                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    MessageForm m = new MessageForm(msg);
                    m.ShowDialog();
                }
            }    
            }
           
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as FrameworkElement).DataContext;
            int index = ListViewSports.Items.IndexOf(item);
            DataRowView row = ListViewSports.Items.GetItemAt(index) as DataRowView;

            

            ConfirmForm c = new ConfirmForm("voulez vous vraiment supprimer ?");
            c.Owner = dade;
            dade.Opacity = 0.5;
            dade.Effect = new BlurEffect();
            if (ConnectedSport == row.Row[0].ToString())
            {
                messageContent.Text = "Vous ne pauvez pas supprimer cet Sport";
                animateBorder(borderMessage);
            }
            else
            {
                if ((bool)c.ShowDialog())
                {
                    cn.Open();
                    cmd.Connection = cn;

                    cmd.CommandText = "delete from Utilisateur where IdUser in (select u.IdUser from Utilisateur u join UtilisateurSportSalle us on u.IdUser = us.IdUser where us.IdType = '" + row.Row[0].ToString() + "')";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "delete from Type_Sport where IdType = '" + row.Row[0].ToString() + "'";
                    cmd.ExecuteNonQuery();
                    cn.Close();

                    messageContent.Text = "Bien supprimé";
                    animateBorder(borderMessage);

                    BtnAjouter.Content = "Ajouter";
                    SportName.Text = null;
                    SportPrix.Text = null;
                    SallesComboBox.SelectedIndex = -1;
                    ListViewSports.UnselectAll();
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

        private void SportPrix_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex reg = new Regex(@"\D");
            e.Handled = reg.IsMatch(e.Text);
        }
    }
}
