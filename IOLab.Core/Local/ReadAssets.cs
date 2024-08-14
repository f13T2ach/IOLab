using IOLab.Core.Assets;
using IOLab.Core.Models;
using IOLab.Core.Models.Records;
using IOLab.Core.Util.FilesBehavior;
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
            string root = ".\\" + FolderNames.RecordFolder + "\\";
            List<string> records = Traversal.GetAllFiles(root,1);
            foreach (string record in records)
            {
                Schema schema = new Schema(root + record);
                Bundles.LocalRecords.Add(schema);
            }
        }

        public static void ReadExperiments()
        {
            string root = ".\\" + FolderNames.AssetsFolder + "\\" + FolderNames.AssetsExperimentLocalFolder + "\\";
            List<string> files = Traversal.GetAllFiles(root, 1);
            foreach (string file in files)
            {
                IOExperiment experiment = new IOExperiment(root + file);
                Bundles.LocalExperiments.Add(experiment);
            }
        }
    }
}
