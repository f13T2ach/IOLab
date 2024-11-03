using IOLab.Resources.Control.ViewModel;
using System.Windows.Controls;

namespace IOLab.Resources.Control
{
    /// <summary>
    /// ExperimentAssistance.xaml 的交互逻辑
    /// </summary>
    public partial class ExperimentAssistance : UserControl
    {
        public ExperimentAssistance()
        {
            InitializeComponent();
            ExperimentAssistanceViewModel viewModel = new ExperimentAssistanceViewModel();
            this.DataContext = viewModel;
        }
    }
}
