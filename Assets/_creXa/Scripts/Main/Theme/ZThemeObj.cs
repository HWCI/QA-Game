using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace creXa.GameBase
{
    public class ZThemeObj : MonoBehaviour
    {
        public Graphic graphic;

        [SerializeField]
        protected int _variation;
        public int Variation
        {
            set { _variation = value; Refresh(); }
            get { return _variation; }
        }

        protected int _theme = 0;
        public int Theme { set { _theme = value; Refresh(); } get { return _theme; } }

        protected bool _dirty;
        public bool dirty
        {
            set { _dirty = value; }
            get { return _dirty; }
        }

        void Awake()
        {
            Link();
            _dirty = true;
        }

        protected virtual void Link()
        {
            if(!graphic) graphic = GetComponent<Graphic>();
        }

#if UNITY_EDITOR

        protected void Refresh()
        {
            dirty = true;
            Link();
            if (_theme < 0 || _variation < 0) { dirty = false; return; }
            ZThemeSys sys = FindObjectOfType<ZThemeSys>();
            if (!sys) return;
            if (_theme >= sys.Themes.Length || _variation >= sys.Themes[_theme].varies.Length)
            { dirty = false; return; }

            RefreshLoad(sys);
        }
#else
        protected void Refresh()
        {
            dirty = true;
            Link();
            if (_theme < 0 || _variation < 0) { dirty = false; return; }
            if (!ZThemeSys.It) {

                Debug.Log("ZThemeSys is required.");
                return;
            }
            if (_theme >= ZThemeSys.It.Themes.Length || _variation >= ZThemeSys.It.Themes[_theme].varies.Length)
                { dirty = false; return; }

            RefreshLoad(ZThemeSys.It);
        }
#endif

        protected virtual void RefreshLoad(ZThemeSys sys)
        {
            if (sys.animated && gameObject.activeInHierarchy && gameObject.activeSelf)
            {
                StartCoroutine(ZTween.Color(graphic.color, sys.Themes[_theme].varies[_variation].Color, sys.animatedDUR, RefreshSetColor));
            }   
            else
                RefreshSetColor(sys.Themes[_theme].varies[_variation].Color);
        }

        protected void RefreshSetColor(Color c)
        {
            graphic.color = c;
        }
    }
}
