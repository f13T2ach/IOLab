using IOLab.Core.Models;
using System;

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
