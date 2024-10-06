/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * Read.cs is part of the IOLab project.
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

using System.Text;

namespace IOLab.Core.Util.FilesBehavior
{
    public static class Read
    {
        /// <summary>
        /// UTF8读取文件
        /// </summary>
        /// <param name="path">绝对路径</param>
        /// <returns></returns>
        public static string? ReadFileUTF8(string path)
        {
            StringBuilder content = new StringBuilder();
            using (FileStream fs = File.OpenRead(path))
            {
                byte[] buffer = new byte[1024];
                UTF8Encoding encoding = new UTF8Encoding(true);
                int bytesRead;
                while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
                {
                    content.Append(encoding.GetString(buffer, 0, bytesRead));
                }
            }
            return content.ToString();
        }

        public static string GetLastLevelName(string path)
        {
            string[] paths = path.Split("\\");
            if (paths[paths.Count() - 1] == "")
            {
                return paths[paths.Count() - 2];
            }
            return paths[paths.Count() - 1];
        }


    }
}
