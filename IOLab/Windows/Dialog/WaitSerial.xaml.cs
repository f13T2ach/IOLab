/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * WaitSerial.xaml.cs is part of the IOLab project.
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

using iNKORE.UI.WPF.Modern.Controls;
using IOLab.Core.Config;
using System;
using System.Windows.Threading;

namespace IOLab.Windows.Dialog
{
    /// <summary>
    /// WaitSerial.xaml 的交互逻辑
    /// </summary>
    public partial class WaitSerial : ContentDialog
    {
        private DispatcherTimer _timer;
        private int _timeLeft;

        public WaitSerial(int index = 0, int sum = 0, string title = "正在等待回应")
        {

            InitializeComponent();
            if (sum == 0)
            {
                Title.Text = $"{title}";
            }
            else
            {
                Title.Text = $"{title} ({index}/{sum})";
            }
            StartCountdown(InternalConfig.OperationTimeout / 1000);  // 设置倒计时时长为5秒
        }

        private void StartCountdown(int seconds)
        {
            _timeLeft = seconds;
            Description.Text = $"操作还需 {_timeLeft} 秒";

            // 初始化 DispatcherTimer
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _timeLeft--;

            if (_timeLeft > 0)
            {
                // 更新倒计时显示
                Description.Text = $"操作还需 {_timeLeft} 秒";
            }
            else
            {
                _timer.Stop();
            }
        }
    }
}
