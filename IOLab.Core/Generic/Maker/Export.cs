using IOLab.Core.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOLab.Core.Generic.Maker
{
    public class Export
    {
        /// <summary>
        /// 实验转JObject
        /// </summary>
        /// <param name="steps">步骤列表</param>
        /// <param name="source">编辑实验信息元</param>
        /// <param name="tags">标签</param>
        /// <returns></returns>
        public JObject ExperimentToJObject(ObservableCollection<StepItem> steps, EditExperiment source, List<string>? tags = null)
        {
            JObject jsonObject = new JObject
            {
                ["name"] = source.Name,
                ["experiment"] = source.PackageName,
                ["tags"] = new JArray(),
                ["formula"] = "",
                ["checkpoints"] = new JArray(),
                ["repeatPins"] = false,
                ["description"] = source.Description
            };

            // 解析tags
            ParseTags(tags, jsonObject);

            // 解析步骤
            JObject stepsObject = new JObject();
            Dictionary<string, CheckpointSource> checkpoints = new Dictionary<string, CheckpointSource>();
            List<int> requiredSensors = new List<int>();

            ParseSteps(steps, stepsObject, checkpoints, requiredSensors);

            jsonObject["steps"] = stepsObject;
            jsonObject["services"] = new JArray(requiredSensors);

            // 添加检查点
            jsonObject["checkpoints"] = BuildCheckpointsJson(checkpoints);

            return jsonObject;
        }

        /// <summary>
        /// 解析 tags
        /// </summary>
        private void ParseTags(List<string>? tags, JObject jsonObject)
        {
            if (tags != null && tags.Any())
            {
                jsonObject["tags"] = new JArray(tags);
            }
        }

        /// <summary>
        /// 解析实验步骤
        /// </summary>
        private void ParseSteps(ObservableCollection<StepItem> steps, JObject stepsObject, Dictionary<string, CheckpointSource> checkpoints, List<int> requiredSensors)
        {
            foreach (StepItem item in steps)
            {
                stepsObject[item.StepName] = item.StepDescription;

                // 如果没有传感器，跳过
                if (item.CurrentStepSensors == null || !item.CurrentStepSensors.Any())
                    continue;

                // 创建或更新检查点
                if (!checkpoints.ContainsKey(item.StepName))
                {
                    checkpoints[item.StepName] = new CheckpointSource();
                }

                // 处理传感器
                foreach (MonitorService service in item.CurrentStepSensors)
                {
                    checkpoints[item.StepName].FrozenList.Add(service.Id);
                    checkpoints[item.StepName].ChartList.Add(service.Id);

                    // 添加到需求传感器列表
                    if (!requiredSensors.Contains(service.Id))
                    {
                        requiredSensors.Add(service.Id);
                    }
                }
            }
        }

        /// <summary>
        /// 构建检查点的JSON
        /// </summary>
        private JArray BuildCheckpointsJson(Dictionary<string, CheckpointSource> checkpoints)
        {
            JArray checkpointsArray = new JArray();

            foreach (var item in checkpoints)
            {
                JObject checkpointObject = new JObject
                {
                    ["name"] = item.Key,
                    ["freeze"] = new JArray(item.Value.FrozenList),
                    ["require"] = new JArray(item.Value.ChartList)
                };

                checkpointsArray.Add(checkpointObject);
            }

            return checkpointsArray;
        }
    }

    class CheckpointSource
    {
        public List<int> FrozenList { get; } = new List<int>(); // 本步骤要冻结的服务
        public List<int> ChartList { get; } = new List<int>(); // 本步骤要见到的服务
    }
}
