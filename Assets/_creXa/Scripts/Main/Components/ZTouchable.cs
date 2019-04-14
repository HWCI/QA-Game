using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace creXa.GameBase
{
    [AddComponentMenu("creXa/Interaction/ZTouchable")]
    public class ZTouchable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
		public bool interactable = true;

        bool hold = false;
        bool invoked = false;
        float countHold = 0.0f;

        public bool holdingInvoke = true;
        public float holdTime = 1.0f;

        [Serializable]
        public class OnTouchEvent : UnityEvent { }
        public OnTouchEvent OnTouch;

        [Serializable]
        public class OnHoldEvent : UnityEvent { }
        public OnHoldEvent OnHold;

        [Serializable]
        public class OnHoldReleaseEvent : UnityEvent { }
        public OnHoldReleaseEvent OnHoldRelease;

        void Update()
        {
            if (hold)
            {
                countHold += Time.deltaTime;
                if (holdingInvoke && countHold >= holdTime)
                {
                    hold = false;
                    invoked = true;
                    OnHold.Invoke();
                }
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
			if (!interactable) return;

            countHold = 0.0f;
            hold = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (invoked && holdingInvoke)
            {
                invoked = false;
                OnHoldRelease.Invoke();
            }
            else
            {
                hold = false;
                if (countHold >= holdTime) OnHold.Invoke();
                else OnTouch.Invoke();
            }
        }

		public void RemoveAllListeners()
		{
			OnTouch.RemoveAllListeners();
			OnHold.RemoveAllListeners();
			OnHoldRelease.RemoveAllListeners();
		}
    }
}
