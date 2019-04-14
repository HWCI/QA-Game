#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;
namespace creXa.GameBase
{
    [CustomEditor(typeof(ZTSpiral))]
    public class _Editor_ZTSpiral : ZEditor<ZTSpiral>
    {
        bool visualFoldout;

        GUIContent alwaysShowGUIC = new GUIContent("Always show", "Show curve even if not selected.");

        protected override void OnSceneGUIRun()
        {
            base.OnSceneGUIRun();
            
        }

        protected override void OnInspectorGUIRun()
        {
            base.OnInspectorGUIRun();
            DrawVisual();
            DrawSeparate();
            DrawFastOperation();
            DrawSeparate();
            DrawData();
        }

        void RegularCheck()
        {
            if (z.regular)
            {
                z.startBound.z = z.startBound.x;
                z.endBound.z = z.endBound.x;
            }
        }

        void DrawFastOperation()
        {
            EditorGUILayout.LabelField("Approx. Length: " + z.ApproxLength);
            
            if (GUILayout.Button("Exp. Rate = 1", BestWidth()))
            {
                z.expRate = 1;
                SceneView.RepaintAll();
            }
            EditorGUILayout.BeginHorizontal(BestWidth());
            EditorGUILayout.LabelField("Start <=> End: ", BestLabelWidth());
            if (GUILayout.Button("X"))
            {
                float tmp = z.startBound.x;
                z.startBound.x = z.endBound.x;
                z.endBound.x = tmp;
                RegularCheck();
                SceneView.RepaintAll();
            }
            if (GUILayout.Button("Y"))
            {
                float tmp = z.startBound.y;
                z.startBound.y = z.endBound.y;
                z.endBound.y = tmp;
                RegularCheck();
                SceneView.RepaintAll();
            }
            if (GUILayout.Button("Z"))
            {
                float tmp = z.startBound.z;
                z.startBound.z = z.endBound.z;
                z.endBound.z = tmp;
                RegularCheck();
                SceneView.RepaintAll();
            }
            if (GUILayout.Button("ALL"))
            {
                Vector3 tmp = z.startBound;
                z.startBound = z.endBound;
                z.endBound = tmp;
                RegularCheck();
                SceneView.RepaintAll();
            }
            EditorGUILayout.EndHorizontal();
        }

        void DrawData()
        {
            EditorGUILayout.PropertyField(sProp["startBound"]);
            EditorGUILayout.PropertyField(sProp["endBound"]);
            EditorGUI.BeginChangeCheck();
            z.regular = EditorGUILayout.Toggle("Regular", z.regular);
            if (EditorGUI.EndChangeCheck())
            {
                RegularCheck();
                SceneView.RepaintAll();
            }
            EditorGUILayout.PropertyField(sProp["expRate"]);
            EditorGUILayout.PropertyField(sProp["frequency"]);
            EditorGUILayout.PropertyField(sProp["phase"]);

        }

        void DrawVisual()
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginHorizontal();
            visualFoldout = EditorGUILayout.Foldout(visualFoldout, "Editor Settings");
            z.visual.alwaysShow = GUILayout.Toggle(z.visual.alwaysShow, alwaysShowGUIC, "radio");
            EditorGUILayout.EndHorizontal();

            if (visualFoldout)
            {
                EditorGUILayout.BeginVertical("Box");
                EditorGUILayout.PropertyField(sProp["visual.resolution"]);
                z.visual.pathColor = EditorGUILayout.ColorField("Path Color", z.visual.pathColor);
                z.visual.inactivePathColor = EditorGUILayout.ColorField("Inactive Path Color", z.visual.inactivePathColor);
                z.visual.handleColor = EditorGUILayout.ColorField("Handle Color", z.visual.handleColor);
                if (GUILayout.Button("Default colors"))
                {
                    z.visual = new ZTrack_Visual();
                }
                EditorGUILayout.EndVertical();
            }
            if (EditorGUI.EndChangeCheck())
            {
                SceneView.RepaintAll();
            }
        }

    }

    

}

#endif
