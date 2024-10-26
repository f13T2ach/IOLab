/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * HomePage.xaml.cs is part of the IOLab project.
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

using iNKORE.UI.WPF.Modern;
using IOLab.Pages.ViewModel;
using IOLab.Resources.Guide;
using IOLab.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace IOLab.Pages
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        HomePageViewModel viewModel;
        public HomePage()
        {
            InitializeComponent();
            Preparations();
            
            viewModel = new HomePageViewModel();


            this.DataContext = viewModel;
            viewModel.OnTryPractiseSuccess += TryPractiseSuccess;
        }

        
        private void TryPractiseSuccess()
        {
            MyInfoBar.ShowWaringBar("已开始实践实验");
        }

        private void Preparations()
        {
            EquationImage.Source = BitmapFrame.Create(
                new Uri(Util.LaTeXImage.GetRequestUrl(Static.Theme.themeMode == ApplicationTheme.Dark, @"\ce{CH_{3}COOH \space \rightleftharpoons \space  CH_{3}COO^{-} \space + \space H^{+}}")),
                BitmapCreateOptions.None,
                BitmapCacheOption.Default);
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is IDisposable disposableContext)
            {
                disposableContext.Dispose();
            }
        }

        private void TestButton_ShowTeachPage_Click(object sender, RoutedEventArgs e)
        {
            TeachPlatform platform = new TeachPlatform();
            platform.Show();
        }

        private void SearchBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            viewModel.OnSearchTextChanged(SearchBox.Text);
        }
    }
}
