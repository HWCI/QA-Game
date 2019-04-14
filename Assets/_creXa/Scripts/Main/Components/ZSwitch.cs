using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using creXa.GameBase.Graphics;
using UnityEngine.Events;

namespace creXa.GameBase
{
    public class ZSwitch : MonoBehaviour, IPointerDownHandler
    {
        RectTransform rect;

        [ReadOnly][SerializeField] bool _value = true;
        public bool Value
        {
            get { return _value; }
            set { DoAni(value); _value = value; if (OnValueChanged != null) OnValueChanged.Invoke(); }
        }

        public bool interactable = true;
        public bool animated = true;

        public ZSRectangle bg;
        public ZSCircle btn;

        public ZAnimated2D bgAni;
        public ZAnimated2D btnAni;

        public Color onColor = new Color(0, 0.6f, 0, 1);
        public Color offColor = new Color(0.6f, 0, 0, 1);

        public UnityEvent OnValueChanged = new UnityEvent();

        void Awake()
        {
            rect = GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, rect.sizeDelta.x / 2);
            bg.GetComponent<RectTransform>().sizeDelta = rect.sizeDelta;
            bg.SetAllRoundRadius(Mathf.RoundToInt(rect.sizeDelta.x / 2));
            bg.transform.localPosition = Vector2.zero;
            btn.GetComponent<RectTransform>().sizeDelta = new Vector2(rect.sizeDelta.x, rect.sizeDelta.x) * 0.45f;
            
            btnAni.SetRectPos(new Vector3((_value ? 1 : -1) * rect.sizeDelta.x / 4, 0, 0));
            bgAni.SetColor(_value ? onColor : offColor);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!interactable) return;
            Value = !Value;
            if (OnValueChanged != null) OnValueChanged.Invoke();
        }

        public void Set(bool f)
        {
            _value = f;
        }

        void DoAni(bool f)
        {
            if (f == _value) return;
            if (!rect) rect = GetComponent<RectTransform>();
            if(gameObject.activeSelf && gameObject.activeInHierarchy && animated)
            {
                StartCoroutine(bgAni.RGBColorTween(_value ? onColor : offColor, f ? onColor : offColor,  ZBase.It.defDUR));
                StartCoroutine(btnAni.RectPositionTween(
                    new Vector3((_value ? 1 : -1) * rect.sizeDelta.x / 4, 0, 0),
                    new Vector3((f ? 1 : -1) * rect.sizeDelta.x / 4, 0, 0),
                    ZBase.It.defDUR));
            }
            else
            {
                bgAni.SetColor(f ? onColor : offColor);
                btnAni.SetRectPos(new Vector3((f ? 1 : -1) * rect.sizeDelta.x / 4, 0, 0));
            }
            
        }


    }
}


