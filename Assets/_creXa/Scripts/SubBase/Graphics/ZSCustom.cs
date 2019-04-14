using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace creXa.GameBase.Graphics
{
    [AddComponentMenu("creXa/Shapes/ZSCustom")]
    public class ZSCustom : ZSCustomRegular
    {
        [SerializeField]
        List<Vector2> _customPoints = new List<Vector2>();
        public List<Vector2> customPoints
        {
            get { return _customPoints; }
            set
            {
                if (_customPoints == value) return;
                _customPoints = value;
                SetVerticesDirty();
                SetMaterialDirty();
            }
        }

        override public Vector2[] GetShapeVertices()
        {
            if (customPoints == null) return null;

            Vector2[] rtn = new Vector2[customPoints.Count];
            for (int i = 0; i < rtn.Length; i++)
                rtn[i] = customPoints[i];
            return rtn;
        }
    }
}
