using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOLab.Core.Models
{
    class DeviceService
    {
        // 检测项目
        public struct Service
        {
            public Connection.USBConnection BaseConnection; // 连接
            public bool isAccepted; // 是否被接受
            public bool isResponded; // 是否有答复

            public List<object> Values; // 历史数据

            public string Name; // 检测项目名 如 Cl-
            public string Unit; // 单位 如 mol/L
            public int ID; // 识别码
        }
    }
}
