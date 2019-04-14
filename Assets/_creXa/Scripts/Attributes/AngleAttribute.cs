using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace creXa.GameBase
{
    public class AngleAttribute : PropertyAttribute
    {
        public readonly float snap, min, max;

        public AngleAttribute()
        {
            snap = 5;
            min = 0;
            max = 360;
        }

        public AngleAttribute(float snap)
        {
            this.snap = snap;
            min = 0;
            max = 360;
        }

        public AngleAttribute(float snap, float min, float max)
        {
            this.snap = snap;
            this.min = min;
            this.max = max;
        }
    }
}


