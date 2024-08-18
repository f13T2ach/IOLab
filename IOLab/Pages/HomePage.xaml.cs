using iNKORE.UI.WPF.Modern;
using IOLab.Pages.ViewModel;
using System;
using System.Windows.Media.Imaging;
using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace IOLab.Pages
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
            Preparations();
            HomePageViewModel viewModel = new HomePageViewModel();
            this.DataContext = viewModel;
        }

        private void Preparations()
        {
            EquationImage.Source = BitmapFrame.Create(
                new Uri(Util.LaTeXImage.GetRequestUrl(Static.Theme.themeMode == ApplicationTheme.Dark, @"\ce{Ba^2+ + SO4^2- = BaSO4 v}")),
                BitmapCreateOptions.None,
                BitmapCacheOption.Default);
        }
    }
}
