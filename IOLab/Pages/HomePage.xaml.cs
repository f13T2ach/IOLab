using iNKORE.UI.WPF.Modern;
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
        }

        private void Preparations()
        {
            MainWindow mw = new MainWindow();
            EquationImage.Source = BitmapFrame.Create(
                new Uri(Util.LaTeXImage.GetRequestUrl(Static.Theme.themeMode == ApplicationTheme.Dark, @"\ce{Ba^2+ + SO4^2- = BaSO4 v}")), 
                BitmapCreateOptions.None, 
                BitmapCacheOption.Default);
        }
    }
}
