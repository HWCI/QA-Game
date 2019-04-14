using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace creXa.GameBase
{
    [Serializable]
    public class IntRangeAttribute : PropertyAttribute
    {
        public int MinLimit, MaxLimit;

        public IntRangeAttribute(int minLimit, int maxLimit)
        {
            MinLimit = minLimit;
            MaxLimit = maxLimit;
        }
    }

    [Serializable]
    public class IntRange
    {
        public int RangeStart, RangeEnd;

        public IntRange(int a, int b)
        {
            RangeStart = a;
            RangeEnd = b;
        }

        private int GetRandomValue()
        {
            return UnityEngine.Random.Range(RangeStart, RangeEnd + 1);
        }

        public static implicit operator int(IntRange d)
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