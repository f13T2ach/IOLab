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

namespace IOLab.Windows.Dialog
{
    /// <summary>
    /// PortDefine.xaml 的交互逻辑
    /// </summary>
    public partial class PortDefine : ContentDialog
    {
        PortDefineViewModel viewModel;
        public PortDefine()
        {
            InitializeComponent();
            viewModel = new PortDefineViewModel();
            this.DataContext = viewModel;
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            // 拒绝用户修改
            ToggleButton button = (ToggleButton)sender;
            button.IsChecked = !button.IsChecked;
            viewModel.SelectedPortID = button.Content.ToString();
            // 处理事件
            int chartId = viewModel.FindPortItemByContent(viewModel.SelectedPortID).ChartId;
            if(chartId == 0)
            {
                chartIDText.Text = "未启动监视";
            }
            else
            {
                chartIDText.Text = chartId.ToString();
            }
        }
    }
}
