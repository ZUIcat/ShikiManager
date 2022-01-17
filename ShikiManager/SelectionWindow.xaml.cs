using HelperTextractor;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace ShikiManager {
    public partial class SelectionWindow : Window {
        public SelectionWindow() {
            InitializeComponent();
            // Event
            Closing += OnWindowClosing;
            ConfirmButton.Click += OnConfirmButtonClick;
        }

        private void OnWindowClosing(object? sender, CancelEventArgs e) {
            e.Cancel = true;
            HideAndDisconnect();
        }

        private async void OnConfirmButtonClick(object sender, RoutedEventArgs e) {
            // Detach Hook
            var selectedItems = TextListBox.SelectedItems.OfType<TextHookData>();
            await TextractorHelper.Instance.DetachProcessByTextHookData(selectedItems);
            // Clear UI
            TextListBox.Items.Clear();
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

        private void ShowTextInListBox(TextHookData textHookData) {
            Application.Current.Dispatcher.BeginInvoke((Action<TextHookData>)((textHookData) => {
                TextListBox.Items.Clear();
                TextractorHelper.Instance.TextractorOutPutDic.Values
                .OrderBy(x => x.HeadData).ToList().ForEach(x => {
                    TextListBox.Items.Add(x);
                });
            }), textHookData);
        }
    }
}
