/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * WorkshopContentUI.xaml.cs is part of the IOLab project.
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
using IOLab.Core.Assets;
using IOLab.Core.Generic;
using IOLab.Core.Models;
using IOLab.Core.Network.Data;
using IOLab.Network;
using IOLab.Pages.ViewModel;

namespace IOLab.Windows.Dialog.Online
{
    /// <summary>
    /// WorkshopContentUI.xaml 的交互逻辑
    /// </summary>
    public partial class WorkshopContentUI : ContentDialog
    {
        static WorkshopContent host = new WorkshopContent();
        public WorkshopContentUI(WorkshopContent content)
        {
            InitializeComponent();
            host = content;
            Title = content.Title;
            AuthorNameText.Text = content.AuthorName;
            DescriptionsText.Text = content.Description;
            
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // 下载并保存
            string fileID = host.JsonFileID;
            if(InstanceBundle.UsingAuth == null)
            {
                return;
            }
            Workshop workshop = new Workshop(InstanceBundle.UsingAuth);
            var result = await workshop.DownloadWorkshopItemAsync(fileID, $".\\{FolderNames.AssetsFolder}\\{FolderNames.AssetsExperimentLocalFolder}\\");
            // 隐藏本窗口
            this.Hide();

            if (!result.Success)
            {
                new MainWindow().CreateDialog("无法在线实践", result.Message);
                return;
            }

            // 成功后，读取实验
            ReadAssets.ReadExperiments();

            foreach(var item in Bundles.LocalExperiments)
            {
                if (item.PackageName == result.Message) // 如果成功 result会返回实验包名
                {
                    // 实践
                    HomePageViewModel hmViewModel = new HomePageViewModel();
                    HomePageViewModel.InvokeReload();
                    hmViewModel.OnShowDetailDialog(item);
                    return;
                }
            }

            new MainWindow().CreateDialog("无法在线实践", "未找到已存储的实验");
            return;
        }
    }
}
