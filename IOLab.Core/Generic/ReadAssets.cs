/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * ReadAssets.cs is part of the IOLab project.
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
using IOLab.Core.Models;
using IOLab.Core.Models.Records;
using IOLab.Core.Util.FilesBehavior;
using Newtonsoft.Json.Linq;

namespace IOLab.Core.Generic
{
    public class ReadAssets
    {
        public static void ReadRecords()
        {
            Bundles.LocalRecords = new List<Schema>();
            string root = $".\\{FolderNames.RecordFolder}\\";
            List<string> records = Traversal.GetAllFiles(root, 1);
            if (records == null)
            {

                return;
            }
            foreach (string record in records)
            {
                Schema schema = new Schema(root + record);
                Bundles.LocalRecords.Add(schema);
            }
        }

        public static void ReadExperiments()
        {
            Bundles.LocalExperiments = new List<IOExperiment>();
            string root = $".\\{FolderNames.AssetsFolder}\\{FolderNames.AssetsExperimentLocalFolder}\\";
            List<string> files = Traversal.GetAllFiles(root, 1);
            if (files == null)
            {

                return;
            }
            foreach (string file in files)
            {
                IOExperiment experiment = new IOExperiment(root + file);
                Bundles.LocalExperiments.Add(experiment);
            }
        }

        public static void ReadLocalServices()
        {
            Bundles.LocalMonitorService = new List<MonitorService>();
            string file = $".\\{FolderNames.AssetsFolder}\\{FolderNames.AssetsServicesLocalFile}";
            string content = Read.ReadFileUTF8(file);
            if (content == null)
            {

                return;
            }
            JArray jArray = JArray.Parse(content);
            foreach (JToken token in jArray)
            {
                JObject item = token as JObject;
                if (item != null)
                {
                    MonitorService service = new MonitorService(item);
                    Bundles.LocalMonitorService.Add(service);
                }
            }

            // Bundles.LocalExperiments.Add(experiment);
        }
    }
}
