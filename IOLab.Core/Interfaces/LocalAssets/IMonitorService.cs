namespace IOLab.Core.Interfaces.LocalAssets
{
    public interface IMonitorService // 监视器接口
    {
        string PortType { get; set; }
        string Name { get; set; }
        int Id { get; set; }
        string YRow { get; set; }
    }
}
