using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using WindowsInput.Events;
using HelperExternDll;

namespace ShikiManager {
    public partial class ToolWindow : Window {
        private int hideDir = 1;
        private int windowPos = 2;
        private int centerScreenPosX = Winuser.GetPrimaryScreenWidth() / 2;
        private int centerScreenPosY = Winuser.GetPrimaryScreenHeight() / 2;

        public ToolWindow() {
            InitializeComponent();
        }

        private void OnToolWindowLoaded(object sender, RoutedEventArgs e) {
            // 设置窗口位置
            SetWindowPosition(windowPos);
            // 设置窗口不获取焦点
            WindowInteropHelper wndHelper = new WindowInteropHelper(this);
            Winuser.SetWindowNoActivate(wndHelper.Handle);
        }

        #region Btn
        private void OnBtn00Click(object sender, RoutedEventArgs e) {
            Btn00CM.IsOpen = true;
        }
        private async void OnBtn01Click(object sender, RoutedEventArgs e) {
            this.Hide();
            await WindowsInput.Simulate.Events()
                .ClickChord(KeyCode.LWin, KeyCode.PrintScreen)
                .Wait(500)
                .Invoke();
            this.Show();
        }
        private async void OnBtn02Click(object sender, RoutedEventArgs e) {
            await WindowsInput.Simulate.Events()
                .ClickChord(KeyCode.Control)
                .Invoke();
        }
        private async void OnBtn03Click(object sender, RoutedEventArgs e) {
            await WindowsInput.Simulate.Events()
                .ClickChord(KeyCode.Space)
                .Invoke();
        }
        private async void OnBtn04Click(object sender, RoutedEventArgs e) {
            await WindowsInput.Simulate.Events()
                .ClickChord(KeyCode.Up)
                .Invoke();
        }
        private async void OnBtn05Click(object sender, RoutedEventArgs e) {
            await WindowsInput.Simulate.Events()
                .ClickChord(KeyCode.Down)
                .Invoke();
        }
        private async void OnBtn06Click(object sender, RoutedEventArgs e) {
            Winuser.POINT curPos;
            Winuser.GetCursorPos(out curPos);
            await WindowsInput.Simulate.Events()
                .MoveTo(centerScreenPosX, centerScreenPosY)
                .Scroll(ButtonCode.VScroll, ButtonScrollDirection.Up)
                .Wait(100)
                .MoveTo(curPos.X, curPos.Y)
                .Invoke();
        }
        private async void OnBtn07Click(object sender, RoutedEventArgs e) {
            Winuser.POINT curPos;
            Winuser.GetCursorPos(out curPos);
            await WindowsInput.Simulate.Events()
                .MoveTo(centerScreenPosX, centerScreenPosY)
                .Scroll(ButtonCode.VScroll, ButtonScrollDirection.Down)
                .Wait(100)
                .MoveTo(curPos.X, curPos.Y)
                .Invoke();
        }
        #endregion

        #region Btn00CM
        // https://www.haolizi.net/example/view_10390.html
        private void OnBtn00CMTopLeftClick(object sender, RoutedEventArgs e) {
            SetWindowPosition(1);
        }

        private void OnBtn00CMTopMiddleClick(object sender, RoutedEventArgs e) {
            SetWindowPosition(2);
        }

        private void OnBtn00CMTopRightClick(object sender, RoutedEventArgs e) {
            SetWindowPosition(3);
        }

        private void OnBtn00CMRightTopClick(object sender, RoutedEventArgs e) {
            SetWindowPosition(4);
        }

        private void OnBtn00CMRightMiddleClick(object sender, RoutedEventArgs e) {
            SetWindowPosition(5);
        }

        private void OnBtn00CMRightButtomClick(object sender, RoutedEventArgs e) {
            SetWindowPosition(6);
        }

        private void OnBtn00CMButtomRightClick(object sender, RoutedEventArgs e) {
            SetWindowPosition(7);
        }

        private void OnBtn00CMButtomMiddleClick(object sender, RoutedEventArgs e) {
            SetWindowPosition(8);
        }

        private void OnBtn00CMButtomLeftClick(object sender, RoutedEventArgs e) {
            SetWindowPosition(9);
        }

        private void OnBtn00CMLeftButtomClick(object sender, RoutedEventArgs e) {
            SetWindowPosition(10);
        }

