using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace creXa.GameBase
{
    [AddComponentMenu("creXa/Varies/ScaleVaries2D")]
    public class ScaleVaries2D : ZAnimated2DTC
    {
        public NVariesCtrl X, Y;
        public Vector3 offset;

        protected override void UpdateRun()
        {
            base.UpdateRun();
            SetScale(offset + new Vector3(
                X.Ctrl? X.Value.Evaluate(X.Varies.Evaluate(timer / (timeCycle * timeScale))) : GetScale().x, 
                Y.Ctrl? Y.Value.Evaluate(Y.Varies.Evaluate(timer / (timeCycle * timeScale))) : GetScale().y, 
                GetScale().z));
        }
    }
}
