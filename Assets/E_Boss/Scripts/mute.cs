using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mute : MonoBehaviour {

    public Image ButtonImage;
    public Sprite muteImageOn;
    public Sprite muteImageOff;

    public void muteGame()
    {
        if(AudioListener.volume == 1f)
        {
            AudioListener.volume = 0f;
            ButtonImage.sprite = muteImageOff;

        }
        else
        {
            AudioListener.volume = 1f;
            ButtonImage.sprite = muteImageOn;

        }
    }
}
