#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.EventSystems;

namespace creXa.GameBase
{
    [CustomEditor(typeof(ZBase))]
    public class _Editor_ZBase : ZEditor<ZBase>
    {
        bool SystemFoldOut;
        bool PluginsFoldOut;
        bool ExpressFoldOut;
        bool PreloadFoldOut;

        protected override void OnEnableRun()
        {
            base.OnEnableRun();
            SystemFoldOut = GetBool("SystemFoldOut");
            PluginsFoldOut = GetBool("PluginsFoldOut");
            ExpressFoldOut = GetBool("ExpressFoldOut");
            PreloadFoldOut = GetBool("PreloadFoldOut");
        }

        protected override void ComponentCheckAll()
        {
            ComponentCheck<StandaloneInputModule, EventSystem>(z.useEventSystems);
            ComponentCheck<ZInput>(z.useZInputSystem);
            ComponentCheck<ZLangSys>(z.useZLanguageSystem);
            ComponentCheck<ZThemeSys>(z.useZThemeSystem);
            ComponentCheck<ZAudio2D>(z.useZAudio2DSystem);
            ComponentCheck<ZDebug>(z.useZDebugSystem);
        }

        protected override void OnInspectorGUIRun()
        {
            EditorGUILayout.LabelField("GameInfo", EditorStyles.centeredGreyMiniLabel);
            EditorGUILayout.BeginVertical("Box");
            z.GameName = EditorGUILayout.TextField("Game Name", z.GameName);
            EditorGUILayout.PropertyField(sProp["Version"]);
            EditorGUILayout.PropertyField(sProp["Deviation"]);
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Resolution");
            z.ResolutionW = EditorGUILayout.FloatField(z.ResolutionW);
            EditorGUILayout.LabelField("x", GUILayout.Width(10));
            z.ResolutionH = EditorGUILayout.FloatField(z.ResolutionH);
            EditorGUILayout.EndHorizontal();

            z.defDUR = EditorGUILayout.FloatField("Default Duration", z.defDUR);

            EditorGUILayout.BeginHorizontal(BestWidth());
            z.isNetworkGame = GUILayout.Toggle(z.isNetworkGame, "Network");
            z.isWebGL = GUILayout.Toggle(z.isWebGL, "WebGL");
            z.isDebug = GUILayout.Toggle(z.isDebug, "Debug");
            EditorGUILayout.EndHorizontal();
            if (z.isNetworkGame)
            {
                EditorGUILayout.PropertyField(sProp["Port"]);
                EditorGUILayout.PropertyField(sProp["ServerIP"]);
                EditorGUILayout.PropertyField(sProp["defTimeOut"]);
                EditorGUILayout.LabelField("Links", EditorStyles.centeredGreyMiniLabel);
                z.Domain = EditorGUILayout.TextField("Domain", z.Domain);
                z.DomainForDebug = EditorGUILayout.TextField("Domain For Debug", z.DomainForDebug);
                EditorGUILayout.PropertyField(sProp["AjaxRelativePath"]);
                if ((z.useZDebugSystem))
                {
                    EditorGUILayout.PropertyField(sProp["LogRelativePath"]);
                    EditorGUILayout.PropertyField(sProp["LogRelativePHP"]);
                }
            }
            
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("HelpBox");
            if (GUILayout.Button("Preloading", EditorStyles.boldLabel))
            {
                PreloadFoldOut = !PreloadFoldOut;
                SetBool("PreloadFoldOut", PreloadFoldOut);
            }
            if (PreloadFoldOut)
            {
                z.isGetPresetFromPlayerPref = GUILayout.Toggle(z.isGetPresetFromPlayerPref, "Get Preset From PlayerPref");
                z.isSavePresetToPlayerPref = GUILayout.Toggle(z.isSavePresetToPlayerPref, "Save Preset To PlayerPref");
                z.isTune3DCameraRatio = EditorGUILayout.Toggle("Tune Camera Ratio", z.isTune3DCameraRatio);
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("HelpBox");
            if (GUILayout.Button("System Settings", EditorStyles.boldLabel))
            {
                SystemFoldOut = !SystemFoldOut;
                SetBool("SystemFoldOut", SystemFoldOut);
            }
            if (SystemFoldOut)
            {
                EditorGUI.BeginChangeCheck();
                z.useEventSystems = GUILayout.Toggle(z.useEventSystems, "Use EventSystems");
                z.useZInputSystem = GUILayout.Toggle(z.useZInputSystem, "Use ZInput System");
                z.useZLanguageSystem = GUILayout.Toggle(z.useZLanguageSystem, "Use ZLanguage System");
                z.useZThemeSystem = GUILayout.Toggle(z.useZThemeSystem, "Use ZTheme System");
                z.useZAudio2DSystem = GUILayout.Toggle(z.useZAudio2DSystem, "Use ZAudio2D System");
                z.useZDebugSystem = GUILayout.Toggle(z.useZDebugSystem, "Use ZDebug System");
                if (!z.isNetworkGame && z.useZDebugSystem)
                    EditorGUILayout.HelpBox("Needs Network Flag To be turned ON.", MessageType.Error);

                if (EditorGUI.EndChangeCheck())
                {
                    ComponentCheckAll();
                }
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("HelpBox");
            if (GUILayout.Button("Plugins", EditorStyles.boldLabel))
            {
                PluginsFoldOut = !PluginsFoldOut;
                SetBool("PluginsFoldOut", PluginsFoldOut);
            }
            if (PluginsFoldOut)
            {
                z.useAdmob = GUILayout.Toggle(z.useAdmob, "Use Admob Plugins");
                if (z.useAdmob)
                {
                    EditorGUILayout.PropertyField(sProp["AndroidAdmob"], true);
                    EditorGUILayout.PropertyField(sProp["iOSAdmob"], true);
                }
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("HelpBox");
            if (GUILayout.Button("Express Functions", EditorStyles.boldLabel))
            {
                ExpressFoldOut = !ExpressFoldOut;
                SetBool("ExpressFoldOut", ExpressFoldOut);
            }
            if (ExpressFoldOut)
            {
                DrawSeparate();
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Copy ZBase Values"))
                {
                    CopyValues();
                }

                if (GUILayout.Button("Restore ZBase Values"))
                {
                    RestoreValues();
                }
                EditorGUILayout.EndHorizontal();

                DrawSeparate();

                if (GUILayout.Button("Create Folder Hierarchy"))
                {
                    CreateFolder();
                }

                if (GUILayout.Button("Insert UICanvas"))
                {
                    InsertUICanvas();
                }

                if (GUILayout.Button("Insert UI Camera Tune"))
                {
                    InsertUICamera();
                }

            }
            EditorGUILayout.EndVertical();
        }

        [MenuItem("creXa/Create Folder Hierarchy")]
        static void CreateFolder()
        {
            string[] folders = new string[]
            {
                "External",
                "Fonts",
                "LanguageBanks",
                "Plugins",
                "Resources", 
                "Scenes",
                "Scripts",
                "Splash",
                "Sprites",
                "Sounds"
            };

            for (int i = 0; i < folders.Length; i++)
                if(!AssetDatabase.IsValidFolder("Assets/" + folders[i]))
                    AssetDatabase.CreateFolder("Assets", folders[i]);
        }

        void InsertUICanvas()
        {
            GameObject tmp = new GameObject("UICanvas");
            tmp.transform.position = Vector3.zero;

            tmp.AddComponent<RectTransform>();

            Canvas cvs = tmp.AddComponent<Canvas>();
            cvs.worldCamera = Camera.main;
            cvs.renderMode = RenderMode.ScreenSpaceCamera;

            CanvasScaler cs = tmp.AddComponent<CanvasScaler>();
            float ratio = 100.0f;
            if ((z.ResolutionW == 16 && z.ResolutionH == 9) || (z.ResolutionW == 9 && z.ResolutionH == 16)) ratio = 100.0f;
            else if ((z.ResolutionW == 16 && z.ResolutionH == 10) || (z.ResolutionW == 10 && z.ResolutionH == 16)) ratio = 100.0f;
            else if ((z.ResolutionW == 4 && z.ResolutionH == 3) || (z.ResolutionW == 3 && z.ResolutionH == 4)) ratio = 256.0f;
            else if ((z.ResolutionW == 5 && z.ResolutionH == 4) || (z.ResolutionW == 4 && z.ResolutionH == 5)) ratio = 256.0f;

            cs.referenceResolution = new Vector2(z.ResolutionW, z.ResolutionH) * ratio;
            cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

            tmp.AddComponent<GraphicRaycaster>();
        }

        void InsertUICamera()
        {
            Camera.main.gameObject.AddComponent<ZUICamera>();
        }

        void CopyValues()
        {
            UnityEditorInternal.ComponentUtility.CopyComponent(z);
        }

        void RestoreValues()
        {
            if (!UnityEditorInternal.ComponentUtility.PasteComponentValues(z))
                Debug.LogWarning("Please Click \"Copy ZBase Values\" from Desired ZBase Component First.");
        }

        [MenuItem("creXa/Insert ZBase")]
        public static void AddToWorld()
        {
            GameObject tmp = new GameObject("_ZBase");
            tmp.AddComponent<ZBase>();
        }
    }
}
#endif

