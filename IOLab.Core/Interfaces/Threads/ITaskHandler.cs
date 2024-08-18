namespace IOLab.Core.Interfaces.Threads
{
    public interface ITaskHandler<TType>
    {
        public TType Handler();

        public string Parameter { get; set; }
    }
}
