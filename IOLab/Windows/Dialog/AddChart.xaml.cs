/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * AddChart.xaml.cs is part of the IOLab project.
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

﻿using iNKORE.UI.WPF.Modern.Controls;
using IOLab.Core.Config;
using IOLab.Core.Models;
using IOLab.Core.Serial;
using IOLab.Windows.Dialog.ViewModel;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace IOLab.Windows.Dialog
{
    /// <summary>
    /// AddChart.xaml 的交互逻辑
    /// </summary>
    public partial class AddChart : ContentDialog
    {
        AddChartViewModel viewModel;
        bool IsValidPortSelected = false;

        public string PortID { get; private set; }
        public MonitorService Service { get; private set; }// 采集方案
        public string ChartName { get; private set; }
        public int ChartID { get; private set; }
        public AddChart()
        {
            InitializeComponent();
            viewModel = new AddChartViewModel();
            this.DataContext = viewModel;

            InitContent(); //初始化内容
            CheckUserContents();


        }

        private void InitContent()
        {
            // 获取表ID最大值
            int maxID = 0;
            foreach (ChartTableModel chart in InstanceBundle.ChartTables)
            {
                if (chart.Id > maxID)
                {
                    maxID = chart.Id;
                }
            }
            maxID = maxID == 0 ? 1 : ++maxID;

            chartIDInput.Value = maxID;


        }

        // 此处与 PortDefine 重复冗余
        bool isEditedFlag = false;
        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            // 拒绝用户修改
            ToggleButton button = (ToggleButton)sender;
            if (isEditedFlag)
            {
                return; // 防止自调用
            }
            isEditedFlag = true;

            button.IsChecked = !button.IsChecked;

            isEditedFlag = false;

            // 重复冗余结束

            // 重建combobox
            UpdateComboBox(sender);

            viewModel.SelectedPortID = button.Content.ToString();

            if (button.IsChecked == true)
            {
                IsValidPortSelected = false;
            }
            else
            {

                IsValidPortSelected = true;
            }
            CheckUserContents();
        }

        public void UpdateComboBox(object sender)
        {
            ToggleButton? button = sender as ToggleButton;
            string name = button.Content.ToString();
            string type = name.Contains("_") ? name.Split("_")[0] : name;


            List<MonitorService> services = new List<MonitorService>();
            foreach (MonitorService item in Bundles.LocalMonitorService)
            {
                if (type == item.PortType)
                {
                    services.Add(item);
                }
            }
            ServiceComboBox.ItemsSource = services;
        }
        private void chartIDInput_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            CheckUserContents();
        }

        private void CheckUserContents()
        {
            if (chartIDInput.Value == 99)
            {
                TipFor99.Visibility = Visibility.Visible;
            }
            // 检查选择端口是否有效
            if (!IsValidPortSelected)
            {
                IsPrimaryButtonEnabled = false;
                return;
            }

            // 检查是否选用模板了
            MonitorService service = (MonitorService)ServiceComboBox.SelectedItem;
            if (service == null)
            {
                IsPrimaryButtonEnabled = false;
                return;
            }

            int count = 0;
            foreach (ChartTableModel chart in InstanceBundle.ChartTables)
            {
                if (chart.Id == chartIDInput.Value)
                {
                    IsPrimaryButtonEnabled = false;
                    return;
                }
                count++;
            }
            if (count >= InternalConfig.MaxChartNumber)
            {
                IsPrimaryButtonEnabled = false;
                return;
            }

            IsPrimaryButtonEnabled = true;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            PortID = viewModel.SelectedPortID;
            ChartID = (int)chartIDInput.Value;
            Service = (MonitorService)ServiceComboBox.SelectedItem;
            if (chartNameInput.Text == "")
            {
                ChartName = $"{Service.Name}";
            }
            else
            {
                ChartName = $"{chartNameInput.Text}";
            }
        }

        private void ServiceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckUserContents();
        }
    }
}
