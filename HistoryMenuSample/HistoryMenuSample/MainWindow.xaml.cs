﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace HistoryMenuSample
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        const int _HistorySize = 10;

        public ObservableFixedQueue<string> Files { get; set; } = new ObservableFixedQueue<string>(_HistorySize);

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;

            // データを詰める
            for (var i = 0; i < _HistorySize; i++)
            {
                var item = Properties.Settings.Default.FilePaths[i];

                Files.Enqueue(item);
            }
        }

        private void MenuItem_Initialized(object sender, EventArgs e)
        {
            var mainMenu = sender as MenuItem;

            /*
            this.DataContext = this;

            // データを詰める
            for (var i = 0; i < _HistorySize; i++)
            {
                var item = Properties.Settings.Default.FilePaths[i];

                Files.Enqueue(item);
            }

            var menus = GetHistoryMenu(_HistorySize);

            foreach(var menu in menus)
            {
                // クリックイベントを追加する例
                menu.Click += (s, args) =>
                {
                    var selectedItem = s as MenuItem;

                    Console.WriteLine(selectedItem.Header);
                };

                mainMenu.Items.Add(menu);
            }
            */
        }

        private IEnumerable<MenuItem> GetHistoryMenu(int count)
        {
            var menu = new List<MenuItem>();

            for (var i = 0; i < count; i++)
            {
                var item = new MenuItem();
                var binding = new Binding();

                binding.Source = this;
                binding.Path = new PropertyPath($"{nameof(Files)}[{i}]");
                binding.Mode = BindingMode.OneWay;
                binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

                item.SetBinding(MenuItem.HeaderProperty, binding);

                menu.Add(item);
            }

            return menu;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var text = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm-ss")} に追加したデータ";
            
            Files.Enqueue(text);

            // 履歴を保存
            Properties.Settings.Default.FilePaths = Files.ToStringCollection();
            Properties.Settings.Default.Save();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = e.OriginalSource as MenuItem;
            MessageBox.Show(String.Format($"「{menuItem.Header}」をクリックしました。"));
        }
    }
}
