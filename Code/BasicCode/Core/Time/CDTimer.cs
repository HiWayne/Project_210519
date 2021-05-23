using System;

namespace GameBasic
{
    [Serializable]
    public struct CDTimer{
        public bool startInst;
        public TimeCounter timeCount;

        public bool Enabled
        {
            get
            {
                return timeCount.enable;
            }
            set
            {
                timeCount.enable = value;
            }
        }

        /// <summary>
        /// Set the timer target, and set to ready
        /// </summary>
        public float Target
        {
            get { return timeCount.target; }
            set {
                timeCount.target = value;
                timeCount.End();
            }
        }

        public void Start()
        {
            timeCount.Start();
        }

        public void End()
        {
            timeCount.End();
        }

        public void Update()
        {
            timeCount.Update();
        }

        public bool ReachTarget()
        {
            return timeCount.ReachTarget();
        }
    }

}