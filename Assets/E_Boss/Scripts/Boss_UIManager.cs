using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Boss_UIManager : MonoBehaviour {
    public GameObject Cover_Canvas,MainPage_Canvas, Intro_Canvas, Rule_Canvas, GamePage_Canvas, Canvas_CV;
    [Space(15)]
    public GameObject MainPage_ExitBox;
    [Space(15)]
    public GameObject GamePage_Target;
    public GameObject GamePage_Start;
    public GameObject GamePage_Finish;
    public GameObject GamePage_TimesUp;
    public GameObject GamePage_GameOver;
    public GameObject GamePage_ExitBox;
    public GameObject GamePage_Staff;
    public GameObject GamePage_Jobs;
    [Space(15)]
    public Text GameOverText;
    [Space(15)]
    public GameObject GamePage_TargetBtn;
    public GameObject NextPaga_btn;
    [Space(15)]
    public GameObject[] talkingbox;

}
