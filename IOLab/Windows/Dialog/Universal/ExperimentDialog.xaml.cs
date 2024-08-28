/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * ExperimentDialog.xaml.cs is part of the IOLab project.
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
using IOLab.Core.Models;
using System.Collections.Generic;

namespace IOLab.Windows.Dialog.Universal
{
    /// <summary>
    /// ExperimentDialog.xaml 的交互逻辑
    /// </summary>
    public partial class ExperimentDialog : ContentDialog
    {
        public string MyTitle { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public string Sensors { get; set; }
        public string Steps { get; set; }
        public ExperimentDialog(IOExperiment experiment)
        {
            InitializeComponent();
            MyTitle = experiment.Name;

            foreach (string item in experiment.Tags)
            {
                Subtitle += $"{item} ";
            }
            Subtitle ??= Text.AboutThisExperiment;

            Description = experiment.Description;

            foreach (MonitorService service in experiment.Parameters)
            {
                Sensors += $"{service.Name} ";
            }
            Sensors ??= Text.NoSensorNeeded;

            foreach (KeyValuePair<string, string> step in experiment.Content)
            {
                Steps += $"{step.Key}\n{step.Value}\n";
            }
            Steps ??= Text.NoDescriptions;
            Steps = Steps.Remove(Steps.LastIndexOf("\n"), 1);

            DataContext = this;
        }
    }
}
