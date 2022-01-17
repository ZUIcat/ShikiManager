using HelperExternDll;
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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ShikiManager {
    public partial class TextWindow : Window {
        public TextWindow() {
            InitializeComponent();
            // Event
            Loaded += Window_Loaded;
            Closing += OnWindowClosing;
            StateChanged += Window_StateChanged;
            MoveButton.PreviewMouseDown += MoveButton_PreviewMouseDown;
            MoveButton.PreviewTouchDown += MoveButton_PreviewTouchDown;
            SettingButton.Click += SettingButton_Click;
            CloseButton.Click += CloseButton_Click;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            // 设置窗口不获取焦点
            WindowInteropHelper wndHelper = new WindowInteropHelper(this);
            Winuser.SetWindowNoActivate(wndHelper.Handle);
        }

        private void OnWindowClosing(object? sender, CancelEventArgs e) {
            e.Cancel = true;
            HideAndDisconnect();
        }

        private void Window_StateChanged(object sender, EventArgs e) {
            if (WindowState != WindowState.Normal) {
                WindowState = WindowState.Normal;
            }
        }

        private void MoveButton_PreviewMouseDown(object sender, MouseButtonEventArgs e) {
            DragMove();
        }

        private void MoveButton_PreviewTouchDown(object sender, TouchEventArgs e) {
            DragMove();
        }

        private void SettingButton_Click(object sender, RoutedEventArgs e) {

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e) {
            HideAndDisconnect();
        }

        public void ShowAndConnect() {
            // Show
            Show();
            // Connect
            TextractorHelper.Instance.TextOutputEvent += ShowText;
        }

        public void HideAndDisconnect() {
            // Disconnect
            TextractorHelper.Instance.TextOutputEvent -= ShowText;
            // Hide
            Hide();
        }

        public void ShowText(TextHookData textHookData) {
            Application.Current.Dispatcher.BeginInvoke((Action<TextHookData>)((textHookData) => {
                TextWarpPanel.Children.Clear();
                var textBlock = new TextBlock();
                textBlock.Text = $"[{textHookData.TextData}]\n{textHookData.TextData}";
                var border = new Border();
                border.Margin = new Thickness(1, 1, 1, 1);
                border.Child = textBlock;
                TextWarpPanel.Children.Add(border);
            }), textHookData);
        }
    }
}
