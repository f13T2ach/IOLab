using iNKORE.UI.WPF.Modern.Controls;
using System.Windows.Controls;

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
