#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using creXa.GameBase;

namespace creXa.GameBase
{

    [CustomPropertyDrawer(typeof(FloatRangeAttribute))]
    public class _Drawer_FloatRange : PropertyDrawer
    {

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) + 16;
        }

        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Now draw the property as a Slider or an IntSlider based on whether it’s a float or integer.
            if (property.type != typeof(FloatRange).Name)
                Debug.LogWarning("Use only with FloatRange type");
            else
            {
                FloatRangeAttribute range = attribute as FloatRangeAttribute;
                SerializedProperty minValue = property.FindPropertyRelative("RangeStart");
                SerializedProperty maxValue = property.FindPropertyRelative("RangeEnd");
                float newMin = minValue.floatValue;
                float newMax = maxValue.floatValue;

                float xDivision = position.width * 0.4f;
                float xLabelDiv = xDivision * 0.125f;

                float yDivision = position.height * 0.5f;
                EditorGUI.LabelField(new Rect(position.x, position.y, xDivision, yDivision)
                , label);


                Rect mmRect = new Rect(position.x + xDivision + xLabelDiv, position.y, position.width - (xDivision + xLabelDiv * 2), yDivision);

                EditorGUI.MinMaxSlider(mmRect, ref newMin, ref newMax, range.MinLimit, range.MaxLimit);


                //to deal with rounding on negative values:
                float newMinI = newMin - range.MinLimit + range.MinLimit;
                float newMaxI = newMax - range.MinLimit + range.MinLimit;

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

                float totalRange = Mathf.Max(range.MaxLimit - range.MinLimit, 1);
                Rect minLabelRect = new Rect(mmRect);
                minLabelRect.x = minLabelRect.x + minLabelRect.width * ((newMin - range.MinLimit) / totalRange);
                minLabelRect.x -= 12;
                minLabelRect.y += yDivision;
                minLabelRect.width = 40;
                newMinI = Mathf.Clamp(EditorGUI.FloatField(minLabelRect, newMinI), range.MinLimit, newMaxI);
                //EditorGUI.LabelField(minLabelRect, newMin.ToString());//old style non moving label

                Rect maxLabelRect = new Rect(mmRect);
                maxLabelRect.x = maxLabelRect.x + maxLabelRect.width * ((newMax - range.MinLimit) / totalRange);
                maxLabelRect.x -= 12;
                maxLabelRect.x = Mathf.Max(maxLabelRect.x, minLabelRect.xMax);
                maxLabelRect.y += yDivision;
                maxLabelRect.width = 40;
                newMaxI = Mathf.Clamp(EditorGUI.FloatField(maxLabelRect, newMaxI), newMinI, range.MaxLimit);
                //EditorGUI.LabelField(maxLabelRect, newMax.ToString());//old style non moving label


                minValue.floatValue = newMin;
                maxValue.floatValue = newMax;
            }
        }

    }
}

#endif