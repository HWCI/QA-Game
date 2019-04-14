using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using creXa.GameBase.Graphics;

namespace creXa.GameBase
{
    public class ZSelectable : MonoBehaviour
    {
        public Button btn;
        public bool Interactable
        {
            get { return btn ? btn.interactable : false; }
            set { if(btn) btn.interactable = value; }
        }

        public ZThemeObj btnTheme;
        public ZThemeObj imgTheme;
        public ZShape zshape;
        public Graphic zsGrahpic;

        public bool useTheme = true;
        public int unselectedBtnTheme = 20, unselectedImgTheme = 19;
        public int selectedBtnTheme = 21, selectedImgTheme = 22;
        public Color unselectedBtnColor, unselectedBtnBorder, unselectedCapColor;
        public Color selectedBtnColor, selectedBtnBorder, selectedCapColor;

        [SerializeField] bool _selected = false;
        public bool Selected
        {
            get { return _selected; }
            set { if (_selected != value) ThemeChange(value); _selected = value; }
        }

        public void Init(bool f)
        {
            _selected = f;
            ThemeChange(f);
        }

        void ThemeChange(bool f)
        {
            if (useTheme)
            {
                if(btnTheme)
                    btnTheme.Variation = f ? selectedBtnTheme : unselectedBtnTheme;
                if(imgTheme)
                    imgTheme.Variation = f ? selectedImgTheme : unselectedImgTheme;
            }
            else
            {
                if(zshape)
                    zshape.color = f ? selectedBtnColor : unselectedBtnColor;
                if (zshape)
                    zshape.borderColor = f ? selectedBtnBorder : unselectedBtnBorder;
                if (zsGrahpic)
                    zsGrahpic.color = f ? selectedCapColor : unselectedCapColor;
            }
            if (zshape) zshape.SetAllDirty();
            if (zsGrahpic) zsGrahpic.SetAllDirty();
            
        }
    }
}


