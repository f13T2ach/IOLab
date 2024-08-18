namespace IOLab.Core.Util
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
