using IOLab.Pages.Appreciations.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IOLab.Pages.Appreciations
{
    /// <summary>
    /// Cover.xaml 的交互逻辑
    /// </summary>
    public partial class Cover : Page
    {
        public Cover()
        {
            InitializeComponent();
            CoverViewModel viewModel = new CoverViewModel();
            CoverViewModel.ProgressValue = 0;
            this.DataContext = viewModel;
        }

        private DoubleAnimation GetAnimation(double from, double to, double time, double amplitude = 0.3)
        {
            DoubleAnimation animation = new DoubleAnimation()
            {
                From = from,
                To = to,
                Duration = TimeSpan.FromSeconds(time),
                EasingFunction = new BackEase()
                {
                    Amplitude = amplitude,
                    EasingMode = EasingMode.EaseOut,
                },
            };
            return animation;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void Progress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
    }

    
}
