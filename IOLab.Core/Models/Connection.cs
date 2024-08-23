/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * Connection.cs is part of the IOLab project.
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

﻿using System.IO.Ports;

namespace IOLab.Core.Models
{
    public class Connection
    {
        public struct USBConnection
        {
            public string DeviceName; // 设备名称
            public List<string> Ports; // port列表

            public string PortName;
            public int BaudRate; // 115200
            public int DataBit;
            public Parity ParityCheckBit;
            public StopBits StopBit;
            public string ComportNumber;
        }
    }
}
