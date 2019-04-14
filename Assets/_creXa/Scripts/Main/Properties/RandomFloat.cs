using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace creXa.GameBase
{
    [System.Serializable]
    public class RandomFloat
    {
        public float _min, _max;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="min">Inclusive</param>
        /// <param name="max">Inclusive</param>
        public RandomFloat(float min, float max)
        {
            _min = min;
            _max = max;
        }

        float GetValue()
        {
            return Random.Range(_min, _max);
        }

        public static implicit operator float(RandomFloat f)
        {
            return f.GetValue();
        }

        public override string ToString()
        {
            return GetValue().ToString();
        }

        public string ToString(string format)
        {
            return GetValue().ToString(format);
        }

    }
}
