using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace creXa.GameBase
{
    [AddComponentMenu("creXa/Varies/ColorVaries2D")]
    public class ColorVaries2D : ZAnimated2DTC
    {
        public NVariesCtrl R, G, B, A;
        public Color offset;

        protected override void UpdateRun()
        {
            base.UpdateRun();

            SetColor(offset + new Color(
                (R.Ctrl ? R.Value.Evaluate(R.Varies.Evaluate(timer / (timeCycle * timeScale))) : GetColor2D().r),
                (G.Ctrl ? G.Value.Evaluate(G.Varies.Evaluate(timer / (timeCycle * timeScale))) : GetColor2D().g),
                (B.Ctrl ? B.Value.Evaluate(B.Varies.Evaluate(timer / (timeCycle * timeScale))) : GetColor2D().b),
                (A.Ctrl ? A.Value.Evaluate(A.Varies.Evaluate(timer / (timeCycle * timeScale))) : GetColor2D().a)
            ));
            
        }
    }
}
