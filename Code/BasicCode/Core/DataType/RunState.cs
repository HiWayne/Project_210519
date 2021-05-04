using UnityEngine;
using System.Collections;

namespace GameBasic
{
    public enum RunState // : byte
    {
        None = 0,
        Start = 1,
        Update = 2,
        Stop = 4,

        //
        StartUpdate = Start | Update,
        StopUpdate = Stop | Update,
        All = Start | Update | Stop
    }
}