using IOLab.Core.Models;

namespace IOLab.Pages.Event.MonitorPages
{
    class ChartSelectionChangedEventArgs
    {
        public ChartTableModel SelectedChart { get; }
        public ChartSelectionChangedEventArgs(ChartTableModel selectedChart)
        {
            SelectedChart = selectedChart;
        }
    }
}
