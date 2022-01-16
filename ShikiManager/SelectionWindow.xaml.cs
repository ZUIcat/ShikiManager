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

            // --
            TextHookDataDic = new Dictionary<TextHookHeadData, TextHookData>();

            // --
            Closing += OnWindowClosing;
            ConfirmButton.Click += OnConfirmButtonClick;
            TestButton.Click += OnTestButtonClick;
        }

        private void OnWindowClosing(object? sender, CancelEventArgs e) {
            e.Cancel = true;
            HideAndDisconnect();
        }

        private async void OnConfirmButtonClick(object sender, RoutedEventArgs e) {
            var allItems = new List<TextHookData>();
            foreach (var item in TextListBox.Items) {
                TextHookData? tmpItem = item as TextHookData;
                if (tmpItem != null) {
                    allItems.Add(tmpItem);
                }
            }
            foreach (var item in TextListBox.SelectedItems) {
                TextHookData? tmpItem = item as TextHookData;
                if (tmpItem != null) {
                    allItems.Remove(tmpItem);
                }
            }
            // Detach
            await TextractorHelper.Instance.DetachProcessByTextHookData(TextListBox.SelectedItems.OfType<TextHookData>());
            // Remove
            //foreach (var item in allItems) {
            //    TextListBox.Items.Remove(item);
            //}
            //
        }

        private void OnTestButtonClick(object sender, RoutedEventArgs e) {
            for (int i = 0; i < 100; i++) {
                ShowTextInListBox(null, new TextHookData($"[3:1C1C:417E20:419E80:{i / 10}:KiriKiriZ:HW-8*14:-8*0@167E20:ああああ.exe] ああああ{i}"));
            }
        }

        public void ShowAndConnect() {
            // Show
            Show();
            // Connect
            TextractorHelper.Instance.TextOutputEvent += ShowTextInListBox;
        }

        public void HideAndDisconnect() {
            // Disconnect
            TextractorHelper.Instance.TextOutputEvent -= ShowTextInListBox;
            // Hide
            Hide();
        }

        private void ShowTextInListBox(TextractorHelper th, TextHookData textHookData) {
            Application.Current.Dispatcher.BeginInvoke((Action<TextHookData>)((textHookData) => {
                // 加到字典中
                if (!TextHookDataDic.ContainsKey(textHookData.HeadData)) {
                    TextHookDataDic.Add(textHookData.HeadData, textHookData);
                } else {
                    TextHookDataDic[textHookData.HeadData] = textHookData;
                }
                // 清除 UI 列表
                TextListBox.Items.Clear(); // TODO 这刷了好几次啊
                // 字典排序后加到 UI 列表中
                TextHookDataDic.OrderBy(x => x.Key).ToList().ForEach(x => {
                    TextListBox.Items.Add(x.Value);
                });
            }), textHookData);
        }
    }
}
