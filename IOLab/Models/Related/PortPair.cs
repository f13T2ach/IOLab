/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * PortPair.cs is part of the IOLab project.
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

﻿namespace IOLab.Models.Related
{
    // 该类被Service对象和viewmodel同时引用
    /// <summary>
    /// 映射对
    /// </summary>
    public class PortPair
    {
        /// <summary>
        /// PortId必须是对单片机的映射
        /// </summary>
        public string? PortId { get; set; }

        public int ChartId { get; set; }

        public bool IsChecked { get; set; }
    }
}
