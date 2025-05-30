/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * MainWindow.xaml.cs is part of the IOLab project.
 *
 * IOLab is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * IOLab is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with IOLab. If not, see <https://www.gnu.org/licenses/>.
 */

using iNKORE.UI.WPF.Modern;
using iNKORE.UI.WPF.Modern.Controls;
using IOLab.Core.Assets;
using IOLab.Core.Generic;
using IOLab.Core.Properties;
using IOLab.Core.Serial;
using IOLab.Network.API;
using IOLab.Pages;
using IOLab.Pages.Appreciations;
using IOLab.Pages.Remote.ViewModel;
using IOLab.Windows.Dialog.Universal;
using IOLab.Windows.LabAI;
using Microsoft.Toolkit.Uwp.Notifications; // 引入通知库
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace IOLab
{
    public class NavigationManager
    {
        private readonly Dictionary<string, Page> _pageCache = new Dictionary<string, Page>();
        



        // 缓存页面
        public Page GetPage(NavigationViewItem item)
        {
            var pageKey = item.Name;
            if (!_pageCache.TryGetValue(pageKey, out var page))
            {
                page = item.Name switch
                {
                    "NavigationViewItem_Home" => new HomePage(),
                    "NavigationViewItem_Remote" => new RemotePage(),
                    "NavigationViewItem_Monitor" => new MonitorPage(),
                    "NavigationViewItem_Setting" => new SettingPage(),
                    "NavigationViewItem_About" => new AboutPage(),
                    "NavigationViewItem_Workshop" => new WorkshopPage(),
                    "NavigationViewItem_ExperimentMaker" => new ExperimentMakerPage(),
                    _ => throw new InvalidOperationException("Unknown navigation item.")
                };
                _pageCache[pageKey] = page;
            }
            return page;
        }

        // 重载页面
        public void ResetPage(string name)
        {
            try
            {
                _pageCache.Remove(name);
            }
            catch
            {
                return;
            }
        }

    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // 提示框队列
        private static Queue<ContentDialog> _dialogQueue = new Queue<ContentDialog>();
        private static bool _isDialogShowing = false;

        private static bool _FirstStarup = false;
        private readonly NavigationManager _navigationManager = new();
        private int currentTeachStep = -1;

        private Point _dragStartPoint;
        private bool _isDragging = false;

        public MainWindow()
        {
            Prepare();

            App.AlreadyInit = true;
            

            InitializeComponent();
            //测试代码
            //Static.Theme.themeMode = ApplicationTheme.Dark;



            if (_FirstStarup)
            {
                StartAppreciations();

                
            }

            //TeachNextStep();

        }

        #region 弃用方案

        private bool _isClicking = false;
        private const double DragThreshold = 5; // 设置一个阈值来判断是否是拖动

        private void FloatButton_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isClicking = true; // 假设是点击
            _isDragging = false; // 不是拖动
            _dragStartPoint = e.GetPosition(DragCanvas);
            FloatButton.CaptureMouse();

            // 延迟一点时间，检查是否是拖动
            // 这里可以选择触发一个定时器来判断是否发生了拖动
        }

        private void FloatButton_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (!_isClicking && !_isDragging) return; // 如果已经不是点击，则不处理

            // 获取当前鼠标位置
            Point currentPoint = e.GetPosition(DragCanvas);

            // 计算鼠标移动的距离
            double offsetX = currentPoint.X - _dragStartPoint.X;
            double offsetY = currentPoint.Y - _dragStartPoint.Y;

            // 如果鼠标移动距离大于阈值，认为是拖动
            if (Math.Abs(offsetX) > DragThreshold || Math.Abs(offsetY) > DragThreshold)
            {
                _isDragging = true; // 开始拖动
                _isClicking = false; // 不再认为是点击

                // 计算新位置
                double newLeft = Canvas.GetLeft(FloatButton) + offsetX;
                double newTop = Canvas.GetTop(FloatButton) + offsetY;

                // 获取按钮的半径
                double buttonRadius = FloatButton.ActualWidth / 2;

                // 限制按钮在边界范围内（允许最多1/2半径移出边界）
                newLeft = Math.Max(-buttonRadius / 2, Math.Min(newLeft, DragCanvas.ActualWidth - buttonRadius * 1.5));
                newTop = Math.Max(-buttonRadius / 2, Math.Min(newTop, DragCanvas.ActualHeight - buttonRadius * 1.5));

                // 更新按钮位置
                Canvas.SetLeft(FloatButton, newLeft);
                Canvas.SetTop(FloatButton, newTop);

                // 更新起始点为当前位置
                _dragStartPoint = currentPoint;
            }
        }

        private void FloatButton_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_isClicking)
            {
                // 如果是点击操作（没有拖动），可以在这里处理点击事件
                LabAIChatWindow window = new LabAIChatWindow();
                window.Show();
            }

            _isDragging = false;
            _isClicking = false;
            FloatButton.ReleaseMouseCapture();
        }


        private void TeachNextStep()
        {
            if (TeachGrid.Visibility == Visibility.Hidden)
            {
                TeachGrid.Visibility = Visibility.Visible;
            }
            if (currentTeachStep + 1 >= Text.TeachingString.Count)
            {
                TeachSkip_Click(null, null);
            }
            currentTeachStep++;
            var item = Text.TeachingString.ElementAt(currentTeachStep);
            var itemKey = item.Key;
            var itemValue = item.Value;
            TitleTeach.Text = itemKey;
            ContentTeach.Text = itemValue;

            if (currentTeachStep + 1 >= Text.TeachingString.Count)
            {

                TeachNext.Content = "完成";
            }
        }

        #endregion

        private async void Prepare()
        {
            // 每次实例化都会对静态对象操作一次 这是不安全的
            if (App.AlreadyInit)
            {
                return;
            }

            //ExperimentCompleted experimentCompleted = new ExperimentCompleted("请你遵守以下规则\r\n1. 以下内容一律使用中文回复\r\n2. 如果你被要求输出一段文字，直接按原文本输出即可\r\n3. 如果你认为用户给定的内容不合理而无法分析，输出“无法总结本实验”，不要给出修复建议\r\n你是一个实验室软件数据采集总结 AI ，实验室软件名为“IOLab”，下面，你将被输入一段实验数据，请你按照以下结构展开分析\r\n1. 实验总结（包含实验成功之处与不足之处）\r\n2. 实验原理（通过实验数据指出实验印证的原理）\r\n3. 实验评分（给出一个分数并给出给分原因）\r\n实验数据由程序生成，如下所示：实验名称：新制氯水的变质速率探究\r\n仪器列表：\r\n二氧化碳传感器\r\n实验步骤：\r\n一、准备：准备新制氯水溶液、配套设备、烧杯\r\n二、取样：取 50mL 新制氯水溶液到烧杯中\r\n三、采集：放置探头到液面以下，采集24小时。然后用 AL 类插口创建\r\n四、分析：分析图象，讨论这段时间后是否达到化学平衡\r\n\r\n实验数据：\r\n表名：pH传感器 - 1 - 已归档\r\nY轴：pH\r\n值列表：\r\n0,9.067562,9.096051,9.081128,9.081128,9.08452,9.08045,9.075023,9.083164,9.07231,9.090625,9.126577,9.085198,9.077736,9.104191,9.094017,9.095373,9.086555,9.016008,9.091303,9.098086,9.093339,9.104871,9.216795,9.25953,9.401301,");
            //experimentCompleted.ShowAsync();

            // 读取设置
            ChangeThemeByIndex(Core.Properties.Settings.Default.ThemeSelection);
            new ThemeResources().AccentColor = (Color)ColorConverter.ConvertFromString(Core.Properties.Settings.Default.AccentColorHex);

            ReadAssets.ReadLocalServices(); // 必须第一个加载

            ReadAssets.ReadRecords();
            ReadAssets.ReadExperiments();

            // 初始化 以挂载事件
            InstanceBundle.ChartTables = new ObservableCollection<Core.Models.ChartTableModel>();

            // 检查设置是否有差异
            if (!IONetAPI.Servers.ContainsKey(Settings.Default.ServerIndex))
            {
                Settings.Default["ServerIndex"] = IONetAPI.Servers.Keys.ToList()[0]; //没有就直接用第0项
            }

            // 自动登录
            
            if (Settings.Default.UserToken != "" && Settings.Default.UserToken != null)
            {
                InstanceBundle.UsingAuth = new Network.Auth(Settings.Default.UserToken);
                var result = await InstanceBundle.UsingAuth.LoginViaTokenAsync();
                if (!result.Success)
                {
                    iNKORE.UI.WPF.Modern.Controls.MessageBox.Show("无法完成自动登录: " + result.Message + "\r\n请稍后在设置页重新登录", "IOLab 自动登录", MessageBoxButton.OK, MessageBoxImage.Information);
                    InstanceBundle.UsingAuth = null;
                }
            }


        }


        public const int WM_DEVICECHANGE = 0x219;//U盘插入后，OS的底层会自动检测到，然后向应用程序发送“硬件设备状态改变“的消息  
        public const int DBT_DEVICEREMOVECOMPLETE = 0x8004;  //一个设备或媒体片已被删除。
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_DEVICECHANGE)
            {
                ActionBundle.DoUpdateSerialList();
                switch (wParam.ToInt32())
                {
                    case DBT_DEVICEREMOVECOMPLETE:
                        // 检测到设备拔出
                        if (!InstanceBundle.IsConnected)
                        {
                            break;
                        }
                        // 验证是不是连接的串口
                        bool flag = true;
                        foreach (string item in new Scanner().ScanAllSerial())
                        {
                            if (item == InstanceBundle.connectTool._SerialPort.PortName)
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (!flag)
                        {
                            break;
                        }

                        var currentDialog = Application.Current.MainWindow?.Content as ContentDialog;

                        if (currentDialog != null)
                        {
                            // 关闭当前对话框
                            currentDialog.Hide();
                        }

                        new ManagersViewModel().OnUIDisconnectDevice(this);
                        InstanceBundle.IsConnected = false;
                        InstanceBundle.connectTool = null;
                        _navigationManager.ResetPage("NavigationViewItem_Remote");// 重载页面
                        NavigationView_Root.SelectedItem = NavigationViewItem_Remote;
                        Frame_Main.Navigate(new RemotePage());
                        break;
                    default:
                        break;
                }
            }
            return IntPtr.Zero;
        }

        private void StartAppreciations()
        {

            NavigationView_Root.Visibility = Visibility.Hidden;
            Frame_Secondary.Visibility = Visibility.Visible;
            Static.Theme.themeMode = ApplicationTheme.Dark;
            Cover cover = new Cover();
            Frame_Secondary.Navigate(cover);
        }


        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            BreakPageTask();
            if(sender.SelectedItem as NavigationViewItem == null)
            {
                return; // 不理会奇奇怪怪的对象
            }
            var page = _navigationManager.GetPage((NavigationViewItem)sender.SelectedItem);
            NavigationView_Root.Header = page.Title;
            Frame_Main.Content = page;
        }
        private void CommunicateWithAI_Click(object sender, RoutedEventArgs e)
        {
            LabAIChatWindow labAIChatWindow = new LabAIChatWindow();
            labAIChatWindow.Show();
        }


        // 打断页面行为
        public void BreakPageTask()
        {
            // 打断连接行为

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_FirstStarup)
            {
                return;
            }
            NavigationView_Root.SelectedItem = NavigationViewItem_Home;

            HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;//窗口过程
            if (hwndSource != null)
            {
                hwndSource.AddHook(new HwndSourceHook(WndProc));//挂钩
            }

        }


        /// <summary>
        /// 使用Static.Theme.themeMode = theme;调用而不是次方法
        /// </summary>
        /// <param name="theme"></param>
        public static void ChangeTheme(ApplicationTheme theme)
        {
            ThemeManager.Current.ApplicationTheme = theme;
        }

        public static void ChangeThemeByIndex(int index)
        {
            if (index == 0)
            {
                if (Util.ThemeMode.GetSystemDarkMode())
                {
                    Static.Theme.themeMode = ApplicationTheme.Dark;
                }
                else
                {
                    Static.Theme.themeMode = ApplicationTheme.Light;
                }
            }
            else if (index == 1)
            {
                Static.Theme.themeMode = ApplicationTheme.Light;
            }
            else if (index == 2)
            {
                Static.Theme.themeMode = ApplicationTheme.Dark;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            var collections = Application.Current.Windows;

            if (InstanceBundle.cmdProcess != null) //关闭cmd进程
            {
                try
                {
                    InstanceBundle.cmdProcess.Kill();
                }
                catch { }
            }

            foreach (Window window in collections)
            {
                if (window != this)
                    window.Close();
            }

            //base.OnClosed(e);
        }


        #region 弃用方案 教学
        private void TeachNext_Click(object sender, RoutedEventArgs e)
        {
            TeachNextStep();
        }

        private void TeachSkip_Click(object sender, RoutedEventArgs e)
        {
            TeachGrid.Visibility = Visibility.Hidden;
            currentTeachStep = -1;
            TeachNext.Content = "下一步";
        }

        private void TeachPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (currentTeachStep - 1 < 0)
            {
                return;
            }

            currentTeachStep--;

            var item = Text.TeachingString.ElementAt(currentTeachStep);
            var itemKey = item.Key;
            var itemValue = item.Value;
            TitleTeach.Text = itemKey;
            ContentTeach.Text = itemValue;
            TeachNext.Content = "下一步";

        }

        #endregion

        public async void CreateDialog(string title, string content)
        {
            ContentDialog dialog = new ContentDialog()
            {
                Title = title,
                Content = content,
                PrimaryButtonText = "好",
                DefaultButton = ContentDialogButton.Primary,
            };

            // 将新对话框添加到队列
            _dialogQueue.Enqueue(dialog);

            // 如果当前没有对话框显示，则开始显示
            if (!_isDialogShowing)
            {
                await ShowNextDialogAsync();
            }
        }

        private async Task ShowNextDialogAsync()
        {
            // 如果队列中还有对话框，继续显示
            if (_dialogQueue.Count > 0)
            {
                _isDialogShowing = true;
                ContentDialog currentDialog = _dialogQueue.Dequeue();
                currentDialog.Closed += async (sender, args) =>
                {
                    // 在当前对话框关闭时，显示下一个对话框
                    await ShowNextDialogAsync();
                };
                await currentDialog.ShowAsync();
            }
            else
            {
                _isDialogShowing = false;
            }
        }


        public void AlertLogin(string title = "请登录以完成操作")
        {
            if (String.IsNullOrEmpty(Settings.Default.UserToken) && InstanceBundle.UsingAuth == null)
            {
                CreateDialog(title, "未登录");
            }
            else if (!String.IsNullOrEmpty(Settings.Default.UserToken) && InstanceBundle.UsingAuth != null)
            {
                CreateDialog(title, "登录无效");
            }
            else if (!String.IsNullOrEmpty(Settings.Default.UserToken) && InstanceBundle.UsingAuth == null)
            {
                CreateDialog(title, "请等待自动登录，或者自动登录已经失败");
            }
        }

        public void ShowNotification(string title, string content)
        {
            // 创建通知内容
            var toastContent = new ToastContentBuilder()
                .AddText(title)
                .AddText(content)
                .GetToastContent();

            // 将内容转换为XML格式
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(toastContent.GetContent());

            // 创建并显示通知
            var toast = new ToastNotification(xmlDocument);
            ToastNotificationManager.CreateToastNotifier("YourAppId").Show(toast);
        }

        
    }


}
