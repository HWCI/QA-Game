#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Linq;
using UnityEditorInternal;
using creXa.GameBase.Graphics;

namespace creXa.GameBase
{
    [CustomEditor(typeof(ZRecordList))]
    public class _Editor_ZRecordList : ZEditor<ZRecordList>
    {
        int numberOfFields;
        float[] fieldWidth;
        string[] fieldName;

        string itemName;

        bool useLangSys = true;
        bool selectable = false;
        bool cellSeparate = false;

        protected override void OnEnableRun()
        {
            base.OnEnableRun();

            numberOfFields = GetInt("numberOfFields", 1);
            fieldWidth = GetFloatArray("fieldWidth", 100, numberOfFields);
            fieldName = GetStringArray("fieldName", "NEW", numberOfFields);

            itemName = GetString("itemName", "Default");
            useLangSys = GetBool("useLangSys", true);
            selectable = GetBool("selectable", false);
            cellSeparate = GetBool("cellSeparate", false);
        }

        protected override void OnInspectorGUIRun()
        {
            base.OnInspectorGUIRun();

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
            for(int i=0; i<numberOfFields; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Column " + i);
                fieldName[i] = EditorGUILayout.TextField(fieldName[i]);
                fieldWidth[i] = EditorGUILayout.FloatField(fieldWidth[i]);
                EditorGUILayout.EndHorizontal();
            }
            if (EditorGUI.EndChangeCheck())
            {
                SetStringArray("fieldName", fieldName);
                SetFloatArray("fieldWidth", fieldWidth);
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
                itemName = EditorGUILayout.TextField("Item Name", itemName);
                useLangSys = EditorGUILayout.Toggle("Use LangSys?" , useLangSys);
                selectable = EditorGUILayout.Toggle("Selectable", selectable);
                cellSeparate = EditorGUILayout.Toggle("Separate Cell?", cellSeparate);
                if (EditorGUI.EndChangeCheck())
                {
                    SetString("itemName", itemName);
                    SetBool("useLangSys", useLangSys);
                    SetBool("selectable", selectable);
                    SetBool("cellSeparate", cellSeparate);
                }
                EditorGUILayout.Space();

                if (GUILayout.Button("Create Hierarchy"))
                {
                    CreateHierarchy();
                }
            }
            
            if(z.transform.childCount != 0)
            {
                if (GUILayout.Button("Update Width"))
                {
                    UpdateField();
                }

                EditorGUILayout.PropertyField(sProp["Header"], true);
                EditorGUILayout.PropertyField(sProp["Root"]);

            }
        }

        void UpdateField(){

            for(int i = 0; i < z.Header.Length; i++)
            {
                z.Header[i].GetComponent<RectTransform>().sizeDelta = new Vector2(fieldWidth[i], 60);

                GameObject caption = z.Header[i].transform.Find("Caption").gameObject;
                Text captionText = caption.GetComponent<Text>();
                captionText.fontSize = 45;
                captionText.text = fieldName[i];

                ZText captionZText = caption.GetComponent<ZText>();
                if (captionZText)
                {
                    captionZText.ID = fieldName[i];
                }
            }

            ZRecord zrecord = z.GetComponentInChildren<ZRecord>();
            for(int i=0; i< zrecord.rects.Length; i++)
            {
                zrecord.rects[i].sizeDelta = new Vector2(fieldWidth[i], zrecord.rects[i].sizeDelta.y);
            }

            float tmpWidth = 10 + fieldWidth.Length;
            for (int i = 0; i < fieldWidth.Length; i++)
                tmpWidth += fieldWidth[i];
            ScrollRect scrollRect = z.GetComponentInChildren<ScrollRect>();
            scrollRect.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.CeilToInt(tmpWidth / 10) * 10, scrollRect.GetComponent<RectTransform>().sizeDelta.y);

        }

        void ResolveArrayLength()
        {
            List<float> tmpFW = fieldWidth == null ? new List<float>() :  fieldWidth.ToList();
            while(tmpFW.Count > numberOfFields)
                tmpFW.RemoveAt(tmpFW.Count - 1);
            while (tmpFW.Count < numberOfFields)
                tmpFW.Add(100.0f);
            fieldWidth = tmpFW.ToArray();

            List<string> tmpFN = fieldName == null ? new List<string>() : fieldName.ToList();
            while (tmpFN.Count > numberOfFields)
                tmpFN.RemoveAt(tmpFN.Count - 1);
            while (tmpFN.Count < numberOfFields)
                tmpFN.Add("NEW");
            fieldName = tmpFN.ToArray();

            SetStringArray("fieldName", fieldName);
            SetFloatArray("fieldWidth", fieldWidth);
        }


