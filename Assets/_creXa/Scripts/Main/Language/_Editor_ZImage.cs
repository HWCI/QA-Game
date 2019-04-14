#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace creXa.GameBase
{
    [CustomEditor(typeof(ZImage))]
    public class _Editor_ZImage : ZEditor<ZImage>
    {
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
            if (z.loadInBank)
            {
                EditorGUILayout.LabelField("Link: Resources.Load(\"Language/\" + Bank[ID]\")");
            }


            if (EditorGUI.EndChangeCheck())
            {
                if (ZLangSys.It && !z.loadInBank &&
                    (z.content == null || z.content.Length != ZLangSys.It.Language.Length))
                {
                    z.content = new Sprite[ZLangSys.It.Language.Length];
                    EditorGUIUtility.ExitGUI();
                }
                else if (z.loadInBank && z.content != null)
                {
                    z.content = null;
                    EditorGUIUtility.ExitGUI();
                }
            }
            if (!z.loadInBank)
            {
                for (int i = 0; i < z.content.Length; i++)
                {
                    DrawSeparate();
                    EditorGUILayout.LabelField("#" + i + " " + (i < ZLangSys.LanguageType.Length ? ZLangSys.LanguageType[i] : ZLangSys.It.Language[i].Description));
                    z.content[i] = (Sprite)EditorGUILayout.ObjectField("Sprite", z.content[i], typeof(Sprite), true);
                }

            }

        }
    }
}

#endif