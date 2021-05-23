using System.Collections.Generic;
using System;

namespace GameBasic
{
    public static class ExtCollection
    {
        #region List

        public static bool IsLegalIndex<T>(this List<T> list, int index)
        {
            return index >= 0 && index < list.Count;
        }

        public static bool IsLegalIndex(this object[] array, int index)
        {
            return index >= 0 && index < array.Length;
        }

        public static bool IsNullOrEmpty<T>(this List<T> list)
        {
            return list == null || list.Count == 0;
        }

        public static Dictionary<K, V> ToDictionary<K, V>(this List<V> list, Func<V, K> keyGetter)
        {
            Dictionary<K, V> result = new Dictionary<K, V>(list.Count);
            for (int i = 0, length = list.Count; i < length; i++)
            {
                V item = list[i];
                result[keyGetter(item)] = item;
            }

            return result;
        }

        public static Dictionary<K, V> ToDictionary<K, V>(this V[] array, Func<V, K> keyGetter)
        {
            Dictionary<K, V> result = new Dictionary<K, V>(array.Length);
            for (int i = 0, length = array.Length; i < length; i++)
            {
                V item = array[i];
                result[keyGetter(item)] = item;
            }

            return result;
        }

        public static void EnsureCapacity<T>(this List<T> list, int capacity)
        {
            if (list.Capacity < capacity)
                list.Capacity = capacity;
            /*
            if (list.Capacity >= capacity)
                return list;
            if (list.Count == 0)
                return new List<T>(capacity);

            List<T> result = new List<T>(capacity);
            result.AddRange(list);
            return result;
            */
        }

        public static void ForEach<T>(this List<T> list, Action<int, T> act)
        {
            int length = list.Count;
            for (int i = 0; i < length; i++)
                act(i, list[i]);
        }
        #endregion

        #region Array
        public static void ForEach<T>(this T[,] array, Action<int, int, T[,]> action)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    action(i, j, array);
                }
            }
        }
        #endregion

        public static void AddAll<K, V>(this Dictionary<K, V> dic, List<V> values, Func<V, K> keyGetter)
        {
            int length = values.Count;
            for (int i = 0; i < length; i++)
                dic.Add(keyGetter(values[i]), values[i]);
        }

        public static void AddAll<K, V>(this Dictionary<K, V> dic, V[] values, Func<V, K> keyGetter)
        {
            for (int i = 0; i < values.Length; i++)
                dic.Add(keyGetter(values[i]), values[i]);
        }
    }
}