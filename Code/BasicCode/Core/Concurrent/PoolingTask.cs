using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace GameBasic
{
    public class PoolingTask
    {
        /// <summary>
        /// report progess
        /// </summary>
        public Action<float> onProgress;
        protected volatile float progress = 0;
        protected bool autoReportProgress = true;
        volatile bool running = false;
        // lambda cache
        WaitCallback execute;

        /// <summary>
        /// Submit to thread pool
        /// </summary>
        public void Submit()
        {
            if (execute == null)
                execute = Execute;

            if (running)
                return;
            running = true;
            ThreadPool.QueueUserWorkItem(execute);
        }

        /// <summary>
        /// Execute the task, called from thread pool.
        /// </summary>
        public void Execute(object obj = null)
        {
            try
            {
                progress = 0;
                if (autoReportProgress)
                    UpdateProgress(0);
                ExecuteImpl();
                if (autoReportProgress)
                    UpdateProgress(1);
            }
            catch (Exception e){
                Debug.LogException(e);
            }

            running = false;
        }


        /// <summary>
        /// For subclass to extend, called from <seealso cref="Execute"/>
        /// </summary>
        protected virtual void ExecuteImpl()
        {
        }

        public float GetProgress()
        {
            return progress;
        }

        public bool IsRunning()
        {
            return running;
        }

        public bool IsDone()
        {
            return !running && progress >= 1;
        }

        protected void UpdateProgress(float progress)
        {
            this.progress = progress;
            // inform progression
            if (onProgress != null)
                onProgress(progress);
        }
    }
}