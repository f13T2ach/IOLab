/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * Theme.cs is part of the IOLab project.
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
using System.Collections.Generic;

namespace IOLab.Static
{
    class Theme
    {
        private static ApplicationTheme _themeMode;
        public static ApplicationTheme themeMode
        {
            get { return _themeMode; }
            set
            {
                _themeMode = value;
                MainWindow.ChangeTheme(_themeMode);
            }
        }

        public static List<string> coloursHex = new List<string>
        {
            "#0078D7",  // 微软蓝
            "#E62129",  // 一中红
            "#ffb8c6",  // 樱粉
            "#ffae00",  // 橘橙
            "#16982b",  // 柳绿
        };
    }
}
