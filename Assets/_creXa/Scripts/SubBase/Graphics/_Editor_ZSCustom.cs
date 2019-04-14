#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace creXa.GameBase.Graphics
{
    [CustomEditor(typeof(ZSCustom)), CanEditMultipleObjects]
    public class _Editor_ZSCustom : ZEditor4ZS<ZSCustom>
    {

        protected override void OnInspectorGUIRun()
        {
            base.OnInspectorGUIRun();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Point Count: " + z.customPoints.Count);
            
            if (GUILayout.Button("+", GUILayout.Width(20)))
            {
                for(int i=0; i<targets.Length; i++)
                {
                    ((ZSCustom)targets[i]).customPoints.Add(new Vector2());
                    Repaint();
                }
            }
            if (GUILayout.Button("-", GUILayout.Width(20)))
            {
                for (int i = 0; i < targets.Length; i++)
                {
                    if (((ZSCustom)targets[i]).customPoints.Count > 0)
                    {
                        ((ZSCustom)targets[i]).customPoints.RemoveAt(((ZSCustom)targets[i]).customPoints.Count - 1);
                        Repaint();
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUI.showMixedValue = sProp["_customPoints"].hasMultipleDifferentValues;
            EditorGUILayout.PropertyField(sProp["_customPoints"], true);
            EditorGUI.showMixedValue = false;


        }

    }

}
#endif