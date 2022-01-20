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
        TextWindow textWindow = null!;
        SelectionWindow selectionWindow = null!;

        public MainWindow() {
            InitializeComponent();
            Trace.Listeners.RemoveAt(0);
            var defaultListener = new DefaultTraceListener {
                //LogFileName = "./sss.log"
            };
            Trace.Listeners.Add(defaultListener);

            //TextHookData.TextDataFilterFunc = TextDataFilter.Remove2SameChar;
        }

        private void TestButton01_Click(object sender, RoutedEventArgs e) {
            TextractorHelper.Instance.Create(ConfigHelper.Instance.AppConfig.TextractorConfig.DirPath);
            //textractorHelper.Init(@"C:\_MyWorkSpace\WorkSpaceTemp\HookTranslator\Textractor\release_v5.1.0");
        }

        private async void TestButton02_Click(object sender, RoutedEventArgs e) {
            await TextractorHelper.Instance.AttachProcess(12944);
        }

        private void TestButton03_Click(object sender, RoutedEventArgs e) {
            TextractorHelper.Instance?.Destroy();
            ConfigHelper.Instance.WriteAppConfig();
        }

        private void TestButton04_Click(object sender, RoutedEventArgs e) {
            textWindow ??= new TextWindow();
            textWindow.ShowAndConnect();
        }

        private void TestButton05_Click(object sender, RoutedEventArgs e) {
            selectionWindow ??= new SelectionWindow();
            selectionWindow.ShowAndConnect();
        }

        private void TestButton06_Click(object sender, RoutedEventArgs e) {

        }
    }
}
