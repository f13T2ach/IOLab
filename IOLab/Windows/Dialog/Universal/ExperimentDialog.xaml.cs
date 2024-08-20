using iNKORE.UI.WPF.Modern.Controls;
using IOLab.Core.Assets;
using IOLab.Core.Models;
using System.Collections.Generic;

namespace IOLab.Windows.Dialog.Universal
{
    /// <summary>
    /// ExperimentDialog.xaml 的交互逻辑
    /// </summary>
    public partial class ExperimentDialog : ContentDialog
    {
        public string MyTitle { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public string Sensors { get; set; }
        public string Steps { get; set; }
        public ExperimentDialog(IOExperiment experiment)
        {
            InitializeComponent();
            MyTitle = experiment.Name;
            
            foreach(string item in experiment.Tags)
            {
                Subtitle += $"{item} ";
            }
            Subtitle ??= Text.AboutThisExperiment;

            Description = experiment.Description;

            foreach(MonitorService service in experiment.Parameters)
            {
                Sensors += $"{service.Name} ";
            }
            Sensors ??= Text.NoSensorNeeded;

            foreach(KeyValuePair<string,string> step in experiment.Content)
            {
                Steps += $"{step.Key}\n{step.Value}\n";
            }
            Steps ??= Text.NoDescriptions;
            Steps = Steps.Remove(Steps.LastIndexOf("\n"), 1);

            DataContext = this;
        }
    }
}
