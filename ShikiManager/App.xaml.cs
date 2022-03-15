using HelperConfig;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ShikiManager {
    public partial class App : Application {
        private MainWindow? mainWindow;

        private void Application_Startup(object sender, StartupEventArgs e) {
            // TODO == Logger ==
            Trace.Listeners.RemoveAt(0);
            var defaultListener = new DefaultTraceListener {
                //LogFileName = "./sss.log"
            };
            Trace.Listeners.Add(defaultListener);
            // == Logger ==

            // Create DataManager
            DataManager.Instance.Create();

            // Show MainWindow
            mainWindow ??= new MainWindow();
            mainWindow.Show();
        }
    }
}
