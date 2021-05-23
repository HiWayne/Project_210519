using UnityEngine;
using System;

namespace GameBasic
{
    [System.Serializable]
    public struct TimeCounter
    {
        public Func<float> timer;// = TimeFunc.DEFAULT_TIMER;

        public bool enable;
        [HideInInspector]
        public float time;
        public float target;

        public TimeCounter(float target)
        {
            timer = TimeFunc.DEFAULT_TIMER;
            enable = true;
            this.time = 0;
            this.target = target;
        }

        /*
        /// <summary>
        /// enabled: target >= 0; 
        /// set enabled true: target = target > 0 ? target : 0
        /// </summary>
        public bool Enabled {
            get { return target >= 0; }
            set { target = value ? (target > 0 ? target : 0) : -1; }
        }
        */

        /// <summary>
        /// Completion percentage [0, 1]
        /// </summary>
        public float Completion
        {
            get
            {
                if (target > 0)
                    return Mathf.Clamp01(time / target);
                else if (target < 0)
                    return 0;
                else
                    return 1;
            }
        }

        public bool Update()
        {
            if (timer == null)
                timer = TimeFunc.DEFAULT_TIMER;

            time = time + timer();

            return time >= target;
        }

        /// <summary>
        /// Update, restart if reached target.
        /// </summary>
        /// <returns></returns>
        public bool UpdateRestart()
        {
            bool reachTarget = Update();

            if (reachTarget)
                Start();

            return reachTarget;
        }

        /*
        /// <summary>
        /// For convinient usage
        /// </summary>
        /// <param name="act"></param>
        /// <param name="stopAct"></param>
        public void Update(Action act, Action stopAct = null)
        {
            if (time < target)
            {
                Update();
                act?.Invoke();

                if(time >= target)
                    stopAct?.Invoke();
            }
        }
        */

        public bool ReachTarget()
        {
            return time >= target;
        }

        public void SetTarget(float target)
        {
            this.target = target;
        }

        /// <summary>
        /// Is target > 0 ?
        /// </summary>
        /// <returns></returns>
        public bool HasTarget()
        {
            return target > 0;
        }

        /// <summary>
        /// Reset the time to zero.
        /// </summary>
        public void Start()
        {
            time = 0;
        }

        /// <summary>
        /// Has target and time is zero.
        /// </summary>
        /// <returns></returns>
        public bool IsStart()
        {
            return target > 0 && time == 0;
        }

        public void End()
        {
            time = target;
        }

        public void SetTime(float time)
        {
            this.time = time;
        }

        public float GetTime()
        {
            return time;
        }

        public float GetNormalizedTime()
        {
            return target > 0 ? time / target : 0;
        }
    }
}


