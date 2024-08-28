/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * AboutPage.xaml.cs is part of the IOLab project.
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

using IOLab.Network;
using IOLab.Windows.Dialog;
using IOLab.Windows.Dialog.Universal;
using System;
using System.Windows;
using Page = iNKORE.UI.WPF.Modern.Controls.Page;


namespace IOLab.Pages
{
    /// <summary>
    /// Interaction logic for AboutPage.xaml
    /// </summary>
    public partial class AboutPage : Page
    {
        public string[] ADTexts
        { get; set; }
        public AboutPage()
        {
            InitializeComponent();
            ADTexts = new string[] { "了解一中招生信息", "办一所让人幸福成长的学校", "在最美的时光遇见你" };
            DataContext = this;
        }

        private async void AboutUsButton_Click(object sender, RoutedEventArgs e)
        {
            LearnMoreAboutDeveloper lmad = new LearnMoreAboutDeveloper();
            await lmad.ShowAsync();
        }

        private async void ADButton_Click(object sender, RoutedEventArgs e)
        {
            await new OpenWebLink(URLBundle.DGYZWebsite).ShowAsync();
        }

        private async void AboutOSPButton_Click(object sender, RoutedEventArgs e)
        {
            await new QuoteOSP().ShowAsync();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is IDisposable disposableContext)
            {
                disposableContext.Dispose();
            }
        }
    }
}
