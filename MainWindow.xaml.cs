using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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

namespace MicrosoftRewardsFarmerUI {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();

            //TODO: Get amount of accounts from accounts.json
            AccountComboBox.ItemsSource = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9};
        }

        private void NumericOnly(object sender, TextCompositionEventArgs e) {
            e.Handled = !int.TryParse(((TextBox)sender).Text + e.Text, out int i) && i >= 1 && i <= 99;
        }
    }
}
