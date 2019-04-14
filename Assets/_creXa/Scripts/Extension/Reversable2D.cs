using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace creXa.GameBase
{
    [AddComponentMenu("creXa/Animation/Reversable2D")]
    public class Reversable2D : ZAnimated2D
    {
        [ReadOnly][SerializeField] int _currentCSS;
        public int CurrentCSS
        {
            get { return _currentCSS; }
            set
            {
                _currentCSS = value;
                if (value < 0 || value >= stateCSS.Length) return;
                if (CtrlPos) SetPos(stateCSS[value].position);
                if (CtrlScale) SetScale(stateCSS[value].scale);
                if (CtrlColor) SetColor(stateCSS[value].color);
            }
        }

        public CSS[] stateCSS;
        [Serializable]
        public class CSS
        {
            public Vector3 position;
            public Vector3 scale;
            public Color color;
            public int nextState = -1;
            public float toNextDUR = -1;
        }

        public delegate void OnEndSequenceDel();
        public OnEndSequenceDel OnEndSequence;

        public float defaultDUR = 0.2f;
        public bool CtrlPos = false;
        public bool CtrlScale = false;
        public bool CtrlColor = false;

        protected override void AwakeRun()
        {
            base.AwakeRun();
            CurrentCSS = 0;
        }

        public void Trigger(int next)
        {

            if (next < 0 || next >= stateCSS.Length)
            {
                if (OnEndSequence != null) OnEndSequence();
                return;
            }
            Tween(_currentCSS, next, stateCSS[_currentCSS].toNextDUR < 0 ? defaultDUR : stateCSS[_currentCSS].toNextDUR);
        }

        public void TriggerNext()
        {
            int next = _currentCSS + 1;
            if (next == stateCSS.Length) next = 0;
            Trigger(next);
        }

        void Tween(int from, int to, float duration)
        {
            StartCoroutine(TweenIE(from, to, duration));
        }

        IEnumerator TweenIE(int from, int to, float duration)
        {
            if (CtrlPos) StartCoroutine(PositionTween(stateCSS[from].position, stateCSS[to].position, duration));
            if (CtrlScale) StartCoroutine(SizeTween(stateCSS[from].scale, stateCSS[to].scale, duration));
            if (CtrlColor) StartCoroutine(RGBColorTween(stateCSS[from].color, stateCSS[to].color, duration));
            yield return new WaitForSeconds(duration);
            CurrentCSS = to;
            Trigger(stateCSS[CurrentCSS].nextState);
        }
        
    }
}
