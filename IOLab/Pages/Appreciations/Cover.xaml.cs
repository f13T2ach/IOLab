/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * Cover.xaml.cs is part of the IOLab project.
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

// 注意！文件结构已经固定
using IOLab.Core.Assets;
using IOLab.Pages.Appreciations.ViewModel;
using Microsoft.Win32;
using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace IOLab.Pages.Appreciations
{
    /// <summary>
    /// Cover.xaml 的交互逻辑
    /// </summary>
    public partial class Cover : Page
    {
        public Cover()
        {
            InitializeComponent();
            CoverViewModel viewModel = new CoverViewModel();
            this.DataContext = viewModel;
            Prepare();

        }


        private void Prepare()
        {
            LoadText.Text = "正在检查驱动";
            CoverViewModel.ProgressValue = 10 * 2;
            if (CheckCH34DriverExists())
            {
                LoadText.Text = "驱动已安装";
                CoverViewModel.ProgressValue = 50 * 2;
                return;
            }
            else
            {
                LoadText.Text = "正在提取驱动";
                InstallDriver();
            }
        }

        private void Progress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void InstallDriver()
        {
            var zipPath = FolderNames.CH341SER_ZIPAddress;
            var newPath = $"{Environment.CurrentDirectory}\\{FolderNames.TemporaryFolder}\\IOLab_Driver_Install";
            DirectoryInfo dinfo =  new DirectoryInfo(newPath);
            if (dinfo.Exists)
            {
                dinfo.Delete(true); // 移除
                Directory.CreateDirectory(newPath); // 重建
            }
            else
            {
                Directory.CreateDirectory(newPath);
            }

            ZipFile.ExtractToDirectory(newPath, zipPath, Encoding.UTF8, true);  // 解压

            CoverViewModel.ProgressValue = 25 * 2;
            LoadText.Text = "正在安装驱动";

            UseCMDInstallDriver(newPath);

            CoverViewModel.ProgressValue = 50 * 2;
            LoadText.Text = "驱动安装成功";

            if (dinfo.Exists)
            {
                dinfo.Delete(true); // 移除
            }

        }

        public static bool CheckCH34DriverExists()
        {
            const string prefix = "ch34";
            using (RegistryKey servicesKey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services"))
            {
                if (servicesKey == null) return false;

                foreach (string serviceName in servicesKey.GetSubKeyNames())
                {
                    // 检查服务名是否以ch34开头（不区分大小写）
                    if (serviceName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }


            return false;
        }

        private static void UseCMDInstallDriver(string path)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
            p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
            p.StartInfo.CreateNoWindow = true;//不显示程序窗口
            p.Start();//启动程序
            p.StandardInput.WriteLine(path +"\\SETUP.EXE /s" + "&exit");
            p.StandardInput.AutoFlush = true;

            p.WaitForExit();//等待程序执行完退出进程
            p.Close();
        }
    }





}
