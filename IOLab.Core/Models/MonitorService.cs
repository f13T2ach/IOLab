/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * MonitorService.cs is part of the IOLab project.
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

using IOLab.Core.Interfaces.LocalAssets;
using Newtonsoft.Json.Linq;

namespace IOLab.Core.Models
{
    public class MonitorService : IMonitorService//监视器
    {
        private string _portType; // 端口类型

        private string _name; // 包名

        private string _yRow; // y轴内容

        private int _id; // 引用ID

        public string PortType { get => _portType; set => _portType = value; }
        public string Name { get => _name; set => _name = value; }
        public int Id { get => _id; set => _id = value; }
        public string YRow { get => _yRow; set => _yRow = value; }

        public MonitorService(JObject data)
        {
            Name = data["name"].ToString();
            Id = (int)data["serviceID"];
            PortType = data["type"].ToString();
            YRow = data["yrow"].ToString();
        }
    }
}
