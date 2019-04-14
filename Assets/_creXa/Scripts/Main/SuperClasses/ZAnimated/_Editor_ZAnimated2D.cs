#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace creXa.GameBase
{
    [CustomEditor(typeof(ZAnimated2D)), CanEditMultipleObjects]
    public class _Editor_ZAnimated2D : ZEditor4ZAnimated<ZAnimated2D>
    {

    }
}

#endif
