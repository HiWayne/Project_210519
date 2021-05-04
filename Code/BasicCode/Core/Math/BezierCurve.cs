using UnityEngine;
using System.Collections;

namespace GameBasic
{
    public class BezierCurve
    {

        public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
        {
            return a + (b - a) * t;
        }

        public static Vector2 Quadratic(Vector2 p0, Vector2 p1, Vector2 c, float t)
        {
            /*
            Vector2 p0 = Lerp(a, b, t);
            Vector2 p1 = Lerp(b, c, t);
            return Lerp(p0, p1, t);
            */

            float omt = 1 - t;

            return
                omt * omt * p0 +
                2f * omt * t * p1 +
                t * t * c;
        }

        public static Vector2 GetFirstDerive(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            return
                2f * (1f - t) * (p1 - p0) +
                2f * t * (p2 - p1);
        }

        public static Vector2 Cubic(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 d, float t)
        {
            /*
            Vector2 p0 = QuadraticCurve(a, b, c, t);
            Vector2 p1 = QuadraticCurve(b, c, d, t);
            return Lerp(p0, p1, t);
            */

            float t1 = 1 - t;
            float t2 = t1 * t1;
            float t3 = t1 * t1 * t1;

            return
                t3 * p0 + 
                (3f * t2 * t) * p1 + 
                (3f * t1 * t * t) * p2 + 
                (t * t * t) * d;
        }

        public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            t = Mathf.Clamp01(t);
            float oneMinusT = 1f - t;
            return
                3f * oneMinusT * oneMinusT * (p1 - p0) +
                6f * oneMinusT * t * (p2 - p1) +
                3f * t * t * (p3 - p2);
        }
    }
}