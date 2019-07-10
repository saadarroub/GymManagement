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
    /// Logique d'interaction pour ModifierClient.xaml
    /// </summary>
    public partial class ModifierClient : Window
    {
        MainApp dade;
        public ModifierClient(MainApp d)
        {
            InitializeComponent();
            this.dade = d;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            dade.Effect = null;
            this.Hide();
        }
    }
}
