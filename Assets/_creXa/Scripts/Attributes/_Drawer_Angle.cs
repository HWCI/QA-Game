#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace creXa.GameBase
{

    [CustomPropertyDrawer(typeof(AngleAttribute))]
    public class _Drawer_Angle : PropertyDrawer
    {

        static Vector2 mousePos;
        static Texture2D KnobBack = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/_creXa/Textures/Editor/Dial.png");
        static Texture2D Knob = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/_creXa/Textures/Editor/DialButton.png");
        static float height = 50;

        AngleAttribute attr
        {
            get { return (AngleAttribute)attribute; }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.floatValue = FloatAngle(ObjectNames.NicifyVariableName(property.name), position, property.floatValue, attr.snap, attr.min, attr.max);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return height;
        }

        public static float FloatAngle(string propertyName, Rect rect, float value)
        {
            return FloatAngle(propertyName, rect, value, -1, -1, -1);
        }

        public static float FloatAngle(string propertyName, Rect rect, float value, float snap)
        {
            return FloatAngle(propertyName, rect, value, snap, -1, -1);
        }

        public static float FloatAngle(string propertyName, Rect rect, float value, float snap, float min, float max)
        {
            return FloatAngle(propertyName, rect, value, snap, min, max, Vector2.up);
        }

        public static float FloatAngle(string propertyName, Rect rect, float value, float snap, float min, float max, Vector2 zeroVector)
        {
            int id = GUIUtility.GetControlID(FocusType.Passive, rect);

            Rect namelabel = new Rect(rect.x, rect.y + (rect.height / 2) - 9, EditorGUIUtility.labelWidth, 18);
            EditorGUI.LabelField(namelabel, propertyName);

            float originalValue = value;
            Rect knobRect = new Rect(namelabel.x + namelabel.width, rect.y, rect.height, rect.height);

            float delta;
            if (min != max)
                delta = ((max - min) / 360);
            else
                delta = 1;

            if (Event.current != null)
            {
                if (Event.current.type == EventType.MouseDown && knobRect.Contains(Event.current.mousePosition))
                {
                    GUIUtility.hotControl = id;
                    mousePos = Event.current.mousePosition;
                }
                else if (Event.current.type == EventType.MouseUp && GUIUtility.hotControl == id)
                {
                    GUIUtility.hotControl = 0;
                }
                else if (Event.current.type == EventType.MouseDrag && GUIUtility.hotControl == id)
                {
                   // Vector2 move = mousePos - Event.current.mousePosition;

                    //if ( knobRect.Contains(mousePosition)  )
                    {
                        Vector2 mouseStartDirection = (mousePos - knobRect.center).normalized;
                        float startAngle = CalculateAngle(Vector2.up, mouseStartDirection);

                        Vector2 mouseNewDirection = (Event.current.mousePosition - knobRect.center).normalized;
                        float newAngle = CalculateAngle(Vector2.up, mouseNewDirection);


                        float sign = Mathf.Sign(newAngle - startAngle);
                        float delta2 = Mathf.Min(Mathf.Abs(newAngle - startAngle), Mathf.Abs(newAngle - startAngle + 360f), Mathf.Abs(newAngle - startAngle - 360f));
                        value -= delta2 * sign;
                    }

                    if (snap > 0)
                    {
                        float mod = value % snap;

                        if (mod < (delta * 3) || Mathf.Abs(mod - snap) < (delta * 3))
                            value = Mathf.Round(value / snap) * snap;
                    }

                    if (value != originalValue)
                    {
                        mousePos = Event.current.mousePosition;
                        GUI.changed = true;
                    }
                }
            }

            float angleOffset = (CalculateAngle(Vector2.up, zeroVector) + 360f) % 360f;

            GUI.DrawTexture(knobRect, KnobBack);
            Matrix4x4 matrix = GUI.matrix;

            if (min != max)
                GUIUtility.RotateAroundPivot((angleOffset + value) * (360 / (max - min)), knobRect.center);
            else
                GUIUtility.RotateAroundPivot((angleOffset + value), knobRect.center);

            GUI.DrawTexture(knobRect, Knob);
            GUI.matrix = matrix;

            Rect label = new Rect(knobRect.x + knobRect.height + 10, rect.y + (rect.height / 2) - 9, rect.height, 18);
            value = EditorGUI.FloatField(label, value);

            if (min != max)
                value = Mathf.Clamp(value, min, max);

            return value;
        }

        public static float CalculateAngle(Vector3 from, Vector3 to)
        {
            Vector3 right = Vector3.right;
            float angle = Vector3.Angle(from, to);
            return (Vector3.Angle(right, to) > 90f) ? 360f - angle : angle;
        }
    }
}
#endif