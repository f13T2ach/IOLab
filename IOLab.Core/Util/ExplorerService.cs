using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOLab.Core.Util
{
    public class ExplorerService
    {
        public static void OpenWithExplorer(string url)
        {

            Process.Start(new ProcessStartInfo()
            {
                FileName = url,
                UseShellExecute = true
            });
        }
    }
}
