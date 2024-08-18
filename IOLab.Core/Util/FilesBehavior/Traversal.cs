using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IOLab.Core.Util;

namespace IOLab.Core.Util.FilesBehavior
{
    public class Traversal
    {
        /// <summary>
        /// 获取所有文件夹下文件
        /// </summary>
        /// <param name="dir">文件夹地址</param>
        /// <param name="level">搜索深度</param>
        public static List<string> GetAllFiles(string dir,int level)
        {
            DirectoryInfo d = new DirectoryInfo(dir);
            FileSystemInfo[] fsinfos;
            try
            {
                fsinfos = d.GetFileSystemInfos();
            }
            catch
            {
                DirectoryInfo newdir = new DirectoryInfo(dir);
                newdir.Create();
                return null;
            }
            List<string> dirs = new List<string>();
            foreach (FileSystemInfo fsinfo in fsinfos)
            {
                if (fsinfo is DirectoryInfo && level > 1)
                {
                    dirs = Collection.Merge<string>(dirs,GetAllFiles(fsinfo.FullName,level-1));
                }
                else
                {
                    dirs.Add(fsinfo.Name);
                }
            }
            return dirs;

        }

    }
}
