using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Boss_TimeManager))]
public class MyTimeManagerEditorAlternative : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Boss_TimeManager mp = (Boss_TimeManager)target;
        EditorGUILayout.LabelField("Timer Range Setting", EditorStyles.boldLabel);

        mp.changeToRedAt = EditorGUILayout.Slider("Red Range", mp.changeToRedAt, 0, 100);
        mp.changeToOrangeAt = EditorGUILayout.Slider("Orange Range", mp.changeToOrangeAt, 0, 100);
        if (mp.changeToOrangeAt < mp.changeToRedAt)
        {
            mp.changeToOrangeAt = mp.changeToRedAt;
        }
        ProgressBar(mp.changeToRedAt, mp.changeToOrangeAt);


        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Time", EditorStyles.boldLabel);
        mp.MaxTime = EditorGUILayout.FloatField("Max Time: ", mp.MaxTime);
        //workerEnergy
        mp.Timer = EditorGUILayout.Slider("Timer", mp.Timer, 0, mp.MaxTime);
        //mp.timeBar.GetComponent<RectTransform>().anchoredPosition = new Vector3((100 - mp.Timer) * -3.3f, 0, 0);
        mp.TimerChange();

    }

    void ProgressBar(float value, float value2)
    {
        value = value / 100;
        value2 = value2 / 100 - value;
        float value3 = 1 - value - value2;
        GUILayout.BeginHorizontal();
        var guicolor_backup = GUI.backgroundColor;
        GUI.backgroundColor = Color.red; GUILayout.Box((value * 100).ToString() + "%", GUILayout.Width(EditorGUIUtility.currentViewWidth * value), GUILayout.MaxHeight(20));
        GUI.backgroundColor = new Color(1, 0.5f, 0); GUILayout.Box((value2 * 100).ToString() + "%", GUILayout.Width(EditorGUIUtility.currentViewWidth * value2), GUILayout.MaxHeight(20));
        GUI.backgroundColor = Color.green; GUILayout.Box((value3 * 100).ToString() + "%", GUILayout.Width(EditorGUIUtility.currentViewWidth * value3), GUILayout.MaxHeight(20));
        GUI.backgroundColor = guicolor_backup;
        GUILayout.EndHorizontal();


        EditorGUILayout.Space();
    }
}