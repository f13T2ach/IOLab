using iNKORE.UI.WPF.Modern.Controls;
using IOLab.Core.Assets;
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
using System.Xml.Linq;

namespace IOLab.Windows
{
    /// <summary>
    /// GlobalExceptionCatch.xaml 的交互逻辑
    /// </summary>
    public partial class GlobalExceptionCatch : Window
    {
        public GlobalExceptionCatch()
        {
            InitializeComponent();
        }

        private async void ReloadStore_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog()
            {
                Title = Text.SureToReset,
                Content = Text.SureToResetExplain,
                PrimaryButtonText = "继续",
                CloseButtonText = "取消",
                DefaultButton = ContentDialogButton.Primary,
            };
            if(await dialog.ShowAsync() == ContentDialogResult.Primary)
            {

            }
        }
    }
}
