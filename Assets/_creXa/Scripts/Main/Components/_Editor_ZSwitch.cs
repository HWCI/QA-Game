#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using creXa.GameBase.Graphics;

namespace creXa.GameBase
{
    [CustomEditor(typeof(ZSwitch))]
    public class _Editor_ZSwitch : ZEditor<ZSwitch>
    {
        protected override void OnInspectorGUIRun()
        {
            base.OnInspectorGUIRun();
            EditorGUILayout.PropertyField(sProp["_value"]);
            DrawSeparate();

            if (GUILayout.Button("Update Size"))
            {
                RectTransform rect = z.GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, rect.sizeDelta.x / 2);
                z.bg.GetComponent<RectTransform>().sizeDelta = rect.sizeDelta;
                z.bg.SetAllRoundRadius(Mathf.RoundToInt(rect.sizeDelta.x / 2));
                z.bg.transform.localPosition = Vector2.zero;
                z.btn.GetComponent<RectTransform>().sizeDelta = new Vector2(rect.sizeDelta.x, rect.sizeDelta.x) * 0.45f;

                z.btnAni.SetRectPos(new Vector3((z.Value ? 1 : -1) * rect.sizeDelta.x / 4, 0, 0));
                z.bgAni.SetColor(z.Value ? z.onColor : z.offColor);
            }

            EditorGUILayout.PropertyField(sProp["interactable"]);
            EditorGUILayout.PropertyField(sProp["animated"]);
            DrawSeparate();

            EditorGUILayout.PropertyField(sProp["bg"]);
            EditorGUILayout.PropertyField(sProp["btn"]);
            EditorGUILayout.PropertyField(sProp["bgAni"]);
            EditorGUILayout.PropertyField(sProp["btnAni"]);
            DrawSeparate();

            EditorGUILayout.PropertyField(sProp["onColor"]);
            EditorGUILayout.PropertyField(sProp["offColor"]);
            DrawSeparate();

            EditorGUILayout.PropertyField(sProp["OnValueChanged"]);
        }
    }
}
#endif
