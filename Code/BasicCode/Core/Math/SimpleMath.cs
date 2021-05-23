using UnityEngine;
using System.Collections.Generic;

namespace GameBasic
{
    public static class SimpleMath
    {
        /// <summary>
        /// weight will be modified
        /// </summary>
        /// <param name="weights"></param>
        /// <returns></returns>
        public static int WeightedRandom(List<float> weights)
        {
            float total = 0;
            for (int i = 0, length = weights.Count; i < length; i++)
            {
                total += weights[i];
                weights[i] = total;
            }

            int index = weights.Count - 1;
            float value = total * Random.value;
            for (int i = 0, length = weights.Count; i < length; i++)
            {
                if(weights[i] > value)
                {
                    index = i;
                    break;
                }
            }

            return index;
        }

        /// <summary>
        /// weightMax will not be modified
        /// </summary>
        /// <param name="weightMax"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static int WeightedRandom(List<float> weightMax, float total)
        {
            int index = weightMax.Count - 1;
            float value = total * Random.value;
            for (int i = 0, length = weightMax.Count; i < length; i++)
            {
                if (weightMax[i] > value)
                {
                    index = i;
                    break;
                }
            }

            return index;
        }

        /// <summary>
        /// Loop power
        /// </summary>
        /// <returns></returns>
        public static float Pow(float f, int p)
        {
            if (p == 0)
                return 1;

            for (int i = 0; i < p; i++)
                f *= f;
            return f;
        }
    }
}