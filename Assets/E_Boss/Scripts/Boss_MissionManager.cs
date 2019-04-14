using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss_MissionManager : MonoBehaviour {
    /*
        請保持生產力數值不跌至40%以下
        請保持資金數值不跌至40%以下
        請保持客戶數值不跌至40%以下
        請保持品質數值不跌至40%以下
        
        連續分配4個員工到最佳位置
        
        共聘請10個員工
     */

    public static Boss_MissionManager instance;
    public List<string> MS;
    [HideInInspector]
    public List<int> PlayerMission;

    [HideInInspector]
    public List<Text> StartPageMission;

    [HideInInspector]
    public List<Text> GamePageMission;

    [HideInInspector]
    public List<Text> FinishMission;

    [HideInInspector]
    public List<Text> TimesUpMission;

    [HideInInspector]
    public List<Image> GamePageMissionStar;

    [HideInInspector]
    public List<Image> FinishMissionStar;

    [HideInInspector]
    public List<Image> TimesUpMissionStar;
    
    [HideInInspector]
    public int playerAnswersNumber = 0;
    [HideInInspector]
    public int playerConsecutiveAnswers = 0;

    Boss_energyManager energyManager;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        energyManager = Boss_energyManager.instance;
        //MissionReset();
    }
    public void MissionReset()
    {
        GamePageMissionStar[0].enabled = false;
        GamePageMissionStar[1].enabled = false;
        GamePageMissionStar[2].enabled = false;

        TimesUpMissionStar[0].enabled = false;
        TimesUpMissionStar[1].enabled = false;
        TimesUpMissionStar[2].enabled = false;

        FinishMissionStar[0].enabled = false;
        FinishMissionStar[1].enabled = false;
        FinishMissionStar[2].enabled = false;
        playerAnswersNumber = -1;
        playerConsecutiveAnswers = 0;
        MissionGenerator();
    }

    public void MissionGenerator()
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




        GamePageMission[0].text = MS[Temp01];
        GamePageMission[1].text = MS[Temp02];
        GamePageMission[2].text = MS[Temp03];

        PlayerMission = new List<int>();
        PlayerMission.Add(Temp01);
        PlayerMission.Add(Temp02);
        PlayerMission.Add(Temp03);

        StartPageMission[0].text = MS[Temp01];
        FinishMission[0].text = MS[Temp01];
        TimesUpMission[0].text = MS[Temp01];

        StartPageMission[1].text = MS[Temp02];
        FinishMission[1].text = MS[Temp02];
        TimesUpMission[1].text = MS[Temp02];

        StartPageMission[2].text = MS[Temp03];
        FinishMission[2].text = MS[Temp03];
        TimesUpMission[2].text = MS[Temp03];

    }

    public void Check()
    {
        if (energyManager.workerEnergy < 40)
        {
            if(PlayerMission[0] == 0)
            {
                GamePageMissionStar[0].enabled = false;
            }
            if(PlayerMission[1] == 0)
            {
                GamePageMissionStar[1].enabled = false;
            }
        }
        else if(energyManager.workerEnergy >= 40)
        {
            if (PlayerMission[0] == 0)
            {
                GamePageMissionStar[0].enabled = true;
            }
            if (PlayerMission[1] == 0)
            {
                GamePageMissionStar[1].enabled = true;  
            }
        }


        if (energyManager.moneyEnergy < 40)
        {
            if (PlayerMission[0] == 1)
            {
                GamePageMissionStar[0].enabled = false;
            }
            if (PlayerMission[1] == 1)
            {
                GamePageMissionStar[1].enabled = false;
            }
        }
        else if (energyManager.moneyEnergy >= 40)
        {
            if (PlayerMission[0] == 1)
            {
                GamePageMissionStar[0].enabled = true;
            }
            if (PlayerMission[1] == 1)
            {
                GamePageMissionStar[1].enabled = true;
            }
        }


        if (energyManager.clientEnergy < 40)
        {
            if (PlayerMission[0] == 2)
            {
                GamePageMissionStar[0].enabled = false;
            }
            if (PlayerMission[1] == 2)
            {
                GamePageMissionStar[1].enabled = false;
            }
        }
        else if (energyManager.clientEnergy >= 40)
        {
            if (PlayerMission[0] == 2)
            {
                GamePageMissionStar[0].enabled = true;
            }
            if (PlayerMission[1] == 2)
            {
                GamePageMissionStar[1].enabled = true;
            }
        }


        if (energyManager.qualityEnergy < 40)
        {
            if (PlayerMission[0] == 3)
            {
                GamePageMissionStar[0].enabled = false;
            }
            if (PlayerMission[1] == 3)
            {
                GamePageMissionStar[1].enabled = false;
            }
        }
        else if (energyManager.qualityEnergy >= 40)
        {
            if (PlayerMission[0] == 3)
            {
                GamePageMissionStar[0].enabled = true;
            }
            if (PlayerMission[1] == 3)
            {
                GamePageMissionStar[1].enabled = true;
            }
        }



        if (playerConsecutiveAnswers >= 8)
        {
            if (PlayerMission[0] == 4)
            {
                GamePageMissionStar[0].enabled = true;
            }
            if (PlayerMission[1] == 4)
            {
                GamePageMissionStar[1].enabled = true;
            }
            if (PlayerMission[2] == 4)
            {
                GamePageMissionStar[2].enabled = true;
            }

        }

        if (playerAnswersNumber >=10)
        {
            if (PlayerMission[1] == 5)
            {
                GamePageMissionStar[1].enabled = true;
            }
            if (PlayerMission[2] == 5)
            {
                GamePageMissionStar[2].enabled = true;
            }
        }
    }

    public void checkPlayerGood(bool isContinuous)
    {
        if (isContinuous)
        {
            //Boss_SoundManager.instance.PlayCorrect();
            playerConsecutiveAnswers++;
        }
        else{
            //Boss_SoundManager.instance.PlayIncorrect();
            playerConsecutiveAnswers = 0;
        }
    }

    public void FinalUI()
    {
        TimesUpMissionStar[0].enabled = GamePageMissionStar[0].enabled;
        TimesUpMissionStar[1].enabled = GamePageMissionStar[1].enabled;
        TimesUpMissionStar[2].enabled = GamePageMissionStar[2].enabled;

        FinishMissionStar[0].enabled = GamePageMissionStar[0].enabled;
        FinishMissionStar[1].enabled = GamePageMissionStar[1].enabled;
        FinishMissionStar[2].enabled = GamePageMissionStar[2].enabled;
    }
}

