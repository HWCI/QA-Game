using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

namespace creXa.GameBase
{
    public class ZPopup : ZSingleton<ZPopup>
    {
        //TODO
        public CanvasGroup background;
        public CanvasGroup msgBox;

        public GameObject msgObj, loadingObj, inputObj, buttonObj;

        public Text msgText, inputText, placeholderText;
        public ZText msgZText, placeholderZText;

        public AButton[] buttons;
        [System.Serializable]
        public class AButton
        {
            public GameObject obj;
            public Button btn;
            public Text caption;
            public ZText ztext;
        }

        public string[] defaultBtnCaption;
        public string[] defaultBtnID;

        public delegate void OnPopupShownDel();
        public event OnPopupShownDel OnPopupShown;

        public delegate void OnBackgroundClickedDel();
        public event OnBackgroundClickedDel OnBackgroundClicked;

        public delegate void OnPopupHiddenDel();
        public event OnPopupHiddenDel OnPopupHidden;

        public delegate void OnCallbackDel(int x);
        public event OnCallbackDel OnCallback;

        public delegate void OnInputCallbackDel(int x, string str);
        public event OnInputCallbackDel OnInputCallback;

        public Mode mode;
        public enum Mode
        {
            None,
            Loading,
            OK,
            OKCancel,
            InputBox, 
            AutoHide
        }

        public float fadeDUR = 0.0f;
        float timer = 0.0f;

        void Update()
        {
            if(mode == Mode.AutoHide && timer > 0)
            {
                timer -= Time.deltaTime;
                Hide(fadeDUR);
            }
        }

        protected override void AwakeRun()
        {
            Button btn = background.gameObject.GetComponent<Button>();
            if(btn) btn.onClick.AddListener(OnBackgroundClick);
            Hide();
        }

        void RemoveAllListeners()
        {
            OnPopupShown = null;
            OnBackgroundClicked = null;
            OnPopupHidden = null;
            OnCallback = null;
            OnInputCallback = null;
            for (int i = 0; i < buttons.Length; i++)
                buttons[i].btn.onClick.RemoveAllListeners();
        }

        void ShowBasic(string showText, float duration)
        {
            if (!msgBox.gameObject.activeSelf) msgBox.gameObject.SetActive(true);
            if (!background.gameObject.activeSelf) background.gameObject.SetActive(true);
            if (gameObject.activeSelf) StartCoroutine(PopupFadeIn(duration));
            if (msgObj.activeSelf != (showText != "")) msgObj.SetActive((showText != ""));
            msgText.text = showText;
        }

        void ZShowBasic(string id, float duration)
        {
            if (!msgBox.gameObject.activeSelf) msgBox.gameObject.SetActive(true);
            if (!background.gameObject.activeSelf) background.gameObject.SetActive(true);
            if (gameObject.activeSelf) StartCoroutine(PopupFadeIn(duration));
            if (msgObj.activeSelf != (id != "")) msgObj.SetActive((id != ""));
            msgZText.ID = id;
        }

        void ClickBGHide(float duration)
        {
            OnBackgroundClicked += delegate { Hide(duration); };
        }

        void SetLoading(bool active)
        {
            if (loadingObj.activeSelf != active) loadingObj.SetActive(active);
        }

        void SetInput(bool active, string ph = "")
        {
            if (inputObj.activeSelf != active) inputObj.SetActive(active);
            if (active) placeholderText.text = ph;
        }

        void ZSetInput(bool active, string ID = "")
        {
            if (inputObj.activeSelf != active) inputObj.SetActive(active);
            if (active) placeholderZText.ID = ID;
        }

        void SetButton(bool active)
        {
            if (buttonObj.activeSelf != active) buttonObj.SetActive(active);
        }