        void CreateHierarchy()
        {
            VerticalLayoutGroup pVLG = z.GetComponent<VerticalLayoutGroup>();
            if(!pVLG) pVLG = z.gameObject.AddComponent<VerticalLayoutGroup>();
            pVLG.childAlignment = TextAnchor.MiddleCenter;

            ContentSizeFitter pCSF = z.gameObject.GetComponent<ContentSizeFitter>();
            if(!pCSF) pCSF = z.gameObject.AddComponent<ContentSizeFitter>();
            pCSF.horizontalFit = pCSF.verticalFit = ContentSizeFitter.FitMode.MinSize;

            GameObject headerObj = CreateLayout("H", z.transform);

            List<ZSelectable> tmp = new List<ZSelectable>();
            //AddHeader
            for(int i =0; i<numberOfFields; i++)
            {
                GameObject header = CreateSelectable<Text>(headerObj.transform);
                header.GetComponent<RectTransform>().sizeDelta = new Vector2(fieldWidth[i], 60);
                ZSRectangle zsRect = header.GetComponent<ZSRectangle>();
                zsRect.borderWidth = 3;
                zsRect.roundCorner = new bool[] { i == numberOfFields - 1, i == numberOfFields - 1, i == 0, i == 0 };
                header.name = useLangSys ? "Column " + i : fieldName[i];

                GameObject caption = header.transform.Find("Caption").gameObject;
                Text captionText = caption.GetComponent<Text>();
                captionText.fontSize = 45;
                captionText.text = fieldName[i];
                if (useLangSys) AddZText(captionText, fieldName[i]);

                tmp.Add(header.GetComponent<ZSelectable>());
            }

            z.Header = tmp.ToArray();

            //Add List
            GameObject scrollRect = CreateScroll(z.transform);
            scrollRect.GetComponent<ScrollRect>().horizontal = false;
            GameObject content = scrollRect.GetComponent<ScrollRect>().content.gameObject;
            content.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1);

            float tmpWidth = 10 + fieldWidth.Length;
            for (int i = 0; i < fieldWidth.Length; i++)
                tmpWidth += fieldWidth[i];
            scrollRect.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.CeilToInt(tmpWidth / 10) * 10, 500);

            VerticalLayoutGroup cVLG = content.AddComponent<VerticalLayoutGroup>();
            cVLG.childAlignment = TextAnchor.UpperCenter;
            cVLG.padding = new RectOffset(0, 0, 10, 0);
            cVLG.spacing = 1;
            z.Root = content.AddComponent<ZRecordRoot>();

            //Add Item

            GameObject item = CreateLayout("H", content.transform);
            item.name = itemName == null || itemName == "" ? "Item" : itemName;
            item.GetComponent<HorizontalLayoutGroup>().spacing = 1;

            ZRecord zrecord = item.AddComponent<ZRecord>();
            Button mGBtn = null;
            if (!cellSeparate)
            {
                mGBtn = item.AddComponent<ZMultiGrahpicButton>();
            }

            List<RectTransform> tmpRect = new List<RectTransform>();
            List<Button> tmpItemBtn = new List<Button>();
            List<Text> tmpItemText = new List<Text>();
            List<ZSelectable> tmpItemSel = new List<ZSelectable>();
            for (int i = 0; i < numberOfFields; i++)
            {   
                GameObject field = selectable? CreateSelectable<Text>(item.transform, mGBtn) : CreateButton(item.transform);
                if(!selectable) field.GetComponent<ZThemeObj>().Variation = 20;
                RectTransform fieldRect = field.GetComponent<RectTransform>();
                fieldRect.sizeDelta = new Vector2(fieldWidth[i], 60);

                ZSRectangle zsRect = field.GetComponent<ZSRectangle>();
                zsRect.borderWidth = 2;
                zsRect.roundCorner = new bool[] { i == numberOfFields - 1, i == numberOfFields - 1, i == 0, i == 0 };
                field.name = useLangSys ? fieldName[i] : "Column " + i;

                GameObject caption = field.transform.Find("Caption").gameObject;
                if (!selectable) caption.GetComponent<ZThemeObj>().Variation = 24;

                Text captionText = caption.GetComponent<Text>();
                captionText.fontSize = 45;

                if (cellSeparate)
                {
                    tmpItemBtn.Add(field.GetComponent<Button>());
                }
                else
                {
                    if (i == 0) tmpItemBtn.Add(mGBtn);
                }

                if (!cellSeparate)
                    DestroyImmediate(field.GetComponent<Button>());

                tmpItemText.Add(captionText);
                tmpRect.Add(fieldRect);
                if (selectable) tmpItemSel.Add(field.GetComponent<ZSelectable>());
            }

            if (!cellSeparate && mGBtn)
            {
                mGBtn.targetGraphic = item.GetComponentInChildren<Graphic>();
            }
                
            zrecord.button = tmpItemBtn.ToArray();
            zrecord.field = tmpItemText.ToArray();
            zrecord.rects = tmpRect.ToArray();

            if (selectable) zrecord.selectable = tmpItemSel.ToArray();


        }

    }
}

#endif
