#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Linq;
using UnityEditorInternal;
using creXa.GameBase.Graphics;
using UnityEngine.EventSystems;

namespace creXa.GameBase
{
    [CustomEditor(typeof(ZSlideSelector))]
    public class _Editor_ZSlideSelector : ZEditor<ZSlideSelector> {

        int numberOfFields;
        string[] fieldName;

        bool useLangSys = true;

        protected override void OnEnableRun()
        {
            base.OnEnableRun();

            numberOfFields = GetInt("numberOfFields", 1);
            fieldName = GetStringArray("fieldName", "1", numberOfFields);

            useLangSys = GetBool("useLangSys", true);
        }

        protected override void OnInspectorGUIRun()
        {
            base.OnInspectorGUIRun();
            EditorGUI.BeginChangeCheck();
            GUI.enabled = z.transform.childCount == 0;
            numberOfFields = EditorGUILayout.IntField("Number of Fields", numberOfFields);
            GUI.enabled = true;
            if (EditorGUI.EndChangeCheck())
            {
                SetInt("numberOfFields", numberOfFields);
                ResolveArrayLength();
            }

            if (z.transform.childCount == 0)
            {
                ResolveArrayLength();

                if (GUILayout.Button(" - Column - "))
                {
                    if (numberOfFields > 1) numberOfFields--;
                    SetInt("numberOfFields", numberOfFields);
                    ResolveArrayLength();
                }
            }

            EditorGUI.BeginChangeCheck();
            for (int i = 0; i < numberOfFields; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Column " + i);
                fieldName[i] = EditorGUILayout.TextField(fieldName[i]);
                EditorGUILayout.EndHorizontal();
            }
            if (EditorGUI.EndChangeCheck())
            {
                SetStringArray("fieldName", fieldName);
            }

            if (z.transform.childCount == 0)
            {
                if (GUILayout.Button(" + Column + "))
                {
                    numberOfFields++;
                    SetInt("numberOfFields", numberOfFields);
                    ResolveArrayLength();
                }

                EditorGUI.BeginChangeCheck();

                EditorGUILayout.PropertyField(sProp["cellWidth"]);
                EditorGUILayout.PropertyField(sProp["cellHeight"]);
                EditorGUILayout.PropertyField(sProp["showNextRatio"]);

                EditorGUILayout.PropertyField(sProp["vertical"]);
                useLangSys = EditorGUILayout.Toggle("Use LangSys?", useLangSys);
                if (EditorGUI.EndChangeCheck())
                {
                    SetBool("useLangSys", useLangSys);
                }
                EditorGUILayout.Space();

                if (GUILayout.Button("Create Hierarchy"))
                {
                    CreateHierarchy();
                }
            }

            if (z.transform.childCount != 0)
            {
                if (GUILayout.Button("Update Fields"))
                {
                    UpdateField();
                }

                DrawSeparate();
                EditorGUILayout.PropertyField(sProp["_selectedValue"]);
                EditorGUILayout.PropertyField(sProp["defaultValue"]);

                DrawSeparate();
                GUI.enabled = false;
                EditorGUILayout.PropertyField(sProp["vertical"]);
                EditorGUILayout.PropertyField(sProp["scrollRect"], true);
                EditorGUILayout.PropertyField(sProp["selectables"], true);
                GUI.enabled = true;

                EditorGUILayout.PropertyField(sProp["OnValueChanged"], true);
                
            }
        }

        void UpdateField()
        {
            z.SetText(fieldName);
            z.SetZText(fieldName);
        }

        void ResolveArrayLength()
        {

            List<string> tmpFN = fieldName == null ? new List<string>() : fieldName.ToList();
            while (tmpFN.Count > numberOfFields)
                tmpFN.RemoveAt(tmpFN.Count - 1);
            while (tmpFN.Count < numberOfFields)
                tmpFN.Add((tmpFN.Count + 1).ToString());
            fieldName = tmpFN.ToArray();

            SetStringArray("fieldName", fieldName);
        }

