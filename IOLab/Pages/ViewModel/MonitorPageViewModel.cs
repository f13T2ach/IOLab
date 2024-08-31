/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * MonitorPageViewModel.cs is part of the IOLab project.
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
using IOLab.Commands;
using IOLab.Core.Assets;
using IOLab.Core.Models;
using IOLab.Core.Generic;
using IOLab.Pages.Event.MonitorPages;
using IOLab.Windows.Dialog;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;

namespace IOLab.Pages.ViewModel
{
    class MonitorPageViewModel : INotifyPropertyChanged
    {
        // Binding用 表对象列表映射
        public ObservableCollection<ChartTableModel> MappedChartTables
        {
            get
            {
                return InstanceBundle.ChartTables;
            }
            set
            {
                InstanceBundle.ChartTables = value;
                OnPropertyChanged(nameof(MappedChartTables));
            }
        }

        // 获取主题色并转换成 hex 再转换成 SolidColorBrush
        public SolidColorBrush AccentColor { get => new SolidColorBrush((Color)ColorConverter.ConvertFromString(new ThemeResources().AccentColor.ToString())); set { } }

        // RelayCommand实现
        public ICommand SelectChartCommand { get; set; }
        public ICommand RemoveSelectedChartsCommand { get; set; }
        public ICommand AddNewChartCommand { get; set; }
        public ICommand SaveRecordsCommand { get; set; }
        public ICommand FollowLatestCommand { get; set; }
        public ICommand MouseDownOnChart { get; set; }



        // 数据Binding
        public string DeviceNameText { get => InstanceBundle.IsConnected ? InstanceBundle.connectTool.Device.DeviceName : Text.ConnectToManagePins; private set { } }

        public bool ManageButtonEnable { get => InstanceBundle.IsConnected; private set { } }
        // 选中事件
        private ChartTableModel _selectedChart;
        public ChartTableModel? SelectedChart
        {
            get => _selectedChart;
            set
            {
                if (_selectedChart != value)
                {
                    _selectedChart = value;
                    OnPropertyChanged(nameof(SelectedChart));
                    System.Diagnostics.Debug.WriteLine($"表 {_selectedChart?.Id} 状态 {_selectedChart?.IsSelected}");
                }
            }
        }

        private bool _isPauseRecord;
        public bool IsPauseRecord
        {
            get => _isPauseRecord;
            set
            {
                if (_isPauseRecord != value)
                {
                    _isPauseRecord = value;
                    ActionBundle.DoPauseOrStart(value);

                    OnPropertyChanged(nameof(IsPauseRecord));
                }
            }
        }

        public MonitorPageViewModel()
        {
            MappedChartTables ??= new ObservableCollection<ChartTableModel>();


            SelectChartCommand = new RelayCommand(OnChartSelected);
            RemoveSelectedChartsCommand = new RelayCommand(RemoveSelectedCharts);
            AddNewChartCommand = new RelayCommand(AddNewChart);
            SaveRecordsCommand = new RelayCommand(SaveMyRecords);
            FollowLatestCommand = new RelayCommand(FollowLatest);
            MouseDownOnChart = new RelayCommand(OnMouseDragChart);

            // 订阅事件
            InstanceBundle.IsConnectedChanged += OnIsConnectedChanged;
            InstanceBundle.ChartTablesUpdated += OnChartsChanged;
            // 订阅外部暂停事件
            ActionBundle.PauseOrStartRecord += OnPauseChangedAction;
        }

        public event EventHandler<ChartSelectionChangedEventArgs> ChartSelectionChanged;
        public event EventHandler<MultipleChartsSelectedEventArgs> MultipleChartsSelected;
        public event EventHandler NoChartSelected;

