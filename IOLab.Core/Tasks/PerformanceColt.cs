/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * PerformanceColt.cs is part of the IOLab project.
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

using IOLab.Core.Generic;
using IOLab.Core.Interfaces.Threads;
using IOLab.Core.Models;
using Newtonsoft.Json.Linq;

namespace IOLab.Core.Tasks
{
    public class PerformanceColt : ITaskHandler<object>
    {
        public static JObject _parameter = null;


        public bool Inhibit { get => false; set { } }

        public ITaskAnnounce? Announcer { get; set; }

        JObject ITaskHandler<object>.Parameter
        {
            get { return _parameter; }
            set { _parameter = value; }
        }

        object ITaskHandler<object>.Handler()
        {
            return handler();
        }

        object handler()
        {
            while (true)
            {
                if (Announcer.tokenSource.IsCancellationRequested)
                {
                    break;
                }
                Thread.Sleep(1000);
                if (InstanceBundle.ChartTables == null)
                {
                    continue;
                }
                foreach (ChartTableModel model in InstanceBundle.ChartTables)
                {
                    model.AddDataPoint(new Random().Next(0, 10));
                }
            }
            return null;
        }
    }
}
