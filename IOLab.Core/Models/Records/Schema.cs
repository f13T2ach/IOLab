/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * Schema.cs is part of the IOLab project.
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

﻿using IOLab.Core.Assets;
using IOLab.Core.Util.FilesBehavior;
using Newtonsoft.Json.Linq;

namespace IOLab.Core.Models.Records
{
    public class Schema
    {
        // 名称不能重复 因此可以用来检索
        private string _name;
        public string Name { get => _name; set { _name = value; } }

        private string _description;
        public string Description { get => _description; set { _description = value; } }

        // 指标详情
        private List<MonitorService> _parameters = new List<MonitorService>();
        public List<MonitorService> Parameters { get => _parameters; set { _parameters = value; } }

        private IOExperiment _experiment;
        public IOExperiment Experiment { get => _experiment; set { _experiment = value; } }

        private List<string> _recordJSONData = new List<string>();
        public List<string> RecordJSONData { get => _recordJSONData; set { _recordJSONData = value; } }

        private long _time;
        public long Time { get => _time; set { _time = value; } }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="address">文件夹绝对路径</param>
        public Schema(string address)
        {
            // 完善路径
            if (!address.EndsWith(@"\") || !address.EndsWith(@"/"))
            {
                address += @"\";
            }
            // 定义字典
            JObject recordDetail = new JObject();

            // 遍历文件夹寻找资源
            List<string> files = Traversal.GetAllFiles(address, 1);
            foreach (string file in files)
            {
                if (file.StartsWith(FileNames.RecordDetail))
                {
                    // 读取JSON数据
                    string? content = Read.ReadFileUTF8(address + file);
                    if (content != null)
                    {
                        recordDetail = JObject.Parse(content);
                    }
                }
                else if (file.StartsWith(FileNames.ChartDataPrefix))
                {
                    string? content = Read.ReadFileUTF8(address + file);
                    if (content != null)
                    {
                        RecordJSONData.Add(content);
                    }
                }
                // TODO 设备
            }

            // 获取资源
            Name = Read.GetLastLevelName(address);
            Time = (long)recordDetail["time"];
            // 遍历匹配实验

            // 遍历匹配指标量

            // 如果没有设定标签，就合成标签
            Description = $"于 {Util.Time.GetDateTime(Time)} 完成";
            return;
        }
    }
}
