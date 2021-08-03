using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using WindowsInput.Events;
using HelperExternDll;
using System.Drawing;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing.Imaging;
using HelperConfig;
using System.IO;

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

        private string screenshotSaveDir;

        public ToolWindow() {
            InitializeComponent();

            hideDir = 1;
            windowPos = 2;
            screenWidth = Winuser.GetPrimaryScreenWidth();
            screenHeight = Winuser.GetPrimaryScreenHeight();
            centerScreenPosX = screenWidth / 2;
            centerScreenPosY = screenHeight / 2;

            keyReleaseDic = new Dictionary<KeyCode, bool>();
            keyReleaseDicLock = new object();

            ReadConfig();
        }

        private void ReadConfig() {
            screenshotSaveDir = ConfigHelper.Instance.AppConfig.ScreenshotPath;
            if (screenshotSaveDir.StartsWith(@".\")) {
                screenshotSaveDir = Path.Combine(ConfigHelper.Instance.AppDataPath, screenshotSaveDir);
            }
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
        /// <summary>
        /// 释放指定按键.
        /// </summary>
        /// <param name="key">按键代码</param>
        private void ReleaseKey(KeyCode key) {
            lock (keyReleaseDicLock) {
                keyReleaseDic[key] = true;
            }
        }

        #region Btn
        private void OnBtnSetClick(object sender, RoutedEventArgs e) {
            Btn00CM.IsOpen = true;
        }

        private async void OnBtnPScreenClick(object sender, RoutedEventArgs e) {
            this.Hide();
            await WindowsInput.Simulate.Events()
                .ClickChord(KeyCode.LWin, KeyCode.PrintScreen)
                .Wait(500)
                .Invoke();
            this.Show();
        }

        private async void OnBtnCtrlPreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            await WindowsInput.Simulate.Events()
                .Hold(KeyCode.Control)
                .Invoke();
        }
        private async void OnBtnCtrlPreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            await WindowsInput.Simulate.Events()
                .Release(KeyCode.Control)
                .Invoke();
        }

        private async void OnBtnSpacePreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            await PressKey(KeyCode.Space);
        }
        private void OnBtnSpacePreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            ReleaseKey(KeyCode.Space);
        }

        private async void OnBtnKUpPreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            await PressKey(KeyCode.Up);
        }
        private void OnBtnKUpPreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            ReleaseKey(KeyCode.Up);
        }

        private async void OnBtnKDownPreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            await PressKey(KeyCode.Down);
        }
        private void OnBtnKDownPreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            ReleaseKey(KeyCode.Down);
        }

        private async void OnBtnHomePreviewMouseDown(object sender, RoutedEventArgs e) {
            await PressKey(KeyCode.Home);
        }        
        private void OnBtnHomePreviewMouseUp(object sender, RoutedEventArgs e) {
            ReleaseKey(KeyCode.Home);
        }

        private async void OnBtnEndPreviewMouseDown(object sender, RoutedEventArgs e) {
            await PressKey(KeyCode.End);
        }
        private void OnBtnEndPreviewMouseUp(object sender, RoutedEventArgs e) {
            ReleaseKey(KeyCode.End);
        }

        private async void OnBtnMUpClick(object sender, RoutedEventArgs e) {
            Winuser.POINT curPos;
            Winuser.GetCursorPos(out curPos);
            await WindowsInput.Simulate.Events()
                .MoveTo(centerScreenPosX, centerScreenPosY)
                .Scroll(ButtonCode.VScroll, ButtonScrollDirection.Up)
                .Wait(100)
                .MoveTo(curPos.X, curPos.Y)
                .Invoke();
        }
        private async void OnBtnMDownClick(object sender, RoutedEventArgs e) {
            Winuser.POINT curPos;
            Winuser.GetCursorPos(out curPos);
            await WindowsInput.Simulate.Events()
                .MoveTo(centerScreenPosX, centerScreenPosY)
                .Scroll(ButtonCode.VScroll, ButtonScrollDirection.Down)
                .Wait(100)
                .MoveTo(curPos.X, curPos.Y)
                .Invoke();
        }

        private async void OnBtnMLeftClick(object sender, RoutedEventArgs e) {
            Winuser.POINT curPos;
            Winuser.GetCursorPos(out curPos);
            await WindowsInput.Simulate.Events()
                .MoveTo(centerScreenPosX, centerScreenPosY)
                .ClickChord(KeyCode.LButton)
                .Wait(100)
                .MoveTo(curPos.X, curPos.Y)
                .Invoke();
        }
        private async void OnBtnMRightClick(object sender, RoutedEventArgs e) {
            Winuser.POINT curPos;
            Winuser.GetCursorPos(out curPos);
            await WindowsInput.Simulate.Events()
                .MoveTo(centerScreenPosX, centerScreenPosY)
                .ClickChord(KeyCode.RButton)
                .Wait(100)
                .MoveTo(curPos.X, curPos.Y)
                .Invoke();
        }

        private void OnBtnTest01Click(object sender, RoutedEventArgs e) {
            this.Hide();
            using (Bitmap cacheBitmap = new Bitmap(screenWidth, screenHeight)) {
                Graphics g = Graphics.FromImage(cacheBitmap);
                g.CopyFromScreen(0, 0, 0, 0, new System.Drawing.Size(screenWidth, screenWidth));
                string imageSavePath = Path.Combine(screenshotSaveDir, System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-ff") + ".png");
                Directory.CreateDirectory(screenshotSaveDir);
                cacheBitmap.Save(imageSavePath, ImageFormat.Png);
            }
            this.Show();
        }
        private void OnBtnTest02Click(object sender, RoutedEventArgs e) {
        }
        #endregion

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
    }
}
