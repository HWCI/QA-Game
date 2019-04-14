#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace creXa.GameBase
{
    [CustomPropertyDrawer(typeof(NVariesCtrl))]
    public class _Drawer_NVariesCtrl : ZPropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            base.OnGUI(position, property, label);
            SerializedProperty valueNV = property.FindPropertyRelative("Value");
            SerializedProperty valueNV_Min = valueNV.FindPropertyRelative("Min");
            SerializedProperty valueNV_Max = valueNV.FindPropertyRelative("Max");
            SerializedProperty varies = property.FindPropertyRelative("Varies");
            SerializedProperty ctrl = property.FindPropertyRelative("Ctrl");

            float fullWidth = position.width;
            Rect startRect = position;
            startRect.width = 40;

            ctrl.boolValue = GUI.Toggle(startRect, ctrl.boolValue, property.displayName);

            if (ctrl.boolValue)
            {
                fullWidth -= startRect.width;
                startRect.x += startRect.width - 13;
                startRect.width = 42;
                EditorGUI.LabelField(startRect, "Min");

                float x = fullWidth - 29 * 2 - 37;

                fullWidth -= startRect.width;
                startRect.x += startRect.width - 13;
                startRect.width = x / 2;

                EditorGUI.showMixedValue = valueNV_Min.hasMultipleDifferentValues;
                EditorGUI.BeginChangeCheck();
                float cMin = EditorGUI.FloatField(startRect, valueNV_Min.floatValue);
                if (EditorGUI.EndChangeCheck())
                {
                    valueNV_Min.floatValue = cMin;
                }
                EditorGUI.showMixedValue = false;

                fullWidth -= startRect.width;
                startRect.x += startRect.width + 2;
                startRect.width = x / 2;

                EditorGUI.showMixedValue = valueNV_Max.hasMultipleDifferentValues;
                EditorGUI.BeginChangeCheck();
                float cMax = EditorGUI.FloatField(startRect, valueNV_Max.floatValue);
                if (EditorGUI.EndChangeCheck())
                {
                    valueNV_Max.floatValue = cMax;
                }
                EditorGUI.showMixedValue = false;

                fullWidth -= startRect.width;
                startRect.x += startRect.width;
                startRect.width = 42;
                EditorGUI.LabelField(startRect, "Max");

                fullWidth -= startRect.width;
                startRect.x += startRect.width - 13;
                startRect.width = 50;
                
                EditorGUI.showMixedValue = varies.hasMultipleDifferentValues;
                EditorGUI.BeginChangeCheck();
                AnimationCurve cVaries = EditorGUI.CurveField(startRect, varies.animationCurveValue);
                if (EditorGUI.EndChangeCheck())
                {
                    varies.animationCurveValue = cVaries;
                }
                EditorGUI.showMixedValue = false;


            }
        }
    }
}

#endif
