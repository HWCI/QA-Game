using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace creXa.GameBase
{
    [Serializable]
    public class NormalizedVaries
    {
        public float Min, Max;

        public float Evaluate(float t)
        {
            return Min + (Max - Min) * t;
        }

        public float GetNorm(float v)
        {
            if (Max == Min) return 0;
            return (v - Min) / (Max - Min);
        }

    }
}
