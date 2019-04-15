using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Boss_QuestionControl : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{

    public Text[] AnswerText;
    [HideInInspector]
    public static Boss_QuestionControl instance = null;
    [HideInInspector]
    [SerializeField]
    GameObject rootCanvas = null, originalCanvas;

    [HideInInspector]
    [SerializeField]
    List<GameObject> ColliderObj, ScaleObj;

    [HideInInspector]
    [SerializeField]
    GameObject selectObj;
    [HideInInspector]
    [SerializeField]
    GameObject option;

    [HideInInspector]
    public Text CharacterName;

    [HideInInspector]
    public Text CharacterDescriptionTxt;

    [HideInInspector]
    public Image CharacterIcon;

    [HideInInspector]
    public int Type;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);
    }

        public void UpdateQuestionData(Boss_QusetionData curQD)
    {
        GetComponent<Image>().enabled=true;
        CharacterDescriptionTxt.enabled = true;
        CharacterName.enabled = true;
        int i = 0;
        foreach (var ans in curQD.Answer)
        {
            AnswerText[i].text = ans;
            i++;
        }
        CharacterName.text = curQD.name;
        CharacterDescriptionTxt.text = curQD.CharacterDescription;
        CharacterIcon.sprite = curQD.CharacterIcon;
        Type = (int)curQD.Type;
    }


    Boss_GameManager GameManager;
    Boss_energyManager energyManager;
    private void Start()
    {
        
        GameManager = Boss_GameManager.instance;
        energyManager = Boss_energyManager.instance;
        print(energyManager);
        print(GameManager);
    }

    public void ReplyAnswer(int PlayerAnswerNo)
    {
        Debug.Log("ReplyAnswer: "+ PlayerAnswerNo);
        Boss_MissionManager.instance.playerAnswersNumber++;
        Boss_MissionManager.instance.Check();
        GameManager.NextQusetion();
    }


    bool canAnswer = false;
    int AnswerNo;
    //Drag System
    public void OnDrag(PointerEventData eventData)
    {
        
        GetComponent<RectTransform>().pivot.Set(0.5f, 0.5f);
        transform.position = Input.mousePosition;
        
        
        float tempRot = Mathf.Clamp((transform.position.x - Screen.width/2) / -10, -17, 17);
        transform.localRotation = Quaternion.Euler(0, 0, tempRot);
        RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition, -Vector2.up);
        if (hit.collider != null)
        {
            int index = ColliderObj.IndexOf(hit.collider.gameObject);

            if (hit.collider.gameObject.name == "originalObject")
            {
                index = 10;
                canAnswer = false;
            }

            else if(index >= 0&& index <=6)
            {
                AnswerNo = index;
                canAnswer = true;
            }
            for (int i = 0; i < ColliderObj.Count; i++)
            {
                if (index == i)
                {
                    ScaleObj[i].transform.localScale = new Vector3(1.0845f, 1.0845f, 1.0845f);
                }
                else
                {
                    ScaleObj[i].transform.localScale = new Vector3(1, 1, 1);
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Boss_SoundManager.instance.PlayDrop();
        selectObj.SetActive(true);
        transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);


        transform.parent = rootCanvas.transform;
        option.GetComponent<Button>().interactable = false;
    }
    public void disableDrag()
    {
        this.gameObject.SetActive(false);
        selectObj.SetActive(false);
        transform.localScale = new Vector3(1f, 1f, 1f);
        transform.parent = originalCanvas.transform;
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        foreach (GameObject item in ScaleObj)
        {
            item.transform.localScale = new Vector3(1, 1, 1);
        }
        GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        CharacterDescriptionTxt.enabled = false;
        CharacterName.enabled = false;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        selectObj.SetActive(false);
        transform.localScale = new Vector3(1f, 1f, 1f);
        transform.parent = originalCanvas.transform;
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        foreach (GameObject item in ScaleObj)
        {
            item.transform.localScale = new Vector3(1, 1, 1);
        }
        GetComponent<RectTransform>().localPosition = new Vector3(0,0,0);
        if (canAnswer&& AnswerNo!=10)
        {
            Debug.Log("V");
            ReplyAnswer(AnswerNo);
            GetComponent<Image>().enabled = false;
            CharacterDescriptionTxt.enabled = false;
            CharacterName.enabled = false;
            GetComponent<PlayMakerFSM>().Fsm.SetState("Start");
        }
        else
        {
            Boss_SoundManager.instance.PlayNoDrop();
        }
        option.GetComponent<Button>().interactable = true;
    }
}