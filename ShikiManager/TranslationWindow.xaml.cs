using HelperExternDll;
using HelperMeCab;
using HelperTextractor;
using HelperTranslator;
using NMeCab.Specialized;
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
    public partial class TranslationWindow : Window {
        private MeCabUniDic22Node[]? nowMeCabNodes = null;
        private string nowOriText = string.Empty;
        private string nowTransText = string.Empty;

        public TranslationWindow() {
            InitializeComponent();
            // Event
            Loaded += OnWindowLoaded;
            Closing += OnWindowClosing;
            StateChanged += OnWindowStateChanged;
            MoveButton.PreviewMouseDown += OnMoveButtonPreviewMouseDown;
            CopyButton.Click += OnCopyButtonClick;
            TranslateButton.Click += OnTranslateButtonClick;
            SettingButton.Click += OnSettingButtonClick;
            HideButton.Click += OnHideButtonClick;
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e) {
            // 设置窗口不获取焦点
            WindowInteropHelper wndHelper = new WindowInteropHelper(this);
            Winuser.SetWindowNoActivate(wndHelper.Handle);
        }

        private void OnWindowClosing(object? sender, CancelEventArgs e) {
            e.Cancel = true;
            Hide();
        }

        private void OnWindowStateChanged(object sender, EventArgs e) {
            if (WindowState != WindowState.Normal) {
                WindowState = WindowState.Normal;
            }
        }

        private void OnMoveButtonPreviewMouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Left && Mouse.LeftButton == MouseButtonState.Pressed) {
                DragMove();
            }
            e.Handled = true;
        }

        private void OnCopyButtonClick(object sender, RoutedEventArgs e) {
            if (nowOriText != null) {
                Clipboard.SetText(nowOriText);
            }
        }

        private async void OnTranslateButtonClick(object sender, RoutedEventArgs e) {
            await ShowText2();
        }

        private void OnSettingButtonClick(object sender, RoutedEventArgs e) {

        }

        private void OnHideButtonClick(object sender, RoutedEventArgs e) {
            Hide();
        }

        public new void Show() {
            // Show
            base.Show();
            // Connect
            DataManager.Instance.TextractorHelper.TextOutputEvent += ShowText;
        }

        public new void Hide() {
            // Disconnect
            DataManager.Instance.TextractorHelper.TextOutputEvent -= ShowText;
            // Hide
            base.Hide();
        }

        public async void ShowText(TextHookData textHookData) {
            if (DataManager.Instance.SelectHeadDataList.Contains(textHookData.HeadData)) {
                var filterFunc = DataManager.Instance.TextFilterFunc;
                nowOriText = filterFunc == null ? textHookData.TextData : filterFunc(textHookData.TextData);
                nowMeCabNodes = DataManager.Instance.MeCabUniDic22Wrapper.ParseSentence(nowOriText);

                // MeCabUniDic22Wrapper.PrintMeCabNodes(nowMeCabNodes);

                await ShowText1();
                await ShowText2();
            }
        }

        private async Task ShowText1() {
            await Application.Current.Dispatcher.BeginInvoke(() => {
                TextWarpPanel1.Children.Clear();

                if (nowMeCabNodes == null) {
                    return;
                }

                foreach (var meCabNode in nowMeCabNodes) {
                    var tmpStackPanel = new StackPanel();
                    tmpStackPanel.Orientation = Orientation.Vertical;

                    var textBox1 = new TextBox();
                    textBox1.BorderThickness = new Thickness(0);
                    textBox1.IsReadOnly = true;
                    textBox1.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                    textBox1.Text = meCabNode.Surface;
                    textBox1.PreviewMouseDoubleClick += OnTextBlockPreviewMouseDoubleClick;
                    tmpStackPanel.Children.Add(textBox1);

                    var textBox2 = new TextBox();
                    textBox2.BorderThickness = new Thickness(0);
                    textBox2.IsReadOnly = true;
                    textBox2.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                    textBox2.Text = meCabNode.Pron;
                    textBox2.PreviewMouseDoubleClick += OnTextBlockPreviewMouseDoubleClick;
                    tmpStackPanel.Children.Add(textBox2);

                    var border = new Border();
                    border.Margin = new Thickness(5, 2, 5, 2);
                    border.Child = tmpStackPanel;

                    TextWarpPanel1.Children.Add(border);
                }
            });
        }

        private async Task ShowText2() {
            string? tmpText = await DataManager.Instance.NowTranslator.TranslateAsync(nowOriText);
            nowTransText = tmpText ?? DataManager.Instance.NowTranslator.GetLastError() ?? string.Empty;

            await Application.Current.Dispatcher.BeginInvoke(() => {
                TextWarpPanel2.Children.Clear();

                var textBox = new TextBox();
                textBox.BorderThickness = new Thickness(0);
                textBox.IsReadOnly = true;
                textBox.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                textBox.TextWrapping = TextWrapping.Wrap;
                textBox.Text = nowTransText;

                TextWarpPanel2.Children.Add(textBox);
            });
        }

        private void OnTextBlockPreviewMouseDoubleClick(object sender, RoutedEventArgs e) {
            if (sender != null) {
                var text = (sender as TextBox)?.Text;
                if (text != null) {
                    Clipboard.SetText(text);
                }
            }
        }
    }
}
