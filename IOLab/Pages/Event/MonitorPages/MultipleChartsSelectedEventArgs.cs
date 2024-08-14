using IOLab.Models.Related;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOLab.Pages.Event.MonitorPages
{
    class MultipleChartsSelectedEventArgs : EventArgs
    {
        public ChartTableModel[] SelectedCharts { get; }

        public MultipleChartsSelectedEventArgs(ChartTableModel[] selectedCharts)
        {
            SelectedCharts = selectedCharts;
        }
    }
}
