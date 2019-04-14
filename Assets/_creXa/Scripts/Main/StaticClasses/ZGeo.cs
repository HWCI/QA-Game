using UnityEngine;
using System.Collections;

namespace creXa.GameBase
{
    public static class ZGeo
    {
        public static Vector3 CalculateBezierPoint(float t, Vector3[] points)
        {
            int n = points.Length - 1;
            Vector3 rtn = Vector3.zero;
            for(int i=0; i<points.Length; i++)
                rtn += ZMath.nCr(n, i) * Mathf.Pow(t, i) * Mathf.Pow(1 - t, n - i) * points[i];

            return rtn;
        }

        public static Vector2 CartesianToLatLon(Vector3 point)
        {
            Vector2 rtn = Vector2.zero;
            rtn.y = Mathf.Atan2(point.x, point.z);

            rtn.x = Mathf.Atan2(-point.y, new Vector2(point.x, point.z).magnitude);
            rtn *= Mathf.Rad2Deg;

            return rtn;
        }

        public static Vector3 LatLonToCartesian(Vector2 latlon)
        {
            return LatLonToCartesian(latlon.x, latlon.y);
        }

        public static Vector3 LatLonToCartesian(float lat, float lon)
        {
            return Quaternion.Euler(lat, lon, 0) * new Vector3(0, 0, 1);
        }

    }
}
