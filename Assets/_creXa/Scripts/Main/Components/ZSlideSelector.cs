using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using creXa.GameBase.Graphics;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace creXa.GameBase
{

    public class ZSlideSelector : MonoBehaviour
    {
        
        [SerializeField] int _selectedValue;
        public int SelectedValue
        {
            get { return _selectedValue; }
            set
            {
                if (_selectedValue == value) return;
                Select(value);
                 _selectedValue = value;
                if (OnValueChanged != null) OnValueChanged.Invoke();
            }
        }

        public int defaultValue = 0;
        public bool vertical = true;
        public ScrollRect scrollRect;
        public ZSelectable[] selectables;
        public float showNextRatio = 0.5f;
        public float cellWidth = 200, cellHeight = 100;

        public UnityEvent OnValueChanged = new UnityEvent();
        public EventTrigger ev;

        void Awake()
        {
            EventTrigger.Entry startDrag = new EventTrigger.Entry();
            startDrag.eventID = EventTriggerType.BeginDrag;
            startDrag.callback.RemoveAllListeners();
            startDrag.callback.AddListener(OnScrollRectStartDrag);
            ev.triggers.Add(startDrag);

            EventTrigger.Entry endDrag = new EventTrigger.Entry();
            startDrag.eventID = EventTriggerType.EndDrag;
            startDrag.callback.RemoveAllListeners();
            startDrag.callback.AddListener(OnScrollRectEndDrag);
            ev.triggers.Add(endDrag);

        }

        void OnEnable()
        {
            scrollRect.onValueChanged.RemoveAllListeners();
            scrollRect.onValueChanged.AddListener(OnScrollRectValueChanged);
        }

        void OnDisable()
        {
            scrollRect.onValueChanged.RemoveAllListeners();
        }

        public void OnScrollRectValueChanged(Vector2 pos)
        {
            int region = Mathf.FloorToInt((vertical ? (1 - pos.y) : pos.x) * selectables.Length);
            region = Mathf.Clamp(region, 0, selectables.Length - 1);
            SelectedValue = region;
        }

        public void OnScrollRectStartDrag(BaseEventData data)
        {
            StopAllCoroutines();
        }

        public void OnScrollRectEndDrag(BaseEventData data)
        {
            float pos = (float)_selectedValue / (selectables.Length - 1);
            Vector2 newPos = new Vector2(vertical ? 0 : pos, vertical ? (1 - pos) : 0);
            StartCoroutine(ZTween.V2(scrollRect.normalizedPosition, newPos, ZBase.It.defDUR, SetNormalizedPos));
        }

        void SetNormalizedPos(Vector2 norm)
        {
            scrollRect.normalizedPosition = norm;
        }


        public void Select(int idx)
        {
            for (int i = 0; i < selectables.Length; i++)
                selectables[i].Selected = (i == idx);
        }

        public void SetValue(int x)
        {
            Select(x);
            float pos = (float)x / (selectables.Length - 1);
            scrollRect.normalizedPosition = new Vector2(vertical? 0 : pos, vertical? (1- pos) : 0);
        }

        public void SetText(string[] str)
        {
            for (int i = 0; i < selectables.Length; i++)
            {
                Text tx = selectables[i].GetComponentInChildren<Text>();
                if (!tx) continue;
                tx.text = str[i];
            }
        }

        public void SetZText(string[] str)
        {
            for (int i = 0; i < selectables.Length; i++)
            {
                if (i >= str.Length) break;
                ZText tx = selectables[i].GetComponentInChildren<ZText>();
                if (!tx) continue;
                tx.ID = str[i];
            }
        }

    }
}
