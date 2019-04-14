using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

/**
    @author: Louiszen
    @update: 20171012
**/

namespace creXa.GameBase
{
    /// <summary>
    /// ZBase class works as a base architecture for every game, providing useful base settings and algorithm
    /// </summary>
    [AddComponentMenu("creXa/ZBase")]
    public sealed class ZBase : ZSingleton<ZBase>
    {
        #region Basic Setting

        //Version
        /// <summary>
        ///  The name of this game.
        /// </summary>
        [Header("Game Info")]
        public string GameName = "Default";
        /// <summary>
        /// The specified version for version checking.
        /// </summary>
        [Regex(@"^(?:(\d)+\.)*(\d)+$", "Version Format Not Compatible For Checking")]
        public string Version = "1.0.0";
        /// <summary>
        /// Deviation of version.
        /// </summary>
        public int Deviation = 0;

        //App Basic
        /// <summary>
        /// Resolution (Width).
        /// </summary>
        [Header("Resolution")]
        public float ResolutionW = 16.0f;
        /// <summary>
        /// Resolution (Height).
        /// </summary>
        public float ResolutionH = 9.0f;
        /// <summary>
        /// Aspect Ratio
        /// </summary>
        public float Resolution { get { return ResolutionW / ResolutionH; } }

        //Network
        /// <summary>
        /// Does this game need server connection for multiple online players?
        /// </summary>
        [Header("Network")]
        public bool isNetworkGame = false;
        /// <summary>
        /// Server Connection Port.
        /// </summary>
        public int Port = 28940;
        /// <summary>
        /// Server IP.
        /// </summary>
        [Regex(@"^(?:\d{1,3}\.){3}\d{1,3}$", "Invalid IP address!\nExample: '127.0.0.1'")]
        public string ServerIP = "127.0.0.1";
        /// <summary>
        /// Default connection time out duration.
        /// </summary>
        public float defTimeOut = 30;

        //Link Setting
        /// <summary>
        /// Server Domain.
        /// </summary>
        [Header("Linkages")]
        public string Domain = "";
        /// <summary>
        /// Server Domain For Debug
        /// </summary>
        public string DomainForDebug = "";
        /// <summary>
        /// Reltaive path for Ajax Return.
        /// </summary>
        public string AjaxRelativePath = "Ajax/";
        /// <summary>
        /// Reltaive path for Log Saving.
        /// </summary>
        public string LogRelativePath = "GameLog/";
        /// <summary>
        /// Reltaive path for Log Saving PHP.
        /// </summary>
        public string LogRelativePHP = "SaveLog.php";

        /// <summary>
        /// Full path for Ajax Return.
        /// </summary>
        public string AjaxPath { get { return (isDebug? DomainForDebug : Domain) + "/" + AjaxRelativePath; } }
        /// <summary>
        /// Full path for Log Saving.
        /// </summary>
        public string LogPath { get { return (isDebug ? DomainForDebug : Domain) + "/" + LogRelativePath; } }
        /// <summary>
        /// Full path for Log Saving PHP.
        /// </summary>
        public string LogPHP { get { return (isDebug ? DomainForDebug : Domain) + "/" + LogRelativePHP; } }

        //Internal Test
        /// <summary>
        /// Flag for Debugging.
        /// </summary>
        [Header("Flags")]
        public bool isDebug = false;
        /// <summary>
        /// Flag for WebGL Output.
        /// </summary>
        public bool isWebGL = false;

        /// <summary>
        /// Flag for using UnityEngine EventSystems.
        /// </summary>
        [Header("Systems")]
        public bool useEventSystems = true;
        /// <summary>
        /// Flag for using ZInput.
        /// </summary>
        public bool useZInputSystem = true;
        /// <summary>
        /// Flag for using ZLangSys.
        /// </summary>
        public bool useZLanguageSystem = true;
        /// <summary>
        /// Flag for using ZThemeSys.
        /// </summary>
        public bool useZThemeSystem = true;
        /// <summary>
        /// Flag for using ZAudio2D.
        /// </summary>
        public bool useZAudio2DSystem = true;
        /// <summary>
        /// Flag for using ZDebug.
        /// </summary>
        public bool useZDebugSystem = false;

        //OnStart Preloading
        /// <summary>
        /// Flag for getting presets from PlayerPref.
        /// </summary>
        [Header("Preloading")]
        public bool isGetPresetFromPlayerPref = true;
        /// <summary>
        /// Flag for saving presets to PlayerPref.
        /// </summary>
        public bool isSavePresetToPlayerPref = true;
        /// <summary>
        /// Flag for auto-tuning 3D camera ratio.
        /// </summary>
        public bool isTune3DCameraRatio = false;

        //Preset Defaults
        [Header("Defaults")]
        [SerializeField] float defaultAnimationDuration = 0.5f;
        /// <summary>
        /// Default animation duration for coroutines.
        /// </summary>
        public float defDUR {
            set { defaultAnimationDuration = value; }
            get { return defaultAnimationDuration; }
        }

