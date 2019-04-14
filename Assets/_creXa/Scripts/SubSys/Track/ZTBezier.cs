using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace creXa.GameBase
{
    [Serializable]
    public class ZTBezierPoint
    {
        public Vector3 position;
        public Vector3 handlePrev;
        public Vector3 handleNext;

        public bool handleStyleSmooth;
        public bool leftTangentFree;
        public bool rightTangentFree;

        public ZTBezierPoint(Vector3 pos)
        {
            position = pos;
            handlePrev = Vector3.back * 5;
            handleNext = Vector3.forward * 5;

            handleStyleSmooth = true;
            leftTangentFree = true;
            rightTangentFree = true;
        }

        public void SetAxisTo(string axis, float v)
        {
            switch (axis)
            {
                case "X": case "x":
                    position.x = handlePrev.x = handleNext.x = v;
                    break;
                case "Y": case "y":
                    position.y = handlePrev.y = handleNext.y = v;
                    break;
                case "Z": case "z":
                    position.z = handlePrev.z = handleNext.z = v;
                    break;
                default:
                    break;
            }
        }

        public void Smoothing()
        {
            handleStyleSmooth = true;
            handleNext = handlePrev * -1;
        }

    }

    [AddComponentMenu("creXa/Track/ZBezier")]
    public class ZTBezier : ZTrack
    {
        public int resolution = 30;

        [SerializeField] bool _closeLoop = true;
        public bool closeLoop
        {
            get { return _closeLoop; }
            set
            {
                if (_closeLoop == value) return;
                _closeLoop = value;
                SetDirty();
            }
        }

        public int GetPointIndex(ZTBezierPoint point)
        {
            for (int i = 0; i < points.Count; i++)
                if (points[i] == point)
                    return i;
            return -1;
        }

        protected override float CalLength()
        {
            float rtn = 0;
            for (int i = 0; i < points.Count - 1; i++)
            {
                rtn += ApproxSegmentLength(points[i], points[i + 1], resolution);
            }

            if (closeLoop) rtn += ApproxSegmentLength(points[points.Count - 1], points[0], resolution);

            return rtn;
        }

        public List<ZTBezierPoint> points = new List<ZTBezierPoint>();
        public ZTBezierPoint this[int index]
        {
            get { return points[index]; }
        }

        public void AddPoint(ZTBezierPoint point)
        {
            points.Add(point);
            SetDirty();
        }

        public void AddPointAt(Vector3 pos)
        {
            AddLocalPointAt(pos - transform.position);
        }

        public void AddLocalPointAt(Vector3 pos)
        {
            AddPoint (new ZTBezierPoint(pos));
        }

        public void RemovePoint(ZTBezierPoint point)
        {
            points.Remove(point);
            SetDirty();
        }

        public ZTBezierPoint[] GetAnchorPoints()
        {
            return points.ToArray();
        }

        public override Vector3 GetLocalPointAt(float t)
        {
            if (points.Count <= 0) return Vector3.zero;
            if (t <= 0) return points[0].position;
            else if (t >= 1) return closeLoop? points[0].position : points[points.Count - 1].position;

            float culmulatedPercent = 0;
            float curvePercent = 0;

            ZTBezierPoint p1 = null, p2 = null;

            for(int i=0; i<points.Count - 1; i++)
            {
                curvePercent = ApproxSegmentLength(points[i], points[i + 1], resolution) / ApproxLength;
                if (culmulatedPercent + curvePercent > t)
                {
                    p1 = points[i];
                    p2 = points[i + 1];
                    break;
                }
                else culmulatedPercent += curvePercent;
            }

            if(closeLoop && p1 == null)
            {
                p1 = points[points.Count - 1];
                p2 = points[0];
            }

            t -= culmulatedPercent;
            return GetPointInSegement(p1, p2, t / curvePercent);

        }

        bool GetSegmentPoint(int index, out ZTBezierPoint p1, out ZTBezierPoint p2)
        {
            if ((index < 0 || index > points.Count) || (!closeLoop && index == points.Count))
            {
                p1 = null;
                p2 = null;
                return false;
            }

            if (index == points.Count)
            {
                p1 = points[points.Count - 1];
                p2 = points[0];
            }
            else
            {
                p1 = points[index];
                p2 = points[index + 1];
            }
            return true;
        }

        public float ApproxSegmentLength(int index, int resolution = 10)
        {
            ZTBezierPoint p1 = null, p2 = null;
            if(GetSegmentPoint(index, out p1, out p2))
                return ApproxSegmentLength(p1, p2, resolution);
            return 0.0f;
        }

        public static float ApproxSegmentLength(ZTBezierPoint p1, ZTBezierPoint p2, int resolution = 10)
        {
            float rtn = 0;
            Vector3 currentPos = p1.position;
            Vector3 nextPos;

            for(int i=0; i<resolution + 1; i++)
            {
                nextPos = GetPointInSegement(p1, p2, i / resolution);
                rtn += (nextPos - currentPos).magnitude;
                currentPos = nextPos;
            }

            return rtn;
        }

        public Vector3 GetPointInSegment(int index, float t)
        {
            ZTBezierPoint p1 = null, p2 = null;
            if(GetSegmentPoint(index, out p1, out p2))
                return GetPointInSegement(p1, p2, t);
            return Vector3.zero;
        }

        public static Vector3 GetPointInSegement(ZTBezierPoint p1, ZTBezierPoint p2, float t)
        {
            t = Mathf.Clamp01(t);
            if(!p1.rightTangentFree && !p2.leftTangentFree)
                return GetPointInSegmentLinear(p1.position, p2.position, t);
            else
                return GetPointInSegmentBezier(p1, p2, t);
        }

        public static Vector3 GetPointInSegmentBezier(ZTBezierPoint p1, ZTBezierPoint p2, float t)
        {
            Vector3[] tmp = new Vector3[4];
            tmp[0] = p1.position;
            tmp[1] = p1.position + p1.handleNext;
            tmp[2] = p2.position + p2.handlePrev;
            tmp[3] = p2.position;

            return ZGeo.CalculateBezierPoint(t, tmp);
        }

        public static Vector3 GetPointInSegmentLinear(Vector3 p1, Vector3 p2, float t)
        {
            return p1 + ((p2 - p1) * t);
        }

        public void SetLinearTangent(int i, bool left)
        {
            int modIndex = -1;
            if (left)
            {
                modIndex = i - 1;
                if (modIndex < 0 && !closeLoop) return;
                if (modIndex < 0 && closeLoop) modIndex = points.Count - 1;
                points[i].handlePrev = Vector3.Normalize(points[modIndex].position - points[i].position);
                points[i].leftTangentFree = false;
                if (points[i].handleStyleSmooth)
                    points[i].handleNext = points[i].handlePrev * -1;
                
            }
            else
            {
                modIndex = i + 1;
                if (modIndex >= points.Count && !closeLoop) return;
                if (modIndex >= points.Count && closeLoop) modIndex = 0; ;
                points[i].handleNext = Vector3.Normalize(points[modIndex].position - points[i].position);
                points[i].rightTangentFree = false;
                if (points[i].handleStyleSmooth)
                    points[i].handlePrev = points[i].handleNext * -1;
            }
            SetDirty();

        }

#if UNITY_EDITOR

        public void OnDrawGizmos()
        {
            if (UnityEditor.Selection.activeGameObject == gameObject || visual.alwaysShow)
            {
                if(points.Count > 1)
                {
                    for(int i=0; i<points.Count; i++)
                    {
                        if (i < points.Count - 1)
                        {
                            ZTBezierPoint p1 = points[i];
                            ZTBezierPoint p2 = points[i + 1];
                            UnityEditor.Handles.DrawBezier(transform.position + p1.position,
                                transform.position + p2.position,
                                transform.position + p1.position + p1.handleNext,
                                transform.position + p2.position + p2.handlePrev,
                                ((UnityEditor.Selection.activeGameObject == gameObject) ? visual.pathColor : visual.inactivePathColor),
                                null, 5);
                        }
                        else if (closeLoop)
                        {
                            ZTBezierPoint p1 = points[i];
                            ZTBezierPoint p2 = points[0];
                            UnityEditor.Handles.DrawBezier(transform.position + p1.position,
                                transform.position + p2.position,
                                transform.position + p1.position + p1.handleNext,
                                transform.position + p2.position + p2.handlePrev,
                                ((UnityEditor.Selection.activeGameObject == gameObject) ? visual.pathColor : visual.inactivePathColor),
                                null, 5);
                        }
                    }
                }
            }
        }

#endif
    }
}

