/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * App.xaml.cs is part of the IOLab project.
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

using System.Text;
using System.Threading.Tasks;
using System;
using System.Windows;
using System.Windows.Threading;
using IOLab.Windows;
using Accessibility;

namespace IOLab
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static bool AlreadyInit = false;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // 订阅 DispatcherUnhandledException 事件
            this.DispatcherUnhandledException += AppDispatcherUnhandledException;

            // 订阅 UnhandledException 事件
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            // 订阅 UnobservedTaskException 事件
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        }

        private void AppDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            
            e.Handled = true;

            if(e.Exception.HResult == -2146233079)
            {
                return; // 不处理只允许打开一个dialog的异常 用户大量点击按钮时会出现
            }
            
            ShowErrorWindow(e.Exception);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            if (ex != null)
            {
                ShowErrorWindow(ex);
            }
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            ShowErrorWindow(e.Exception);
            e.SetObserved();
        }

        private void ShowErrorWindow(Exception exception)
        {
            // 在UI线程上显示错误窗口
            Application.Current.Dispatcher.Invoke(() =>
            {
                var errorWindow = new GlobalExceptionCatch(exception);
                errorWindow.ShowDialog();
            });
        }

    }
}
