using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using System.Diagnostics;
using System.Reflection;
#endif

namespace creXa.GameBase
{
    public static class ZMsg
    {
        public static void Log(string message = "")
        {
#if UNITY_EDITOR
            if (ZBase.It.isDebug)
            {
                StackFrame frame = new StackFrame(1);
                MethodBase method = frame.GetMethod();
                string className = method.DeclaringType.ToString();
                className = className.Substring(className.LastIndexOf(".") + 1);
                string methodName = method.Name;
                UnityEngine.Debug.LogWarning(className + ":::" + methodName + "() -> { " + message + " }");
            }         
#endif
        }
    }
}
