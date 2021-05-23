using UnityEngine;
using System.Collections.Generic;
using System.Threading;

namespace GameBasic
{
    public class UnityThread : Singleton<UnityThread>
    {
        Object lockObject = new Object();

        List<System.Action> tasks = new List<System.Action>();
        List<System.Collections.IEnumerator> coroutingTasks = new List<System.Collections.IEnumerator>();

        public int id;
        public MonoBehaviour mono;

        public override void OnCreate()
        {
            id = Thread.CurrentThread.ManagedThreadId;
        }

        public void AddTask(System.Action task)
        {
            if (IsUnityThread())
            {
                task();
            }
            else
            {
                lock (lockObject)
                {
                    tasks.Add(task);
                }
            }
        }

        public void StartCoroutine(System.Collections.IEnumerator i)
        {
            if (IsUnityThread())
                mono.StartCoroutine(i);
            else
            {
                lock (lockObject)
                {
                    coroutingTasks.Add(i);
                }
            }
        }

        public bool IsUnityThread()
        {
            return id == Thread.CurrentThread.ManagedThreadId;
        }

        public void Update()
        {
            int coroutingCount = coroutingTasks.Count;
            if (coroutingCount > 0)
            {
                lock (lockObject)
                {
                    for (int i = 0; i < coroutingCount; i++)
                    {
                        mono.StartCoroutine(coroutingTasks[i]);
                    }
                    coroutingTasks.Clear();
                }
            }

            int taskCount = tasks.Count;
            if(taskCount > 0)
            {
                lock (lockObject)
                {
                    for (int i = 0; i < taskCount; i++)
                    {
                        tasks[i]();
                    }
                    tasks.Clear();
                }
            }
        }
    }
}