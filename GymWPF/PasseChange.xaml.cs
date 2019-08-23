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
using System.Windows.Media.Animation;

namespace GymWPF
{
    /// <summary>
    /// Logique d'interaction pour PasseChange.xaml
    /// </summary>
    public partial class PasseChange : Page
    {
        
        MainWindow mw;
        public PasseChange(MainWindow w)
        {
            InitializeComponent();
            this.mw = w;
        }
        //Declaration -------------------------//
        SqlConnection cn = new SqlConnection("Data Source=.;Initial Catalog=NSS_Salle_Application;Integrated Security=True");
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter("", "Data Source=.;Initial Catalog=NSS_Salle_Application;Integrated Security=True");
        DataSet ds = new DataSet();
        SqlDataReader dr;
        //------------------------------------//

        private void LoginPageBtn_Click(object sender, RoutedEventArgs e)
        {
            valid1.Visibility = Visibility.Hidden;
            valid2.Visibility = Visibility.Hidden;
            mw.connexionFrame.Navigate(new Login(mw));
            
        }

        
        public void validation()
        {            
            if (NewPassTextBox.Password.Length < 8)
            {
                valid1.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            }
            else if (NewPassTextBox.Password.Length >= 8 && NewPassTextBox.Password.Length < 11)
            {
                valid1.Fill = new SolidColorBrush(Color.FromRgb(255, 155, 0));
            }
            else
            {
                valid1.Fill = new SolidColorBrush(Color.FromRgb(0, 255, 0));
            }           
            
        }
        public void validation2()
        {
            if (NewPassTextBox.Password != ConfirmNewPassTextBox.Password)
            {
                valid2.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            }
            else
            {
                valid2.Fill = new SolidColorBrush(Color.FromRgb(0, 255, 0));
            }
        }
        private void NewPassTextBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            valid1.Visibility = Visibility.Visible;  
            validation();            
        }
        private void ConfirmNewPassTextBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            valid2.Visibility = Visibility.Visible;
            validation2();
        }

        public void changePass()
        {
            if (UsertextBox.Text=="" || OldPassTextBox.Password=="")
            {
                messageContent.Text = "Merci de saisir tout les informations";
                animateBorder(borderMessage);
            }
            else
            {
                try
                {
                    cn.Open();
                    cmd.Connection = cn;
                    cmd.CommandText = "select * from Utilisateur where UserName = '" + UsertextBox.Text.Replace("'","''") + "' and Password_User = '" + OldPassTextBox.Password.Replace("'", "''") + "'";
                    dr = cmd.ExecuteReader();
                    if (dr.Read() && NewPassTextBox.Password == ConfirmNewPassTextBox.Password && NewPassTextBox.Password.Length >= 8)
                    {
                        dr.Close();
                        cmd.CommandText = "update Utilisateur set Password_User = '" + NewPassTextBox.Password.Replace("'", "''") + "' where UserName ='" + UsertextBox.Text.Replace("'","''") + "'";
                        cmd.ExecuteNonQuery();

                        messageContent.Text = "Bien modifié";
                        animateBorder(borderMessage);


                    }
                    else
                    {
                        messageContent.Text = "Merci de confirmer votre mot de passe";
                        animateBorder(borderMessage);
                    }


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
                }
            }
            
        }

        private void ChangerPassBtn_Click(object sender, RoutedEventArgs e)
        {
            changePass();
        }
        public void animateBorder(Border c)
        {
            ((Storyboard)GridGlobal.Resources["animate"]).Begin(c);
        }

    }
}
