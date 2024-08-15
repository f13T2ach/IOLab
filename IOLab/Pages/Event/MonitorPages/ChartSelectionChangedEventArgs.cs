using IOLab.Core.Models;
using IOLab.Models.Related;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOLab.Pages.Event.MonitorPages
{
    class ChartSelectionChangedEventArgs
    {
        public ChartTableModel SelectedChart { get; }
        public ChartSelectionChangedEventArgs(ChartTableModel selectedChart) {
            SelectedChart = selectedChart;
        }
    }
}
