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
using System.Windows.Media.Animation;

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
        bool isPostBack = false;

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
          
            loaded();
            
        }

       

        DataTable dt = new DataTable();
        private void loaded()
        {
            if (isPostBack == false)
            {
                ch1.IsChecked = true;
                isPostBack = true;
            }

            if (ConnectedSalle.ToString() == "" && ConnectedSport.ToString() == "")
            {
                AjouterClientBtn.IsEnabled = false;
                AjouterClientBtn.Foreground = new SolidColorBrush(Color.FromRgb(128, 128, 128));

                PayementsClientModalBtn.IsEnabled = false;
                ModifierClientModalBtn.IsEnabled = false;
               

                PayementsClientModalBtn.Foreground = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                ModifierClientModalBtn.Foreground = new SolidColorBrush(Color.FromRgb(128, 128, 128));


                cn.Open();
                cmd.Connection = cn;
                if (dt != null)
                {
                    dt.Clear();
                }

                

                cmd.CommandText = "select c.IdClient as id,UPPER(c.nom) +' '+ UPPER(c.prenom) as Title,c.Tel as Tel,c.img as Photo,s.IdClient,s.IdType, c.Active as Active from Clients c join SportClients s on c.IdClient=s.IdClient order by c.IdClient DESC";

                dr = cmd.ExecuteReader();
                dt.Load(dr);
                ListClient.DataContext = dt;
                cn.Close();
            }
            else
            {     
                cn.Open();
                cmd.Connection = cn;
                if (dt != null)
                {
                    dt.Clear();
                }

                cmd.CommandText = "select c.IdClient as id,UPPER(c.nom) +' '+ UPPER(c.prenom) as Title,c.Tel as Tel,c.img as Photo,s.IdClient,s.IdType, c.Active as Active from Clients c join SportClients s on c.IdClient=s.IdClient where s.IdSalle='" + ConnectedSalle.ToString() + "' and s.IdType='" + ConnectedSport.ToString() + "' order by c.IdClient DESC";

                dr = cmd.ExecuteReader();
                dt.Load(dr);
                ListClient.DataContext = dt;
                cn.Close();                
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
            int index = ListClient.SelectedIndex;
            DataRowView row = ListClient.Items.GetItemAt(index) as DataRowView;
            string id = row.Row[0].ToString();

            dade.Opacity = 0.5;
            dade.Effect = new BlurEffect();
            PayementsPage pp = new PayementsPage(dade, id, ConnectedSalle, ConnectedSport);
            pp.ShowDialog();
        }

        private void ModifierClientModalBtn_Click(object sender, RoutedEventArgs e)
        {
            int index = ListClient.SelectedIndex;
            DataRowView row = ListClient.Items.GetItemAt(index) as DataRowView;
            string id = row.Row[0].ToString();

            dade.Opacity = 0.5;
            dade.Effect = new BlurEffect();
            ModifierClient mc = new ModifierClient(dade, id, ConnectedSalle, ConnectedSport);
            mc.ShowDialog();            
        }



        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ch1.IsChecked == true)
            {
               
                dt.DefaultView.RowFilter = "Title like '%" + search.Text.Replace("'","''") + "%'";
            }
            if (ch2.IsChecked == true)
            {
                dt.DefaultView.RowFilter = "Title like '%" + search.Text.Replace("'", "''") + "%' and Active = '" + true + "'";
            }
            if (ch3.IsChecked == true)
            {
                dt.DefaultView.RowFilter = "Title like '%" + search.Text.Replace("'", "''") + "%' and Active = '" + false + "'";
            }

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

            if (ListClient.SelectedIndex != -1)
            {
                int index = ListClient.SelectedIndex;
                DataRowView row = ListClient.Items.GetItemAt(index) as DataRowView;
                int id = int.Parse(row.Row[0].ToString());

                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = "select Active from Clients where IdClient = '" + id + "'";
                if (cmd.ExecuteScalar().ToString() == "")
                {
                    cn.Close();
                }
                else
                {
                    bool active = Convert.ToBoolean(cmd.ExecuteScalar().ToString());
                    cn.Close();
                    if (active == true)
                    {
                        icon.Foreground = new SolidColorBrush(Color.FromRgb(52, 255, 72));
                        state.Text = "Activé";
                        OnOffClientModalBtn.ToolTip = "Desactiver";
                    }

                    else
                    {
                        icon.Foreground = new SolidColorBrush(Color.FromRgb(255, 52, 73));
                        state.Text = "Desactivé";
                        OnOffClientModalBtn.ToolTip = "Activer";
                    }
                }
            }
               
        }

        private void SupprimerClientModalBtn_Click(object sender, RoutedEventArgs e)
        {
            int index = ListClient.SelectedIndex;
            DataRowView row = ListClient.Items.GetItemAt(index) as DataRowView;
            int id = int.Parse(row.Row[0].ToString());

            ConfirmForm c = new ConfirmForm("voulez vous vraiment supprimer ?");
            c.Owner = dade;
            dade.Opacity = 0.5;
            dade.Effect = new BlurEffect();
            if((bool)c.ShowDialog())
            {
                cn.Open();
            cmd.Connection = cn;
            cmd.CommandText = "delete from Clients where IdClient = '" + id + "'";
            cmd.ExecuteNonQuery();
            cn.Close();

                messageContent.Text = "Bien supprimé";
                animateBorder(borderMessage);

                loaded();
            }
            dade.Opacity =1;
            dade.Effect = null;

           
        }

        private void AjouterClientBtn_Click(object sender, RoutedEventArgs e)
        {
            dade.Opacity = 0.5;
            dade.Effect = new BlurEffect();
            AjouterClient ac = new AjouterClient(dade, ConnectedSalle, ConnectedSport);
            ac.ShowDialog();
        }

        private void OnOffClientModalBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ListClient.SelectedIndex != -1)
            {
                int index = ListClient.SelectedIndex;
                DataRowView row = ListClient.Items.GetItemAt(index) as DataRowView;
                int id = int.Parse(row.Row[0].ToString());
                try
                {
                    if (state.Text == "Activé")
                    {
                        icon.Foreground = new SolidColorBrush(Color.FromRgb(255, 52, 73));
                        state.Text = "Desactivé";

                        cn.Open();
                        cn = cmd.Connection;
                        cmd.CommandText = "update Clients set Active = '" + false + "' where IdClient = '" + id + "'";
                        cmd.ExecuteNonQuery();
                        cn.Close();

                        messageContent.Text = "Client Desactivé";
                        animateBorder(borderMessage);
                    }
                    else if (state.Text == "Desactivé")
                    {
                        icon.Foreground = new SolidColorBrush(Color.FromRgb(52, 255, 72));
                        state.Text = "Activé";


                        cn.Open();
                        cn = cmd.Connection;
                        cmd.CommandText = "update Clients set Active = '" + true + "' where IdClient = '" + id + "'";
                        cmd.ExecuteNonQuery();
                        cn.Close();

                        messageContent.Text = "Client Activé";
                        animateBorder(borderMessage);
                    }
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    MessageForm m = new MessageForm(msg);
                    m.ShowDialog();
                }
               
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            loaded();

            dt.DefaultView.RowFilter = "";

            ListClient.UnselectAll();
            if (MenuClientModal.Visibility == Visibility.Visible)
            {
                MenuClientModal.Visibility = Visibility.Collapsed;
            }
            

        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            loaded();

            dt.DefaultView.RowFilter = "Active = '"+ true +"'";

            ListClient.UnselectAll();
            if (MenuClientModal.Visibility == Visibility.Visible)
            {
                MenuClientModal.Visibility = Visibility.Collapsed;
            }
        }

        private void RadioButton_Checked_2(object sender, RoutedEventArgs e)
        {
            loaded();

            dt.DefaultView.RowFilter = "Active = '" + false + "'";

            ListClient.UnselectAll();
            if (MenuClientModal.Visibility == Visibility.Visible)
            {
                MenuClientModal.Visibility = Visibility.Collapsed;
            }

        }

        public void animateBorder(Border c)
        {
            ((Storyboard)GridGlobal.Resources["animate"]).Begin(c);

        }
        //public string AddQuotationToString(string String_In)
        //{
        //    string String_Out;
        //    string[] ArrString = new string[] {String_In};
        //    for(int i = 0; i < ArrString.Count(); i++)
        //    {
        //        if(ArrString[i] == "'")
        //        {
        //            ArrString.Resize(ref ArrString, ArrString.Count() + 1);
        //            ArrString.SetValue("'", i + 1);
        //        }
        //    }

        //    String_Out = ArrString.ToString();
        //    return String_Out;
        //}
    }
}
