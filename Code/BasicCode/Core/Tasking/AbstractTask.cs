using UnityEngine;

namespace GameBasic
{
    public abstract class AbstractTask : ITask
    {
        protected TaskStatus status;
        public TaskStatus Status { get { return status; } set { status = value; } }

        public void Submit()
        {
            TaskRunner.AddToDefault(this);
        }

        public void Stop()
        {
            status = TaskStatus.Stop;
        }

        public void Kill()
        {
            status = TaskStatus.Kill;
        }

        public abstract bool OnUpdate();
    }
}