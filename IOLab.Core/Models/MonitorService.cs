
using IOLab.Core.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOLab.Core.Models
{
    public class MonitorService//监视器
    {
        private string _portType; // 端口类型

        private string _name; // 包名

        private string _yRow; // 包名

        private int _id; // 引用ID

        public string PortType { get => _portType; set => _portType = value; }
        public string Name { get => _name; set => _name = value; }
        public int Id { get => _id; set => _id = value; }
        public string YRow { get => _yRow; set => _yRow = value; }

        public MonitorService(JObject data) {
             Name = data["name"].ToString();
             Id = (int)data["serviceID"];
             PortType = data["type"].ToString();
             YRow = data["yrow"].ToString();
        }
    }
}
