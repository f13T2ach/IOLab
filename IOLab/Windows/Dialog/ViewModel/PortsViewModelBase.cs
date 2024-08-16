using IOLab.Commands;
using IOLab.Core.Models;
using IOLab.Core.Serial;
using IOLab.Models.Related;
using IOLab.Pages.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IOLab.Windows.Dialog.ViewModel
{
    public class PortsViewModelBase : INotifyPropertyChanged
    {
        public ObservableCollection<PortPair> MyPorts { get; set; }
        private string _selectedPortID = "?";
        public string SelectedPortID { get=>_selectedPortID; set {
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
            List<string> ports = InstanceBundle.connectTool.UartPort.Ports;

            // 代码优化：直接读取静态MonitorPageViewModel.ChartTables
            // 再从监听服务列表中读取所有监听
            //List<Service> services = InstanceBundle.services;
            foreach (string port in ports)
            {
                
                PortPair target = new PortPair {PortId=port,ChartId=0,IsChecked=false };
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
                    if(chart.PortId == port)
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
            foreach(PortPair item in MyPorts)
            {
                if(item.PortId == content)
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
                if (chart.PortId == pair.PortId) {
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
