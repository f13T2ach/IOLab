using IOLab.Core.Assets;
using IOLab.Core.Config;
using IOLab.Core.Interfaces.Threads;

namespace IOLab.Core.Tasks
{
    public class ShakeHand : ITaskHandler<object>
    {
        public static string _parameter = "";

        string ITaskHandler<object>.Parameter
        {
            get { return _parameter; }
            set { _parameter = value; }
        }

        object ITaskHandler<object>.Handler()
        {
            // 等待验证
            for (int i = 0; i < InternalConfig.ConnectTimeout / InternalConfig.ShakeHandTimeStep; i++)
            {
                if (_parameter != "command")
                {
                    Thread.Sleep(InternalConfig.ShakeHandTimeStep);
                }
                else
                {
                    // 配置一个串口实例
                    return "";
                }
            }
            return Text.ConnectTimeOut;
        }
    }
}