        void ShowButton(int idx, OnCallbackDel callback = null, string[] captions = null)
        {
            SetButton(true);
            OnCallback += callback;
            for (int i = 0; i < buttons.Length; i++)
                if (i == idx)
                {
                    if (!buttons[i].obj.activeSelf) buttons[i].obj.SetActive(true);
                    if (captions != null && i < captions.Length)
                        buttons[i].caption.text = captions[i];
                    else
                        buttons[i].caption.text = defaultBtnCaption[i];
                    buttons[i].btn.onClick.RemoveAllListeners();
                    int x = i;
                    buttons[i].btn.onClick.AddListener(() => OnButtonClick(x));
                }
                else
                {
                    if (buttons[i].obj.activeSelf) buttons[i].obj.SetActive(false);
                }
        }

        void ZShowButton(int idx, OnCallbackDel callback = null, string[] captions = null)
        {
            SetButton(true);
            OnCallback += callback;
            for (int i = 0; i < buttons.Length; i++)
                if (i == idx)
                {
                    if (!buttons[i].obj.activeSelf) buttons[i].obj.SetActive(true);
                    if (captions != null && i < captions.Length)
                        buttons[i].ztext.ID = captions[i];
                    else
                        buttons[i].ztext.ID = defaultBtnID[i];
                    buttons[i].btn.onClick.RemoveAllListeners();
                    int x = i;
                    buttons[i].btn.onClick.AddListener(() => OnButtonClick(x));
                }
                else
                {
                    if (buttons[i].obj.activeSelf) buttons[i].obj.SetActive(false);
                }
        }

        void ShowButtonForInput(int idx, OnInputCallbackDel callback = null, string[] captions = null)
        {
            SetButton(true);
            OnInputCallback += callback;
            for (int i = 0; i < buttons.Length; i++)
                if (i == idx)
                {
                    if (!buttons[i].obj.activeSelf) buttons[i].obj.SetActive(true);
                    if (captions != null && i < captions.Length)
                        buttons[i].caption.text = captions[i];
                    else
                        buttons[i].caption.text = defaultBtnCaption[i];
                    buttons[i].btn.onClick.RemoveAllListeners();
                    int x = i;
                    buttons[i].btn.onClick.AddListener(() => OnButtonInputClick(x));
                }
                else
                {
                    if (buttons[i].obj.activeSelf) buttons[i].obj.SetActive(false);
                    buttons[i].btn.onClick.RemoveAllListeners();
                }
        }

        void ZShowButtonForInput(int idx, OnInputCallbackDel callback = null, string[] captions = null)
        {
            SetButton(true);
            OnInputCallback += callback;
            for (int i = 0; i < buttons.Length; i++)
                if (i == idx)
                {
                    if (!buttons[i].obj.activeSelf) buttons[i].obj.SetActive(true);
                    if (captions != null && i < captions.Length)
                        buttons[i].ztext.ID = captions[i];
                    else
                        buttons[i].ztext.ID = defaultBtnID[i];
                    buttons[i].btn.onClick.RemoveAllListeners();
                    int x = i;
                    buttons[i].btn.onClick.AddListener(() => OnButtonInputClick(x));
                }
                else
                {
                    if (buttons[i].obj.activeSelf) buttons[i].obj.SetActive(false);
                    buttons[i].btn.onClick.RemoveAllListeners();
                }
        }

        void ShowButtons(int num, OnCallbackDel callback = null, string[] captions = null)
        {
            SetButton(true);
            OnCallback += callback;
            for (int i = 0; i < buttons.Length; i++)
                if (i < num)
                {
                    if (!buttons[i].obj.activeSelf) buttons[i].obj.SetActive(true);
                    if(captions != null && i < captions.Length)
                        buttons[i].caption.text = captions[i];
                    else
                        buttons[i].caption.text = defaultBtnCaption[i];
                    buttons[i].btn.onClick.RemoveAllListeners();
                    int x = i;
                    buttons[i].btn.onClick.AddListener(() => OnButtonClick(x));
                }
                else
                {
                    if (buttons[i].obj.activeSelf) buttons[i].obj.SetActive(false);
                }
        }

