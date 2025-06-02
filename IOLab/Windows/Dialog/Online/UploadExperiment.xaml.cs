/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * UploadExperiment.xaml.cs is part of the IOLab project.
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
using IOLab.Core.Generic;
using IOLab.Core.Generic.Maker;
using IOLab.Core.Util.FilesBehavior;
using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace IOLab.Windows.Dialog.Online
{
    /// <summary>
    /// UploadExperiment.xaml 的交互逻辑
    /// </summary>
    public partial class UploadExperiment : ContentDialog
    {
        private string PickedFilePath;
        private string ContentTitle;
        private string ContentDescriptions;
        public UploadExperiment()
        {
            InitializeComponent();

        }

        private void CloseButton_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Hide();
        }

        private void PickFile_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var dlg = new CommonOpenFileDialog
            {
                IsFolderPicker = false,
                InitialDirectory = System.IO.Directory.GetCurrentDirectory(),
                Title = "选择实验包",
            };

            // 添加文件类型筛选器，只允许选择 .json 文件
            dlg.Filters.Add(new CommonFileDialogFilter("实验 JSON 文件", "*.json"));

            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                // 获取选中的文件路径
                string selectedFile = dlg.FileName;

                EditExperiment source = new EditExperiment();
                source.SaveAddress = selectedFile;
                string? raw = Read.ReadFileUTF8(selectedFile);

                if (raw == null)
                {
                    // 读取失败
                    ExperimentWarning.Text = "无法打开实验";
                    return;
                }

                JObject json;
                try
                {
                    json = JObject.Parse(raw);
                }
                catch (Exception ex)
                {
                    ExperimentWarning.Text = "无法解析实验";
                    return;
                }

                if (((bool?)json["system"]) == true)
                {
                    ExperimentWarning.Text = "该实验是系统自带实验，不能上传";
                    return;
                }

                if (!File.Exists(selectedFile))
                {
                    ExperimentWarning.Text = "实验不存在";
                    return;
                }

                // 当包名和文件名不一致时 警告用户
                if (json["experiment"]?.ToString() != Path.GetFileNameWithoutExtension(Read.GetLastLevelName(selectedFile)))
                {
                    ExperimentWarning.Text = "未遵循设计规范或者选中的不是实验包";
                    return;
                }

                PickedFilePath = selectedFile;
                ExperimentNameText.Text = json["name"]?.ToString();


            }
        }

        private void SubmitExperimentButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {

            if (string.IsNullOrEmpty(PickedFilePath))
            {
                ExperimentWarning.Text = "未选中实验";
                return;
            }
            ContentTitle = ContentTitleInput.Text;
            ContentDescriptions = ContentDescriptionInput.Text;
            if (string.IsNullOrEmpty(ContentTitle.Trim()) || string.IsNullOrEmpty(ContentDescriptions.Trim()))
            {
                ExperimentWarning.Text = "内容不可为空";
                return;
            }
            if (ContentTitle.Length <= 5 || ContentTitle.Length <= 5)
            {
                ExperimentWarning.Text = "内容过短";
                return;
            }

            if (InstanceBundle.UsingAuth == null)
            {
                ExperimentWarning.Text = "还没有登录";
                return;
            }

            //WaitSerial waitSerial = new WaitSerial(title: "正在上传");

        }
    }
}
