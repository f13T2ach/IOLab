/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * ConnectTool.cs is part of the IOLab project.
 *
 * IOLab is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * IOLab is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with IOLab. If not, see <https://www.gnu.org/licenses/>.
 */

using IOLab.Core.Assets;
using IOLab.Core.Generic;
using IOLab.Core.Interfaces.Threads;
using IOLab.Core.Models;
using Newtonsoft.Json.Linq;
using System.IO.Ports;
using System.Text;

// 注意！！正式做串口通信要修改测试代码
namespace IOLab.Core;
public class ConnectTool
{
    // 串口参数
    public Connection Device = new Connection();
    public System.IO.Ports.SerialPort _SerialPort = new System.IO.Ports.SerialPort();

    //实例化串口类

    //============================== 定义线程 ===========================================
    Thread ListenThread;
    public bool rx_thread_stop_flag = false;//线程停止标志
    public bool rx_thread_start_flag = false;//线程停止标志

    public void InitializeSerialSet(string portName)
    {
        // 串口名
        Device.PortName = portName;
        // 波特率
        Device.BaudRate = 115200;
        // 数据位
        Device.DataBit = 8;
        // 停止位
        Device.StopBit = (StopBits)1;
        // 校验位
        Device.ParityCheckBit = 0; // 不校验
        // 空置
        Device.DeviceName = "";
        // 测试 
        //Device.Ports = new List<string> { "I2C_1", "I2C_2", "EXT_A", "EXT_B" };
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
        _SerialPort.PortName = Device.PortName;            //串口名称
        _SerialPort.BaudRate = Device.BaudRate;      //波特率
        _SerialPort.DataBits = Device.DataBit;      //数据位
        _SerialPort.Parity = Device.ParityCheckBit;//校验位
        _SerialPort.StopBits = Device.StopBit;      //停止位
        //_SerialPort.ReadBufferSize = 4096;

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
            _SerialPort.Dispose();
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

            _SerialPort.Encoding = Encoding.UTF8;
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

    private StringBuilder _buffer = new StringBuilder();

    public void ReceiveData(object sender, SerialDataReceivedEventArgs e)
    {
        try
        {
            string content = _SerialPort.ReadExisting(); // 读取所有可用的数据
            _buffer.Append(content); // 将新数据追加到缓冲区

            // 处理完整的数据包（假设以换行符作为消息结束符）
            while (_buffer.ToString().Contains("\n"))
            {
                string line = _buffer.ToString();
                int index = line.IndexOf("\n");

                if (index >= 0)
                {
                    string message = line.Substring(0, index).Trim();
                    _buffer.Remove(0, index + 1); // 移除已处理的数据

                    if (!string.IsNullOrEmpty(message))
                    {
                        System.Diagnostics.Debug.WriteLine(message);
                        if (!message.StartsWith('['))
                        {
                            ProcessMessage(message);
                        }

                    }
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"发生读取错误: {ex.Message}");
        }
    }

    private void ProcessMessage(string content)
    {
        try
        {
            JObject obj = JObject.Parse(content); // 解析JSON数据
            string tag = Tags.ShakeHand;

            // 在线程列表中匹配 tag
            foreach (ITaskAnnounce announcer in InstanceBundle.list)
            {
                if (announcer.Tag == tag)
                {
                    if (!announcer.GetHandlerInstance().Inhibit)
                    {
                        Task.Run(() =>
                        {
                            announcer.GetHandlerInstance().Parameter = obj;
                        });
                    }
                }
            }

            // 全局消息线广播
            ActionBundle.DoUpdateNewest(obj);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"无法读取 JSON 数据: {ex.Message}");
        }
    }

}

