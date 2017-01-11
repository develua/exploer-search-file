using MahApps.Metro.Controls;
using System.Windows;
using System;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Media;
using System.Threading.Tasks;
using System.Threading;

namespace lab_work_2_1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        FileFolderManager manager;

        public MainWindow()
        {
            InitializeComponent();

            manager = new FileFolderManager();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            manager.LoadDrives(treeFolder.Items);
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            searchButton.IsEnabled = false;

            listFile.Items.Clear();

            manager.IsCancel = false;

            SearchData data = new SearchData();
            data.PathFolder = GetFullPath(treeFolder.SelectedItem);
            data.Text = search.Text;
            data.PogressBarMax = ProgressBarMax;
            data.PogressBarIncement = ProgressBarIncrement;
            data.AddNewFileToList = AddNewFileToList;

            bool? isCheked = searchInFile.IsChecked;

            await Task.Factory.StartNew(() =>
            {
                if (isCheked == true)
                    manager.SearchTextInFile(data);
                else
                    manager.SearchFile(data);

                Thread.Sleep(1000);
            });

            progressBar.Value = 0;
            searchButton.IsEnabled = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            manager.IsCancel = true;
        }

        private void TreeFolder_Expanded(object sender, RoutedEventArgs e)
        {
            manager.OpenFolder(treeFolder.SelectedItem);
        }

        private void ListFile_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Process.Start((listFile.SelectedItem as FilePath).PathFile);
        }

        private TreeViewItem GetParentItem(TreeViewItem item)
        {
            for (var i = VisualTreeHelper.GetParent(item); i != null; i = VisualTreeHelper.GetParent(i))
                if (i is TreeViewItem)
                    return (TreeViewItem)i;

            return null;
        }

        private string GetFullPath(object selected)
        {
            TreeViewItem node = (TreeViewItem)selected;

            if (node == null)
                return "";

            var result = Convert.ToString(node.Header);

            for (var i = GetParentItem(node); i != null; i = GetParentItem(i))
                result = i.Header + "\\" + result;

            return result;
        }

        private void ProgressBarIncrement()
        {
            Dispatcher.BeginInvoke(new Action(() => progressBar.Value += 1));
        }

        private void ProgressBarMax(int max)
        {
            Dispatcher.BeginInvoke(new Action(() => progressBar.Maximum = max));
        }

        private void AddNewFileToList(FilePath fp)
        {
            Dispatcher.BeginInvoke(new Action(() => listFile.Items.Add(fp)));
        }

    }
}
