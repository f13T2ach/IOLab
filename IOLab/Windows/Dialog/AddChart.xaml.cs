using iNKORE.UI.WPF.Modern.Controls;
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
using System.Windows.Shapes;
using IOLab.Interfaces.ViewModel;
using iNKORE.UI.WPF.Modern;
using iNKORE.UI.WPF.Helpers;
using System.Windows.Controls.Primitives;
using IOLab.Windows.Dialog.ViewModel;
using IOLab.Models.Related;
using IOLab.Pages.ViewModel;
using LiveCharts.Wpf;
using IOLab.Core.Config;
using IOLab.Core.Serial;
using IOLab.Core.Models;
using System.Collections.ObjectModel;

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
        public string ChartXRow { get; private set; }
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
            foreach(ChartTableModel chart in InstanceBundle.ChartTables)
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
            string type = button.Content.ToString().Split("_")[0];

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
            if(chartIDInput.Value == 99)
            {
                TipFor99.Visibility = Visibility.Visible;
            }
            // 检查选择端口是否有效
            if(!IsValidPortSelected) {
                IsPrimaryButtonEnabled = false;
                return;
            }

            // 检查是否选用模板了
            MonitorService service = (MonitorService)ServiceComboBox.SelectedItem;
            if(service == null)
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
            if(count >= InternalConfig.MaxChartNumber)
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
            if(chartNameInput.Text == "")
            {
                ChartXRow = $"{Service.Name}";
            }
            else
            {
                ChartXRow = $"{chartNameInput.Text}";
            }
        }

        private void ServiceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckUserContents();
        }
    }
}