        void CreateHierarchy()
        {
            RectTransform rect = z.GetComponent<RectTransform>();
            if (!rect) rect = z.gameObject.AddComponent<RectTransform>();
            if(z.vertical)
                rect.sizeDelta = new Vector2(z.cellWidth, z.cellHeight * ( 1 + 2 * z.showNextRatio));
            else
                rect.sizeDelta = new Vector2(z.cellWidth * (1 + 2 * z.showNextRatio), z.cellHeight);
            Mask mask = z.GetComponent<Mask>();
            if(!mask) mask = z.gameObject.AddComponent<Mask>();
            ZSRectangle zsRect = z.GetComponent<ZSRectangle>();
            if(!zsRect) zsRect = z.gameObject.AddComponent<ZSRectangle>();
            zsRect.color = new Color(0.117f, 0.117f, 0.117f, 1);
            zsRect.borderColor = new Color(0.235f, 0.235f, 0.235f, 1);
            zsRect.borderWidth = 3;
            zsRect.border = false;
            zsRect.SetAllRoundRadius(25);
            AddZThemeObjZS(z.gameObject, 20);
            

            GameObject scrollRect = CreateScroll(z.transform);
            z.ev = scrollRect.AddComponent<EventTrigger>();

            ScrollRect scroll = scrollRect.GetComponent<ScrollRect>();
            scroll.horizontal = !z.vertical;
            scroll.vertical = z.vertical;
            scroll.movementType = ScrollRect.MovementType.Elastic;
            z.scrollRect = scroll;

            GameObject content = scrollRect.GetComponent<ScrollRect>().content.gameObject;
            HorizontalOrVerticalLayoutGroup layout;
            if (z.vertical)
            {
                layout = content.AddComponent<VerticalLayoutGroup>();
                layout.padding = new RectOffset(0, 0, Mathf.RoundToInt( z.cellHeight * z.showNextRatio), Mathf.RoundToInt(z.cellHeight * z.showNextRatio));
            }
            else {
                layout = content.AddComponent<HorizontalLayoutGroup>();
                layout.padding = new RectOffset(Mathf.RoundToInt(z.cellWidth * z.showNextRatio), Mathf.RoundToInt(z.cellWidth * z.showNextRatio), 0, 0);
            }

            ContentSizeFitter csf = content.GetComponent<ContentSizeFitter>();
            csf.horizontalFit = z.vertical ? ContentSizeFitter.FitMode.Unconstrained : ContentSizeFitter.FitMode.MinSize;
            csf.verticalFit = z.vertical ? ContentSizeFitter.FitMode.MinSize : ContentSizeFitter.FitMode.Unconstrained;

            layout.childControlWidth = z.vertical;
            layout.childControlHeight = !z.vertical;

            List<ZSelectable> tmp = new List<ZSelectable>();
            for(int i=0; i<numberOfFields; i++)
            {
                GameObject item = CreateSelectable<Text>(content.transform);
                RectTransform itemRect = item.GetComponent<RectTransform>();
                itemRect.sizeDelta = new Vector2(z.cellWidth, z.cellHeight);

                ZSelectable select = item.GetComponent<ZSelectable>();
                select.zshape.border = false;

                GameObject caption = item.transform.Find("Caption").gameObject;

                Text captionText = caption.GetComponent<Text>();
                captionText.fontSize = 45;
                captionText.text = fieldName[i];

                if (useLangSys) AddZText(captionText, fieldName[i]);

                DestroyImmediate(item.GetComponent<Button>());
                tmp.Add(select);
            }
            z.selectables = tmp.ToArray();

            GameObject imgObj = AddUIGameObject("Cover", z.transform);
            Image img = imgObj.AddComponent<Image>();
            img.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_creXa/Textures/MaskOut.png");
            img.color = new Color(1, 1, 1, 0.75f);
            img.raycastTarget = false;

            SetFullScreen(scrollRect);
            SetFullScreen(content, z.vertical, !z.vertical);
            SetFullScreen(imgObj);


        }

    }
}
#endif
