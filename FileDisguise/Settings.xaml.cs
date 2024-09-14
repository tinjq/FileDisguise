using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FileDisguise
{
    /// <summary>
    /// Settings.xaml 的交互逻辑
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
            this.DataContext = new SeetingsModel();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            //Console.WriteLine(Password);
            //Console.WriteLine(ExperienceCode);

            Console.WriteLine(JsonSerializer.Serialize(DataContext));

            //using (FileStream fileStream = new FileStream(MainWindow.SettingsFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            //{
            //    StreamReader reader = new StreamReader(fileStream);
            //    string content = reader.ReadToEnd();
            //    Console.WriteLine(content);
            //}

            Hide();
        }
    }
}