        // 更新选中表
        private void OnChartSelected(object parameter)
        {
            if (parameter is ChartTableModel chart)
            {

                // 选中表
                if (chart.IsSelected)
                {
                    // 获取已选数量
                    int selectedCount = MappedChartTables.Count(c => c.IsSelected);
                    if (selectedCount > 2) // 两个为选中上限
                    {
                        System.Diagnostics.Debug.WriteLine($"触发选中限制");

                    }

                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"选中状态更新 ID = {chart.Id} 的表取消选中");
                }

                // 更新选中情况
                if (chart.IsSelected)
                {
                    SelectedChart = chart;
                }
                else
                {
                    if (SelectedChart == chart)
                    {
                        SelectedChart = null;
                    }
                }

                // 触发选中事件
                int newSelectedCount = MappedChartTables.Count(c => c.IsSelected);
                if (newSelectedCount == 1) //选中一个时
                {
                    ChartSelectionChanged?.Invoke(this, new ChartSelectionChangedEventArgs(chart));
                }
                else if (newSelectedCount >= 2)
                {
                    var selectedCharts = MappedChartTables.Where(c => c.IsSelected).ToArray();
                    MultipleChartsSelected?.Invoke(this, new MultipleChartsSelectedEventArgs(selectedCharts));
                }
                if (newSelectedCount == 0)
                {
                    NoChartSelected?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        // 删除选中表
        private async void RemoveSelectedCharts(object sender)
        {
            var selectedCharts = MappedChartTables.Where(c => c.IsSelected).ToList();
            foreach (var chart in selectedCharts)
            {
                // 如果用户在实验期间打算删除实验记录，就警告他（不过应该没人这么做吧...）
                if(chart.isFrozen&&InstanceBundle.UsingExperiment != null)
                {
                    var result = await GetSureToDeleteDialog(chart.Name).ShowAsync();
                    if(result != ContentDialogResult.Primary)
                    {
                        continue;
                    }
                }
                chart.IsSelected = false;
                OnChartSelected(chart);
                MappedChartTables.Remove(chart);
                // TODO 表删除时销毁监听
            }
            System.Diagnostics.Debug.WriteLine($"移除了共 {selectedCharts.Count()} 个选中的表");
        }

        // 保存表
        private async void SaveMyRecords(object sender)
        {
            SaveRecords saveRecords;
            if (MappedChartTables.Count == 0)
            {
                saveRecords = new SaveRecords(0);
                await saveRecords.ShowAsync();
                return;
            }
            saveRecords = new SaveRecords(1);
            var result = await saveRecords.ShowAsync();
        }
        // 新增一个表
        private async void AddNewChart(object sender)
        {
            AddChart add = new AddChart();
            var result = await add.ShowAsync();
            if (result != ContentDialogResult.Primary)
            {
                return;
            }

            // 防止突然拔出
            if (!InstanceBundle.IsConnected)
            {
                return;
            }

            MappedChartTables.Add(new ChartTableModel(add.Service.YRow, add.ChartName, (new ThemeResources().AccentColor.ToString()))
            {
                Id = add.ChartID,
                XRow = "秒",
                PortId = add.PortID,
                ChartServicer = add.Service,

            });

        }

        //连接状态变更
        private void OnIsConnectedChanged(bool isConnected)
        {
            OnPropertyChanged(nameof(DeviceNameText));  // 通知DeviceNameText属性的更改
            OnPropertyChanged(nameof(ManageButtonEnable));
        }

        // 图表跟随最新点
        private void FollowLatest(object parameter)
        {
            int id = (int)parameter;
            foreach (ChartTableModel model in InstanceBundle.ChartTables)
            {
                if (model.Id == id)
                {
                    model.isDragged = false;
                    model.AdjustToLatestPoint();
                    break;
                }
            }
        }

        // 图表被拖动（放弃跟随最新点）
        private void OnMouseDragChart(object parameter)
        {
            int id = (int)parameter;
            foreach (ChartTableModel model in InstanceBundle.ChartTables)
            {
                if (model.Id == id)
                {
                    model.isDragged = true;
                    break;
                }
            }
        }

        // 外部暂停事件
        private void OnPauseChangedAction(bool isPause)
        {
            IsPauseRecord = isPause;
        }

        // 外部修改图表事件
        private void OnChartsChanged()
        {
            OnPropertyChanged(nameof(MappedChartTables));
        }

        // 获取询问是否继续删除窗口
        private ContentDialog GetSureToDeleteDialog(string name)
        {
            ContentDialog dialog = new ContentDialog()
            {
                Title = Text.SureToDelete,
                Content = $"{Text.SureToDeleteExplain_Part1}{name}{Text.SureToDeleteExplain_Part2}",
                PrimaryButtonText = "删除",
                CloseButtonText = "取消",
                DefaultButton = ContentDialogButton.Primary,
            };
            return dialog;
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

}
