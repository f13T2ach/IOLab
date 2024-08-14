using IOLab.Core.Assets;
using IOLab.Core.Util.FilesBehavior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOLab.Core.Local
{
    internal class ReadAssets
    {
        public static void ReadRecords()
        {
            string root = ".\\" + FolderNames.RecordFolder;
            List<string> records = Traversal.GetAllFiles(root,1);
        }
    }
}
