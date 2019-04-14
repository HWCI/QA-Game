#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace creXa.GameBase.Graphics
{
    [CustomEditor(typeof(ZSCustomRegular)), CanEditMultipleObjects]
    public class _Editor_ZSCustomRegular : ZEditor4ZS<ZSCustomRegular>
    {

        protected override void OnInspectorGUIRun()
        {
            base.OnInspectorGUIRun();

            EditorGUI.showMixedValue = sProp["_sides"].hasMultipleDifferentValues;
            EditorGUILayout.PropertyField(sProp["_sides"]);
            EditorGUI.showMixedValue = false;

        }

    }

}
#endif