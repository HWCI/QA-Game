using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace creXa.GameBase
{
    [AddComponentMenu("creXa/Base/Audio")]
    public class ZAudio2D : ZSingleton<ZAudio2D>
    {

        [Range(0, 1)]
        public float defaultBGMVolume = 1;
        [Range(0, 1)]
        public float defaultSFXVolume = 1;
        [Range(0, 1)]
        public float defaultVOVolume = 1;

        public bool defaultBGMOn = true;
        public bool defaultSFXOn = true;
        public bool defaultVOOn = true;

        public GameObject BGSrc;
        public GameObject FXSrc;
        public GameObject VOSrc;

        [ReadOnly][SerializeField] float _BGMVolume;
        [ReadOnly][SerializeField]  float _SFXVolume;
        [ReadOnly][SerializeField]  float _VOVolume;

        [ReadOnly][SerializeField]  bool _BGMOn;
        [ReadOnly][SerializeField]  bool _SFXOn;
        [ReadOnly][SerializeField]  bool _VOOn;

        public bool autoSetup = true;

        #region Setters/Getters
        public float BGMVolume { set { SetBGMVolume(value); } get { return _BGMVolume; } }
        public float SFXVolume { set { SetSFXVolume(value); } get { return _SFXVolume; } }
        public float VOVolume { set { SetVOVolume(value); } get { return _VOVolume; } }

        public bool BGMOn { set { SetBGMOn(value); } get { return _BGMOn; } }
        public bool SFXOn { set { SetSFXOn(value); } get { return _SFXOn; } }
        public bool VOOn { set { SetVOOn(value); } get { return _VOOn; } }
        #endregion

        protected override void AwakeRun()
        {
            if (!BGSrc && !autoSetup) Debug.LogWarning("BGSrc not assigned.");
            if (!FXSrc && !autoSetup) Debug.LogWarning("FXSrc not assigned.");
            if (!VOSrc && !autoSetup) Debug.LogWarning("VOSrc not assigned.");
        }


        void OnEnable()
        {
            SceneManager.sceneLoaded += AutoSetup;
        }

        void OnDisable()
        {
            SceneManager.sceneLoaded -= AutoSetup;
        }

        void Start()
        {
            //Sound Sys Init
            BGMVolume = ZBase.It.isGetPresetFromPlayerPref ? PlayerPrefs.GetFloat("ZBGMVolume", defaultBGMVolume) : defaultBGMVolume;
            SFXVolume = ZBase.It.isGetPresetFromPlayerPref ? PlayerPrefs.GetFloat("ZSFXVolume", defaultSFXVolume) : defaultSFXVolume;
            BGMOn = ZBase.It.isGetPresetFromPlayerPref ? (PlayerPrefsX.GetBool("ZBGMOn", defaultBGMOn)) : defaultBGMOn;
            SFXOn = ZBase.It.isGetPresetFromPlayerPref ? (PlayerPrefsX.GetBool("ZSFXOn", defaultSFXOn)) : defaultSFXOn;
            VOOn = ZBase.It.isGetPresetFromPlayerPref ? (PlayerPrefsX.GetBool("ZVOOn", defaultVOOn)) : defaultVOOn;
        }

        #region Set Function
        private void SetBGMVolume(float vol)
        {
            _BGMVolume = Mathf.Clamp(vol, 0, 1);
            UpdateBGM();
        }

        private void SetSFXVolume(float vol)
        {
            _SFXVolume = Mathf.Clamp(vol, 0, 1);
            UpdateSFX();
        }

        private void SetVOVolume(float vol)
        {
            _VOVolume = Mathf.Clamp(vol, 0, 1);
            UpdateVO();
        }

        private void SetBGMOn(bool on)
        {
            _BGMOn = on;
            UpdateBGM();
        }

        private void SetSFXOn(bool on)
        {
            _SFXOn = on;
            UpdateSFX();
        }

        private void SetVOOn(bool on)
        {
            _VOOn = on;
            UpdateVO();
        }
        #endregion

        #region Update AudioSrc
        private void UpdateBGM()
        {
            if (!BGSrc) return;
            AudioSource[] srcs;
            srcs = BGSrc.GetComponents<AudioSource>();
            for (int i = 0; i < srcs.Length; i++)
                if (srcs[i] != null)
                {
                    srcs[i].volume = _BGMVolume;
                    srcs[i].mute = !_BGMOn;
                }
        }

        private void UpdateSFX()
        {
            if (!FXSrc) return;
            AudioSource[] srcs;
            srcs = FXSrc.GetComponents<AudioSource>();
            for (int i = 0; i < srcs.Length; i++)
                if (srcs[i] != null)
                {
                    srcs[i].volume = _SFXVolume;
                    srcs[i].mute = !_SFXOn;
                }
        }

        private void UpdateVO()
        {
            if (!VOSrc) return;
            AudioSource[] srcs;
            srcs = VOSrc.GetComponents<AudioSource>();
            for (int i = 0; i < srcs.Length; i++)
                if (srcs[i] != null)
                {
                    srcs[i].volume = _VOVolume;
                    srcs[i].mute = !_VOOn;
                }
        }
        #endregion

        #region Play Function
        public void PlayBGM(AudioClip clip)
        {
            if (!BGSrc) return;
            AudioSource src = null;
            AudioSource[] srcs;
            srcs = BGSrc.GetComponents<AudioSource>();
            if (srcs.Length > 0)
            {
                for (int i = 0; i < srcs.Length; i++)
                    if (srcs[i] != null)
                        DestroyImmediate(srcs[i]);
            }

            src = BGSrc.AddComponent<AudioSource>();

            if (src)
            {
                if (src.clip == clip) return;
                src.mute = !_BGMOn;
                src.clip = clip;
                src.loop = true;
                src.Play();
            }
        }

        public AudioSource PlaySFX(AudioClip clip, float duration = -1, float vol = -1, bool loop = false)
        {
            if (!FXSrc) return null;
            AudioSource src = FXSrc.AddComponent<AudioSource>();
            src.clip = clip;
            src.mute = !_SFXOn;
            src.loop = loop;
            src.volume = vol >= 0 ? vol : _SFXVolume;
            src.Play();
            if (!loop) Destroy(src, duration > 0 ? duration : clip.length + 0.2f);
            return src;
        }

        public AudioSource PlayVO(AudioClip clip, float duration = -1, float vol = -1, bool loop = false)
        {
            if (!VOSrc) return null;
            AudioSource src = VOSrc.AddComponent<AudioSource>();
            src.clip = clip;
            src.mute = !_VOOn;
            src.loop = false;
            src.volume = vol >= 0 ? vol : _VOVolume;
            src.Play();
            if (!loop) Destroy(src, duration > 0 ? duration : clip.length + 0.2f);
            return src;
        }

        public void CrossFadeBGM(AudioClip clip, float duration = 1.0f, float cutin = 0.5f)
        {
            if (!BGSrc) return;
            StartCoroutine(CrossFadeBGMIE(clip, duration, cutin));
        }

        public void FadeOut(AudioSource src, float duration)
        {
            if (src) StartCoroutine(FadeOutIE(src, duration));
        }

        private IEnumerator FadeOutIE(AudioSource src, float duration)
        {
            yield return StartCoroutine(VolumneTween(src, BGMVolume, 0, duration / 2));
            if (src) Destroy(src);
        }

        private IEnumerator CrossFadeBGMIE(AudioClip clip, float duration, float cutin)
        {
            AudioSource oldsrc, newsrc;
            oldsrc = BGSrc.GetComponent<AudioSource>();
            StartCoroutine(VolumneTween(oldsrc, BGMVolume, 0, duration / 2));
            yield return new WaitForSeconds(cutin);
            newsrc = BGSrc.AddComponent<AudioSource>();
            newsrc.clip = clip;
            newsrc.loop = true;
            newsrc.Play();
            yield return StartCoroutine(VolumneTween(newsrc, 0, BGMVolume, duration / 2));
            if (oldsrc) Destroy(oldsrc);
        }

        public IEnumerator VolumneTween(AudioSource src, float startv, float destv, float duration)
        {
            float timer = 0.0f;
            src.volume = startv;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                src.volume = startv + (destv - startv) * timer / duration;
                yield return null;
            }
            src.volume = destv;
        }

        #endregion

        #region Stop Function
        public void StopAllBGM()
        {
            if (!BGSrc) return;
            AudioSource[] srcs;
            srcs = BGSrc.GetComponents<AudioSource>();
            for (int i = 0; i < srcs.Length; i++)
                if (srcs[i] != null)
                    srcs[i].Stop();
        }

        public void StopAllSFX()
        {
            if (!FXSrc) return;
            AudioSource[] srcs;
            srcs = FXSrc.GetComponents<AudioSource>();
            for (int i = 0; i < srcs.Length; i++)
                if (srcs[i] != null)
                    Destroy(srcs[i]);
        }

        public void StopAllVO()
        {
            if (!VOSrc) return;
            AudioSource[] srcs;
            srcs = VOSrc.GetComponents<AudioSource>();
            for (int i = 0; i < srcs.Length; i++)
                if (srcs[i] != null)
                    Destroy(srcs[i]);
        }
        #endregion

        public void SetupRootsToMainCamera()
        {
            SetupRootsTo(Camera.main.gameObject);
        }

        public void SetupRootsTo(GameObject obj)
        {
            ClearAllRoots();
            BGSrc = new GameObject("BGSrc");
            BGSrc.transform.SetParent(obj ? obj.transform : null);
            BGSrc.transform.localScale = Vector3.one;
            BGSrc.transform.localPosition = Vector3.zero;

            FXSrc = new GameObject("FXSrc");
            FXSrc.transform.SetParent(obj ? obj.transform : null);
            FXSrc.transform.localScale = Vector3.one;
            FXSrc.transform.localPosition = Vector3.zero;

            VOSrc = new GameObject("VOSrc");
            VOSrc.transform.SetParent(obj ? obj.transform : null);
            VOSrc.transform.localScale = Vector3.one;
            VOSrc.transform.localPosition = Vector3.zero;
        }

        public void ClearAllRoots()
        {
            if (BGSrc) DestroyImmediate(BGSrc);
            if (FXSrc) DestroyImmediate(FXSrc);
            if (VOSrc) DestroyImmediate(VOSrc);
            BGSrc = null;
            FXSrc = null;
            VOSrc = null;
        }

        void AutoSetup(Scene scene, LoadSceneMode mode)
        {
            Debug.Log("AutoSetup");
            if (autoSetup)
            {
                ClearAllRoots();
                SetupRootsToMainCamera();
            }
        }
    }
}
