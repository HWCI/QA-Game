using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace creXa.GameBase.Graphics
{
    public class ZThemeObjZS : ZThemeObj
    {
        public ZShape shape;

        protected override void Link()
        {
            base.Link();
            if (!shape) shape = GetComponent<ZShape>();
        }

        protected override void RefreshLoad(ZThemeSys sys)
        {
            if (sys.animated && gameObject.activeInHierarchy && gameObject.activeSelf)
            {
                StartCoroutine(ZTween.Color(graphic.color, sys.Themes[_theme].varies[_variation].Color, sys.animatedDUR, RefreshSetColor));
                StartCoroutine(ZTween.Color(shape.borderColor, sys.Themes[_theme].varies[_variation].BorderColor, sys.animatedDUR, RefreshSetBorder));
            }
            else
            {
                RefreshSetColor(sys.Themes[_theme].varies[_variation].Color);
                RefreshSetBorder(sys.Themes[_theme].varies[_variation].BorderColor);
            }
                
        }

        protected void RefreshSetBorder(Color c)
        {
            shape.borderColor = c;
        }
    }
}


