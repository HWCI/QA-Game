#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace creXa.GameBase.Graphics {

    public class _Editor_Menu : ZEditor<MonoBehaviour> {

        [MenuItem("creXa/_Clear/ZLangObj (Irreversable)", false, 0)]
        static void ClearAllZLangObj()
        {
            ZLangObj[] objs = FindObjectsOfType<ZLangObj>();
            for (int i = 0; i < objs.Length; i++)
                DestroyImmediate(objs[i]);
        }

        [MenuItem("creXa/_Clear/ZThemeObj (Irreversable)", false, 0)]
        static void ClearAllZThemeObj()
        {
            ZThemeObj[] objs = FindObjectsOfType<ZThemeObj>();
            for (int i = 0; i < objs.Length; i++)
                DestroyImmediate(objs[i]);
        }

        [MenuItem("GameObject/creXa/UI/ZPopup", false, 0)]
        static void CreatePopupIE()
        {
            Selection.activeGameObject = CreatePopup(Selection.activeTransform);
        }

        [MenuItem("GameObject/creXa/UI/ZSlideSelector", false, 0)]
        static void CreateSlideSelectorIE()
        {
            Selection.activeGameObject = CreateSlideSelector(Selection.activeTransform);
        }

        [MenuItem("GameObject/creXa/UI/ZRecordList", false, 0)]
        static void CreateRecordListIE()
        {
            Selection.activeGameObject = CreateRecordList(Selection.activeTransform);
        }

        [MenuItem("GameObject/creXa/UI/Horizontal Layout Group", false, 0)]
        static void CreateLayoutHorizontalIE()
        {
            Selection.activeGameObject = CreateLayout("H", Selection.activeTransform);
        }

        [MenuItem("GameObject/creXa/UI/Vertical Layout Group", false, 0)]
        static void CreateLayoutVerticalIE()
        {
            Selection.activeGameObject = CreateLayout("V", Selection.activeTransform);
        }

        #region ColorPickers

        [MenuItem("GameObject/creXa/UI/ColorPicker/CirclePanel", false, 0)]
        static void CreateColorPicker1IE()
        {
            Selection.activeGameObject = CreateColorPicker(Selection.activeTransform, 1);
        }

        [MenuItem("GameObject/creXa/UI/ColorPicker/FlowerPanel", false, 0)]
        static void CreateColorPicker2IE()
        {
            Selection.activeGameObject = CreateColorPicker(Selection.activeTransform, 2);
        }

        [MenuItem("GameObject/creXa/UI/ColorPicker/RingPanel", false, 0)]
        static void CreateColorPicker3IE()
        {
            Selection.activeGameObject = CreateColorPicker(Selection.activeTransform, 3);
        }

        [MenuItem("GameObject/creXa/UI/ColorPicker/RectanglePanel", false, 0)]
        static void CreateColorPicker4IE()
        {
            Selection.activeGameObject = CreateColorPicker(Selection.activeTransform, 4);
        }

        [MenuItem("GameObject/creXa/UI/ColorPicker/WheelPanel", false, 0)]
        static void CreateColorPicker5IE()
        {
            Selection.activeGameObject = CreateColorPicker(Selection.activeTransform, 5);
        }

        #endregion

        #region ZShapes

        [MenuItem("GameObject/creXa/Shapes/Star")]
        static void CreateZSStar()
        {
            Selection.activeGameObject = CreateShape<ZSStar>("Star", Selection.activeTransform);
        }

        [MenuItem("GameObject/creXa/Shapes/Radial")]
        static void CreateZSRadial()
        {
            Selection.activeGameObject = CreateShape<ZSRadial>("Radial", Selection.activeTransform);
        }

        [MenuItem("GameObject/creXa/Shapes/Circle")]
        static void CreateZSCircle()
        {
            Selection.activeGameObject = CreateShape<ZSCircle>("Circle", Selection.activeTransform);
        }

        [MenuItem("GameObject/creXa/Shapes/Custom")]
        static void CreateZSCustom()
        {
            Selection.activeGameObject = CreateShape<ZSCustom>("Custom", Selection.activeTransform);
        }

        [MenuItem("GameObject/creXa/Shapes/CustomRegular")]
        static void CreateZSCustomRegular()
        {
            Selection.activeGameObject = CreateShape<ZSCustomRegular>("CustomRegular", Selection.activeTransform);
        }

        [MenuItem("GameObject/creXa/Shapes/Rectangle")]
        static void CreateZSRectangle()
        {
            Selection.activeGameObject = CreateShape<ZSRectangle>("Rectangle", Selection.activeTransform);
        }

        #endregion

        [MenuItem("GameObject/creXa/UI/Loading", false, 0)]
        static void CreateLoadingIE()
        {
            Selection.activeGameObject = CreateLoading(Selection.activeTransform);
        }

        [MenuItem("GameObject/creXa/UI/Selectable Text", false, 0)]
        static void CreateSelectableTextIE()
        {
            Selection.activeGameObject = CreateSelectable<Text>(Selection.activeTransform);
        }

        [MenuItem("GameObject/creXa/UI/Selectable Image", false, 0)]
        static void CreateSelectableImageIE()
        {
            Selection.activeGameObject = CreateSelectable<Image>(Selection.activeTransform);
        }

        [MenuItem("GameObject/creXa/UI/CheckBox", false, 0)]
        static void CreateCheckBoxIE()
        {
            Selection.activeGameObject = CreateCheckBox(Selection.activeTransform);
        }

        [MenuItem("GameObject/creXa/UI/Radio Button", false, 0)]
        static void CreateRadioButtonImageIE()
        {
            Selection.activeGameObject = CreateRadioButton(Selection.activeTransform);
        }

        [MenuItem("GameObject/creXa/UI/Switch", false, 0)]
        static void CreateSwitchIE()
        {
            Selection.activeGameObject = CreateSwitch(Selection.activeTransform);
        }

        [MenuItem("GameObject/creXa/UI/Button", false, 0)]
        static void CreateButtonIE()
        {
            Selection.activeGameObject = CreateButton(Selection.activeTransform);
        }

        [MenuItem("GameObject/creXa/UI/Button (Multi Graphic)", false, 0)]
        static void CreateMultiGrahpicButtonIE()
        {
            Selection.activeGameObject = CreateMultiGrahpicButton(Selection.activeTransform);
        }

        [MenuItem("GameObject/creXa/UI/InputBox", false, 0)]
        static void CreateInputBoxIE()
        {
            Selection.activeGameObject = CreateInputBox(Selection.activeTransform);
        }

        [MenuItem("GameObject/creXa/UI/Scroll/ScrollRect", false, 0)]
        static void CreateScrollIE()
        {
            Selection.activeGameObject = CreateScroll(Selection.activeTransform);
        }

        [MenuItem("GameObject/creXa/UI/Scroll/Horizontal Scrollbar", false, 0)]
        static void AddScrollBarHorizontalIE()
        {
            AddScrollBarHorizontal(Selection.activeGameObject);
        }

        [MenuItem("GameObject/creXa/UI/Scroll/Vertical Scrollbar", false, 0)]
        static void AddScrollBarVerticaIEl()
        {
            AddScrollBarVertical(Selection.activeGameObject);
        }

    }
}

#endif
