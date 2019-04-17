using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffManager : MonoBehaviour
{
    public List<Worker> workers;
    public static StaffManager instance;
    public Worker[] uiworker;
    private void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);
    }

    public void AddWorker(Worker w)
    {
        workers.Add(w);
    }

    public void SetWorkers()
    {
        int i = 0;
        foreach (var worker in workers)
        {
            uiworker[i].face = worker.face;
            uiworker[i].A_Ability = worker.A_Ability;
            uiworker[i].P_Ability = worker.P_Ability;
            uiworker[i].T_Ability = worker.T_Ability;
        }
    }
}
