#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace creXa.GameBase
{
    public class ZEditor4ZThemeObj<T> : ZEditor<T> where T : ZThemeObj
    {
        protected override void OnInspectorGUIRun()
        {
            GUI.enabled = false;
            EditorGUILayout.IntField("Theme", z.Theme);
            GUI.enabled = true;

            
            EditorGUI.BeginChangeCheck();
            ZThemeSys sys = FindObjectOfType<ZThemeSys>();
            EditorGUI.showMixedValue = sProp["_variation"].hasMultipleDifferentValues;
            int variation = EditorGUILayout.IntSlider("Variation", z.Variation, 0, sys ? sys.Variation - 1 : 20);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
            {
                for(int i=0; i<targets.Length; i++)
                    ((T)targets[i]).Variation = variation;
            }
        }
    }
}

#endif