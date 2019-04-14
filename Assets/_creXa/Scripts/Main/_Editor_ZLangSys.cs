#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Linq;

namespace creXa.GameBase
{
    [CustomEditor(typeof(ZLangSys))]
    public class _Editor_ZLangSys : ZEditor<ZLangSys> {

        bool defaultFoldOut = true;
        ReorderableList langList;

        protected override void OnEnableRun()
        {
            base.OnEnableRun();
            defaultFoldOut = GetBool("defaultFoldOut");

            SetupReorderableList();
        }

        void SetupReorderableList()
        {
            langList = new ReorderableList(sObj, sProp["Language"], false, true, true, true);
            langList.elementHeight *= 1.0f;

            langList.drawElementCallback = (rect, index, active, focused) =>
            {
                Rect startRect = rect;
                rect.y += startRect.height * 0.05f;
                rect.height = EditorGUIUtility.singleLineHeight * 1.2f;
                if (index > z.Language.Length - 1) return;
                rect.height -= 2;
                float fullWidth = GetSuitableWidth();
                rect.width = 30;
                fullWidth -= rect.width;
                GUI.Label(rect, "#" + (index + 1));

                rect.x += rect.width;
                rect.width = (fullWidth - 60)/2;
                fullWidth -= rect.width;
                z.Language[index].languageBanks = (TextAsset)EditorGUI.ObjectField(rect, z.Language[index].languageBanks, typeof(TextAsset), true);

                rect.x += rect.width;
                fullWidth -= rect.width;
                z.Language[index].defaultFont = (Font)EditorGUI.ObjectField(rect, z.Language[index].defaultFont, typeof(Font), true);

                rect.x += rect.width + 10;
                rect.width = 50;
                fullWidth -= rect.width;
                
                if (index < ZLangSys.LanguageType.Length)
                {
                    GUI.enabled = false;
                    GUI.TextField(rect, ZLangSys.LanguageType[index]);
                    GUI.enabled = true;
                }   
                else
                    z.Language[index].Description = GUI.TextField(rect, z.Language[index].Description);
                
            };

            langList.drawHeaderCallback = rect =>
            {
                Rect startRect = rect;
                rect.y += startRect.height * 0.05f;
                rect.height = EditorGUIUtility.singleLineHeight * 1.2f;
                rect.height -= 2;
                float fullWidth = GetSuitableWidth();
                rect.width = 30;
                fullWidth -= rect.width;

                rect.x += rect.width;
                rect.width = (fullWidth - 60) / 2;
                fullWidth -= rect.width;
                GUI.Label(rect, "TextAsset");

                rect.x += rect.width;
                fullWidth -= rect.width;
                GUI.Label(rect, "Font");

                rect.x += rect.width + 10;
                rect.width = 50;
                fullWidth -= rect.width;
                GUI.Label(rect, "Abbr.");
            };

            langList.onAddCallback = list =>
            {
                List<ZLangSys.LanguageSet> tmp = z.Language != null ? z.Language.ToList() : new List<ZLangSys.LanguageSet>();
                tmp.Add(new ZLangSys.LanguageSet());
                z.Language = tmp.ToArray();

                ResolveLangArray();

            };

            langList.onRemoveCallback = list =>
            {
                ReorderableList.defaultBehaviours.DoRemoveButton(list);
                ResolveLangArray();
            };
        }

        void ResolveLangArray()
        {
            ZText[] allText = FindObjectsOfType<ZText>();
            for (int i = 0; i < allText.Length; i++)
            {
                if (allText[i].Langs == null) continue;
                List<ZText.Lang> tmpa = allText[i].Langs.ToList();
                while (tmpa.Count < z.Language.Length)
                    tmpa.Add(new ZText.Lang());
                while (tmpa.Count > z.Language.Length - 1)
                    tmpa.RemoveAt(tmpa.Count - 1);
                allText[i].Langs = tmpa.ToArray();
            }
        }

        protected override void OnInspectorGUIRun()
        {
            EditorGUILayout.PropertyField(sProp["_AppLanguage"]);
            if (EditorGUIUtility.wideMode) EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(sProp["loadPackageOnStart"]);
            EditorGUILayout.PropertyField(sProp["detectOSLanguage"]);
            if (EditorGUIUtility.wideMode) EditorGUILayout.EndHorizontal();

            DrawSeparate();

            if(GUILayout.Button("Resolve Specified Lang Array"))
            {
                ResolveLangArray();
            }

            if (z.Language != null)
            {
                EditorGUILayout.BeginHorizontal();
                for (int i = 0; i < z.Language.Length; i++)
                {
                    if (GUILayout.Button(i.ToString(), GUILayout.Width(20)))
                    {
                        z.AppLanguage = i;
                    }
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.LabelField("Language Bank", EditorStyles.centeredGreyMiniLabel);
            langList.DoLayoutList();

            DrawSeparate();
            

            //defaults
            EditorGUILayout.BeginVertical("HelpBox");
            if (GUILayout.Button("Defaults", EditorStyles.toolbarButton))
            {
                defaultFoldOut = !defaultFoldOut;
                SetBool("defaultFoldOut", defaultFoldOut);
            }
            if (defaultFoldOut)
            {
                z.defaultLanguage = EditorGUILayout.Popup("Default Language", z.defaultLanguage, ZLangSys.LanguageType);
            }
            EditorGUILayout.EndVertical();


        }

    }
}

#endif
