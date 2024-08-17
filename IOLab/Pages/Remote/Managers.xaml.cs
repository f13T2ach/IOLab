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

using IOLab.Interfaces.ViewModel;
using IOLab.Windows.Dialog;
using IOLab.Util.StaticModel;
using Page = iNKORE.UI.WPF.Modern.Controls.Page;
using IOLab.Windows.Dialog.Universal;
using IOLab.Pages.Remote.ViewModel;
using iNKORE.UI.WPF.Modern;

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
