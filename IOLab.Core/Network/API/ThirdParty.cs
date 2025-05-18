/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * ThirdParty.cs is part of the IOLab project.
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

namespace IOLab.Network.API
{
    public class ThirdParty
    {

        public static string DeepseekAIToken = "sk-0f38460bff1a4d4d91ff679fc1518cf1";

        public static string DeepseekAIAliyunToken = "sk-5cd408488a994283b3e0432184df959c";

        public static string AliyunDeepseekURL = "https://dashscope.aliyuncs.com/compatible-mode/v1/chat/completions";

        public static string LocalURL = "http://127.0.0.1:11434/v1/chat/completions";

        public static string RemoteModelName = "qwen-plus";

        public static string LocalModelName = "qwen2.5:0.5b";
    }
}
