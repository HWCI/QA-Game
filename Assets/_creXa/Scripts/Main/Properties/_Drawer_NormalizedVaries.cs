#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace creXa.GameBase
{
    [CustomPropertyDrawer(typeof(NormalizedVaries))]
    public class _Drawer_NormalizedVaries : ZPropertyDrawer
    {
        //UNDER IMPLEMENTATION
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty minValue = property.FindPropertyRelative("Min");
            SerializedProperty maxValue = property.FindPropertyRelative("Max");

            float fullWidth = position.width;
            Rect startRect = position;
            startRect.width = EditorGUIUtility.labelWidth;
            EditorGUI.LabelField(position, new GUIContent(property.displayName));

            fullWidth -= startRect.width;
            startRect.x += startRect.width - 13;
            startRect.width = 42;
            EditorGUI.LabelField(startRect, "Min");

            float x = fullWidth - 20 * 2;

            fullWidth -= startRect.width;
            startRect.x += startRect.width - 13;
            startRect.width = x / 2;

            EditorGUI.showMixedValue = minValue.hasMultipleDifferentValues;
            EditorGUI.BeginChangeCheck();
            float cMin = EditorGUI.FloatField(startRect, minValue.floatValue);
            if (EditorGUI.EndChangeCheck())
            {
                minValue.floatValue = cMin;
            }
            EditorGUI.showMixedValue = false;

            fullWidth -= startRect.width;
            startRect.x += startRect.width - 13;
            startRect.width = x / 2;

            EditorGUI.showMixedValue = maxValue.hasMultipleDifferentValues;
            EditorGUI.BeginChangeCheck();
            float cMax = EditorGUI.FloatField(startRect, maxValue.floatValue);
            if (EditorGUI.EndChangeCheck())
            {
                maxValue.floatValue = cMax;
            }
            EditorGUI.showMixedValue = false;

            fullWidth -= startRect.width;
            startRect.x += startRect.width - 13;
            startRect.width = 42;
            EditorGUI.LabelField(startRect, "Max");

        }
    }
}
#endif