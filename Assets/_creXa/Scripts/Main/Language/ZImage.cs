using UnityEngine;
using UnityEngine.UI;

namespace creXa.GameBase
{
    [AddComponentMenu("creXa/LangSys/ZImage")]
    [RequireComponent(typeof(Image))]
    public class ZImage : ZLangObj
    {
        public Image image;

        #region Properties

        [SerializeField] private Sprite[] _content;
        public Sprite[] content {
            set { _content = value; Refresh(); }
            get { return _content; }
        }

        #endregion

        override protected void Link()
        {
            if (!image) image = GetComponent<Image>();
        }

        override protected bool LoadByID(ZLangSys sys)
        {
            if (sys && !sys.Language[Language].dictionary.ContainsKey(ID))
            {
                image.sprite = null;
                return false;
            }
            image.sprite = Resources.Load<Sprite>("Language/" + sys.Language[Language].dictionary[ID]);
            return true;
        }

        override protected bool LoadInLocal()
        {
            image.sprite = content[Language];
            return true;
        }

    }
}
