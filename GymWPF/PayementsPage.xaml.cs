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

namespace GymWPF
{
    /// <summary>
    /// Logique d'interaction pour PayementsPage.xaml
    /// </summary>
    public partial class PayementsPage : Window
    {
        //declaration --------------------------------------
        SqlConnection cn = new SqlConnection("Data Source=.;Initial Catalog=NSS_Salle_Application;Integrated Security=True");
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter("", "Data Source=.;Initial Catalog=NSS_Salle_Application;Integrated Security=True");
        DataSet ds = new DataSet();
        SqlDataReader dr;
        //------------------------------------------------------


        string ConnectedSalle, ConnectedSport,id;

        MainApp dade;
        public PayementsPage(MainApp d, string id, string ConnectedSalle, string ConnectedSport)
        {
            this.ConnectedSalle = ConnectedSalle;
            this.ConnectedSport = ConnectedSport;
            this.id = id;
            InitializeComponent();
            this.dade = d;
        }

        private void loaded()
        {
            cn.Open();
            cmd.Connection = cn;
            cmd.CommandText = "select p.IdPayment as ID,c.nom+' '+c.prenom as clinet,s.nom_Salle as salle,t.nom_Type as sport,FORMAT (p.date_Payment , 'dd-MM-yyyy') as date,p.Prix as prix,c.IdClient,s.IdSalle,t.IdType from Payments p join Clients c on p.IdClient=c.IdClient join Salle s on p.IdSalle=s.IdSalle join Type_Sport t on p.IdType=t.IdType where c.IdClient='" + id+"' and s.IdSalle = '"+ConnectedSalle+"' and t.IdType = '"+ConnectedSport+"'";
            dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            ListPayments.DataContext = dt;
            cn.Close();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loaded();
        }

        private void ListPayments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListPayments.SelectedIndex != -1)
            {
                ajouter.Content = "Nouveau";
                int index = ListPayments.SelectedIndex;
                DataRowView row = ListPayments.Items.GetItemAt(index) as DataRowView;
                NomTextBox.Text = row.Row[4].ToString();
                PrixTextBox.Text = row.Row[5].ToString();
            }
        }

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as FrameworkElement).DataContext;
            int index = ListPayments.Items.IndexOf(item);
            DataRowView row = ListPayments.Items.GetItemAt(index) as DataRowView;

            MessageBoxResult messageBoxResult = MessageBox.Show("voulez vous vraiment supprimer ?", "Message", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = "delete from Payments where IdPayment = '" + row.Row[0].ToString() + "'";
                cmd.ExecuteNonQuery();
                cn.Close();
                ajouter.Content = "Ajouter";
                NomTextBox.Text = null;
                PrixTextBox.Text = null;
                ListPayments.UnselectAll();
                loaded();

            }
        }

        private void Modifier_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = ListPayments.SelectedIndex;
                DataRowView row = ListPayments.Items.GetItemAt(index) as DataRowView;
                int id = int.Parse(row.Row[0].ToString());

                cn.Open();
                cmd.Connection = cn;             

                cmd.CommandText = "update  Payments set date_Payment ='" + NomTextBox.Text + "', Prix ='" + PrixTextBox.Text + "' where IdPayment ='" + id + "'";
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
                ajouter.Content = "Ajouter";
                NomTextBox.Text = null;
                PrixTextBox.Text = null;
                ListPayments.UnselectAll();

                loaded();
            }
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            dade.Effect = null;
            dade.Opacity = 1;
            this.Hide();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ajouter.Content.ToString() == "Nouveau")
            {
                ajouter.Content = "Ajouter";
                NomTextBox.Text = null;
                PrixTextBox.Text = null;
                ListPayments.UnselectAll();
            }
            else if (ajouter.Content.ToString() == "Ajouter")
            {
                try
                {
                    cn.Open();
                    cmd.Connection = cn;
                    cmd.CommandText = "insert into Payments values ('" + NomTextBox.Text + "','" + id.ToString() + "','" + ConnectedSalle.ToString() + "','" + ConnectedSport.ToString() + "','" + PrixTextBox.Text + "')";
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
                    NomTextBox.Text = null;
                    PrixTextBox.Text = null;
                }
            }

              
        }
    }
}