        void ZShowButtons(int num, OnCallbackDel callback = null, string[] captions = null)
        {
            SetButton(true);
            OnCallback += callback;
            for (int i = 0; i < buttons.Length; i++)
                if (i < num)
                {
                    if (!buttons[i].obj.activeSelf) buttons[i].obj.SetActive(true);
                    if (captions != null && i < captions.Length)
                        buttons[i].ztext.ID = captions[i];
                    else
                        buttons[i].ztext.ID = defaultBtnID[i];
                    buttons[i].btn.onClick.RemoveAllListeners();
                    int x = i;
                    buttons[i].btn.onClick.AddListener(() => OnButtonClick(x));
                }
                else
                {
                    if (buttons[i].obj.activeSelf) buttons[i].obj.SetActive(false);
                }
        }

        void ShowButtonsForInput(int num, OnInputCallbackDel callback = null, string[] captions = null)
        {
            SetButton(true);
            OnInputCallback += callback;
            for (int i = 0; i < buttons.Length; i++)
                if (i < num)
                {
                    if (!buttons[i].obj.activeSelf) buttons[i].obj.SetActive(true);
                    if (captions != null && i < captions.Length)
                        buttons[i].caption.text = captions[i];
                    else
                        buttons[i].caption.text = defaultBtnCaption[i];
                    buttons[i].btn.onClick.RemoveAllListeners();
                    int x = i;
                    buttons[i].btn.onClick.AddListener(() => OnButtonInputClick(x));
                }
                else
                {
                    if (buttons[i].obj.activeSelf) buttons[i].obj.SetActive(false);
                    buttons[i].btn.onClick.RemoveAllListeners();
                }
        }

        void ZShowButtonsForInput(int num, OnInputCallbackDel callback = null, string[] captions = null)
        {
            SetButton(true);
            OnInputCallback += callback;
            for (int i = 0; i < buttons.Length; i++)
                if (i < num)
                {
                    if (!buttons[i].obj.activeSelf) buttons[i].obj.SetActive(true);
                    if (captions != null && i < captions.Length)
                        buttons[i].ztext.ID = captions[i];
                    else
                        buttons[i].ztext.ID = defaultBtnID[i];
                    buttons[i].btn.onClick.RemoveAllListeners();
                    int x = i;
                    buttons[i].btn.onClick.AddListener(() => OnButtonInputClick(x));
                }
                else
                {
                    if (buttons[i].obj.activeSelf) buttons[i].obj.SetActive(false);
                    buttons[i].btn.onClick.RemoveAllListeners();
                }
        }

        public void Alert(string showText, bool OKBtn = false, float duration = 0.0f)
        {
            RemoveAllListeners();
            ShowBasic(showText, duration);
            SetLoading(false);
            SetInput(false);

            if (OKBtn)
                ShowButton(0, (int x) => Hide(duration));
            else
            {
                ClickBGHide(duration);
                SetButton(false);
            }    
        }

        public void ZAlert(string ID, bool OKBtn = false, float duration = 0.0f)
        {
            RemoveAllListeners();
            ZShowBasic(ID, duration);
            SetLoading(false);
            SetInput(false);

            if (OKBtn)
                ZShowButton(0, (int x) => Hide(duration));
            else
            {
                ClickBGHide(duration);
                SetButton(false);
            }
        }

        public void Loading(string showText, bool CancelBtn = false, float duration = 0.0f, OnCallbackDel CancelCallback = null)
        {
            RemoveAllListeners();
            ShowBasic(showText, duration);
            SetLoading(true);
            SetInput(false);

            if (CancelBtn)
                ShowButton(1, CancelCallback);
            else
                SetButton(false);
        }

        public void ZLoading(string ID, bool CancelBtn = false, float duration = 0.0f, OnCallbackDel CancelCallback = null)
        {
            RemoveAllListeners();
            ZShowBasic(ID, duration);
            SetLoading(true);
            SetInput(false);

            if (CancelBtn)
                ZShowButton(1, CancelCallback);
            else
                SetButton(false);
        }

