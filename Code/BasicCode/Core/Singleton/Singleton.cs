using UnityEngine;
using System.Collections;

namespace GameBasic
{

    public class Singleton<T> where T : Singleton<T>, new()
    {
        public static T Instance;

        public static T Create()
        {
            if (Instance == null)
            {
                Instance = new T();
                Instance.OnCreate();
            }
            return Instance;
        }

        public virtual void OnCreate()
        {

        }

        public static void Destroy()
        {
            if (Instance != null)
                Instance.OnDestroy();
            Instance = null;
        }

        protected virtual void OnDestroy()
        {

        }
    }
}