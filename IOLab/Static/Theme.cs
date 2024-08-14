using iNKORE.UI.WPF.Modern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace IOLab.Static
{
    class Theme
    {
        private static ApplicationTheme _themeMode;
        public static ApplicationTheme themeMode
        { 
            get { return _themeMode; }
            set { 
                _themeMode = value;
                MainWindow.ChangeTheme(_themeMode);
            }
        }

        public static List<string> coloursHex = new List<string>
        {
            "#0078D7",  // 微软蓝
            "#E62129",  // 一中红
            "#ffb8c6",  // 樱粉
            "#ffae00",  // 橘橙
            "#16982b",  // 柳绿
        };
    }
}
