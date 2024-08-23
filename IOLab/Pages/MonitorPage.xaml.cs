/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * MonitorPage.xaml.cs is part of the IOLab project.
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

﻿using IOLab.Pages.Event.MonitorPages;
using IOLab.Pages.ViewModel;
using IOLab.Windows.Dialog;
using System;
using System.Linq;
using System.Windows;
using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace IOLab.Pages
{
    /// <summary>
    /// Interaction logic for MonitorPage.xaml
    /// </summary>
    public partial class MonitorPage : Page
    {
        private MonitorPageViewModel viewModel;
        public MonitorPage()
        {
            InitializeComponent();
            viewModel = new MonitorPageViewModel();
            // 绑定事件
            viewModel.ChartSelectionChanged += OnChartSelectionChanged;
            viewModel.MultipleChartsSelected += OnMultipleChartSelected;
            viewModel.NoChartSelected += OnNoChartSelected;

            this.DataContext = viewModel;
        }

        #region 按钮响应
        private async void ManagePins_Click(object sender, RoutedEventArgs e)
        {
            PortDefine portDefine = new PortDefine();
            var result = await portDefine.ShowAsync();
        }
        #endregion

        #region MVVM回调

        private void OnChartSelectionChanged(object sender, ChartSelectionChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"单个表选中，ID = {e.SelectedChart.Id}");
            SingleChartOperation();
        }

        private void OnMultipleChartSelected(object sender, MultipleChartsSelectedEventArgs e)
        {
            if (e.SelectedCharts.Count() == 2)
            {
                System.Diagnostics.Debug.WriteLine($"两个表选中，ID = {e.SelectedCharts[0].Id}，ID1 = {e.SelectedCharts[1].Id}");
                DoubleChartOperation();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"共 {e.SelectedCharts.Count()} 个表被选中");
                MultipleChartOperation();
            }
        }

        private void OnNoChartSelected(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"没有选中的表");
            NoChartOperation();
        }
        #endregion

        #region 显示操作
        // 单表操作
        private void SingleChartOperation()
        {
            Tool_DeleteCharts.IsEnabled = true;
            Tool_EditChart.IsEnabled = true;
            Tool_MergeCharts.IsEnabled = false;
        }

        // 双表操作
        private void DoubleChartOperation()
        {
            Tool_DeleteCharts.IsEnabled = true;
            Tool_EditChart.IsEnabled = false;
            Tool_MergeCharts.IsEnabled = true;
        }

        // 批量操作
        private void MultipleChartOperation()
        {
            Tool_DeleteCharts.IsEnabled = true;
            Tool_EditChart.IsEnabled = false;
            Tool_MergeCharts.IsEnabled = false;
        }

        // 无可用操作
        private void NoChartOperation()
        {
            Tool_DeleteCharts.IsEnabled = false;
            Tool_EditChart.IsEnabled = false;
            Tool_MergeCharts.IsEnabled = false;
        }
        #endregion

    }
}
