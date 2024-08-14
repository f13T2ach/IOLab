using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iNKORE.UI.WPF.Modern.Controls.Primitives;
using IOLab.Interfaces.ViewModel;
namespace IOLab.Util.StaticModel
{
    class DeviceName : INameEditViewModel
    {
        private static string? _title;
        public static string? Title {
            get
            {
                return _title;
            }
            set
            {
                // 只读
            }
        }

        private static string? _textContent;
        public static string? TextContent
        {
            get
            {
                return _textContent;
            }
            set
            {
                _textContent = value;
            }
        }

        public DeviceName()
        {
            _title = "修改设备名称";
        }
    }
}
