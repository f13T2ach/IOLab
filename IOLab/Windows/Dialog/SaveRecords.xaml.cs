using iNKORE.UI.WPF.Modern.Controls;
using IOLab.Core.Assets;
using Newtonsoft.Json;
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

namespace IOLab.Windows.Dialog
{
    /// <summary>
    /// SaveRecords.xaml 的交互逻辑
    /// </summary>
    public partial class SaveRecords : ContentDialog
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mode">0：不能保存1：正常保存2：强制保存</param>
        public SaveRecords(int mode)
        {
            InitializeComponent();
            switch (mode) {
                case 0:
                    ConatinerA.Visibility = Visibility.Hidden;
                    ConatinerB.Visibility = Visibility.Visible;
                    LoadTipForFun();
                    PrimaryButtonText = "";
                    break;
                case 1:
                    LoadInformation();
                    break;
                case 2:
                    SecondaryButtonText = "";
                    break;

            }
        }

        public void LoadInformation()
        {

        }

        private void LoadTipForFun()
        {
            int length = Text.TipForFun.Length;
            int index = new Random().Next(length);
            TipForFun.Text = Text.TipForFun[index];
        }
    }
}
