#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace creXa.GameBase
{
    public class ZEditor4ZAnimated<T> : ZEditor<T> where T : ZAnimated
    {
        protected override void OnInspectorGUIRun()
        {
            EditorGUILayout.PropertyField(sProp["pause"]);
            EditorGUILayout.PropertyField(sProp["timeCycle"]);
            EditorGUILayout.PropertyField(sProp["timeScale"]);  
        }
    }

}


#endif
