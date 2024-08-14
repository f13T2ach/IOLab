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

namespace IOLab.Windows.Dialog
{
    /// <summary>
    /// PortDefine.xaml 的交互逻辑
    /// </summary>
    public partial class PortDefine : ContentDialog
    {
        
        public PortDefine(int max)
        {
            InitializeComponent();
            GenerateToggleButtons(max);
        }

        private void GenerateToggleButtons(int length)
        {
            ToggleButton[] PortButton = new ToggleButton[length];
            for (int i = 0; i < length; i++)
            {
                //实例化
                PortButton[i] = new ToggleButton();
                //定义控件名称
                PortButton[i].Name = "Port_"+i.ToString();
                //定义text属性，可以用string数组初始化为指定值
                PortButton[i].Content = (i+1).ToString();
                //注：如果不指定父容器，则坐标是相对于主窗体的
                
                //定义坐标
                //调整大小
                //bnt[i].AutoSize = true;
                PortButton[i].Width = 64;
                PortButton[i].Height = 64;
                PortButton[i].Margin = new Thickness(0, 0, 4, 0);
                //批量添加事件
                PortButton[i].Click += ToggleButtons_Click;
                ToggleButtonContainer.Children.Add(PortButton[i]);
            }

        }

        private void ToggleButtons_Click(object sender, EventArgs e)
        {
            ToggleButton button = (ToggleButton)sender;
            button.IsChecked = false;
        }
    }
}
