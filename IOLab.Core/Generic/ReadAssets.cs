using IOLab.Core.Assets;
using IOLab.Core.Models;
using IOLab.Core.Models.Records;
using IOLab.Core.Util.FilesBehavior;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOLab.Core.Local
{
    public class ReadAssets
    {
        public static void ReadRecords()
        {
            string root = $".\\{FolderNames.RecordFolder}\\";
            List<string> records = Traversal.GetAllFiles(root,1);
            foreach (string record in records)
            {
                Schema schema = new Schema(root + record);
                Bundles.LocalRecords.Add(schema);
            }
        }

        public static void ReadExperiments()
        {
            string root = $".\\{FolderNames.AssetsFolder}\\{FolderNames.AssetsExperimentLocalFolder}\\";
            List<string> files = Traversal.GetAllFiles(root, 1);
            foreach (string file in files)
            {
                IOExperiment experiment = new IOExperiment(root + file);
                Bundles.LocalExperiments.Add(experiment);
            }
        }

        public static void ReadLocalServices()
        {
            string file = $".\\{FolderNames.AssetsFolder}\\{FolderNames.AssetsServicesLocalFile}";
            string content = Read.ReadFileUTF8(file);
            JArray jArray = JArray.Parse(content);
            foreach(JToken token in jArray)
            {
                JObject item = token as JObject;
                if(item != null)
                {
                    MonitorService service = new MonitorService(item);
                    Bundles.LocalMonitorService.Add(service);
                }
            }

            // Bundles.LocalExperiments.Add(experiment);
        }
    }
}
