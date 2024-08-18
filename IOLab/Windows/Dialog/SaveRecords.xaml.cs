using iNKORE.UI.WPF.Modern.Controls;
using IOLab.Core.Assets;
using System;
using System.Windows;

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
            switch (mode)
            {
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
