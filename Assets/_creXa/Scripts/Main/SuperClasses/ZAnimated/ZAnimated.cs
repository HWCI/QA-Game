using UnityEngine;
using System.Collections;
using System;

namespace creXa.GameBase
{
    [AddComponentMenu("creXa/Animation/ZAnimated")]
    public class ZAnimated : ZStateCtrl
    {

        public bool pause = false;
        public float timeCycle = 1.0f;
        public float timeScale = 1.0f;

        sealed protected override void Awake()
        {
            AwakeRun();
        }

        protected virtual void AwakeRun() { }

        #region Basics

        public void Disappear()
        {
            if (gameObject.activeSelf) gameObject.SetActive(false);
        }

        public void Appear()
        {
			if (!gameObject.activeSelf) gameObject.SetActive(true);
        }

        protected IEnumerator FloatTween(float from, float to, float duration, Action<float> SetVal)
        {
            SetVal(from);
            float timer = 0.0f;
            while (timer < duration)
            {
                if (pause) yield return null;
                timer += Time.deltaTime * timeScale;
                timer = Mathf.Min(timer, duration);
                SetVal(from + (to - from) * Mathf.Clamp01(timer / duration));
                yield return null;
            }
        }

        protected IEnumerator IntTween(int from, int to, float duration, Action<int> SetVal)
        {
            SetVal(from);
            float timer = 0.0f;
            while (timer < duration)
            {
                if (pause) yield return null;
                timer += Time.deltaTime * timeScale;
                timer = Mathf.Min(timer, duration);
                SetVal(Mathf.RoundToInt(from + (to - from) * Mathf.Clamp01(timer / duration)));
                yield return null;
            }
        }

        protected IEnumerator IntTween(int from, int to, float duration, Action<float> SetVal)
        {
            SetVal(from);
            float timer = 0.0f;
            while (timer < duration)
            {
                if (pause) yield return null;
                timer += Time.deltaTime * timeScale;
                timer = Mathf.Min(timer, duration);
                SetVal(from + (to - from) * Mathf.Clamp01(timer / duration));
                yield return null;
            }
        }

        protected IEnumerator ColorTween(Color from, Color to, float duration, Action<Color> SetVal)
        {
            SetVal(from);
            float timer = 0.0f;
            while (timer < duration)
            {
                if (pause) yield return null;
                timer += Time.deltaTime * timeScale;
                timer = Mathf.Min(timer, duration);
                SetVal(from + (to - from) * Mathf.Clamp01(timer / duration));
                yield return null;
            }
        }

        protected IEnumerator V2Tween(Vector2 from, Vector2 to, float duration, Action<Vector2> SetVal)
        {
            SetVal(from);
            float timer = 0.0f;
            while (timer < duration)
            {
                if (pause) yield return null;
                timer += Time.deltaTime * timeScale;
                timer = Mathf.Min(timer, duration);
                SetVal(from + (to - from) * Mathf.Clamp01(timer / duration));
                yield return null;
            }
        }

        protected IEnumerator V3Tween(Vector3 from, Vector3 to, float duration, Action<Vector3> SetVal)
        {
            SetVal(from);
            float timer = 0.0f;
            while (timer < duration)
            {
                if (pause) yield return null;
                timer += Time.deltaTime * timeScale;
                timer = Mathf.Min(timer, duration);
                SetVal(from + (to - from) * Mathf.Clamp01(timer / duration));
                yield return null;
            }
        }

        protected IEnumerator V4Tween(Vector4 from, Vector4 to, float duration, Action<Vector4> SetVal)
        {
            SetVal(from);
            float timer = 0.0f;
            while (timer < duration)
            {
                if (pause) yield return null;
                timer += Time.deltaTime * timeScale;
                timer = Mathf.Min(timer, duration);
                SetVal(from + (to - from) * Mathf.Clamp01(timer / duration));
                yield return null;
            }
        }

        protected IEnumerator V2ACTween(AnimationCurve curvex, AnimationCurve curvey, float duration, Action<Vector2> SetVal)
        {
            float timer = 0.0f;
            if (curvex == null) curvex = new AnimationCurve();
            if (curvey == null) curvey = new AnimationCurve();

            SetVal(new Vector2(curvex.Evaluate(0), curvey.Evaluate(0)));

            while (timer < duration)
            {
                if (pause) yield return null;
                timer += Time.deltaTime * timeScale;
                timer = Mathf.Min(timer, duration);
                SetVal(new Vector2(curvex.Evaluate(Mathf.Clamp01(timer / duration)), curvey.Evaluate(Mathf.Clamp01(timer / duration))));
                yield return null;
            }
            SetVal(new Vector2(curvex.Evaluate(1), curvey.Evaluate(1)));
        }

        protected IEnumerator V3ACTween(AnimationCurve curvex, AnimationCurve curvey, AnimationCurve curvez, float duration, Action<Vector3> SetVal)
        {
            float timer = 0.0f;
            if (curvex == null) curvex = new AnimationCurve();
            if (curvey == null) curvey = new AnimationCurve();
            if (curvez == null) curvez = new AnimationCurve();
            SetVal(new Vector3(curvex.Evaluate(0), curvey.Evaluate(0), curvez.Evaluate(0)));

            while (timer < duration)
            {
                if (pause) yield return null;
                timer += Time.deltaTime * timeScale;
                timer = Mathf.Min(timer, duration);
                SetVal(new Vector3(curvex.Evaluate(Mathf.Clamp01(timer / duration)), curvey.Evaluate(Mathf.Clamp01(timer / duration)), curvez.Evaluate(Mathf.Clamp01(timer / duration))));
                yield return null;
            }
            SetVal(new Vector3(curvex.Evaluate(1), curvey.Evaluate(1), curvez.Evaluate(1)));
        }

