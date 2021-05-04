using UnityEngine;
using System.Collections.Generic;

namespace GameBasic
{
    /// <summary>
    /// Cache object by instanceId in a dictionary
    /// </summary>
    public class ObjectDic
    {
        public Dictionary<int, Object> caches = new Dictionary<int, Object>();

        public Object Get(int id)
        {
            Object result;
            caches.TryGetValue(id, out result);
            return result;
        }

        public T Get<T>(int id) where T : Object
        {
            T result = null;
            Object obj;
            if(caches.TryGetValue(id, out obj))
            {
                result = obj as T;
            }

            return result;
        }

        public void Add(int id, Object obj)
        {
            if (caches.ContainsKey(id))
                return;
            caches.Add(id, obj);
        }

        public void Remove(int id)
        {
            caches.Remove(id);
        }
    }
}