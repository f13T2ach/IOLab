using IOLab.Core.Properties;
using IOLab.Network.API;
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
            // 模拟故障上报逻辑
            MessageBox.Show("故障已上报！");
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
                }
            }

            ServerName.Text = Settings.Default.ServerIndex;

            //todo 从服务器获取
            TotalAccess = "3,000";
            Load = 26;
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