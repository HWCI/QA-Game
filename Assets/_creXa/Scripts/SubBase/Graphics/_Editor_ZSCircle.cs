#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace creXa.GameBase.Graphics
{
    [CustomEditor(typeof(ZSCircle)), CanEditMultipleObjects]
    public class _Editor_ZSCircle : ZEditor4ZS<ZSCircle> {

        protected override void OnInspectorGUIRun()
        {
            base.OnInspectorGUIRun();

            EditorGUI.showMixedValue = sProp["_reverse"].hasMultipleDifferentValues;
            EditorGUILayout.PropertyField(sProp["_reverse"], new GUIContent("Reverse?"));
            EditorGUI.showMixedValue = false;

            EditorGUILayout.PropertyField(sProp["_fill"]);
            EditorGUILayout.PropertyField(sProp["_phase"]);
            
        }

    }

}
#endif