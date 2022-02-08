using HelperTextractor;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ShikiManager {
    public partial class SelectionWindow : Window {
        /// <summary>
        /// 自动 Detach 其它未选中的 Hook
        /// </summary>
        public bool AutoDetach { get; set; } = true;

        public SelectionWindow() {
            InitializeComponent();
            // Binding
            DataContext = this;
            AutoDetachCheckBox.SetBinding(CheckBox.IsCheckedProperty, new Binding("AutoDetach") {
                AsyncState = BindingMode.TwoWay // TODO 不起作用
            });
            // Event
            Closing += OnWindowClosing;
            ConfirmButton.Click += OnConfirmButtonClick;
        }

        private void OnWindowClosing(object? sender, CancelEventArgs e) {
            e.Cancel = true;
            Hide();
        }

        private async void OnConfirmButtonClick(object sender, RoutedEventArgs e) {
            var selectedItems = TextListBox.SelectedItems.OfType<TextHookData>();

            DataManager.Instance.SelectHeadDataList.Clear();
            foreach (var item in selectedItems) {
                DataManager.Instance.SelectHeadDataList.Add(item.HeadData);
            }
            
            if (AutoDetach) {
                // Detach Hook
                await DataManager.Instance.TextractorHelper.DetachProcessByTextHookData(selectedItems);
                // Clear UI
                TextListBox.Items.Clear();
            }
        }

        public new void Show() {
            // Show
            base.Show();
            // Connect
            DataManager.Instance.TextractorHelper.TextOutputEvent += ShowTextInListBox;
        }

        public new void Hide() {
            // Disconnect
            DataManager.Instance.TextractorHelper.TextOutputEvent -= ShowTextInListBox;
            // Hide
            base.Hide();
        }

        private void ShowTextInListBox(TextHookData textHookData) {
            Application.Current.Dispatcher.BeginInvoke((Action<TextHookData>)((textHookData) => {
                TextListBox.Items.Clear();
                DataManager.Instance.TextractorHelper.TextractorOutPutDic.Values
                    .OrderBy(x => x.HeadData)
                    .ToList()
                    .ForEach(x => {
                        TextListBox.Items.Add(x);
                    });
            }), textHookData);
        }
    }
}
