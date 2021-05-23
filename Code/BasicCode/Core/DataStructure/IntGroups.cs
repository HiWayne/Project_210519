using UnityEngine;
using System;

namespace GameBasic
{
    [Serializable]
    public class IntGroups
    {
        public ushort[] partition;
        public ushort[] data;

        public int GroupCount { get { return (partition == null || partition.Length == 0) ? 1 : partition.Length; } }

        /// <summary>
        /// Get the start index and end index (Exclusive)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Vector2Int GetGroup(int id)
        {
            if (partition == null || partition.Length <= 1)
                return new Vector2Int(0, data.Length);

            int start = partition[id];
            // is last?
            int length = id == partition.Length - 1 ? data.Length : partition[id + 1];
            return new Vector2Int(start, length);
        }

        public int GetSize(int groupId)
        {
            // start(n+1) - start(n)
            return (groupId == partition.Length - 1 ? data.Length : partition[groupId + 1]) - partition[groupId];
        }

        public int Get(int groupId, int index)
        {
            if (partition == null || partition.Length <= 1)
                return data[index];

            return data[partition[groupId] + index];
        }

        public void ForGroup(int groupId, Action<int> action)
        {
            Vector2Int group = GetGroup(groupId);

            for (int i = group.x; i < group.y; i++)
                action(data[i]);
        }
    }
}