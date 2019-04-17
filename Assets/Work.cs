using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum WorkType
{
    Design = 0,
    Programming = 1,
    Testing = 2
}
public class Work : MonoBehaviour
{
    public WorkType type;
    private TextMeshProUGUI text;
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
