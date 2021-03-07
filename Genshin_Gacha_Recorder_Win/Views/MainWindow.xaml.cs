using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Genshine_Gacha_Recorder_Win;

namespace Genshine_Gacha_Recorder_Win
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModels.GachaItemsViewModel gachaItems;

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = gachaItems;

            gachaItems = new ViewModels.GachaItemsViewModel();

            DataGrid_JueSe.ItemsSource = gachaItems.Info_Records[301];
            DataGrid_Wuqi.ItemsSource = gachaItems.Info_Records[302];
            DataGrid_ChangZhu.ItemsSource = gachaItems.Info_Records[200];
            DataGrid_XinShou.ItemsSource = gachaItems.Info_Records[100];

            DataGrid_JueSe_Sum.ItemsSource = gachaItems.Info_Results[301];
            DataGrid_WuQi_Sum.ItemsSource = gachaItems.Info_Results[302];
            DataGrid_ChangZhu_Sum.ItemsSource = gachaItems.Info_Results[200];
            DataGrid_XinShou_Sum.ItemsSource = gachaItems.Info_Results[100];

            DataGrid_JueSe_Records.ItemsSource = gachaItems.Info_5x_Items[301];
            DataGrid_WuQi_Records.ItemsSource = gachaItems.Info_5x_Items[302];
            DataGrid_ChangZhu_Records.ItemsSource = gachaItems.Info_5x_Items[200];
            DataGrid_XinShou_Records.ItemsSource = gachaItems.Info_5x_Items[100];
        }

        private async void Button_ReadData_Click(object sender, RoutedEventArgs e)
        {
            if (gachaItems.IsOkToLoadData())
            {
                Label_ReadData.Content = "正在读取数据...";
                Button_ReadData.IsEnabled = false;

                await Task.Run(new Action(() => {
                    gachaItems.Update();
                    Button_ReadData.Dispatcher.Invoke(new Action(() =>
                    {
                        Button_ReadData.IsEnabled = true;
                    }));
                    Label_ReadData.Dispatcher.Invoke(new Action(() =>
                    {
                        Label_ReadData.Content = "更新时间:" + DateTime.Now.ToString();
                        gachaItems.Save();
                    }));
                }));

            }
            else
            {
                Label_ReadData.Content = "未找到URL,请进入原神打开祈愿记录界面后重试";
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Button_CloseMainWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_MinimizeMainWindow_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Button_MaximizeMainWindow_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

    }
}
