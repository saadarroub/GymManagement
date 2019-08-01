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
       
       

        private void AddBtn_Click_1(object sender, RoutedEventArgs e)
        {
           
            
                try
                {
                    if (NomtextBox.Text =="" || PrenomtextBox.Text =="" || UsertextBox.Text =="" || Pass.Text =="")
                    {
                    string msg = "Merci de remplire tout les champs";
                    MessageForm m = new MessageForm(msg);
                    m.ShowDialog();
                }
                else
                {
                    cn.Open();
                    cmd.Connection = cn;
                    cmd.CommandText = "insert into Utilisateur values ('" + NomtextBox.Text + "','" + PrenomtextBox.Text + "','" + UsertextBox.Text + "','" + Pass.Text + "','" + true + "')";
                    cmd.ExecuteNonQuery();
                    
                    //dire hna yrjer l conexion page

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
    }

