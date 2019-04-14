#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;

namespace creXa.GameBase
{
    [CustomEditor(typeof(ZDebug))]
    public class _Editor_ZDebug : ZEditor<ZDebug> {

        protected override void OnInspectorGUIRun()
        {
            
            z.SendLogToServer = GUILayout.Toggle(z.SendLogToServer, "Send Log To Server");

            if (z.SendLogToServer)
            {
                DrawSeparate();
                z.SendLogOnApplicationQuit = GUILayout.Toggle(z.SendLogOnApplicationQuit, "Send Log On Application Quit");
                z.ExceptionQuit = GUILayout.Toggle(z.ExceptionQuit, "Send Log On Exeception Quit");

                DrawSeparate();
                EditorGUILayout.LabelField("Type of Logs to be Sent", EditorStyles.centeredGreyMiniLabel);
                EditorGUILayout.BeginHorizontal();
                z.SendDebugLog = GUILayout.Toggle(z.SendDebugLog, "Debug");
                z.SendWarning = GUILayout.Toggle(z.SendWarning, "Warning");
                z.SendError = GUILayout.Toggle(z.SendError, "Error");
                if (!EditorGUIUtility.wideMode)
                {
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                }
                z.SendException = GUILayout.Toggle(z.SendException, "Exeception");
                z.SendAssert = GUILayout.Toggle(z.SendAssert, "Assert");
                EditorGUILayout.EndHorizontal();
            }


        }

    }
}
#endif
