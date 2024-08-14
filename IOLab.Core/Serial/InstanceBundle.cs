using IOLab.Core.Interfaces.Threads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOLab.Core.Serial
{
    public static class InstanceBundle
    {
        public static volatile List<ITaskAnnounce>? list;
    }
}
