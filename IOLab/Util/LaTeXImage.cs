/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * LaTeXImage.cs is part of the IOLab project.
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

uamespace IOLab.Util
{
    class LaTeXImage
    {
        //https://latex.codecogs.com/png.download?\huge\fg{FFFFFF}%20\ce{Ba^2+%20+%20SO4^2-%20=%20BaSO4%20v}
        static string LatexURL = @"https://latex.codecogs.com/png.download?\huge";

        public static string GetRequestUrl(bool isDark, string content)
        {
            string url = LatexURL + @"\fg{" + (isDark ? "FFFFFF" : "000000") + "}%20" + content;
            return url;
        }
    }
}
