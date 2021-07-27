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
using WindowsInput.Events;

namespace ShikiManager {
    public partial class ToolWindow : Window {
        public ToolWindow() {
            InitializeComponent();

            //var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            //this.Left = desktopWorkingArea.Right - this.Width;
            //this.Top = desktopWorkingArea.Bottom - this.Height;
            //MessageBox.Show(this.Width + "|" + this.Height);
        }

        private void OnToolWindowLoaded(object sender, RoutedEventArgs e) {
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - this.Width / 2;
            this.Top = 0;
            MessageBox.Show(this.Width + "|" + this.Height);
        }

        private async void OnButton001Click(object sender, RoutedEventArgs e) {
            await WindowsInput.Simulate.Events().ClickChord(KeyCode.LWin, KeyCode.PrintScreen).Invoke();
        }

        private void OnButton002Click(object sender, RoutedEventArgs e) {

        }
        private void OnButton003Click(object sender, RoutedEventArgs e) {

        }
        private void OnButton004Click(object sender, RoutedEventArgs e) {

        }
        private void OnButton005Click(object sender, RoutedEventArgs e) {

        }
        private void OnButton006Click(object sender, RoutedEventArgs e) {

        }
        private void OnButton007Click(object sender, RoutedEventArgs e) {

        }
    }
}
