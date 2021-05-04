using UnityEngine;
using System.Collections;
using System.Globalization;

namespace GameBasic
{
    public class TextUtil
    {
        public static float Parse(string data)
        {
            return float.Parse(data, CultureInfo.InvariantCulture);
        }

        public static bool TryParse(string data, out float result)
        {
            return float.TryParse(data, NumberStyles.Float, CultureInfo.InvariantCulture, out result);
        }

        public static string ToString(float f)
        {
            return f.ToString(CultureInfo.InvariantCulture);
        }
    }
}