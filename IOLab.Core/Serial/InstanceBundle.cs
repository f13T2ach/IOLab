using IOLab.Core.Interfaces.Threads;
using IOLab.Core.Models;
using System.Collections.ObjectModel;

namespace IOLab.Core.Serial
{
    public static class InstanceBundle
    {
        public static volatile List<ITaskAnnounce>? list = new List<ITaskAnnounce>();

        public static ConnectTool connectTool; // 连接实例
        public static ObservableCollection<ChartTableModel> ChartTables { get; set; } // 监视器
    }
}
