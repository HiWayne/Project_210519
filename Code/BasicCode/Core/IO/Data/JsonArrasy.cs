using System;
using System.Collections.Generic;

namespace GameBasic.IO
{
    [Serializable]
    public class JsonArray<T>
    {
        public const string ARRAY_NAME = "datas";

        public List<T> datas;

        public JsonArray() {
            datas = new List<T>();
        }

        public JsonArray(List<T> datas) {
            this.datas = datas;
        }

        public void Clear()
        {
            datas.Clear();
        }
    }
}