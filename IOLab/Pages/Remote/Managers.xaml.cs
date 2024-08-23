/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * Managers.xaml.cs is part of the IOLab project.
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

﻿using IOLab.Pages.Remote.ViewModel;
using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace IOLab.Pages.Remote
{
    /// <summary>
    /// Manager.xaml 的交互逻辑
    /// </summary>
    public partial class Managers : Page
    {
        public Managers()
        {
            InitializeComponent();
            ManagersViewModel viewModel = new ManagersViewModel();
            this.DataContext = viewModel;

            MyInfoBar.ForceStop();
            MyInfoBar.ShowWaringBar("采集仪器：连接成功");
            // InitInformation();
        }

        private void InitInformation()
        {
            // Header.Text = DeviceName.TextContent;
            // DeviceNameText.Text = DeviceName.TextContent;

        }

        /**
        private async void Rename_Click(object sender, RoutedEventArgs e)
        {
            DeviceName dn = new DeviceName();
            NameEdit edit = new NameEdit(dn);

            string? originName = DeviceName.TextContent;
            var result = await edit.ShowAsync();
            if(result == iNKORE.UI.WPF.Modern.Controls.ContentDialogResult.Secondary)
            {
                DeviceName.TextContent = originName;
                return;
            }

            // 处理更名业务
            InitInformation(); //成功后变更
        }**/
    }
}
