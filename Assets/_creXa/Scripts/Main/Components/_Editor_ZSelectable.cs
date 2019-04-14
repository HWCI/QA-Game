#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace creXa.GameBase
{
    [CustomEditor(typeof(ZSelectable)), CanEditMultipleObjects]
    public class _Editor_ZSelectable : ZEditor<ZSelectable>
    {
        protected override void OnInspectorGUIRun()
        {
            base.OnInspectorGUIRun();

            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = sProp["_selected"].hasMultipleDifferentValues;
            bool selected = EditorGUILayout.Toggle("Selected", z.Selected);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
            {
                for (int i = 0; i < targets.Length; i++)
                    zGet(i).Selected = selected;
            }
            DrawSeparate();

            EditorGUILayout.PropertyField(sProp["btn"], new GUIContent("Button"));
            DrawSeparate();
            EditorGUILayout.PropertyField(sProp["useTheme"], new GUIContent("Use Theme System?"));

            EditorGUILayout.LabelField("Links", EditorStyles.centeredGreyMiniLabel);

            if (z.useTheme || sProp["useTheme"].hasMultipleDifferentValues)
            {
                EditorGUILayout.PropertyField(sProp["btnTheme"], new GUIContent("Button ThemeObj"));
                EditorGUILayout.PropertyField(sProp["imgTheme"], new GUIContent("Graphic ThemeObj"));
            }

            if (!z.useTheme || sProp["useTheme"].hasMultipleDifferentValues)
            {
                EditorGUILayout.PropertyField(sProp["zshape"], new GUIContent("Button Shape"));
                EditorGUILayout.PropertyField(sProp["zsGrahpic"], new GUIContent("Caption Grahpic"));
            }

            EditorGUILayout.LabelField("Unselected", EditorStyles.centeredGreyMiniLabel);

            if (z.useTheme || sProp["useTheme"].hasMultipleDifferentValues)
            {
                EditorGUILayout.PropertyField(sProp["unselectedBtnTheme"], new GUIContent("Button Theme"));
                EditorGUILayout.PropertyField(sProp["unselectedImgTheme"], new GUIContent("Caption Theme"));

            }

            if (!z.useTheme || sProp["useTheme"].hasMultipleDifferentValues)
            {
                EditorGUILayout.PropertyField(sProp["unselectedBtnColor"], new GUIContent("Button Color"));
                EditorGUILayout.PropertyField(sProp["unselectedBtnBorder"], new GUIContent("Button Border"));
                EditorGUILayout.PropertyField(sProp["unselectedCapColor"], new GUIContent("Caption Color"));
            }

            EditorGUILayout.LabelField("Selected", EditorStyles.centeredGreyMiniLabel);

            if (z.useTheme || sProp["useTheme"].hasMultipleDifferentValues)
            {
                EditorGUILayout.PropertyField(sProp["selectedBtnTheme"], new GUIContent("Button Theme"));
                EditorGUILayout.PropertyField(sProp["selectedImgTheme"], new GUIContent("Caption Theme"));

            }

            if (!z.useTheme || sProp["useTheme"].hasMultipleDifferentValues)
            {
                EditorGUILayout.PropertyField(sProp["selectedBtnColor"], new GUIContent("Button Color"));
                EditorGUILayout.PropertyField(sProp["selectedBtnBorder"], new GUIContent("Button Border"));
                EditorGUILayout.PropertyField(sProp["selectedCapColor"], new GUIContent("Caption Color"));
            }
        }
    }
}

#endif
