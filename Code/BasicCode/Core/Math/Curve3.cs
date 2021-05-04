using UnityEngine;
using System.Collections;

namespace GameBasic
{
    public class Curve3
    {
        public AnimationCurve x;
        public AnimationCurve y;
        public AnimationCurve z;

        public Vector3 defaultValue;
        public float duration;
        public int frameCount;

        public void Set(Vector3[] points, float duration = 1)
        {
            if (x == null)
            {
                x = new AnimationCurve(new Keyframe[points.Length]);
                y = new AnimationCurve(new Keyframe[points.Length]);
                z = new AnimationCurve(new Keyframe[points.Length]);
            }

            this.duration = duration > 0 ? duration : 1;
            frameCount = points.Length;

            float time = duration / points.Length;

            Keyframe[] xk = x.keys;
            Keyframe[] yk = y.keys;
            Keyframe[] zk = z.keys;
            for (int i = 0; i < points.Length; i++)
            {
                float t = time * i;
                xk[i] = new Keyframe(t, points[i].x);
                yk[i] = new Keyframe(t, points[i].y);
                zk[i] = new Keyframe(t, points[i].z);
            }
            x.keys = xk;
            y.keys = yk;
            z.keys = zk;

            /*
            float[] xs = new float[points.Length];
            float[] ys = new float[points.Length];
            float[] zs = new float[points.Length];
            for (int i = 0; i < xs.Length; i++)
            {
                xs[i] = points[i].x;
                ys[i] = points[i].y;
                zs[i] = points[i].z;
            }
            Set(xs, ys, zs, duration);
            */
        }

        public void Set(float[] xs = null, float[] ys = null, float[] zs = null, float duration = 1)
        {
            this.duration = duration > 0 ? duration : 1;
            if (xs != null)
            {
                x = new AnimationCurve(CreateFrame(xs, duration));
                frameCount = xs.Length;
            }
            if (ys != null)
            {
                y = new AnimationCurve(CreateFrame(ys, duration));
                frameCount = xs.Length;
            }
            if (zs != null)
            {
                z = new AnimationCurve(CreateFrame(zs, duration));
                frameCount = xs.Length;
            }
        }

        public static Keyframe[] CreateFrame(float[] values, float duration = 1)
        {
            Keyframe[] curveFrame = new Keyframe[values.Length];
            float time = duration / curveFrame.Length;
            for (int i = 0; i < curveFrame.Length; i++)
            {
                curveFrame[i].value = values[i];
                curveFrame[i].time = time * i;
            }
            return curveFrame;
        }

        public Vector3 Evaluate(float time)
        {
            return new Vector3(
                x == null ? defaultValue.x : x.Evaluate(time),
                y == null ? defaultValue.y : y.Evaluate(time),
                z == null ? defaultValue.z : z.Evaluate(time));
        }

        public float EvaluateX(float time)
        {
            return x == null ? defaultValue.x : x.Evaluate(time);
        }

        public float EvaluateY(float time)
        {
            return y == null ? defaultValue.y : y.Evaluate(time);
        }

        public float EvaluateZ(float time)
        {
            return z == null ? defaultValue.z : z.Evaluate(time);
        }

        public Vector3 GetValue(float time)
        {

            time = time > duration ? duration : time;
            int index = (int)(frameCount * time / duration);
            index = index > frameCount - 1 ? frameCount - 1 : index;

            return GetValue(index);
        }

        public Vector3 GetValue(int index)
        {
            Vector3 result = defaultValue;
            if (x != null)
                result.x = x.keys[index].value;
            if (y != null)
                result.y = y.keys[index].value;
            if (z != null)
                result.z = z.keys[index].value;

            return result;
        }
    }
}