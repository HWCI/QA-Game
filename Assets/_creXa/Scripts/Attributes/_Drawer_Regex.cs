#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;

namespace creXa.GameBase
{
    [CustomPropertyDrawer(typeof(RegexAttribute))]
    public class _Drawer_Regex : PropertyDrawer
    {

        const int helpHeight = 30;
        const int textHeight = 16;

        RegexAttribute regex { get { return ((RegexAttribute)attribute); } }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (IsValid(property))
                return base.GetPropertyHeight(property, label);
            else
                return base.GetPropertyHeight(property, label) + helpHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect textFieldPosition = position;
            textFieldPosition.height = textHeight;
            DrawTextField(textFieldPosition, property, label);

            Rect helpPosition = EditorGUI.IndentedRect(position);
            helpPosition.y += textHeight;
            helpPosition.height = helpHeight;
            DrawHelpBox(helpPosition, property);

        }

        void DrawTextField(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginChangeCheck();
            string value = EditorGUI.TextField(position, label, property.stringValue);
            if (EditorGUI.EndChangeCheck())
                property.stringValue = value;
        }

        void DrawHelpBox(Rect position, SerializedProperty property)
        {
            if (IsValid(property)) return;

            EditorGUI.HelpBox(position, regex.helpMessage, MessageType.Error);
        }

        bool IsValid(SerializedProperty property)
        {
            return Regex.IsMatch(property.stringValue, regex.pattern);
        }

    }
}
#endif
