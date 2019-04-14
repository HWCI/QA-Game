#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace creXa.GameBase.Graphics
{
    [CustomEditor(typeof(ZThemeObjZS)), CanEditMultipleObjects]
    public class _Editor_ZThemeObjZS : ZEditor4ZThemeObj<ZThemeObjZS>
    {

    }
}

#endif