        private void OnBtn00CMLeftMiddleClick(object sender, RoutedEventArgs e) {
            SetWindowPosition(11);
        }

        private void OnBtn00CMLeftTopClick(object sender, RoutedEventArgs e) {
            SetWindowPosition(12);
        }

        private void OnBtn00CMHideClick(object sender, RoutedEventArgs e) {
            this.Hide();
        }
        #endregion

        /// <summary>
        /// 设置窗口位置, 12 个位置, 顺时针数一遭.
        /// </summary>
        /// <param name="locationType"></param>
        private void SetWindowPosition(int locationType) {
            windowPos = locationType;
            switch (locationType) {
                case 1:
                    this.ToolStackPanel.Orientation = Orientation.Horizontal;
                    this.UpdateLayout();
                    this.Top = 0;
                    this.Left = 0;
                    hideDir = 1;
                    break;
                case 2:
                    this.ToolStackPanel.Orientation = Orientation.Horizontal;
                    this.UpdateLayout();
                    this.Top = 0;
                    this.Left = System.Windows.SystemParameters.PrimaryScreenWidth / 2 - this.Width / 2;
                    hideDir = 1;
                    break;
                case 3:
                    this.ToolStackPanel.Orientation = Orientation.Horizontal;
                    this.UpdateLayout();
                    this.Top = 0;
                    hideDir = 1;
                    this.Left = System.Windows.SystemParameters.PrimaryScreenWidth - this.Width;
                    break;
                case 4:
                    this.ToolStackPanel.Orientation = Orientation.Vertical;
                    this.UpdateLayout();
                    this.Top = 0;
                    this.Left = System.Windows.SystemParameters.PrimaryScreenWidth - this.Width;
                    hideDir = 2;
                    break;
                case 5:
                    this.ToolStackPanel.Orientation = Orientation.Vertical;
                    this.UpdateLayout();
                    this.Top = System.Windows.SystemParameters.PrimaryScreenHeight / 2 - this.Height / 2;
                    this.Left = System.Windows.SystemParameters.PrimaryScreenWidth - this.Width;
                    hideDir = 2;
                    break;
                case 6:
                    this.ToolStackPanel.Orientation = Orientation.Vertical;
                    this.UpdateLayout();
                    this.Top = System.Windows.SystemParameters.PrimaryScreenHeight - this.Height;
                    this.Left = System.Windows.SystemParameters.PrimaryScreenWidth - this.Width;
                    hideDir = 2;
                    break;
                case 7:
                    this.ToolStackPanel.Orientation = Orientation.Horizontal;
                    this.UpdateLayout();
                    this.Top = System.Windows.SystemParameters.PrimaryScreenHeight - this.Height;
                    this.Left = System.Windows.SystemParameters.PrimaryScreenWidth - this.Width;
                    hideDir = 3;
                    break;
                case 8:
                    this.ToolStackPanel.Orientation = Orientation.Horizontal;
                    this.UpdateLayout();
                    this.Top = System.Windows.SystemParameters.PrimaryScreenHeight - this.Height;
                    this.Left = System.Windows.SystemParameters.PrimaryScreenWidth / 2 - this.Width / 2;
                    hideDir = 3;
                    break;
                case 9:
                    this.ToolStackPanel.Orientation = Orientation.Horizontal;
                    this.UpdateLayout();
                    this.Top = System.Windows.SystemParameters.PrimaryScreenHeight - this.Height;
                    this.Left = 0;
                    hideDir = 3;
                    break;
                case 10:
                    this.ToolStackPanel.Orientation = Orientation.Vertical;
                    this.UpdateLayout();
                    this.Top = System.Windows.SystemParameters.PrimaryScreenHeight - this.Height;
                    this.Left = 0;
                    hideDir = 4;
                    break;
                case 11:
                    this.ToolStackPanel.Orientation = Orientation.Vertical;
                    this.UpdateLayout();
                    this.Top = System.Windows.SystemParameters.PrimaryScreenHeight / 2 - this.Height / 2;
                    this.Left = 0;
                    hideDir = 4;
                    break;
                case 12:
                    this.ToolStackPanel.Orientation = Orientation.Vertical;
                    this.UpdateLayout();
                    this.Top = 0;
                    this.Left = 0;
                    hideDir = 4;
                    break;
                default:
                    break;
            }
        }
    }
}
