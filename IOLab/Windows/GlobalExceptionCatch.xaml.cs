using iNKORE.UI.WPF.Modern.Controls;
using IOLab.Core.Assets;
using System.Windows;

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
            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
            {

            }
        }
    }
}
