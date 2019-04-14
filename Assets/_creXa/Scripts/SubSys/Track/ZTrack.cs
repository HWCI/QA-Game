using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace creXa.GameBase
{
    [Serializable]
    public class ZTrack_Visual
    {
        public bool alwaysShow = true;
        public Color pathColor = Color.cyan;
        public Color inactivePathColor = Color.gray;
        public Color handleColor = Color.yellow;
        [Range(1, 3000)]
        public int resolution = 1000;

        public ZTrack_Visual()
        {
            alwaysShow = true;
            pathColor = Color.cyan;
            inactivePathColor = Color.gray;
            handleColor = Color.yellow;
        }
    }

    public abstract class ZTrack : MonoBehaviour
    {
        public ZTrack_Visual visual;

        public bool dirty { get; private set; }
        public void SetDirty()
        {
            dirty = true;
        }

        float _approxLength = -1;
        public float ApproxLength
        {
            get
            {
                if (dirty || _approxLength < 0)
                {
                    _approxLength = CalLength();
                    SetDirty();
                }
                return _approxLength;
            }
        }

        protected abstract float CalLength();

        public virtual Vector3 GetPointAt(float t)
        {
            return transform.position + GetLocalPointAt(t);
        }
        public abstract Vector3 GetLocalPointAt(float t);

    }
}
