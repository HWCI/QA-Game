using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace creXa.GameBase
{
    [Serializable]
    public class ZTSpiral : ZTrack
    {
        public bool regular = true;
        [Range(0, 50)]
        public float expRate = 1.0f;
        public Vector3 startBound = Vector3.zero;
        public Vector3 endBound = Vector3.one * 3;

        [Angle]
        public float phase = 0;
        public float frequency = 50;

        public override Vector3 GetLocalPointAt(float t)
        {
            Vector3 rtn = Vector3.zero;
            float radiusx, radiusz, rad;
            radiusx = startBound.x + (endBound.x - startBound.x) * Mathf.Pow(t, expRate);
            radiusz = regular ? radiusx : startBound.z + (endBound.z - startBound.z) * t;
            rad = phase * Mathf.Deg2Rad + t * Mathf.PI * frequency;
            rtn.x = radiusx * Mathf.Sin(rad);
            rtn.z = radiusz * Mathf.Cos(rad);
            rtn.y = startBound.y + (endBound.y - startBound.y) * t;
            return rtn;
        }

        protected override float CalLength()
        {
            float rtn = 0.0f;
            for (int i = 0; i < visual.resolution; i++)
            {
                rtn += Mathf.Abs((GetPointAt((float)(i + 1) / visual.resolution) - GetPointAt((float)i / visual.resolution)).magnitude);
            }
            return rtn;
        }

#if UNITY_EDITOR
        public void OnDrawGizmos()
        {
            if (UnityEditor.Selection.activeGameObject == gameObject || visual.alwaysShow)
            {
                Vector3[] points = new Vector3[visual.resolution * 2];
                points[0] = GetPointAt(0);
                for (int i = 1; i < visual.resolution; i++)
                {
                    points[i * 2 - 1] = points[i * 2] = GetPointAt((float)i / visual.resolution);
                }
                points[visual.resolution * 2 - 1] = GetPointAt(1);
                Color ori = UnityEditor.Handles.color;
                UnityEditor.Handles.color = (UnityEditor.Selection.activeGameObject == gameObject) ? visual.pathColor : visual.inactivePathColor;
                UnityEditor.Handles.DrawLines(points);
                UnityEditor.Handles.color = ori;
            }
        }
#endif
    }
}

