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
using System.IO.Ports;
using Newtonsoft.Json.Linq;
using System.Windows;

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

        // 通知类
        public class Notification
        {
            public string Title { get; set; }
            public string Message { get; set; }
            public bool IsOpen { get; set; } = true; // 默认为打开状态
            public InfoBarSeverity Severity { get; set; } // 新增Severity属性
        }

        public ObservableCollection<Notification> Notifications { get; } = new ObservableCollection<Notification>();

        // 获取主题色并转换成 hex 再转换成 SolidColorBrush
        public SolidColorBrush AccentColor { get => new SolidColorBrush((Color)ColorConverter.ConvertFromString(new ThemeResources().AccentColor.ToString())); set { } }

        // RelayCommand实现
        public ICommand SelectChartCommand { get; set; }
        public ICommand RemoveSelectedChartsCommand { get; set; }
        public ICommand AddNewChartCommand { get; set; }
        public ICommand SaveRecordsCommand { get; set; }
        public ICommand FollowLatestCommand { get; set; }
        public ICommand MouseDownOnChart { get; set; }

        public ICommand FreezeChartsCommand { get; set; }
        public ICommand ConfigChartCommand { get; set; }


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

            // 订阅 RelayCommand
            SelectChartCommand = new RelayCommand(OnChartSelected);
            RemoveSelectedChartsCommand = new RelayCommand(RemoveSelectedCharts);
            AddNewChartCommand = new RelayCommand(AddNewChart);
            SaveRecordsCommand = new RelayCommand(SaveMyRecords);
            FollowLatestCommand = new RelayCommand(FollowLatest);
            MouseDownOnChart = new RelayCommand(OnMouseDragChart);
            FreezeChartsCommand = new RelayCommand(FreezeCharts);
            ConfigChartCommand = new RelayCommand(ConfigChart);

            // 订阅事件
            InstanceBundle.IsConnectedChanged += OnIsConnectedChanged;
            InstanceBundle.ChartTablesUpdated += OnChartsChanged;
            // 订阅外部暂停事件
            ActionBundle.PauseOrStartRecord += OnPauseChangedAction;
            ActionBundle.NewestSerialCommand += ProcessJSONNotification;
        }

        public event EventHandler<ChartSelectionChangedEventArgs> ChartSelectionChanged;
        public event EventHandler<MultipleChartsSelectedEventArgs> MultipleChartsSelected;
        public event EventHandler NoChartSelected;

        // 归档选中表
        private async void FreezeCharts(object sender)
        {
            var selectedCharts = MappedChartTables.Where(c => c.IsSelected).ToList();
            // TODO 修改文案
            var result = await GetSureToDialog(Text.SureToFreeze, Text.SureToFreezeExplain).ShowAsync();
            if (result != ContentDialogResult.Primary)
            {
                return;
            }    

            foreach (var chart in selectedCharts)
            {
                if (chart.isFrozen)
                    continue;
                chart.IsSelected = false;
                chart.isFrozen = true;
                
                chart.Name = chart.Name + Text.FrozenChart;
                OnPropertyChanged(nameof(MappedChartTables));

                // TODO 表删除时销毁监听
            }
            CreateNotification("归档表", $"归档了共 {selectedCharts.Count()} 个选中的表", InfoBarSeverity.Success);
            System.Diagnostics.Debug.WriteLine($"冻结了共 {selectedCharts.Count()} 个选中的表");
        }


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
                if (chart.isFrozen && InstanceBundle.UsingExperiment != null)
                {
                    var result = await GetSureToDeleteDialog(chart.Name).ShowAsync();
                    if (result != ContentDialogResult.Primary)
                    {
                        continue;
                    }
                }
                chart.IsSelected = false;
                chart.isFrozen = true; // 标记为归档，以直接取消占用
                OnChartSelected(chart);
                MappedChartTables.Remove(chart);
                // TODO 表删除时销毁监听
            }
            System.Diagnostics.Debug.WriteLine($"移除了共 {selectedCharts.Count()} 个选中的表");
        }

        //编辑表按钮触发
        private async void ConfigChart(object sender)
        {
            ChartConfig config = new ChartConfig(SelectedChart);
            await config.ShowAsync();
        }

        // 保存所有表
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

            
            // 显示等待弹窗
            WaitSerial wait = new WaitSerial();
            wait.ShowAsync(); // 无需等待
            try
            {
                var hooker = new SerialCommandHooker();

                // 发送消息并等待 ID 为 10010 的响应
                var (responseMsg, isSuccess) = await hooker.SendMessageAsync($"#Enable#{add.PortID}#{add.Service.Id}", 10010);
                if (!isSuccess)
                {
                    wait.Hide();
                    return;
                }
            }
            catch (TimeoutException ex)
            {
                // 处理超时异常
                wait.Hide();
                CreateNotification("创建表", "操作超时，请重试", InfoBarSeverity.Error);
                // TODO 发送撤销操作指令
                return;
            }
            catch (Exception ex)
            {
                wait.Hide();
                CreateNotification("创建表", ex.Message, InfoBarSeverity.Error);
                return;
            }
            wait.Hide();
            
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

        // 获取询问是否继续删除
        // 此方法可以被替代
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

        // 获取询问继续
        private ContentDialog GetSureToDialog(string title,string content)
        {
            ContentDialog dialog = new ContentDialog()
            {
                Title = title,
                Content = content,
                PrimaryButtonText = "继续",
                CloseButtonText = "取消",
                DefaultButton = ContentDialogButton.Primary,
            };
            return dialog;
        }

        // 处理并添加JSON通知
        public void ProcessJSONNotification(JObject obj)
        {
            if(Application.Current == null)
            {
                return;
            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (obj["command"].ToString() != "Message")
                {
                    return;
                }
                InfoBarSeverity severity;
                string type = obj["type"].ToString();
                if (type == "error")
                {
                    severity = InfoBarSeverity.Error;
                }
                else if (type == "callback")
                {
                    severity = InfoBarSeverity.Informational;
                }
                else
                {
                    severity = InfoBarSeverity.Informational;
                }
                CreateNotification(obj["title"].ToString(), obj["msg"].ToString(), severity);
            });
        }

        // 添加通知
        public void CreateNotification(string title, string message, InfoBarSeverity severity)
        {
            Notifications.Add(new Notification
            {
                Title = title,
                Message = message,
                Severity = severity // 设置通知类型
            });
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

}
