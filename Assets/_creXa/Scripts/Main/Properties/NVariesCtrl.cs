using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace creXa.GameBase
{
    [Serializable]
    public class NVariesCtrl
    {
        public NormalizedVaries Value;
        public AnimationCurve Varies;
        public bool Ctrl;
    }
}
