

using System.IO.Packaging;

/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * FolderNames.cs is part of the IOLab project.
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
    public class FolderNames
    {
        public static string Slash = @"\";
        public static string AssetsFolder = "Assets";
        public static string TemporaryFolder = "Temporary";
        public static string AssetsServicesLocalFile = "local_services.json";
        public static string AssetsExperimentLocalFolder = "LocalExperiment";
        public static string AssetsExperimentNetworkFolder = "NetworkExperiment";   //TODO 实装分区
        public static string AssetsDriverFolder = "Drivers";

        // 尽量不要使用网络服务做采集器
        public static string AssetsServicesNetworkFolder = "NetworkServices";

        public static string RecordFolder = "Record";

        // 工具链
        public static string ToolChain = "ToolChain";

        // ollama ai
        public static string OllamaFolder = "Ollama";

        // 串口驱动
        public static string CH341SER_ZIPAddress = $"{AssetsFolder}\\{AssetsDriverFolder}\\CH341SER.ZIP";


    }
}
