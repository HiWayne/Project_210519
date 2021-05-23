using UnityEngine;
using System.Collections;
using System;

namespace GameBasic
{
    [Serializable]
    public class Task : AbstractTask
    {
        public int id;
        public TimeCounter timer;
        public object data;

        bool running;
        internal float progress;

        public Action<Task> onStart;
        public Func<Task, bool> onUpdate;
        public Action<Task> onStop;

        // running instructions

        public bool StopByTimer { get { return timer.enable; } set { timer.enable = value; } }

        public Task()
        {

        }

        public Task(float time)
        {
            SetDuration(time);
        }

        public void SetDuration(float duration)
        {
            timer.enable = true;
            timer.target = duration;
        }

        public override bool OnUpdate()
        {
            if (status == TaskStatus.Kill)
                return false;

            // start
            if (!running)
            {
                progress = 0;
                running = true;
                timer.Start();
                if (onStart != null)
                    onStart.Invoke(this);
            }

            // update
            timer.Update();

            // use timer
            if (timer.enable)
            {
                progress = timer.Completion;
                running = !timer.ReachTarget();

                if (onUpdate != null)
                    onUpdate.Invoke(this);
            }
            else
            {
                running = onUpdate != null ? onUpdate(this) : false;
            }

            // stop
            if (status == TaskStatus.Stop)
                running = false;

            if (!running)
            {
                progress = 1;
                timer.End();
                if (onStop != null)
                    onStop.Invoke(this);
            }

            return running;
        }
    }
}