        /// <summary>
        /// Flag for using Google Admob.
        /// </summary>
        [Header("Plugins")]
        public bool useAdmob = false;
        public AdmobIDs AndroidAdmob, iOSAdmob;
        [Serializable]
        public class AdmobIDs
        {
            public string bannerID, interstitialID, rewardVideoID;
        }
        /// <summary>
        /// Return Admob Banner Ads ID.
        /// </summary>
        public string AdmobBannerID
        {
            get
            {
                if (Application.platform == RuntimePlatform.Android)
                    return AndroidAdmob.bannerID;
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                    return iOSAdmob.bannerID;
                return "";
            }
        }
        /// <summary>
        /// Return Admob Interstitial Ads ID
        /// </summary>
        public string AdmobInterstitialID
        {
            get
            {
                if (Application.platform == RuntimePlatform.Android)
                    return AndroidAdmob.interstitialID;
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                    return iOSAdmob.interstitialID;
                return "";
            }
        }
        /// <summary>
        /// Return Admob Reward Video Ads ID
        /// </summary>
        public string AdmobRewardVideoID
        {
            get
            {
                if (Application.platform == RuntimePlatform.Android)
                    return AndroidAdmob.rewardVideoID;
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                    return iOSAdmob.rewardVideoID;
                return "";
            }
        }

        #endregion

        #region Runtime Global Variables
        /// <summary>
        /// Device ID of User
        /// </summary>
        public string DeviceID { private set; get; }
        /// <summary>
        /// Detect if Touch Pad
        /// </summary>
        public bool isTouchPad { private set; get; }

        #endregion

        override protected void AwakeRun()
        {
            //Platform Detection
            isTouchPad = (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer);
            DeviceID = SystemInfo.deviceUniqueIdentifier;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void Start()
        {
            SavePreset();
            Tune3DCameraRatio();
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Tune3DCameraRatio();
        }

        /// <summary>
        /// Tune the 3D Camera Ratio
        /// </summary>
        public void Tune3DCameraRatio()
        {
            if (isTune3DCameraRatio)
            {
                ZUICamera cam = FindObjectOfType<ZUICamera>();
                cam.Tune3DCameraRatio(Resolution);
            }
        }

        /// <summary>
        /// Save Presets to PlayerPref
        /// </summary>
        public void SavePreset()
        {
            if (!isSavePresetToPlayerPref) return;
            if (ZLangSys.It) PlayerPrefs.SetInt("ZLanguage", ZLangSys.It.AppLanguage);
            if (ZThemeSys.It) PlayerPrefs.SetInt("ZTheme", ZThemeSys.It.Theme);
            if (ZAudio2D.It)
            {
                PlayerPrefs.SetFloat("ZBGMVolume", ZAudio2D.It.BGMVolume);
                PlayerPrefs.SetFloat("ZSFXVolume", ZAudio2D.It.SFXVolume);
                PlayerPrefs.SetFloat("ZVOVolume", ZAudio2D.It.VOVolume);
                PlayerPrefsX.SetBool("ZBGMOn", ZAudio2D.It.BGMOn);
                PlayerPrefsX.SetBool("ZSFXOn", ZAudio2D.It.SFXOn);
                PlayerPrefsX.SetBool("ZVOOn", ZAudio2D.It.VOOn);
            }
        }

        /// <summary>
        /// Get location information.
        /// </summary>
        /// <param name="Callback">Callback function for receiving LocationInfo.</param>
        /// <param name="Failed">Callback function in case of failure.</param>
        /// <returns></returns>
        public IEnumerator GetLocation(Action<LocationInfo> Callback, Action Failed = null)
        {
            if (!Input.location.isEnabledByUser)
            {
                if (Failed != null) Failed();
                yield break;
            }
            Input.location.Start();
            int maxWait = 20;
            while(Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                yield return new WaitForSeconds(1);
                maxWait--;
            }

            if (maxWait < 1)
            {
                if (Failed != null) Failed();
                yield break;
            }

            if(Input.location.status == LocationServiceStatus.Failed)
            {
                if (Failed != null) Failed();
                yield break;
            }
            else
            {
                Callback(Input.location.lastData);
            }
            Input.location.Stop();
        }

        /// <summary>
        /// Version checking.
        /// </summary>
        /// <param name="version">Server version.</param>
        /// <param name="deviation">Server deviation.</param>
        /// <returns></returns>
        public bool VersionCheck(string version, int deviation = 0)
        {
            if (deviation != Deviation) return false;

            string[] _input = version.Split('.');
            string[] _nowVer = Version.Split('.');
            if (_input.Length != _nowVer.Length) return false;

            int x, y;
            for(int i=0; i < _input.Length; i++)
            {
                if (int.TryParse(_input[i], out x) && int.TryParse(_nowVer[i], out y))
                {
                    if (x < y) return false;
                }
                else
                    return false;
            }
            return true;
        }

#if UNITY_EDITOR

#endif
    }
}
