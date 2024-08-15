
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
        public struct Service //监视器
        {
            public Connection.USBConnection BaseConnection; // 连接
            public bool isAccepted; // 是否被接受
            public bool isResponded; // 是否有答复
            public string PortType; // 端口类型
            public string Name; // 包名
            public int Id; // 引用ID
        }
    }
}
