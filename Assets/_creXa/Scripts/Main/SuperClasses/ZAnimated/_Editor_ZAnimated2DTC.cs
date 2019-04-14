#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace creXa.GameBase
{
    [CustomEditor(typeof(ZAnimated2DTC)), CanEditMultipleObjects]
    public class _Editor_ZAnimated2DTC : ZEditor4ZAnimated2DTC<ZAnimated2DTC>
    {

    }
}

#endif