#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace creXa.GameBase
{
    [CustomEditor(typeof(ZAnimated)), CanEditMultipleObjects]
    public class _Editor_ZAnimated : ZEditor4ZAnimated<ZAnimated>
    {

    }
}

#endif
