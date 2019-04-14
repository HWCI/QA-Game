#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Linq;

namespace creXa.GameBase
{
    [CustomEditor(typeof(ZPopup))]
    public class _Editor_ZPopup : ZEditor<ZPopup>
    {
        protected override void OnInspectorGUIRun()
        {
            base.OnInspectorGUIRun();

            if(z.background == null)   
            {
                if (GUILayout.Button("Create Hierarchy"))
                {
                    CreateHierarchy();
                }

                DrawSeparate();
                EditorGUILayout.Space();
            }
            else
            {
                if (GUILayout.Button(z.background.gameObject.activeSelf? "Invisible" : "Visible"))
                {
                    z.background.gameObject.SetActive(!z.background.gameObject.activeSelf);
                    z.msgBox.gameObject.SetActive(!z.msgBox.gameObject.activeSelf);
                }
            }

            EditorGUILayout.PropertyField(sProp["background"]);
            EditorGUILayout.PropertyField(sProp["msgBox"]);
            DrawSeparate();

            EditorGUILayout.PropertyField(sProp["msgObj"]);
            EditorGUILayout.PropertyField(sProp["loadingObj"]);
            EditorGUILayout.PropertyField(sProp["inputObj"]);
            EditorGUILayout.PropertyField(sProp["buttonObj"]);
            DrawSeparate();

            EditorGUILayout.PropertyField(sProp["msgText"]);
            EditorGUILayout.PropertyField(sProp["inputText"]);
            EditorGUILayout.PropertyField(sProp["placeholderText"]);
            DrawSeparate();

            EditorGUILayout.PropertyField(sProp["msgZText"]);
            EditorGUILayout.PropertyField(sProp["placeholderZText"]);
            DrawSeparate();

            EditorGUILayout.PropertyField(sProp["defaultBtnCaption"], true);
            EditorGUILayout.PropertyField(sProp["defaultBtnID"], true);
            DrawSeparate();

            if(GUILayout.Button("Create Button"))
            {
                AddButton("Default");
            }
            if(z && z.buttons != null)
                EditorGUILayout.PropertyField(sProp["buttons"], true);
                    
            if (GUILayout.Button("Remove Button"))
            {
                RemoveButton();
            }
        }

        void AddButton(string caption, string ID = "")
        {
            List<ZPopup.AButton> tmpList = z.buttons != null ? z.buttons.ToList() : new List<ZPopup.AButton>();

            ZPopup.AButton tmp = new ZPopup.AButton();

            tmp.obj = CreateButton(z.buttonObj.transform, out tmp.btn, out tmp.caption, true, caption);
            tmp.caption.text = caption;
            tmp.ztext = AddZText(tmp.caption, ID);

            tmpList.Add(tmp);

            z.buttons = tmpList.ToArray();

        }

        void RemoveButton()
        {
            List<ZPopup.AButton> tmpList = z.buttons != null ? z.buttons.ToList() : new List<ZPopup.AButton>();
            DestroyImmediate(z.buttons[z.buttons.Length - 1].obj);
            if (tmpList.Count > 0)
                tmpList.RemoveAt(tmpList.Count - 1);

            z.buttons = tmpList.ToArray();
        }

        void CreateHierarchy()
        {
            z.buttons = null;
            GameObject BG = AddUIGameObject("BG", z.transform);
            BG.AddComponent<Image>();
            Button BGbtn = BG.AddComponent<Button>();
            BGbtn.transition = Selectable.Transition.None;
            BGbtn.onClick.AddListener(z.OnBackgroundClick);
            AddZThemeObj(BG, 9);
            z.background = BG.AddComponent<CanvasGroup>();

            GameObject msgBox = AddUIGameObject("MsgBox", z.transform, true);
            AddZSRectangle(msgBox, 80, 0);
            AddZThemeObjZS(msgBox, 10);
            VerticalLayoutGroup msgBoxVLG = msgBox.AddComponent<VerticalLayoutGroup>();
            msgBoxVLG.padding = new RectOffset(50, 50, 50, 50);
            msgBoxVLG.childAlignment = TextAnchor.MiddleCenter;
            msgBoxVLG.spacing = 30;
            z.msgBox = msgBox.AddComponent<CanvasGroup>();

            GameObject v1 = CreateLayout("V", msgBox.transform, 30, TextAnchor.MiddleCenter, true);

            z.msgObj = AddUIGameObject("Caption", v1.transform);
            z.msgText = AddText(z.msgObj, "Caption");
            z.msgZText = AddZText(z.msgText);
            z.msgText.resizeTextForBestFit = true;
            z.msgText.resizeTextMinSize = 30;
            z.msgText.resizeTextMaxSize = 65;
            AddZThemeObj(z.msgObj, 11);

            z.loadingObj = CreateLoading(v1.transform);
            z.inputObj = CreateInputBox(v1.transform, out z.inputText, out z.placeholderText, true);
            z.placeholderZText = AddZText(z.placeholderText, "DEFAULT_PLACEHOLDER");

            GameObject h1 = CreateLayout("H", msgBox.transform, 30, TextAnchor.MiddleCenter, true);

            z.buttonObj = h1;

            AddButton("OK", "DEFAULT_OK");
            AddButton("Cancel", "DEFAULT_CANCEL");

            SetFullScreen(BG);

        }
    }
}

#endif
