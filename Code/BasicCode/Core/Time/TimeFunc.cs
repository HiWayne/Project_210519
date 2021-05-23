using UnityEngine;
using System;

namespace GameBasic
{
    public class TimeFunc
    {
        public static Func<float> DEFAULT_TIMER = Delta;
        public readonly static Func<float> DELTA_TIMER = Delta;
        public readonly static Func<float> UNSCALED_TIMER = UnscaledDelta;
        public readonly static Func<float> FIXED_TIMER = Fixed;

        /// <summary>
        /// For Unity fixed update
        /// </summary>
        public static float Fixed()
        {
            return Time.fixedDeltaTime;
        }
        /// <summary>
        /// For Unity update
        /// </summary>
        public static float Delta()
        {
            return Time.deltaTime;
        }

        public static float UnscaledDelta()
        {
            return Time.unscaledDeltaTime;
        }
    }
}