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

        private Thread t = null;

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = gachaItems;

            gachaItems = new ViewModels.GachaItemsViewModel();
            DataGrid_JueSe.ItemsSource = gachaItems.Info_Record[301];
            DataGrid_Wuqi.ItemsSource = gachaItems.Info_Record[302];
            DataGrid_ChangZhu.ItemsSource = gachaItems.Info_Record[200];
            DataGrid_XinShou.ItemsSource = gachaItems.Info_Record[100];

            DataGrid_JueSe_Sum.ItemsSource = gachaItems.Info_Result[301];
            DataGrid_WuQi_Sum.ItemsSource = gachaItems.Info_Result[302];
            DataGrid_ChangZhu_Sum.ItemsSource = gachaItems.Info_Result[200];
            DataGrid_XinShou_Sum.ItemsSource = gachaItems.Info_Result[100];
        }

        private void Button_ReadData_Click(object sender, RoutedEventArgs e)
        {
            if (gachaItems.IsOkToLoadData())
            {
                Label_ReadData.Content = "正在读取数据...";
                Button_ReadData.IsEnabled = false;
                t = new Thread(new ThreadStart(gachaItems.Update));
                t.Start();
                t.IsBackground = true;
                DispatcherTimer timer = new DispatcherTimer();
                timer.Tick += new EventHandler(OnGachaInfoLoaded);
                timer.Interval = new TimeSpan(0, 0, 1);
                timer.Start();
            }
            else
            {
                Label_ReadData.Content = "请进入原神打开祈愿记录界面后重试";
            }
        }
        private void OnGachaInfoLoaded(object sender, EventArgs e)
        {
            DispatcherTimer timer = ((DispatcherTimer)sender);

            if(t.ThreadState == ThreadState.Stopped)
            {
                t.Join();
                t = null;

                gachaItems.Save();
                Button_ReadData.IsEnabled = true;
                timer.Stop();
                Label_ReadData.Content = "更新时间:" + DateTime.Now.ToString(); 
            }

        }
    }
}
