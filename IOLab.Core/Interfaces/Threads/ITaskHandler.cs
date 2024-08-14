using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOLab.Core.Interfaces.Threads
{
    public interface ITaskHandler<TType>
    {
        public TType Handler();

        public string Parameter { get; set; }
    }
}
