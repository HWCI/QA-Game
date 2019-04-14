using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace creXa.GameBase
{ 
    [AddComponentMenu("creXa/Varies/PositionVaries2D")]
    public class PositionVaries2D : ZAnimated2DTC
    {
        public NVariesCtrl X, Y;
        public Vector3 offset;

        protected override void UpdateRun()
        {
            base.UpdateRun();
            
            SetRectPos(offset + new Vector3(
                X.Ctrl ? X.Value.Evaluate(X.Varies.Evaluate(timer / (timeCycle * timeScale))) : GetRectPos().x,
                Y.Ctrl ? Y.Value.Evaluate(Y.Varies.Evaluate(timer / (timeCycle * timeScale))) : GetRectPos().y,
                GetRectPos().z));
        }
    }
}
