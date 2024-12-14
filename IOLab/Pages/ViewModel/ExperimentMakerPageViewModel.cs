using IOLab.Core.Assets;
using IOLab.Core.Commands;
using IOLab.Core.Generic.Maker;
using IOLab.Core.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using static IOLab.Core.Models.IOExperiment;

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
        public ExperimentMakerPageViewModel(EditExperiment? source = null, JObject? experiment = null)
        {
            if(source == null && experiment == null)
                throw new InvalidOperationException();
            // 遍历添加传感器节点
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


        /// <summary>
        /// 读取实验显示到 UI
        /// </summary>
        /// <param name="experiment"></param>
        private LoadSourceResult LoadExperiment(JObject experiment)
        {
            try
            {
                // 实例化 EditExperiment 并检查其类型
                EditExperiment editExperiment;
                try
                {
                    editExperiment = new EditExperiment(experiment, Source.SaveAddress);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error initializing EditExperiment: {ex.Message}");
                    return LoadSourceResult.BadJSON;
                }

                if (editExperiment.GetEditExperimentType() == EditExperimentType.Undefined)
                    return LoadSourceResult.BadJSON;

                // 使用 EditExperiment 提取基本信息
                ExperimentName = editExperiment.Name;

                // 加载标签
                //Tags.Clear();
                //if (experiment["tags"] is JArray tagsArray)
                //{
                //    foreach (var tag in tagsArray)
                //    {
                //        Tags.Add(tag.ToString());
                //    }
                //}

                // 加载服务列表
                Services.Clear();
                if (experiment["services"] is JArray servicesArray)
                {
                    foreach (var serviceToken in servicesArray)
                    {

                        int? serviceId = (int)serviceToken;
                        if (serviceId == null)
                            continue;

                        MonitorService? service = Bundles.FindMonitorServiceById(serviceId);
                        if (service == null)
                            continue;


                        Services.Add(service);
                    }
                }

                // 加载实验步骤
                Steps.Clear();
                if (experiment["steps"] is JObject stepsObject)
                {
                    foreach (var step in stepsObject)
                    {
                        Steps.Add(new StepItem
                        {
                            StepName = step.Key,
                            StepDescription = step.Value.ToString()
                        });
                    }
                }

                // 加载检查点
                if (experiment["checkpoints"] is JArray checkpointsArray)
                {
                    foreach (var checkpoint in checkpointsArray)
                    {
                        var checkpointName = checkpoint["name"]?.ToString();
                        var matchingStep = Steps.FirstOrDefault(s => s.StepName.Contains(checkpointName));
                        if (matchingStep != null)
                        {
                            if (checkpoint["freeze"] is JArray freezeArray)
                            {
                                foreach (var freezeToken in freezeArray)
                                {
                                    int? freezeId = (int)freezeToken;
                                    if (freezeId == null)
                                        continue;

                                    MonitorService? service = Bundles.FindMonitorServiceById(freezeId);
                                    if (service == null)
                                        continue;

                                    matchingStep.CurrentStepSensors.Add(service);
                                    
                                }
                            }
                            // TODO 拆分两个需求
                            if (checkpoint["require"] is JArray requireArray)
                            {
                                foreach (var requireToken in requireArray)
                                {
                                    int? requireId = (int)requireToken;
                                    if (requireId == null)
                                        continue;

                                    MonitorService? service = Bundles.FindMonitorServiceById(requireId);
                                    if (service == null)
                                        continue;

                                    if(!matchingStep.CurrentStepSensors.Contains(service))
                                        matchingStep.CurrentStepSensors.Add(service);
                                }
                            }
                        }
                    }
                }
                return LoadSourceResult.Success;
            }
            catch (Exception ex)
            {
                // 异常处理
                Console.WriteLine($"Error loading experiment: {ex.Message}");
                return LoadSourceResult.UnknownError;
            }
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

    public enum LoadSourceResult
    {
        Success,
        UnknownError,
        FileAuthority,
        BadJSON,
    }


}
