using UnityEngine;
using System.Collections;
using System;

namespace GameBasic.IO
{
    public struct FileRead
    {
        public string path;
        public Action<string> onComplete;

        public FileRead(string path, Action<string> onComplete)
        {
            this.path = path;
            this.onComplete = onComplete;
        }
    }
}