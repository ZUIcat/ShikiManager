using HelperConfig;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ShikiManager {
    public partial class App : Application {
        private MainWindow? mainWindow;

        private void Application_Startup(object sender, StartupEventArgs e) {
            // Init DataManager
            DataManager.Instance.Create();
            // Read App Config
            ConfigHelper.Instance.ReadAppConfig();
            // Show MainWindow
            mainWindow ??= new MainWindow();
            mainWindow.Show();
        }
    }
}
