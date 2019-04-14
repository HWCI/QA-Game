#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace creXa.GameBase
{
    [CustomPropertyDrawer(typeof(RandomInt))]
    public class _Drawer_RandomInt : ZPropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            base.OnGUI(position, property, label);
            
            SerializedProperty minValue = property.FindPropertyRelative("_min");
            SerializedProperty maxValue = property.FindPropertyRelative("_max");

            float fullWidth = position.width;
            Rect startRect = position;
            startRect.width = EditorGUIUtility.labelWidth;
            EditorGUI.LabelField(position, new GUIContent(property.displayName));

            fullWidth -= startRect.width;
            float x = fullWidth - 42 - 70;

            fullWidth -= startRect.width;
            startRect.x += startRect.width;
            startRect.width = x / 2;

            EditorGUI.showMixedValue = minValue.hasMultipleDifferentValues;
            EditorGUI.BeginChangeCheck();
            int cMin = EditorGUI.IntField(startRect, minValue.intValue);
            if (EditorGUI.EndChangeCheck())
            {
                minValue.intValue = cMin;
            }
            EditorGUI.showMixedValue = false;

            fullWidth -= startRect.width;
            startRect.x += startRect.width;
            startRect.width = 20;
            EditorGUI.LabelField(startRect, " ~ ");

            fullWidth -= startRect.width;
            startRect.x += startRect.width;
            startRect.width = x / 2;

            EditorGUI.showMixedValue = maxValue.hasMultipleDifferentValues;
            EditorGUI.BeginChangeCheck();
            int cMax = EditorGUI.IntField(startRect, maxValue.intValue);
            if (EditorGUI.EndChangeCheck())
            {
                maxValue.intValue = cMax;
            }
            EditorGUI.showMixedValue = false;

            fullWidth -= startRect.width;
            startRect.x += startRect.width + 10;
            startRect.width = 60;
            EditorGUI.LabelField(startRect, "[Random]");


        }
    }
}

#endif
