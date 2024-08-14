using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOLab.Core.Config
{
    public class InternalConfig
    {
        public static int ConnectTimeout = 3000;   // 3000毫秒后超时

        public static int ShakeHandTimeStep = 100; // 100毫秒检测一次
    }
}
