using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOLab.Core.Models
{
    class Connection
    {
        public struct USBConnection
        {
            public string DeviceName; // 设备名称
            public int ProtocolVersion; // 交互协议id

            public string PortName;
            public int BaudRate; // 115200
            public int DataBit;
            public Parity ParityCheckBit;
            public StopBits StopBit;
            public string ComportNumber;
        }
    }
}
