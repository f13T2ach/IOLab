using IOLab.Core.Models.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using static IOLab.Core.Models.DeviceService;
using static IOLab.Core.Models.IOExperiment;

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
        private static List<DeviceService> _localDeviceService = new List<DeviceService>();

        public static List<DeviceService> LocalDeviceService
        {
            get { return _localDeviceService; }
            set { _localDeviceService = value; } 
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
