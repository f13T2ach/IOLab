/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * DeviceName.cs is part of the IOLab project.
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

﻿using IOLab.Interfaces.ViewModel;
namespace IOLab.Util.StaticModel
{
    class DeviceName : INameEditViewModel
    {
        private static string? _title;
        public static string? Title
        {
            get
            {
                return _title;
            }
            set
            {
                // 只读
            }
        }

        private static string? _textContent;
        public static string? TextContent
        {
            get
            {
                return _textContent;
            }
            set
            {
                _textContent = value;
            }
        }

        public DeviceName()
        {
            _title = "修改设备名称";
        }
    }
}
