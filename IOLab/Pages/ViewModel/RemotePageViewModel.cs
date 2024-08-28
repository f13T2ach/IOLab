/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * RemotePageViewModel.cs is part of the IOLab project.
 *
 * IOLab is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * IOLab is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with IOLab. If not, see <https://www.gnu.org/licenses/>.
 */

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
