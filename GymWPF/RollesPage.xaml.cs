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

namespace GymWPF
{
    /// <summary>
    /// Logique d'interaction pour RollesPage.xaml
    /// </summary>
    public partial class RollesPage : Page
    {
        MainApp dade;
        public RollesPage(MainApp dade)
        {
            InitializeComponent();
            this.dade = dade;
        }
    }
}
