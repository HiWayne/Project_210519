using UnityEngine;

namespace GameBasic
{
    public struct Line
    {
        public Vector3 start;
        public Vector3 end;

        public Vector3 Lerp(float amount)
        {
            return Vector3.Lerp(start, end, amount);
        }

        /// <summary>
        /// Square interpolate
        /// </summary>
        /// <param name="amount"></param>
        public Vector3 Serp(float amount)
        {
            amount = amount - 1;
            amount = 1 - amount * amount;
            return Vector3.Lerp(start, end, amount);
        }
    }
}