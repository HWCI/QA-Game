using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffManager : MonoBehaviour
{
    public List<Worker> workers;
    public static StaffManager instance;
    public Worker[] uiworker;
    int i = 0;
    private void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);
    }


    public void SetWorkers(Sprite face,int A, int P, int T)
    {
         uiworker[i].face = face;
         uiworker[i].A_Ability = A;
         uiworker[i].P_Ability = P;
         uiworker[i].T_Ability = T;
         uiworker[i].Refresh();
         i++;
    }

    public void DoWork()
    {
        foreach (var worker in uiworker)
        {
            worker.Work();
        }
    }
}
