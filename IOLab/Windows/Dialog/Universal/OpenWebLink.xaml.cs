using iNKORE.UI.WPF.Modern.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IOLab.Windows.Dialog.Universal
{
    /// <summary>
    /// OpenWebLink.xaml 的交互逻辑
    /// </summary>
    public partial class OpenWebLink : ContentDialog
    {
        public OpenWebLink(string link = "http://localhost")
        {
            InitializeComponent();
            MyWebLink.Text = link;
        }

        // 方法来自 https://blog.csdn.net/InfSein/article/details/125975626 有改动
        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            dynamic? kstr;
            string url = MyWebLink.Text;
            string s;
            try
            {
                // 从注册表中读取默认浏览器可执行文件路径
                RegistryKey? key2 = Registry.ClassesRoot.OpenSubKey(@"http\shell\open\command\");
                RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice\");
                if (key != null)
                {
                    kstr = key.GetValue("ProgId");
                    if (kstr != null)
                    {
                        s = kstr.ToString();
                        // "ChromeHTML","MSEdgeHTM" etc.
                        if (Registry.GetValue(@"HKEY_CLASSES_ROOT\" + s + @"\shell\open\command", null, null) is string path)
                        {
                            var split = path.Split('"');
                            path = split.Length >= 2 ? split[1] : "";
                            if (path != "")
                            {
                                Process.Start(path, url);
                                return;
                            }
                        }
                    }
                }
                if (key2 != null)
                {
                    kstr = key2.GetValue("");
                    if (kstr != null)
                    {
                        s = kstr.ToString();
                        var lastIndex = s.IndexOf(".exe", StringComparison.Ordinal);
                        if (lastIndex == -1)
                        {
                            lastIndex = s.IndexOf(".EXE", StringComparison.Ordinal);
                        }
                        var path = s.Substring(1, lastIndex + 3);
                        var result1 = Process.Start(path, url);
                        if (result1 == null)
                        {
                            var result2 = Process.Start("explorer.exe", url);
                            if (result2 == null)
                            {
                                Process.Start(url);
                            }
                        }
                    }
                }
                else
                {
                    var result2 = Process.Start("explorer.exe", url);
                    if (result2 == null)
                    {
                        Process.Start(url);
                    }
                }
            }
            catch
            {
                
            }
        }
    }
}
