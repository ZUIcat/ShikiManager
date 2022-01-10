using HelperConfig;
using HelperTextractor;
using System;
using System.Collections.Generic;
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
        TextractorHelper textractorHelper;

        public MainWindow() {
            InitializeComponent();
            Trace.Listeners.RemoveAt(0);
            var defaultListener = new DefaultTraceListener {
                //LogFileName = "./sss.log"
            };
            Trace.Listeners.Add(defaultListener);

            ConfigHelper.Instance.ReadAppConfig();
        }

        private void TestButton01_Click(object sender, RoutedEventArgs e) {
            textractorHelper = new TextractorHelper();
            textractorHelper.Create(ConfigHelper.Instance.AppConfig.TextractorConfig.DirPath);
            //textractorHelper.Init(@"C:\_MyWorkSpace\WorkSpaceTemp\HookTranslator\Textractor\release_v5.1.0");
        }

        private async void TestButton02_Click(object sender, RoutedEventArgs e) {
            await textractorHelper.AttachProcess(26348);
        }

        private void TestButton03_Click(object sender, RoutedEventArgs e) {
            textractorHelper?.Destroy();
            ConfigHelper.Instance.WriteAppConfig();
        }
    }
}
