#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace creXa.GameBase.Graphics
{
    [CustomEditor(typeof(ZSRectangle)), CanEditMultipleObjects]
    public class _Editor_ZSRectangle : ZEditor4ZS<ZSRectangle>
    {

        int allroundradius = 25;

        protected override void OnInspectorGUIRun()
        {
            base.OnInspectorGUIRun();
            string[] symbol = new string[] { "Top Right ╗", "Bottom Right ╝", "Bottom Left ╚", "Top Right ╔"};

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Set All Unrounded"))
            {
                for (int i = 0; i < targets.Length; i++)
                    zGet(i).SetAllRoundCorner(false);
            }
            if (GUILayout.Button("Set All Rounded: "))
            {
                for (int i = 0; i < targets.Length; i++)
                    zGet(i).SetAllRoundRadius(allroundradius);
            }
            allroundradius = EditorGUILayout.IntField(allroundradius);
            EditorGUILayout.EndHorizontal();
            DrawSeparate();

            EditorGUI.BeginChangeCheck();
            for(int i=0; i<z.Side; i++)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.PropertyField(sProp["_roundCorner"].GetArrayElementAtIndex(i), new GUIContent(symbol[i]));
                if (z.roundCorner[i])
                {
                    EditorGUILayout.PropertyField(sProp["_roundRadius"].GetArrayElementAtIndex(i), new GUIContent("Radius: "));
                }
                    
                EditorGUILayout.EndHorizontal();
            }
            if (EditorGUI.EndChangeCheck())
            {
                z.SetAllDirty();
            }

            DrawSeparate();

            EditorGUILayout.PropertyField(sProp["_detail"]);

        }

    }

}
#endif