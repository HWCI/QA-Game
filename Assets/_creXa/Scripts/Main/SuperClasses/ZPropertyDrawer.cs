#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;

namespace creXa.GameBase
{
    public class ZPropertyDrawer : PropertyDrawer {

        /* UNDER CONSTRUCTION */
        protected Dictionary<string, SerializedProperty> sProp;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            
        }

        protected void DrawSeparate(float space = 3)
        {
            GUILayout.Box("", GUILayout.Width(GetSuitableWidth() + 8), GUILayout.Height(3));
            GUILayout.Space(space);
        }

        public float GetSuitableWidth(float x = -40)
        {
            return EditorGUIUtility.currentViewWidth + x;
        }

        public GUILayoutOption BestWidth(float x = 0)
        {
            return GUILayout.Width(GetSuitableWidth() + x);
        }

        public GUILayoutOption BestCellWidth(float x = 0)
        {
            return BestWidth(-EditorGUIUtility.labelWidth + 0);
        }

        public GUILayoutOption BestLabelWidth(float x = 0)
        {
            return GUILayout.Width(EditorGUIUtility.labelWidth + x);
        }

        public GUILayoutOption BestFieldWidth(float x = 0)
        {
            return GUILayout.Width(EditorGUIUtility.fieldWidth + x);
        }

    }
}
#endif