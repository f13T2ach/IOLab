using IOLab.Core.Interfaces.Threads;
using IOLab.Core.Serial;

namespace IOLab.Core.Tasks
{
    public class TaskAnnouncer : ITaskAnnounce
    {
        public CancellationTokenSource tokenSource { get; set; }
        public CancellationToken cancellationToken { get; set; }

        private string _tag;

        public Task<object> task;

        public ITaskHandler<object> handler;

        public string Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }



        public TaskAnnouncer(ITaskHandler<object> handle, string tag)
        {
            tokenSource = new CancellationTokenSource();
            cancellationToken = tokenSource.Token;

            Tag = tag;

            handler = handle;
            handler.Announcer = this;
            task = new Task<object>(handler.Handler, cancellationToken);
            task.Start();
            MemoryReserve();

        }

        public async void MemoryReserve()
        {
            await GetTaskInstance().ConfigureAwait(GetTaskInstance().IsCompleted);
            if (InstanceBundle.list != null)
            {
                try
                {
                    InstanceBundle.list.Remove(this);
                }
                catch { }
            }
            if (task != null)
            {
                task.Dispose();
            }
        }
        public Task GetTaskInstance()
        {
            return task;
        }

        public ITaskHandler<object> GetHandlerInstance()
        {
            return handler;
        }

        public void StopTask()
        {
            if (task != null)
            {
                tokenSource.Cancel();
                task.Dispose();
            }
        }
    }
}
