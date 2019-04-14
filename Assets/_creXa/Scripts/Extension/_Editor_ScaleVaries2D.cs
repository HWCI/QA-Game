#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace creXa.GameBase
{
    [CustomEditor(typeof(ScaleVaries2D)), CanEditMultipleObjects]
    public class _Editor_ScaleVaries2D : ZEditor4ZAnimated2DTC<ScaleVaries2D>
    {
        protected override void OnInspectorGUIRun()
        {
            base.OnInspectorGUIRun();
            DrawSeparate();

            EditorGUILayout.PropertyField(sProp["offset"]);
            EditorGUILayout.Space();

            string[] fields = new string[] { "X", "Y" };
            for (int i = 0; i < fields.Length; i++)
                EditorGUILayout.PropertyField(sProp[fields[i]]);

        }
    }
}
#endif
