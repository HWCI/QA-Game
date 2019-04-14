#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace creXa.GameBase
{
    public class ZEditor4ZAnimated2DTC<T> : ZEditor4ZAnimated<T> where T : ZAnimated2DTC
    {
        protected override void OnInspectorGUIRun()
        {
            base.OnInspectorGUIRun();
            DrawSeparate();
            EditorGUILayout.PropertyField(sProp["initTime"]);
        }
    }
}

#endif