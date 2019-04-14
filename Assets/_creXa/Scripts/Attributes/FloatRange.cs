using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace creXa.GameBase
{
    public class FloatRangeAttribute : PropertyAttribute
    {
        public float MinLimit, MaxLimit;

        public FloatRangeAttribute(float minLimit, float maxLimit)
        {
            MinLimit = minLimit;
            MaxLimit = maxLimit;
        }
    }

    [System.Serializable]
    public class FloatRange
    {
        public float RangeStart = 0, RangeEnd = 0;

        public FloatRange(float a, float b)
        {
            RangeStart = a;
            RangeEnd = b;
        }

        private float GetRandomValue()
        {
            return Random.Range(RangeStart, RangeEnd);
        }

        public static implicit operator float(FloatRange d)
        {
            return d.GetRandomValue();
        }

        public override string ToString()
        {
            return GetRandomValue().ToString();
        }

        public string ToString(string format = "")
        {
            return GetRandomValue().ToString(format);
        }
    }
}
