using IOLab.Core.Interfaces.Threads;
using IOLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IOLab.Core.Models.DeviceService;

namespace IOLab.Core.Serial
{
    public static class InstanceBundle
    {
        public static volatile List<ITaskAnnounce>? list;

        public static ConnectTool connectTool; // 连接实例

        public static List<Service> services; // 服务列表
    }
}
