using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace creXa.GameBase
{
    public class ZAnimated2DTC : ZAnimated2D
    {
        public RandomFloat initTime;

        protected float timer = 0.0f;

        protected override void AwakeRun()
        {
            base.AwakeRun();
            timer = initTime * timeCycle * timeScale;
        }

        protected virtual void Update() { UpdateRun(); }
        protected virtual void UpdateRun()
        {
            if (pause) return;
            timer += Time.deltaTime;
        }
    }
}
