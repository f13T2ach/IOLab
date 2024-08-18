using iNKORE.UI.WPF.Modern;
using IOLab.Core.Serial;
using IOLab.Pages.Remote;
using IOLab.Pages.ViewModel;
using System;
using System.Windows.Media.Imaging;
using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace IOLab.Pages
{
    /// <summary>
    /// Interaction logic for RemotePage.xaml
    /// </summary>
    public partial class RemotePage : Page
    {
        NoRemoteTarget PageRemote_NoRemoteTarget = new NoRemoteTarget();
        Managers PageRemote_Managers = new Managers();
        public RemotePage()
        {
            InitializeComponent();
            Preparations();
            this.DataContext = new RemotePageViewModel();
        }

        private void Preparations()
        {
            if (InstanceBundle.connectTool == null)
            {
                ViewField.Navigate(PageRemote_NoRemoteTarget);
            }
            else
            {
                ViewField.Navigate(PageRemote_Managers);
            }

            ShallowShadow.Source = BitmapFrame.Create(
                new Uri(Static.Theme.themeMode == ApplicationTheme.Dark ? @"pack://application:,,,/Resources/Images/Tiny/ShallowShadowDark.png" : "pack://application:,,,/Resources/Images/Tiny/ShallowShadow.png"),
                BitmapCreateOptions.None,
                BitmapCacheOption.Default);
        }
    }
}
