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
using IOLab.Core.Generic;
using IOLab.Core.Serial;
using IOLab.Pages;
using IOLab.Pages.Appreciations;
using IOLab.Pages.Remote.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
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
                    _ => throw new InvalidOperationException("Unknown navigation item.")
                };
                _pageCache[pageKey] = page;
            }
            return page;
        }

    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static bool _FirstStarup = false;
        private readonly NavigationManager _navigationManager = new();

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
        }

        private void Prepare()
        {
            // 每次实例化都会对静态对象操作一次 这是不安全的
            if (App.AlreadyInit)
            {
                return;
            }

            // 读取设置
            ChangeThemeByIndex(Properties.Settings.Default.ThemeSelection);
            new ThemeResources().AccentColor = (Color)ColorConverter.ConvertFromString(Properties.Settings.Default.AccentColorHex);

            ReadAssets.ReadLocalServices(); // 必须第一个加载
            ReadAssets.ReadRecords();
            ReadAssets.ReadExperiments();


        }

        public const int WM_DEVICECHANGE = 0x219;//U盘插入后，OS的底层会自动检测到，然后向应用程序发送“硬件设备状态改变“的消息  
        public const int DBT_DEVICEREMOVECOMPLETE = 0x8004;  //一个设备或媒体片已被删除。
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_DEVICECHANGE)
            {
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
            var page = _navigationManager.GetPage((NavigationViewItem)sender.SelectedItem);
            NavigationView_Root.Header = page.Title;
            Frame_Main.Content = page;
        }

        // 打断页面行为
        public void BreakPageTask()
        {
            // 打断连接行为
            if (InstanceBundle.connectTool != null)
            {
                if (!InstanceBundle.IsConnected)
                {
                    InstanceBundle.connectTool.CloseComport();
                    InstanceBundle.connectTool = null;
                }
            }
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

            foreach (Window window in collections)
            {
                if (window != this)
                    window.Close();
            }

            //base.OnClosed(e);
        }





    }


}
