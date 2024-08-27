/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * PerformanceCollTask.cs is part of the IOLab project.
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

using IOLab.Core.Assets;
using IOLab.Core.Interfaces.Threads;

namespace IOLab.Core.Tasks
{
    public class PerformanceColtTask : ITaskAnnounce
    {

        private string _tag;

        public Task<object> task;

        public ITaskHandler<object> handler;

        public string Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }



        public PerformanceColtTask()
        {
            Tag = Tags.ShakeHand;

            handler = new PerformanceColt();
            task = new Task<object>(handler.Handler);
            task.Start();


        }

        public async void GetResult()
        {
            await task.ConfigureAwait(false);
            return;
        }


        public Task GetTaskInstance()
        {
            return task;
        }

        public ITaskHandler<object> GetHandlerInstance()
        {
            return handler;
        }
    }
}
