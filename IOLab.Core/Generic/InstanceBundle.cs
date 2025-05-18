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
using IOLab.Network;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;

namespace IOLab.Core.Generic
{
    public static class InstanceBundle
    {

        public static Process cmdProcess = new Process(); // cmd进程

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

        private static ObservableCollection<ChartTableModel> _chartTables = new ObservableCollection<ChartTableModel>();

        public static ObservableCollection<ChartTableModel> ChartTables
        {
            get => _chartTables;
            set
            {
                if (_chartTables != value)
                {
                    // 取消旧集合的事件监听
                    if (_chartTables != null)
                    {
                        _chartTables.CollectionChanged -= OnChartTablesCollectionChanged;
                    }

                    _chartTables = value ?? new ObservableCollection<ChartTableModel>();

                    // 监听新集合的 CollectionChanged 事件
                    _chartTables.CollectionChanged += OnChartTablesCollectionChanged;

                    // 手动触发 ChartTablesUpdated 事件
                    ChartTablesUpdated?.Invoke();
                }
            }
        }

        private static void OnChartTablesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // 当集合内容变化时触发 ChartTablesUpdated 事件
            ChartTablesUpdated?.Invoke();
        }

        // 声明事件
        public static event Action ChartTablesUpdated;
        public static event Action<IOExperiment> UsingExperimentChanged; // 变更选用实验事件

        private static IOExperiment? _usingExperiment;
        public static IOExperiment? UsingExperiment
        {
            get => _usingExperiment; set
            {
                _usingExperiment = value;
                UsingExperimentChanged?.Invoke(UsingExperiment);
            }
        }

        public static event Action<Auth> UsingAuthChanged; // 变更选用实验事件

        private static Auth? _usingAuth;
        public static Auth? UsingAuth
        {
            get => _usingAuth; set
            {
                _usingAuth = value;
                UsingAuthChanged?.Invoke(UsingAuth);
            }
        }


    }
}
