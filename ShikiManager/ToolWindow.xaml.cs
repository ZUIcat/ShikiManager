using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using WindowsInput.Events;
using HelperExternDll;
using System.Drawing;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace ShikiManager {
    public partial class ToolWindow : Window {
        private static readonly int KEY_STEP_TIME = 100;
        private static readonly int KEY_RRESS_TIME = 1000;

        private int hideDir;
        private int windowPos;
        private int screenWidth;
        private int screenHeight;
        private int centerScreenPosX;
        private int centerScreenPosY;

        private Dictionary<KeyCode, bool> keyReleaseDic;
        private object keyReleaseDicLock;

        public ToolWindow() {
            InitializeComponent();

            // Value Init
            hideDir = 1;
            windowPos = 2;
            screenWidth = Winuser.GetPrimaryScreenWidth();
            screenHeight = Winuser.GetPrimaryScreenHeight();
            centerScreenPosX = screenWidth / 2;
            centerScreenPosY = screenHeight / 2;

            keyReleaseDic = new Dictionary<KeyCode, bool>();
            keyReleaseDicLock = new object();
        }

        private void OnToolWindowLoaded(object sender, RoutedEventArgs e) {
            // 设置窗口位置
            SetWindowPosition(windowPos);
            // 设置窗口不获取焦点
            WindowInteropHelper wndHelper = new WindowInteropHelper(this);
            Winuser.SetWindowNoActivate(wndHelper.Handle);
        }

        /// <summary>
        /// 按下指定按键. 按下并保持一定时间后就会反复产生按下事件.
        /// </summary>
        /// <param name="key">按键代码</param>
        /// <returns></returns>
        private async Task PressKey(KeyCode key) {
            await WindowsInput.Simulate.Events().ClickChord(key).Invoke();
            await Task.Run(async () => {
                int accuTime = 0;
                bool longState = false;
                bool isRelease = false;
                lock (keyReleaseDicLock) {
                    keyReleaseDic[key] = false;
                }
                while (!isRelease) {
                    if (!longState) { accuTime += KEY_STEP_TIME; }
                    if (longState || accuTime >= KEY_RRESS_TIME) {
                        longState = true;
                        await WindowsInput.Simulate.Events().Hold(key).Invoke();
                    }
                    Thread.Sleep(KEY_STEP_TIME);
                    lock (keyReleaseDicLock) {
                        isRelease = keyReleaseDic.TryGetValue(key, out isRelease) && isRelease;
                    }
                }
                if (longState) { await WindowsInput.Simulate.Events().Release(key).Invoke(); }
            });
        }
        private void ReleaseKey(KeyCode key) {
            lock (keyReleaseDicLock) {
                keyReleaseDic[key] = true;
            }
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
        private async void OnBtn02PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            await WindowsInput.Simulate.Events()
                .Hold(KeyCode.Control)
                .Invoke();
        }
        private async void OnBtn02PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            await WindowsInput.Simulate.Events()
                .Release(KeyCode.Control)
                .Invoke();
        }

        private void OnBtn03PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            PressKey(KeyCode.Space);
        }
        private void OnBtn03PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            ReleaseKey(KeyCode.Space);
        }

        private async void OnBtn04PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            await WindowsInput.Simulate.Events()
                .Hold(KeyCode.Up)
                .Invoke();
        }
        private async void OnBtn04PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            await WindowsInput.Simulate.Events()
                .Release(KeyCode.Up)
                .Invoke();
        }
        private async void OnBtn05PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            await WindowsInput.Simulate.Events()
                .Hold(KeyCode.Down)
                .Invoke();
        }
        private async void OnBtn05PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            await WindowsInput.Simulate.Events()
                .Release(KeyCode.Down)
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
        private async void OnBtn08Click(object sender, RoutedEventArgs e) {
            await WindowsInput.Simulate.Events()
                .ClickChord(KeyCode.Home)
                .Invoke();
        }
        private async void OnBtn09Click(object sender, RoutedEventArgs e) {
            await WindowsInput.Simulate.Events()
                .ClickChord(KeyCode.End)
                .Invoke();
        }
        private void OnBtnTest01Click(object sender, RoutedEventArgs e) {
            using (Bitmap cacheBitmap = new Bitmap(screenWidth, screenHeight)) {
                Graphics g = Graphics.FromImage(cacheBitmap);
                g.CopyFromScreen(0, 0, 0, 0, new System.Drawing.Size(screenWidth, screenWidth));
                cacheBitmap.Save(@"C:\Users\1\Desktop\aaa.jpg");
            }
        }
        private void OnBtnTest02Click(object sender, RoutedEventArgs e) {
        }
        #endregion

        // https://www.haolizi.net/example/view_10390.html
        #region Btn00CM
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

        private void Btn09_Click(object sender, RoutedEventArgs e) {

        }
    }
}
