using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOLab.Core.Util.LabAI
{
    public static class TextPrompts
    {
        public static string MAJOR_PROMPT = "This PROMPT is a part of IOLab.";

        public static string ROLE_PROMPT = "请你遵守以下规则\n" +
            "1. 以下内容一律使用中文回复\n" +
            "2. 如果你被要求输出一段文字，直接按原文本输出即可\n" +
            "3. 如果你认为用户给定的内容不合理而无法分析，输出“无法总结本实验”，不要给出修复建议\n" +
            "你是一个实验室软件数据采集总结 AI ，实验室软件名为“IOLab”，下面，你将被输入一段实验数据，请你按照以下结构展开分析\n" +
            "1. 实验总结（包含实验成功之处与不足之处）\n" +
            "2. 实验原理（通过实验数据指出实验印证的原理）\n" +
            "3. 实验评分（给出一个分数并给出给分原因）\n" +
            "实验数据由程序生成，如下所示：";
        public static string EXPERIMENT_NAME = "实验名称：";

        public static string EQUIPMENTS_LIST = "仪器列表：";

        public static string STEPS_LIST = "实验步骤：";

        public static string DATA_LIST = "实验数据：";

    }
}
