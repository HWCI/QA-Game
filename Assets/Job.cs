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
        int rnd = Random.Range(0, 8);
        int prnd = Random.Range(0,6);
        design = rnd;
        prgm = Math.Abs(8 - rnd);
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
    }
}
