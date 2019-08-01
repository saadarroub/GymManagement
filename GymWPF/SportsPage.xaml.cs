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
    /// Logique d'interaction pour SportsPage.xaml
    /// </summary>
    public partial class SportsPage : Page
    {
        //Declaration -------------------------//
        SqlConnection cn = new SqlConnection("Data Source=.;Initial Catalog=NSS_Salle_Application;Integrated Security=True");
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter("", "Data Source=.;Initial Catalog=NSS_Salle_Application;Integrated Security=True");
        DataSet ds = new DataSet();
        SqlDataReader dr;
        //------------------------------------//

        MainApp dade;
        public SportsPage(MainApp dade)
        {
            InitializeComponent();
            this.dade = dade;
        }

        
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            loaded();
        }
        public void loaded()
        {
            cn.Open();
            cmd.Connection = cn;
            cmd.CommandText = "select * from Salle";
            dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            SallesComboBox.ItemsSource = dt.DefaultView;
            SallesComboBox.DisplayMemberPath = "nom_Salle";
            SallesComboBox.SelectedValuePath = "IdSalle";
            cn.Close();

            cn.Open();
            cmd.Connection = cn;
            cmd.CommandText = "select t.IdType as IdType,s.nom_Salle as nom_Salle,t.nom_Type as nom_Type,ss.prix as prix,s.IdSalle as IdSalle from Salle s join SportSalle ss on s.IdSalle=ss.IdSalle join  Type_Sport t on ss.IdType=t.IdType";
            dr = cmd.ExecuteReader();
            DataTable dt2 = new DataTable();
            dt2.Load(dr);
            ListViewSports.DataContext = dt2;
            cn.Close();
        }

        private void BtnAjouter_Click(object sender, RoutedEventArgs e)
        {
            if (BtnAjouter.Content.ToString() == "Nouveau")
            {
                BtnAjouter.Content = "Ajouter";
                SportName.Text = null;
                SportPrix.Text = null;
                SallesComboBox.SelectedIndex = -1;
                ListViewSports.UnselectAll();
            }
            else if (BtnAjouter.Content.ToString() == "Ajouter")
            {
                if (SportName.Text == "" || SportPrix.Text == "" ||  SallesComboBox.SelectedIndex == -1 )
                {
                    string msg = "Merci de remplire tout les champs";
                    MessageForm m = new MessageForm(msg);
                    m.ShowDialog();
                }
                else
                {
                    try
                    {
                        
                            cn.Open();
                            cmd.Connection = cn;
                            cmd.CommandText = "insert into Type_Sport values ('" + SportName.Text + "')";
                            cmd.ExecuteNonQuery();

                            cmd.CommandText = "select MAX(IdType) from Type_Sport";
                            int id = int.Parse(cmd.ExecuteScalar().ToString());

                            cmd.CommandText = "insert into SportSalle values ('" + SallesComboBox.SelectedValue + "','" + id + "','" + SportPrix.Text + "')";
                            cmd.ExecuteNonQuery();

                            cn.Close();
                            loaded();
                            SportName.Text = null;
                            SportPrix.Text = null;
                            SallesComboBox.SelectedIndex = -1;


                            string msg = "Sport ajouter avec success";
                            MessageForm m = new MessageForm(msg);
                            m.ShowDialog();

                        
                       
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                        MessageForm m = new MessageForm(msg);
                        m.ShowDialog();
                    }
                }               
            }

               
        }

        private void ListViewSports_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListViewSports.SelectedIndex != -1)
            {
                int index = ListViewSports.SelectedIndex;
                DataRowView row = ListViewSports.Items.GetItemAt(index) as DataRowView;

                BtnAjouter.Content = "Nouveau";
                
                SallesComboBox.SelectedValue = row.Row[4].ToString();
                SportName.Text = row.Row[2].ToString();
                SportPrix.Text = row.Row[3].ToString();
            }
            
        }

        private void BtnModifier_Click(object sender, RoutedEventArgs e)
        {
            if (SportName.Text == "" || SportPrix.Text == "" || SallesComboBox.SelectedIndex == -1)
            {
                string msg = "Merci de remplire tout les champs";
                MessageForm m = new MessageForm(msg);
                m.ShowDialog();
            }
            else
            {
                try
                {
                    int index = ListViewSports.SelectedIndex;
                    DataRowView row = ListViewSports.Items.GetItemAt(index) as DataRowView;
                    int id = int.Parse(row.Row[0].ToString());

                    cn.Open();
                    cmd.Connection = cn;
                    cmd.CommandText = "update Type_Sport set nom_Type = '" + SportName.Text + "' where IdType ='" + id + "'";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "update  SportSalle set IdSalle ='" + SallesComboBox.SelectedValue + "', prix ='" + SportPrix.Text + "' where IdType ='" + id + "'";
                    cmd.ExecuteNonQuery();

                    cn.Close();
                    BtnAjouter.Content = "Ajouter";
                    SportName.Text = null;
                    SportPrix.Text = null;
                    SallesComboBox.SelectedIndex = -1;
                    ListViewSports.UnselectAll();

                    loaded();


                    string msg = "Sport modifier avec success";
                    MessageForm m = new MessageForm(msg);
                    m.ShowDialog();


                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    MessageForm m = new MessageForm(msg);
                    m.ShowDialog();
                }
            }
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as FrameworkElement).DataContext;
            int index = ListViewSports.Items.IndexOf(item);
            DataRowView row = ListViewSports.Items.GetItemAt(index) as DataRowView;

            MessageBoxResult messageBoxResult = MessageBox.Show("voulez vous vraiment supprimer ?", "Message", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = "delete from Type_Sport where IdType = '" + row.Row[0].ToString() + "'";
                cmd.ExecuteNonQuery();
                cn.Close();
                BtnAjouter.Content = "Ajouter";
                SportName.Text = null;
                SportPrix.Text = null;
                SallesComboBox.SelectedIndex = -1;
                ListViewSports.UnselectAll();
                loaded();
                
            }
        }
    }
}
