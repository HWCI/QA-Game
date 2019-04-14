using UnityEngine;
using System.Collections;

namespace creXa.GameBase
{
    public abstract class ZLangObj : MonoBehaviour
    { 
        [SerializeField] private string _ID = "";
        public string ID {
            set { _ID = value; if (value != "") loadInBank = true; Refresh(); }
            get { return _ID; }
        }

        protected int _Language = -1;
        public int Language { set { _Language = value; Refresh(); } get { return _Language; } }

        public bool loadInBank = true;

        protected bool _dirty;
        public bool dirty {
            set { _dirty = value; }
            get { return _dirty; }
        }

        void Awake()
        {
            Link();
            _dirty = true;
        }

        void LateUpdate()
        {
            if(_dirty && Time.frameCount % 20 == 0) Refresh();
        }

        abstract protected void Link();
        abstract protected bool LoadByID(ZLangSys sys);
        abstract protected bool LoadInLocal();
#if UNITY_EDITOR
        protected void Refresh()
        {
            dirty = true;
            Link();
            if (_Language < 0) { dirty = false; return; }
            ZLangSys sys = FindObjectOfType<ZLangSys>();
            if (!sys) { Debug.Log("ZLangSys is required."); return; }
            if (sys && _ID != "" && loadInBank)
            {
                if (Language >= sys.Language.Length) { dirty = true; return; }
                if (sys.Language[Language].dictionary == null) { dirty = true; return; }
                dirty = !LoadByID(sys);
            }
            else
                dirty = !LoadInLocal();

        }
#else
        protected void Refresh()
        {
            dirty = true;
            Link();
            if (_Language < 0) { dirty = false; return; }
            if (!ZLangSys.It) { Debug.Log("ZLangSys is required."); return; }
            if (ZLangSys.It && _ID != "" && loadInBank)
            {
                if (Language >= ZLangSys.It.Language.Length) { dirty = true; return; }
                if (ZLangSys.It.Language[Language].dictionary == null) { dirty = true; return; }
                dirty = !LoadByID(ZLangSys.It);
            }
            else
                dirty = !LoadInLocal();

        }
#endif
    }
}
