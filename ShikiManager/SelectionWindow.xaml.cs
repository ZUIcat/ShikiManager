using HelperTextractor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ShikiManager {
    public partial class SelectionWindow : Window {
        public Dictionary<TextHookHeadData, TextHookData> TextHookDataDic { get; set; }

        public SelectionWindow() {
            InitializeComponent();

            // -- //
            TextHookDataDic = new Dictionary<TextHookHeadData, TextHookData>();

            // -- //
            Closing += OnWindowClosing;
            ConfirmButton.Click += OnConfirmButtonClick;
            TestButton.Click += OnTestButtonClick;
        }

        public void ShowAndConnect() {
            // Show
            Show();
            // Connect
        }

        public void HideAndDisconnect() {
            // Disconnect
            // Hide
            Hide();
        }

        private void OnWindowClosing(object? sender, CancelEventArgs e) {
            e.Cancel = true;
            HideAndDisconnect();
        }

        public void ShowTextInListBox(TextHookData textHookData) {
            if (!TextHookDataDic.ContainsKey(textHookData.HeadData)) {
                TextHookDataDic.Add(textHookData.HeadData, textHookData);
            } else {
                TextHookDataDic[textHookData.HeadData] = textHookData;
            }
            TextListBox.Items.Clear();
            foreach (var data in TextHookDataDic.Values) {
                TextListBox.Items.Add(data);
            }
        }

        public void OnConfirmButtonClick(object sender, RoutedEventArgs e) {
            foreach (var item in TextListBox.SelectedItems) {
                Trace.TraceInformation((item as TextHookData)?.TextData);
            }
        }

        private void OnTestButtonClick(object sender, RoutedEventArgs e) {
            for (int i = 0; i < 100; i++) {
                ShowTextInListBox(new TextHookData($"[3:1C1C:417E20:419E80:{i / 10}:KiriKiriZ:HW-8*14:-8*0@167E20:ああああ.exe] ああああ{i}"));
            }
        }
    }
}
