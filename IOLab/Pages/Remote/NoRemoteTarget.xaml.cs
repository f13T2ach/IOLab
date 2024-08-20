using IOLab.Core;
using IOLab.Core.Assets;
using IOLab.Core.Interfaces.Threads;
using IOLab.Core.Serial;
using IOLab.Core.Tasks;
using IOLab.Resources.Control.Cyber;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Navigation;
using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace IOLab.Pages.Remote
{
    /// <summary>
    /// NoRemoteTarget.xaml 的交互逻辑
    /// </summary>
    public partial class NoRemoteTarget : Page
    {
        public NoRemoteTarget()
        {
            InitializeComponent();
            InitUI();
        }

        private void InitUI()
        {
            this.Dispatcher.Invoke(new Action(delegate
            {
                ManualConfig.IsEnabled = true;
                RefreshConfig.IsEnabled = true;
                Title.Text = "通过 USB 连接采集仪器";
            }));
        }

        private async void ManualConfig_Click(object sender, RoutedEventArgs e)
        {

            // 等待对话
            Windows.Dialog.ManualConnect mc = new Windows.Dialog.ManualConnect();
            var result = await mc.ShowAsync();
            if (!Windows.Dialog.ViewModel.ManualConnectViewModel.isConfirmed)
            {
                return;
            }
            int index = MyDataContainer.selectedItemIndex;

            // 等待连接
            ManualConfig.IsEnabled = false;
            RefreshConfig.IsEnabled = false;
            Title.Text = "请稍等";
            TryConnect(index, MyDataContainer.comArray);
        }

        private async void TryConnect(int index, string[]? array)
        {
            ConnectTool ct = new ConnectTool();
            AttentionBar.ForceStop(); // 防止中途打开

            if (array != null)
            {
                ct.InitializeSerialSet(array[index]);
                Exception? result = ct.OpenComport();
                if (result == null)
                {
                    // 测试
                    ShakeHandTask st = new ShakeHandTask();
                    InstanceBundle.list.Add(st);


                    await st.task.ConfigureAwait(false);

                    // 判断错误返回
                    if (st.task.Result.ToString() != "")
                    {
                        this.Dispatcher.Invoke(new Action(delegate
                        {
                            AttentionBar.ShowWaringBar(st.task.Result.ToString());
                        }));
                        ct.CloseComport();
                        InstanceBundle.connectTool = null;
                    }
                    // 可以连接
                    else
                    {
                        // 配置实例
                        InstanceBundle.connectTool = ct;
                        this.Dispatcher.Invoke(new Action(delegate
                        {
                            if (this.NavigationService != null)
                            {
                                NavigationService.GetNavigationService(this).Navigate(new Managers());
                            }
                        }));
                    }
                }
                // 判断串口通信异常
                else
                {
                    ct.CloseComport();
                    // 显示错误 判断是否为端口占用
                    AttentionBar.ShowWaringBar(result.Message.EndsWith("denied.")?Text.ConnectAccessDenied: result.Message);
                    InstanceBundle.connectTool = null;

                }

            }
            InitUI();

        }

        private void RefreshConfig_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
