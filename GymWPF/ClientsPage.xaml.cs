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
using System.Windows.Forms;

namespace GymWPF
{
    /// <summary>
    /// Logique d'interaction pour ClientsPage.xaml
    /// </summary>
    public partial class ClientsPage : Page
    {
        MainApp dade;
        Point p2;

        public ClientsPage(MainApp dade)
        {
            InitializeComponent();
            this.dade = dade;
            List<TodoItem> items = new List<TodoItem>();
            items.Add(new TodoItem() { Title = "Complete this WPF tutorial", Tel = 0666666693 });
            items.Add(new TodoItem() { Title = "Learn C#", Tel = 0680739829 });
            items.Add(new TodoItem() { Title = "Wash the car", Tel = 0627381793 });
            items.Add(new TodoItem() { Title = "Complete this WPF tutorial", Tel = 0666666693 });
            items.Add(new TodoItem() { Title = "Learn C#", Tel = 0680739829 });
            items.Add(new TodoItem() { Title = "Wash the car", Tel = 0627381793 });
            items.Add(new TodoItem() { Title = "Complete this WPF tutorial", Tel = 0666666693 });
            items.Add(new TodoItem() { Title = "Learn C#", Tel = 0680739829 });
            items.Add(new TodoItem() { Title = "Wash the car", Tel = 0627381793 });
            items.Add(new TodoItem() { Title = "Complete this WPF tutorial", Tel = 0666666693 });
            items.Add(new TodoItem() { Title = "Learn C#", Tel = 0680739829 });
            items.Add(new TodoItem() { Title = "Wash the car", Tel = 0627381793 });
            items.Add(new TodoItem() { Title = "Complete this WPF tutorial", Tel = 0666666693 });
            items.Add(new TodoItem() { Title = "Learn C#", Tel = 0680739829 });
            items.Add(new TodoItem() { Title = "Wash the car", Tel = 0627381793 });

            ListClient.ItemsSource = items;
           

        }
        public class TodoItem
        {
            public string Title { get; set; }
            public int Tel { get; set; }
        }

        private void MenuClientBtn_Click(object sender, RoutedEventArgs e)
        {
            
            
            p2 = Mouse.GetPosition(GridGlobal);
            MenuClientModal.Margin = new Thickness(p2.X - 100, p2.Y, 0, 0);

            if (MenuClientModal.Visibility == Visibility.Collapsed)
            {
                MenuClientModal.Visibility = Visibility.Visible;
            }
            

        }

        private void MenuClientBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (MenuClientModal.Visibility == Visibility.Visible)
            {
                MenuClientModal.Visibility = Visibility.Collapsed;
            }
        }

        private void Page_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (MenuClientModal.Visibility == Visibility.Visible)
            {
                MenuClientModal.Visibility = Visibility.Collapsed;
            }
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (MenuClientModal.Visibility == Visibility.Visible)
            {
                MenuClientModal.Visibility = Visibility.Collapsed;
            }
        }

        private void ListClient_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (MenuClientModal.Visibility == Visibility.Visible)
            {
                MenuClientModal.Visibility = Visibility.Collapsed;
            }
        }

        private void WrapPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (MenuClientModal.Visibility == Visibility.Visible)
            {
                MenuClientModal.Visibility = Visibility.Collapsed;
            }
        }
    }
}
