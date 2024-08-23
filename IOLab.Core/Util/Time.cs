/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * Time.cs is part of the IOLab project.
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

﻿namespace IOLab.Core.Util
{
    // 使用Int存储时间是危险的
    public static class Time
    {
        /// <summary>
        /// 取当前时间的时间戳，高并发情况下会有重复。想要解决这问题请使用加锁或其他方式。
        /// </summary>
        /// <param name="accurateToMilliseconds">是否精确到毫秒</param>
        /// <returns>返回时间戳</returns>
        public static long GetTimeStamp(bool accurateToMilliseconds = false)
        {
            if (accurateToMilliseconds)
            {
                return (long)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            }
            else
            {
                return (long)DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            }
        }

        /// <summary>
        ///  时间戳转为本地日期格式 yyyy/MM/dd HH:mm:ss
        /// </summary>
        /// <param name="timeStamp">时间戳</param>
        /// <param name="onlyDate">只要日期</param>
        /// <returns></returns>
        public static string GetDateTime(long timeStamp, bool onlyDate = true)
        {
            long lTime = timeStamp;
            if (onlyDate)
            {
                return DateTimeOffset.FromUnixTimeSeconds(lTime).LocalDateTime.ToShortDateString();
            }
            else
            {
                return DateTimeOffset.FromUnixTimeSeconds(lTime).LocalDateTime.ToLongDateString();
            }
        }


    }
}
