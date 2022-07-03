using HelperProcess;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace ShikiManager {
    public partial class ProcessWindow : Window {
        public ProcessWindow() {
            InitializeComponent();
            // Event
            Loaded += OnWindowLoaded;
            Closing += OnWindowClosing;
            RefreshButton.Click += OnRefreshButtonClick;
            ConfirmButton.Click += OnConfirmButtonClick;
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e) {
            ShowProcessInListBox();
        }

        private void OnWindowClosing(object? sender, CancelEventArgs e) {
            e.Cancel = true;
            Hide();
        }

        private void OnRefreshButtonClick(object sender, RoutedEventArgs e) {
            ShowProcessInListBox();
        }

        private void OnConfirmButtonClick(object sender, RoutedEventArgs e) {
            if (ProcessListBox.SelectedItem != null) {
                DataManager.Instance.ProcessInfo = (ProcessInfo)ProcessListBox.SelectedItem;
                Hide();
            } else {
                MessageBox.Show("请至少选择一项！");
            }
        }

        public new void Show() {
            base.Show();
            ShowProcessInListBox();
        }

        private void ShowProcessInListBox() {
            ProcessListBox.Items.Clear();
            ProcessHelper.GetProcessInfoList()
                .OrderByDescending(x => x.StartTime)
                .ToList()
                .ForEach(x => {
                    ProcessListBox.Items.Add(x);
                });
        }
    }
}
