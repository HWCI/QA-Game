using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkManager : MonoBehaviour
{
    public List<Work> works;
    public GameObject DesignJob;
    public GameObject PrgmJob;
    public GameObject TestJob;
    public int ADJ;
    public int ATJ;
    public int APJ;
    public int BDJ;
    public int BTJ;
    public int BPJ;
    public int CDJ;
    public int CTJ;
    public int CPJ;
    public static WorkManager instance;
    
    private void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);
    }


    public void CreateWork(Job.JobType job, WorkType workt)
    {
        if (workt == WorkType.Design)
        {
            GameObject newjob = Instantiate(DesignJob, this.transform);
            newjob.GetComponent<Work>().job = job;
            newjob.GetComponent<Work>().type = workt;
            if (job == Job.JobType.A)
            {
                ADJ++;
            }if (job == Job.JobType.B)
            {
                BDJ++;
            }if (job == Job.JobType.C)
            {
                CDJ++;
            }
        }

        if (workt == WorkType.Programming)
        {
            GameObject newjob = Instantiate(PrgmJob, this.transform);
            newjob.GetComponent<Work>().job = job;
            newjob.GetComponent<Work>().type = workt;
            if (job == Job.JobType.A)
            {
                APJ++;
            }if (job == Job.JobType.B)
            {
                BPJ++;
            }if (job == Job.JobType.C)
            {
                CPJ++;
            }
        }

        if (workt == WorkType.Testing)
        {
            GameObject newjob = Instantiate(TestJob, this.transform);
            newjob.GetComponent<Work>().job = job;
            newjob.GetComponent<Work>().type = workt;
            if (job == Job.JobType.A)
            {
                ATJ++;
            }if (job == Job.JobType.B)
            {
                BTJ++;
            }if (job == Job.JobType.C)
            {
                CTJ++;
            }
        }
    }
}
