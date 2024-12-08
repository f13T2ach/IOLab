using iNKORE.UI.WPF.Modern.Controls;
using IOLab.Core.Assets;
using System;
using System.Windows;

namespace IOLab.Windows
{
    /// <summary>
    /// GlobalExceptionCatch.xaml 的交互逻辑
    /// </summary>
    public partial class GlobalExceptionCatch : Window
    {

        bool isSelfClose = false;
        public GlobalExceptionCatch(Exception exception)
        {
            InitializeComponent();

            Exception inner = exception.InnerException;
            ExceptionName.Text = inner.GetType().Name;
            ExceptionConatiner.Text =
                "异常源\n" + inner.Source +
                "\n\n异常消息\n" + inner.Message +
                "\n\n堆栈追踪\n" + inner.StackTrace +
                "\n\n" + GetComputerInfo();

        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (!isSelfClose)
            {
                e.Cancel = true; // 阻止关闭
            }

        }

        public static string GetComputerInfo()
        {
            string info = "计算机信息\n";

            // 获取操作系统信息
            info += "OS Version: " + Environment.OSVersion.ToString() + "\n";
            info += "64-bit OS: " + Environment.Is64BitOperatingSystem + "\n";

            // 获取系统架构
            info += "System Architecture: " + Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE") + "\n";

            // 获取可用物理内存
            info += "Available Physical Memory: " + Environment.WorkingSet + "\n";

            return info;
        }

        private async void ReloadStore_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog()
            {
                Title = Text.SureToReset,
                Content = Text.SureToResetExplain,
                PrimaryButtonText = "继续",
                CloseButtonText = "取消",
                DefaultButton = ContentDialogButton.Primary,
            };
            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
            {

            }
        }

        private void IgnoreException_Click(object sender, RoutedEventArgs e)
        {
            isSelfClose = true;
            this.Close();
        }

    }
}
