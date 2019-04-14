using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Boss_TimeManager : MonoBehaviour {
    [HideInInspector]
    public float changeToOrangeAt = 50;
    [HideInInspector]
    public float changeToRedAt = 10;
    [HideInInspector]
    public Sprite time_bule, time_orange, time_red;

    [HideInInspector]
    public float Timer;
    [HideInInspector]
    public GameObject timeBar;
    [HideInInspector]
    public Text timeText;
    [HideInInspector]
    public float MaxTime;
    [HideInInspector]
    public bool canUpdate;
    public void ReSet()
    {
        canUpdate = false;
        Timer = MaxTime;
        TimerChange();
    }
    private void Update()
    {
        if (canUpdate&& Timer>0)
        {
            Timer -= Time.deltaTime;
            TimerChange();
            if (Timer <= 0)
            {
                Timer = 0;
                timeText.text = "00:00";
                Boss_SoundManager.instance.PlayTimesUp();
                Boss_QuestionControl.instance.disableDrag();
                canUpdate = false;
                Boss_MissionManager.instance.FinalUI();
                Boss_GameManager.instance.curGameStatus = Boss_GameManager.InvestigativeGameStatus.End;
                Boss_GameManager.instance.ShowGameEnd_TimeUp(true);
            }
        }

    }
    public void TimerChange()
    {
        timeBar.GetComponent<Image>().fillAmount = Timer / MaxTime;
        int minutes = Mathf.FloorToInt(Timer / 60F);
        int seconds = Mathf.FloorToInt(Timer - minutes * 60);
        string niceTime = string.Format("{0:00}:{1:00}", minutes, seconds);
        timeText.text = niceTime;
        if (Timer / MaxTime * 100 <= changeToRedAt)
            timeBar.GetComponent<Image>().sprite = time_red;
        else if (Timer / MaxTime * 100 > changeToRedAt && Timer / MaxTime * 100 <= changeToOrangeAt)
            timeBar.GetComponent<Image>().sprite = time_orange;
        else
            timeBar.GetComponent<Image>().sprite = time_bule;
    }
}
