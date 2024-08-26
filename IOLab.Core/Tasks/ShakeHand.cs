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

﻿using IOLab.Core.Assets;
using IOLab.Core.Config;
using IOLab.Core.Interfaces.Threads;
using IOLab.Core.Models;
using IOLab.Core.Serial;
using IOLab.Core.Util;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Reflection.Metadata;

namespace IOLab.Core.Tasks
{
    public class ShakeHand : ITaskHandler<object>
    {
        public static JObject _parameter = null;


        public bool Inhibit { get => false; set { } }


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
                if (_parameter == null)
                {
                    //System.Diagnostics.Debug.WriteLine("Failed First"+i);
                    Thread.Sleep(InternalConfig.ShakeHandTimeStep);
                }
                else
                {
                    if (!isSecondCallback)
                    {
                        //System.Diagnostics.Debug.WriteLine("Success First" + i);
                        // 握手
                        InstanceBundle.connectTool._SerialPort.WriteLine(CommandsV1.ReceivedRequest);
                        return handler(true);
                    }
                    else
                    {
                        
                        if (_parameter["name"] == null)
                        {
                            //System.Diagnostics.Debug.WriteLine("Failed Second" + i);
                            Thread.Sleep(InternalConfig.ShakeHandTimeStep);
                            continue;
                        }
                        //System.Diagnostics.Debug.WriteLine("Success Second" + i);
                        // 补全串口信息
                        InstanceBundle.connectTool.Device.DeviceName = _parameter["name"].ToString();
                        InstanceBundle.connectTool.Device.Ports = JSONConverter.JArrayToListString(JArray.Parse(_parameter["ports"].ToString()));
                        InstanceBundle.IsConnected = true;
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
