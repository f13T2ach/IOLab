/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * HomePageViewModel.cs is part of the IOLab project.
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
using IOLab.Core.Commands;
using IOLab.Core.Generic;
using IOLab.Core.Models;
using IOLab.Core.Models.Records;
using IOLab.Windows.Dialog.Universal;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace IOLab.Pages.ViewModel
{
    internal class HomePageViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Schema> _recordsSchema;
        public ObservableCollection<Schema> RecordsSchema
        {
            get => _recordsSchema;
            set
            {
                _recordsSchema = value;
                OnPropertyChanged(nameof(RecordsSchema));
            }
        }
        private ObservableCollection<IOExperiment> _ioExperiments;
        public ObservableCollection<IOExperiment> IOExperiments
        {
            get => _ioExperiments;
            set
            {
                _ioExperiments = value;
                OnPropertyChanged(nameof(IOExperiments));
            }
        }



        public ICommand OnShowDetailDialogCommand { get; set; }
        public Visibility CanShowNoRecordText { get => RecordsSchema.Count == 0 ? Visibility.Visible : Visibility.Hidden; protected set { } }
        public HomePageViewModel()
        {
            RecordsSchema = new ObservableCollection<Schema>();
            foreach (Schema item in Bundles.LocalRecords)
            {
                RecordsSchema.Add(item);
            }
            // 读入实验
            ResetExperiments();



            OnShowDetailDialogCommand = new RelayCommand(OnShowDetailDialog);
        }

        // 统计数据

        public string LocalExperimentCount { get => Bundles.LocalExperiments.Count > 99 ? "99" : Bundles.LocalExperiments.Count.ToString(); set { } }
        public string LocalRecordCount { get => Bundles.LocalRecords.Count > 99 ? "99" : Bundles.LocalRecords.Count.ToString(); set { } }
        public string LocalServicesCount { get => Bundles.LocalMonitorService.Count > 99 ? "99" : Bundles.LocalMonitorService.Count.ToString(); set { } }

        public event PropertyChangedEventHandler PropertyChanged;

        // 事件
        public event Action OnTryPractiseSuccess;
        private void ResetExperiments()
        {
            IOExperiments = new ObservableCollection<IOExperiment>();
            foreach (IOExperiment item in Bundles.LocalExperiments)
            {
                IOExperiments.Add(item);
            }
        }

        public async void OnShowDetailDialog(object sender)
        {
            IOExperiment? experiment = sender as IOExperiment;
            if (experiment == null)
            {
                return;
            }
            var result = await new ExperimentDialog(experiment).ShowAsync();
            if (result != ContentDialogResult.Primary)
            {
                return;
            }
            TryPractise(experiment);
        }

        private async void TryPractise(IOExperiment experiment)
        {
            // 如果有表正在运行
            if (InstanceBundle.ChartTables != null)
            {
                if (InstanceBundle.ChartTables.Count > 0)
                {
                    ShowBannedDialog($"请处理正在运行的 {InstanceBundle.ChartTables.Count} 个监视器");
                    return;
                }
            }
            // 如果没有连接
            if (InstanceBundle.connectTool == null)
            {
                ShowBannedDialog($"未连接设备或者此设备无效");
                return;
            }
            // 如果已经在实验中
            if (InstanceBundle.UsingExperiment != null)
            {
                ContentDialog Dialog = new ContentDialog()
                {
                    Title = "无法实践此实验",
                    Content = "尽管没有运行中的图表，你仍在进行实验\n继续则覆盖当前实验",
                    PrimaryButtonText = "继续",
                    SecondaryButtonText = "取消"
                };
                if (await Dialog.ShowAsync() == ContentDialogResult.Secondary)
                {
                    return;
                }
            }
            InstanceBundle.UsingExperiment = experiment;
            OnTryPractiseSuccess?.Invoke();
        }


        private async void ShowBannedDialog(string text)
        {
            ContentDialog Dialog;
            Dialog = new ContentDialog()
            {
                Title = "无法实践此实验",
                Content = text,
                CloseButtonText = "好"
            };
            await Dialog.ShowAsync();
        }

        public void OnSearchTextChanged(string SearchText)
        {
            ResetExperiments();
            if (SearchText == "" || SearchText == "[" || SearchText == "]")
            {
                return;
            }
            // 防止直接复制指针
            ObservableCollection<IOExperiment> unmodifiableCollection = new ObservableCollection<IOExperiment>();
            foreach (IOExperiment experiment in IOExperiments)
            {
                unmodifiableCollection.Add(experiment);
            }
            foreach (IOExperiment experiment in IOExperiments)
            {
                if (!experiment.RealName.Contains(SearchText))
                {
                    unmodifiableCollection.Remove(experiment);
                }

            }
            IOExperiments = unmodifiableCollection;

        }
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
