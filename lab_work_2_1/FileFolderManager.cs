using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;
using System.ComponentModel;

namespace lab_work_2_1
{
    class FileFolderManager
    {
        public bool IsCancel { get; set; }

        public void LoadDrives(ItemCollection collection)
        {
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                TreeViewItem item = new TreeViewItem();
                item.Tag = drive;
                item.Header = drive.ToString();
                item.Items.Add("*");
                collection.Add(item);
            }
        }

        public void OpenFolder(object selectedItem)
        {
            TreeViewItem item = (TreeViewItem)selectedItem;
            item.Items.Clear();
            DirectoryInfo dir;
            if (item.Tag is DriveInfo)
            {
                DriveInfo drive = (DriveInfo)item.Tag;
                dir = drive.RootDirectory;
            }
            else dir = (DirectoryInfo)item.Tag;
            try
            {
                foreach (DirectoryInfo subDir in dir.GetDirectories())
                {
                    TreeViewItem newItem = new TreeViewItem();
                    newItem.Tag = subDir;
                    newItem.Header = subDir.ToString();
                    newItem.Items.Add("*");
                    item.Items.Add(newItem);
                }
            }
            catch
            { }
        }

        public void SearchFile(SearchData data)
        {
            IEnumerable<string> enumerable = SafeWalk.EnumerateFiles(data.PathFolder, "*.*", SearchOption.AllDirectories);

            int countFile = enumerable.Count();

            data.PogressBarMax(countFile == 0 ? 1 : countFile);

            foreach (var item in enumerable)
            {
                if (IsCancel == true)
                    return;

                string fileName = Path.GetFileName(item);

                if (Regex.IsMatch(fileName, data.Text, RegexOptions.IgnoreCase))
                    data.AddNewFileToList(new FilePath(fileName, item));

                data.PogressBarIncement();

                Thread.Sleep(1);
            }
        }

        public void SearchTextInFile(SearchData data)
        {
            IEnumerable<string> enumerable = SafeWalk.EnumerateFiles(data.PathFolder, "*.*", SearchOption.AllDirectories);

            int countFile = enumerable.Count();

            data.PogressBarMax(countFile == 0 ? 1 : countFile);

            foreach (var item in enumerable)
            {
                if (IsCancel == true)
                    return;

                string[] textFile = new string[2];

                try
                {
                    textFile[0] = File.ReadAllText(item, Encoding.Default);
                    textFile[1] = File.ReadAllText(item, Encoding.UTF8);
                }
                catch { }

                for (int j = 0; j < 2; j++)
                    if (Regex.IsMatch(textFile[j], data.Text, RegexOptions.IgnoreCase))
                        data.AddNewFileToList(new FilePath(Path.GetFileName(item), item));

                data.PogressBarIncement();
            }
        }

    }
}
