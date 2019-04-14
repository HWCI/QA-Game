#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace creXa.GameBase.Graphics
{
    [CustomEditor(typeof(ZSStar)), CanEditMultipleObjects]
    public class _Editor_ZSStar : ZEditor4ZS<ZSStar>
    {

        protected override void OnInspectorGUIRun()
        {
            base.OnInspectorGUIRun();

            EditorGUI.showMixedValue = sProp["_sides"].hasMultipleDifferentValues;
            EditorGUILayout.PropertyField(sProp["_sides"]);
            EditorGUI.showMixedValue = false;

            EditorGUI.showMixedValue = sProp["_innerRadius"].hasMultipleDifferentValues;
            EditorGUILayout.PropertyField(sProp["_innerRadius"]);
            EditorGUI.showMixedValue = false;

        }

    }

}
#endif