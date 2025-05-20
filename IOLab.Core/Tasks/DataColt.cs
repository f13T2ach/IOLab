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
    /// <summary>
    /// Tag: Collect 收集实验数据
    /// </summary>
    public class DataColt : ITaskHandler<object>
    {
        public static JObject? _parameter = null;

        public static Action<JObject>? parameterAction;


        public bool Inhibit { get => false; set { } }

        private bool pause = false;

        public ITaskAnnounce? Announcer { get; set; }

        JObject ITaskHandler<object>.Parameter
        {
            get { return _parameter; }
            set
            {
                _parameter = value;
                if (value != null)
                    parameterAction?.Invoke(value);
            }
        }

        object ITaskHandler<object>.Handler()
        {
            ActionBundle.PauseOrStartRecord += ChangePauseStat;
            parameterAction += handler;
            return null;
        }

        private void ChangePauseStat(bool value)
        {
            pause = value;
        }

        void handler(JObject value)
        {
            if (pause)
            {
                return;
            }

            foreach (ChartTableModel model in InstanceBundle.ChartTables)
            {
                // TODO 实装采集数据
                // 注意：实装时不需要判断是不是Frozen态 因为它会主动取消监听
                if (!model.isFrozen && !model.isPause)
                {
                    if (value["data"] != null)
                    {
                        try
                        {
                            
                            model.AddDataPoint((float)value["data"]);
                        }
                        catch
                        {
                            // fuck you
                        }
                    }
                }
            }
        }
    }
}
