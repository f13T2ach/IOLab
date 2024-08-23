/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * Cover.xaml.cs is part of the IOLab project.
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

﻿using IOLab.Pages.Appreciations.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace IOLab.Pages.Appreciations
{
    /// <summary>
    /// Cover.xaml 的交互逻辑
    /// </summary>
    public partial class Cover : Page
    {
        public Cover()
        {
            InitializeComponent();
            CoverViewModel viewModel = new CoverViewModel();
            CoverViewModel.ProgressValue = 0;
            this.DataContext = viewModel;
        }

        private DoubleAnimation GetAnimation(double from, double to, double time, double amplitude = 0.3)
        {
            DoubleAnimation animation = new DoubleAnimation()
            {
                From = from,
                To = to,
                Duration = TimeSpan.FromSeconds(time),
                EasingFunction = new BackEase()
                {
                    Amplitude = amplitude,
                    EasingMode = EasingMode.EaseOut,
                },
            };
            return animation;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Progress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
    }


}
