using UnityEngine;
using System.Collections.Generic;
using System;

namespace GameBasic
{
    /// <summary>
    /// TODO: Multi-thread unsafe
    /// </summary>
    public class TaskRunner 
    {
        static TaskRunner Instance;

        List<ITask> tasks;

        public TaskRunner()
        {
            tasks = new List<ITask>();
        }

        public static void SetDefault(TaskRunner runner)
        {
            Instance = runner;
        }

        public static void AddToDefault(ITask task)
        {
            if (Instance == null)
                return;

            Instance.Add(task);
        }

        public void Add(ITask task)
        {
            if (task.Status == TaskStatus.Running)
                return;

            tasks.Add(task);
            task.Status = TaskStatus.Running;
        }

        public void Update()
        {
            if (tasks.Count == 0)
                return;

            tasks.RemoveAll(t => {
                bool remove = true;

                try
                {
                    remove = !t.OnUpdate();
                }
                catch (Exception e)
                {
                    remove = true;
                    Debug.LogException(e);
                }

                if (remove)
                    t.Status = TaskStatus.End;

                return remove;
            });
        }

        public bool HasTask()
        {
            return tasks.Count > 0;
        }

        public void ClearTask()
        {
            tasks.Clear();
        }
    }
}