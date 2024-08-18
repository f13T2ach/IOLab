using System.IO.Ports;

namespace IOLab.Core.Serial
{
    public class Scanner
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
