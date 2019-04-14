#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Linq;

namespace creXa.GameBase
{
    [CustomEditor(typeof(ZRecord)), CanEditMultipleObjects]
    public class _Editor_ZRecord : ZEditor<ZRecord> {
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
            
            EditorGUILayout.PropertyField(sProp["keyRef"]);
            DrawSeparate();

            EditorGUILayout.PropertyField(sProp["selectable"], true);
            EditorGUILayout.PropertyField(sProp["button"], true);
            EditorGUILayout.PropertyField(sProp["field"], true);
        }

    }
}
#endif