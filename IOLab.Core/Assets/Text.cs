/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * Text.cs is part of the IOLab project.
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

﻿namespace IOLab.Core.Assets
{
    public class Text
    {
        public static string WelcomeDialog_Hello = "你好";
        public static string WelcomeDialog_Content = "";

        public static string SplitSymbol = "、";

        public static string MonitorNotStartYet = "未启动监视";

        public static List<string> HeaderTitle = new List<string>()
        {

        };

        public static string[] TipForFun = new string[]
        {
            "来看看这是什么记录",
            "虚空索敌？",
            "操作无效",
        };
        public static string ConnectTimeOut = "连接失败：认证超时";
        public static string ConnectAccessDenied = "连接失败：拒绝访问";

        public static string ReceivingMessageFromIONet = "正在从 IONet 收取信息";
        public static string ReceivingUpdateFromIONet = "正在从 IONet 下载重要更新";
        public static string DownloadUpdateCompleted = "更新已完成，关闭 IOLab 后开始安装";
        public static string HotFixCompleted = "更新已完成，将自动进入 IOLab";
        public static string ProcessingResources = "正在处理资源";
        public static string DownloadingDriver = "正在下载驱动程序";
        public static string InstallingDriver = "正在安装驱动程序";
        public static string WaitForExceptionUpload = "IOLab 出现致命错误，正在上传错误记录 ERRCODE:{0}"; //ErrorCode
        public static string ExceptionUploadFailed = "IOLab 出现致命错误，且无法上传错误记录 ERRCODE:{0}_{1}"; //ErrorCode NetworkErrorCode
        public static string ExceptionCompleted = "IOLab 错误上报已完成，请重新启动程序 ERRCODE:{0}"; // ErrorCode
        public static string PleaseWait = "请稍等";
        public static string InstallCompleted = "准备就绪，即将重新启动";

        public static string NeedToPrepare = "需要准备";
        public static string Collector = "采集仪器";

        public static string NotSupportRFID = "当前固件版本不支持 RFID 射频识别技术";

        public static string AboutThisExperiment = "关于此实验";
        public static string NoSensorNeeded = "此实验不需要传感器";
        public static string NoDescriptions = "此实验没有介绍\n"; // \n是必须的
    }
}
