using IOLab.Core.Models.Records;

namespace IOLab.Core.Models
{
    public class Bundles
    {
        // 实验列表
        private static List<IOExperiment> _localExperiments = new List<IOExperiment>();

        public static List<IOExperiment> LocalExperiments
        {
            get { return _localExperiments; }
            set { _localExperiments = value; }
        }

        // 采集器列表
        private static List<MonitorService> _localMonitorService = new List<MonitorService>();

        public static List<MonitorService> LocalMonitorService
        {
            get { return _localMonitorService; }
            set { _localMonitorService = value; }
        }

        // 本地记录
        private static List<Schema> _localRecords = new List<Schema>();

        public static List<Schema> LocalRecords
        {
            get { return _localRecords; }
            set { _localRecords = value; }
        }

        // TODO 在线实验列表
    }
}
