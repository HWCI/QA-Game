using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace creXa.GameBase
{
    public class ZSelectableGroup : MonoBehaviour
    {
        int _value;
        public int Value
        {
            get { return _value; }
            set { _value = value; Select(value); }
        }

        public UnityEvent OnValueChange;

        public bool Interactable
        {
            set
            {
                for (int i = 0; i < selectables.Length; i++)
                    selectables[i].Interactable = value;
            }
        }

        public ZSelectable[] selectables;

        void Awake()
        {
            for (int i = 0; i < selectables.Length; i++)
            {
                selectables[i].btn.onClick.RemoveAllListeners();
                int x = i;
                selectables[i].btn.onClick.AddListener(() => Value = x);
            }
        }

        void Select(int x)
        {
            for (int i = 0; i < selectables.Length; i++)
                selectables[i].Selected = x == i;
            if (OnValueChange != null) OnValueChange.Invoke();
        }
    }
}
