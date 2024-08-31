using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOLab.Core.Generic
{
    public static class ActionBundle
    {
        public static event Action<bool>? PauseOrStartRecord;
        /// <summary>
        /// 暂停或继续
        /// </summary>
        /// <param name="value">true：暂停 false：继续</param>
        public static void DoPauseOrStart(bool value)
        {
            PauseOrStartRecord?.Invoke(value);
        }
    }
}
