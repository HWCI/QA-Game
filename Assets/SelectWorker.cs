using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectWorker : MonoBehaviour
{
    public int selected;

    public void picked()
    {
        selected++;
        if (selected >= 5)
        {
            Boss_GameManager.instance.ShowGamePage_Jobs(true);
            Boss_GameManager.instance.ShowGamePage_Staff(false);
        }
    }

}
