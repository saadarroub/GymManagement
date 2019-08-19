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
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Win32;
using System.IO;
using System.Windows.Media.Animation;

namespace GymWPF
{
    /// <summary>
    /// Logique d'interaction pour MainApp.xaml
    /// </summary>
    public partial class MainApp : Window
    {
        //Declaration -------------------------//
        SqlConnection cn = new SqlConnection("Data Source=.;Initial Catalog=NSS_Salle_Application;Integrated Security=True");
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter("", "Data Source=.;Initial Catalog=NSS_Salle_Application;Integrated Security=True");
        DataSet ds = new DataSet();
        SqlDataReader dr;
        //------------------------------------//

        SallesPage salles;
        SportsPage sports;
        UtilisateursPage utilisateurs;
        ClientsPage clients;
        DepensesPage depenses;
        Profil profil;

        string nom, prenom, ConnectedSalle, ConnectedSport,iduser;
        bool valid;


        public MainApp(string nom,string prenom, bool valid,string ConnectedSalle,string ConnectedSport,string iduser)
        {
            da.SelectCommand.CommandText = "select u.Nom,u.Prenom,s.nom_Salle,t.nom_Type,u.Valide,s.IdSalle,t.IdType,u.IdUser from Utilisateur u join UtilisateurSportSalle us on u.IdUser = us.IdUser join Salle s on s.IdSalle = us.IdSalle join Type_Sport t on t.IdType = us.IdType where s.IdSalle = '" + ConnectedSalle + "' and t.IdType = '" + ConnectedSport + "' and u.IdUser = '" + iduser + "'";
            da.Fill(ds, "infos");

            this.nom = nom;
            this.prenom = prenom;
            this.valid = valid;
            this.ConnectedSalle = ConnectedSalle;
            this.ConnectedSport = ConnectedSport;
            this.iduser = iduser;
            InitializeComponent();


            
            


        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UserName.Text = nom.ToUpper() + ' ' + prenom.ToUpper();

            da.SelectCommand.CommandText = "select u.Nom,u.Prenom,s.nom_Salle,t.nom_Type,u.Valide,s.IdSalle,t.IdType,u.IdUser from Utilisateur u join UtilisateurSportSalle us on u.IdUser = us.IdUser join Salle s on s.IdSalle = us.IdSalle join Type_Sport t on t.IdType = us.IdType where s.IdSalle = '" + ConnectedSalle + "' and t.IdType = '" + ConnectedSport + "' and u.IdUser = '" + iduser + "'";
            da.Fill(ds, "infos");

            if (valid == false)
            {
                MainAppNavBtnToSalles.IsEnabled = false;
                MainAppNavBtnToSprorts.IsEnabled = false;
                MainAppNavBtnToUsers.IsEnabled = false;

                MainAppNavBtnToSalles.Foreground = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                MainAppNavBtnToSprorts.Foreground = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                MainAppNavBtnToUsers.Foreground = new SolidColorBrush(Color.FromRgb(128, 128, 128));
            }

            profil = new Profil(this, ConnectedSalle, ConnectedSport, iduser, nom, prenom);
            MainFrame.Navigate(profil);

            if (ds.Tables["infos"].Rows.Count != 0)
            {
                
                if (Convert.ToBoolean(ds.Tables["infos"].Rows[0][4]) == true)
                {
                    Acsess.Text = "ADMIN";


                }
                else
                {
                    Acsess.Text = "EDITEUR";

                }

            }
            else
            {


                Acsess.Text = "ADMIN PRINCIPALE";

            }

            da.SelectCommand.CommandText = "select * from Utilisateur";
            da.Fill(ds, "users");

            DataTable dataTable = ds.Tables["users"];

            foreach (DataRow row in dataTable.Rows)
            {

                if (row[0].ToString() == iduser)
                {
                    if (row[6].ToString() != "")
                    {
                        byte[] blob = (byte[])row[6];
                        MemoryStream stream = new MemoryStream();
                        stream.Write(blob, 0, blob.Length);
                        stream.Position = 0;

                        System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();

                        MemoryStream ms = new MemoryStream();
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                        ms.Seek(0, SeekOrigin.Begin);
                        bi.StreamSource = ms;
                        bi.EndInit();
                        ProfilImage.Source = bi;
                    }
                    else
                    {
                        ProfilImage.Source = new BitmapImage(new Uri("/Resource/user.png", UriKind.Relative));
                    }


                }
            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
            
        }

        private void CloseAppBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MinimizeAppBtn_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        public void HideShowRecNavBtn()
        {
            RecSalles.Visibility = Visibility.Hidden;
            RecSports.Visibility = Visibility.Hidden;
            RecUsers.Visibility = Visibility.Hidden;
            RecClients.Visibility = Visibility.Hidden;
            RecDepenses.Visibility = Visibility.Hidden;
        }

        public void HideShowRecNavBtn(Rectangle active)
        {
            RecSalles.Visibility = Visibility.Hidden;
            RecSports.Visibility = Visibility.Hidden;
           
            RecUsers.Visibility = Visibility.Hidden;
            RecClients.Visibility = Visibility.Hidden;
           
            RecDepenses.Visibility = Visibility.Hidden;
            active.Visibility = Visibility.Visible;
        }
        public void HideShowMenuPanel()
        {
            if (MenuPanel.Visibility == Visibility.Collapsed)
            {
                MenuPanel.Visibility = Visibility.Visible;
            }
            else
            {
                MenuPanel.Visibility = Visibility.Collapsed;
            }
        }
       
       

        private void MainAppNavBtnToSalles_Click(object sender, RoutedEventArgs e)
        {
            HideShowRecNavBtn(RecSalles);
            salles = new SallesPage(this);
            MainFrame.Navigate(salles);
        }

        private void MainAppNavBtnToSprorts_Click(object sender, RoutedEventArgs e)
        {
            HideShowRecNavBtn(RecSports);
            sports = new SportsPage(this);
            MainFrame.Navigate(sports);
        }

        private void MainAppNavBtnToUsers_Click(object sender, RoutedEventArgs e)
        {
            HideShowRecNavBtn(RecUsers);
            utilisateurs = new UtilisateursPage(this, iduser);
            MainFrame.Navigate(utilisateurs);
        }

        private void MenuProfilBtn_Click(object sender, RoutedEventArgs e)
        {

            HideShowRecNavBtn();
            HideShowMenuPanel();
            profil = new Profil(this, ConnectedSalle, ConnectedSport, iduser, nom, prenom);
            MainFrame.Navigate(profil);
        }

        private void MenuDeconnexionBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            MainWindow m = new MainWindow();
            m.Show();
        }

