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

﻿using iNKORE.UI.WPF.Modern;
using IOLab.Models.Related;
using IOLab.Static;
using System.Collections.Generic;
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

            InitializeColorPicker();
            TotalClick = 0;
        }

        private void LoadSettings()
        {
            ComboBox_ThemeMode.SelectedIndex = Properties.Settings.Default.ThemeSelection;

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
                Properties.Settings.Default["AccentColorHex"] = selectedColor.Color.Color.ToString();
                Properties.Settings.Default.Save();
            }
        }

        // 连接社区
        private async void ToggleSwitch_UseOnlineCommunity_Toggled(object sender, RoutedEventArgs e)
        {
            if (ToggleSwitch_UseOnlineCommunity.IsOn == false)
            {
                return; // 下面的语句会重新触发此方法
            }
            ToggleSwitch_UseOnlineCommunity.IsOn = false;
            Windows.Dialog.ConnectCommunity cc = new Windows.Dialog.ConnectCommunity();
            var result = await cc.ShowAsync();
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

            Properties.Settings.Default["ThemeSelection"] = index;
            Properties.Settings.Default.Save();
        }

        private void EntryOfGame_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TotalClick++;
            if (TotalClick == 6)
            {
                TotalClick = 0;
            }
        }


    }
}
