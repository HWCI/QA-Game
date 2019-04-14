using UnityEngine;
using System.Collections;

namespace creXa.GameBase
{
    [AddComponentMenu("creXa/Base/Input")]
    [RequireComponent(typeof(ZBase))]
    public class ZInput : ZSingleton<ZInput>
    {
        public bool defaultTouchCtrl;
        public float defaultTouchInterval = 0.1f;

        public bool isTouchCtrl;
        public float touchInterval;

		public delegate void OnSwipeDel(float distance, float direction, float speed);
		public event OnSwipeDel OnSwipe;

        //runtime variables
        bool touchlock = false;
        float timer = 0.0f;
		Vector2 touchStartPos;
		float touchStartTime;

        protected override void AwakeRun()
        {
            base.AwakeRun();
            isTouchCtrl = defaultTouchCtrl;
            touchInterval = defaultTouchInterval;
        }

        protected virtual void Update() { UpdateRun(); }
        protected virtual void UpdateRun()
        {
            if (isTouchCtrl && touchlock)
            {
                timer -= Time.deltaTime;
                if (timer <= 0.0f)
                {
                    touchlock = false;
                }
            }

			//Swipe
			if (OnSwipe != null)
			{
				if(Input.touchCount > 0){

					//Start
					if (Input.GetTouch(0).phase == TouchPhase.Began)
					{
						touchStartPos = Input.GetTouch(0).position;
						touchStartTime = Time.time;
					}

					//End
					if (Input.GetTouch(0).phase == TouchPhase.Ended)
					{
						Vector2 touchEndPos = Input.GetTouch(0).position;
						EndSwipe(touchEndPos);
					}
				}

				if (Input.GetMouseButtonDown(0))
				{
					touchStartPos = Input.mousePosition;
					touchStartTime = Time.time;
				}

				if (Input.GetMouseButtonUp(0))
				{
					Vector2 touchEndPos = Input.mousePosition;
					EndSwipe(touchEndPos);
				}
			}

        }

		void EndSwipe(Vector2 touchEndPos)
		{
			Vector2 delta = touchEndPos - touchStartPos;
			float touchDuration = Time.time - touchStartTime;

			float touchDistance = Vector2.Distance(touchStartPos, touchEndPos);
			float touchDirection = Mathf.Atan2(delta.y , delta.x) * (180.0f / Mathf.PI);
			if (touchDirection < 0) touchDirection += 360;

			float touchSpeed = touchDistance / touchDuration;
			if (OnSwipe != null) OnSwipe(touchDistance, touchDirection, touchSpeed);
		}

        public bool Touched()
        {
            if (!isTouchCtrl) return false;
            if (touchlock) return false;
            touchlock = true;
            timer = touchInterval;
            return true;
        }

    }
}
