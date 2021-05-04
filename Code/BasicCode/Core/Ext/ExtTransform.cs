using UnityEngine;

namespace GameBasic
{
    public static class ExtTransform
    {
        public static Vector3 GetPostion(this Transform transform)
        {
            return transform.position;
        }

        public static Quaternion GetRotation(this Transform transform)
        {
            return transform.rotation;
        }

        public static Vector3 GetScale(this Transform transform)
        {
            return transform.localScale;
        }
    }
}