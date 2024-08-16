using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOLab.Models.Related
{
    // 该类被Service对象和viewmodel同时引用
    /// <summary>
    /// 映射对
    /// </summary>
    public class PortPair
    {
        /// <summary>
        /// PortId必须是对单片机的映射
        /// </summary>
        public string? PortId { get; set; }

        public int ChartId { get; set; }

        public bool IsChecked { get; set; }
    }
}
