using HelperExternDll;
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
        private string nowOriText = string.Empty;
        private string nowTransText = string.Empty;

        public TranslationWindow() {
            InitializeComponent();
            // Event
            Loaded += OnWindowLoaded;
            Closing += OnWindowClosing;
            StateChanged += OnWindowStateChanged;
            MoveButton.PreviewMouseDown += OnMoveButtonPreviewMouseDown;
            MoveButton.PreviewTouchDown += OnMoveButtonPreviewTouchDown;
            CopyButton.Click += OnCopyButtonClick;
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
            DragMove();
        }

        private void OnMoveButtonPreviewTouchDown(object sender, TouchEventArgs e) {
            DragMove();
        }

        private void OnCopyButtonClick(object sender, RoutedEventArgs e) {
            if (nowOriText != null) {
                Clipboard.SetText(nowOriText);
            }
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
                var meCabNodes = DataManager.Instance.MeCabUniDic22Wrapper.ParseSentence(nowOriText);

                // PrintMeCabNodes(meCabNodes);

                // Add jp
                await Application.Current.Dispatcher.BeginInvoke(() => {
                    TextWarpPanel1.Children.Clear();

                    foreach (var meCabNode in meCabNodes) {
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

                // Add zh
                try {
                    string? tmpText = await DataManager.Instance.NowTranslator.TranslateAsync(nowOriText); // TODO 不知为什么返回 null 就会报错，先 try 起来
                    nowTransText = tmpText ?? DataManager.Instance.NowTranslator.GetLastError() ?? string.Empty;
                } catch (Exception e) {
                    Trace.TraceError(e.Message);
                }

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
        }

        private void OnTextBlockPreviewMouseDoubleClick(object sender, RoutedEventArgs e) {
            if (sender != null) {
                var text = (sender as TextBox)?.Text;
                if (text != null) {
                    Clipboard.SetText(text);
                }
            }
        }

        public void PrintMeCabNodes(MeCabUniDic22Node[] meCabNodes) {
            Trace.TraceInformation($"=======================================");
            Trace.TraceInformation(string.Join(" ", meCabNodes.Select(x => x.Surface).ToList()));
            foreach (var meCabNode in meCabNodes) {
                Trace.TraceInformation($"================");
                Trace.TraceInformation($"meCabNode.Surface: {meCabNode.Surface}");
                // 品詞大分類
                Trace.TraceInformation($"meCabNode.Pos1(品詞大分類): {meCabNode.Pos1}");
                // 品詞中分類
                Trace.TraceInformation($"meCabNode.Pos2(品詞中分類): {meCabNode.Pos2}");
                // 品詞小分類
                Trace.TraceInformation($"meCabNode.Pos3(品詞小分類): {meCabNode.Pos3}");
                // 品詞細分類
                Trace.TraceInformation($"meCabNode.Pos4(品詞細分類): {meCabNode.Pos4}");
                // 活用型
                Trace.TraceInformation($"meCabNode.CType(活用型): {meCabNode.CType}");
                // 活用形
                Trace.TraceInformation($"meCabNode.CForm(活用形): {meCabNode.CForm}");
                // 語彙素読み
                Trace.TraceInformation($"meCabNode.LForm(語彙素読み): {meCabNode.LForm}");
                // 語彙素（語彙素表記＋語彙素細分類）
                Trace.TraceInformation($"meCabNode.Lemma(語彙素（語彙素表記＋語彙素細分類）): {meCabNode.Lemma}");
                // 書字形出現形
                Trace.TraceInformation($"meCabNode.Orth(書字形出現形): {meCabNode.Orth}");
                // 発音形出現形
                Trace.TraceInformation($"meCabNode.Pron(発音形出現形): {meCabNode.Pron}");
                // 書字形基本形
                Trace.TraceInformation($"meCabNode.OrthBase(書字形基本形): {meCabNode.OrthBase}");
                // 発音形基本形
                Trace.TraceInformation($"meCabNode.PronBase(発音形基本形): {meCabNode.PronBase}");
                // 語種
                Trace.TraceInformation($"meCabNode.Goshu(語種): {meCabNode.Goshu}");
                // 語頭変化型
                Trace.TraceInformation($"meCabNode.IType(語頭変化型): {meCabNode.IType}");
                // 語頭変化形
                Trace.TraceInformation($"meCabNode.IForm(語頭変化形): {meCabNode.IForm}");
                // 語末変化型
                Trace.TraceInformation($"meCabNode.FType(語末変化型): {meCabNode.FType}");
                // 語末変化形
                Trace.TraceInformation($"meCabNode.FForm(語末変化形): {meCabNode.FForm}");
                // 語頭変化結合形
                Trace.TraceInformation($"meCabNode.IConType(語頭変化結合形): {meCabNode.IConType}");
                // 語末変化結合形
                Trace.TraceInformation($"meCabNode.FConType(語末変化結合形): {meCabNode.FConType}");
                // 語彙素類
                // この項目の「英語」の名前は、 UniDic 2.2.0 や 2.3.0 の配布物に含まれる dicrc ファイルでは「type」となっているが、 UniDic
                // の FAQ (https://unidic.ninjal.ac.jp/faq#col_name) には「lType」と記載されている。
                Trace.TraceInformation($"meCabNode.LType(語彙素類): {meCabNode.LType}");
                // 仮名形出現形
                Trace.TraceInformation($"meCabNode.Kana(仮名形出現形): {meCabNode.Kana}");
                // 仮名形基本形
                Trace.TraceInformation($"meCabNode.KanaBase(仮名形基本形): {meCabNode.KanaBase}");
                // 語形出現形
                Trace.TraceInformation($"meCabNode.Form(語形出現形): {meCabNode.Form}");
                // 語形基本形
                Trace.TraceInformation($"meCabNode.FormBase(語形基本形): {meCabNode.FormBase}");
                // アクセント型
                Trace.TraceInformation($"meCabNode.AType(アクセント型): {meCabNode.AType}");
                // アクセント結合型
                Trace.TraceInformation($"meCabNode.AConType(アクセント結合型): {meCabNode.AConType}");
                // アクセント修飾型
                Trace.TraceInformation($"meCabNode.AModType(アクセント修飾型): {meCabNode.AModType}");
                // 語彙表ID
                Trace.TraceInformation($"meCabNode.LId(語彙表ID): {meCabNode.LId}");
                // 語彙素ID
                Trace.TraceInformation($"meCabNode.LemmaId(語彙素ID): {meCabNode.LemmaId}");
                Trace.TraceInformation($"================");
            }
        }
    }
}
