using UnityEngine;
using System.Collections;

namespace creXa.GameBase.Graphics
{
    [AddComponentMenu("creXa/Shapes/ZSStar")]
    public class ZSStar : ZSCustomRegular
    {
        [SerializeField]
        float _innerRadius = 0.5f;
        public float innerRadius
        {
            get { return _innerRadius; }
            set
            {
                if (_innerRadius == value) return;
                _innerRadius = value;
                SetVerticesDirty();
                SetMaterialDirty();
            }
        }

        override public Vector2[] GetShapeVertices()
        {
            Vector2[] rtn = new Vector2[sides * 2];
            float deg = 360f / (sides * 2);
            for (int i = 0; i < rtn.Length; i++)
            {
                float rad = Mathf.Deg2Rad * (-90 + i * deg);
                float c = Mathf.Cos(rad) * (i % 2 == 0 ? 1 : innerRadius);
                float s = Mathf.Sin(rad) * (i % 2 == 0 ? 1 : innerRadius);
                rtn[i] = new Vector2(c, s);
            }
            return rtn;
        }
    }
}
