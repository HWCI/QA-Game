using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace creXa.GameBase
{
    [AddComponentMenu("creXa/Varies/FrameVaries2D")]
    public class FrameVaries2D : ZAnimated2D
    {
        int _aniState = -1;
        public int aniState
        {
            get { return _aniState; }
            set
            {
                _aniState = value;
                if(_aniState < aniStateInfos.Length)
                {
                    currASInfo = aniStateInfos[_aniState];
                    frameState = currASInfo.initFrame;
                }    
                else
                {
                    Debug.LogWarning("No info initialized.");
                    _aniState = -1;
                    currASInfo = null;
                }
            }
        }
        public Image aniImage;
        public float GapTime = 0.0f;

        public AniStateInfo currASInfo;
        public AniStateInfo[] aniStateInfos;
        [Serializable]
        public class AniStateInfo
        {
            public Sprite[] Sprites;
            public float swipeTime = 0.02f;
            public int initFrame = 0;
            public int endFrame = 0;
            public bool loop = true;
            public string resourcesPath = "";

            public void LoadDirectly(ref Sprite[] source, bool autoEndFrame = true)
            {
                Sprites = source;
                if (Sprites.Length == 0)
                {
                    Debug.LogWarning("No sprites were loaded.");
                    return;
                }
                if (autoEndFrame)
                    endFrame = Sprites.Length - 1;
            }

            public void LoadFromResources(string path = "", bool autoEndFrame = true)
            {
                if(path == "")
                {
                    path = resourcesPath;
                }
                Sprites = Resources.LoadAll<Sprite>(path);
                if (Sprites.Length == 0)
                {
                    Debug.LogWarning("No sprites were loaded.");
                    return;
                }
                if (autoEndFrame)
                    endFrame = Sprites.Length - 1;
            }

            public void Load(Sprite[] sprites, bool autoEndFrame = true)
            {
                Sprites = sprites;
                if (Sprites.Length == 0)
                {
                    Debug.LogWarning("No sprites were loaded.");
                    return;
                }
                if (autoEndFrame)
                    endFrame = Sprites.Length - 1;
            }

            public static implicit operator bool(AniStateInfo f)
            {
                return f != null;
            }
        }

        int frameState = 0;
        float ftimer = 0.0f;

        public delegate void OnEndOfFrameDel();
        public OnEndOfFrameDel OnEndOfFrame;

        protected virtual void Update() { UpdateRun(); }
        protected virtual void UpdateRun()
        {
            if (aniState == -1) aniState = 0;
            if (pause) return;
            if (currASInfo && currASInfo.Sprites != null)
            {
                ftimer -= Time.deltaTime;
                if (ftimer < 0)
                {
                    ftimer = currASInfo.swipeTime;
                    frameState++;
                    if(currASInfo.loop && frameState == currASInfo.Sprites.Length)
                        frameState = 0;

                    UpdateSprite();

                    if (frameState == currASInfo.endFrame)
                    {
                        if (OnEndOfFrame != null) OnEndOfFrame();
                        ftimer = GapTime;
                    }         
                }
            }
        }

        protected virtual void UpdateSprite()
        {
            if (aniImage && currASInfo && frameState < currASInfo.Sprites.Length)
                aniImage.sprite = currASInfo.Sprites[frameState];
        }

    }

}
