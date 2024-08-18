using System.Diagnostics;

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
