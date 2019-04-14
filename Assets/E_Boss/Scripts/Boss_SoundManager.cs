using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_SoundManager : MonoBehaviour {
    public static Boss_SoundManager instance = null;
    [SerializeField]
    GameObject SFX_Pref;

    [Header("Background music")]
    [SerializeField]
    AudioSource BGM;
    public AudioClip OtherBGM_clip;
    public AudioClip GamePageBGM_clip;

    [Space(15)]
    [Header("Correct")]
    [SerializeField]
    AudioClip correct_clip;
    [SerializeField]
    [Range(0f, 1f)]
    float correct_soundVolume;

    [Header("Incorrect")]
    [SerializeField]
    AudioClip incorrect_clip;
    [SerializeField]
    [Range(0f, 1f)]
    float incorrect_soundVolume;

    [Space(15)]
    [Header("TimesUp")]
    [SerializeField]
    AudioClip TimesUp_clip;
    [SerializeField]
    [Range(0f, 1f)]
    float TimesUp_soundVolume;

    [Header("Finish")]
    [SerializeField]
    AudioClip Finish_clip;
    [SerializeField]
    [Range(0f, 1f)]
    float Finish_soundVolume;

    [Header("NoEnergy")]
    [SerializeField]
    AudioClip NoEnergy_clip;
    [SerializeField]
    [Range(0f, 1f)]
    float NoEnergy_soundVolume;

	[Header("CountDown")]
	[SerializeField]
	AudioClip CountDown_clip;
	[SerializeField]
	[Range(0f, 1f)]
	float CountDown_soundVolume;

	[Header("Beep")]
	[SerializeField]
	AudioClip Beep_clip;
	[SerializeField]
	[Range(0f, 1f)]
	float Beep_soundVolume;

    [Space(15)]
    [Header("UI Button")]
    [SerializeField]
    AudioClip btn_clip;
    [SerializeField]
    [Range(0f, 1f)]
    float btn_soundVolume;

    [Space(15)]
    [Header("Drop")]
    [SerializeField]
    AudioClip Drop_clip;
    [SerializeField]
    [Range(0f, 1f)]
    float Drop_soundVolume;

    [Space(15)]
    [Header("No_Drop")]
    [SerializeField]
    AudioClip No_Drop_clip;
    [SerializeField]
    [Range(0f, 1f)]
    float No_Drop_soundVolume;
    //------------------------------
    List<GameObject> m_availableObjects = new List<GameObject>();


    private void Awake()
    {
        BGM.clip = OtherBGM_clip;
        BGM.Play();
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);
    }

    public void SwitchBGMClip(int no)
    {
        if (no == 1)
        {
            BGM.clip = GamePageBGM_clip;
            BGM.Play();
        }
        else
        {
            BGM.clip = OtherBGM_clip;
            BGM.Play();
        }
    }
    public void PlayCorrect()
    {
        StartCoroutine(PlaySoundCoroutine(correct_clip, correct_soundVolume));
    }
    public void PlayIncorrect()
    {
        StartCoroutine(PlaySoundCoroutine(incorrect_clip, incorrect_soundVolume));
    }
    public void PlayTimesUp()
    {
        StartCoroutine(PlaySoundCoroutine(TimesUp_clip, TimesUp_soundVolume));
    }
    public void PlayFinish()
    {
        StartCoroutine(PlaySoundCoroutine(Finish_clip, Finish_soundVolume));
    }
    public void PlayNoEnergy()
    {
        StartCoroutine(PlaySoundCoroutine(NoEnergy_clip, NoEnergy_soundVolume));
	}
	public void PlayCountDown()
	{
		StartCoroutine(PlaySoundCoroutine(CountDown_clip, CountDown_soundVolume));
	}
	public void PlayBeep()
	{
		StartCoroutine(PlaySoundCoroutine(Beep_clip, Beep_soundVolume));
	}
    public void PlayBtn()
    {
        StartCoroutine(PlaySoundCoroutine(btn_clip, btn_soundVolume));
    }
    public void PlayDrop()
    {
        StartCoroutine(PlaySoundCoroutine(Drop_clip, Drop_soundVolume));
    }
    public void PlayNoDrop()
    {
        StartCoroutine(PlaySoundCoroutine(No_Drop_clip, No_Drop_soundVolume));
    }
    IEnumerator PlaySoundCoroutine(AudioClip audioClip, float volume)
    {
        int lastIndex = m_availableObjects.Count - 1;
        GameObject SFX;
        if (lastIndex >= 0)
        {
            SFX = m_availableObjects[lastIndex];
            m_availableObjects.RemoveAt(lastIndex);
            SFX.SetActive(true);
        }
        else
        {
            SFX = Instantiate<GameObject>(SFX_Pref, this.transform);
        }
        AudioSource SFX_AudioSource = SFX.GetComponent<AudioSource>();
        SFX_AudioSource.volume = volume;
        SFX_AudioSource.PlayOneShot(audioClip);
        yield return new WaitForSeconds(audioClip.length);
        BackToPool(SFX);
    }
    public void BackToPool(GameObject go)
    {
        lock (m_availableObjects)
        {
            m_availableObjects.Add(go);
            go.SetActive(false);
        }
    }


}
