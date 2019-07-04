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

namespace GymWPF
{
    /// <summary>
    /// Logique d'interaction pour MainApp.xaml
    /// </summary>
    public partial class MainApp : Window
    {

        SallesPage salles;
        SportsPage sports;
        UtilisateursPage utilisateurs;
        RollesPage rolles;
        ClientsPage clients;
        public MainApp()
        {
            InitializeComponent();

            salles = new SallesPage(this);
            sports = new SportsPage(this);
            utilisateurs = new UtilisateursPage(this);
            rolles = new RollesPage(this);
            clients = new ClientsPage(this);
            
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
            
        }

        private void CloseAppBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MinimizeAppBtn_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        public void HideShowRecNavBtn(Rectangle active)
        {
            RecSalles.Visibility = Visibility.Hidden;
            RecSports.Visibility = Visibility.Hidden;
            RecRolles.Visibility = Visibility.Hidden;
            RecUsers.Visibility = Visibility.Hidden;
            RecClients.Visibility = Visibility.Hidden;
            RecPayements.Visibility = Visibility.Hidden;
            RecDepenses.Visibility = Visibility.Hidden;
            active.Visibility = Visibility.Visible;
        }
        public void HideShowMenuPanel()
        {
            if (MenuPanel.Visibility == Visibility.Collapsed)
            {
                MenuPanel.Visibility = Visibility.Visible;
            }
            else
            {
                MenuPanel.Visibility = Visibility.Collapsed;
            }
        }
       

        private void MainAppNavBtnToSalles_Click(object sender, RoutedEventArgs e)
        {
            HideShowRecNavBtn(RecSalles);
            MainFrame.Navigate(salles);
        }

        private void MainAppNavBtnToSprorts_Click(object sender, RoutedEventArgs e)
        {
            HideShowRecNavBtn(RecSports);
            MainFrame.Navigate(sports);
        }

        private void MainAppNavBtnToUsers_Click(object sender, RoutedEventArgs e)
        {
            HideShowRecNavBtn(RecUsers);
            MainFrame.Navigate(utilisateurs);
        }

        private void MainAppNavBtnToRolles_Click(object sender, RoutedEventArgs e)
        {
            HideShowRecNavBtn(RecRolles);
            MainFrame.Navigate(rolles);
        }

        private void MainAppNavBtnToClients_Click(object sender, RoutedEventArgs e)
        {
            HideShowRecNavBtn(RecClients);
            MainFrame.Navigate(clients);
        }

        private void MainAppNavBtnToPayments_Click(object sender, RoutedEventArgs e)
        {
            HideShowRecNavBtn(RecPayements);
        }

        private void MainAppNavBtnToDepenses_Click(object sender, RoutedEventArgs e)
        {
            HideShowRecNavBtn(RecDepenses);
        }

        private void ShowHideMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            HideShowMenuPanel();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MenuPanel.Visibility = Visibility.Collapsed;
            
        }
    }
}
