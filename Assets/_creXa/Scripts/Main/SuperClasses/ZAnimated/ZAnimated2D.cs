using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace creXa.GameBase
{
    [AddComponentMenu("creXa/Animation/ZAnimated2D")]
    [RequireComponent(typeof(RectTransform))]
    public class ZAnimated2D : ZAnimated
    {
        public RectTransform rect;
        public CanvasGroup cvsgrp;

        protected override void AwakeRun()
        {
            if (!rect) rect = GetComponent<RectTransform>();
            if (!cvsgrp) cvsgrp = GetComponent<CanvasGroup>();
        }

		#region Basics

		public void Hide()
		{
			Graphic[] srs = GetComponentsInChildren<Graphic>(true);
			foreach (Graphic sr in srs)
				sr.color = ZColor.TRANSPARENT;
		}

		public void Show()
		{
			Graphic[] srs = GetComponentsInChildren<Graphic>(true);
			foreach (Graphic sr in srs)
				sr.color = Color.white;
		}

        public void Hide(float duration)
        {
            StartCoroutine(RGBColorTween(GetColor2D(), ZColor.TRANSPARENT, duration));
        }

        public void Show(float duration, Color? targetColor = null)
        {
            if (!targetColor.HasValue) targetColor = Color.white;
            StartCoroutine(RGBColorTween(ZColor.TRANSPARENT, targetColor.Value, duration));
        }

        #endregion

        #region Setters

        public void SetPic(Sprite sp)
		{
			Image sr = GetComponent<Image>();
			if (sr) sr.sprite = sp;
		}

		public void SetRectScale(Vector3 s)
		{
            if (!rect) AwakeRun();
            rect.localScale = s;
		}

		public void SetColor(Color c)
		{
			Graphic[] srs = GetComponentsInChildren<Graphic>(true);
			foreach (Graphic sr in srs)
				sr.color = c;
		}

		public void SetAlpha(float alpha)
		{
			Graphic[] srs = GetComponentsInChildren<Graphic>(true);
			foreach (Graphic sr in srs)
				sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
		}

		public void SetRectPos(Vector3 p)
		{
            if (!rect) AwakeRun();
            if (rect) rect.localPosition = p;
		}

		public void SetRectRot(Vector3 r)
		{
            if (!rect) AwakeRun();
            if (rect) rect.localEulerAngles = r;
		}

        public void SetCvsgrpAlpha(float f)
        {
            if (!cvsgrp) AwakeRun();
            if (cvsgrp) cvsgrp.alpha = f;
        }

		#endregion

		#region Getters

		public Vector3 GetRectScale()
		{
            if (!rect) AwakeRun();
            return rect ? rect.localScale : Vector3.zero;
		}

		public Vector3 GetRectPos()
		{
            if (!rect) AwakeRun();
            return rect ? rect.localPosition : Vector3.zero;
        }

		public Vector3 GetRectRot()
		{
            if (!rect) AwakeRun();
            return rect ? rect.localEulerAngles: Vector3.zero;
        }

        public float GetCvsgrpAlpha()
        {
            if (!cvsgrp) AwakeRun();
            return cvsgrp ? cvsgrp.alpha : 0.0f;
        }

        public Color GetColor2D()
        {
            Graphic sr = GetComponent<Graphic>();
            return sr ? sr.color : Color.clear;
        }

        #endregion

        #region Canvas Group

        public IEnumerator CvsgrpAlphaTween(float ori, float dest, float duration)
        {
            yield return StartCoroutine(ZTween.Float(ori, dest, duration, SetCvsgrpAlpha));
        }

        #endregion

        #region V3IE   

        public IEnumerator RectRotateTween(Vector3 ori, Vector3 dest, float duration)
        {
            yield return StartCoroutine(ZTween.V3(ori, dest, duration, SetRectRot));
        }    

        public IEnumerator RectSizeTween(Vector3 ori, Vector3 dest, float duration)
        {
            yield return StartCoroutine(V3Tween(ori, dest, duration, SetRectScale));
        }

        public IEnumerator RectPositionTween(Vector3 ori, Vector3 dest, float duration)
        {
            yield return StartCoroutine(V3Tween(ori, dest, duration, SetRectPos));
        }

        #endregion

        #region V3ACIE

        public IEnumerator RectPositionTween(AnimationCurve curvex, AnimationCurve curvey, float duration)
        {
            yield return StartCoroutine(TweenV3AC(curvex, curvey, null, duration, SetRectPos));
        }

        public IEnumerator RectRotateTween(AnimationCurve curvex, AnimationCurve curvey, float duration)
        {
            yield return StartCoroutine(TweenV3AC(curvex, curvey, null, duration, SetRectRot));
        }

        public IEnumerator RectSizeTween(AnimationCurve curvex, AnimationCurve curvey, float duration)
        {
            yield return StartCoroutine(TweenV3AC(curvex, curvey, null, duration, SetRectScale));
        }
		
        #endregion

		#region ColorTween

		public IEnumerator RGBColorTween(Color ori, Color dest, float duration)
        {
            yield return StartCoroutine(ColorTween(ori, dest, duration, SetColor));
        }

        public IEnumerator HSVColorTween(Vector4 ori, Vector4 dest, float duration)
        {
            yield return StartCoroutine(ColorTween(ZColor.HSVtoRGB(ori), ZColor.HSVtoRGB(dest), duration, SetColor));
        }

		#endregion

		#region V3LocusIE

		public IEnumerator RectLocus2DTween(Vector2 origin, Vector2 oripos, Vector2 destpos, float duration, int clockwise = 0, int times = 1)
		{
			float oridist = Vector2.Distance(oripos, origin);
			float oriangle = Mathf.Atan2(oripos.y - origin.y, oripos.x - origin.x) * Mathf.Rad2Deg;
			float destdist = Vector2.Distance(destpos, origin);
			float destangle = Mathf.Atan2(destpos.y - origin.y, destpos.x - origin.x) * Mathf.Rad2Deg;

			yield return StartCoroutine(RectLocus2DTween(origin, oridist, oriangle, destdist, destangle, duration, clockwise, times));

		}

		public IEnumerator RectLocus2DTween(Vector2 origin, float oridist, float oriangle, float destdist, float destangle, float duration, int clockwise = 0, int times = 1)
		{
			oriangle += oriangle < 0 ? 360 : 0;
			destangle += destangle < 0 ? 360 : 0;
			times = times < 1 ? 1 : times;

			bool clockwiseTF;
			if (clockwise == 0)
			{
				float diff = oriangle - destangle;
				diff += diff < 0 ? 360 : 0;
				clockwiseTF = diff < 180;
			}
			else
				clockwiseTF = clockwise > 0;		

			float timer = 0.0f;
			SetRectPos(RectLocus2DPosition(origin, oridist, oriangle));

			float anglediff = destangle - oriangle;

			anglediff += clockwiseTF ? (anglediff > 0 ? -360 : 0) : (anglediff < 0 ? 360 : 0);
			anglediff += (clockwiseTF ? -1 : 1) * 360 * (times - 1);

			while (timer < duration)
			{
                if (pause) yield return null;
                timer += Time.deltaTime;
				timer = Mathf.Min(timer, duration);

				SetRectPos(RectLocus2DPosition(origin, 
					oridist + (destdist - oridist) * timer / duration,
					oriangle + (anglediff) * timer / duration
					));

				yield return null;
			}

			SetRectPos(RectLocus2DPosition(origin, destdist, destangle));

		}

		Vector3 RectLocus2DPosition(Vector3 origin, float dist, float angleInD)
		{
			return new Vector3(origin.x + dist * Mathf.Cos(angleInD * Mathf.Deg2Rad), origin.y + dist * Mathf.Sin(angleInD * Mathf.Deg2Rad), origin.z);
		}

		#endregion

    }
}
