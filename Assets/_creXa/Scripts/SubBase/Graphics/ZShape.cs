using UnityEngine;
using UnityEngine.UI;

namespace creXa.GameBase.Graphics
{
    [ExecuteInEditMode]
    public abstract class ZShape : MaskableGraphic
    {
        [SerializeField] Texture _texture;
        public override Texture mainTexture
        {
            get { return _texture == null ? s_WhiteTexture : _texture; }
        }
        public Texture texture
        {
            get { return _texture; }
            set
            {
                if (_texture == value) return;
                _texture = value;
                SetVerticesDirty();
                SetMaterialDirty();
            }
        }

        [SerializeField]
        bool _border = true;
        public bool border
        {
            get { return _border; }
            set
            {
                if (_border == value) return;
                _border = value;
                SetVerticesDirty();
                SetMaterialDirty();
            }
        }

        [SerializeField]
        int _borderWidth = 1;
        public int borderWidth
        {
            get { return _borderWidth; }
            set
            {
                if (_borderWidth == value) return;
                _borderWidth = value;
                SetVerticesDirty();
                SetMaterialDirty();
            }
        }

        [SerializeField]
        Color _borderColor = Color.black;
        public Color borderColor
        {
            get { return _borderColor; }
            set
            {
                if (_borderColor == value) return;
                _borderColor = value;
                SetVerticesDirty();
                SetMaterialDirty();
            }
        }

        [SerializeField]
        bool _regularSize = false;
        public bool regularSize
        {
            get { return _regularSize; }
            set
            {
                if (_regularSize == value) return;
                _regularSize = value;
                SetVerticesDirty();
                SetMaterialDirty();
            }
        }

        [SerializeField]
        [Range(0, 1)] float _match = 0.5f;
        public float match
        {
            get { return _match; }
            set
            {
                if (_match == value) return;
                _match = value;
                SetVerticesDirty();
                SetMaterialDirty();
            }
        }

        protected UIVertex[] SetVBO(Vector2[] vertices, Vector2[] uvs, Color vColor)
        {
            UIVertex[] vbo = new UIVertex[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                var vert = UIVertex.simpleVert;
                vert.color = vColor;
                vert.position = vertices[i];
                vert.uv0 = uvs[i];
                vbo[i] = vert;
            }
            return vbo;
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            SetVertices(ref vh);
            vh.FillMesh(s_Mesh);
        }

        protected void SetVertices(ref VertexHelper vh)
        {
            vh.Clear();
            Vector2[] vtx = GetShapeVertices();
            if (vtx == null) return;
            float outerW = -rectTransform.pivot.x * rectTransform.rect.width;
            float outerH = -rectTransform.pivot.x * rectTransform.rect.height;
            if (regularSize)
            {
                outerH = outerW = outerH * (1 - match) + outerW * match;
            }
            Vector2[] uv = new Vector2[] {
            new Vector2(0, 0),
            new Vector2(1, 1),
            new Vector2(1, 0),
            new Vector2(0, 0)
        };

            Vector2[] pos = new Vector2[4];
            if(color.a > 0)
            {
                for (int i = 0; i < vtx.Length; i++)
                {
                    Vector2 from = vtx[i];
                    Vector2 to = vtx[(i + 1) % vtx.Length];

                    pos[0] = new Vector2(outerW * from.x, outerH * from.y);
                    pos[1] = new Vector2(outerW * to.x, outerH * to.y);
                    pos[2] = Vector3.zero;
                    pos[3] = Vector3.zero;
                    vh.AddUIVertexQuad(SetVBO(pos, uv, color));
                }
            }

            if (border && borderWidth > 0)
            {
                int seg = 30;
                //draw lines
                for (int i = 0; i < vtx.Length; i++)
                {
                    Vector2 from = vtx[i];
                    Vector2 to = vtx[(i + 1) % vtx.Length];
                    float rad = 360 - Mathf.Atan2(to.y - from.y, to.x - from.x) * 180 / Mathf.PI;
                    rad *= Mathf.Deg2Rad;

                    //Circle Head
                    for (int j = 0; j < seg; j++)
                    {

                        pos[0] = new Vector2(outerW * from.x + borderWidth * Mathf.Sin(rad - Mathf.PI / seg * j),
                                         outerH * from.y + borderWidth * Mathf.Cos(rad - Mathf.PI / seg * j));
                        pos[1] = new Vector2(outerW * from.x + borderWidth * Mathf.Sin(rad - Mathf.PI / seg * (j + 1)),
                                             outerH * from.y + borderWidth * Mathf.Cos(rad - Mathf.PI / seg * (j + 1)));
                        pos[2] = new Vector2(outerW * from.x,
                                         outerH * from.y);
                        pos[3] = new Vector2(outerW * from.x,
                                         outerH * from.y);
                        vh.AddUIVertexQuad(SetVBO(pos, uv, borderColor));

                        pos[0] = new Vector2(outerW * to.x - borderWidth * Mathf.Sin(rad + Mathf.PI / seg * j),
                                         outerH * to.y - borderWidth * Mathf.Cos(rad + Mathf.PI / seg * j));
                        pos[1] = new Vector2(outerW * to.x - borderWidth * Mathf.Sin(rad + Mathf.PI / seg * (j + 1)),
                                             outerH * to.y - borderWidth * Mathf.Cos(rad + Mathf.PI / seg * (j + 1)));
                        pos[2] = new Vector2(outerW * to.x,
                                         outerH * to.y);
                        pos[3] = new Vector2(outerW * to.x,
                                         outerH * to.y);
                        vh.AddUIVertexQuad(SetVBO(pos, uv, borderColor));

                    }

                    pos[0] = new Vector2(outerW * from.x + borderWidth * Mathf.Sin(rad),
                                         outerH * from.y + borderWidth * Mathf.Cos(rad));
                    pos[1] = new Vector2(outerW * to.x + borderWidth * Mathf.Sin(rad),
                                         outerH * to.y + borderWidth * Mathf.Cos(rad));
                    pos[2] = new Vector2(outerW * to.x - borderWidth * Mathf.Sin(rad),
                                         outerH * to.y - borderWidth * Mathf.Cos(rad));
                    pos[3] = new Vector2(outerW * from.x - borderWidth * Mathf.Sin(rad),
                                         outerH * from.y - borderWidth * Mathf.Cos(rad));
                    vh.AddUIVertexQuad(SetVBO(pos, uv, borderColor));

                }
            }
        }

        public abstract Vector2[] GetShapeVertices();

    }
}
