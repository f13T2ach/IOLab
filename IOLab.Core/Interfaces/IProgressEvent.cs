using System;

namespace IOLab.Core.Interfaces
{
    internal interface IProgressEvent
    {
        /// <summary>
        /// 当前任务的进度，取值为 0.0 到 100.0。
        /// </summary>
        double Progress { get; }

        /// <summary>
        /// 在进度发生变化时触发的事件。
        /// </summary>
        event EventHandler<double> ProgressChanged;

        /// <summary>
        /// 更新当前任务的进度。
        /// </summary>
        /// <param name="progress">新的进度值，范围为 0.0 到 100.0。</param>
        void UpdateProgress(double progress);
    }
}
