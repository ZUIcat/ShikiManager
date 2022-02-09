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
                // Refresh UI
                ShowTextInListBox(null!);
            }

            MessageBox.Show("已成功选择！\n可手动关闭本窗口。");
            // Hide();
        }

        public new void Show() {
            // Show
            base.Show();
            // Connect
            DataManager.Instance.TextractorHelper.TextOutputEvent += ShowTextInListBox;
            // 为了即时显示，每次 Show 之后就立即显示一次
            ShowTextInListBox(null!);
        }

        public new void Hide() {
            // Disconnect
            DataManager.Instance.TextractorHelper.TextOutputEvent -= ShowTextInListBox;
            // Hide
            base.Hide();
        }

        private void ShowTextInListBox(TextHookData _) {
            // 虽然每次输出都调用一次这个方法有些浪费
            // 但是这不是长期开着的窗口，感觉问题不大
            Application.Current.Dispatcher.BeginInvoke(() => {
                // 把 Items 全清了后，把 TextractorHelper 暂存字典里的值都拿来排序后显示
                TextListBox.Items.Clear();
                DataManager.Instance.TextractorHelper.TextractorOutPutDic.Values
                    .OrderBy(x => x.HeadData)
                    .ToList()
                    .ForEach(x => {
                        TextListBox.Items.Add(x);
                    });
                // 若存在已经选中的项，选中它们
                // TextListBox.SelectedItems.Clear(); // 上面已经 Clear 了 Items，这里不需要 Clear
                DataManager.Instance.TextractorHelper.TextractorOutPutDic.Values
                   .Where(x => DataManager.Instance.SelectHeadDataList.Contains(x.HeadData)) // 改成 Linq 嵌套？
                   .ToList()
                   .ForEach((x) => {
                       TextListBox.SelectedItems.Add(x);
                   });
            });
        }
    }
}
