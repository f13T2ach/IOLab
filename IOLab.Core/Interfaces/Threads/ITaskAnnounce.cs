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
