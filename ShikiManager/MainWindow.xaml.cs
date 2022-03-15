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
        private Uri homePageUri;
        private Uri settingPageUri;
        private Uri aboutPageUri;

        public MainWindow() {
            InitializeComponent();
            // Event
            Closing += OnWindowClosing;
            Loaded += OnWindowLoaded;
            HomeButton.Click += OnHomeButtonClick;
            SettingButton.Click += OnSettingButtonClick;
            AboutButton.Click += OnAboutButtonClick;

            homePageUri = new Uri("MainWindowPage/HomePage.xaml", UriKind.Relative);
            settingPageUri = new Uri("MainWindowPage/SettingPage.xaml", UriKind.Relative);
            aboutPageUri = new Uri("MainWindowPage/AboutPage.xaml", UriKind.Relative);
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e) {
            // Default to load the home page
            PageFrame.Navigate(homePageUri);
        }

        private void OnWindowClosing(object? sender, CancelEventArgs e) {
            // Destroy DataManager
            DataManager.Instance.Destroy();
        }

        private void OnHomeButtonClick(object sender, RoutedEventArgs e) {
            PageFrame.Navigate(homePageUri);
        }

        private void OnSettingButtonClick(object sender, RoutedEventArgs e) {
            PageFrame.Navigate(settingPageUri);
        }

        private void OnAboutButtonClick(object sender, RoutedEventArgs e) {
            PageFrame.Navigate(aboutPageUri);
        }
    }
}
