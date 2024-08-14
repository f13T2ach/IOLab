using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;

namespace IOLab.Util.Serial
{
    class Scanner
    {
        public SerialPort serialPort = new SerialPort();
        public void ScanAvailableSerial()
        {

        }

        public string[] ScanAllSerial()
        {
            return SerialPort.GetPortNames();
        }
    }
}
