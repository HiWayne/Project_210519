using UnityEngine;

using System;
using System.Collections;
using System.Threading;
using System.Globalization;

namespace GameBasic
{
    public class SystemUtil
    {
        public static string GetSystemInfo()
        {
            return "OS: " + Environment.OSVersion + " | " + CultureInfo.CurrentCulture + "/" + CultureInfo.CurrentUICulture + "(UI)";
        }
    }
}