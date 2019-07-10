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
            loaded();
        }
        private void loaded()
        {
            cn.Open();
            cmd.Connection = cn;
            cmd.CommandText = "select d.Depense as Depense,d.date_dep as date_dep,d.prix as prix,u.UserName as UserName,s.nom_Salle as nom_Salle,t.nom_Type as nom_Type,d.IdSalle,d.IdType,d.IdUser from Depenses d join Utilisateur u on d.IdUser=u.IdUser join Salle s on d.IdSalle=s.IdSalle join Type_Sport t on d.IdType=t.IdType where d.IdSalle='"+ConnectedSalle+"' and d.IdType='"+ConnectedSport+"' and d.IdUser='"+iduser+"'";
            dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            ListViewUtilisateurs.DataContext = dt;
            cn.Close();
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
                loaded();
            }
        }
    }
}
