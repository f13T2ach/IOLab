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

using iNKORE.UI.WPF.Modern.Controls;
using IOLab.Commands;
using IOLab.Core.Assets;
using IOLab.Core.Interfaces.Threads;
using IOLab.Core.Serial;
using IOLab.Windows.Dialog;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace IOLab.Pages.Remote.ViewModel
{
    class ManagersViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand DisconnectCommand { get; set; }

        public string DeviceName { get => InstanceBundle.IsConnected ? InstanceBundle.connectTool.Device.DeviceName : ""; private set { } }
        public event EventHandler ChangePageEvent;
        public ManagersViewModel()
        {
            DisconnectCommand = new RelayCommand(OnUIDisconnectDevice);
        }

        public async void OnUIDisconnectDevice(object sender)
        {
            bool directly = false;

            if (sender == null)
            {
                var cd = new ContentDialog
                {
                    Title = Text.DisconnectDialog_SureToDisconnect,
                    Content = Text.DisconnectDialog_AlertText,
                    PrimaryButtonText = "继续",
                    SecondaryButtonText = "取消",
                    DefaultButton = ContentDialogButton.Primary,
                };
                var result = await cd.ShowAsync();
                if (result != ContentDialogResult.Primary)
                {
                    return;
                }
                ChangePageEvent?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                var cd = new ContentDialog
                {
                    Title = Text.DisconnectDialog_Disconnected,
                    Content = Text.DisconnectDialog_Hint,
                    PrimaryButtonText = "好",
                    DefaultButton = ContentDialogButton.Primary,
                };
                try
                {
                    await cd.ShowAsync();
                }
                catch
                {
                    directly = true; // 无法再显示对话框
                }
            }

            if (!InstanceBundle.IsConnected)
            {
                return;
            }

            // 恢复标记
            InstanceBundle.IsConnected = false;
            // 清除所有队列
            foreach (ITaskAnnounce announce in InstanceBundle.list)
            {
                announce.StopTask();
            }
            InstanceBundle.list.Clear();
            // 重置连接器
            InstanceBundle.connectTool.CloseComport();
            InstanceBundle.connectTool = null;

            // 强制要求保存所有表
            if (InstanceBundle.ChartTables != null)
            {
                if (InstanceBundle.ChartTables.Count > 0 && !directly)
                {
                    await new SaveRecords(2).ShowAsync();
                    InstanceBundle.ChartTables.Clear();
                }

            }

            // 如果无法显示对话框
            if (directly)
            {
                if (iNKORE.UI.WPF.Modern.Controls.MessageBox.Show(Text.DisconnectDialog_HintFailed,
                    Text.DisconnectDialog_Disconnected,
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (InstanceBundle.ChartTables != null)
                    {
                        if (InstanceBundle.ChartTables.Count > 0)
                        {
                            // await new SaveRecords(2).ShowAsync();
                            // 保存
                        }
                        InstanceBundle.ChartTables.Clear();
                    }
                }
            }
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
