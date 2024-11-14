using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace IOLab.Core.Network.Data
{
    public class ContentCollection : ObservableCollection<WorkshopContent>, INotifyPropertyChanged
    {
        public ContentCollection() { }

        // 解析 JSON 对象并填充集合
        public void Parse(JObject networkData)
        {
            // 检查 networkData 是否包含需要的文案列表数据
            if (networkData == null || !networkData.ContainsKey("items"))
                return;

            // 尝试从 JSON 数据中提取 items 数组
            JArray itemsArray = networkData["items"] as JArray;
            if (itemsArray == null)
                return;

            // 清空当前集合内容
            this.Clear();

            // 遍历 items 数组，将每个文案对象解析为 WorkshopContent 并添加到集合中
            foreach (JObject item in itemsArray)
            {
                WorkshopContent content = new WorkshopContent();
                content.Parse(item);  // 调用 WorkshopContent 的 Parse 方法
                this.Add(content);
            }
        }

        // 判断集合是否为空
        public bool IsEmpty()
        {
            return this.Count == 0;
        }
    }
}
