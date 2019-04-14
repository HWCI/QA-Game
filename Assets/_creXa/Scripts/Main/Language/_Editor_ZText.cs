#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace creXa.GameBase
{
    [CustomEditor(typeof(ZText))]
    public class _Editor_ZText : ZEditor<ZText>
    {
        bool defaultFoldOut;

        protected override void OnEnableRun()
        {
            base.OnEnableRun();
            defaultFoldOut = GetBool("defaultFoldOut");
        }

        protected override void OnInspectorGUIRun()
        {
            GUI.enabled = false;
            EditorGUILayout.IntField("Language", z.Language);
            GUI.enabled = true;

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginHorizontal();
            z.loadInBank = GUILayout.Toggle(z.loadInBank, "Load In Bank" + (z.loadInBank ? ", ID:" : ""), BestLabelWidth());
            if (z.loadInBank)
            {
                z.ID = EditorGUILayout.TextField(z.ID);
            }
            EditorGUILayout.EndHorizontal();


            if (EditorGUI.EndChangeCheck())
            {
                if (ZLangSys.It && !z.loadInBank && 
                    (z.Langs == null || z.Langs.Length != ZLangSys.It.Language.Length))
                {
                    z.Langs = new ZText.Lang[ZLangSys.It.Language.Length];
                    EditorGUIUtility.ExitGUI();
                }
            }
            if (z.loadInBank)
            {
                if (GUILayout.Button("Specified", EditorStyles.toolbarButton))
                {
                    defaultFoldOut = !defaultFoldOut;
                    SetBool("defaultFoldOut", defaultFoldOut);
                }
            }
            if ((!z.loadInBank || defaultFoldOut) && z.Langs != null)
            {
                for(int i=0; i<z.Langs.Length; i++)
                {
                    DrawSeparate();
                    EditorGUILayout.LabelField("#" + i + " " + (i < ZLangSys.LanguageType.Length? ZLangSys.LanguageType[i]: ZLangSys.It.Language[i].Description));
                    EditorGUILayout.PrefixLabel("Content");
                    z.Langs[i].content = EditorGUILayout.TextArea(z.Langs[i].content, GUILayout.MinHeight(EditorGUIUtility.singleLineHeight * 2));

                    EditorGUILayout.BeginHorizontal();
                    z.Langs[i].setFont = GUILayout.Toggle(z.Langs[i].setFont, "Specify Font", z.Langs[i].setFont? BestLabelWidth() : BestWidth());
                    if (z.Langs[i].setFont) {
                        z.Langs[i].font = (Font)EditorGUILayout.ObjectField(z.Langs[i].font, typeof(Font), true);
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    z.Langs[i].setFontSize = GUILayout.Toggle(z.Langs[i].setFontSize, "Specify Font Size", z.Langs[i].setFontSize ? BestLabelWidth() : BestWidth());
                    if (z.Langs[i].setFontSize)
                    {
                        z.Langs[i].fontSize = EditorGUILayout.IntField(z.Langs[i].fontSize);
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    z.Langs[i].setFontStyle = GUILayout.Toggle(z.Langs[i].setFontStyle, "Specify Font Style", z.Langs[i].setFontStyle ? BestLabelWidth() : BestWidth());
                    if (z.Langs[i].setFontStyle)
                    {
                        z.Langs[i].fontStyle = (FontStyle)EditorGUILayout.EnumPopup(z.Langs[i].fontStyle);
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    z.Langs[i].setLineSpacing = GUILayout.Toggle(z.Langs[i].setLineSpacing, "Specify Line Spacing", z.Langs[i].setLineSpacing ? BestLabelWidth() : BestWidth());
                    if (z.Langs[i].setLineSpacing)
                    {
                        z.Langs[i].lineSpacing = EditorGUILayout.FloatField(z.Langs[i].lineSpacing);
                    }
                    EditorGUILayout.EndHorizontal();

                }
                    
            }

        }
    }
}

#endif
