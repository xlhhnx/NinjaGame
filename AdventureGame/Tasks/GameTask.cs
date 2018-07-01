using System.Threading.Tasks;

namespace NinjaGame.Tasks
{
    public abstract class GameTask
    {
        protected Task _task;

        public GameTask(Task task)
        {
            Task = task;
        }

        public void Start()
        {
            Task.Start();
        }

        public bool IsCompleted { get { return Task.IsCompleted; } }
        public TaskStatus Status { get { return Task.Status; } }
        public Task Task { get => _task; set => _task = value; }
    }
}
