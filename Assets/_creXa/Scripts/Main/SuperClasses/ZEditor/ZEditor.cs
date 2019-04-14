#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using creXa.GameBase.Graphics;

namespace creXa.GameBase
{
    public abstract class ZEditor<T> : Editor where T : MonoBehaviour
    {
        protected T z;
        protected SerializedObject sObj;
        protected Dictionary<string, SerializedProperty> sProp;
        protected T zGet(int i)
        {
            if (targets == null || i < 0 || i >= targets.Length) return null;
            return (T)targets[i];
        }

        protected string[] defaultViewGUIC =
            new string[]
            {
                "Tailor",
                "Data",
                "Hide"  
            };
        protected int defaultView = 0;
        
        void OnEnable()
        {
            z = (T) target;
            if (z == null) return;
            defaultView = GetInt("defaultView");
            LinkSerializedProperties();
            ComponentCheckAll();
            OnEnableRun();
        }
        protected virtual void OnEnableRun() { }

        #region PlayerPref

        protected virtual float[] GetFloatArray(string field, float def = 0.0f, int defSize= 1)
        {
            return PlayerPrefsX.GetFloatArray(z.GetInstanceID() + "_" + field, def, defSize);
        }

        protected virtual void SetFloatArray(string field, float[] value)
        {
            PlayerPrefsX.SetFloatArray(z.GetInstanceID() + "_" + field, value);
        }

        protected virtual string[] GetStringArray(string field, string def = "", int defSize = 1)
        {
            return PlayerPrefsX.GetStringArray(z.GetInstanceID() + "_" + field, def, defSize);
        }

        protected virtual void SetStringArray(string field, string[] value)
        {
            PlayerPrefsX.SetStringArray(z.GetInstanceID() + "_" + field, value);
        }

        protected virtual int[] GetIntArray(string field, int def = 0, int defSize = 1)
        {
            return PlayerPrefsX.GetIntArray(z.GetInstanceID() + "_" + field, def, defSize);
        }

        protected virtual void SetIntArray(string field, int[] value)
        {
            PlayerPrefsX.SetIntArray(z.GetInstanceID() + "_" + field, value);
        }

        protected virtual bool[] GetBoolArray(string field, bool def = false, int defSize = 1)
        {
            return PlayerPrefsX.GetBoolArray(field, def, defSize);
        }

        protected virtual void SetBoolArray(string field, bool[] value)
        {
            PlayerPrefsX.SetBoolArray(field, value);
        }

        protected virtual float GetFloat(string field, float def = 0.0f)
        {
            return PlayerPrefs.GetFloat(z.GetInstanceID() + "_" + field, def);
        }

        protected virtual void SetFloat(string field, float value)
        {
            PlayerPrefs.SetFloat(z.GetInstanceID() + "_" + field, value);
        }

        protected virtual string GetString(string field, string def = "")
        {
            return PlayerPrefs.GetString(z.GetInstanceID() + "_" + field, def);
        }

        protected virtual void SetString(string field, string value)
        {
            PlayerPrefs.SetString(z.GetInstanceID() + "_" + field, value);
        }

        protected virtual int GetInt(string field, int def = 0)
        {
            return PlayerPrefs.GetInt(z.GetInstanceID() + "_" + field, def);
        }

        protected virtual void SetInt(string field, int value)
        {
            PlayerPrefs.SetInt(z.GetInstanceID() + "_" + field, value);
        }

        protected virtual bool GetBool(string field, bool def = false)
        {
            return PlayerPrefsX.GetBool(field, def);
        }

        protected virtual void SetBool(string field, bool value)
        {
            PlayerPrefsX.SetBool(field, value);
        }

        #endregion

        protected void ComponentCheck<M, N>(bool f) where M : Component where N : Component
        {
            bool dirty = false;
            if (f && !z.gameObject.GetComponent<M>()) z.gameObject.AddComponent<M>();
            if (!f && z.gameObject.GetComponent<M>()) { DestroyImmediate(z.gameObject.GetComponent<M>()); dirty = true; }
            if (f && !z.gameObject.GetComponent<N>()) z.gameObject.AddComponent<N>();
            if (!f && z.gameObject.GetComponent<N>()) { DestroyImmediate(z.gameObject.GetComponent<N>()); dirty = true; }
            if (dirty) EditorGUIUtility.ExitGUI();
        }
        protected void ComponentCheck<N>(bool f) where N : Component
        {
            if (f && !z.gameObject.GetComponent<N>()) z.gameObject.AddComponent<N>();
            if (!f && z.gameObject.GetComponent<N>()) { DestroyImmediate(z.gameObject.GetComponent<N>()); EditorGUIUtility.ExitGUI(); }
        }
        protected virtual void ComponentCheckAll() { }

