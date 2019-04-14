using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

namespace creXa.GameBase
{
    public class ZThemeSys : ZSingleton<ZThemeSys>
    {
        public int defaultTheme = 0;
        public bool animated = false;
        public float animatedDUR = 1f;

        [ReadOnly][SerializeField]
        int _theme = -1;
        public int Theme {
            set { SetTheme(value); }
            get { return _theme; }
        }

        public string ThemeName
        {
            set { SetThemeByName(value); }
            get { return Themes[Theme].Name; }
        }

        public string[] VariationName = new string[]
        {
            "BG",
            "Text",
            "Button",
            "Button Text",
            "Input",
            "Input Text",
            "Input PH",
            "Loading",
            "Loading BG",
            "Popup BG",
            "Popup Box",
            "Popup Text",
            "Popup Button",
            "Popup Button Text",
            "Popup Input",
            "Popup Input Text",
            "Popup PH",
            "ScrollBar Handle",
            "ScrollBar BG", 
            "Unselected Sel Cap",
            "Unselected Sel Btn",
            "Selected Sel Btn",
            "Selected Sel Cap",
            "Panel",
            "Panel Text"
        };

        [Range(1, 50)]
        public int Variation = 23;
        public ThemeSet[] Themes;

        [Serializable]
        public class ThemeSet
        {
            public string Name;
            public Variation[] varies;

            [Serializable]
            public class Variation
            {
                public Color Color;
                public Color BorderColor;
            }

            public ThemeSet()
            {
                varies = new Variation[1];
                varies[0] = new Variation();
            }
            
        }

        override protected void AwakeRun()
        {
            Theme = ZBase.It.isGetPresetFromPlayerPref ? PlayerPrefs.GetInt("ZTheme", 0) : defaultTheme;
        }

        public void SetAllDirty()
        {
            Canvas canvas = FindMainCanvas();
            if (!canvas) return;
            ZThemeObj[] allobjs = canvas.GetComponentsInChildren<ZThemeObj>(true);
            for (int i = 0; i < allobjs.Length; i++)
                allobjs[i].dirty = true;
        }

        void SetThemeByName(string themeName)
        {
            if (Themes == null)
            {
                Debug.LogWarning("No Themes.");
                return;
            }
            for (int i = 0; i < Themes.Length; i++)
                if (themeName == Themes[i].Name)
                {
                    Theme = i;
                    return;
                }
            Debug.Log("ThemeName cannot be found.");

        }

        void SetTheme(int t)
        {
            if (Themes == null)
            {
                Debug.LogWarning("No Themes.");
                return;
            }
            if (t < 0 || t > Themes.Length)
            {
                Debug.LogWarning("No Theme Available for ThemeSet: " + t);
                return;
            }
            _theme = t;
            Canvas canvas = FindMainCanvas();
            if (!canvas) return;
            ZThemeObj[] allobjs = canvas.GetComponentsInChildren<ZThemeObj>(true);
            for (int i = 0; i < allobjs.Length; i++)
                allobjs[i].Theme = t;
        }

        Canvas FindMainCanvas()
        {
            Canvas[] canvass = FindObjectsOfType<Canvas>();
            if(canvass.Length > 1) { Debug.LogWarning("There are more than 1 Canvas in the Scene."); }
            for (int i = canvass.Length - 1; i >= 0; i--)
                if (canvass[i].transform.parent == null)
                    return canvass[i];
            return null;
        }

        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SetTheme(_theme);
        }
    }
}
