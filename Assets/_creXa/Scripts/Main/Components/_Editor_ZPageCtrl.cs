#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace creXa.GameBase
{
    [CustomEditor(typeof(ZPageCtrl))]
    public class _Editor_ZPageCtrl : ZEditor<ZPageCtrl> {

        ReorderableList list;

        protected override void OnEnableRun()
        {
            base.OnEnableRun();
            SetupReorderableList();
        }

        void SetupReorderableList()
        {
            list = new ReorderableList(sObj, sProp["pages"], true, true, true, true);

            list.drawElementCallback = (rect, index, active, focused) =>
            {
                //each element
                Rect startRect = rect;
                rect.y += startRect.height * 0.1f;
                rect.height = EditorGUIUtility.singleLineHeight * 1.2f;
                if (index > z.pages.Count - 1) return;
                rect.height -= 2;
                float fullWidth = GetSuitableWidth() - 16;
                rect.width = 30;
                fullWidth -= rect.width;
                GUI.Label(rect, "#" + (index));

                rect.x += rect.width;
                rect.width = fullWidth - 16;
                z.pages[index] = (CanvasGroup)EditorGUI.ObjectField(rect, z.pages[index], typeof(CanvasGroup), true);
            };

            list.drawHeaderCallback = rect =>
            {
                GUI.Label(rect, "Total Pages: " + (z.pages != null ? z.pages.Count : 0));
            };

            list.onAddCallback = list =>
            {
                GameObject tmp = new GameObject("NewPage");
                CanvasGroup cg = tmp.AddComponent<CanvasGroup>();
                tmp.transform.SetParent(z.transform);
                tmp.transform.localPosition = Vector3.zero;
                tmp.transform.localScale = Vector3.one;
                RectTransform rT = tmp.AddComponent<RectTransform>();
                rT.anchorMin = Vector2.zero;
                rT.anchorMax = Vector2.one;
                rT.sizeDelta = Vector2.zero;
                z.pages.Add(cg);
            };

        }

        protected override void OnInspectorGUIRun()
        {
            base.OnInspectorGUIRun();
            EditorGUILayout.BeginVertical("Box");

            EditorGUILayout.PropertyField(sProp["nowPage"]);
            if (z.pages != null)
            {
                EditorGUILayout.BeginHorizontal();
                for (int i = 0; i < z.pages.Count; i++)
                {
                    if (GUILayout.Button(i.ToString(), GUILayout.Width(20)))
                    {
                        z.Show(i);
                    }
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.LabelField("Pages", EditorStyles.centeredGreyMiniLabel);
            list.DoLayoutList();

            EditorGUILayout.EndVertical();
        }
    }
}

#endif
