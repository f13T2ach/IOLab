using iNKORE.UI.WPF.Modern;
using iNKORE.UI.WPF.Modern.Controls;
using IOLab.Core.Local;
using IOLab.Core.Models.Records;
using IOLab.Pages.Appreciations;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Printing.IndexedProperties;
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

namespace IOLab
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static bool _FirstStarup = false;

        public MainWindow()
        {
            Prepare();
            App.AlreadyInit = true;
            InitializeComponent();
            //测试代码
            //Static.Theme.themeMode = ApplicationTheme.Dark;
            Util.StaticModel.DeviceName.TextContent = "瑞克五代";

            if (_FirstStarup)
            {
                StartAppreciations();
            }
        }

        private void Prepare()
        {
            // 每次实例化都会对静态对象操作一次 这是不安全的
            if (App.AlreadyInit)
            {
                return;
            }
            ReadAssets.ReadRecords();
            ReadAssets.ReadExperiments();
            ReadAssets.ReadLocalServices();
        }

        private void StartAppreciations()
        {
            
            NavigationView_Root.Visibility = Visibility.Hidden;
            Frame_Secondary.Visibility = Visibility.Visible;
            ChangeTheme(ApplicationTheme.Dark);
            Cover cover = new Cover();
            Frame_Secondary.Navigate(cover);
        }
        

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            
            var item = sender.SelectedItem;
            Page? page = null;

            if (item == NavigationViewItem_Home)
            {
                Pages.HomePage Page_Home = new Pages.HomePage();
                page = Page_Home;
            }
            else if (item == NavigationViewItem_Remote)
            {
                Pages.RemotePage Page_Remote = new Pages.RemotePage();
                page = Page_Remote;
            }
            else if (item == NavigationViewItem_Monitor)
            {
                Pages.MonitorPage Page_Monitor = new Pages.MonitorPage();
                page = Page_Monitor;
            }
            else if(item == NavigationViewItem_Setting)
            {
                Pages.SettingPage Page_Setting = new Pages.SettingPage();
                page = Page_Setting;
            }
            else if(item == NavigationViewItem_About)
            {
                Pages.AboutPage Page_About = new Pages.AboutPage();
                page = Page_About;
            }

            if (page != null)
            {
                NavigationView_Root.Header = page.Title;
                Frame_Main.Navigate(page);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_FirstStarup)
            {
                return;
            }
            NavigationView_Root.SelectedItem = NavigationViewItem_Home;
            
            
        }


        public static void ChangeTheme(ApplicationTheme theme)
        {
            ThemeManager.Current.ApplicationTheme = theme;
            
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            var collections = Application.Current.Windows;

            foreach (Window window in collections)
            {
                if (window != this)
                    window.Close();
            }

            //base.OnClosed(e);
        }
    }
    
}
