using IOLab.Core.Interfaces.Threads;

namespace IOLab.Core
{
    public class TaskPool
    {
        List<ITaskAnnounce> tasks;

        public TaskPool()
        {
            tasks = new List<ITaskAnnounce>();
        }

        public ITaskAnnounce FindTaskByTag(string tag)
        {
            foreach (ITaskAnnounce announcer in tasks)
            {
                if (announcer.Tag == tag)
                {
                    return announcer;
                }
            }
            return null;
        }


        public void AddTaskToPool(ITaskAnnounce task)
        {
            tasks.Add(task);
        }

        public void ForceCloseByTag(string tag)
        {

        }
    }
}
