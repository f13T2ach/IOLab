using IOLab.Core.Models;

namespace IOLab.Core.Interfaces.LocalAssets
{
    /// <summary>
    /// IExperiment 接口定义了实验对象的基本结构和行为。
    /// </summary>
    public interface IExperiment
    {
        /// <summary>
        /// 实验名称
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 包名
        /// </summary>
        string PackageName { get; set; }

        /// <summary>
        /// 标签集合
        /// </summary>
        List<string> Tags { get; set; }

        /// <summary>
        /// 化学方程式
        /// </summary>
        string Formula { get; set; }

        /// <summary>
        /// 步骤内容
        /// </summary>
        Dictionary<string, string> Content { get; set; }

        /// <summary>
        /// 指标集合
        /// </summary>
        List<MonitorService> Parameters { get; set; }

        /// <summary>
        /// 实验描述
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// 检查点集合
        /// </summary>
        List<IOExperiment.Checkpoint> Checkpoints { get; set; }

        /// <summary>
        /// UI 应用的实际名称
        /// </summary>
        string RealName { get; set; }
    }
}
