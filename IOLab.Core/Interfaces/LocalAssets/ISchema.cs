using IOLab.Core.Models;

namespace IOLab.Core.Interfaces.LocalAssets
{
    public interface ISchema
    {
        string Name { get; set; }
        string Description { get; set; }
        List<MonitorService> Parameters { get; set; }
        IOExperiment Experiment { get; set; }
        List<string> RecordJSONData { get; set; }
        long Time { get; set; }
    }
}

