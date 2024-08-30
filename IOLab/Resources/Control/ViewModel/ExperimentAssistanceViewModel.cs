using IOLab.Core.Generic;
using IOLab.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IOLab.Resources.Control.ViewModel
{
    internal class ExperimentAssistanceViewModel : INotifyPropertyChanged
    {
        
        private IOExperiment current;
        public string ExperimentName { get => current == null?"未选择实验":current.Name;private set { } }
        private string _stepName = "无步骤";
        public string StepName { get => _stepName; set
            {
                _stepName = value;
                OnPropertyChanged(nameof(StepName));
            } 
        }

        private string _stepDescription = "你正在自由实验，实践一个实验，小助手才可帮你";
        public string StepDescription
        {
            get => _stepDescription; set
            {
                _stepName = value;
                OnPropertyChanged(nameof(StepDescription));
            }
        }
        // 步骤列表
        private List<KeyValuePair<string,string>> steps = new List<KeyValuePair<string,string>>();
        private List<IOExperiment.Checkpoint> checkpoints = new List<IOExperiment.Checkpoint>();

        private bool _enableForNextStep;

        public bool EnableForNextStep { get => _enableForNextStep; set {
                _enableForNextStep = value;
                OnPropertyChanged(nameof(EnableForNextStep));
            }
        }

        private int stepIndex = 0;
        public ExperimentAssistanceViewModel()
        {

            current = InstanceBundle.UsingExperiment;
            InstanceBundle.UsingExperimentChanged += OnUsingExperimentChanged;
            InstanceBundle.ChartTablesUpdated += ValidateStep; // Add事件不能由此触发！
            ReadAll(); 
        }

        private void OnUsingExperimentChanged(IOExperiment experiment)
        {
            steps.Clear();
            checkpoints.Clear();
            if (current == experiment) return;
            if (experiment == null) return;
            current = InstanceBundle.UsingExperiment;
            ReadAll();
        }

        private void ReadAll()
        {
            

            if (current == null) return;
            foreach(KeyValuePair<string,string> pair in current.Content)
            {
                steps.Add(pair);
            }
            foreach(IOExperiment.Checkpoint checkpoint in current.Checkpoints)
            {
                checkpoints.Add(checkpoint);
            }
            stepIndex = -1;
            UpdateStep();
        }

        private void UpdateStep()
        {
            if (stepIndex >= 0) {
                FreezeStep();   // 冻结（第一步是不用冻结上一步的）
            }
            stepIndex++;
            _stepName = steps[stepIndex].Key;
            _stepDescription = steps[stepIndex].Value;
            OnPropertyChanged(nameof(StepDescription));
            OnPropertyChanged(nameof(StepName));
            ValidateStep();     // 检测
        }

        private void ValidateStep()
        {
            bool flag = false;
            if (current == null) return;

            IOExperiment.Checkpoint? nullableCheckpoint = FindCheckpointByName(steps[stepIndex].Key);

            if (nullableCheckpoint == null)
            {
                // 这一条不需要检查
                flag = true;
                EnableForNextStep = flag;
                return;
            }
            IOExperiment.Checkpoint checkpoint = (IOExperiment.Checkpoint)nullableCheckpoint;
            // 检查
            if (checkpoint.ChartList.Count > 0)
            {
                flag = true;
                foreach (int item in checkpoint.ChartList)
                {
                    bool loopFlag = false;
                    foreach (ChartTableModel chart in InstanceBundle.ChartTables)
                    {
                        if (chart.ChartServicer.Id == item)
                        {
                            loopFlag = true;
                            break;
                        }
                    }
                    if(loopFlag == false)
                    {
                        flag = false;
                        break;
                    }
                }
            }
            
            EnableForNextStep = flag;
        }

        private void FreezeStep()
        {
            if (current == null) return;
            IOExperiment.Checkpoint? nullableCheckpoint = FindCheckpointByName(steps[stepIndex].Key);

            if (nullableCheckpoint == null) return;
            IOExperiment.Checkpoint checkpoint = (IOExperiment.Checkpoint)nullableCheckpoint;
            // 检查
            if (checkpoint.FrozenList.Count > 0)
            {
                foreach (int item in checkpoint.FrozenList) {
                    foreach (ChartTableModel chart in InstanceBundle.ChartTables)
                    {
                        if (chart.ChartServicer.Id == item)
                        {
                            chart.isFrozen = true;
                        }
                    }
                }
            }
        }

        private IOExperiment.Checkpoint? FindCheckpointByName(string name)
        {
            foreach(IOExperiment.Checkpoint checkpoint in checkpoints)
            {
                if(checkpoint.Name == name)
                {
                    return checkpoint;
                }
            }
            return null;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
