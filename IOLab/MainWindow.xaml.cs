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

﻿using iNKORE.UI.WPF.Modern;
using iNKORE.UI.WPF.Modern.Controls;
using IOLab.Core.Generic;
using IOLab.Core.Serial;
using IOLab.Pages.Appreciations;
using System;
using System.Windows;
using System.Windows.Media;
using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace IOLab
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static bool _FirstStarup = false;

        public MainWindow()
        {
            Prepare();

            App.AlreadyInit = true;
            InitializeComponent();
            //测试代码
            //Static.Theme.themeMode = ApplicationTheme.Dark;
            Util.StaticModel.DeviceName.TextContent = "瑞克五代";

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

            var item = sender.SelectedItem;
            Page? page = null;

            if (item == NavigationViewItem_Home)
            {
                Pages.HomePage Page_Home = new Pages.HomePage();
                page = Page_Home;
            }
            else if (item == NavigationViewItem_Remote)
            {
                Pages.RemotePage Page_Remote = new Pages.RemotePage();
                page = Page_Remote;
            }
            else if (item == NavigationViewItem_Monitor)
            {
                Pages.MonitorPage Page_Monitor = new Pages.MonitorPage();
                page = Page_Monitor;
            }
            else if (item == NavigationViewItem_Setting)
            {
                Pages.SettingPage Page_Setting = new Pages.SettingPage();
                page = Page_Setting;
            }
            else if (item == NavigationViewItem_About)
            {
                Pages.AboutPage Page_About = new Pages.AboutPage();
                page = Page_About;
            }

            if (page != null)
            {
                NavigationView_Root.Header = page.Title;
                Frame_Main.Navigate(page);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_FirstStarup)
            {
                return;
            }
            NavigationView_Root.SelectedItem = NavigationViewItem_Home;


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
