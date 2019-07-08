using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;


namespace GymWPF
{
    /// <summary>
    /// Logique d'interaction pour AjouterClient.xaml
    /// </summary>
    public partial class AjouterClient : Window
    {
        MainApp dade;
        public AjouterClient(MainApp d)
        {
            InitializeComponent();
            this.dade = d;
        }

        SqlConnection cn = new SqlConnection("Data Source=.;Initial Catalog=NSS_Salle_Application;Integrated Security=True");
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter("", "Data Source=.;Initial Catalog=NSS_Salle_Application;Integrated Security=True");
        DataSet ds = new DataSet();
        SqlDataReader dr;

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            dade.Effect = null;
            this.Hide();
        }
        string chemin;
        private void Upload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                chemin = op.FileName;
                image.Source = new BitmapImage(new Uri(chemin));

            }


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //if (NomTextBox.Text=="" || PrenomTextBox.Text=="")
            //{
            //    System.Windows.Forms.MessageBox.Show("errors");

            //}
            //else
            //{
            //    try
            //    {
            //        byte[] tabimg = null;
            //        FileStream fs = new FileStream(chemin, FileMode.Open);
            //        BinaryReader br = new BinaryReader(fs);
            //        tabimg = br.ReadBytes((int)fs.Length);
            //        fs.Close();

            //        cn.Open();
            //        cmd.Connection = cn;
            //        cmd.CommandText = "insert into Client values ('" + NomTextBox.Text + "','" + PrenomTextBox.Text + "','" + TelTextBox.Text + "','" + tabimg + "')";
            //        cmd.ExecuteNonQuery();
            //        cn.Close();
            //        System.Windows.Forms.MessageBox.Show("ok");
            //    }
            //    catch (Exception ex)
            //    {

            //        System.Windows.Forms.MessageBox.Show(ex.Message);
            //    }
            //}
            
        }
    }
}
