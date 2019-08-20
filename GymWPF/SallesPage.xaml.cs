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
using System.ComponentModel;
using System.Windows.Media.Effects;
using System.Windows.Media.Animation;

namespace GymWPF
{
    /// <summary>
    /// Logique d'interaction pour SallesPage.xaml
    /// </summary>
    public partial class SallesPage : Page
    {
        //Declaration -------------------------//
        SqlConnection cn = new SqlConnection("Data Source=.;Initial Catalog=NSS_Salle_Application;Integrated Security=True");
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter("", "Data Source=.;Initial Catalog=NSS_Salle_Application;Integrated Security=True");
        DataSet ds = new DataSet();
        SqlDataReader dr;       
        //------------------------------------//
        
        MainApp dade;
        string ConnectedSalle;
        public SallesPage(MainApp dade, string ConnectedSalle)
        {
            this.ConnectedSalle = ConnectedSalle;
            InitializeComponent();
            this.dade = dade;
            
        }
        public void LoadResoource()
        {           
           
            cn.Open();
            cmd.Connection = cn;
            cmd.CommandText = "select IdSalle,nom_Salle from Salle";
            dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            ListViewSalles.DataContext = dt;
            cn.Close();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadResoource();              
        }

        private void ListViewSalles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if(ListViewSalles.SelectedIndex != -1)
            {
                int index = ListViewSalles.SelectedIndex;
                DataRowView row = ListViewSalles.Items.GetItemAt(index) as DataRowView;

                BtnAjouter.Content = "Nouveau";
                SalleName.Text = row.Row[1].ToString();
            }
            
                
        }

        private void BtnAjouter_Click(object sender, RoutedEventArgs e)
        {
               
            if (BtnAjouter.Content.ToString() == "Nouveau")
            {
                BtnAjouter.Content = "Ajouter";
                SalleName.Text = null;
                ListViewSalles.UnselectAll();
            }
            else if (BtnAjouter.Content.ToString() == "Ajouter")
            {
                try
                {
                    if (SalleName.Text == "")
                    {
                        messageContent.Text = "Merci de remplire tout les champs";
                        animateBorder(borderMessage);
                    }
                    
                    else 
                    {
                        cn.Open();
                        cmd.Connection = cn;
                        cmd.CommandText = "insert into Salle values ('" + SalleName.Text + "')";
                        cmd.ExecuteNonQuery();
                        cn.Close();

                        messageContent.Text = "Bien ajoutée";
                        animateBorder(borderMessage);

                        ListViewSalles.UnselectAll();
                        SalleName.Text = null;
                        LoadResoource();

                    }
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    MessageForm m = new MessageForm(msg);
                    m.ShowDialog();
                }
            }
                    
        }

        private void BtnModifier_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewSalles.SelectedIndex == -1)
            {
                messageContent.Text = "veuillez selectioner une ligne";
                animateBorder(borderMessage);
            }
            else
            {            
                int index = ListViewSalles.SelectedIndex;
            DataRowView row = ListViewSalles.Items.GetItemAt(index) as DataRowView;
            int id = int.Parse(row.Row[0].ToString());

                try
                {
                    if (SalleName.Text == "")
                    {
                        messageContent.Text = "Merci de remplire tout les champs";
                        animateBorder(borderMessage);
                    }
                    else
                    {
                        cn.Open();
                        cmd.Connection = cn;
                        cmd.CommandText = "update Salle set nom_Salle = '"+SalleName.Text+ "' where IdSalle = '"+id+"'";
                        cmd.ExecuteNonQuery();
                        cn.Close();

                        messageContent.Text = "Bien modifiée";
                        animateBorder(borderMessage);

                        BtnAjouter.Content = "Ajouter";
                        SalleName.Text = null;
                        ListViewSalles.UnselectAll();
                        LoadResoource();
                    }
                   
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    MessageForm m = new MessageForm(msg);
                    m.ShowDialog();
                }
            }          
            
        }

       

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as FrameworkElement).DataContext;
            int index = ListViewSalles.Items.IndexOf(item);
            DataRowView row = ListViewSalles.Items.GetItemAt(index) as DataRowView;
            int id = int.Parse(row.Row[0].ToString());


          
            ConfirmForm c = new ConfirmForm("voulez vous vraiment supprimer ?");
            c.Owner = dade;
            dade.Opacity = 0.5;
            dade.Effect = new BlurEffect();
            if (ConnectedSalle == row.Row[0].ToString())
            {
                messageContent.Text = "Vous ne pauvez pas supprimer cet salle";
                animateBorder(borderMessage);
            }
            else
            {
                if ((bool)c.ShowDialog())
                {
                    cn.Open();
                    cmd.Connection = cn;

                    cmd.CommandText = "delete from Utilisateur where IdUser in (select u.IdUser from Utilisateur u join UtilisateurSportSalle us on u.IdUser = us.IdUser where us.IdSalle = '" + id + "')";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "delete from Salle where IdSalle = '" + id + "'";
                    cmd.ExecuteNonQuery();
                    cn.Close();

                    messageContent.Text = "Bien supprimée";
                    animateBorder(borderMessage);

                    BtnAjouter.Content = "Ajouter";
                    SalleName.Text = null;
                    ListViewSalles.UnselectAll();
                    LoadResoource();

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
