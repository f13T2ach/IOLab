using IOLab.Core.Util.FilesBehavior;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOLab.Core.Models
{
    public class IOExperiment
    {
        // 名称
        private static string _name;
        public static string Name { get { return _name; } set { _name = value; } }

        // 包名
        private static string _packageName;
        public static string PackageName { get { return _packageName; } set { _packageName = value; } }
        // 标签(如果有）
        private static List<string> _tags = new List<string>();
        public static List<string> Tags { get { return _tags; } set { _tags = value; } }

        // 化学方程式（如果有）
        private static string _formula;
        public static string Formula { get { return _formula; } set { _formula = value; } }

        // 步骤
        private static Dictionary<string, string> _content = new Dictionary<string, string>();
        public static Dictionary<string, string> Content { get { return _content; } set { _content = value; } }

        // 指标
        private static List<MonitorService> _parameters = new List<MonitorService>();
        public static List<MonitorService> Parameters { get => _parameters; set { _parameters = value; } }

        // 描述
        private static string _description;
        public static string Description { get => _description; set { _description = value; } }
        // ui应用的实际名称
        private static string _realName;
        public static string RealName {get{ return _realName; } set{ _realName = value; } }

        /// <summary>
        /// 本地实验构造函数
        /// </summary>
        /// <param name="address">json文件绝对路径</param>
        public IOExperiment(string address)
        {
            // 读取包名
            PackageName = Path.GetFileNameWithoutExtension(Read.GetLastLevelName(address));
            
            JObject json = JObject.Parse(Read.ReadFileUTF8(address));
            // 读取名称和化学方程式
            Name = json["name"].ToString();
            Formula = json["formula"].ToString();

            // 读取Tag
            JArray tags = (JArray)json["tags"];
            string realNamePrefix = "";
            if (tags.Count != 0)
            {
                // 获取小标签
                foreach(var item in tags)
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
            foreach(var child in stepsNode)
            {
                Content.Add(child.Key, child.Value.ToString());
            }

            // 读取描述
            Description = json["description"].ToString();
            if (Description == "")
            {
                // 没有预设标签就根据指标合成描述
                Description = "测试";
            }
            
        }
    }
}
