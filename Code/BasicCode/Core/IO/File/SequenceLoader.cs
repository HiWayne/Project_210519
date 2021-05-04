using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace GameBasic.IO
{
    public class SequenceLoader : PoolingTask
    {
        public int loadCount = 0;
        public List<FileRead> tasks;
        FilePersistTask task;

        public SequenceLoader()
        {
            tasks = new List<FileRead>();
            task = new FilePersistTask();
            task.type = FilePersistTask.Type.Read;
        }

        protected override void ExecuteImpl()
        {
            loadCount = 0;
            for (int i = 0; i < tasks.Count; i++)
            {
                task.data = null;
                task.path = tasks[i].path;
                task.Execute();
                tasks[i].onComplete(task.data);

                // Debug.Log(tasks[i].path + " loaded");

                loadCount++;
                UpdateProgress((float)(i + 1) / tasks.Count);
            }

            //
            tasks.Clear();
        }
    }
}