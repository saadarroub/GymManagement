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
using System.Data;
using System.Data.SqlClient;
using Microsoft.Win32;
using System.IO;
using System.Drawing.Imaging;
using System.Windows.Media.Animation;
using System.Text.RegularExpressions;

namespace GymWPF
{
    /// <summary>
    /// Logique d'interaction pour ModifierClient.xaml
    /// </summary>
    public partial class ModifierClient : Window
    {
        MainApp dade;
        string id, ConnectedSalle, ConnectedSport;
        string strName, imageName;



        //declaration --------------------------------------
        SqlConnection cn = new SqlConnection("Data Source=.;Initial Catalog=NSS_Salle_Application;Integrated Security=True");
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter("", "Data Source=.;Initial Catalog=NSS_Salle_Application;Integrated Security=True");
        DataSet ds = new DataSet();
        SqlDataReader dr;
        //------------------------------------------------------
        public ModifierClient(MainApp d, string id, string ConnectedSalle, string ConnectedSport)
        {
            this.id = id;
            InitializeComponent();
            this.dade = d;
            this.ConnectedSalle = ConnectedSalle;
            this.ConnectedSport = ConnectedSport;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            dade.Effect = null;
            dade.Opacity = 1;
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            da.SelectCommand.CommandText = "select * from Clients";
            da.Fill(ds, "Clients");

            DataTable dataTable = ds.Tables["Clients"];

            foreach (DataRow row in dataTable.Rows)
            {
                
                if (row[0].ToString() == id)
                {
                    NomTextBox.Text = row[1].ToString();
                    PrenomTextBox.Text = row[2].ToString();
                    TelTextBox.Text = row[3].ToString();

                    if (row[4].ToString() != "")
                    {
                        byte[] blob = (byte[])row[4];
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
                        image.Source = bi;
                    }
                    
                    
                }
            }
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (NomTextBox.Text == "" || PrenomTextBox.Text == "")
            {
                messageContent.Text = "Merci de remplire tout les champs";
                animateBorder(borderMessage);
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
                        cmd.CommandText = "update Clients set nom = '" + NomTextBox.Text.Replace("'","''") + "', prenom ='" + PrenomTextBox.Text.Replace("'","''") + "', Tel ='" + TelTextBox.Text.Replace("'","''") + "', img = @img  where IdClient = '" + id + "'";
                        cmd.Parameters.AddWithValue("img", imgByte);
                        cmd.ExecuteNonQuery();

                        messageContent.Text = "Bien modifié";
                        animateBorder(borderMessage);

                    }
                    else
                    {
                        cn.Open();
                        cmd.Connection = cn;
                        cmd.CommandText = "update Clients set nom = '" + NomTextBox.Text.Replace("'","''") + "', prenom ='" + PrenomTextBox.Text.Replace("'","''") + "', Tel ='" + TelTextBox.Text.Replace("'","''") + "'  where IdClient = '" + id + "'";
                        cmd.ExecuteNonQuery();

                        messageContent.Text = "Bien modifié";
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

                    dade.MainFrame.Navigate(new ClientsPage(dade, ConnectedSalle, ConnectedSport));
                    dade.Effect = null;
                    dade.Opacity = 1;
                    this.Close();
                }
            }
           
        }

        private void TelTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex reg = new Regex(@"\D");
            e.Handled = reg.IsMatch(e.Text);
        }

        FileDialog fl;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                fl = new OpenFileDialog();
                fl.InitialDirectory = Environment.SpecialFolder.MyPictures.ToString();
                if (fl.ShowDialog() == true)
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
                string msg = ex.Message;
                MessageForm m = new MessageForm(msg);
                m.ShowDialog();
            }
        }

       

        public void animateBorder(Border c)
        {
            ((Storyboard)GridGlobal.Resources["animate"]).Begin(c);
        }
    }
}
