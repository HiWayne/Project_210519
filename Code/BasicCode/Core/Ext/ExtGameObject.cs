using UnityEngine;

namespace GameBasic
{
    public static class ExtGameObject
    {
        public static T GetComponentInDirectChildren<T>(this GameObject obj) where T : Component
        {
            for (int i = 0; i < obj.transform.childCount; i++)
            {
                T t = obj.transform.GetChild(i).GetComponent<T>();
                if (t != null)
                    return t;
            }
            return null;
        }

        public static T GetComponentInDirectChildren<T>(this Component com) where T : Component
        {
            for (int i = 0; i < com.transform.childCount; i++)
            {
                T t = com.transform.GetChild(i).GetComponent<T>();
                if (t != null)
                    return t;
            }
            return null;
        }

        public static void ForeachChild(this Transform tr, System.Func<int, Transform, bool> act, bool recursive = false)
        {
            int length = tr.childCount;
            for (int i = 0; i < length; i++)
            {
                Transform c = tr.GetChild(i);
                bool stop = act(i, c);
                if (stop)
                    return;
            }
        }

        /*
        public static bool Foreach(int index, Transform tr, System.Func<int, Transform, bool> act, bool recursive)
        {
            // self
            if (act(index, tr) || recursive)
                return false;

            int length = tr.childCount;
            for (int i = 0; i < length; i++)
            {
                if(TransformAct(i, tr.GetChild(i), recursive)
            }
        }
        */
    }
}