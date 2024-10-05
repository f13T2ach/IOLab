using iNKORE.UI.WPF.DragDrop;
using iNKORE.UI.WPF.Modern.Controls;
using IOLab.Commands;
using IOLab.Core.Assets;
using IOLab.Core.Generic;
using IOLab.Core.Models;
using IOLab.Windows.Dialog.Universal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace IOLab.Resources.Control.ViewModel
{
    internal class ExperimentAssistanceViewModel : INotifyPropertyChanged
    {
        
        private IOExperiment current;

        private string _experimentName = Text.NoItemSelected;

        private int newestStepId = 0;

        private bool absolveStep = false;
        public bool _absolveStep
        {
            get => absolveStep; set
            {
                absolveStep = value;
                OnPropertyChanged(nameof(_absolveStep));
                OnPropertyChanged(nameof(AbsolveStepVisibility));
            }
        }

        public Visibility AbsolveStepVisibility
        {
            get
            {
                if (absolveStep)
                    return Visibility.Visible;
                else
                    return Visibility.Hidden;
            }
            set
            {

            }
        }

        public string ExperimentName
        {
            get => _experimentName; set
            {
                _experimentName = value;
                OnPropertyChanged(nameof(ExperimentName));
            }
        }

        private string _stepName = Text.NoStepExists;
        public string StepName
        {
            get => _stepName; set
            {
                _stepName = value;
                OnPropertyChanged(nameof(StepName));
            }
        }

        private string _stepDescription = Text.NoStepExistsExplanation;
        public string StepDescription
        {
            get => _stepDescription; set
            {
                _stepDescription = value;
                OnPropertyChanged(nameof(StepDescription));
            }
        }


        // 步骤列表
        private List<KeyValuePair<string, string>> steps = new List<KeyValuePair<string, string>>();
        private List<IOExperiment.Checkpoint> checkpoints = new List<IOExperiment.Checkpoint>();

        private bool _enableForNextStep;

        public bool EnableForNextStep
        {
            get => _enableForNextStep; set
            {
                _enableForNextStep = value;
                OnPropertyChanged(nameof(EnableForNextStep));
            }
        }

        private int stepIndex = 0;

        // RelayCommand 实现
        public ICommand NextStepButtonClicked { get; set; }

        public ICommand PreviousStepButtonClicked { get; set; }

        public ICommand ReleaseExperimentButtonClicked { get; set; }
        public ExperimentAssistanceViewModel()
        {

            current = InstanceBundle.UsingExperiment;
            InstanceBundle.UsingExperimentChanged += OnUsingExperimentChanged;
            InstanceBundle.ChartTablesUpdated += ValidateStep; // Add事件不能由此触发！

            NextStepButtonClicked = new RelayCommand(OnNextStepButtonClicked);
            PreviousStepButtonClicked = new RelayCommand(OnPreviousStepButtonClicked);
            ReleaseExperimentButtonClicked = new RelayCommand(ReleaseExperiment);
            ReadAll();
        }

        private void OnUsingExperimentChanged(IOExperiment experiment)
        {
            steps.Clear();
            checkpoints.Clear();
            newestStepId = 0;
            absolveStep = false;

            if (current == experiment) return;
            if (experiment == null)
            {
                StepName = Text.NoStepExists;
                StepDescription = Text.NoStepExistsExplanation;
                ExperimentName = Text.NoItemSelected;
            };
            current = InstanceBundle.UsingExperiment;
            ReadAll();
        }

        private void ReadAll()
        {
            if (current == null) return;
            ExperimentName = current.Name;
            foreach (KeyValuePair<string, string> pair in current.Content)
            {
                steps.Add(pair);
            }
            foreach (IOExperiment.Checkpoint checkpoint in current.Checkpoints)
            {
                checkpoints.Add(checkpoint);
            }
            stepIndex = -1;
            UpdateStep();
        }

        private async void UpdateStep()
        {
            if (stepIndex >= 0)
            {
                FreezeStep();   // 冻结（第一步是不用冻结上一步的）
            }
            stepIndex++;

            if(newestStepId == stepIndex)
            {
                _absolveStep = false;
            }
            newestStepId = stepIndex; // 更新最新步骤

            if (stepIndex >= steps.Count)
            {
                ExperimentCompleted();
                return;
            }
            StepName = steps[stepIndex].Key;
            StepDescription = steps[stepIndex].Value;
            ValidateStep();     // 检测
            // 阻止继续下一步
        }

        /// <summary>
        /// 解除（结束）实验
        /// </summary>
        /// <param name="sender"></param>
        private async void ReleaseExperiment(object sender)
        {
            // 如果不是null就问问
            if(sender != null)
            {
                var cd = new ContentDialog
                {
                    Title = Text.ReleaseExperiment,
                    Content = Text.ReleaseExperiment_Hint,
                    PrimaryButtonText = "继续",
                    SecondaryButtonText = "取消",
                    DefaultButton = ContentDialogButton.Primary,
                };
                if (await cd.ShowAsync() != ContentDialogResult.Primary) return;
            }
            ActionBundle.DoPauseOrStart(true);
            InstanceBundle.UsingExperiment = null;
        }

        /// <summary>
        /// 实验完成
        /// </summary>
        private async void ExperimentCompleted()
        {
            // 显示完成页面
            ExperimentCompleted ec = new ExperimentCompleted(current);
            await ec.ShowAsync();
            ReleaseExperiment(null);
            return;
        }


        private async void PreviousStep()
        {
            if (stepIndex - 1 < 0) return;

            var dialog = new ContentDialog()
            {
                Title = "确定回到上一步吗",
                Content = "本步骤产生的表会被归档，已归档的表不能继续接受数据",
                PrimaryButtonText = "继续",
                SecondaryButtonText = "取消"
            };
            if(await dialog.ShowAsync() != ContentDialogResult.Primary)
            {
                return;
            }
            FreezeStep(true); // 归档
            stepIndex--;
            

            StepName = steps[stepIndex].Key;
            StepDescription = steps[stepIndex].Value;
            ValidateStep();     // 检测
        }


        private void ValidateStep()
        {
            bool flag = false;
            if (current == null) return;
            if(stepIndex < newestStepId) {
                _absolveStep = true;// 本步骤为先前的步骤
                EnableForNextStep = true;
                return;
            }

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
            // TODO 支持单步骤重复传感器
            if (checkpoint.ChartList.Count > 0)
            {
                flag = true;
                foreach (int item in checkpoint.ChartList)
                {
                    bool loopFlag = false;
                    foreach (ChartTableModel chart in InstanceBundle.ChartTables)
                    {
                        if (chart.ChartServicer.Id == item && chart.isFrozen == false) 
                        {
                            loopFlag = true;
                            break;
                        }
                    }
                    if (loopFlag == false)
                    {
                        flag = false;
                        break;
                    }
                }
            }
            else
            {
                flag = true;
            }


            EnableForNextStep = flag;
        }

        /// <summary>
        /// 冻结当前freeze列表中的表（含之前所有步骤）
        /// </summary>
        /// <param name="gentle">为false时直接删除这个表</param>
        private void FreezeStep(bool gentle = true)
        {
            if (current == null) return;
            IOExperiment.Checkpoint? nullableCheckpoint = FindCheckpointByName(steps[stepIndex].Key);

            if (nullableCheckpoint == null) return;
            IOExperiment.Checkpoint checkpoint = (IOExperiment.Checkpoint)nullableCheckpoint;
            // 检查
            if (checkpoint.FrozenList.Count > 0)
            {
                if (!gentle) // 删除表
                {
                    List<ChartTableModel> deleteModels = new List<ChartTableModel>();
                    foreach (int item in checkpoint.FrozenList)
                    {
                        foreach (ChartTableModel chart in InstanceBundle.ChartTables)
                        {
                            if (chart.ChartServicer.Id == item)
                            {
                                deleteModels.Add(chart);
                            }
                        }
                    }
                    // 集合取差集
                    var exceptResult = InstanceBundle.ChartTables.Except(deleteModels);
                    InstanceBundle.ChartTables = new ObservableCollection<ChartTableModel>(exceptResult);
                }
                else // 归档表
                {
                    foreach (int item in checkpoint.FrozenList)
                    {
                        foreach (ChartTableModel chart in InstanceBundle.ChartTables)
                        {
                            if (chart.ChartServicer.Id == item)
                            {
                                chart.isFrozen = true; // 标记为已归档
                                chart.Name += Text.FrozenChart;
                            }
                        }
                    }
                }
            }
        }

        

        private IOExperiment.Checkpoint? FindCheckpointByName(string name)
        {
            foreach (IOExperiment.Checkpoint checkpoint in checkpoints)
            {
                if (checkpoint.Name == name)
                {
                    return checkpoint;
                }
            }
            return null;
        }

        public void OnNextStepButtonClicked(object sender)
        {
            if (current == null) return;
            UpdateStep();
        }

        public void OnPreviousStepButtonClicked(object sender)
        {
            if (current == null) return;
            PreviousStep();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
