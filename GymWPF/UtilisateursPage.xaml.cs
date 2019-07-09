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
        public UtilisateursPage(MainApp dade)
        {
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
            cmd.CommandText = "select u.IdUser as IdUser,u.Nom as Nom,u.Prenom as Prenom,u.UserName as UserName,u.Password_User as Password_User,s.nom_Salle+t.nom_Type as Sport from Utilisateur u join UtilisateurSportSalle uss on u.IdUser=uss.IdUser join Salle s on uss.IdSalle=s.IdSalle join Type_Sport t on uss.IdType=t.IdType";
            dr = cmd.ExecuteReader();
            DataTable dt2 = new DataTable();
            dt2.Load(dr);
            ListViewUtilisateurs.DataContext = dt2;
            cn.Close();
        }

        private void BtnAjouter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = "insert into Utilisateur values ('" + NomTextBox.Text + "','" + PrenomTextBox.Text + "','" + UserNameTextBox.Text + "','" + PassTextBox.Text + "',@valide)";
                if (ch1.IsChecked == true)
                {
                    cmd.Parameters.AddWithValue("@valide", true);
                    cmd.ExecuteNonQuery();

                }
                else
                {
                    cmd.Parameters.AddWithValue("@valide", false);
                    cmd.ExecuteNonQuery();

                }

                cmd.CommandText = "select MAX(IdUser) from Utilisateur";
                int IdUser = int.Parse(cmd.ExecuteScalar().ToString());

                cmd.CommandText = "select IdSalle from SportSalle where IdType='"+SportsComboBox.SelectedValue+"'";
                int IdSalle = int.Parse(cmd.ExecuteScalar().ToString());

                cmd.CommandText = "insert into UtilisateurSportSalle values ('" + IdSalle + "','" + SportsComboBox.SelectedValue + "','" + IdUser + "')";


                cmd.ExecuteNonQuery();

                MessageBox.Show("ok");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                cn.Close();

                loaded();
            }

        }

        private void ListViewUtilisateurs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ListViewUtilisateurs.SelectedIndex;
            DataRowView row = ListViewUtilisateurs.Items.GetItemAt(index) as DataRowView;
            NomTextBox.Text = row.Row[1].ToString();
            PrenomTextBox.Text = row.Row[2].ToString();
            UserNameTextBox.Text = row.Row[3].ToString();
            PassTextBox.Text = row.Row[4].ToString();

        }

        private void BtnModifier_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as FrameworkElement).DataContext;
            int index = ListViewUtilisateurs.Items.IndexOf(item);
            DataRowView row = ListViewUtilisateurs.Items.GetItemAt(index) as DataRowView;

            MessageBoxResult messageBoxResult = MessageBox.Show("voulez vous vraiment supprimer ?", "Message", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = "delete from Utilisateur where IdUser = '" + row.Row[0].ToString() + "'";
                cmd.ExecuteNonQuery();
                cn.Close();
                loaded();

            }
        }
    }
}
