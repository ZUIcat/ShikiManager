using HelperConfig;
using HelperTextractor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

namespace ShikiManager {
    public partial class MainWindow : Window {

        public MainWindow() {
            InitializeComponent();
            // Event
            Closing += OnWindowClosing;

            PageFrame.Navigate(new Uri("MainWindowPage/HomePage.xaml", UriKind.Relative));
        }

        private void OnWindowClosing(object? sender, CancelEventArgs e) {
            // Destroy DataManager
            DataManager.Instance.Destroy();
        }
    }
}
