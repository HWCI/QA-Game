#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace creXa.GameBase {

    [CustomEditor(typeof(ZAudio2D))]
    public class _Editor_ZAudio2D : ZEditor<ZAudio2D> {

        bool defaultFoldOut = true;
        GameObject obj;

        protected override void OnEnableRun()
        {
            base.OnEnableRun();
            defaultFoldOut = GetBool("defaultFoldOut"); 
        }

        protected override void OnInspectorGUIRun()
        {
            EditorGUILayout.BeginHorizontal(BestWidth());
            z.BGMOn = GUILayout.Toggle(z.BGMOn, "BGM", GUILayout.Width(50));
            z.BGMVolume = EditorGUILayout.Slider(z.BGMVolume, 0, 1);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal(BestWidth());
            z.SFXOn = GUILayout.Toggle(z.SFXOn, "SFX", GUILayout.Width(50));
            z.SFXVolume = EditorGUILayout.Slider(z.SFXVolume, 0, 1);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal(BestWidth());
            z.VOOn = GUILayout.Toggle(z.VOOn, "VO", GUILayout.Width(50));
            z.VOVolume = EditorGUILayout.Slider(z.VOVolume, 0, 1);
            EditorGUILayout.EndHorizontal();

            DrawSeparate();
            EditorGUILayout.PropertyField(sProp["BGSrc"]);
            EditorGUILayout.PropertyField(sProp["FXSrc"]);
            EditorGUILayout.PropertyField(sProp["VOSrc"]);

            DrawSeparate();
            EditorGUILayout.PropertyField(sProp["autoSetup"]);
            DrawSeparate();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Clear Roots"))
            {
                z.ClearAllRoots();
            }

            if (GUILayout.Button("Setup Roots To Main Camera"))
            {
                z.SetupRootsToMainCamera();
            }
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Setup Roots To..."))
            {
                z.SetupRootsTo(obj);
            }

            obj = (GameObject)EditorGUILayout.ObjectField(obj, typeof(GameObject), true);

            DrawSeparate();

            //defaults
            EditorGUILayout.BeginVertical("HelpBox");
            if (GUILayout.Button("Defaults", EditorStyles.toolbarButton))
            {
                defaultFoldOut = !defaultFoldOut;
                SetBool("defaultFoldOut", defaultFoldOut);
            }
            if (defaultFoldOut)
            {
                EditorGUILayout.BeginHorizontal(BestWidth());
                z.defaultBGMOn = GUILayout.Toggle(z.defaultBGMOn, "BGM", GUILayout.Width(50));
                z.defaultBGMVolume = EditorGUILayout.Slider(z.defaultBGMVolume, 0, 1);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal(BestWidth());
                z.defaultSFXOn = GUILayout.Toggle(z.defaultSFXOn, "SFX", GUILayout.Width(50));
                z.defaultSFXVolume = EditorGUILayout.Slider(z.defaultSFXVolume, 0, 1);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal(BestWidth());
                z.defaultVOOn = GUILayout.Toggle(z.defaultVOOn, "VO", GUILayout.Width(50));
                z.defaultVOVolume = EditorGUILayout.Slider(z.defaultVOVolume, 0, 1);
                EditorGUILayout.EndHorizontal();

            }
            EditorGUILayout.EndVertical();

        }

    }
}

#endif
