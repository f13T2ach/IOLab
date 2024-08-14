using IOLab.Core.Util.FilesBehavior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IOLab.Core.Assets;
using static IOLab.Core.Models.Experiment;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IOLab.Core.Models.Records
{
    internal abstract class Schema
    {
        // 记录名称
        private string _name;
        public string Name { get => _name;set { _name = value; } }

        // 记录详情
        private string _description;
        public string Description { get => _description; set { _description = value; } }

        // 指标详情
        private List<DeviceService> _parameters;
        public List<DeviceService> Parameters { get => _parameters; set { _parameters = value; } }

        // 实验
        private IOExperiment _experiment;
        public IOExperiment Experiment { get => _experiment; set { _experiment = value; } }

        // 记录JSON数据（防止读取耗时过长 此时不展开）
        private List<string> _recordJSONData;
        public List<string> RecordJSONData { get => _recordJSONData; set { _recordJSONData = value; } }

        // 记录时间戳
        private ushort _time; // int会导致2038危机
        public ushort Time { get => _time;set { _time = value; } }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="address">路径</param>
        public Schema(string address)
        {
            // 完善路径
            if (!address.EndsWith(@"\")|| !address.EndsWith(@"/"))
            {
                address += @"\";
            }
            // 定义字典
            JObject recordDetail = new JObject();

            // 遍历文件夹寻找资源
            List<string> files = Traversal.GetAllFiles(address, 1);
            foreach(string file in files)
            {
                // 由于记录文件不多，故不做可拓展
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
            Name = recordDetail["experiment"]["name"].ToString();
        }
    }
}
