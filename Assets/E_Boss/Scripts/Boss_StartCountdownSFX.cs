using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_StartCountdownSFX : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayStartCountdownSFX()
    {
        Boss_SoundManager.instance.PlayCountDown();
    }
}
