/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * SaveRecords.xaml.cs is part of the IOLab project.
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
using IOLab.Core.Properties;
using IOLab.Network;
using System;
using System.Diagnostics;
using System.Windows;

namespace IOLab.Windows.Dialog
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : ContentDialog
    {

        private static Auth _authInstance;

        /// <summary>
        /// 构造函数
        /// </summary>
        public Login()
        {
            InitializeComponent();
            
        }

        private async void IForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            await new IForgot().ShowAsync();
            
        }

        private async void RegistNewAccount_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            await new Register().ShowAsync();
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            InstanceBundle.UsingAuth = new Auth(UsernameInput.Text, PasswordInput.Text);

            // 登录
            // 战术停顿，以防止用户操作
            WaitSerial wait = new WaitSerial(0);
            wait.ShowAsync();

            var loginResult = await InstanceBundle.UsingAuth.LoginAsync();

            wait.Hide();

            this.Hide();

            if (loginResult.Success)
            {
                // 登录成功后输出用户数据
                Debug.WriteLine("User data: " + loginResult.Data.ToString());


                // 然后保留token
                Settings.Default["UserToken"] = InstanceBundle.UsingAuth.Token;
                Settings.Default.Save();

                ShowDialog("欢迎回来: " + loginResult.Data["username"].ToString());
                return;
            }
            InstanceBundle.UsingAuth = null;
            ShowDialog("无法登录: " + loginResult.Message);
        }

        private async void ShowDialog(string text)
        {
            ContentDialog Dialog;
            Dialog = new ContentDialog()
            {
                Title = "登录",
                Content = text,
                CloseButtonText = "好"
            };
            await Dialog.ShowAsync();
        }
    }
}
