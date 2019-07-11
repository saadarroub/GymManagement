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
using System.Data;
using System.Data.SqlClient;

namespace GymWPF
{
    /// <summary>
    /// Logique d'interaction pour DepensesPage.xaml
    /// </summary>
    public partial class DepensesPage : Page
    {
        string  ConnectedSalle, ConnectedSport, iduser;     

        MainApp dade;


        //declaration --------------------------------------
        SqlConnection cn = new SqlConnection("Data Source=.;Initial Catalog=NSS_Salle_Application;Integrated Security=True");
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter("", "Data Source=.;Initial Catalog=NSS_Salle_Application;Integrated Security=True");
        DataSet ds = new DataSet();
        SqlDataReader dr;
        //------------------------------------------------------

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (ConnectedSalle.ToString() == "" && ConnectedSport.ToString() == "")
            {
                BtnAjouter.IsEnabled = false;
                BtnModifier.IsEnabled = false;
                DepensesTextBox.IsEnabled = false;
                DateTimePicker.IsEnabled = false;
                PrixTextBox.IsEnabled = false;
                BtnAjouter.Foreground = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                BtnModifier.Foreground = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = "select d.IdDep as IdDep, d.Depense as Depense,d.date_dep as date_dep,d.prix as prix,u.UserName as UserName,s.nom_Salle as nom_Salle,t.nom_Type as nom_Type,d.IdSalle,d.IdType,d.IdUser from Depenses d join Utilisateur u on d.IdUser=u.IdUser join Salle s on d.IdSalle=s.IdSalle join Type_Sport t on d.IdType=t.IdType ";
                dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                ListViewUtilisateurs.DataContext = dt;
                cn.Close();
            }
            else
            {
                loaded();

            }
        }
        private void loaded()
        {
            cn.Open();
            cmd.Connection = cn;
            cmd.CommandText = "select d.IdDep as IdDep, d.Depense as Depense,d.date_dep as date_dep,d.prix as prix,u.UserName as UserName,s.nom_Salle as nom_Salle,t.nom_Type as nom_Type,d.IdSalle,d.IdType,d.IdUser from Depenses d join Utilisateur u on d.IdUser=u.IdUser join Salle s on d.IdSalle=s.IdSalle join Type_Sport t on d.IdType=t.IdType where d.IdSalle='" + ConnectedSalle+"' and d.IdType='"+ConnectedSport+"' and d.IdUser='"+iduser+"'";
            dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            ListViewUtilisateurs.DataContext = dt;
            cn.Close();
        }

        private void ListViewUtilisateurs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListViewUtilisateurs.SelectedIndex != -1)
            {
                BtnAjouter.Content = "Nouveau";

                int index = ListViewUtilisateurs.SelectedIndex;
                DataRowView row = ListViewUtilisateurs.Items.GetItemAt(index) as DataRowView;
                DepensesTextBox.Text = row.Row[1].ToString();
                DateTimePicker.Text = row.Row[2].ToString();
                PrixTextBox.Text = row.Row[3].ToString();
            }
        }

        private void BtnModifier_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                int index = ListViewUtilisateurs.SelectedIndex;
                DataRowView row = ListViewUtilisateurs.Items.GetItemAt(index) as DataRowView;
                int id = int.Parse(row.Row[0].ToString());

                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = "update Depenses set  Depense ='" + DepensesTextBox.Text + "', date_dep = '" + DateTimePicker.Text + "', prix ='" + PrixTextBox.Text + "' where IdDep = '" + id + "'";
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
                BtnAjouter.Content = "Ajouter";
                DepensesTextBox.Text = null;
                DateTimePicker.Text = null;
                PrixTextBox.Text = null;
                ListViewUtilisateurs.UnselectAll();

                loaded();
            }
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
                cmd.CommandText = "delete from Depenses where IdDep = '" + row.Row[0].ToString() + "'";
                cmd.ExecuteNonQuery();
                cn.Close();
                BtnAjouter.Content = "Ajouter";
                DepensesTextBox.Text = null;
                DateTimePicker.Text = null;
                PrixTextBox.Text = null;
                ListViewUtilisateurs.UnselectAll();
                loaded();

            }
        }

        public DepensesPage(MainApp d, string ConnectedSalle, string ConnectedSport, string iduser)
        {
            this.ConnectedSalle = ConnectedSalle;
            this.ConnectedSport = ConnectedSport;
            this.iduser = iduser;
            InitializeComponent();
            this.dade = d;
        }
        private void BtnAjouter_Click(object sender, RoutedEventArgs e)
        {
            if (BtnAjouter.Content.ToString() == "Nouveau")
            {
                BtnAjouter.Content = "Ajouter";
                DepensesTextBox.Text = null;
                DateTimePicker.Text = null;
                PrixTextBox.Text = null;                
                ListViewUtilisateurs.UnselectAll();
            }
            else if (BtnAjouter.Content.ToString() == "Ajouter")

            {
                try
                {
                    cn.Open();
                    cmd.Connection = cn;
                    cmd.CommandText = "insert into Depenses values ('" + DepensesTextBox.Text + "','" + DateTimePicker.Text + "','" + PrixTextBox.Text + "','" + ConnectedSalle + "','" + ConnectedSport + "','" + iduser + "')";
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
                    DepensesTextBox.Text = null;
                    DateTimePicker.Text = null;
                    PrixTextBox.Text = null;
                    loaded();
                }
            }
               
        }
    }
}
