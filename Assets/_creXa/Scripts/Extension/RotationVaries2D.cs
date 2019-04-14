using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace creXa.GameBase
{
    [AddComponentMenu("creXa/Varies/RotationVaries2D")]
    public class RotationVaries2D : ZAnimated2DTC
    {
        public NVariesCtrl X, Y, Z;
        public Vector3 offset;

        protected override void AwakeRun()
        {
            base.AwakeRun();
        }

        protected override void UpdateRun()
        {
            base.UpdateRun();

            SetRectRot(offset + new Vector3(
                X.Ctrl ? X.Value.Evaluate(X.Varies.Evaluate(timer / (timeCycle * timeScale))) : GetRectRot().x,
                Y.Ctrl ? Y.Value.Evaluate(Y.Varies.Evaluate(timer / (timeCycle * timeScale))) : GetRectRot().y,
                Z.Ctrl ? Z.Value.Evaluate(Z.Varies.Evaluate(timer / (timeCycle * timeScale))) : GetRectRot().z));
        }
    }
}
