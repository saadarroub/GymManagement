using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Effects;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Windows.Controls;

namespace GymWPF
{
    /// <summary>
    /// Logique d'interaction pour ClientsPage.xaml
    /// </summary>
    public partial class ClientsPage : Page
    {
        MainApp dade;
        Point p2;
        string ConnectedSalle, ConnectedSport;

        public ClientsPage(MainApp dade ,string ConnectedSalle, string ConnectedSport)
        {
            this.ConnectedSalle = ConnectedSalle;
            this.ConnectedSport = ConnectedSport;

            InitializeComponent();
            this.dade = dade;
            //List<TodoItem> items = new List<TodoItem>();
            //items.Add(new TodoItem() { Title = "Complete this WPF tutorial", Tel = 0666666693 });
            //items.Add(new TodoItem() { Title = "Learn C#", Tel = 0680739829 });
            //items.Add(new TodoItem() { Title = "Wash the car", Tel = 0627381793 });
            //items.Add(new TodoItem() { Title = "Complete this WPF tutorial", Tel = 0666666693 });
            //items.Add(new TodoItem() { Title = "Learn C#", Tel = 0680739829 });
            //items.Add(new TodoItem() { Title = "Wash the car", Tel = 0627381793 });
            //items.Add(new TodoItem() { Title = "Complete this WPF tutorial", Tel = 0666666693 });
            //items.Add(new TodoItem() { Title = "Learn C#", Tel = 0680739829 });
            //items.Add(new TodoItem() { Title = "Wash the car", Tel = 0627381793 });
            //items.Add(new TodoItem() { Title = "Complete this WPF tutorial", Tel = 0666666693 });
            //items.Add(new TodoItem() { Title = "Learn C#", Tel = 0680739829 });
            //items.Add(new TodoItem() { Title = "Wash the car", Tel = 0627381793 });
            //items.Add(new TodoItem() { Title = "Complete this WPF tutorial", Tel = 0666666693 });
            //items.Add(new TodoItem() { Title = "Learn C#", Tel = 0680739829 });
            //items.Add(new TodoItem() { Title = "Wash the car", Tel = 0627381793 });

            //ListClient.ItemsSource = items;
           

        }
        //declaration --------------------------------------
        SqlConnection cn = new SqlConnection("Data Source=.;Initial Catalog=NSS_Salle_Application;Integrated Security=True");
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter("", "Data Source=.;Initial Catalog=NSS_Salle_Application;Integrated Security=True");
        DataSet ds = new DataSet();
        SqlDataReader dr;
        //------------------------------------------------------

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (ConnectedSalle.ToString() == "" && ConnectedSport.ToString() == "")
            {
                AjouterClientBtn.IsEnabled = false;
                AjouterClientBtn.Foreground = new SolidColorBrush(Color.FromRgb(128, 128, 128));
            }
            else
            {
                da.SelectCommand.CommandText = "select c.IdClient as id,c.nom+' '+c.prenom as Title,c.Tel as Tel,c.img as Photo,s.IdClient,s.IdType from Clients c join SportClients s on c.IdClient=s.IdClient where s.IdSalle='" + ConnectedSalle.ToString() + "' and s.IdType='" + ConnectedSport.ToString() + "'";
                da.Fill(ds, "clients");
                ListClient.ItemsSource = ds.Tables["clients"].DefaultView;
            }

           
            
        }


        //public class TodoItem
        //{
        //    public string Title { get; set; }
        //    public int Tel { get; set; }
        //}

        

        

        private void Page_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ListClient.UnselectAll();
            if (MenuClientModal.Visibility == Visibility.Visible)
            {
                MenuClientModal.Visibility = Visibility.Collapsed;
            }
           
        }

        

        private void ListClient_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ListClient.UnselectAll();
            if (MenuClientModal.Visibility == Visibility.Visible)
            {
                MenuClientModal.Visibility = Visibility.Collapsed;
            }
           
        }

        private void WrapPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ListClient.UnselectAll();
            if (MenuClientModal.Visibility == Visibility.Visible)
            {
                MenuClientModal.Visibility = Visibility.Collapsed;
            }
           
        }


        private void PayementsClientModalBtn_Click(object sender, RoutedEventArgs e)
        {
            dade.Effect = new BlurEffect();
            PayementsPage pp = new PayementsPage(dade);
            pp.ShowDialog();
        }

        private void ModifierClientModalBtn_Click(object sender, RoutedEventArgs e)
        {
            dade.Effect = new BlurEffect();
            ModifierClient mc = new ModifierClient(dade);
            mc.ShowDialog();

            //int index = ListClient.SelectedIndex;
            //DataRowView row = ListClient.Items.GetItemAt(index) as DataRowView;
            //MessageBox.Show(row.Row[0].ToString());
        }



            private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ds.Tables["clients"].DefaultView.RowFilter = "Title like '%" + search.Text + "%'";

        }

        private void WrapPanel_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            ListClient.UnselectAll();
            if (MenuClientModal.Visibility == Visibility.Visible)
            {
                MenuClientModal.Visibility = Visibility.Collapsed;
            }
            
        }

        private void WrapPanel_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ListClient.UnselectAll();
            if (MenuClientModal.Visibility == Visibility.Visible)
            {
                MenuClientModal.Visibility = Visibility.Collapsed;
            }
        }

        private void ListClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            p2 = Mouse.GetPosition(GridGlobal);
            MenuClientModal.Margin = new Thickness(p2.X+5, p2.Y+5, 0, 0);

            if (MenuClientModal.Visibility == Visibility.Collapsed)
            {
                MenuClientModal.Visibility = Visibility.Visible;
            }
        }

        private void AjouterClientBtn_Click(object sender, RoutedEventArgs e)
        {
            dade.Effect = new BlurEffect();
            AjouterClient ac = new AjouterClient(dade, ConnectedSalle, ConnectedSport);
            ac.ShowDialog();
        }
    }
}
