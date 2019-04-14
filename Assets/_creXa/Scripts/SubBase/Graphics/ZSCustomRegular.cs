using UnityEngine;
using System.Collections;

namespace creXa.GameBase.Graphics
{
    [AddComponentMenu("creXa/Shapes/ZSCustomRegular")]
    public class ZSCustomRegular : ZShape
    {
        [SerializeField]
		[Range(3, 100)] int _sides = 5;
        public int sides
        {
            get { return _sides; }
            set
            {
                if (_sides == value) return;
                _sides = value;
                SetVerticesDirty();
                SetMaterialDirty();
            }
        }

        override public Vector2[] GetShapeVertices()
        {
            Vector2[] rtn = new Vector2[sides];
            float deg = 360f / sides;
            for (int i = 0; i < rtn.Length; i++)
            {
                float rad = Mathf.Deg2Rad * (-90 + i * deg);
                float c = Mathf.Cos(rad);
                float s = Mathf.Sin(rad);
                rtn[i] = new Vector2(c, s);
            }
            return rtn;
        }

    }
}
