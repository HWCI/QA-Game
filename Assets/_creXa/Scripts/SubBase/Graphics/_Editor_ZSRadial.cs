#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace creXa.GameBase.Graphics
{
    [CustomEditor(typeof(ZSRadial)), CanEditMultipleObjects]
    public class _Editor_ZSRadial : ZEditor4ZS<ZSRadial>
    {

        protected override void OnInspectorGUIRun()
        {
            base.OnInspectorGUIRun();

            if(GUILayout.Button(" - Delete Field - "))
            {
                for(int i=0; i<targets.Length; i++)
                {
                    List<float> tmp = zGet(i).values.ToList();
                    tmp.RemoveAt(tmp.Count - 1);
                    zGet(i).values = tmp.ToArray();
                    zGet(i).sides = zGet(i).values.Length;
                }
                
            }

            for (int i = 0; i < z.values.Length; i++)
                EditorGUILayout.PropertyField(sProp["_values"].GetArrayElementAtIndex(i), new GUIContent("Value " + (i + 1)));

            if (GUILayout.Button(" + Add Field + "))
            {
                for (int i = 0; i < targets.Length; i++)
                {
                    List<float> tmp = zGet(i).values.ToList();
                    tmp.Add(Random.Range(0f, 1f));
                    zGet(i).values = tmp.ToArray();
                    zGet(i).sides = zGet(i).values.Length;
                }
            }

        }

    }

}
#endif