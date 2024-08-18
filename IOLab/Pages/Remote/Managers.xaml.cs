using IOLab.Pages.Remote.ViewModel;
using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace IOLab.Pages.Remote
{
    /// <summary>
    /// Manager.xaml 的交互逻辑
    /// </summary>
    public partial class Managers : Page
    {
        public Managers()
        {
            InitializeComponent();
            ManagersViewModel viewModel = new ManagersViewModel();
            this.DataContext = viewModel;
            // InitInformation();
        }

        private void InitInformation()
        {
            // Header.Text = DeviceName.TextContent;
            // DeviceNameText.Text = DeviceName.TextContent;

        }

        /**
        private async void Rename_Click(object sender, RoutedEventArgs e)
        {
            DeviceName dn = new DeviceName();
            NameEdit edit = new NameEdit(dn);

            string? originName = DeviceName.TextContent;
            var result = await edit.ShowAsync();
            if(result == iNKORE.UI.WPF.Modern.Controls.ContentDialogResult.Secondary)
            {
                DeviceName.TextContent = originName;
                return;
            }

            // 处理更名业务
            InitInformation(); //成功后变更
        }**/
    }
}
