using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Boss_GameManager : MonoBehaviour {
    [HideInInspector]
    public static Boss_GameManager instance = null;
    [HideInInspector]
    public Boss_UIManager uiManager;
    [HideInInspector]
    public Boss_QuestionDataManager QDManager;
    [HideInInspector]
    [SerializeField]
    GameObject option;
    public Boss_energyManager energyManager;
    //Awake is always called before any Start functions
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);

        if (Boss_energyManager.instance == null)
        {
            Boss_energyManager.instance = energyManager;
        }
        uiManager = GetComponent<Boss_UIManager>();
        QDManager = GetComponent<Boss_QuestionDataManager>();
        

        uiManager.GamePage_Canvas.SetActive(false);
        uiManager.Cover_Canvas.SetActive(true);
        uiManager.MainPage_Canvas.SetActive(true);
        //reStart();
    }

    public enum InvestigativeGameStatus
    {
        MainPage,
        Intro,
        Rule,
        Ready,
        Started,
        End,
        Pause 
    }
    //[HideInInspector]
    public InvestigativeGameStatus curGameStatus = InvestigativeGameStatus.MainPage;

    
    [Header("Timing Setting")][Tooltip("3,2,1 Countdown Time.")][SerializeField][Range(0f, 6.0f)]
    float CountdownTime = 3;

    [SerializeField][Tooltip("Next Question Time")][Range(0f, 4.0f)]
    float NextQuestionTime = 3;

    [Header("Score Setting")]
    public int Increase_highestScore;
    public int Increase_secondScore;
    [Space(10)]
    public int Decrease_highestScore;
    public int Decrease_secondScore;

    //Score System
    //[HideInInspector]
    //public int score = 0, AnsweredNo = 0;
    [HideInInspector]
    public Boss_QuestionControl QC;
    [HideInInspector]
    public Boss_TimeManager TM;


    [Header("Comment Setting")]
    //comment
    public string workerEnergyComment;
    public string moneyEnergyComment;
    public string clientEnergyComment;
    public string qualityEnergyComment;

    // Update is called once per frame
    void Update()
    {

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space)&& curGameStatus == InvestigativeGameStatus.Started)
        {
            UpdateQusetion();
        }
