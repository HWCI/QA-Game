using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace creXa.GameBase
{
    [AddComponentMenu("creXa/LangSys/ZText")]
    [RequireComponent(typeof(Text))]
    public class ZText : ZLangObj
    {
        public Text text;

        #region Properties

        [SerializeField] Lang[] _langs;
        public Lang[] Langs
        {
            get { return _langs; }
            set { _langs = value; Refresh(); }
        }
        public void SetLang(int id, Lang langObj)
        {
            _langs[id] = langObj;
            Refresh();
        }

        [Serializable]
        public class Lang
        {
            public string content;
            public Font font;
            public int fontSize;
            public FontStyle fontStyle;
            public float lineSpacing;

            public bool setFont = false;
            public bool setFontSize = false;
            public bool setFontStyle = false;
            public bool setLineSpacing = false;
        }

        #endregion

        override protected void Link()
        {
            if (!text) text = GetComponent<Text>();
        }

        override protected bool LoadByID(ZLangSys sys)
        {
            ChangeAll();

            int startIdx = ID.IndexOf('[');
            int endIdx = ID.IndexOf(']');

            if (startIdx >= 0 && endIdx > startIdx)
            {
                string strID = ID.Substring(startIdx + 1, endIdx - startIdx - 1);
                text.text = ID.Substring(0, startIdx) + sys.GetByID(strID, Language) + ID.Substring(endIdx + 1, ID.Length - endIdx - 1);
            }
            else
            {
                text.text = sys.GetByID(ID, Language);
            }

            return true;
        }

        override protected bool LoadInLocal()
        {
            ChangeAll();
            text.text = (_langs != null && Language < _langs.Length) ? _langs[Language].content.Replace("\\n", "\n") : "";
            return true;
        }

        private void ChangeAll()
        {
            ChangeFont();
            ChangeFontSize();
            ChangeLineSpacing();
            ChangeFontStyle();
        }

        private void ChangeFont()
        {
            if (_langs != null && Language < _langs.Length && _langs[Language].setFont)
            {
                text.font = _langs[Language].font;
            }
            else
            {
                if (ZLangSys.It && Language < ZLangSys.It.Language.Length)
                    text.font = ZLangSys.It.Language[Language].defaultFont;
            }
        }

        private void ChangeFontSize()
        {
            if (_langs != null && Language < _langs.Length && _langs[Language].setFontSize)
                text.fontSize = _langs[Language].fontSize;
        }

        private void ChangeLineSpacing()
        {
            if (_langs != null && Language < _langs.Length && _langs[Language].setLineSpacing)
                text.lineSpacing = _langs[Language].lineSpacing;
        }

        private void ChangeFontStyle()
        {
            if (_langs != null && Language < _langs.Length && _langs[Language].setFontStyle)
                text.fontStyle = _langs[Language].fontStyle;
        }

#if UNITY_EDITOR
        void Reset()
        {
            if(ZLangSys.It && ZLangSys.It.Language != null)
            {
                _langs = new Lang[ZLangSys.It.Language.Length];
                for (int i = 0; i < _langs.Length; i++)
                    _langs[i] = new Lang();
            }  
        }
#endif

    }
}
