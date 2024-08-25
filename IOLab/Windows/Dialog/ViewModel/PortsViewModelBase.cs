/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * PortsViewModelBase.cs is part of the IOLab project.
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

﻿using IOLab.Core.Models;
using IOLab.Core.Serial;
using IOLab.Models.Related;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace IOLab.Windows.Dialog.ViewModel
{
    public class PortsViewModelBase : INotifyPropertyChanged
    {
        public ObservableCollection<PortPair> MyPorts { get; set; }
        private string _selectedPortID = "?";
        public string SelectedPortID
        {
            get => _selectedPortID; set
            {
                _selectedPortID = value;
                OnPropertyChanged(nameof(SelectedPortID));
            }
        }
        public PortsViewModelBase()
        {
            MyPorts = new ObservableCollection<PortPair>();
            // 从连接实例中的通信实例读取所有端口
            if (InstanceBundle.connectTool == null) // 如果没有初始化过 就返回
            {
                return;
            }
            List<string> ports = InstanceBundle.connectTool.Device.Ports;

            // 代码优化：直接读取静态MonitorPageViewModel.ChartTables
            // 再从监听服务列表中读取所有监听
            //List<Service> services = InstanceBundle.services;
            foreach (string port in ports)
            {

                PortPair target = new PortPair { PortId = port, ChartId = 0, IsChecked = false };
                /**
                // 寻找监听列表 看是否匹配
                if(services != null)
                {
                    foreach(Service service in services)
                    {
                        if(service.Id == port)
                        {
                            target.IsChecked = true;
                            break;
                        }
                    }
                }**/
                // 寻找表列表，看是否有匹配的
                foreach (ChartTableModel chart in InstanceBundle.ChartTables)
                {
                    if (chart.PortId == port)
                    {
                        target.ChartId = chart.Id;
                        target.IsChecked = true;
                        break;
                    }
                }
                MyPorts.Add(target);
            }
        }

        // 可优化 PortPair的用法
        public PortPair FindPortPairByContent(string content)
        {
            foreach (PortPair item in MyPorts)
            {
                if (item.PortId == content)
                {
                    return item;
                }
            }
            return null;
        }

        public ChartTableModel FindChartByPortPair(PortPair pair)
        {
            foreach (ChartTableModel chart in InstanceBundle.ChartTables)
            {
                if (chart.PortId == pair.PortId)
                {
                    return chart;
                }
            }
            return null;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
