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
using HelperConfig;

namespace ShikiManager {
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void Home_Btn_Click(object sender, RoutedEventArgs e) {
            Console.WriteLine("APP Start!");
            ConfigHelper.Instance.ReadAppConfig();
            ConfigHelper.Instance.WriteAppConfig();
            Console.WriteLine("APP End!");
        }
    }
}
