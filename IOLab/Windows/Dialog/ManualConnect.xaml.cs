/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * ManualConnect.xaml.cs is part of the IOLab project.
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

﻿using iNKORE.UI.WPF.Modern.Controls;
using System.Windows.Controls;

namespace IOLab.Windows.Dialog
{
    /// <summary>
    /// ManualConnect.xaml 的交互逻辑
    /// </summary>
    public partial class ManualConnect : ContentDialog
    {
        public ManualConnect()
        {
            InitializeComponent();

            ViewModel.ManualConnectViewModel vm = new ViewModel.ManualConnectViewModel();
            ViewModel.ManualConnectViewModel.isConfirmed = false;
            this.DataContext = vm;


        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IsPrimaryButtonEnabled = true;

        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            ViewModel.ManualConnectViewModel.isConfirmed = true;
        }
    }
}
