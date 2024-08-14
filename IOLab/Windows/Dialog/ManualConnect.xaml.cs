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
using iNKORE.UI.WPF.Modern.Controls;

namespace IOLab.Windows.Dialog
{
    /// <summary>
    /// ManualConnect.xaml 的交互逻辑
    /// </summary>
    public partial class ManualConnect : ContentDialog
    {
        public ManualConnect()
        {
            InitializeComponent();

            ViewModel.ManualConnectViewModel vm = new ViewModel.ManualConnectViewModel();
            ViewModel.ManualConnectViewModel.isConfirmed = false;
            this.DataContext = vm;

            
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IsPrimaryButtonEnabled = true;
            
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            ViewModel.ManualConnectViewModel.isConfirmed = true;
        }
    }
}
