#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace creXa.GameBase
{

    [CustomPropertyDrawer(typeof(IntRangeAttribute))]
    public class _Drawer_IntRange : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) + 16;
        }

        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Now draw the property as a Slider or an IntSlider based on whether it’s a float or integer.
            if (property.type != typeof(IntRange).Name)
                Debug.LogWarning("Use only with IntRange type");
            else
            {
                IntRangeAttribute range = (IntRangeAttribute)attribute;
                SerializedProperty minValue = property.FindPropertyRelative("RangeStart");
                SerializedProperty maxValue = property.FindPropertyRelative("RangeEnd");
                float newMin = minValue.intValue;
                float newMax = maxValue.intValue;

                float xDivision = position.width * 0.4f;
                float xLabelDiv = xDivision * 0.125f;

                float yDivision = position.height * 0.5f;
                EditorGUI.LabelField(new Rect(position.x, position.y, xDivision, yDivision)
                , label);


                Rect mmRect = new Rect(position.x + xDivision + xLabelDiv, position.y, position.width - (xDivision + xLabelDiv * 2), yDivision);

                EditorGUI.MinMaxSlider(mmRect, ref newMin, ref newMax, range.MinLimit, range.MaxLimit);


                //to deal with rounding on negative values:
                int newMinI = (int)(newMin - (float)range.MinLimit) + range.MinLimit;
                int newMaxI = (int)(newMax - (float)range.MinLimit) + range.MinLimit;

                //left label
                Rect minRangeRect = new Rect(position.x + xDivision, position.y, xLabelDiv, yDivision);
                minRangeRect.x += xLabelDiv * 0.5f - 12;
                minRangeRect.width = 24;
                EditorGUI.LabelField(minRangeRect, range.MinLimit.ToString());

                //right label
                Rect maxRangeRect = new Rect(minRangeRect);
                maxRangeRect.x = mmRect.xMax;
                maxRangeRect.x = mmRect.xMax + xLabelDiv * 0.5f - 12;
                maxRangeRect.width = 24;
                EditorGUI.LabelField(maxRangeRect, range.MaxLimit.ToString());

                int totalRange = Mathf.Max(range.MaxLimit - range.MinLimit, 1);
                Rect minLabelRect = new Rect(mmRect);
                minLabelRect.x = minLabelRect.x + minLabelRect.width * ((newMin - range.MinLimit) / totalRange);
                minLabelRect.x -= 12;
                minLabelRect.y += yDivision;
                minLabelRect.width = 24;
                newMinI = Mathf.Clamp(EditorGUI.IntField(minLabelRect, newMinI), range.MinLimit, newMaxI);
                //EditorGUI.LabelField(minLabelRect, newMin.ToString());//old style non moving label

                Rect maxLabelRect = new Rect(mmRect);
                maxLabelRect.x = maxLabelRect.x + maxLabelRect.width * ((newMax - range.MinLimit) / totalRange);
                maxLabelRect.x -= 12;
                maxLabelRect.x = Mathf.Max(maxLabelRect.x, minLabelRect.xMax);
                maxLabelRect.y += yDivision;
                maxLabelRect.width = 24;
                newMaxI = Mathf.Clamp(EditorGUI.IntField(maxLabelRect, newMaxI), newMinI, range.MaxLimit);
                //EditorGUI.LabelField(maxLabelRect, newMax.ToString());//old style non moving label


                minValue.intValue = (int)newMin;
                maxValue.intValue = (int)newMax;
            }
        }
    }
}
#endif