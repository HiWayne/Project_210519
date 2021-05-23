using UnityEngine;
using System.Collections;

namespace GameBasic
{
    public static class ExtFloat
    {
        public static float Serp(this float number, bool accelerate = false)
        {
            if (accelerate)
            {
                return number * number;
            }
            else
            {
                number = number - 1;
                return 1 - number * number;
            }
        }
    }
}