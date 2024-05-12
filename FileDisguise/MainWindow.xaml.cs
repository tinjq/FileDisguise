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

namespace FileDisguise
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        int disguiseLength = 1024 * 1024;
        List<string> dragedPathList = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void txtFilePath_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effects = DragDropEffects.Link;
            else
                e.Effects = DragDropEffects.None;
        }

        private void txtFilePath_Drop(object sender, DragEventArgs e)
        {
            Array fileObjectArray = (Array)e.Data.GetData(DataFormats.FileDrop);
            dragedPathList.Clear();
            foreach (var item in fileObjectArray)
            {
                dragedPathList.Add(item.ToString());
            }

            txtFilePath.Text = string.Join(Environment.NewLine, dragedPathList);
        }

        /// <summary>
        /// 伪装/还原文件类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisguiseType_Click(object sender, RoutedEventArgs e)
        {
            List<string> pathList = new List<string>();
            dragedPathList.ForEach(path => pathList.AddRange(FileNameDisguise.GetAllFiles(path, false)));

            try
            {
                pathList.ForEach(item => FileContentDisguise.FileDisguiseRecover(item, disguiseLength));
                MessageBox.Show("成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 还原文件类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void RecoverType_Click(object sender, RoutedEventArgs e)
        //{
        //    List<string> pathList = new List<string>();
        //    dragedPathList.ForEach(path => pathList.AddRange(FileNameDisguise.GetAllFiles(path, false)));

        //    try
        //    {
        //        pathList.ForEach(x => FileContentDisguise.FileRecover(x, disguiseLength));
        //        MessageBox.Show("成功");
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        /// <summary>
        /// 伪装文件名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisguiseName_Click(object sender, RoutedEventArgs e)
        {
            List<string> pathList = new List<string>();
            dragedPathList.ForEach(path => pathList.AddRange(FileNameDisguise.GetAllFiles(path, true)));

            try
            {
                pathList.ForEach(x => FileNameDisguise.EncodeFileName(x));
                MessageBox.Show("成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 还原文件名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RecoverName_Click(object sender, RoutedEventArgs e)
        {
            List<string> pathList = new List<string>();
            dragedPathList.ForEach(path => pathList.AddRange(FileNameDisguise.GetAllFiles(path, true)));

            try
            {
                pathList.ForEach(x => FileNameDisguise.DecodeFileName(x));
                MessageBox.Show("成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox combobox = sender as ComboBox;
            if (combobox == null) return;

            ComboBoxItem item = combobox.SelectedItem as ComboBoxItem;
            string sizeStr = item.Content.ToString();

            disguiseLength = getSize(sizeStr);
        }

        int getSize(string sizeStr)
        {
            string baseStr = sizeStr.Substring(sizeStr.Length - 1);

            int size = int.Parse(sizeStr.Substring(0, sizeStr.Length - 1));
            return size * getBase(baseStr);
        }

        int getBase(string baseStr)
        {
            switch (baseStr)
            {
                case "B": return 1;
                case "K": return 1024;
                case "M": return 1024 * 1024;
                case "G": return 1024 * 1024 * 1024;
                default: return 1;
            }
        }
    }
}
