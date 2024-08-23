/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * Traversal.cs is part of the IOLab project.
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

﻿namespace IOLab.Core.Util.FilesBehavior
{
    public class Traversal
    {
        /// <summary>
        /// 获取所有文件夹下文件
        /// </summary>
        /// <param name="dir">文件夹地址</param>
        /// <param name="level">搜索深度</param>
        public static List<string> GetAllFiles(string dir, int level)
        {
            DirectoryInfo d = new DirectoryInfo(dir);
            FileSystemInfo[] fsinfos;
            try
            {
                fsinfos = d.GetFileSystemInfos();
            }
            catch
            {
                DirectoryInfo newdir = new DirectoryInfo(dir);
                newdir.Create();
                return null;
            }
            List<string> dirs = new List<string>();
            foreach (FileSystemInfo fsinfo in fsinfos)
            {
                if (fsinfo is DirectoryInfo && level > 1)
                {
                    dirs = Collection.Merge<string>(dirs, GetAllFiles(fsinfo.FullName, level - 1));
                }
                else
                {
                    dirs.Add(fsinfo.Name);
                }
            }
            return dirs;

        }

    }
}
