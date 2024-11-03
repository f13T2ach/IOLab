/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * Register.xaml.cs is part of the IOLab project.
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
using IOLab.Network;

namespace IOLab.Windows.Dialog
{
    /// <summary>
    /// Register.xaml 的交互逻辑
    /// </summary>
    public partial class Register : ContentDialog
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Register()
        {
            InitializeComponent();
        }

        private async void CreateDialog(string title, string content, bool retry = true)
        {
            Hide();
            string closeBtn = "好";
            ContentDialog dialog = new ContentDialog()
            {
                Title = title,
                Content = content,
                PrimaryButtonText = closeBtn,
                DefaultButton = ContentDialogButton.Primary,
            };
            await dialog.ShowAsync();


            if (retry)
            {
                await ShowAsync();
            }
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (Register_Email.Text == "" | Register_Username.Text == "" | Register_Password.Text == "")
            {
                CreateDialog("注册失败", "请补全你的信息");
                return;
            }
            else if (Register_Password.Text != Register_PasswordVerify.Text)
            {
                CreateDialog("注册失败", "两次输入的密码不一致");
                return;
            }

            // 显示提示
            ContentDialog dialog = new ContentDialog()
            {
                Title = "正在注册",
                Content = "请等待服务器响应",
            };
            _ = dialog.ShowAsync();

            Auth auth = new Auth(Register_Username.Text, Register_Password.Text, Register_Email.Text);
            var result = await auth.RegisterAsync();

            dialog.Hide();

            if (result.Success)
            {
                CreateDialog("注册成功", "成功注册身份: " + Register_Username.Text + " 你现在可以登录了", retry: false);
            }
            else
            {
                CreateDialog("注册时遇到问题", "服务器未能完成你的请求: " + result.Message + "\n请稍候再试");
            }
        }
    }
}
