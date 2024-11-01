/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * SettingPage.xaml.cs is part of the IOLab project.
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
using IOLab.Core.Generic;
using IOLab.Core.Properties;
using IOLab.Models.Related;
using IOLab.Network.API;
using IOLab.Static;
using System;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Page = iNKORE.UI.WPF.Modern.Controls.Page;


namespace IOLab.Pages
{


    /// <summary>
    /// Interaction logic for SettingPage.xaml
    /// </summary>
    public partial class SettingPage : Page
    {

        private int TotalClick = 0;
        private bool isLoaded = false;
        public SettingPage()
        {
            InitializeComponent();
            isLoaded = true;
            LoadSettings();
            LoadUserInfomation();

            InitializeColorPicker();
            InitializeServerPicker();

            TotalClick = 0;

            // 绑定Actions
            ActionBundle.LoginStat += OnLoginStatChanged;
        }

        private void LoadSettings()
        {
            ComboBox_ThemeMode.SelectedIndex = Core.Properties.Settings.Default.ThemeSelection;
            

        }

        private void LoadUserInfomation(bool stat = true)
        {
            // 优先判断传入的stat
            if(stat == false || InstanceBundle.UsingAuth == null)
            {
                IONet_UserName.Text = "未登录";
                IONet_UserExperience.Text = "0";
                if (Settings.Default.UserToken != "" && Settings.Default.UserToken != null)
                {
                    IONet_UserEmail.Text = "IOLab 自动登录失败，点「重新登录」可重试";
                }
                else
                {
                    IONet_UserEmail.Text = "登录以访问 IOLab Community";
                }
                UserPanel_Offline.Visibility = Visibility.Visible;
                UserPanel_Online.Visibility = Visibility.Collapsed;
                return;
            }


            IONet_UserName.Text = InstanceBundle.UsingAuth.Username;
            IONet_UserExperience.Text = InstanceBundle.UsingAuth.Experience.ToString() ;
            IONet_UserEmail.Text = InstanceBundle.UsingAuth.Email;
            UserPanel_Offline.Visibility = Visibility.Collapsed;
            UserPanel_Online.Visibility = Visibility.Visible;
        }

        private void OnLoginStatChanged(bool stat)
        {

            LoadUserInfomation(stat);
        }

        private void InitializeServerPicker() {

            ServerPickerBox.Items.Clear();
            foreach(var name in IONetAPI.Servers.Keys)
            {
                ServerPickerBox.Items.Add(name);
            }
            ServerPickerBox.SelectedItem = Settings.Default.ServerIndex;
        }

        // 颜色选取器载入
        // 试用功能 未封装控件
        private void InitializeColorPicker()
        {

            var colors = new List<ColorItem>();
            // 遍历加入颜色
            foreach (string str in Theme.coloursHex)
            {
                colors.Add(new ColorItem { Color = new SolidColorBrush((Color)ColorConverter.ConvertFromString(str)) });
            }

            ColorPickerComboBox.ItemsSource = colors;

            Color? setColor = new ThemeResources().AccentColor;
            // 遍历找到设置颜色
            for (int index = 0; index < colors.Count; index++)
            {
                ColorItem item = colors[index];
                if (item.Color.Color == setColor)
                {
                    ColorPickerComboBox.SelectedIndex = index;
                    break;
                }
            }

        }
        private void ColorPickerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ColorPickerComboBox.SelectedItem is ColorItem selectedColor)
            {
                new ThemeResources().AccentColor = selectedColor.Color.Color;
                Settings.Default["AccentColorHex"] = selectedColor.Color.Color.ToString();
                Settings.Default.Save();
            }
        }


        // 主题改变
        private void ComboBox_ThemeMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isLoaded)
            {
                return;
            }
            int index = ComboBox_ThemeMode.SelectedIndex;
            MainWindow.ChangeThemeByIndex(index);

            Settings.Default["ThemeSelection"] = index;
            Settings.Default.Save();
        }

        private void EntryOfGame_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TotalClick++;
            if (TotalClick == 6)
            {
                TotalClick = 0;
            }
        }
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is IDisposable disposableContext)
            {
                disposableContext.Dispose();
            }
        }

        private void ServerPickerBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Settings.Default["ServerIndex"] = ServerPickerBox.SelectedItem;
            Settings.Default.Save();

        }

        private async void CeaseSession_Click(object sender, RoutedEventArgs e)
        {
            if(InstanceBundle.UsingAuth == null)
            {
                OnLoginStatChanged(false);
                return;
            }

            Settings.Default["UserToken"] = "";
            Settings.Default.Save();


            await InstanceBundle.UsingAuth.CeaseSessionAsync();
            InstanceBundle.UsingAuth = null;
            
        }
    }
}
