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
        private string _deviceName = ""; // 设备名称
        private List<string> _ports = new List<string>(); // port列表

        private string _portName;
        private int _baudRate; // 115200
        private int _dataBit;
        private Parity _parityCheckBit;
        private StopBits _stopBit;
        private string _comportNumber;

        public string DeviceName { get => _deviceName; set => _deviceName = value; }
        public List<string> Ports { get => _ports; set => _ports = value; }
        public string PortName { get => _portName; set => _portName = value; }
        public int BaudRate { get => _baudRate; set => _baudRate = value; }
        public int DataBit { get => _dataBit; set => _dataBit = value; }
        public Parity ParityCheckBit { get => _parityCheckBit; set => _parityCheckBit = value; }
        public StopBits StopBit { get => _stopBit; set => _stopBit = value; }
        public string ComportNumber { get => _comportNumber; set => _comportNumber = value; }
    }
}
