using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;



//UI
[CustomEditor(typeof(Boss_energyManager))]
public class MyEnergyManagerEditorAlternative : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Boss_energyManager mp = (Boss_energyManager)target;

        mp.defaultValue = EditorGUILayout.IntSlider("Default Value", mp.m_defaultValue, 0, 100);
        mp.m_defaultValue = mp.defaultValue;

        EditorGUILayout.LabelField("Energy", EditorStyles.boldLabel);
        mp.workerEnergy = EditorGUILayout.Slider("Worker Energy", mp.workerEnergy, 0, 100);
        mp.moneyEnergy = EditorGUILayout.Slider("Money Energy", mp.moneyEnergy, 0, 100);
        mp.clientEnergy = EditorGUILayout.Slider("Client Energy", mp.clientEnergy, 0, 100);
        mp.qualityEnergy = EditorGUILayout.Slider("Quality Energy", mp.qualityEnergy, 0, 100);


        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Energy Range Setting", EditorStyles.boldLabel);

        mp.changeToRedAt = EditorGUILayout.Slider("Red Range", mp.changeToRedAt, 0, 100);
        mp.changeToOrangeAt = EditorGUILayout.Slider("Orange Range", mp.changeToOrangeAt, 0, 100);
        if (mp.changeToOrangeAt < mp.changeToRedAt)
        {
            mp.changeToOrangeAt = mp.changeToRedAt;
        }
        ProgressBar(mp.changeToRedAt, mp.changeToOrangeAt);


        mp.EnergyChange();
    }

    // Custom GUILayout progress bar.
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