        protected void LinkSerializedProperties()
        {
            sObj = serializedObject;
            sProp = new Dictionary<string, SerializedProperty>();

            SerializedProperty it = serializedObject.GetIterator();
            while (it.NextVisible(true))
            { // or NextVisible, also, the bool argument specifies whether to enter on children or not
                sProp.Add(it.propertyPath, sObj.FindProperty(it.propertyPath));
            }
        }
        

        public override void OnInspectorGUI()
        {
            DrawDefaultButton();
            sObj.Update();
            GUILayout.BeginVertical("Box", BestWidth());

            Undo.RecordObject(z, z.name + " Modified on Inspector");
            if (defaultView == 2) {
                DrawEndLine();
                return;
            }
            if (defaultView == 1) {
                if (DrawDefaultInspector())
                    ComponentCheckAll();
                DrawEndLine();
                return;
            }
            GUI.enabled = false;
            EditorGUILayout.PropertyField(sProp["m_Script"]);
            GUI.enabled = true;
            OnInspectorGUIRun();
            sObj.ApplyModifiedProperties();
            DrawEndLine();

            
        }
        protected virtual void OnInspectorGUIRun() { }

        public void OnSceneGUI()
        {
            Undo.RecordObject(z, z.name + " Modified on Scene");
            OnSceneGUIRun();
        }
        protected virtual void OnSceneGUIRun() { }

        protected void DrawSeparate(float space = 3)
        {
            GUILayout.Box("", GUILayout.Width(GetSuitableWidth() + 8), GUILayout.Height(3));
            GUILayout.Space(space);
        }

        protected void DrawEndLine()
        {
            GUILayout.Space(1);
            DrawSeparate(1);
            GUILayout.EndVertical();
        }

        protected void DrawDefaultButton()
        {
            GUILayout.BeginHorizontal("Box", BestWidth());
            EditorGUILayout.PrefixLabel("GUID: " + z.GetInstanceID());
            EditorGUI.BeginChangeCheck();
            defaultView = GUILayout.Toolbar(defaultView, defaultViewGUIC, BestCellWidth());
            if (EditorGUI.EndChangeCheck())
                SetInt("defaultView", defaultView);
            GUILayout.EndHorizontal();
        }

        public float GetSuitableWidth(float x = -40)
        {
            return EditorGUIUtility.currentViewWidth + x;
        }

        public GUILayoutOption BestWidth(float x = 0)
        {
            return GUILayout.Width(GetSuitableWidth() + x);
        }

        public GUILayoutOption BestCellWidth(float x = 0)
        {
            return BestWidth(-EditorGUIUtility.labelWidth + 0);
        }

        public GUILayoutOption BestLabelWidth(float x = 0)
        {
            return GUILayout.Width(EditorGUIUtility.labelWidth + x);
        }

        public GUILayoutOption BestFieldWidth(float x = 0)
        {
            return GUILayout.Width(EditorGUIUtility.fieldWidth + x);
        }

        public bool PropertyExists(string key)
        {
            return sProp.ContainsKey(key);
        }

        public static void SetFullScreen(GameObject obj, bool horizontal = true, bool vertical = true)
        {
            SetFullScreen(obj.GetComponent<RectTransform>(), horizontal, vertical);
        }

        public static void SetFullScreen(RectTransform rect, bool horizontal = true, bool vertical = true)
        {
            rect.anchorMin = new Vector2(horizontal ? 0 : 0.5f, vertical ? 0 : 0.5f);
            rect.anchorMax = new Vector2(horizontal ? 1 : 0.5f, vertical ? 1 : 0.5f);
            rect.pivot = Vector2.one * 0.5f;
            rect.sizeDelta = Vector2.zero;
            rect.localPosition = Vector2.zero;
        }

