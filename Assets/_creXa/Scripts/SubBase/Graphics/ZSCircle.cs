using UnityEngine;
using System.Collections;

namespace creXa.GameBase.Graphics
{
    [AddComponentMenu("creXa/Shapes/ZSCircle")]
    public class ZSCircle : ZShape
    {
        int sides = 100;

        [SerializeField]
        bool _reverse = false;
        public bool reverse
        {
            get { return _reverse; }
            set
            {
                if (_reverse == value) return;
                _reverse = value;
                SetVerticesDirty();
                SetMaterialDirty();
            }
        }

        [SerializeField]
        [Range(0, 1)] float _fill = 1.0f;
        public float fill
        {
            get { return _fill; }
            set
            {
                if (_fill == value) return;
                _fill = value;
                SetVerticesDirty();
                SetMaterialDirty();
            }
        }

        [SerializeField]
        [Angle(1, 0, 360)] float _phase = 0.0f;
        public float phase
        {
            get { return _phase; }
            set
            {
                if (_phase == value) return;
                _phase = value;
                SetVerticesDirty();
                SetMaterialDirty();
            }
        }

        override public Vector2[] GetShapeVertices()
        {
            Vector2[] rtn = new Vector2[Mathf.RoundToInt(sides * fill) + (fill < 1 ? 1 : 0)];

            float deg = 360f / sides;
            for (int i = 0; i < rtn.Length; i++)
            {
                if (fill < 1 && i == rtn.Length - 1)
                {
                    rtn[i] = Vector2.zero;
                }
                else
                {
                    float rad = Mathf.Deg2Rad * (-90 - phase - (i * deg * (_reverse? -1 : 1)));
                    float c = Mathf.Cos(rad);
                    float s = Mathf.Sin(rad);
                    rtn[i] = new Vector2(c, s);
                }
            }

            return rtn;
        }

    }
}
