using HelperTextractor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class TextFilterWindow : Window {
        private Dictionary<string, Func<string, string>> textFilterDic = new Dictionary<string, Func<string, string>>();
        private Func<string, string>? selectedFilter;

        public TextFilterWindow() {
            InitializeComponent();
            // Event
            Closing += OnWindowClosing;
            TextFilterComboBox.SelectionChanged += OnTextFilterComboBoxSelectionChanged;
            ConfirmButton.Click += OnConfirmButtonClick;

            textFilterDic.Add("Remove2SameChar", TextDataFilter.Remove2SameChar);
            textFilterDic.Add("Remove3SameChar", TextDataFilter.Remove3SameChar);
            textFilterDic.Add("Remove4SameChar", TextDataFilter.Remove4SameChar);
            textFilterDic.Add("Remove5SameChar", TextDataFilter.Remove5SameChar);
            foreach (var filterName in textFilterDic.Keys) {
                TextFilterComboBox.Items.Add(filterName);
            }
        }

        private void OnWindowClosing(object? sender, CancelEventArgs e) {
            e.Cancel = true;
            Hide();
        }
        private void OnTextFilterComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (TextFilterComboBox.SelectedItem is string selectedItem) {
                textFilterDic.TryGetValue(selectedItem, out selectedFilter);
            }
        }

        private void OnConfirmButtonClick(object sender, RoutedEventArgs e) {
            DataManager.Instance.TextFilterFunc = selectedFilter;
            Hide();
        }

        public new void Show() {
            // Show
            base.Show();
            // Connect
            DataManager.Instance.TextractorHelper.TextOutputEvent += ShowTextInUI;
        }

        public new void Hide() {
            // Disconnect
            DataManager.Instance.TextractorHelper.TextOutputEvent -= ShowTextInUI;
            // Hide
            base.Hide();
        }

        private void ShowTextInUI(TextHookData textHookData) {
            if(DataManager.Instance.SelectHeadDataList.Contains(textHookData.HeadData)) {
                Application.Current.Dispatcher.BeginInvoke((Action<TextHookData>)((textHookData) => {
                    OldText.Text = textHookData.TextData;
                    NewText.Text = selectedFilter == null? textHookData.TextData : selectedFilter(textHookData.TextData);
                }), textHookData);
            }
        }
    }
}
