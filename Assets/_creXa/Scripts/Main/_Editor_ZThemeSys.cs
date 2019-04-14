#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using SimpleJSON;

namespace creXa.GameBase
{
    [CustomEditor(typeof(ZThemeSys))]
    public class _Editor_ZThemeSys : ZEditor<ZThemeSys>
    {
        string dataPath = "Assets/_creXa/Data/ThemeSets.dat";
        bool confirmDefault = false;
        bool confirmSave = false;
        bool defaultFoldOut = true;
        ReorderableList themelist;

        protected override void OnEnableRun()
        {
            base.OnEnableRun();
            defaultFoldOut = GetBool("defaultFoldOut");

            SetupReorderableList();
        }

        void SetupReorderableList()
        {
            themelist = new ReorderableList(sObj, sProp["Themes"], false, true, true, true);
            themelist.elementHeight = (2.0f + z.Variation) * EditorGUIUtility.singleLineHeight * 1.2f;

            themelist.drawElementCallback = (rect, index, active, focused) => {
                //Rect startRect = rect;
                rect.y += EditorGUIUtility.singleLineHeight * 0.1f;
                rect.height = EditorGUIUtility.singleLineHeight * 1.2f;
                if (index > z.Themes.Length - 1) return;
                rect.height -= 2;
                float fullWidth = GetSuitableWidth();
                rect.width = 30;
                fullWidth -= rect.width;
                GUI.Label(rect, "#" + (index + 1));

                float savex = rect.x;
                rect.x += rect.width;
                float savefw = fullWidth;
                rect.width = fullWidth;
                z.Themes[index].Name = GUI.TextField(rect, z.Themes[index].Name);

                rect.y += EditorGUIUtility.singleLineHeight * 1.2f;
                rect.x = savex;

                float namew = 120;
                float cw = (fullWidth - namew) / 2;

                rect.x += namew;

                rect.width = cw;
                fullWidth -= rect.width;
                GUI.Label(rect, "Color");

                rect.x += rect.width;
                rect.width = cw;
                fullWidth -= rect.width;
                GUI.Label(rect, "Border");


                for (int i = 0; i < z.Themes[index].varies.Length; i++)
                {
                    rect.y += EditorGUIUtility.singleLineHeight * 1.2f;
                    rect.x = savex;
                    fullWidth = savefw;

                    rect.width = 30;
                    fullWidth -= rect.width;
                    GUI.Label(rect, "~" + i);

                    rect.x += rect.width;
                    rect.width = namew;
                    fullWidth -= rect.width;
                    GUI.Label(rect, i < z.VariationName.Length ? z.VariationName[i] + ":" : "Var " + i + ":");

                    rect.x += rect.width;
                    rect.width = cw;
                    fullWidth -= rect.width;
                    z.Themes[index].varies[i].Color = EditorGUI.ColorField(rect, z.Themes[index].varies[i].Color);

                    rect.x += rect.width;
                    rect.width = cw;
                    fullWidth -= rect.width;
                    z.Themes[index].varies[i].BorderColor = EditorGUI.ColorField(rect, z.Themes[index].varies[i].BorderColor);

                }
            };

            themelist.drawHeaderCallback = rect =>
            {
                Rect startRect = rect;
                rect.y += startRect.height * 0.05f;
                rect.height = EditorGUIUtility.singleLineHeight * 1.2f;
                rect.height -= 2;
                float fullWidth = GetSuitableWidth();
                rect.width = 30;
                fullWidth -= rect.width;

                rect.width = fullWidth;
                fullWidth -= rect.width;
                GUI.Label(rect, "Count: " + z.Variation);
            };

            themelist.onAddCallback = list =>
            {
                List<ZThemeSys.ThemeSet> tmp;
                if (z.Themes != null)
                {
                    tmp = z.Themes.ToList();    
                }
                else
                {
                    tmp = new List<ZThemeSys.ThemeSet>();
                }

                tmp.Add(CreateNewThemeSet(tmp.Count == 0? null : tmp[tmp.Count - 1]));
                z.Themes = tmp.ToArray();
            };
        }

        ZThemeSys.ThemeSet CreateNewThemeSet(ZThemeSys.ThemeSet set = null)
        {
            if(set == null)
            {
                JSONNode data = JSONNode.LoadFromFile(dataPath);
                set = FromData(data[0]);
            }

            ZThemeSys.ThemeSet rtn = new ZThemeSys.ThemeSet();
            rtn.Name = "Default";
            rtn.varies = new ZThemeSys.ThemeSet.Variation[z.Variation];
            for (int i = 0; i < rtn.varies.Length; i++)
            {
                rtn.varies[i] = new ZThemeSys.ThemeSet.Variation();
                rtn.varies[i].Color = set != null && set.varies[i] != null ? set.varies[i].Color : ZColor.Rand;
                rtn.varies[i].BorderColor = set != null && set.varies[i] != null ? set.varies[i].BorderColor : ZColor.Rand;
            }
                
            return rtn;
        }

