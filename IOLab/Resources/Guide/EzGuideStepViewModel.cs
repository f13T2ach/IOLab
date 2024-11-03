// 文件出处 https://www.bilibili.com/read/cv39493164 以上版权声明为自动生成 此文件中不具备效力
using IOLab.Commands;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace IOLab.Resources.Guide
{
    public class EzGuideStepViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private string _titleString;
        private string _descriptString;
        public string TitleString
        {
            get => _titleString;
            set
            {
                if (_titleString != value)
                {
                    _titleString = value;
                    OnPropertyChanged(nameof(TitleString)); // 触发 PropertyChanged 事件
                }
            }
        }
        public string DescriptString
        {
            get => _descriptString;
            set
            {
                if (_descriptString != value)
                {
                    _descriptString = value;
                    OnPropertyChanged(nameof(DescriptString)); // 触发 PropertyChanged 事件
                }
            }
        }
        public ICommand NextStepCommand { get; set; }
        public ICommand SkipCommand { get; set; }
        public EzGuideStepViewModel(Action nextStepAction, Action skipAction)
        {
            // 设置初始值
            TitleString = "引导标题";
            DescriptString = "这是引导的描述信息。";

            // 初始化命令
            NextStepCommand = new RelayCommand((p) => nextStepAction?.Invoke());
            SkipCommand = new RelayCommand((p) => skipAction?.Invoke());
        }
    }
}
