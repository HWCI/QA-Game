using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_CountDownSFX : MonoBehaviour {

	public void PlayCountDown(){
		Boss_SoundManager.instance.PlayCountDown ();
	}

	public void PlayBeep(){
		Boss_SoundManager.instance.PlayBeep ();
	}
}
