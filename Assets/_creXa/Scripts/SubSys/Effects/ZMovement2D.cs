using UnityEngine;
using System.Collections;

namespace creXa.GameBase
{
    [System.Serializable]
    public class ZMovement2D
    {
        public float triggerTime;

        public ZMovementType2D type;
        public float lifeTime;
        public int[] refPointsIdx;
        public Vector3[] refPointsOffsets;
    }
}
