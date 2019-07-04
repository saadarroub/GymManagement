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
    /// Logique d'interaction pour Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        //Declaration -------------------------//
        SqlConnection cn = new SqlConnection("Data Source=.;Initial Catalog=NSS_Salle_Application;Integrated Security=True");
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter("", "Data Source=.;Initial Catalog=NSS_Salle_Application;Integrated Security=True");
        DataSet ds = new DataSet();
        SqlDataReader dr;
        //------------------------------------//

        
        PasseChange p;
        MainWindow mw;
        public Login( MainWindow w)
        {
            InitializeComponent();
            this.mw = w;
        }

        private void ChangerPasseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!mw.connexionFrame.CanGoForward)
            {
                p = new PasseChange(mw);
                mw.connexionFrame.Navigate(p);
            }
            else
            {
                mw.connexionFrame.GoForward();
            }
            
            
        }
        public void connexion()
        {
            try
            {
                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = "select * from Utilisateur where UserName = '" + UsertextBox.Text + "' and Password_User = '" + PassTextBox.Password + "'";
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    string nom = dr[1].ToString();
                    string prenom = dr[2].ToString();
                    MainApp app = new MainApp(nom, prenom);
                    mw.Hide();
                    app.Show();
                }
                else
                {
                    MessageBox.Show("errors");
                }
                dr.Close();

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
        
        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            connexion();            
        }

       

        private void Page_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                connexion();
            }
        }
    }
}