        protected static GameObject CreateShape<U>(string name, Transform parent, bool sizefit = false) where U : ZShape
        {
            GameObject rtn = AddUIGameObject(name, parent, sizefit);
            rtn.AddComponent<U>();
            return rtn;
        }

        protected static GameObject AddUIGameObject(string name, Transform parent, bool sizefit = false)
        {
            RectTransform rect;
            return AddUIGameObject(name, parent, out rect, sizefit);
        }

        protected static GameObject AddUIGameObject(string name, Transform parent, out RectTransform rect, bool sizefit = false)
        {
            GameObject rtn = new GameObject(name);
            rtn.transform.SetParent(parent);
            rtn.transform.localScale = Vector3.one;
            rtn.transform.localPosition = Vector3.zero;
            rect = rtn.AddComponent<RectTransform>();
            if (sizefit)
            {
                ContentSizeFitter csf = rtn.AddComponent<ContentSizeFitter>();
                csf.horizontalFit = ContentSizeFitter.FitMode.MinSize;
                csf.verticalFit = ContentSizeFitter.FitMode.MinSize;
            }

            return rtn;
        }

        protected static void AddZSRectangle(GameObject o, int roundRadius, int borderWidth)
        {
            Graphics.ZSRectangle ZSRect = o.AddComponent<Graphics.ZSRectangle>();
            ZSRect.SetAllRoundRadius(roundRadius);
            ZSRect.borderWidth = borderWidth;
            ZSRect.border = borderWidth != 0;
        }

        protected static void AddZThemeObj(GameObject o, int variation)
        {
            ZThemeObj t = o.AddComponent<ZThemeObj>();
            t.Variation = variation;
        }

        protected static void AddZThemeObjZS(GameObject o, int variation)
        {
            Graphics.ZThemeObjZS t = o.AddComponent<Graphics.ZThemeObjZS>();
            t.Variation = variation;
        }

        protected static Text AddText(GameObject o, string def = "", float w = 600, float h = 60, int fontSize = 45)
        {
            Text rtn = o.AddComponent<Text>();
            RectTransform rectP = o.GetComponent<RectTransform>();
            if (rectP)
                rectP.sizeDelta = new Vector2(w, h);

            rtn.fontSize = fontSize;
            rtn.alignment = TextAnchor.MiddleCenter;
            rtn.text = def;
            return rtn;
        }

        protected static InputField AddInputField(GameObject o, Text text)
        {
            InputField rtn = o.AddComponent<InputField>();
            text.supportRichText = false;
            rtn.textComponent = text;
            return rtn;
        }

        protected static Scrollbar AddScrollBar(string mode)
        {
            if (mode != "Horizontal" && mode != "Vertical") return null;
            GameObject tmp = new GameObject(mode + " Scrollbar");
            RectTransform rect = tmp.AddComponent<RectTransform>();
            Image img = tmp.AddComponent<Image>();
            Scrollbar rtn = tmp.AddComponent<Scrollbar>();
            ZThemeObj tmpTheme = tmp.AddComponent<ZThemeObj>();
            tmpTheme.Variation = 17;

            img.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
            img.type = Image.Type.Sliced;
            img.fillCenter = true;
            rect.sizeDelta = mode == "Horizontal" ? new Vector2(160, 20) : new Vector2(20, 160);
            rect.localPosition = Vector3.zero;

            GameObject slideArea = new GameObject("Sliding Area");
            RectTransform saRect = slideArea.AddComponent<RectTransform>();
            saRect.transform.SetParent(rect.transform);
            saRect.anchorMin = Vector2.zero;
            saRect.anchorMax = Vector2.one;
            saRect.transform.localScale = Vector3.one;
            saRect.offsetMin = new Vector2(10, 10);
            saRect.offsetMax = new Vector2(-10, -10);

            GameObject handle = new GameObject("Handle");
            RectTransform hRect = handle.AddComponent<RectTransform>();
            hRect.transform.SetParent(slideArea.transform);
            hRect.offsetMin = new Vector2(-10, -10);
            hRect.offsetMax = new Vector2(10, 10);
            Image hImg = handle.AddComponent<Image>();
            hImg.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
            hImg.type = Image.Type.Sliced;
            hImg.fillCenter = true;
            ZThemeObj handleTheme = handle.AddComponent<ZThemeObj>();
            handleTheme.Variation = 18;

            rtn.handleRect = hRect;
            rtn.targetGraphic = hImg;
            rtn.direction = mode == "Horizontal" ? Scrollbar.Direction.LeftToRight : Scrollbar.Direction.TopToBottom;

            return rtn;
        }

