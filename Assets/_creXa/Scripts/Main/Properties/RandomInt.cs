using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace creXa.GameBase
{
    [System.Serializable]
    public class RandomInt
    {
        public int _min, _max;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="min">Inclusive</param>
        /// <param name="max">Inclusive</param>
        public RandomInt(int min, int max)
        {
            _min = min;
            _max = max;
        }

        int GetValue()
        {
            return Random.Range(_min, _max + 1);
        }

        public static implicit operator int(RandomInt f)
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
