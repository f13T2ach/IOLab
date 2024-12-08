using IOLab.Core.Commands;
using IOLab.Core.Generic.Maker;
using IOLab.Core.Models;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace IOLab.Pages.ViewModel
{
    public class ExperimentMakerPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        // NotifyPropertyChanged Utility
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        EditExperiment Source;

        // 是否已经创建实验
        private bool _isExperimentCreated;
        public bool IsExperimentCreated
        {
            get => _isExperimentCreated;
            set
            {
                if (_isExperimentCreated != value)
                {
                    _isExperimentCreated = value;
                    OnPropertyChanged();
                }
            }
        }

        // 实验名称
        private string _experimentName;
        public string ExperimentName
        {
            get => _experimentName;
            set
            {
                if (_experimentName != value)
                {
                    _experimentName = value;
                    OnPropertyChanged();
                }
            }
        }

        // 步骤列表
        public ObservableCollection<StepItem> Steps { get; set; } = new ObservableCollection<StepItem>();

        public ObservableCollection<MonitorService> Services { get; set; } = new ObservableCollection<MonitorService>();

        // 当前选中的步骤
        private StepItem? _currentStep;
        public StepItem? CurrentStep
        {
            get => _currentStep;
            set
            {
                if (_currentStep != value)
                {
                    _currentStep = value;
                    OnPropertyChanged();
                }
            }
        }

        // 选中传感器
        private MonitorService _currentMonitor;
        public MonitorService CurrentMonitor
        {
            get => _currentMonitor;
            set
            {
                if (_currentMonitor != value)
                {
                    _currentMonitor = value;
                    OnPropertyChanged();
                }
            }
        }

        // 预选择传感器
        private MonitorService _selectAddMonitor;
        public MonitorService SelectAddMonitor
        {
            get => _selectAddMonitor;
            set
            {
                if (_selectAddMonitor != value)
                {
                    _selectAddMonitor = value;
                    OnPropertyChanged();
                }
            }
        }



        // 步数和检查点数
        public int StepCount => Steps.Count;
        public int CheckpointCount => CurrentStep == null ? 0 : CurrentStep.CurrentStepSensors.Count;

        // Commands
        public ICommand SaveCommand { get; }
        public ICommand SaveAsCommand { get; }
        public ICommand CloseEditorCommand { get; }
        public ICommand AddStepCommand { get; }
        public ICommand ApplyStepCommand { get; }
        public ICommand CloneStepCommand { get; }
        public ICommand DeleteStepCommand { get; }
        public ICommand AddCheckpointCommand { get; }
        public ICommand RemoveCheckpointCommand { get; }

        // Constructor
        public ExperimentMakerPageViewModel(EditExperiment source, JObject experiment = null)
        {

            foreach (var item in Bundles.LocalMonitorService)
            {
                Services.Add(item);
            }

            SaveCommand = new RelayCommand(SaveExperiment);                 // 保存实验
            SaveAsCommand = new RelayCommand(SaveExperimentAs);             // 另存为
            CloseEditorCommand = new RelayCommand(CloseEditor);             // 关闭编辑器
            AddStepCommand = new RelayCommand(AddStep);                     // 添加步骤
            ApplyStepCommand = new RelayCommand(ApplyCurrentStep);          // 应用
            CloneStepCommand = new RelayCommand(CloneCurrentStep);          // 克隆步骤
            DeleteStepCommand = new RelayCommand(DeleteCurrentStep);        // 删除步骤
            AddCheckpointCommand = new RelayCommand(AddCheckpoint);         // 添加检查点
            RemoveCheckpointCommand = new RelayCommand(RemoveCheckpoint);   // 移除检查点

            Source = source;


            IsExperimentCreated = true;
            ExperimentName = source.Name;
            Steps.Clear();
            CurrentStep = null;

            LoadExperiment(experiment); //加载
        }



        private void LoadExperiment(JObject experiment)
        {

        }



        private void SaveExperiment(object sender)
        {
            Export export = new Export();
            JObject value = export.ExperimentToJObject(Steps, Source);
        }

        private void SaveExperimentAs(object sender)
        {
            // 保存为副本逻辑
        }

        private void CloseEditor(object sender)
        {
            IsExperimentCreated = false;
        }

        private void AddStep(object sender)
        {
            var newStep = new StepItem
            {
                StepName = $"步骤 {Steps.Count + 1}",
                StepDescription = "新步骤描述"
            };
            Steps.Add(newStep);
            CurrentStep = newStep;
        }

        private void ApplyCurrentStep(object sender)
        {
            // 应用当前步骤逻辑
        }

        private void CloneCurrentStep(object sender)
        {
            if (CurrentStep != null)
            {
                var clonedStep = new StepItem
                {
                    StepName = $"{CurrentStep.StepName} - 副本",
                    StepDescription = CurrentStep.StepDescription
                };
                Steps.Add(clonedStep);
            }
        }

        private void DeleteCurrentStep(object sender)
        {
            if (CurrentStep != null)
            {
                int newIndex = -1;
                if (Steps.IndexOf(CurrentStep) - 1 >= 0)
                {
                    newIndex = Steps.IndexOf(CurrentStep) - 1;
                }
                Steps.Remove(CurrentStep);

                if (newIndex != -1)
                {
                    CurrentStep = Steps[newIndex];
                }
                else
                {
                    CurrentStep = null;
                }

            }
        }

        private void AddCheckpoint(object sender)
        {
            if (CurrentStep == null)
                return;
            CurrentStep.CurrentStepSensors.Add(SelectAddMonitor);
            OnPropertyChanged(nameof(CurrentStep.CurrentStepSensors));
        }

        private void RemoveCheckpoint(object sender)
        {
            if (CurrentStep == null || CurrentMonitor == null)
                return;
            CurrentStep.CurrentStepSensors.Remove(CurrentMonitor);
            OnPropertyChanged(nameof(CurrentStep.CurrentStepSensors));
        }
    }



}
