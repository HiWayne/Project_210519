using UnityEngine;
using UnityEditor;
using System;

namespace GameBasic.IO
{
    public struct FileWrite
    {
        public string path;
        public Func<string> onStart;
    }
}