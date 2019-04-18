using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.UIElements.StyleSheets;
using UnityEngine.UI;

public enum WorkType
{
    Design = 0,
    Programming = 1,
    Testing = 2
}
public class Work : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public WorkType type;
    public Job.JobType job;
    public TextMeshProUGUI text;
    public TextMeshProUGUI pertext;
    public Text defectTxt;
    public Image BG;
    public bool working;
    public float Completion;
    public GameObject hitObject;
    [HideInInspector]
    [SerializeField]
    GameObject rootCanvas = null, originalCanvas;

    private void Start()
    {
        rootCanvas = WorkManager.instance.root;
    }

    private void OnEnable()
    {
        //int ran = Random.Range(0, 4);
        //type = (WorkType) ran;
        //text.text = type.ToString();
        //Created();
    }

    public void WorkedOn(float amount)
    {
        Completion += amount;
        

        if (Completion >= 100)
        {
            Completed();
        }
        else
         {int ran = Random.Range(0, 100);
             if (ran > 70+amount*0.2)
                     {
                         Defect();
                     }
         }
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        
        GetComponent<RectTransform>().anchorMin.Set(0.5f, 0.5f);
        GetComponent<RectTransform>().anchorMax.Set(0.5f, 0.5f);
        GetComponent<RectTransform>().pivot.Set(0.5f, 0.5f);
        transform.position = Input.mousePosition;
        
        
        //float tempRot = Mathf.Clamp((transform.position.x - Screen.width/2) / -10, -17, 17);
        //transform.localRotation = Quaternion.Euler(0, 0, tempRot);
        RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition, -Vector2.up);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.name == "WorkArea")
            {
                hitObject = hit.collider.gameObject;
            }
        }
        else
        {
            hitObject = null;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (working)
        {
            hitObject.GetComponentInParent<Worker>().currentWorks.Remove(this);
            working = false;
        }
        Boss_SoundManager.instance.PlayDrop();
        //selectObj.SetActive(true);
        //transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);


        transform.parent = rootCanvas.transform;
        //option.GetComponent<Button>().interactable = false;
    }
    public void disableDrag()
    {
        this.gameObject.SetActive(false);
        //selectObj.SetActive(false);
        //transform.localScale = new Vector3(1f, 1f, 1f);
        transform.parent = WorkManager.instance.transform;
        //transform.localRotation = Quaternion.Euler(0, 0, 0);
        //foreach (GameObject item in ScaleObj)
        //{
        //    item.transform.localScale = new Vector3(1, 1, 1);
        //}
        //GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        //CharacterDescriptionTxt.enabled = false;
        //CharacterName.enabled = false;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        //selectObj.SetActive(false);
        //transform.localScale = new Vector3(1f, 1f, 1f);
        if (hitObject != null)
        {
            if (hitObject.GetComponentInParent<Worker>().currentWorks.ToList().Count < 2)
            {
                transform.parent = hitObject.transform;
                hitObject.GetComponentInParent<Worker>().currentWorks.Add(this);
                working = true;
            }else
            {
                transform.parent = WorkManager.instance.transform;
            }
        }
        else
        {
            transform.parent = WorkManager.instance.transform;
        }
        //transform.localRotation = Quaternion.Euler(0, 0, 0);
        //foreach (GameObject item in ScaleObj)
        //{
        //    item.transform.localScale = new Vector3(1, 1, 1);
        //}
        //GetComponent<RectTransform>().localPosition = new Vector3(0,0,0);
        
    }

    public void Completed()
    {
        WorkManager.instance.CompleteWork(job, type);
        GetComponentInParent<Worker>().currentWorks.Remove(this);
        Destroy(this.gameObject);
    }

    public void Defect()
    {
        WorkManager.instance.CreateWork(job, WorkType.Testing);
        defectTxt.gameObject.SetActive(true);
        Invoke("CloseDefect", 5f);
    }

    public void CloseDefect()
    {
        defectTxt.gameObject.SetActive(false);
        
    }

    public void Created()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        if(job == Job.JobType.A)
                        GetComponent<Image>().color = new Color(1,0.2877358f,0.2939443f,1);
                if(job == Job.JobType.B)
                        GetComponent<Image>().color = new Color(0.3555822f,1,0.2862746f,1);
                if(job == Job.JobType.C)
                        GetComponent<Image>().color = new Color(0.2862746f,0.438413f,1,1);

                pertext.text = Completion.ToString();

    }
}
