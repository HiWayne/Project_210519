using UnityEngine;
using System.Collections;
using System;

namespace GameBasic
{
    public class FunctionFactory
    {
        public static Func<GameObject, string> getObjectName = GetName;
        public static Func<GameObject, string> getMonoName = GetName;
        public static Func<GameObject, string> getSpriteName = GetName;

        public static string GetName(GameObject t)
        {
            return t.name;
        }

        public static string GetName(MonoBehaviour t)
        {
            return t.name;
        }

        public static string GetName(Sprite t)
        {
            return t.name;
        }
    }
}