        protected static GameObject CreatePopup(Transform parent)
        {
            GameObject obj = AddUIGameObject("Popup", parent);
            obj.AddComponent<ZPopup>();
            return obj;
        }

        protected static GameObject CreateSlideSelector(Transform parent)
        {
            GameObject obj = AddUIGameObject("SlideSelector", parent);
            obj.AddComponent<ZSlideSelector>();
            return obj;
        }

        protected static GameObject CreateRecordList(Transform parent)
        {
            GameObject obj = AddUIGameObject("RecordList", parent);
            obj.AddComponent<ZRecordList>();
            return obj;
        }

        protected static GameObject CreateColorPicker(Transform parent, int type)
        {
            GameObject obj = AddUIGameObject("ColorPicker", parent);
            ZColorPicker cp = obj.AddComponent<ZColorPicker>();      
            cp.colorPanel = obj.AddComponent<Image>();
            cp.colorPanel.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_creXa/Textures/Colorpanel/panel" + type + ".png");
            cp.colorPanel.SetNativeSize();

            cp.picker = AddUIGameObject("Picker", obj.transform);
            Image img = cp.picker.AddComponent<Image>();
            img.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_creXa/Textures/picker.png");
            img.SetNativeSize();

            return obj;
        }

        protected static GameObject CreateLoading(Transform parent)
        {
            GameObject obj = AddUIGameObject("Loading", parent);
            Image img = obj.AddComponent<Image>();
            img.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_creXa/Textures/LoadingB.png");
            AddZThemeObj(obj, 8);

            GameObject turn = AddUIGameObject("Turning", obj.transform);
            Image turnImg = turn.AddComponent<Image>();
            turnImg.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_creXa/Textures/LoadingC.png");
            AddZThemeObj(turn, 7);

            RotationVaries2D rot = turn.AddComponent<RotationVaries2D>();
            rot.Z = new NVariesCtrl();
            rot.Z.Ctrl = true;
            rot.Z.Value = new NormalizedVaries();
            rot.Z.Value.Min = 0;
            rot.Z.Value.Max = 360;
            rot.Z.Varies = new AnimationCurve();
            rot.Z.Varies.AddKey(0, 0);
            rot.Z.Varies.AddKey(1, 1);
            rot.Z.Varies.postWrapMode = WrapMode.Loop;

            SetFullScreen(turn);
            return obj;
        }

        protected static ZText AddZText(Text text, string ID = "")
        {
            ZText rtn = text.gameObject.AddComponent<ZText>();
            rtn.loadInBank = ID != "";
            rtn.ID = ID;
            return rtn;
        }

        protected static GameObject CreateMultiGrahpicButton(Transform parent, bool popup = false, string caption = "", float w = 300, float h = 100, int rr = 25)
        {
            RectTransform objRect;
            GameObject obj = AddUIGameObject("MButton", parent, out objRect);
            objRect.sizeDelta = new Vector2(w, h);

            ZMultiGrahpicButton btn = obj.AddComponent<ZMultiGrahpicButton>();

            Button button;
            Text text;
            GameObject b = CreateButton(obj.transform, out button, out text, popup, caption, w, h, rr);
            btn.targetGraphic = button.GetComponent<Graphic>();
            SetFullScreen(b);
            return obj;
        }

        protected static GameObject CreateButton(Transform parent, bool popup = false, string caption = "", float w = 300, float h = 100, int rr = 25)
        {
            Button btn;
            Text text;
            return CreateButton(parent, out btn, out text, popup, caption, w, h, rr);
        }

        protected static GameObject CreateButton(Transform parent, out Button button, out Text text, bool popup = false, string caption = "", float w = 300, float h = 100, int rr = 25)
        {
            RectTransform objRect;
            GameObject obj = AddUIGameObject("Button", parent, out objRect);

            objRect.sizeDelta = new Vector2(w, h);

            AddZSRectangle(obj, rr, 4);
            AddZThemeObjZS(obj, popup ? 12 : 2);
            button = obj.AddComponent<Button>();

            ///
            GameObject captionObj = AddUIGameObject("Caption", obj.transform);
            text = AddText(captionObj, caption, w, h);
            AddZThemeObj(captionObj, popup ? 13 : 3);

            SetFullScreen(captionObj);
            return obj;
        }

