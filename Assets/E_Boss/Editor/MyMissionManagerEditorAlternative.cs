using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

[CustomEditor(typeof(Boss_MissionManager))]
[CanEditMultipleObjects]
public class MyMissionManagerEditorAlternative : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Boss_MissionManager mp = (Boss_MissionManager)target;
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("測試隨機任務", EditorStyles.boldLabel);
        GUI.color = Color.green;
        if (GUILayout.Button("Mission Generator"))
        {

            int Temp01;
            Temp01 = Random.Range(0, 5);


            int Temp02;
            do
            {
                Temp02 = Random.Range(0, 5);
            } while (Temp02 == Temp01);


            int Temp03;
            do
            {
                Temp03 = Random.Range(4, 6);
            } while (Temp03 == Temp01 || Temp03 == Temp02);

            mp.GamePageMission[0].text = mp.MS[Temp01];
            mp.GamePageMission[1].text = mp.MS[Temp02];
            mp.GamePageMission[2].text = mp.MS[Temp03];
            mp.GamePageMission[0].enabled = false;
            mp.GamePageMission[1].enabled = false;
            mp.GamePageMission[2].enabled = false;
            mp.GamePageMission[0].enabled = true;
            mp.GamePageMission[1].enabled = true;
            mp.GamePageMission[2].enabled = true;
        }
        GUI.color = Color.white;
    }
}

