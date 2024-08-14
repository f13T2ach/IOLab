using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOLab.Interfaces.ViewModel
{
    public interface INameEditViewModel
    {
        public static string? Title { get; set; }
        public static string? TextContent { get; set; }
    }
}
