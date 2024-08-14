using IOLab.Pages.Event.MonitorPages;
using IOLab.Pages.ViewModel;
using IOLab.Windows.Dialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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

        private async void ManagePins_Click(object sender, RoutedEventArgs e)
        {
            PortDefine portDefine = new PortDefine(6);
            var result = await portDefine.ShowAsync();
        }

        private void OnChartSelectionChanged(object sender, ChartSelectionChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"单个表选中，ID = {e.SelectedChart.Id}");
            SingleChartOperation();
        }

        private void OnMultipleChartSelected(object sender,  MultipleChartsSelectedEventArgs e)
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

        private void OnNoChartSelected(object sender,EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"没有选中的表");
            NoChartOperation();
        }

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
    }
}
