using IOLab.Commands;
using IOLab.Models.Related;
using IOLab.Pages.Event.MonitorPages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IOLab.Pages.ViewModel
{
    class MonitorPageViewModel : INotifyPropertyChanged
    {
        // 表对象列表
        public static ObservableCollection<ChartTableModel> ChartTables { get; set; }
        
        // RelayCommand实现
        public ICommand SelectChartCommand { get; set; }
        public ICommand RemoveSelectedChartsCommand { get; set; }

        public ICommand AddNewChartCommand { get; set; }

        // 选中事件
        private ChartTableModel _selectedChart;
        public ChartTableModel SelectedChart
        {
            get => _selectedChart;
            set
            {
                if (_selectedChart != value)
                {
                    _selectedChart = value;
                    OnPropertyChanged();
                    System.Diagnostics.Debug.WriteLine($"表 {_selectedChart?.Id} 状态 {_selectedChart?.IsSelected}");
                }
            }
        }

        public MonitorPageViewModel()
        {
            ChartTables = new ObservableCollection<ChartTableModel>();
            for (int i = 1; i <= 4; i++)
            {
                ChartTables.Add(new ChartTableModel { Id = i });
            }

            SelectChartCommand = new RelayCommand(OnChartSelected);
            RemoveSelectedChartsCommand = new RelayCommand(RemoveSelectedCharts);
            AddNewChartCommand = new RelayCommand(AddNewChart);
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
                    int selectedCount = ChartTables.Count(c => c.IsSelected);
                    if (selectedCount > 2) // 两个为选中上限
                    {
                        System.Diagnostics.Debug.WriteLine($"触发选中限制");

                    }

                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"选中状态更新 ID = {chart.Id}的表取消选中");
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
                int newSelectedCount = ChartTables.Count(c => c.IsSelected);
                if (newSelectedCount == 1) //选中一个时
                {
                    ChartSelectionChanged?.Invoke(this, new ChartSelectionChangedEventArgs(chart));
                }
                else if (newSelectedCount >= 2)
                {
                    var selectedCharts = ChartTables.Where(c => c.IsSelected).ToArray();
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
            var selectedCharts = ChartTables.Where(c => c.IsSelected).ToList();
            foreach (var chart in selectedCharts)
            {
                chart.IsSelected = false;
                OnChartSelected(chart);
                ChartTables.Remove(chart);
                // 销毁监听线程
            }
            System.Diagnostics.Debug.WriteLine($"移除了共 {selectedCharts.Count()} 个选中的表");
        }

        // 新增一个表
        private void AddNewChart(object sender)
        {
            
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

}