        protected override void OnInspectorGUIRun()
        {
            EditorGUILayout.PropertyField(sProp["_theme"]);
            EditorGUILayout.PropertyField(sProp["defaultTheme"]);
            EditorGUILayout.PropertyField(sProp["animated"]);
            if(z.animated)
                EditorGUILayout.PropertyField(sProp["animatedDUR"]);

            EditorGUILayout.BeginHorizontal();
            GUI.enabled = confirmSave;
            if (GUILayout.Button("Save Default Values"))
            {
                SaveDeafult();
            }
            GUI.enabled = true;
            confirmSave = GUILayout.Toggle(confirmSave, "Confirm?");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUI.enabled = confirmDefault;
            if (GUILayout.Button("Load Default Values"))
            {
                LoadDefault();
            }
            GUI.enabled = true;
            confirmDefault = GUILayout.Toggle(confirmDefault, "Confirm?");
            EditorGUILayout.EndHorizontal();

            EditorGUI.BeginChangeCheck();
            int variation = EditorGUILayout.IntSlider("Variation", z.Variation, 1, 30);
            if (EditorGUI.EndChangeCheck())
            {
                if (variation >= 0)
                {
                    z.Variation = variation;
                    if (z.Themes != null)
                    {
                        for (int i = 0; i < z.Themes.Length; i++)
                        {
                            List<ZThemeSys.ThemeSet.Variation> tmp = null;
                            if (z.Themes[i].varies == null)
                            {
                                tmp = new List<ZThemeSys.ThemeSet.Variation>();
                                for (int j = 0; j < z.Variation; j++)
                                    tmp.Add(new ZThemeSys.ThemeSet.Variation());
                            }
                            else if (z.Themes[i].varies.Length != z.Variation)
                            {
                                tmp = z.Themes[i].varies.ToList();
                                while (tmp.Count < z.Variation)
                                    tmp.Add(new ZThemeSys.ThemeSet.Variation());
                                while (tmp.Count > z.Variation)
                                    tmp.RemoveAt(tmp.Count - 1);
                            }
                            else
                            {
                                tmp = z.Themes[i].varies.ToList();
                            }
                            z.Themes[i].varies = tmp.ToArray();
                        }

                        SetupReorderableList();
                        EditorGUIUtility.ExitGUI();
                    }
                    else
                        Debug.LogWarning("No Themes.");

                }

            }

            DrawSeparate();
            EditorGUILayout.BeginHorizontal();
            if (z.Themes != null)
            {
                for (int i = 0; i < z.Themes.Length; i++)
                {
                    if (GUILayout.Button(i.ToString(), GUILayout.Width(20)))
                    {
                        z.Theme = i;
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.LabelField("Themes", EditorStyles.centeredGreyMiniLabel);
            if(GUILayout.Button((defaultFoldOut ? "Hide" : "Show"))){
                defaultFoldOut = !defaultFoldOut;
                SetBool("defaultFoldOut", defaultFoldOut);
            }
            if(defaultFoldOut)
                themelist.DoLayoutList();

        }

        void SaveDeafult()
        {
            JSONNode data = new JSONArray();
            for(int i=0; i<z.Themes.Length; i++)
            {
                data[i]["Name"] = z.Themes[i].Name;
                for(int v=0; v<z.Themes[i].varies.Length; v++)
                {
                    data[i]["Varies"][v]["Color"][0].AsFloat = z.Themes[i].varies[v].Color.r;
                    data[i]["Varies"][v]["Color"][1].AsFloat = z.Themes[i].varies[v].Color.g;
                    data[i]["Varies"][v]["Color"][2].AsFloat = z.Themes[i].varies[v].Color.b;
                    data[i]["Varies"][v]["Color"][3].AsFloat = z.Themes[i].varies[v].Color.a;
                    data[i]["Varies"][v]["Border"][0].AsFloat = z.Themes[i].varies[v].BorderColor.r;
                    data[i]["Varies"][v]["Border"][1].AsFloat = z.Themes[i].varies[v].BorderColor.g;
                    data[i]["Varies"][v]["Border"][2].AsFloat = z.Themes[i].varies[v].BorderColor.b;
                    data[i]["Varies"][v]["Border"][3].AsFloat = z.Themes[i].varies[v].BorderColor.a;
                }
            }

            data.SaveToFile(dataPath);
        }

        void LoadDefault()
        {
            JSONNode data = JSONNode.LoadFromFile(dataPath);
            
            if (data.Count == 0) return;

            z.Themes = new ZThemeSys.ThemeSet[data.Count];
            z.Variation = data[0]["Varies"].Count;

            for(int i=0; i<data.Count; i++)
                z.Themes[i] = FromData(data[i]);

            SetupReorderableList();
            EditorGUIUtility.ExitGUI();

        }

        ZThemeSys.ThemeSet FromData(JSONNode data)
        {
            ZThemeSys.ThemeSet rtn;

            rtn = new ZThemeSys.ThemeSet();
            rtn.Name = data["Name"];
            rtn.varies = new ZThemeSys.ThemeSet.Variation[data["Varies"].Count];

            for (int v = 0; v < data["Varies"].Count; v++)
            {
                rtn.varies[v] = new ZThemeSys.ThemeSet.Variation();
                rtn.varies[v].Color = new Color(
                    data["Varies"][v]["Color"][0].AsFloat,
                    data["Varies"][v]["Color"][1].AsFloat,
                    data["Varies"][v]["Color"][2].AsFloat,
                    data["Varies"][v]["Color"][3].AsFloat);
                rtn.varies[v].BorderColor = new Color(
                    data["Varies"][v]["Border"][0].AsFloat,
                    data["Varies"][v]["Border"][1].AsFloat,
                    data["Varies"][v]["Border"][2].AsFloat,
                    data["Varies"][v]["Border"][3].AsFloat);
            }
            return rtn;
        }

    }
}

#endif
