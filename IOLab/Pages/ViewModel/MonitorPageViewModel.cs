using iNKORE.UI.WPF.Modern;
using iNKORE.UI.WPF.Modern.Controls;
using IOLab.Commands;
using IOLab.Core.Models;
using IOLab.Core.Serial;
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

        public MonitorPageViewModel()
        {
            MappedChartTables ??= new ObservableCollection<ChartTableModel>();


            SelectChartCommand = new RelayCommand(OnChartSelected);
            RemoveSelectedChartsCommand = new RelayCommand(RemoveSelectedCharts);
            AddNewChartCommand = new RelayCommand(AddNewChart);
            SaveRecordsCommand = new RelayCommand(SaveMyRecords);
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
        private void RemoveSelectedCharts(object sender)
        {
            var selectedCharts = MappedChartTables.Where(c => c.IsSelected).ToList();
            foreach (var chart in selectedCharts)
            {
                chart.IsSelected = false;
                OnChartSelected(chart);
                MappedChartTables.Remove(chart);
                // 销毁监听线程
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
            MappedChartTables.Add(new ChartTableModel(add.Service.YRow, add.ChartName, (new ThemeResources().AccentColor.ToString()))
            {
                Id = add.ChartID,
                XRow = "秒",
                PortId = add.PortID,
                ChartSevice = add.Service,

            });
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

}
