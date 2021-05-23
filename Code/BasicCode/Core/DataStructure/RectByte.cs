using UnityEngine;
using System.Collections;

namespace GameBasic
{
    [System.Serializable]
    public struct RectByte
    {
        public byte xMin;
        public byte yMin;
        public byte width;
        public byte height;

        public int xMax { get { return xMin + width; } }
        public int yMax { get { return yMin + height; } }
    }
}
