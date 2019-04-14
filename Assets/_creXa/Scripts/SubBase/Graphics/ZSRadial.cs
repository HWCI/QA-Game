using UnityEngine;
using System.Collections;

namespace creXa.GameBase.Graphics
{
    [AddComponentMenu("creXa/Shapes/ZSRadial")]
    public class ZSRadial : ZSCustomRegular
    {
        [SerializeField]
        [Range(0, 1)] float[] _values;
        public float[] values
        {
            get { return _values; }
            set
            {
                if (_values == value) return;
                _values = value;
                sides = _values.Length;
                SetVerticesDirty();
                SetMaterialDirty();
            }
        }

        override public Vector2[] GetShapeVertices()
        {
            if (values == null || values.Length != sides) return null;

            Vector2[] rtn = new Vector2[sides];
            float deg = 360f / sides;
            for (int i = 0; i < rtn.Length; i++)
            {
                float rad = Mathf.Deg2Rad * (-90 + i * deg);
                float c = Mathf.Cos(rad) * values[i];
                float s = Mathf.Sin(rad) * values[i];
                rtn[i] = new Vector2(c, s);
            }
            return rtn;
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            _values = new float[sides];
            for (int i = 0; i < _values.Length; i++)
                _values[i] = Random.Range(0f, 1f);
        }
#endif
    }
}
