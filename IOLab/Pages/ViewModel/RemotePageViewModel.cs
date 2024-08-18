using System;
using System.ComponentModel;
using System.Windows.Threading;

namespace IOLab.Pages.ViewModel
{
    class RemotePageViewModel : INotifyPropertyChanged
    {
        private string _currentTime;
        private DispatcherTimer _timer;

        //构造函数
        public RemotePageViewModel()
        {
            InitTimer();
        }

        private void InitTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);//更新频率设为每秒
            _timer.Tick += TimeTick;
            _timer.Start();
            UpdateTime();//初始化
        }

        private void TimeTick(object sender, EventArgs e)
        {
            UpdateTime();
        }

        private void UpdateTime()
        {
            CurrentTime = DateTime.Now.ToString("HH:mm:ss");
        }

        public string CurrentTime
        {
            get { return _currentTime; }
            set
            {
                _currentTime = value;
                OnPropertyChanged(nameof(CurrentTime));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
