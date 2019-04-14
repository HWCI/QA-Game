using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

namespace creXa.GameBase
{
    [AddComponentMenu("creXa/Base/Language System")]
    [RequireComponent(typeof(ZBase))]
    public class ZLangSys : ZSingleton<ZLangSys>
    {
        public bool loadPackageOnStart = true;
        public bool detectOSLanguage = true;
        public int defaultLanguage = 0;

        [ReadOnly][SerializeField]
        int _AppLanguage = -1;
        public int AppLanguage {
            set { SetLanguage(value); }
            get { return _AppLanguage; }
        }

        public LanguageSet[] Language;
        [Serializable]
        public class LanguageSet
        {
            public string Description;
            public Dictionary<string, string> dictionary;
            public TextAsset languageBanks;
            public Font defaultFont;

            public void BuildDictionary(int id)
            {
                if (dictionary == null && languageBanks != null)
                {
                    string[] bank = languageBanks.text.Split('\n');
                    dictionary = new Dictionary<string, string>();
                    char[] sep = new char[] { '=' };
                    for (int i = 0; i < bank.Length; i++)
                    {
                        if (bank[i] == "") continue;
                        string[] text = bank[i].Split(sep, 2);
                        string tmp;
                        if (!dictionary.TryGetValue(text[0], out tmp))
                            dictionary.Add(text[0], text[1]);
                        else
                            Debug.LogWarning("Language " + (id < LanguageType.Length? LanguageType[id] : Description) + "\n" + " Key duplicated and ignored on line " + i + " : " + text[0]);
         
                    }
                    Debug.Log("Language " + (id < LanguageType.Length ? LanguageType[id] : Description) + " Loaded Pair: " + dictionary.Count);
                }
            }
        }

        public static string[] LanguageType = new string[]
        {
            "en",
            "zh-TW",
            "zh-CN",
            "ja-JP",
            "ko-KR",
            "it-IT",
            "fr-FR",
            "de-DE"
        };

        int detectedLang;

        override protected void AwakeRun()
        {
            //Language Detection
            detectedLang = ZBase.It.isGetPresetFromPlayerPref ? PlayerPrefs.GetInt("ZLanguage", -1) : defaultLanguage;

            if (detectOSLanguage && (!ZBase.It.isGetPresetFromPlayerPref || detectedLang == -1))
            {
                Debug.Log("Detected Language: " + Application.systemLanguage);
                switch (Application.systemLanguage)
                {
                    case SystemLanguage.English: detectedLang = 0; break;
                    case SystemLanguage.Chinese:
                    case SystemLanguage.ChineseTraditional: detectedLang = 1; break;
                    case SystemLanguage.ChineseSimplified: detectedLang = 2; break;
                    case SystemLanguage.Japanese: detectedLang = 3; break;
                    case SystemLanguage.Korean: detectedLang = 4; break;
                    case SystemLanguage.Italian: detectedLang = 5; break;
                    case SystemLanguage.French: detectedLang = 6; break;
                    case SystemLanguage.German: detectedLang = 7; break;
                }
            }
            else if (detectedLang == -1)
            {
                detectedLang = defaultLanguage;
            }

            if (loadPackageOnStart)
                BuildDictionary();
        }

        void Start()
        {
            AppLanguage = detectedLang;
        }

        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        #region Build Library From Text Assets
        public void BuildDictionary()
        {
            for (int i = 0; i < Language.Length; i++)
                Language[i].BuildDictionary(i);
        }

        #endregion

        public void SetAllDirty()
        {
            Canvas canvas = FindMainCanvas();
            if (!canvas) return;
            ZLangObj[] allobjs = canvas.GetComponentsInChildren<ZLangObj>(true);
            for (int i = 0; i < allobjs.Length; i++)
                allobjs[i].dirty = true;
        }

        //find and set all active ZLangObj
        private void SetLanguage(int lang)
        {
            if(Language == null)
            {
                Debug.LogWarning("No Language Bank.");
                return;
            }
            if (lang < 0 || lang >= Language.Length)
            {
                Debug.LogWarning("No Language Bank Available for LanguageID: " + lang);
                return;
            }
                
            _AppLanguage = lang;
            Language[lang].BuildDictionary(lang);

            Canvas canvas = FindMainCanvas();
            if (!canvas) return;
            ZLangObj[] allobjs = canvas.GetComponentsInChildren<ZLangObj>(true);
            for (int i = 0; i < allobjs.Length; i++)
                allobjs[i].Language = lang;
        }

        Canvas FindMainCanvas()
        {
            Canvas[] canvass = FindObjectsOfType<Canvas>();
            if (canvass.Length > 1) { Debug.LogWarning("There are more than 1 Canvas in the Scene."); }
            for (int i = canvass.Length - 1; i >= 0; i--)
                if (canvass[i].transform.parent == null)
                    return canvass[i];
            return null;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SetLanguage(_AppLanguage);
        }

        public string GetByID(string ID, int lang)
        {
            if (lang < 0) lang = AppLanguage;
            if (Language[lang].dictionary == null) return "NULL";
            if (!Language[lang].dictionary.ContainsKey(ID))
            {
                Debug.LogWarning("Dictionary Missing: <" + ID + ">");
                return "[NULL]";
            }
            return Language[lang].dictionary[ID].Replace("\\n", "\n");
        }

    }
}
