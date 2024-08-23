/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * IOExperiment.cs is part of the IOLab project.
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

namespace IOLab.Core.Models
{
    public class IOExperiment
    {
        // 名称
        private string _name;
        public string Name { get { return _name; } set { _name = value; } }

        // 包名
        private string _packageName;
        public string PackageName { get { return _packageName; } set { _packageName = value; } }
        // 标签(如果有）
        private List<string> _tags = new List<string>();
        public List<string> Tags { get { return _tags; } set { _tags = value; } }

        // 化学方程式（如果有）
        private string _formula;
        public string Formula { get { return _formula; } set { _formula = value; } }

        // 步骤
        private Dictionary<string, string> _content = new Dictionary<string, string>();
        public Dictionary<string, string> Content { get { return _content; } set { _content = value; } }

        // 指标
        private List<MonitorService> _parameters = new List<MonitorService>();
        public List<MonitorService> Parameters { get => _parameters; set { _parameters = value; } }

        // 描述
        private string _description;
        public string Description { get => _description; set { _description = value; } }
        // ui应用的实际名称
        private string _realName;
        public string RealName { get { return _realName; } set { _realName = value; } }

        /// <summary>
        /// 本地实验构造函数
        /// </summary>
        /// <param name="address">json文件绝对路径</param>
        public IOExperiment(string address)
        {
            // 读取包名
            PackageName = Path.GetFileNameWithoutExtension(Read.GetLastLevelName(address));
            // 读取所有内容
            JObject json = JObject.Parse(Read.ReadFileUTF8(address));

            // 读取名称和化学方程式
            Name = json["name"].ToString();
            Formula = json["formula"].ToString();

            // 读取标签
            JArray tags = (JArray)json["tags"];
            string realNamePrefix = "";
            if (tags.Count != 0)
            {
                // 获取小标签
                foreach (var item in tags)
                {
                    string str = item.ToString();
                    Tags.Add(str);
                    // 合成realname
                    realNamePrefix += "[" + str + "]";
                }
            }
            RealName = realNamePrefix + Name;

            // 读取步骤
            JObject stepsNode = (JObject)json["steps"];
            foreach (var child in stepsNode)
            {
                Content.Add(child.Key, child.Value.ToString());
            }

            // 读取指标
            string preloadDescription = "";
            JArray services = (JArray)json["services"];
            foreach (var child in services)
            {
                int id = (int)child;

                foreach (MonitorService service in Bundles.LocalMonitorService)
                {
                    if (service.Id == id)
                    {
                        Parameters.Add(service);
                        preloadDescription += service.Name + Text.SplitSymbol;
                        break;
                    }
                }
            }

            // 处理预合成指标
            if (preloadDescription != "")
            {
                preloadDescription = preloadDescription.Remove(preloadDescription.LastIndexOf(Text.SplitSymbol), 1);
            }
            else
            {
                preloadDescription = Text.Collector;
            }

            // 读取描述
            Description = json["description"].ToString();
            if (Description == "")
            {
                // 没有预设标签就根据指标合成描述
                Description = $"{Text.NeedToPrepare} {preloadDescription}";
            }


        }
    }
}
