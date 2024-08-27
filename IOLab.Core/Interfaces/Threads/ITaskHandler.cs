

using Newtonsoft.Json.Linq;
/**
* IOLab - A Full-Stack Experimentation Platform
*
* Copyright (C) 2024 Redmaple. All rights reserved.
*
* ITaskHandler.cs is part of the IOLab project.
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
namespace IOLab.Core.Interfaces.Threads
{
    public interface ITaskHandler<TType>
    {
        public ITaskAnnounce Announcer { get; set; }
        public TType Handler();

        public JObject Parameter { get; set; }

        /// <summary>
        /// 抑制标志（赋值只开放一次）置为 true 则总是准许赋值
        /// </summary>
        public bool Inhibit { get; set; } 
    }
}
