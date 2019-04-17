using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.UIElements.StyleSheets;
using UnityEngine.UI;

public enum WorkType
{
    Design = 0,
    Programming = 1,
    Testing = 2
}
public class Work : MonoBehaviour
{
    public WorkType type;
    public Job.JobType job;
    private TextMeshProUGUI text;
    public Image BG;
    public float Completion;

    private void Start()
    {
    }

    private void OnEnable()
    {
        text = gameObject.GetComponentInChildren<TextMeshProUGUI>();

        int ran = Random.Range(0, 4);
        type = (WorkType) ran;
        text.text = type.ToString();
        Created();
    }

    public void WorkedOn(float amount)
    {
        Completion += amount;
        if (Completion >= 100)
        {
            Completed();
        }
    }

    public void Completed()
    {
        
    }

    public void Defect()
    {
        
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

    }
}
