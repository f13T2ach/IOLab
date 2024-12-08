using IOLab.Core.Properties;
using IOLab.Network.API;
using IOLab.Windows.Dialog;
using System;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace IOLab.Resources.Control
{
    public partial class ServerStatus : UserControl
    {
        // 依赖属性用于数据绑定
        public static readonly DependencyProperty DelayProperty =
            DependencyProperty.Register("Delay", typeof(double), typeof(ServerStatus), new PropertyMetadata(0.0, OnValueChanged));

        public static readonly DependencyProperty LoadProperty =
            DependencyProperty.Register("Load", typeof(double), typeof(ServerStatus), new PropertyMetadata(0.0, OnValueChanged));

        public static readonly DependencyProperty TotalAccessProperty =
            DependencyProperty.Register("TotalAccess", typeof(string), typeof(ServerStatus), new PropertyMetadata("--"));

        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register("Status", typeof(string), typeof(ServerStatus), new PropertyMetadata("DIFF"));

        // 属性包装器
        public double Delay
        {
            get { return (double)GetValue(DelayProperty); }
            set { SetValue(DelayProperty, value); }
        }

        public double Load
        {
            get { return (double)GetValue(LoadProperty); }
            set { SetValue(LoadProperty, value); }
        }

        public string TotalAccess
        {
            get { return (string)GetValue(TotalAccessProperty); }
            set { SetValue(TotalAccessProperty, value); }
        }

        public string Status
        {
            get { return (string)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }


        private Exception? CurrentException;

        public ServerStatus()
        {
            InitializeComponent();
            UpdateServerStatus();
        }

        private void ReTestButton_Click(object? sender, RoutedEventArgs? e)
        {
            // 模拟重新测试逻辑
            UpdateServerStatus();
        }

        private void ReportIssueButton_Click(object sender, RoutedEventArgs e)
        {
            // 显示等待
            WaitSerial wait = new WaitSerial(title: "正在测试连接服务器");
            wait.ShowAsync();

            // 移除之前的异常
            CurrentException = null;
            UpdateServerStatus(); // 再测试一次

            wait.Hide();

            if (CurrentException == null) // 连接成功
            {
                new MainWindow().ShowNotification("没有可用帮助", "连接测试已通过（此疑难解答只适用于无法连接服务器的情形）");
                return;
            }

            
            string advice = GetExceptionResolutionAdvice(CurrentException);
            new MainWindow().ShowNotification("疑难解答", "解决方案：\n"+advice+"\n依旧无法解决？\n联系技术支持 Email: f12fds@163.com（周一至周五9:00-16:00，法定节假日除外）");
        }

        private async void UpdateServerStatus()
        {
            string url = IONetAPI.Servers[Settings.Default.ServerIndex];
            string host = ExtractHost(url);
            int port = ExtractPort(url);

            using (TcpClient client = new TcpClient())
            {
                try
                {
                    DateTime startTime = DateTime.Now;
                    await client.ConnectAsync(host, port);
                    TimeSpan elapsedTime = DateTime.Now - startTime;

                    Delay = (int)elapsedTime.TotalMilliseconds;
                    Status = "ONLINE";
                    
                }
                catch (Exception ex)
                {
                    Delay = 9999;
                    Status = $"ERROR";
                    CurrentException = ex;
                }
            }

            ServerName.Text = Settings.Default.ServerIndex;

            //todo 从服务器获取
            TotalAccess = "3,000";
            Load = 26;
        }

        private string GetExceptionResolutionAdvice(Exception exception)
        {
            if (exception is SocketException socketEx)
            {
                switch (socketEx.SocketErrorCode)
                {
                    case SocketError.HostNotFound:
                        return "目标主机不存在\n1. 确认你的 DNS 服务器与代理软件配置正常\n2. 确保你的组织网络（如校园网）允许你访问互联网";
                    case SocketError.TimedOut:
                        return "连接超时\n1. 检查网络连接\n2. 关注 IOLab 公告提示，服务器可能在停服维护";
                    case SocketError.ConnectionRefused:
                        return "服务器拒绝连接\n1. 如果是自定义服务器，确保端口填写正确\n2. 关注 IOLab 公告提示，服务器可能在停服维护\n3. 如果是官方服务器，且你 IP 存在非法行为，你可能被列入黑名单（通常在封禁前有提示）";
                    default:
                        return $"网络错误：{socketEx.Message}，请检查网络设置并重试。";
                }
            }
            else if (exception is InvalidOperationException)
            {
                return "操作无效";
            }
            else if (exception is ArgumentNullException)
            {
                return "参数无效";
            }
            else
            {
                return $"未知错误：{exception.Message}，请联系技术支持。";
            }
        }

        private string ExtractHost(string url)
        {
            Uri uri = new Uri(url);
            return uri.Host;
        }

        private int ExtractPort(string url)
        {
            Uri uri = new Uri(url);
            return uri.Port;
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var serverStatus = (ServerStatus)d;
            serverStatus.UpdateSignalStrength();
        }

        private void UpdateSignalStrength()
        {
            // 300ms 为最大可接受延迟
            double signalStrength = ((Delay / 300)*100 + Load) / 2;
            if (signalStrength > 100) signalStrength = 100;
            signalStrength = 100 - signalStrength;
            int opacityLevel = (int)(signalStrength / 25); //格数

            for (int i = 0; i < 4; i++)
            {
                Border border = GetBorderByIndex(i);
                if (i < opacityLevel)
                {
                    border.Opacity = 1.0;
                }
                else if (i == opacityLevel && signalStrength % 25 != 0)
                {
                    border.Opacity = 1;
                }
                else
                {
                    border.Opacity = 0.5;
                }
            }
        }

        private Border GetBorderByIndex(int index)
        {
            switch (index)
            {
                case 0:
                    return SignalStrengthBox1;
                case 1:
                    return SignalStrengthBox2;
                case 2:
                    return SignalStrengthBox3;
                case 3:
                    return SignalStrengthBox4;
                default:
                    throw new ArgumentOutOfRangeException(nameof(index));
            }
        }
    }
}