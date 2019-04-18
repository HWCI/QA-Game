using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Candid : MonoBehaviour
{
    public SelectWorker total;
    public Text stat;
    public Image photo;
    private int A_value;
    private int P_value;
    private int T_value;
    private void OnEnable()
    {
        Get_Value();
    }

    private void Get_Value()
    {
        int ran_A = Random.Range(1, 10);
        int ran_P = Random.Range(1, 10);
        int ran_T = Random.Range(1, 10);

            A_value = ran_A;
            P_value = ran_P;
            T_value = ran_T;

            if (ran_A + ran_T + ran_P < 12)
            {
                Get_Value();
            }
            else
            {

                stat.text = "D:" + A_value + " P:" + P_value + " T:" + T_value;
            }
    }

    public void Selected()
    {
        total.picked();
        StaffManager.instance.SetWorkers(photo.sprite,A_value,P_value,T_value);
        this.gameObject.SetActive(false);
    }
}
