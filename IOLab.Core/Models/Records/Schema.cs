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
using static IOLab.Core.Models.DeviceService;

namespace IOLab.Core.Models.Records
{
    public class Schema
    {
        private string _name;
        public string Name { get => _name;set { _name = value; } }

        private string _description;
        public string Description { get => _description; set { _description = value; } }

        // 指标详情
        private List<Service> _parameters = new List<Service>();
        public List<Service> Parameters { get => _parameters; set { _parameters = value; } }

        private IOExperiment _experiment;
        public IOExperiment Experiment { get => _experiment; set { _experiment = value; } }

        private List<string> _recordJSONData = new List<string>();
        public List<string> RecordJSONData { get => _recordJSONData; set { _recordJSONData = value; } }

        private ushort _time;
        public ushort Time { get => _time;set { _time = value; } }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="address">文件夹绝对路径</param>
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
            // 遍历匹配实验

            // 遍历匹配指标量
            return;
        }
    }
}
