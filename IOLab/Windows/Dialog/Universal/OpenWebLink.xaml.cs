/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * OpenWebLink.xaml.cs is part of the IOLab project.
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
using Microsoft.Win32;
using System;
using System.Diagnostics;

namespace IOLab.Windows.Dialog.Universal
{
    /// <summary>
    /// OpenWebLink.xaml 的交互逻辑
    /// </summary>
    public partial class OpenWebLink : ContentDialog
    {
        public OpenWebLink(string link = "http://localhost")
        {
            InitializeComponent();
            MyWebLink.Text = link;
        }

        // 方法来自 https://blog.csdn.net/InfSein/article/details/125975626 有改动
        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            dynamic? kstr;
            string url = MyWebLink.Text;
            string s;
            try
            {
                // 从注册表中读取默认浏览器可执行文件路径
                RegistryKey? key2 = Registry.ClassesRoot.OpenSubKey(@"http\shell\open\command\");
                RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice\");
                if (key != null)
                {
                    kstr = key.GetValue("ProgId");
                    if (kstr != null)
                    {
                        s = kstr.ToString();
                        // "ChromeHTML","MSEdgeHTM" etc.
                        if (Registry.GetValue(@"HKEY_CLASSES_ROOT\" + s + @"\shell\open\command", null, null) is string path)
                        {
                            var split = path.Split('"');
                            path = split.Length >= 2 ? split[1] : "";
                            if (path != "")
                            {
                                Process.Start(path, url);
                                return;
                            }
                        }
                    }
                }
                if (key2 != null)
                {
                    kstr = key2.GetValue("");
                    if (kstr != null)
                    {
                        s = kstr.ToString();
                        var lastIndex = s.IndexOf(".exe", StringComparison.Ordinal);
                        if (lastIndex == -1)
                        {
                            lastIndex = s.IndexOf(".EXE", StringComparison.Ordinal);
                        }
                        var path = s.Substring(1, lastIndex + 3);
                        var result1 = Process.Start(path, url);
                        if (result1 == null)
                        {
                            var result2 = Process.Start("explorer.exe", url);
                            if (result2 == null)
                            {
                                Process.Start(url);
                            }
                        }
                    }
                }
                else
                {
                    var result2 = Process.Start("explorer.exe", url);
                    if (result2 == null)
                    {
                        Process.Start(url);
                    }
                }
            }
            catch
            {

            }
        }
    }
}