        public void Show(int numOfButtons, string showText = "", float duration = 0.0f, OnCallbackDel Callback = null, string[] captions = null)
        {
            RemoveAllListeners();
            ShowBasic(showText, duration);
            SetLoading(false);
            SetInput(false);

            if(numOfButtons == 0)
            {
                ClickBGHide(duration);
                SetButton(false);
            }
            else
            {
                ShowButtons(numOfButtons, Callback, captions);
            }
        }

        public void ZShow(int numOfButtons, string ID = "", float duration = 0.0f, OnCallbackDel Callback = null, string[] captions = null)
        {
            RemoveAllListeners();
            ZShowBasic(ID, duration);
            SetLoading(false);
            SetInput(false);

            if (numOfButtons == 0)
            {
                ClickBGHide(duration);
                SetButton(false);
            }
            else
            {
                ZShowButtons(numOfButtons, Callback, captions);
            }
        }

        public void InputBox(string showText, string placeholder = "", float duration = 0.0f, OnInputCallbackDel Callback = null, string[] captions = null)
        {
            RemoveAllListeners();
            ShowBasic(showText, duration);
            SetLoading(false);
            SetInput(true, placeholder);
            

            ShowButtonsForInput(2, Callback, captions);
        }

        public void ZInputBox(string ID, string placeholderID = "", float duration = 0.0f, OnInputCallbackDel Callback = null, string[] captions = null)
        {
            RemoveAllListeners();
            ZShowBasic(ID, duration);
            SetLoading(false);
            ZSetInput(true, placeholderID);

            ZShowButtonsForInput(2, Callback, captions);
        }

        
        public void Hide(float duration = 0.0f)
        {
            if (background.gameObject.activeSelf) background.gameObject.SetActive(false);
            if(msgBox.gameObject.activeSelf) StartCoroutine(PopupFadeOut(duration, true));
        }


        public void OnBackgroundClick()
        {
            Debug.Log("OnBackgroundClick");
            if(OnBackgroundClicked != null) OnBackgroundClicked.Invoke();
        }

        public void OnButtonClick(int x)
        {
            Debug.Log("OnButtonClick(" + x + ")");
            if (OnCallback != null) OnCallback.Invoke(x);
        }

        public void OnButtonInputClick(int x)
        {
            Debug.Log("OnButtonInputClick(" + x + ")");
            if (OnInputCallback != null) OnInputCallback.Invoke(x, inputText.text);
        }

        IEnumerator PopupFadeOut(float duration, bool hideall = false)
        {
            if (!msgBox.gameObject.activeSelf) yield break;
            yield return StartCoroutine(PopupFade(1, 0, duration));
            if (msgBox.gameObject.activeSelf) msgBox.gameObject.SetActive(false);
            if (hideall && background.gameObject.activeSelf) background.gameObject.SetActive(false);
            if (OnPopupHidden != null) OnPopupHidden.Invoke();
        }

        IEnumerator PopupFadeIn(float duration)
        {
            yield return StartCoroutine(PopupFade(0, 1, duration));
            if (OnPopupShown != null) OnPopupShown.Invoke();
            Canvas.ForceUpdateCanvases();
        }

        IEnumerator PopupFade(float ori, float dest, float duration)
        {
            if (!msgBox.gameObject.activeSelf || !background.gameObject.activeSelf) yield break;
            msgBox.alpha = background.alpha = ori;
            
            float timer = 0.0f;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                msgBox.alpha = background.alpha = ori + (dest - ori) * Mathf.Min(timer / duration, 1);
                yield return null;
            }
            msgBox.alpha = background.alpha = dest;
        }

#if UNITY_EDITOR
        void Reset()
        {
            RectTransform rect = GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.pivot = Vector2.one * 0.5f;
            rect.sizeDelta = Vector2.zero;
            buttons = new AButton[0];
        }
#endif

    }
}
