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
using IOLab.Core.Models;
using IOLab.Core.Assets;

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

        // ischecked是否已被修改过
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
            

            viewModel.SelectedPortID = button.Content.ToString();
            // 处理事件
            PortPair pair = viewModel.FindPortPairByContent(viewModel.SelectedPortID);
            int chartId = pair.ChartId;
            ChartTableModel model = viewModel.FindChartByPortPair(pair);
            if (chartId == 0)
            {
                chartNameText.Text = Text.MonitorNotStartYet;
                chartServiceText.Text = Text.MonitorNotStartYet;
            }
            else
            {
                chartNameText.Text = model.XRow.ToString();
                chartServiceText.Text = model.ChartSevice.Name;
            }

            isEditedFlag = false; // 复位标志


        }
    }
}