        protected static GameObject CreateInputBox(Transform parent, bool popup = false, string caption = "", float w = 600, float h = 100, int rr = 25)
        {
            Text inputText, placeholderText;
            return CreateInputBox(parent, out inputText, out placeholderText, popup, caption, w, h, rr);
        }

        protected static GameObject CreateInputBox(Transform parent, out Text inputText, out Text placeholderText, bool popup = false, string caption = "", float w = 600, float h = 100, int rr = 25)
        {
            RectTransform objRect;
            GameObject obj = AddUIGameObject("InputBox", parent, out objRect);
            objRect.sizeDelta = new Vector2(w, h);

            AddZSRectangle(obj, rr, 4);
            AddZThemeObjZS(obj, popup ? 14 : 4);

            ///
            RectTransform rect;
            GameObject input = AddUIGameObject("Input", obj.transform, out rect);
            inputText = AddText(input);
            inputText.text = caption;
            InputField inputF = AddInputField(input, inputText);
            AddZThemeObj(input, popup ? 15 : 5);

            ///

            GameObject placeholder = AddUIGameObject("Placeholder", input.transform);
            placeholderText = AddText(placeholder, "(PlaceHolder)");
            AddZThemeObj(placeholder, popup ? 16 : 6);

            inputF.placeholder = placeholderText;

            SetFullScreen(input);
            SetFullScreen(placeholder);

            return obj;
        }

        protected static GameObject CreateSwitch(Transform parent)
        {
            GameObject obj = AddUIGameObject("Switch", parent);
            RectTransform rect = obj.GetComponent<RectTransform>();
            ZSwitch sw = obj.AddComponent<ZSwitch>();

            GameObject bg = AddUIGameObject("BG", obj.transform);
            sw.bgAni = bg.AddComponent<ZAnimated2D>();
            sw.bg = bg.AddComponent<ZSRectangle>();
            RectTransform bgrect = bg.GetComponent<RectTransform>();

            GameObject circle = AddUIGameObject("Circle", obj.transform);
            sw.btnAni = circle.AddComponent<ZAnimated2D>();
            sw.btn = circle.AddComponent<ZSCircle>();
            RectTransform cirrect = circle.GetComponent<RectTransform>();

            rect.sizeDelta = new Vector2(200, 100);
            bgrect.sizeDelta = new Vector2(200, 100);
            cirrect.sizeDelta = new Vector3(90, 90);

            sw.bg.border = false;
            sw.bg.SetAllRoundRadius(100);
            sw.bg.color = sw.Value ? sw.onColor : sw.offColor;
            sw.btn.border = false;
            sw.btnAni.SetPos(new Vector3((sw.Value ? 1 : 1) * 50, 0, 0));


            return obj;
        }

        protected static GameObject CreateRadioButton(Transform parent, Button linkButton = null)
        {
            GameObject obj = AddUIGameObject("Radio", parent);
            RectTransform rect = obj.GetComponent<RectTransform>();
            ZSelectable sel = obj.AddComponent<ZSelectable>();

            rect.sizeDelta = new Vector2(100, 100);

            ZSCircle zsRect = obj.AddComponent<ZSCircle>();
            zsRect.color = new Color(0.117f, 0.117f, 0.117f, 1);
            zsRect.borderColor = new Color(0.235f, 0.235f, 0.235f, 1);
            zsRect.borderWidth = 8;
            zsRect.border = true;
            AddZThemeObjZS(obj, 20);

            GameObject img = AddUIGameObject("Caption", obj.transform);
            RectTransform imgRect = img.GetComponent<RectTransform>();

            ZSCircle imgzsRect = img.AddComponent<ZSCircle>();
            imgzsRect.color = new Color(0.117f, 0.117f, 0.117f, 1);
            imgzsRect.border = false;
            AddZThemeObjZS(img, 20);

            if (linkButton == null)
            {
                sel.btn = obj.AddComponent<Button>();
            }
            else
            {
                sel.btn = linkButton;
            }

            sel.btnTheme = obj.GetComponent<ZThemeObj>();
            sel.imgTheme = img.GetComponent<ZThemeObj>();
            sel.zshape = zsRect;
            sel.zsGrahpic = imgzsRect;

            sel.useTheme = true;
            sel.unselectedCapColor = new Color(0.235f, 0.235f, 0.235f, 1);
            sel.unselectedBtnColor = new Color(0.117f, 0.117f, 0.117f, 1);
            sel.unselectedBtnBorder = new Color(0.235f, 0.235f, 0.235f, 1);
            sel.selectedBtnColor = new Color(0.235f, 0.235f, 0.235f, 1);
            sel.selectedBtnBorder = new Color(0.235f, 0.235f, 0.235f, 1);
            sel.selectedCapColor = new Color(0.6f, 1, 0.8f, 1);
            sel.unselectedImgTheme = 20;

            SetFullScreen(imgRect);

            return obj;
        }

