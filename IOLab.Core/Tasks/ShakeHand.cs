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

namespace IOLab.Core.Tasks
{
    public class ShakeHand : ITaskHandler<object>
    {
        public static string _parameter = "";

        string ITaskHandler<object>.Parameter
        {
            get { return _parameter; }
            set { _parameter = value; }
        }

        object ITaskHandler<object>.Handler()
        {
            // 等待验证  
            for (int i = 0; i < InternalConfig.ConnectTimeout / InternalConfig.ShakeHandTimeStep; i++)
            {
                if (_parameter != "command")
                {
                    Thread.Sleep(InternalConfig.ShakeHandTimeStep);
                }
                else
                {
                    // 配置一个串口实例
                    return "";
                }
            }
            return Text.ConnectTimeOut;
        }
    }
}
