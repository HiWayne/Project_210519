using UnityEngine;
using System.Collections.Generic;
using System;

namespace GameBasic
{
    public class TaskList<T, K>
    {
        public List<T> tasks;
        public int total;
        public int complete;
        public float progress;
        public Action<int, K> onProgress;

        public TaskList()
        {

        }

        public TaskList(List<T> taskList)
        {
            this.tasks = taskList;
            Init();
        }

        public TaskList(List<T> taskList, Action<int, K> onProgress)
        {
            this.tasks = taskList;
            this.onProgress = onProgress;
            Init();
        }

        public void Init()
        {
            total = tasks.Count;
            complete = 0;
            progress = 0;
        }

        public void OnProgress(int taskId, K obj)
        {
            complete++;
            progress = (float)complete / total;

            if (onProgress != null)
                onProgress.Invoke(taskId, obj);
        }

        public void OnProgress(K obj)
        {
            int taskId = complete;

            complete++;
            progress = (float)complete / total;

            if (onProgress != null)
                onProgress.Invoke(taskId, obj);
        }

        public bool IsDone()
        {
            return total == complete;
        }
    }
}