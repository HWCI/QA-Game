using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace creXa.GameBase
{
    public static class ZTween
    {
        public static IEnumerator Float(float from, float to, float duration, Action<float> SetVal)
        {
            SetVal(from);
            float timer = 0.0f;
            while(timer < duration)
            {
                timer += Time.deltaTime;
                timer = Mathf.Min(timer, duration);
                SetVal(from + (to - from) * Mathf.Clamp01(timer / duration));
                yield return null;
            }
        }

        public static IEnumerator Int(int from, int to, float duration, Action<int> SetVal)
        {
            SetVal(from);
            float timer = 0.0f;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                timer = Mathf.Min(timer, duration);
                SetVal(Mathf.RoundToInt(from + (to - from) * Mathf.Clamp01(timer / duration)));
                yield return null;
            }
        }

        public static IEnumerator Int(int from, int to, float duration, Action<float> SetVal)
        {
            SetVal(from);
            float timer = 0.0f;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                timer = Mathf.Min(timer, duration);
                SetVal(from + (to - from) * Mathf.Clamp01(timer / duration));
                yield return null;
            }
        }

        public static IEnumerator Color(Color from, Color to, float duration, Action<Color> SetVal)
        {
            SetVal(from);
            float timer = 0.0f;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                timer = Mathf.Min(timer, duration);
                SetVal(from + (to - from) * Mathf.Clamp01(timer / duration));
                yield return null;
            }
        }

        public static IEnumerator V2(Vector2 from, Vector2 to, float duration, Action<Vector2> SetVal)
        {
            SetVal(from);
            float timer = 0.0f;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                timer = Mathf.Min(timer, duration);
                SetVal(from + (to - from) * Mathf.Clamp01(timer / duration));
                yield return null;
            }
        }

        public static IEnumerator V3(Vector3 from, Vector3 to, float duration, Action<Vector3> SetVal)
        {
            SetVal(from);
            float timer = 0.0f;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                timer = Mathf.Min(timer, duration);
                SetVal(from + (to - from) * Mathf.Clamp01(timer / duration));
                yield return null;
            }
        }

        public static IEnumerator V4(Vector4 from, Vector4 to, float duration, Action<Vector4> SetVal)
        {
            SetVal(from);
            float timer = 0.0f;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                timer = Mathf.Min(timer, duration);
                SetVal(from + (to - from) * Mathf.Clamp01(timer / duration));
                yield return null;
            }
        }

        public static IEnumerator V2AC(AnimationCurve curvex, AnimationCurve curvey, float duration, Action<Vector2> SetVal)
        {
            float timer = 0.0f;
            if (curvex == null) curvex = new AnimationCurve();
            if (curvey == null) curvey = new AnimationCurve();

            SetVal(new Vector2(curvex.Evaluate(0), curvey.Evaluate(0)));

            while (timer < duration)
            {
                timer += Time.deltaTime;
                timer = Mathf.Min(timer, duration);
                SetVal(new Vector2(curvex.Evaluate(Mathf.Clamp01(timer / duration)), curvey.Evaluate(Mathf.Clamp01(timer / duration))));
                yield return null;
            }
            SetVal(new Vector2(curvex.Evaluate(1), curvey.Evaluate(1)));
        }

        public static IEnumerator V3AC(AnimationCurve curvex, AnimationCurve curvey, AnimationCurve curvez, float duration, Action<Vector3> SetVal)
        {
            float timer = 0.0f;
            if (curvex == null) curvex = new AnimationCurve();
            if (curvey == null) curvey = new AnimationCurve();
            if (curvez == null) curvez = new AnimationCurve();
            SetVal(new Vector3(curvex.Evaluate(0) , curvey.Evaluate(0), curvez.Evaluate(0)));

            while (timer < duration)
            {
                timer += Time.deltaTime;
                timer = Mathf.Min(timer, duration);
                SetVal(new Vector3(curvex.Evaluate(Mathf.Clamp01(timer / duration)), curvey.Evaluate(Mathf.Clamp01(timer / duration)), curvez.Evaluate(Mathf.Clamp01(timer / duration))));
                yield return null;
            }
            SetVal(new Vector3(curvex.Evaluate(1), curvey.Evaluate(1), curvez.Evaluate(1)));
        }

        public static IEnumerator V4AC(AnimationCurve curvex, AnimationCurve curvey, AnimationCurve curvez, AnimationCurve curvew, float duration, Action<Vector4> SetVal)
        {
            float timer = 0.0f;
            if (curvex == null) curvex = new AnimationCurve();
            if (curvey == null) curvey = new AnimationCurve();
            if (curvez == null) curvez = new AnimationCurve();
            if (curvew == null) curvew = new AnimationCurve();
            SetVal(new Vector4(curvex.Evaluate(0), curvey.Evaluate(0), curvez.Evaluate(0), curvew.Evaluate(0)));

            while (timer < duration)
            {
                timer += Time.deltaTime;
                timer = Mathf.Min(timer, duration);
                SetVal(new Vector4(curvex.Evaluate(Mathf.Clamp01(timer / duration)), curvey.Evaluate(Mathf.Clamp01(timer / duration)), curvez.Evaluate(Mathf.Clamp01(timer / duration)), curvew.Evaluate(Mathf.Clamp01(timer / duration))));
                yield return null;
            }
            SetVal(new Vector4(curvex.Evaluate(1), curvey.Evaluate(1), curvez.Evaluate(1), curvew.Evaluate(1)));
        }

    }
}
