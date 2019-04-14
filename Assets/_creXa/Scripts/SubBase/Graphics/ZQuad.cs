using UnityEngine;
using System.Collections;

namespace creXa.GameBase.Graphics
{
    public abstract class ZQuad : ZShape
    {
        int _side = 4;
        public int Side
        {
            get { return _side; }
        }
    }
}
