using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace creXa.GameBase
{
    public class ZColorPicker : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler
    {
        RectTransform rect;
        Canvas canvas;
        public bool isDraggable;
        public Color pickedColor = Color.white;
        public GameObject picker;
        public Image colorPanel;
        public UnityEvent OnValueChanged = new UnityEvent();

        [Range(0, 1)]
        public float minAlpha = 1;

        void Start()
        {
            colorPanel = GetComponent<Image>();
            rect = GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();

            Debug.Log(TakeColorAt(0, 0));
            Vector2 pos = RectTransformUtility.WorldToScreenPoint(Camera.main, picker.transform.position);

            Vector2 posInImage = GetClickPosAtImage(pos);
            Color actColor = TakeColorAt(Mathf.RoundToInt(posInImage.x), Mathf.RoundToInt(posInImage.y));
            if (actColor.a >= minAlpha)
            {
                pickedColor = actColor;
            }
        }

        public Vector2 GetClickPosAtImage(Vector2 pos)
        {
            Vector2 rtn;
            Vector2 thisPos;
            if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                thisPos = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, transform.position);
            }
            else
            {
                thisPos = transform.position;
            }
            Debug.Log("Pos: " + pos);
            Debug.Log("thisPos: " + thisPos);
            Debug.Log("scaleFactor" + canvas.scaleFactor);
            rtn.x = (pos.x - thisPos.x) * (1 / rect.localScale.x) * (1 / canvas.scaleFactor) * (colorPanel.sprite.texture.width / rect.sizeDelta.x) + colorPanel.sprite.texture.width * rect.pivot.x;
            rtn.y = (pos.y - thisPos.y) * (1 / rect.localScale.x) * (1 / canvas.scaleFactor) * (colorPanel.sprite.texture.height / rect.sizeDelta.y) + colorPanel.sprite.texture.height * rect.pivot.y;

            Debug.Log("rtn: " + rtn);
            return rtn;
        }

        public Color TakeColorAt(int x, int y)
        {
            return colorPanel.sprite.texture.GetPixel(x, y);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (isDraggable)
            {
                Vector2 posInImage = GetClickPosAtImage(eventData.position);
                Color actColor = TakeColorAt(Mathf.RoundToInt(posInImage.x), Mathf.RoundToInt(posInImage.y));
                if (actColor.a >= minAlpha)
                {
                    pickedColor = actColor;
                    if (OnValueChanged != null) OnValueChanged.Invoke();
                    if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
                    {
                        Vector3 outPos;
                        RectTransformUtility.ScreenPointToWorldPointInRectangle(rect, eventData.position, canvas.worldCamera, out outPos);
                        picker.transform.position = outPos;
                    }
                    else
                    {
                        picker.transform.position = eventData.position;
                    }
                    
                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isDraggable = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isDraggable = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            OnPointerDown(eventData);
        }
    }
}
