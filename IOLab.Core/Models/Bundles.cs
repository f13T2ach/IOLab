/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * Bundles.cs is part of the IOLab project.
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

using IOLab.Core.Models.Records;

namespace IOLab.Core.Models
{
    public class Bundles
    {
        // 实验列表
        private static List<IOExperiment> _localExperiments = new List<IOExperiment>();

        public static List<IOExperiment> LocalExperiments
        {
            get { return _localExperiments; }
            set { _localExperiments = value; }
        }

        // 采集器列表
        private static List<MonitorService> _localMonitorService = new List<MonitorService>();

        public static List<MonitorService> LocalMonitorService
        {
            get { return _localMonitorService; }
            set { _localMonitorService = value; }
        }

        // 本地记录
        private static List<Schema> _localRecords = new List<Schema>();

        public static List<Schema> LocalRecords
        {
            get { return _localRecords; }
            set { _localRecords = value; }
        }

        // TODO 在线实验列表
    }
}