        protected IEnumerator V4ACTween(AnimationCurve curvex, AnimationCurve curvey, AnimationCurve curvez, AnimationCurve curvew, float duration, Action<Vector4> SetVal)
        {
            float timer = 0.0f;
            if (curvex == null) curvex = new AnimationCurve();
            if (curvey == null) curvey = new AnimationCurve();
            if (curvez == null) curvez = new AnimationCurve();
            if (curvew == null) curvew = new AnimationCurve();
            SetVal(new Vector4(curvex.Evaluate(0), curvey.Evaluate(0), curvez.Evaluate(0), curvew.Evaluate(0)));

            while (timer < duration)
            {
                if (pause) yield return null;
                timer += Time.deltaTime * timeScale;
                timer = Mathf.Min(timer, duration);
                SetVal(new Vector4(curvex.Evaluate(Mathf.Clamp01(timer / duration)), curvey.Evaluate(Mathf.Clamp01(timer / duration)), curvez.Evaluate(Mathf.Clamp01(timer / duration)), curvew.Evaluate(Mathf.Clamp01(timer / duration))));
                yield return null;
            }
            SetVal(new Vector4(curvex.Evaluate(1), curvey.Evaluate(1), curvez.Evaluate(1), curvew.Evaluate(1)));
        }

        #endregion

        #region Setters

        public void SetScale(Vector3 s)
        {
            transform.localScale = s;
        }

        public void SetPos(Vector3 p)
        {
            transform.localPosition = p;
        }

        public void SetWorldPos(Vector3 p)
        {
            transform.position = p;
        }

        public void SetRot(Vector3 r)
        {
            transform.localEulerAngles = r;
        }

        public void SetParent(Transform t)
        {
            transform.SetParent(t);
        }

        #endregion

        #region Getters

        public Vector3 GetScale()
        {
            return transform.localScale;
        }

        public Vector3 GetPos()
        {
            return transform.localPosition;
        }

        public Vector3 GetWorldPos()
        {
            return transform.position;
        }

        public Vector3 GetRot()
        {
            return transform.localEulerAngles;
        }

        public Vector3 GetWorldRot()
        {
            return transform.eulerAngles;
        }

        #endregion

        public Vector3 WorldPointToRelative(Vector3 pos)
        {
            return (pos - transform.position) / transform.lossyScale.x;
        }

        #region V3IE

        public IEnumerator WorldPositionTween(Vector3 ori, Vector3 dest, float duration)
        {
            yield return StartCoroutine(V3Tween(ori, dest, duration, SetWorldPos));
        }

        public IEnumerator RotateTween(Vector3 ori, Vector3 dest, float duration)
        {
            yield return StartCoroutine(V3Tween(ori, dest, duration, SetRot));
        }

        public IEnumerator SizeTween(Vector3 ori, Vector3 dest, float duration)
        {
            yield return StartCoroutine(V3Tween(ori, dest, duration, SetScale));
        }

        public IEnumerator PositionTween(Vector3 ori, Vector3 dest, float duration)
        {
            yield return StartCoroutine(V3Tween(ori, dest, duration, SetPos));
        }

        #endregion

        protected IEnumerator TweenV3AC(AnimationCurve curvex, AnimationCurve curvey, AnimationCurve curvez, float duration, Action<Vector3> SetVal)
        {
            float timer = 0.0f;
            if (curvex == null) curvex = new AnimationCurve();
            if (curvey == null) curvey = new AnimationCurve();
            if (curvez == null) curvez = new AnimationCurve();
            SetVal(new Vector3(curvex.Evaluate(0), curvey.Evaluate(0), curvez.Evaluate(0)));

            while (timer < duration)
            {
                if (pause) yield return null;
                timer += Time.deltaTime * timeScale;
                timer = Mathf.Min(timer, duration);
                SetVal(new Vector3(curvex.Evaluate(Mathf.Clamp01(timer / duration)), curvey.Evaluate(Mathf.Clamp01(timer / duration)), curvez.Evaluate(Mathf.Clamp01(timer / duration))));
                yield return null;
            }
            SetVal(new Vector3(curvex.Evaluate(1), curvey.Evaluate(1), curvez.Evaluate(1)));
        }

        #region V3ACIE

        public IEnumerator WorldPositionTween(AnimationCurve curvex, AnimationCurve curvey, AnimationCurve curvez, float duration)
        {
            yield return StartCoroutine(TweenV3AC(curvex, curvey, curvez, duration, SetWorldPos));
        }

        public IEnumerator PositionTween(AnimationCurve curvex, AnimationCurve curvey, AnimationCurve curvez, float duration)
        {
            yield return StartCoroutine(TweenV3AC(curvex, curvey, curvez, duration, SetPos));
        }

        public IEnumerator RotateTween(AnimationCurve curvex, AnimationCurve curvey, AnimationCurve curvez, float duration)
        {
            yield return StartCoroutine(TweenV3AC(curvex, curvey, curvez, duration, SetRot));
        }

        public IEnumerator SizeTween(AnimationCurve curvex, AnimationCurve curvey, AnimationCurve curvez, float duration)
        {
            yield return StartCoroutine(TweenV3AC(curvex, curvey, curvez, duration, SetScale));
        }

        #endregion
		

	}
}
