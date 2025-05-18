using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IOLab.Core.Assets;

namespace IOLab.Core.Util.LabAI.LocalAI
{
    public static class LocalAIUtils
    {
        public static bool isLocalAIUtilsDownloaded()
        {
            string currentDirectory = Directory.GetCurrentDirectory();

            // 拼接 Toolchain/Ollama 文件夹的路径
            string targetPath = Path.Combine(currentDirectory, FolderNames.ToolChain, FolderNames.OllamaFolder);

            if (Directory.Exists(targetPath))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static (bool,string) DeleteAIToolChain()
        {
            // 获取当前工作目录
            string currentDirectory = Directory.GetCurrentDirectory();

            // 拼接 ollama 文件夹的路径
            string ollamaPath = Path.Combine(currentDirectory, "ollama");

            // 检查文件夹是否存在
            if (Directory.Exists(ollamaPath))
            {
                try
                {
                    // 删除文件夹及其所有子文件和文件夹
                    Directory.Delete(ollamaPath, true); // true 表示递归删除
                    return (true,"文件夹及其内容已成功删除。");
                }
                catch (Exception ex)
                {
                    // 捕获并处理可能的异常
                    return (false,$"删除文件夹时出错: {ex.Message}");
                }
            }
            else
            {
                return (false,"工具链不存在，无需删除。");
            }
        }
    }
}
