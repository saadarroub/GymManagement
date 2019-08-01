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
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Win32;
using System.IO;
using System.Windows.Navigation;
using System.Windows.Shapes;



namespace GymWPF
{
    /// <summary>
    /// Logique d'interaction pour AjouterClient.xaml
    /// </summary>
    public partial class AjouterClient : Window
    {
        MainApp dade;
        string ConnectedSalle, ConnectedSport;
        public AjouterClient(MainApp d , string ConnectedSalle, string ConnectedSport)
        {
            this.ConnectedSalle = ConnectedSalle;
            this.ConnectedSport = ConnectedSport;
            InitializeComponent();
            this.dade = d;
        }

        //declaration --------------------------------------
        SqlConnection cn = new SqlConnection("Data Source=.;Initial Catalog=NSS_Salle_Application;Integrated Security=True");
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter("", "Data Source=.;Initial Catalog=NSS_Salle_Application;Integrated Security=True");
        DataSet ds = new DataSet();
        SqlDataReader dr;
        //------------------------------------------------------

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            dade.Effect = null;
            dade.Opacity = 1;
            this.Hide();
        }
        string strName, imageName;
        private void Upload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FileDialog fl = new OpenFileDialog();
                fl.InitialDirectory = Environment.SpecialFolder.MyPictures.ToString();
                fl.ShowDialog();
                {
                    strName = fl.SafeFileName;
                    imageName = fl.FileName;
                    ImageSourceConverter isc = new ImageSourceConverter();
                    image.SetValue(Image.SourceProperty, isc.ConvertFromString(imageName));
                }
                fl = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            insertclient();
        }

        private void insertclient()
        {
            if (NomTextBox.Text==""|| PrenomTextBox.Text=="")
            {
                MessageBox.Show("remplire");
            }
            else
            {
                try
                {
                    if (imageName != null)
                    {
                        FileStream fs = new FileStream(imageName, FileMode.Open, FileAccess.Read);
                        byte[] imgByte = new byte[fs.Length];
                        fs.Read(imgByte, 0, Convert.ToInt32(fs.Length));
                        fs.Close();


                        cn.Open();
                        cmd.Connection = cn;
                        cmd.CommandText = "insert into Clients values('" + NomTextBox.Text + "','" + PrenomTextBox.Text + "','" + TelTextBox.Text + "',@img)";
                        cmd.Parameters.AddWithValue("img", imgByte);
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "select MAX(IdClient) from Clients";
                        int id = int.Parse(cmd.ExecuteScalar().ToString());

                        cmd.CommandText = "insert into SportClients values ('" + id + "','" + ConnectedSalle + "','" + ConnectedSport + "')";
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("ok");

                    }
                    else
                    {
                        cn.Open();
                        cmd.Connection = cn;
                        cmd.CommandText = "insert into Clients values('" + NomTextBox.Text + "','" + PrenomTextBox.Text + "','" + TelTextBox.Text + "',NULL)";
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "select MAX(IdClient) from Clients";
                        int id = int.Parse(cmd.ExecuteScalar().ToString());

                        cmd.CommandText = "insert into SportClients values ('" + id + "','" + ConnectedSalle + "','" + ConnectedSport + "')";
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("ok");
                    }
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
