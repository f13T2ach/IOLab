/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * AnimatedTextSwitcher.xaml.cs is part of the IOLab project.
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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace IOLab.Resources.Control
{
    public partial class AnimatedTextSwitcher : UserControl
    {
        public static readonly DependencyProperty TextArrayProperty =
            DependencyProperty.Register("TextArray", typeof(string[]), typeof(AnimatedTextSwitcher),
                new PropertyMetadata(new string[] { "Default Text" }, OnTextArrayChanged));

        public static readonly DependencyProperty IntervalProperty =
            DependencyProperty.Register("Interval", typeof(TimeSpan), typeof(AnimatedTextSwitcher),
                new PropertyMetadata(TimeSpan.FromSeconds(2), OnIntervalChanged));

        private readonly DispatcherTimer _timer;
        private int _currentTextIndex = 0;

        public AnimatedTextSwitcher()
        {
            InitializeComponent();

            _timer = new DispatcherTimer();
            _timer.Tick += Timer_Tick;

            // Set the initial interval
            _timer.Interval = Interval;
        }

        public string[] TextArray
        {
            get { return (string[])GetValue(TextArrayProperty); }
            set { SetValue(TextArrayProperty, value); }
        }

        public TimeSpan Interval
        {
            get { return (TimeSpan)GetValue(IntervalProperty); }
            set { SetValue(IntervalProperty, value); }
        }

        private static void OnTextArrayChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AnimatedTextSwitcher animatedTextBlock && animatedTextBlock.TextArray.Length > 0)
            {
                animatedTextBlock._currentTextIndex = 0;
                animatedTextBlock.textBlock.Text = animatedTextBlock.TextArray[0];

                // 手动设置控件宽度为第一个文本的宽度
                animatedTextBlock.textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                animatedTextBlock.animatedUserControl.Width = animatedTextBlock.textBlock.DesiredSize.Width;

                // 启动计时器以开始显示后续的文本
                animatedTextBlock._timer.Start();
            }
        }

        private static void OnIntervalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AnimatedTextSwitcher animatedTextBlock && e.NewValue is TimeSpan newInterval)
            {
                animatedTextBlock._timer.Interval = newInterval;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // 开始渐变消失动画
            var fadeOutStoryboard = (Storyboard)FindResource("FadeOutStoryboard");

            fadeOutStoryboard.Completed -= FadeOutCompleted; // 移除之前的事件绑定，防止重复触发
            fadeOutStoryboard.Completed += FadeOutCompleted;

            fadeOutStoryboard.Begin();
        }

        private void FadeOutCompleted(object sender, EventArgs e)
        {
            // 更新文本
            _currentTextIndex = (_currentTextIndex + 1) % TextArray.Length;
            textBlock.Text = TextArray[_currentTextIndex];

            // 计算 TextBlock 的实际宽度并设置到 UserControl
            textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            double desiredWidth = textBlock.DesiredSize.Width;
            animatedUserControl.Width = desiredWidth;

            // 开始渐变出现动画
            var fadeInStoryboard = (Storyboard)FindResource("FadeInStoryboard");
            fadeInStoryboard.Begin();
        }
    }
}