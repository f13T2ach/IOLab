using iNKORE.UI.WPF.Modern.Controls;
using IOLab.Core.Assets;
using IOLab.Core.Models;
using IOLab.Models.Related;
using IOLab.Windows.Dialog.ViewModel;
using System.Windows;
using System.Windows.Controls.Primitives;

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
                chartNameText.Text = model.Name.ToString();
                chartServiceText.Text = model.ChartSevice.Name;
            }

            isEditedFlag = false; // 复位标志


        }
    }
}