        protected static GameObject CreateCheckBox(Transform parent, Button linkButton = null)
        {
            GameObject obj = AddUIGameObject("CheckBox", parent);
            RectTransform rect = obj.GetComponent<RectTransform>();
            ZSelectable sel = obj.AddComponent<ZSelectable>();

            rect.sizeDelta = new Vector2(100, 100);

            ZSRectangle zsRect = obj.AddComponent<ZSRectangle>();
            zsRect.color = new Color(0.117f, 0.117f, 0.117f, 1);
            zsRect.borderColor = new Color(0.235f, 0.235f, 0.235f, 1);
            zsRect.borderWidth = 8;
            zsRect.border = true;
            zsRect.SetAllRoundRadius(25);
            AddZThemeObjZS(obj, 20);

            GameObject img = AddUIGameObject("Caption", obj.transform);
            RectTransform imgRect = img.GetComponent<RectTransform>();

            ZSRectangle imgzsRect = img.AddComponent<ZSRectangle>();
            imgzsRect.color = new Color(0.117f, 0.117f, 0.117f, 1);
            imgzsRect.border = false;
            imgzsRect.SetAllRoundRadius(25);
            AddZThemeObjZS(img, 20);

            if (linkButton == null)
            {
                sel.btn = obj.AddComponent<Button>();
            }
            else
            {
                sel.btn = linkButton;
            }

            sel.btnTheme = obj.GetComponent<ZThemeObj>();
            sel.imgTheme = img.GetComponent<ZThemeObj>();
            sel.zshape = zsRect;
            sel.zsGrahpic = imgzsRect;

            sel.useTheme = true;
            sel.unselectedCapColor = new Color(0.235f, 0.235f, 0.235f, 1);
            sel.unselectedBtnColor = new Color(0.117f, 0.117f, 0.117f, 1);
            sel.unselectedBtnBorder = new Color(0.235f, 0.235f, 0.235f, 1);
            sel.selectedBtnColor = new Color(0.235f, 0.235f, 0.235f, 1);
            sel.selectedBtnBorder = new Color(0.235f, 0.235f, 0.235f, 1);
            sel.selectedCapColor = new Color(0.6f, 1, 0.8f, 1);
            sel.unselectedImgTheme = 20;

            SetFullScreen(imgRect);

            return obj;
        }

