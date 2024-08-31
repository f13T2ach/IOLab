/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * ShakeHand.cs is part of the IOLab project.
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
using IOLab.Core.Config;
using IOLab.Core.Generic;
using IOLab.Core.Interfaces.Threads;
using IOLab.Core.Util;
using Newtonsoft.Json.Linq;

namespace IOLab.Core.Tasks
{
    public class ShakeHand : ITaskHandler<object>
    {
        public JObject _parameter = null;


        public bool Inhibit { get => false; set { } }

        public ITaskAnnounce? Announcer { get; set; }


        // 握手步骤
        // 单片机发送请求
        // 电脑发送成功
        // 单片机发送设备详细
        JObject ITaskHandler<object>.Parameter
        {
            get { return _parameter; }
            set { _parameter = value; }
        }

        object ITaskHandler<object>.Handler()
        {

            return handler(false);
        }

        /// <summary>
        /// 处理器（自迭代）
        /// </summary>
        /// <param name="isSecondCallback">是否等待第二次握手</param>
        /// <returns></returns>
        string handler(bool isSecondCallback)
        {
            // 等待验证  
            for (int i = 0; i < InternalConfig.ConnectTimeout / InternalConfig.ShakeHandTimeStep; i++)
            {
                if (Announcer.tokenSource.IsCancellationRequested)
                {
                    return Text.ForcedCanceledBySystem;
                }

                if (_parameter == null)
                {
                    Thread.Sleep(InternalConfig.ShakeHandTimeStep);
                }
                else
                {
                    if (!isSecondCallback)
                    {
                        // 握手
                        if (InstanceBundle.connectTool == null)
                        {
                            return "对象超时或消失";
                        }
                        InstanceBundle.connectTool._SerialPort.WriteLine(CommandsV1.ReceivedRequest);
                        return handler(true);
                    }
                    else
                    {

                        if (_parameter["name"] == null)
                        {
                            Thread.Sleep(InternalConfig.ShakeHandTimeStep);
                            continue;
                        }
                        // 补全串口信息
                        InstanceBundle.connectTool.Device.DeviceName = _parameter["name"].ToString();
                        InstanceBundle.connectTool.Device.Ports = JSONConverter.JArrayToListString(JArray.Parse(_parameter["ports"].ToString()));
                        InstanceBundle.IsConnected = true;
                        // 添加性能收集检测器
                        new TaskAnnouncer(new DataColt(), Tags.PerformanceColt);

                        return "";

                    }

                }
            }
            if (!isSecondCallback)
            {
                return Text.ConnectTimeOut;
            }
            else
            {
                return Text.CallbackTimeOut;
            }
        }
    }
}
