/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * NewProject.xaml.cs is part of the IOLab project.
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
using IOLab.Core.Generic.Maker;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace IOLab.Windows.Dialog.Maker
{
    public partial class NewProject : ContentDialog
    {
        public EditExperiment? source;
        public NewProject()
        {
            InitializeComponent();
        }


        public static bool IsValidPackageName(string packageName)
        {
            // 正则表达式：只允许字母（大小写）、数字和下划线
            string pattern = @"^[a-zA-Z0-9_]+$";

            // 使用正则表达式验证包名
            return Regex.IsMatch(packageName, pattern);
        }

        // 处理 "继续" 按钮的点击事件
        private void PrimaryButton_Click(object sender, ContentDialogButtonClickEventArgs e)
        {
            // 获取用户输入
            string name = NameBox.Text;
            string packageName = PackageNameBox.Text;
            string description = DescriptionBox.Text;
            string saveAddress = LocalAddressText.Text;

            // 验证用户输入
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(packageName))
            {
                errorTextBlock.Text = "名称和包名不能为空";
                e.Cancel = true;
                return;
            }

            if (!IsValidPackageName(packageName))
            {
                errorTextBlock.Text = "包名只接受英文、数字和下划线";
                e.Cancel = true;
                return;
            }

            if (string.IsNullOrEmpty(saveAddress))
            {
                errorTextBlock.Text = "未选定保存位置";
                e.Cancel = true;
                return;
            }

            // 执行保存操作，例如创建实验项目
            try
            {
                // 假设 SaveProject 是一个方法，保存实验信息
                Save(name, packageName, description, saveAddress);
                errorTextBlock.Text = "";
            }
            catch (Exception ex)
            {
                errorTextBlock.Text = $"保存失败: {ex.Message}";
                e.Cancel = true;
            }
        }

        // 处理 "选择文件夹" 按钮的点击事件
        private void ChooseFolderButton_Click(object sender, RoutedEventArgs e)
        {
            // 打开文件夹选择对话框


            var dlg = new CommonOpenFileDialog();
            dlg.IsFolderPicker = true;
            dlg.InitialDirectory = System.IO.Directory.GetCurrentDirectory();

            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                LocalAddressText.Text = dlg.FileName;
            }

        }

        // 假设的保存方法
        private void Save(string name, string packageName, string description, string saveAddress)
        {
            source = new EditExperiment();
            source.Name = name;
            source.PackageName = packageName;
            source.Description = description;
            source.SaveAddress = saveAddress;
        }
    }
}

