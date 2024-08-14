using IOLab.Core.Assets;
using IOLab.Core.Interfaces.Threads;

namespace IOLab.Core.Tasks
{
    public class ShakeHandTask : ITaskAnnounce
    {

        private string _tag;

        public Task<object> task;

        public ITaskHandler<object> handler;

        public string Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }



        public ShakeHandTask()
        {
            Tag = Tags.ShakeHand;

            handler = new ShakeHand();
            task = new Task<object>(handler.Handler);
            task.Start();


        }

        public async void GetResult()
        {
            await task.ConfigureAwait(false);
            return;
        }


        public Task GetTaskInstance()
        {
            return task;
        }

        public ITaskHandler<object> GetHandlerInstance()
        {
            return handler;
        }
    }
}