        protected static GameObject CreateSelectable<U>(Transform parent, Button linkButton = null) where U :Graphic
        {
            GameObject obj = AddUIGameObject("Selectable", parent);
            RectTransform rect = obj.GetComponent<RectTransform>();
            ZSelectable sel = obj.AddComponent<ZSelectable>();

            rect.sizeDelta = new Vector2(100, 100);

            ZSRectangle zsRect = obj.AddComponent<ZSRectangle>();
            zsRect.color = new Color(0.117f, 0.117f, 0.117f, 1);
            zsRect.borderColor = new Color(0.235f, 0.235f, 0.235f, 1);
            zsRect.borderWidth = 4;
            zsRect.border = true;
            zsRect.SetAllRoundRadius(25);
            AddZThemeObjZS(obj, 20);

            GameObject img = AddUIGameObject("Caption", obj.transform);
            RectTransform imgRect = img.GetComponent<RectTransform>();
            U text = img.AddComponent<U>();
            if( text is Text)
            {
                Text tmpText = (Text) (Graphic) text;
                tmpText.resizeTextForBestFit = true;
                tmpText.resizeTextMinSize = 30;
                tmpText.fontSize = 45;
                tmpText.alignment = TextAnchor.MiddleCenter;
            }
            AddZThemeObj(img, 19);

            if(linkButton == null)
            {
                sel.btn =  obj.AddComponent<Button>();
            }
            else
            {
                sel.btn = linkButton;
            }

            sel.btnTheme = obj.GetComponent<ZThemeObj>();
            sel.imgTheme = img.GetComponent<ZThemeObj>();
            sel.zshape = zsRect;
            sel.zsGrahpic = text;

            sel.useTheme = true;
            sel.unselectedCapColor = new Color(0.235f, 0.235f, 0.235f, 1);
            sel.unselectedBtnColor = new Color(0.117f, 0.117f, 0.117f, 1);
            sel.unselectedBtnBorder = new Color(0.235f, 0.235f, 0.235f, 1);
            sel.selectedBtnColor = new Color(0.235f, 0.235f, 0.235f, 1);
            sel.selectedBtnBorder = new Color(0.235f, 0.235f, 0.235f, 1);
            sel.selectedCapColor = new Color(0.6f, 1, 0.8f, 1);

            SetFullScreen(imgRect);

            return obj;
        }

        protected static GameObject CreateLayout(string mode, Transform parent, int spacing = 0, TextAnchor anchor = TextAnchor.MiddleCenter, bool sizefit = false)
        {
            GameObject rtn = AddUIGameObject(mode, parent, sizefit);

            if(mode == "V")
            {
                VerticalLayoutGroup v = rtn.AddComponent<VerticalLayoutGroup>();
                v.childAlignment = anchor;
                v.spacing = spacing;
            }
            else
            {
                HorizontalLayoutGroup h = rtn.AddComponent<HorizontalLayoutGroup>();
                h.childAlignment = anchor;
                h.spacing = spacing;
            }

            ContentSizeFitter csf = rtn.AddComponent<ContentSizeFitter>();
            csf.horizontalFit = ContentSizeFitter.FitMode.MinSize;
            csf.verticalFit = ContentSizeFitter.FitMode.MinSize;

            return rtn;
        }

        #region Scroll

        protected static GameObject CreateScroll(Transform parent)
        {
            GameObject obj = AddUIGameObject("ScrollRect", parent);
            ScrollRect x = obj.AddComponent<ScrollRect>();

            GameObject viewport = AddUIGameObject("Viewport", obj.transform);
            if(!viewport.GetComponent<RectTransform>())
                viewport.AddComponent<RectTransform>();
            viewport.AddComponent<Image>();
            Mask mask = viewport.AddComponent<Mask>();
            mask.showMaskGraphic = false;

            x.viewport = viewport.GetComponent<RectTransform>();
            x.viewport.offsetMin = Vector2.zero;
            x.viewport.offsetMax = Vector2.zero;
            x.viewport.anchorMin = Vector2.zero;
            x.viewport.anchorMax = Vector2.one;

            GameObject content = AddUIGameObject("Content", viewport.transform);
            ContentSizeFitter csf = content.AddComponent<ContentSizeFitter>();
            csf.horizontalFit = ContentSizeFitter.FitMode.MinSize;
            csf.verticalFit = ContentSizeFitter.FitMode.MinSize;
            x.content = content.GetComponent<RectTransform>();
            return obj;
        }

        protected static void AddScrollBarHorizontal(GameObject o)
        {
            ScrollRect x = o.GetComponent<ScrollRect>();
            if (!x) return;
            x.horizontalScrollbar = AddScrollBar("Horizontal");
            x.horizontalScrollbar.transform.SetParent(x.transform);
            x.horizontalScrollbar.transform.localScale = Vector3.one;
            x.horizontalScrollbar.transform.localPosition = Vector3.zero;
        }

        protected static void AddScrollBarVertical(GameObject o)
        {
            ScrollRect x = o.GetComponent<ScrollRect>();
            if (!x) return;
            x.verticalScrollbar = AddScrollBar("Vertical");
            x.verticalScrollbar.transform.SetParent(x.transform);
            x.verticalScrollbar.transform.localScale = Vector3.one;
            x.verticalScrollbar.transform.localPosition = Vector3.zero;
        }

        #endregion

    }
}

#endif
