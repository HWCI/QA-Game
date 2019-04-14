#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace creXa.GameBase.Graphics
{
    [CanEditMultipleObjects]
    public class ZEditor4ZS<T> : ZEditor<T> where T : ZShape
    {
        protected override void OnInspectorGUIRun()
        {
            base.OnInspectorGUIRun();

            EditorGUILayout.PropertyField(sProp["m_RaycastTarget"]);
            EditorGUILayout.PropertyField(sProp["m_Material"]);
            EditorGUILayout.PropertyField(sProp["_texture"]);
            EditorGUILayout.PropertyField(sProp["m_Color"]);
            DrawSeparate();

            EditorGUI.showMixedValue = sProp["_border"].hasMultipleDifferentValues;
            EditorGUILayout.PropertyField(sProp["_border"], new GUIContent("Have Border?"));
            EditorGUI.showMixedValue = false;

            if (z.border)
            {
                EditorGUI.showMixedValue = sProp["_borderWidth"].hasMultipleDifferentValues;
                EditorGUILayout.PropertyField(sProp["_borderWidth"], new GUIContent("Width"));
                EditorGUI.showMixedValue = false;

                EditorGUI.showMixedValue = sProp["_borderColor"].hasMultipleDifferentValues;
                EditorGUILayout.PropertyField(sProp["_borderColor"], new GUIContent("Color"));
                EditorGUI.showMixedValue = false;
            }

            DrawSeparate();

            EditorGUI.showMixedValue = sProp["_regularSize"].hasMultipleDifferentValues;
            EditorGUILayout.PropertyField(sProp["_regularSize"], new GUIContent("Regular Size?"));
            EditorGUI.showMixedValue = false;

            EditorGUI.showMixedValue = sProp["_match"].hasMultipleDifferentValues;
            EditorGUILayout.PropertyField(sProp["_match"], new GUIContent("Match"));
            EditorGUI.showMixedValue = false;
            DrawSeparate();
        }
    }

}


#endif