#endif

        if (curGameStatus == InvestigativeGameStatus.Started)
        {
            TM.canUpdate = true;

        }
        else
        {
            TM.canUpdate = false;
        }
    }

    public void NextQusetion()
    {
        Invoke("UpdateQusetion", NextQuestionTime);
    }

    public void reStart()
    {
        uiManager.GamePage_Target.SetActive(true);
    }

    void UpdateQusetion()
    {
        
        
    }

    public void ShowTalkingBox(int s)
    {
        if(s == 1 || s == 2)
        {
            Boss_SoundManager.instance.PlayCorrect();
        }
        else if (s == 3 || s == 4)
        {
            Boss_SoundManager.instance.PlayIncorrect();
        }
        for (int i = 0; i < uiManager.talkingbox.Length; i++)
        {
            uiManager.talkingbox[i].SetActive(s == i);

        }
    }





    //Page ctrl


    public void ShowMainPage(bool isOpen)
    {
        uiManager.MainPage_Canvas.SetActive(isOpen);
        if (isOpen)
            curGameStatus = InvestigativeGameStatus.MainPage;
    }

    public void ShowIntro_Canvas(bool isOpen)
    {
        uiManager.Intro_Canvas.SetActive(isOpen);
        if (isOpen)
            curGameStatus = InvestigativeGameStatus.Intro;
    }

    public void ShowRule_Canvas(bool isOpen)
    {
        uiManager.Rule_Canvas.SetActive(isOpen);
        if (isOpen)
            curGameStatus = InvestigativeGameStatus.Rule;
    }

    public void ShowMainPage_ExitBox(bool isOpen)
    {
        uiManager.MainPage_ExitBox.SetActive(isOpen);
    }

    public void ShowGamePage_Canvas(bool isOpen)
    {
        uiManager.GamePage_Canvas.SetActive(isOpen);
        //uiManager.Canvas_CV.SetActive(isOpen);
        if (isOpen)
        {
            //reStart
            curGameStatus = InvestigativeGameStatus.Ready;
            reStart();
            
            //TurnOn_GamePage_Start();
            ShowGamePage_Target(true);
            Boss_SoundManager.instance.SwitchBGMClip(1);
        }
        else
        {
            Boss_SoundManager.instance.SwitchBGMClip(0);
        }

    }

    public void ShowGamePage_ExitBox(bool isOpen)
    {
        uiManager.GamePage_ExitBox.SetActive(isOpen);
        if (isOpen)
        {
            curGameStatus = InvestigativeGameStatus.Pause;
        }
        else
        {
            curGameStatus = InvestigativeGameStatus.Started;
        }
    }

    public void ShowGamePage_Target(bool isOpen)
    {
        uiManager.GamePage_Target.SetActive(isOpen);
        if (isOpen)
        {
            Invoke("TurnOnGamePage_TargetBtn", 1);
        }
        else
        {
            uiManager.GamePage_TargetBtn.SetActive(false);
        }

    }
    void TurnOnGamePage_TargetBtn()
    {

        uiManager.GamePage_TargetBtn.SetActive(true);
    }

    public void TurnOn_GamePage_Start()
    {
        
    }
    void TurnOff_GamePage_Start()
    {
        curGameStatus = InvestigativeGameStatus.Started;
        uiManager.GamePage_Start.GetComponent<Animator>().ResetTrigger("TurnOn");
        uiManager.GamePage_Start.SetActive(false);

        //Boss_MissionManager.instance.Check();
    }

    void StartGame()
    {
        
    }

    public void ShowGameEnd_TimeUp(bool isOpen)
    {
        if (isOpen)
        {
            //GameEnd;
            uiManager.GamePage_TimesUp.SetActive(isOpen);

            //InvestigativeSoundManager.instance.PlayTimesUp();
            curGameStatus = InvestigativeGameStatus.End;
            Invoke("ShowNextPaga_btn", 1);
        }
        else
        {
            uiManager.GamePage_TimesUp.SetActive(isOpen);
        }

    }
    

    public void ShowGameEnd_Finish(bool isOpen)
    {
        if (isOpen)
        {
            //WinGame;
            uiManager.GamePage_Finish.SetActive(isOpen);
            //InvestigativeSoundManager.instance.PlayFinish();
            curGameStatus = InvestigativeGameStatus.End;
            Invoke("ShowNextPaga_btn", 1);
        }
        else
        {
            uiManager.GamePage_Finish.SetActive(isOpen);
        }
    }
    public IEnumerator ShowGameOverText(string text)
    {
        yield return new WaitForSeconds(1f);
        if(text == "生產力")
        {
            uiManager.GameOverText.text = workerEnergyComment;
        }
        else if (text == "資金")
        {
            uiManager.GameOverText.text = moneyEnergyComment;
        }
        else if (text == "客戶")
        {
            uiManager.GameOverText.text = clientEnergyComment;
        }
        else if (text == "品質")
        {
            uiManager.GameOverText.text = qualityEnergyComment;
        }
        ShowGamePage_GameOver(true);
    }
    public void ShowGamePage_GameOver(bool isOpen)
    {
        if (isOpen )//&& !uiManager.GamePage_GameOver.active
        {

            Boss_SoundManager.instance.PlayNoEnergy();
            //GameEnd;
            uiManager.GamePage_GameOver.SetActive(true);

            //InvestigativeSoundManager.instance.PlayTimesUp();
            curGameStatus = InvestigativeGameStatus.End;
            Invoke("ShowNextPaga_btn", 1);
        }
        else
        {
            uiManager.GamePage_GameOver.SetActive(false);
        }

    }

    void ShowNextPaga_btn()
    {
        uiManager.NextPaga_btn.SetActive(true);
    }

    //Scene Ctrl
    public void SwitchScene(string SceneName)
    {
        if(SceneName != null && SceneName != "")
        {
            SceneManager.LoadScene(SceneName);
        }
        else
        {
            Debug.Log("No Scene?");
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        Debug.Log("Quit Game");
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}