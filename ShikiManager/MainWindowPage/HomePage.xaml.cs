using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShikiManager.MainWindowPage {
    public partial class HomePage : Page {
        ProcessWindow processWindow = null!;
        SelectionWindow selectionWindow = null!;
        TextFilterWindow textFilterWindow = null!;
        TranslationWindow textWindow = null!;

        public HomePage() {
            InitializeComponent();
            // Event
            ProcessButton.Click += OnProcessButtonClick;
            SelectTextButton.Click += OnSelectTextButtonClick;
            SelectFilterButton.Click += OnSelectFilterButtonClick;
            ShowTranslationWindow.Click += OnShowTranslationWindowClick;
        }

        private async void OnProcessButtonClick(object sender, RoutedEventArgs e) {
            await DataManager.Instance.TextractorHelper.DetachAllProcess();

            processWindow ??= new ProcessWindow();
            processWindow.Show();
        }

        private async void OnSelectTextButtonClick(object sender, RoutedEventArgs e) {
            if(string.IsNullOrEmpty(DataManager.Instance.ProcessInfo.Name)) {
                MessageBox.Show("请先选择游戏进程!");
                return;
            }

            await DataManager.Instance.TextractorHelper.AttachProcess(DataManager.Instance.ProcessInfo.ID);

            selectionWindow ??= new SelectionWindow();
            selectionWindow.Show();
        }

        private void OnSelectFilterButtonClick(object sender, RoutedEventArgs e) {
            if (DataManager.Instance.SelectHeadDataList.Count == 0) {
                MessageBox.Show("请先选择需要的游戏 Hook!");
                return;
            }

            textFilterWindow ??= new TextFilterWindow();
            textFilterWindow.Show();
        }

        private void OnShowTranslationWindowClick(object sender, RoutedEventArgs e) {
            if (DataManager.Instance.SelectHeadDataList.Count == 0) {
                MessageBox.Show("请先选择需要的游戏 Hook!");
                return;
            }

            textWindow ??= new TranslationWindow();
            textWindow.Show();
        }
    }
}
