using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace creXa.GameBase
{
    public class ZRecord : MonoBehaviour
    {
        public string keyRef;

        public RectTransform[] rects;
        public ZSelectable[] selectable;
        public Button[] button;
        public Text[] field;

        [SerializeField] bool _selected = false;
        public bool Selected
        {
            get { return _selected; }
            set { Select(value); _selected = value; }
        }

        public void Init(string key, string[] _field)
        {
            keyRef = key;
            for(int i=0; i<field.Length; i++)
            {
                if (i >= _field.Length) break;
                field[i].text = _field[i];
            }
        }

        public void SetOnClick(UnityAction<ZRecord> _action)
        {
            for (int i = 0; i < button.Length; i++)
            {
                button[i].onClick.RemoveAllListeners();
                button[i].onClick.AddListener(() => _action(this));
            }
                
        }

        public void SetOnClick(UnityAction<ZRecord, int> _action)
        {
            for (int i = 0; i < button.Length; i++)
            {
                int x = i;
                button[i].onClick.RemoveAllListeners();
                button[i].onClick.AddListener(() => _action(this, x));
            }
                
        }

        void Select(bool f)
        {
            for (int i = 0; i < selectable.Length; i++)
                selectable[i].Selected = f;
        }

        

    }
}

