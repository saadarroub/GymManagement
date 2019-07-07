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
using System.ComponentModel;

namespace GymWPF
{
    /// <summary>
    /// Logique d'interaction pour SallesPage.xaml
    /// </summary>
    public partial class SallesPage : Page
    {
        //Declaration -------------------------//
        SqlConnection cn = new SqlConnection("Data Source=.;Initial Catalog=NSS_Salle_Application;Integrated Security=True");
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter("", "Data Source=.;Initial Catalog=NSS_Salle_Application;Integrated Security=True");
        DataSet ds = new DataSet();
        SqlDataReader dr;
       
        //------------------------------------//
        
        MainApp dade;
        public SallesPage(MainApp dade)
        {
            InitializeComponent();
            this.dade = dade;
            
        }
        public void LoadResoource()
        {
            if (ds.Tables["Salle"] != null)
            {
                ds.Tables["Salle"].Clear();
            }

            da.SelectCommand.CommandText = "select IdSalle,nom_Salle from Salle";
            da.Fill(ds, "Salle");
            ListViewSalles.DataContext = ds.Tables["Salle"].DefaultView;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadResoource();  
            
        }

        private void ListViewSalles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
                int index = ListViewSalles.SelectedIndex;
                DataRowView row = ListViewSalles.Items.GetItemAt(index) as DataRowView;
                SalleName.Text = row.Row[1].ToString();
            
                
        }

        private void BtnAjouter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SalleName.Text == "")
                {
                    MessageBox.Show("saisir le nom de la salle");
                }
                else if (ds.Tables["Salle"].Rows.Count > 1)
                {
                    MessageBox.Show("you have just two salle in youre liscence");
                }
                else
                {
                    //cn.Open();
                    //cmd.Connection = cn;
                    //cmd.CommandText = "insert into Salle values ('" + SalleName.Text + "')";
                    //cmd.ExecuteNonQuery();
                    //cn.Close();
                    //MessageBox.Show("votre salle est bien ajouter");
                    DataRow r = ds.Tables["Salle"].NewRow();
                    r[1] = SalleName.Text;
                    ds.Tables["Salle"].Rows.Add(r);
                    da.SelectCommand.CommandText = "select IdSalle,nom_Salle from Salle";
                    SqlCommandBuilder cb = new SqlCommandBuilder(da);
                    da.Update(ds, "Salle");
                    ListViewSalles.UnselectAll();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }           
        }

        private void BtnModifier_Click(object sender, RoutedEventArgs e)
        {

            int index = ListViewSalles.SelectedIndex;
            DataRowView row = ListViewSalles.Items.GetItemAt(index) as DataRowView;
            int id = int.Parse(row.Row[0].ToString());

            try
            {
                //cn.Open();
                //cmd.Connection = cn;
                //cmd.CommandText = "update Salle set nom_Salle = '" + SalleName.Text + "' where IdSalle = '" + id + "'";
                //cmd.ExecuteNonQuery();
                //cn.Close();
                //MessageBox.Show("votre salle est bien modifier");
                DataRow r = ds.Tables["Salle"].Select("IdSalle="+id.ToString())[0];
                r.BeginEdit();
                r[1] = SalleName.Text;
                r.EndEdit();
                da.SelectCommand.CommandText = "select IdSalle,nom_Salle from Salle";
                SqlCommandBuilder cb = new SqlCommandBuilder(da);
                da.Update(ds, "Salle");
                ListViewSalles.UnselectAll();


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

       

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as FrameworkElement).DataContext;
            int index = ListViewSalles.Items.IndexOf(item);
            DataRowView row = ListViewSalles.Items.GetItemAt(index) as DataRowView;
            
            MessageBoxResult messageBoxResult = MessageBox.Show("voulez vous vraiment supprimer ?", "Message" , MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = "delete from Salle where IdSalle = '" + row.Row[0].ToString() + "'";
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }
    }
}
