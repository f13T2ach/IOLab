using IOLab.Core.Assets;
using IOLab.Core.Interfaces.Threads;
using IOLab.Core.Serial;
using System.IO.Ports;
using System.Text;

// 注意！！正式做串口通信要修改测试代码
namespace IOLab.Core;
public class ConnectTool
{
    // 串口参数
    public Models.Connection.USBConnection UartPort;
    public System.IO.Ports.SerialPort _SerialPort = new System.IO.Ports.SerialPort();

    //实例化串口类

    //============================== 定义线程 ===========================================
    Thread ListenThread;
    public bool rx_thread_stop_flag = false;//线程停止标志
    public bool rx_thread_start_flag = false;//线程停止标志

    public void InitializeSerialSet(string portName)
    {
        // 串口名
        UartPort.PortName = portName;
        // 波特率
        UartPort.BaudRate = 115200;
        // 数据位
        UartPort.DataBit = 8;
        // 停止位
        UartPort.StopBit = (StopBits)1;
        // 校验位
        UartPort.ParityCheckBit = 0; // 不校验

        // 测试 
        UartPort.Ports = new List<string> { "I2C_1", "I2C_2", "EXT_A", "EXT_B" };
    }

    public Exception? OpenComport()
    {
        //打开串口
        if (_SerialPort.IsOpen)
        {
            //关闭线程
            RxThreadOnOff(false);
            //关闭后重新打开
            CloseComport();
            if (_SerialPort.IsOpen)//如果关闭失败就退出
            {

                return new Models.Exceptions.SerialFault("无法关闭 " + _SerialPort);
            }

        }
        //串口设置
        _SerialPort.PortName = UartPort.PortName;            //串口名称
        _SerialPort.BaudRate = UartPort.BaudRate;      //波特率
        _SerialPort.DataBits = UartPort.DataBit;      //数据位
        _SerialPort.Parity = UartPort.ParityCheckBit;//校验位
        _SerialPort.StopBits = UartPort.StopBit;      //停止位

        try
        {
            _SerialPort.Open(); //打开串口
                                //_SerialPort.DataReceived += new SerialDataReceivedEventHandler(_SerialPort_DataReceived);//串口接收处理函数
            RxThreadOnOff(true);//开启线程
        }
        catch (Exception exception)
        {

            return exception;
        }

        return null;
    }

    public Exception? CloseComport()
    {
        try
        {
            _SerialPort.ReadBufferSize = 4096;
            _SerialPort.Close(); //关闭串口
            return null;
        }
        catch (Exception exception)
        {
            return exception;
        }
    }


    private void RxThreadOnOff(bool on_off)
    {
        if (on_off)
        {
            _SerialPort.DataReceived += new SerialDataReceivedEventHandler(ReceiveData);

        }
        else
        {
            rx_thread_stop_flag = true;
            while (ListenThread.IsAlive)//等待停止
            { }
            rx_thread_stop_flag = false;
        }
    }

    public void ReceiveData(object sender,SerialDataReceivedEventArgs e)
    {
        //接收线程
        while (_SerialPort.IsOpen && !rx_thread_stop_flag)//线程循环
        {
            if (InstanceBundle.list == null)
            {
                continue;
            }
            try
            {
                string content = _SerialPort.ReadLine();
                System.Diagnostics.Debug.WriteLine(content);
                if(content == null)
                {
                    continue;
                }
            }
            catch
            {
                continue;
            }
            // 测试 没有实际内容上面的会卡住
            string tag = Tags.ShakeHand;
            string jsonCommand = "command";

            // 在线程列表中匹配 tag
            // 测试
            foreach (ITaskAnnounce announcer in InstanceBundle.list)
            {
                if (announcer.Tag == tag)
                {
                    announcer.GetHandlerInstance().Parameter = jsonCommand;
                }
            }
        }

    }

}

