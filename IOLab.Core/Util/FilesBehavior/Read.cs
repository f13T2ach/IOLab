using System.Text;

namespace IOLab.Core.Util.FilesBehavior
{
    public static class Read
    {
        /// <summary>
        /// UTF8读取文件
        /// </summary>
        /// <param name="path">绝对路径</param>
        /// <returns></returns>
        public static string? ReadFileUTF8(string path)
        {
            string? content = null;
            using (FileStream fs = File.OpenRead(path))
            {
                byte[] b = new byte[1024];
                UTF8Encoding temp = new UTF8Encoding(true);
                while (fs.Read(b, 0, b.Length) > 0)
                {
                    content += (temp.GetString(b) + "\n");
                }
            }
            return content;

        }

        public static string GetLastLevelName(string path)
        {
            string[] paths = path.Split("\\");
            if (paths[paths.Count() - 1] == "")
            {
                return paths[paths.Count() - 2];
            }
            return paths[paths.Count() - 1];
        }


    }
}
