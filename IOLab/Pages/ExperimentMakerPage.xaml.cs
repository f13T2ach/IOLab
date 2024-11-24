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

using IOLab.Core.Network.Data;
using IOLab.Pages.ViewModel;
using IOLab.Resources.Control;
using IOLab.Windows.Dialog.Maker;
using Newtonsoft.Json.Linq;
using Page = iNKORE.UI.WPF.Modern.Controls.Page;

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
            //this.DataContext = this; ²»½áÊø°ó¶¨
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
    }

}
