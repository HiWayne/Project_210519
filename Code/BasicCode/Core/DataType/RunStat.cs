using UnityEngine;
using System.Collections;

namespace GameBasic
{
    public struct RunStat
    {
        public readonly static RunStat None = new RunStat();
        public readonly static RunStat All = new RunStat(true, true, true);
        public readonly static RunStat StartUpdate = new RunStat(true, true, false);
        public readonly static RunStat StopUpdate = new RunStat(false, true, true);

        //
        public bool start;
        public bool update;
        public bool stop;
        // public bool paused;

        public RunStat(bool start, bool update, bool end)
        {
            this.start = start;
            this.update = update;
            this.stop = end;
        }

        public RunStat(float pOld, float pNew)
        {
            this.start = pOld == 0;
            this.update = true;
            this.stop = pNew == 1;
        }

        public RunStat(RunState mask)
        {
            start = (mask & RunState.Start) == RunState.Start;
            update = (mask & RunState.Update) == RunState.Update;
            stop = (mask & RunState.Stop) == RunState.Stop;
        }
    }
}