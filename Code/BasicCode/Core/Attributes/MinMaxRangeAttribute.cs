using UnityEngine;
using System;

namespace GameBasic
{
    [AttributeUsage(AttributeTargets.Field)]
    public class MinMaxRangeAttribute : PropertyAttribute
    {
        public readonly float max;
        public readonly float min;
        public bool HideLabel { get; set; }
        //public readonly bool drawLabel;

        public MinMaxRangeAttribute(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
    }
}