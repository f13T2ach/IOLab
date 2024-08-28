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
using System;
using System.Windows;

namespace IOLab.Windows.Dialog
{
    /// <summary>
    /// SaveRecords.xaml 的交互逻辑
    /// </summary>
    public partial class SaveRecords : ContentDialog
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mode">0：不能保存1：正常保存2：强制保存</param>
        public SaveRecords(int mode)
        {
            InitializeComponent();
            switch (mode)
            {
                case 0:
                    ConatinerA.Visibility = Visibility.Hidden;
                    ConatinerB.Visibility = Visibility.Visible;
                    LoadTipForFun();
                    PrimaryButtonText = "";
                    break;
                case 1:
                    LoadInformation();
                    break;
                case 2:
                    SecondaryButtonText = "";
                    break;

            }
        }

        public void LoadInformation()
        {

        }

        private void LoadTipForFun()
        {
            int length = Text.TipForFun.Length;
            int index = new Random().Next(length);
            TipForFun.Text = Text.TipForFun[index];
        }
    }
}
