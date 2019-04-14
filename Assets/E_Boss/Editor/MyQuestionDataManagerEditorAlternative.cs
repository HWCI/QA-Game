using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Boss_QuestionDataManager))]
[CanEditMultipleObjects]
public class MyQuestionDataManagerEditorAlternative : Editor
{
    int index = 0;
    string[] options;
    Boss_QuestionDataManager mp;
    public override void OnInspectorGUI()
    {

        mp = (Boss_QuestionDataManager)target;
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("測試", EditorStyles.boldLabel);

        options = new string[mp.QusetionDataBase.Count];

        for (int i = 0; i < mp.QusetionDataBase.Count; i++)
        {
            options[i] = mp.QusetionDataBase[i].name;
        }

        GUILayout.BeginHorizontal();
        index = EditorGUILayout.Popup("Character", index, options);
        GUI.color = Color.green;
        if (GUILayout.Button(" < "))
        { index--; }
        GUI.color = Color.white;
        GUI.color = Color.yellow;
        if (GUILayout.Button(" > "))
        { index++; }
        GUI.color = Color.white;
        if (index < 0)
            index = options.Length - 1;
        if (index >= options.Length)
        {
            index = 0;
        }
        GUILayout.EndHorizontal();
        UpdataCharacter();

        EditorGUILayout.Space();
        DrawDefaultInspector();

    }

    void UpdataCharacter()
    {
#if UNITY_EDITOR
        if (Application.isPlaying)
            return;
#endif
        mp.QuestionControl.CharacterName.text = mp.QusetionDataBase[index].name;
        mp.QuestionControl.CharacterDescriptionTxt.text = mp.QusetionDataBase[index].CharacterDescription;
        mp.QuestionControl.CharacterIcon.sprite = mp.QusetionDataBase[index].CharacterIcon;
        mp.QuestionControl.CharacterName.enabled = false;
        mp.QuestionControl.CharacterName.enabled = true;
    }
}
