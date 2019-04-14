#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace creXa.GameBase
{
    [CustomEditor(typeof(ZInput))]
    public class _Editor_ZInput : ZEditor<ZInput>
    {
        bool defaultFoldOut = true;

        protected override void OnEnableRun()
        {
            base.OnEnableRun();

            defaultFoldOut = GetBool("defaultFoldOut"); 
        }

        protected override void OnInspectorGUIRun()
        {
            EditorGUILayout.PropertyField(sProp["isTouchCtrl"]);
            EditorGUILayout.PropertyField(sProp["touchInterval"]);

            //defaults
            EditorGUILayout.BeginVertical("HelpBox");
            if (GUILayout.Button("Defaults", EditorStyles.toolbarButton))
            {
                defaultFoldOut = !defaultFoldOut;
                SetBool("defaultFoldOut", defaultFoldOut);
            }
            if (defaultFoldOut)
            {
                EditorGUILayout.PropertyField(sProp["defaultTouchCtrl"]);
                EditorGUILayout.PropertyField(sProp["defaultTouchInterval"]);
            }
            EditorGUILayout.EndVertical();


        }
    }
}

#endif
