using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkManager : MonoBehaviour
{
    public List<Work> works;
    public GameObject DesignJob;
    public GameObject PrgmJob;
    public GameObject TestJob;
    public int AJ;
    public int BJ;
    public int CJ;
    public int ADJ;
    public int ATJ;
    public int APJ;
    public int BDJ;
    public int BTJ;
    public int BPJ;
    public int CDJ;
    public int CTJ;
    public int CPJ;
    public GameObject root;
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
            newjob.GetComponent<Work>().type = WorkType.Design;
            if (job == Job.JobType.A)
            {
                ADJ++;
                AJ++;
            }if (job == Job.JobType.B)
            {
                BDJ++;
                BJ++;
            }if (job == Job.JobType.C)
            {
                CDJ++;
                CJ++;
            }
        }

        if (workt == WorkType.Programming)
        {
            GameObject newjob = Instantiate(PrgmJob, this.transform);
            newjob.GetComponent<Work>().job = job;
            newjob.GetComponent<Work>().type = WorkType.Programming;
            if (job == Job.JobType.A)
            {
                APJ++;
                AJ++;
            }if (job == Job.JobType.B)
            {
                BPJ++;
                BJ++;
            }if (job == Job.JobType.C)
            {
                CPJ++;
                CJ++;
            }
        }

        if (workt == WorkType.Testing)
        {
            GameObject newjob = Instantiate(TestJob, this.transform);
            newjob.GetComponent<Work>().job = job;
            newjob.GetComponent<Work>().type = WorkType.Testing;
            if (job == Job.JobType.A)
            {
                ATJ++;
                //AJ++;
            }if (job == Job.JobType.B)
            {
                BTJ++;
                //BJ++;
            }if (job == Job.JobType.C)
            {
                CTJ++;
                //CJ++;
            }
        }
    }

    public void CompleteWork(Job.JobType job, WorkType workt)
    {
        if (workt == WorkType.Design)
        {
            if (job == Job.JobType.A)
            {
                ADJ--;
            }if (job == Job.JobType.B)
            {
                BDJ--;
            }if (job == Job.JobType.C)
            {
                CDJ--;
            }
        }
        if (workt == WorkType.Programming)
        {
            if (job == Job.JobType.A)
            {
                APJ--;
            }if (job == Job.JobType.B)
            {
                BPJ--;
            }if (job == Job.JobType.C)
            {
                CPJ--;
            }
        }
        if (workt == WorkType.Testing)
        {
            if (job == Job.JobType.A)
            {
                ATJ--;
            }if (job == Job.JobType.B)
            {
                BTJ--;
            }if (job == Job.JobType.C)
            {
                CTJ--;
            }
        }

        if (ADJ == 0 && APJ == 0 && ATJ == 0)
        {
            Boss_GameManager.instance.ACompleted = true;
            Boss_GameManager.instance.score += AJ * 100;
        } if (BDJ == 0 && BPJ == 0 && BTJ == 0)
        {
            Boss_GameManager.instance.BCompleted = true;
            Boss_GameManager.instance.score += BJ * 100;
        } if (CDJ == 0 && CPJ == 0 && CTJ == 0)
        {
            Boss_GameManager.instance.CCompleted = true;
            Boss_GameManager.instance.score += CJ * 100;
        }
    }

    public int GetPenalty()
    {
        int pen = 0;
        if (ADJ != 0 || APJ != 0 || ATJ != 0)
        {
            pen += AJ * 40;
        }

        if (BDJ != 0 || BPJ != 0 || BTJ != 0)
        {
            pen += BJ * 40;
        }

        if (CDJ != 0 || CPJ != 0 || CTJ != 0)
        {
            pen += CJ * 40;
        }

        return pen;
    }
}
