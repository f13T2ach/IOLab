using Newtonsoft.Json.Linq;

namespace IOLab.Core.Generic
{
    public static class ActionBundle
    {

        public static event Action<bool> LoginStat;

        public static event Action LoginStatusChange;

        public static event Action<bool>? PauseOrStartRecord;

        public static event Action? UpdateSerialList;
        /// <summary>
        /// 暂停或继续
        /// </summary>
        /// <param name="value">true：暂停 false：继续</param>
        public static void DoPauseOrStart(bool value)
        {
            PauseOrStartRecord?.Invoke(value);
        }

        public static event Action<JObject>? NewestSerialCommand;

        public static void DoUpdateNewest(JObject value)
        {
            NewestSerialCommand?.Invoke(value);
        }

        public static void DoUpdateSerialList()
        {
            UpdateSerialList?.Invoke();
        }

        public static void DoChangeLoginStat(bool val)
        {
            LoginStat?.Invoke(val);
            LoginStatusChange?.Invoke();
        }
    }
}
