using IOLab.Network;
using IOLab.Windows.Dialog;
using IOLab.Windows.Dialog.Universal;
using System.Windows;
using Page = iNKORE.UI.WPF.Modern.Controls.Page;


namespace IOLab.Pages
{
    /// <summary>
    /// Interaction logic for AboutPage.xaml
    /// </summary>
    public partial class AboutPage : Page
    {
        public string[] ADTexts
        { get; set; }
        public AboutPage()
        {
            InitializeComponent();
            ADTexts = new string[] { "了解一中招生信息", "办一所让人幸福成长的学校", "在最美的时光遇见你" };
            DataContext = this;
        }

        private async void AboutUsButton_Click(object sender, RoutedEventArgs e)
        {
            LearnMoreAboutDeveloper lmad = new LearnMoreAboutDeveloper();
            await lmad.ShowAsync();
        }

        private async void ADButton_Click(object sender, RoutedEventArgs e)
        {
            await new OpenWebLink(URLBundle.DGYZWebsite).ShowAsync();
        }

        private async void AboutOSPButton_Click(object sender, RoutedEventArgs e)
        {
            await new QuoteOSP().ShowAsync();
        }
    }
}
