using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace creXa.GameBase
{
    public class ZPageCtrl : MonoBehaviour
    {
        [ReadOnly][SerializeField] int nowPage = -1;
        public int NowPage { get { return nowPage; } }
        public List<CanvasGroup> pages = new List<CanvasGroup>();

        public delegate void OnPageShownDel();
        public OnPageShownDel OnPageShown;

        public delegate void OnPageHiddenDel();
        public OnPageHiddenDel OnPageHidden;

        public void Show(int id, float duration = 0.0f, bool hideOthers = true)
        {
            if (id < 0 || id > pages.Count) return;
            nowPage = id;
            if (!pages[id].gameObject.activeSelf) pages[id].gameObject.SetActive(true);
            if (gameObject.activeSelf) StartCoroutine(PageFadeIn(id, duration));
            if (hideOthers)
                HideOthers(id, duration);
        }

        public void CrossFade(int id, float duration = 0.0f, float delay = 0.0f, bool hideOthers = true)
        {
            if (id < 0 || id > pages.Count) return;
            nowPage = id;
            if (!pages[id].gameObject.activeSelf) pages[id].gameObject.SetActive(true);
            if (gameObject.activeSelf) StartCoroutine(PageFadeIn(id, duration));
            if (hideOthers)
                HideOthers(id, duration, delay);
        }

        public void HideOthers(int except = -1, float duration = 0.0f, float delay = 0.0f)
        {
            for (int i = 0; i < pages.Count; i++)
                if (i != except && pages[i].gameObject.activeSelf)
                    if (gameObject.activeSelf) StartCoroutine(PageFadeOut(i, duration, delay));
                    else pages[i].gameObject.SetActive(false);
        }

        public void HideAll(float duration = 0.0f)
        {
            for (int i = 0; i < pages.Count; i++)
                if (pages[i].gameObject.activeSelf)
                    Hide(i, duration);
        }

        public void Hide(int id, float duration = 0.0f)
        {
            if (gameObject.activeSelf) StartCoroutine(PageFadeOut(id, duration));
            else pages[id].gameObject.SetActive(false);
        }

        IEnumerator PageFadeOut(int id, float duration, float delay = 0.0f)
        {
            if(delay > 0) yield return new WaitForSeconds(delay);
            if (!pages[id].gameObject.activeSelf) yield break;
            if (duration > 0)
                yield return StartCoroutine(PageFade(id, 1, 0, duration));
            else
                pages[id].alpha = 0;
            if (pages[id].gameObject.activeSelf) pages[id].gameObject.SetActive(false);
            if (OnPageHidden != null) OnPageHidden();
        }

        IEnumerator PageFadeIn(int id, float duration, float delay = 0.0f)
        {
            if (delay > 0) yield return new WaitForSeconds(delay);
            if (duration > 0)
                yield return StartCoroutine(PageFade(id, 0, 1, duration));
            else
                pages[id].alpha = 1;
            if (OnPageShown != null) OnPageShown();
        }

        IEnumerator PageFade(int id, float ori, float dest, float duration)
        {
            if (!pages[id].gameObject.activeSelf) yield break;
            pages[id].alpha = ori;
            float timer = 0.0f;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                pages[id].alpha = ori + (dest - ori) * Mathf.Min(timer / duration, 1);
                yield return null;
            }
            pages[id].alpha = dest;
        }
    }
}

