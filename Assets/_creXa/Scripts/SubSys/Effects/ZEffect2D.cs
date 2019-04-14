using UnityEngine;
using System.Collections;

namespace creXa.GameBase
{
    [System.Serializable]
    public class ZEffect2D
    {
        public float triggerTime;

        public ZEffectType2D type;
        public int refPointsIdx;

        public float lifeTime;
        public GameObject targetObject;
    }
}
