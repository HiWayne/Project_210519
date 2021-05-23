using UnityEngine;
using UnityEditor;

namespace GameBasic
{
    public class GameBasicGlobal
    {
        public const string AM_Root = "GameBasic";
        public const string AM_Anim = AM_Root + "/Anim";
        public const string AM_Res = AM_Root + "/Res";
        // public const string AM_Bhv = AM_Root + "/Bhv";
        public const string AM_Movie = AM_Root + "/Movie";

        public const int AMOrder = -1000;
        public const int AMOrder_Anim = AMOrder + 30;
        // public const int AMOrder_Bhv = AMOrder + 35;
        public const int AMOrder_Event = AMOrder + 40;
        public const int AMOrder_Movie = AMOrder + 45;
        public const int AMOrder_Res = AMOrder + 100;
    }
}