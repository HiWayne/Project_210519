using UnityEngine;
using System;

namespace GameBasic
{
    public class EnumAttr : PropertyAttribute
    {
        public Type enumType;

        public EnumAttr(Type enumType)
        {
            this.enumType = enumType;
        }
    }
}