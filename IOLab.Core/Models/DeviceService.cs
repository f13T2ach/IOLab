
using IOLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOLab.Core.Models
{
    public class DeviceService
    {
        // 检测项目
        public struct Service
        {
            public Connection.USBConnection BaseConnection; // 连接
            public bool isAccepted; // 是否被接受
            public bool isResponded; // 是否有答复

            public List<object> Values; // 历史数据
            public string Id; // 端口
        }
    }
}
