using IOLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOLab.Core.Generic.Maker
{
    public class StepItem
    {
        public string StepName { get; set; }
        public string StepDescription { get; set; }

        // 当前步骤的检查点列表
        public ObservableCollection<MonitorService> CurrentStepSensors { get; set; } = new ObservableCollection<MonitorService>();
    }
}
