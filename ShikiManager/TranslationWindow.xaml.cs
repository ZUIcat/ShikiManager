using HelperExternDll;
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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ShikiManager {
    public partial class TextWindow : Window {
        public TextWindow() {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            // 设置窗口不获取焦点
            WindowInteropHelper wndHelper = new WindowInteropHelper(this);
            Winuser.SetWindowNoActivate(wndHelper.Handle);
        }

        private void Window_Closed(object sender, EventArgs e) {

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
            ChangeTextImpl("szdgfsg");
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e) {
            Hide();
            //Close();
        }

        public void ChangeText(TextHookData textHookData) {
            Trace.TraceInformation("dddddddddddddddddddddd");
            Application.Current.Dispatcher.BeginInvoke((Action<string>)((text) => ChangeTextImpl(text)), textHookData.TextData);
        }

        public void ChangeTextImpl(string text) {
            Trace.TraceInformation("ffffffffffffffffffffffffff"); // TODO 为啥关闭了之后还会调用？？
            TextWarpPanel.Children.Clear();
            TextBlock textBlock = new TextBlock();
            textBlock.Text = $"[{text}]\n{text}";
            Border border = new Border();
            border.Margin = new Thickness(1, 1, 1, 1);
            border.Child = textBlock;
            TextWarpPanel.Children.Add(border);
        }
    }
}
