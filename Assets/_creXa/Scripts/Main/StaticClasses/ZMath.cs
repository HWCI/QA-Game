using UnityEngine;
using System.Collections;

namespace creXa.GameBase
{
    public static class ZMath
    {
        public static int Factorial(int n)
        {
            if (n < 0) return -1;
            if (n < 1) return 1;
            int rtn = n;
            for (int i = n - 1; i >= 1; i--)
                rtn *= i;
            return rtn;
        }

        public static int nPr(int n, int r)
        {
            if (n < 0 || r < 0 || r > n) return -1;
            int rtn = 1;
            for (int i = n; i > n - r; i--)
                rtn *= i;
            return rtn;
        }

        public static int nCr(int n, int r)
        {
            if (n < 0 || r < 0 || r > n) return -1;
            int rtn = 1;
            for (int i = n; i > n - r; i--)
                rtn *= i;
            for (int i = r; i > 1; i--)
                rtn /= i;
            return rtn;
        }

        public static float N(float x, float mean, float sigma)
        {
            return Z(x - mean / sigma);
        }

        public static float Z(float z)
        {
            return (1 / Mathf.Sqrt(2 * Mathf.PI)) * Mathf.Exp(-Mathf.Pow(z, 2) / 2);
        }

        public static float NIntegral(float x, float mean, float sigma, float interval = 20000)
        {
            return ZIntegral((x - mean) / sigma, interval);
        }

        public static float ZIntegral(float z, float interval = 20000)
        {
            float rtn = 0.0f;
            float a = 0.0f;
            float delta = Mathf.Abs(z) / interval;
            while (a < Mathf.Abs(z))
            {
                rtn += Z(a) * delta;
                a += delta;
            }
            return rtn;
        }

    }
}
