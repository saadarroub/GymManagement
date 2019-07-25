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
    /// Interaction logic for PricipaleUtilisateur.xaml
    /// </summary>
    public partial class PricipaleUtilisateur : Page
    {
        //declaration --------------------------------------
        SqlConnection cn = new SqlConnection("Data Source=.;Initial Catalog=NSS_Salle_Application;Integrated Security=True");
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter("", "Data Source=.;Initial Catalog=NSS_Salle_Application;Integrated Security=True");
        DataSet ds = new DataSet();
        SqlDataReader dr;
        //------------------------------------------------------

        MainWindow mw;
        public PricipaleUtilisateur(MainWindow w)
        {
            InitializeComponent();
            this.mw = w;
        }
       
        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (NomtextBox.Text == null || PrenomtextBox.Text == null || UsertextBox.Text == null ||  Pass.Text== null)
            {

            }
            else
            {
                try
                {
                    cn.Open();
                    cmd.Connection = cn;
                    cmd.CommandText = "insert into Utilisateur values ('" + NomtextBox.Text + "','" + PrenomtextBox.Text + "','" + UsertextBox.Text + "','" + Pass.Text + "','" + true + "')";
                    cmd.ExecuteNonQuery();
                    mw.connexionFrame.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    cn.Close();
                }
            }
           
        }
    }
}
