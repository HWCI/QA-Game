using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum WorkType
{
    UI = 0,
    Art = 1,
    FrontEnd = 2,
    BackEnd = 3,
    Testing = 4
}
public class Work : MonoBehaviour
{
    public WorkType type;
    private TextMeshProUGUI text;
    public float Completion;

    private void Start()
    {
        text = gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        int ran = Random.Range(0, 4);
        type = (WorkType) ran;
        text.text = type.ToString();
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


    // Update is called once per frame
    void Update()
    {
        
    }
}
