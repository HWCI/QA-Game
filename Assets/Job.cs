using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Job : MonoBehaviour
{
    public enum JobType
    {
        A,
        B,
        C
    }

    public JobType jobT;
    public int design;
    public int prgm;
    public Text des_txt;
    public Text prgm_txt;

    private void OnEnable()
    {
        int rnd = Random.Range(4, 8);
        int prnd = Random.Range(1,6);
        design = prnd;
        prgm = Math.Max(rnd - prnd, 1);
        des_txt.text = design.ToString();
        prgm_txt.text = prgm.ToString();
    }

    public void OnClick()
    {
        for(int i = design;i > 0; i--)
        WorkManager.instance.CreateWork(jobT, WorkType.Design);
        for(int i = prgm;i > 0; i--)
        WorkManager.instance.CreateWork(jobT, WorkType.Programming);
        this.gameObject.SetActive(false);
        switch (jobT)
        {
            case JobType.A:
                Boss_GameManager.instance.ACompleted = false;
                break;
            case JobType.B:
                Boss_GameManager.instance.BCompleted = false;
                break;
            case JobType.C:
                Boss_GameManager.instance.CCompleted = false;
                break;
            default:
                break;
        }
    }
}