        string strName, imageName;
        private void AddProfilImageBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FileDialog fl = new OpenFileDialog();
                fl.InitialDirectory = Environment.SpecialFolder.MyPictures.ToString();
                if (fl.ShowDialog() == true)
                {
                    imageName = fl.FileName;
                    ImageSourceConverter isc = new ImageSourceConverter();
                    ProfilImage.SetValue(Image.SourceProperty, isc.ConvertFromString(imageName));
                    if (imageName != null)
                    {
                        FileStream fs = new FileStream(imageName, FileMode.Open, FileAccess.Read);
                        byte[] imgByte = new byte[fs.Length];
                        fs.Read(imgByte, 0, Convert.ToInt32(fs.Length));
                        fs.Close();


                        cn.Open();
                        cmd.Connection = cn;
                        cmd.CommandText = "update Utilisateur set imgUser = @img where IdUser = '" + iduser + "'";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("img", imgByte);
                        cmd.ExecuteNonQuery();
                        cn.Close();

                    }
                }
               
               


            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                MessageForm m = new MessageForm(msg);
                m.ShowDialog();
            }
        }

        private void MainAppNavBtnToClients_Click(object sender, RoutedEventArgs e)
        {
            HideShowRecNavBtn(RecClients);
            clients = new ClientsPage(this, ConnectedSalle, ConnectedSport);
            MainFrame.Navigate(clients);
        }

        

        private void MainAppNavBtnToDepenses_Click(object sender, RoutedEventArgs e)
        {
            HideShowRecNavBtn(RecDepenses);
            depenses = new DepensesPage(this , ConnectedSalle, ConnectedSport, iduser);
            MainFrame.Navigate(depenses);
        }

        private void ShowHideMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            HideShowMenuPanel();
        }

      

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MenuPanel.Visibility = Visibility.Collapsed;
            
        }
    }
}
