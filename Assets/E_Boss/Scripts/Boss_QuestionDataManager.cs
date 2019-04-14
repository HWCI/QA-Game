using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public enum EmployeeType
{
    秘書部 = 0,
    維修部 = 1,
    市場調查部 = 2,
    廣告部 = 3,
    銷售部=4,
    管理層=5
}
[Serializable]
public class Boss_QusetionData
{
    public string name;
    [Multiline(2)]
    public string CharacterDescription;
    public string[] Answer;
    public Sprite CharacterIcon;
    public EmployeeType Type;
}
public class Boss_QuestionDataManager : MonoBehaviour {
    [HideInInspector]
    public Boss_QuestionControl QuestionControl;

    public List<Boss_QusetionData> QusetionDataBase;
    [HideInInspector]
    public List<Boss_QusetionData> PlayerQusetion;

    public void QusetionGenerator()
    {
        PlayerQusetion = new List<Boss_QusetionData>(QusetionDataBase);
    }
}

