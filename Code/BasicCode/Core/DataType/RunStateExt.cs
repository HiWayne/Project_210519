using UnityEngine;
using System.Collections;

namespace GameBasic
{
    public static class RunStateExt
    {
        public static bool Has(this RunState m1, RunState m2)
        {
            return (m1 & m2) != 0;
        }
    }
}