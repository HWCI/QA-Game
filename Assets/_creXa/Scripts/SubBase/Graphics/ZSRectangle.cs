using UnityEngine;
using System.Collections.Generic;

namespace creXa.GameBase.Graphics
{
    [AddComponentMenu("creXa/Shapes/ZSRectangle")]
    public class ZSRectangle : ZQuad
    {
        protected override void Awake()
        {
            base.Awake();
            if(_roundCorner == null || _roundCorner.Length != 4)
            {
                Debug.LogWarning("Round Corner Not Set Properly.");
                _roundCorner = new bool[] { true, true, true, true };
            }
                
            if (_roundRadius == null || _roundRadius.Length != 4)
            {
                Debug.LogWarning("Round Radius Not Set Properly.");
                _roundRadius = new int[] { 1, 1, 1, 1 };
            }
                
        }

        [SerializeField]
        bool[] _roundCorner = new bool[] { true, true, true, true };
        public bool[] roundCorner
        {
            get { return _roundCorner; }
            set
            {
                if (_roundCorner == value) return;
                _roundCorner = value;
                SetVerticesDirty();
                SetMaterialDirty();
            }
        }

        [SerializeField]
        int[] _roundRadius = new int[] { 1, 1, 1, 1 };
        public int[] roundRadius
        {
            get { return _roundRadius; }
            set
            {
                if (_roundRadius == value) return;
                _roundRadius = value;
                SetVerticesDirty();
                SetMaterialDirty();
            }
        }

        [SerializeField]
        [Range(0, 25)] int _detail = 25;
        public int detail
        {
            get { return _detail; }
            set
            {
                if (_detail == value) return;
                _detail = value;
                SetVerticesDirty();
                SetMaterialDirty();
            }
        }

        bool AllNotRound
        {
            get
            {
                for (int i = 0; i < roundCorner.Length; i++)
                    if (roundCorner[i])
                        return false;
                return true;
            }
        }

        public void SetAllRoundCorner(bool f)
        {
            for (int i = 0; i < _roundCorner.Length; i++)
                _roundCorner[i] = f;
            SetVerticesDirty();
            SetMaterialDirty();
        }

        public void SetAllRoundRadius(int x)
        {
            for (int i = 0; i < _roundRadius.Length; i++)
                _roundRadius[i] = x;
            SetAllRoundCorner(x != 0);
        }

        override public Vector2[] GetShapeVertices()
        {
            Vector2[] rtn = null;

            Vector2[] corner = new Vector2[]
            {
            new Vector2(-1, -1),
            new Vector2(-1, 1),
            new Vector2(1, 1),
            new Vector2(1, -1)
            };

            if (AllNotRound)
                rtn = corner;
            else
            {
                List<Vector2> points = new List<Vector2>();
                for (int i = 0; i < corner.Length; i++)
                {
                    if (!roundCorner[i])
                    {
                        points.Add(corner[i]);
                        continue;
                    }
                    else
                    {
                        float roundRadiusX = Mathf.Clamp((roundRadius[i]) / rectTransform.rect.width, 0, 1);
                        float roundRadiusY = Mathf.Clamp((roundRadius[i]) / rectTransform.rect.height, 0, 1);
                        Vector2 ori = new Vector2(corner[i].x + (corner[i].x > 0 ? -1 : 1) * roundRadiusX,
                                                            corner[i].y + (corner[i].y > 0 ? -1 : 1) * roundRadiusY);
                        for (int j = 0; j < detail; j++)
                            points.Add(
                                ori - new Vector2(roundRadiusX * Mathf.Sin(2 * Mathf.PI / (detail * 4) * (j + i * detail)),
                                                  roundRadiusY * Mathf.Cos(2 * Mathf.PI / (detail * 4) * (j + i * detail))));
                    } 
                }
                rtn = points.ToArray();
            }
            

            return rtn;
        }

    }
}
