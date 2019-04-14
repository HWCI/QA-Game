#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace creXa.GameBase
{
    [CustomEditor(typeof(ZThemeObj)), CanEditMultipleObjects]
    public class _Editor_ZThemeObj : ZEditor4ZThemeObj<ZThemeObj>
    {

    }
}

#endif