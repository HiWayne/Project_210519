using UnityEngine;
using System.Collections;
using System;

namespace GameBasic
{
    public class IndexMask
    {
        public int[] masks;
        public int onBitCount;

        public IndexMask(int size)
        {
            masks = new int[size];
        }

        public void Set(int index, bool on)
        {
            int bit = 1 << (index & 31);
            int i = index >> 5;
            int result = on ? masks[i] | bit : masks[i] & (~bit);

            if(masks[i] != result)
                onBitCount += on ? 1 : -1;

            masks[i] = result;

            //Debug.Log(new Vector2Int(i, result));
        }

        public void ForeachOnBit(Action<int> act, bool clear)
        {
            if (onBitCount == 0)
                return;

            int count = onBitCount;
            int highBit = 0;
            for (int i = 0; i < masks.Length; i++)
            {
                int mask = masks[i];
                if (clear)
                    masks[i] = 0;

                for (int j = 0; j < 32; j++)
                {
                    if (mask == 0)
                        break;

                    // is bit on?
                    if ((mask & 1) != 0 )
                    {
                        act(j + highBit);
                        count--;
                        if (count == 0)
                            return;
                    }

                    mask >>= 1;
                }
                highBit += 32;
            }
        }

        public void Clear()
        {
            for (int i = 0; i < masks.Length; i++)
                masks[i] = 0;
            onBitCount = 0;
        }

        public int this[int index]
        {
            get { return masks[index]; }
            set { masks[index] = value; }
        }
    }
}