using IOLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using static IOLab.Core.Models.DeviceService;
using static IOLab.Core.Models.Experiment;

namespace IOLab.Core.Assets
{
    internal class Bundles
    {
        // 实验列表
        private static List<IOExperiment> _localExperiments;

        public static List<IOExperiment> LocalExperiments;

        // 采集器列表
        private static List<Service> _localDeviceService;

        public static List<Service> LocalDeviceService;
    }
}
