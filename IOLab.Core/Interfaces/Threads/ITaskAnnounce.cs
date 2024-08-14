using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IOLab.Core.Interfaces.Threads
{
    public interface ITaskAnnounce
    {
        public void GetResult();

        public Task GetTaskInstance();

        public ITaskHandler<object> GetHandlerInstance();

        public string Tag { get; set; }
    }
}
