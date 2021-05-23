using UnityEngine;
using System.Collections;

namespace GameBasic
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        public static T Instance;

        protected virtual void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("[Singleton] More than 1 instance created: " + typeof(T));
                Destroy(this);
            }
            else
                Instance = (T)this;
        }

        protected virtual void OnDestroy()
        {
            Instance = null;
        }
    }
}