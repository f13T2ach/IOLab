/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * InstanceBundle.cs is part of the IOLab project.
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

using IOLab.Core.Interfaces.Threads;
using IOLab.Core.Models;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;

namespace IOLab.Core.Serial
{
    public static class InstanceBundle
    {
        public static ConcurrentQueue<ITaskAnnounce>? list = new ConcurrentQueue<ITaskAnnounce>();

        public static ConnectTool connectTool; // 连接实例

        private static bool _isConnected = false;

        public static event Action<bool> IsConnectedChanged; // 变更连接状态事件

        public static bool IsConnected
        {
            get => _isConnected;
            set
            {
                if (_isConnected != value)
                {
                    _isConnected = value;
                    IsConnectedChanged?.Invoke(_isConnected);
                }
            }
        }

        private static ObservableCollection<ChartTableModel> _chartTables = new ObservableCollection<ChartTableModel> { new ChartTableModel("Test", "测试", "#0078D7") };
        public static ObservableCollection<ChartTableModel> ChartTables { get => _chartTables; set => _chartTables = value; } // 监视器
    }
}
