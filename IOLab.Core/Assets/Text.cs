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

namespace IOLab.Core.Assets
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

        public static Dictionary<string, string> TeachingString = new Dictionary<string, string>()
        {
            { "连接设备","在管理页面点击选取端口，连接设备，插拔连接线多出的设备即为你设备的编号" },
            { "开始实验","在实验列表选中实验，点击详情后点实践即可开始实验" },
            { "仅采集而不做实验","如果不需要实验，仅需采集，可点监视下方的加号添加图表" },
            { "添加图表","插口对应设备上的插口，采集方案需与传感器匹配" },
            { "实验助手","开始实验后，可点击监视上方的助手，查看实验" },
        };

        // 窗口
        public static string DisconnectDialog_SureToDisconnect = "确定断开连接吗";
        public static string DisconnectDialog_AlertText = "如果你有未保存的工作，继续操作可保存\n断开连接后实验助理不再可用";

        public static string Help = "帮助";
        public static string Help_Connect_Content = "1. 拒绝访问\n" +
            "某些应用程序如 Arduino 和 Serial Studio 会占用目标端口，建议关闭程序或断开连接以取消占用。\n" +
            "2. 连接超时\n" +
            "IOLab 会等待 10 秒，如果 10 秒内该端口没有发送有效验证信息，即为超时。请检查串口驱动，并确保固件没有被修改。\n" +
            "3. 连接断开\n" +
            "检查你的接线，确保设备插口和 USB 线都正常。\n" +
            "4. 我无法找到端口\n" +
            "a. 先拔出设备，然后打开设备列表，记录列表。再关闭列表，插入设备，再次打开列表，多出的一项即为正确的端口；\n" +
            "b. 如果没有多出任何一项，你可能没有安装串口驱动。安装 CH340 驱动；\n" +
            "c. 某些 USB 线内部只有两根导线，无法传输数据；尝试在其它计算机连接设备排除计算机的问题。";
        public static string DisconnectDialog_Disconnected = "设备断开连接";
        public static string DisconnectDialog_Hint = "你仍可保存未完成的工作，如果这并非你所操作，请检查你的 USB 线缆";
        public static string DisconnectDialog_HintFailed = "如果这并非你所操作，请检查你的 USB 线缆\n你正在配置的窗口阻碍我们提示你，如果你在配置与采集有关的项目，保存配置不会有效\n你仍可保存未完成的工作，但暂时不能重命名实验了\n点击“是”我们就为你保存（如果有的话）";
        // 线程
        public static string ConnectTimeOut = "连接失败：认证超时";
        public static string CallbackTimeOut = "连接失败：回调超时";
        public static string ConnectAccessDenied = "连接失败：拒绝访问";
        public static string ForcedCanceledBySystem = "操作被强制中止";

        // 加载页面
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

        // 设备管理器与实验管理
        public static string NeedToPrepare = "需要准备";
        public static string Collector = "采集仪器";

        public static string NotSupportRFID = "当前固件版本不支持 RFID 射频识别技术";

        public static string AboutThisExperiment = "关于此实验";
        public static string NoSensorNeeded = "此实验不需要传感器";
        public static string NoDescriptions = "此实验没有介绍\n"; // \n是必须的

        public static string ConnectToManagePins = "连接设备以管理插口";



        // 实验助手
        public static string NoItemSelected = "未定实验项";
        public static string NoStepExists = "无步骤";
        public static string NoStepExistsExplanation = "你正在自由实验，因此实验助手无法帮到你；可前往主页选择一个实验以实践。";
        public static string FrozenChart = " - 已归档";
        public static string SureToDelete = "确定移除吗";
        public static string SureToDeleteExplain_Part1 = "你选择的表「";
        public static string SureToDeleteExplain_Part2 = "」是当前实验需要的表，继续删除可能对实验有所影响\n如果你在较早的步骤记录了这张表，则不要删除它：为防止插口占用，每回溯一次都会删除上一步的表";
        public static string ReleaseExperiment = "确定结束吗";
        public static string ReleaseExperiment_Hint = "你不会丢失实验数据，且已经被冻结的表不能再接收数据";

        public static string SureToFreeze = "确认归档吗";
        public static string SureToFreezeExplain = "归档表格相当于删除表格但保留表格的数据，这意味着表格不能再接收数据，此操作不可撤销。";
        // 异常捕获

        public static string SureToReset = "确定重置 IOLab 吗";
        public static string SureToResetExplain = "这将删除你的实验记录和设置，并且会重新生成本地实验\n这可能需要一些时间，IOLab 将自动重启";
    }
}
