using UnityEngine;
using System.Collections.Generic;
using System;

namespace GameBasic.IO
{
    public class FilePersist : PoolingTask, IProgress
    {
        // volatile?
        int completeCount;
        int total;

        public List<FileRead> readTasks;
        public List<FileWrite> writeTasks;

        FilePersistTask read;
        FilePersistTask write;
        public Action onComplete;
        public bool clearOnComplete = true;

        public int Complete => completeCount;
        public int Total => readTasks.Count + writeTasks.Count;

        public FilePersist()
        {
            readTasks = new List<FileRead>();
            writeTasks = new List<FileWrite>();
            read = new FilePersistTask(FilePersistTask.Type.Read);
            write = new FilePersistTask(FilePersistTask.Type.Write);
        }

        public void Clear()
        {
            onComplete = null;
            readTasks.Clear();
            writeTasks.Clear();
            total = 0;
            completeCount = 0;
        }

        protected override void ExecuteImpl()
        {
            completeCount = 0;
            total = readTasks.Count + writeTasks.Count;

            // write
            for (int i = 0, length = writeTasks.Count; i < length; i++)
            {
                FileWrite task = writeTasks[i];

                write.data = task.onStart();
                write.path = task.path;
                write.Execute();
                write.data = null;

                Progress();
            }

            // read
            for (int i = 0, length = readTasks.Count; i < length; i++)
            {
                FileRead task = readTasks[i];

                read.data = null;
                read.path = task.path;
                read.Execute();
                task.onComplete.Invoke(read.data);

                Progress();
            }

            onComplete?.Invoke();

            if (clearOnComplete)
                Clear();
        }

        void Progress()
        {
            completeCount++;
            UpdateProgress((float)completeCount / total);
        }

        public override string ToString()
        {
            string result = Complete + "/" + Total + "\nWrite:";
            for (int i = 0; i < writeTasks.Count; i++)
                result += "\n" + writeTasks[i].path;
            result += "\nRead:";
            for (int i = 0; i < readTasks.Count; i++)
                result += "\n" + readTasks[i].path;

            return result;
        }
    }
}