using Newtonsoft.Json.Linq;

namespace IOLab.Core.Generic.Maker
{
    public enum EditExperimentType
    {
        AddressOnly,
        FullInformation,
        Undefined,
    }

    /// <summary>
    /// 编辑器所知实验基本要素
    /// </summary>
    public class EditExperiment
    {
        /// <summary>
        /// 实验名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 实验描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 实验包名
        /// </summary>
        public string PackageName { get; set; } = string.Empty;

        /// <summary>
        /// 保存地址
        /// </summary>
        public string SaveAddress { get; set; } = string.Empty;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public EditExperiment() { }

        /// <summary>
        /// 使用 JSON 和地址构造实验
        /// </summary>
        /// <param name="source">实验的 JSON 数据</param>
        /// <param name="address">保存地址</param>
        /// <exception cref="ArgumentNullException">如果 source 或 address 为空</exception>
        /// <exception cref="InvalidOperationException">如果实验包名无效</exception>
        public EditExperiment(JObject source, string address)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (string.IsNullOrWhiteSpace(address)) throw new ArgumentNullException(nameof(address));

            Name = source["name"]?.ToString() ?? "未知实验";
            Description = source["description"]?.ToString() ?? string.Empty;
            PackageName = source["experiment"]?.ToString() ?? string.Empty;
            SaveAddress = address;

            if (string.IsNullOrWhiteSpace(PackageName))
            {
                throw new InvalidOperationException("实验包名不能为空");
            }
        }

        /// <summary>
        /// 获取实验类型
        /// </summary>
        /// <returns>实验类型</returns>
        /// <summary>
        /// 获取实验类型
        /// </summary>
        /// <returns>实验类型</returns>
        public EditExperimentType GetEditExperimentType()
        {
            if (string.IsNullOrWhiteSpace(Name) &&
                string.IsNullOrWhiteSpace(Description) &&
                string.IsNullOrWhiteSpace(PackageName) &&
                string.IsNullOrWhiteSpace(SaveAddress))
            {
                return EditExperimentType.Undefined;
            }

            if (string.IsNullOrWhiteSpace(Name) &&
                string.IsNullOrWhiteSpace(Description) &&
                string.IsNullOrWhiteSpace(PackageName) &&
                !string.IsNullOrWhiteSpace(SaveAddress))
            {
                return EditExperimentType.AddressOnly;
            }

            return EditExperimentType.FullInformation;
        }

    }
}
