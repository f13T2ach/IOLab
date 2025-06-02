using IOLab.Core.Generic;
using System.Text;

namespace IOLab.Core.Util.LabAI
{
    public class PromptGenerator
    {
        public PromptGenerator() { }

        public string GeneratePrompt()
        {
            StringBuilder prompt = new StringBuilder();
            if (InstanceBundle.UsingExperiment == null || InstanceBundle.ChartTables == null || InstanceBundle.ChartTables.Count == 0)
            {
                return "输出：无法总结本实验，因为没有进行的实验或者实验数据为空";
            }

            prompt.Append(TextPrompts.ROLE_PROMPT); // 提示词

            prompt.Append(TextPrompts.EXPERIMENT_NAME + InstanceBundle.UsingExperiment.Name + "\n");    // 实验名称
            prompt.Append(TextPrompts.EQUIPMENTS_LIST + "\n");  // 仪器列表
            foreach (var item in InstanceBundle.UsingExperiment.Parameters) // 轮询列表
            {
                prompt.Append(item.Name);
            }

            prompt.Append("\n");

            prompt.Append(TextPrompts.STEPS_LIST + "\n");

            foreach (var item in InstanceBundle.UsingExperiment.Content) // 轮询字典
            {
                prompt.Append(item.Key + "：" + item.Value + "\n");
            }

            prompt.Append("\n");

            prompt.Append(TextPrompts.DATA_LIST + "\n");

            foreach (var item in InstanceBundle.ChartTables) // 轮询列表
            {
                prompt.Append("表名：" + item.Name + "\n");  // 表名
                prompt.Append("Y轴：" + item.YRow + "\n");    //Y轴
                prompt.Append("值列表：\n");
                if (item.Series[0] == null)
                {
                    prompt.Append("无数据\n");
                    continue;
                }

                foreach (var value in item.Series[0].Values)
                {
                    prompt.Append(value + ",");
                }
                prompt.Append("\n");
            }

            return prompt.ToString();

        }
    }
}
