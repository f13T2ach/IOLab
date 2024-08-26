/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * ManagersViewModel.cs is part of the IOLab project.
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

using IOLab.Commands;
using IOLab.Core.Serial;
using System.ComponentModel;
using System.Windows.Input;

namespace IOLab.Pages.Remote.ViewModel
{
    class ManagersViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand DisconnectCommand { get; set; }

        public string DeviceName { get => InstanceBundle.IsConnected ? InstanceBundle.connectTool.Device.DeviceName : ""; private set { } }

        public ManagersViewModel()
        {
            DisconnectCommand = new RelayCommand(OnUIDisconnectDevice);
        }

        public void OnUIDisconnectDevice(object sender)
        {

        }

        private void OnPropertyChanged(string name = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }


    }
}
