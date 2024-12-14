/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * ExperimentMakerPage.xaml.cs is part of the IOLab project.
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

using IOLab.Pages.ViewModel;
using IOLab.Windows.Dialog.Maker;
using IOLab.Core.Generic.Maker;
using IOLab.Core.Util.FilesBehavior;

using Microsoft.WindowsAPICodePack.Dialogs;

using System;
using Page = iNKORE.UI.WPF.Modern.Controls.Page;
using Newtonsoft.Json.Linq;
using System.IO;

namespace IOLab.Pages
{
    public partial class ExperimentMakerPage : Page
    {

        ExperimentMakerPageViewModel? model;
        public ExperimentMakerPage()
        {
            InitializeComponent();
            Visibility_NotCreated();

        }

        private async void CreateNew_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NewProject dialog = new NewProject();
            var result = await dialog.ShowAsync();
            if (result != iNKORE.UI.WPF.Modern.Controls.ContentDialogResult.Primary)
                return;
            if (dialog.source == null)
                return;
            model = new ExperimentMakerPageViewModel(dialog.source);
            this.DataContext = model;
            Visibility_Created();
        }

        private void CloseEditor_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Visibility_NotCreated();
            //this.DataContext = this; 不结束绑定
            model = null;
        }



        private void Visibility_NotCreated()
        {
            Container_Created.Visibility = System.Windows.Visibility.Collapsed;
            Continer_NotCreated.Visibility = System.Windows.Visibility.Visible;
        }

        private void Visibility_Created()
        {
            Container_Created.Visibility = System.Windows.Visibility.Visible;
            Continer_NotCreated.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void OpenProject_Click(object sender, System.Windows.RoutedEventArgs e)
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

                if(raw == null)
                {
                    // 读取失败
                    new MainWindow().CreateDialog("无法加载实验", "IOLab 无法打开项目");
                    return;
                }

                JObject json;
                try
                {
                    json = JObject.Parse(raw);
                }
                catch(Exception ex)
                {
                    new MainWindow().CreateDialog("无法加载实验", "IOLab 解析包失败\n"+ex.Message);
                    return;
                }
                
                model = new ExperimentMakerPageViewModel(source,json);
                this.DataContext = model;
                Visibility_Created();

                // 当包名和文件名不一致时 警告用户
                 if  (json["experiment"]?.ToString() != Path.GetFileNameWithoutExtension(Read.GetLastLevelName(selectedFile)))
                {
                    new MainWindow().CreateDialog("请遵循设计规范", "IOLab 要求包名与文件名一致，你打开的文件包名与文件名不一致。这将导致你手动导入包到本地时，IOLab 无法正确解析包以生成本地实验包列表。");
                }

            }
        }

    }

}
