/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * RemotePage.xaml.cs is part of the IOLab project.
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

using iNKORE.UI.WPF.Modern;
using IOLab.Core.Generic;
using IOLab.Pages.Remote;
using IOLab.Pages.ViewModel;
using System;
using System.Windows.Media.Imaging;
using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace IOLab.Pages
{
    /// <summary>
    /// Interaction logic for RemotePage.xaml
    /// </summary>
    public partial class RemotePage : Page
    {
        NoRemoteTarget PageRemote_NoRemoteTarget = new NoRemoteTarget();
        Managers PageRemote_Managers = new Managers();
        public RemotePage()
        {
            InitializeComponent();
            Preparations();
            this.DataContext = new RemotePageViewModel();
        }

        private void Preparations()
        {
            if (InstanceBundle.IsConnected)
            {
                ViewField.Navigate(PageRemote_Managers);
            }
            else
            {
                ViewField.Navigate(PageRemote_NoRemoteTarget);
            }

            ShallowShadow.Source = BitmapFrame.Create(
                new Uri(Static.Theme.themeMode == ApplicationTheme.Dark ? @"pack://application:,,,/Resources/Images/Tiny/ShallowShadowDark.png" : "pack://application:,,,/Resources/Images/Tiny/ShallowShadow.png"),
                BitmapCreateOptions.None,
                BitmapCacheOption.Default);
        }
    }
}
