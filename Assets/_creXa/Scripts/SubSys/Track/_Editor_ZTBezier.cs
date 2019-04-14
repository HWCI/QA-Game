#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;

namespace creXa.GameBase
{
    [CustomEditor(typeof(ZTBezier))]
    public class _Editor_ZTBezier : ZEditor<ZTBezier>
    {
        ReorderableList pointList;

        public enum ManipulationMode
        {
            Free,
            SelectAndTransform
        }

        public enum NewKeyPointMode
        {
            LastKeyPoint,
            WorldCenter,
            LocalCenter,
            Specified,
            SpecifiedLocal
        }

        //GUIContent
        GUIContent addPointGUIC = new GUIContent("Add Point", "Add a key point to the curve");
        GUIContent deletePointGUIC = new GUIContent("X", "Delete this keypoint");
        GUIContent alwaysShowGUIC = new GUIContent("Always show", "Show curve even if not selected.");
        GUIContent gotoPointGUIC = new GUIContent("Goto", "Focus the scene camera on this point");
        GUIContent smoothGUIC = new GUIContent("o───o", "Smooth Handle");
        GUIContent brokenGUIC = new GUIContent("o─x─o", "Broken Handle");
        GUIContent LeftFreeGUIC = new GUIContent("~o", "Free Tangent");
        GUIContent LeftLinearGUIC = new GUIContent("-o", "Linear Tangent");
        GUIContent RightFreeGUIC = new GUIContent("o~", "Free Tangent");
        GUIContent RightLinearGUIC = new GUIContent("o-", "Linear Tangent");
        GUIContent closeLoopGUIC = new GUIContent("Close the loop ∞", "Close the loop");

        //Editor Variables
        bool visualFoldout;
        bool showRawValues;
        bool manipulationFoldout;
        NewKeyPointMode addPointMode;
        ManipulationMode handlePositionMode;
        ManipulationMode pointPositionMode;
        Vector3 addPos = Vector3.zero;

        //Editor Runtime
        int selectedIndex = -1;

        protected override void OnEnableRun()
        {
            base.OnEnableRun();
            SetupEditorVariables();
            SetupReorderableList();
        }

        void SetupEditorVariables()
        {
            addPointMode = (NewKeyPointMode)GetInt("addPointMode");
            handlePositionMode = (ManipulationMode)GetInt("handlePositionMode");
            pointPositionMode = (ManipulationMode)GetInt("pointPositionMode", 1);
        }

        void SetupReorderableList()
        {
            pointList = new ReorderableList(sObj, sProp["points"], true, true, false, false);
            pointList.elementHeight *= 2.2f;

            pointList.drawElementCallback = (rect, index, active, focused) =>
            {
                //each element
                Rect startRect = rect;
                rect.y += startRect.height * 0.1f;
                rect.height = EditorGUIUtility.singleLineHeight * 1.2f;
                if (index > z.points.Count - 1) return;
                rect.height -= 2;
                float fullWidth = GetSuitableWidth() - 16;
                rect.width = 30;
                fullWidth -= rect.width;
                GUI.Label(rect, "#" + (index + 1));

                rect.x += rect.width;
                rect.width = 30;
                fullWidth -= rect.width;
                if (GUI.Button(rect, z.points[index].leftTangentFree ? LeftFreeGUIC : LeftLinearGUIC))
                {
                    if (z.points[index].leftTangentFree)
                        z.SetLinearTangent(index, true);
                    else
                        z.points[index].leftTangentFree = true;
                    SceneView.RepaintAll();
                    z.SetDirty();
                }

                rect.x += rect.width;
                rect.width = 50;
                fullWidth -= rect.width;
                if (GUI.Button(rect, z.points[index].handleStyleSmooth ? smoothGUIC : brokenGUIC))
                {
                    z.points[index].handleStyleSmooth = !z.points[index].handleStyleSmooth;
                    if (z.points[index].handleStyleSmooth)
                    {
                        z.points[index].handleNext = z.points[index].handlePrev * -1;
                    }

                    SceneView.RepaintAll();
                    z.SetDirty();
                }

                rect.x += rect.width;
                rect.width = 30;
                fullWidth -= rect.width;
                if (GUI.Button(rect, z.points[index].rightTangentFree ? RightFreeGUIC : RightLinearGUIC))
                {
                    if (z.points[index].rightTangentFree)
                        z.SetLinearTangent(index, false);
                    else
                        z.points[index].rightTangentFree = true;
                    SceneView.RepaintAll();
                    z.SetDirty();
                }

                rect.x += rect.width + fullWidth - 20 - 40;
                rect.width = 40;
                fullWidth -= rect.width;
                if(GUI.Button(rect, gotoPointGUIC))
                {
                    pointList.index = index;
                    selectedIndex = index;
                    SceneView.lastActiveSceneView.LookAt(z.points[index].position);
                    SceneView.lastActiveSceneView.Repaint();
                }

                rect.x += rect.width;
                rect.width = 20;
                rect.height += EditorGUIUtility.singleLineHeight * 1.2f;
                fullWidth -= rect.width;
                if (GUI.Button(rect, deletePointGUIC))
                {
                    z.points.Remove(z.points[index]);
                    SceneView.RepaintAll();
                    z.SetDirty();
                }

                rect.x = startRect.x + 30;
                rect.y += EditorGUIUtility.singleLineHeight * 1.2f + startRect.height * 0.05f;
                rect.height = startRect.height / 2;
                fullWidth = GetSuitableWidth() - 16 - 20 - 30;
                rect.width = fullWidth;
                EditorGUI.BeginChangeCheck();
                z.points[index].position = EditorGUI.Vector3Field(rect, "", z.points[index].position);
                if (EditorGUI.EndChangeCheck())
                {
                    SceneView.RepaintAll();
                    z.SetDirty();
                }

            };

            pointList.drawHeaderCallback = rect =>
            {
                float fullWidth = GetSuitableWidth() - 16;

                rect.width = 56;
                GUI.Label(rect, "Sum: " + z.points.Count);

                rect.x += rect.width;
                fullWidth -= rect.width;
                rect.width = fullWidth - 50;
                GUI.Label(rect, "Total Distance: " + z.ApproxLength);

                rect.x += rect.width;
                rect.width = 50;
            };

            pointList.onSelectCallback = l =>
            {
                selectedIndex = l.index;
                SceneView.RepaintAll();
            };
        }

        protected override void OnInspectorGUIRun()
        {
            DrawVisual();
            DrawManipulationDropdown();

            DrawSeparate();
            DrawFastOperation();

            GUILayout.Space(3);
            DrawPointList();

            GUILayout.Space(3);
            DrawRawValues();

        }

        void DrawManipulationDropdown()
        {
            manipulationFoldout = EditorGUILayout.Foldout(manipulationFoldout, "Transform manipulation mode");
            EditorGUI.BeginChangeCheck();
            if (manipulationFoldout)
            {
                EditorGUILayout.BeginVertical("Box");
                pointPositionMode = (ManipulationMode)EditorGUILayout.EnumPopup("Point Translation", pointPositionMode);
                handlePositionMode = (ManipulationMode)EditorGUILayout.EnumPopup("Handle Translation", handlePositionMode);
                EditorGUILayout.EndVertical();
            }
            if (EditorGUI.EndChangeCheck())
            {
                SetInt("pointPositionMode", (int)pointPositionMode);
                SetInt("handlePositionMode", (int)handlePositionMode);
                SceneView.RepaintAll();
            }
        }

        void DrawVisual()
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginHorizontal();
            visualFoldout = EditorGUILayout.Foldout(visualFoldout, "Editor Settings");
            z.visual.alwaysShow = GUILayout.Toggle(z.visual.alwaysShow, alwaysShowGUIC, "radio");
            EditorGUILayout.EndHorizontal();

            if (visualFoldout)
            {
                EditorGUILayout.BeginVertical("Box");
                z.visual.pathColor = EditorGUILayout.ColorField("Path Color", z.visual.pathColor);
                z.visual.inactivePathColor = EditorGUILayout.ColorField("Inactive Path Color", z.visual.inactivePathColor);
                z.visual.handleColor = EditorGUILayout.ColorField("Handle Color", z.visual.handleColor);
                if(GUILayout.Button("Default colors"))
                {
                    z.visual = new ZTrack_Visual();
                }
                EditorGUILayout.EndVertical();
            }
            if (EditorGUI.EndChangeCheck())
            {
                SceneView.RepaintAll();
            }
        }

        void DrawFastOperation()
        {
            EditorGUI.BeginChangeCheck();
            
            GUILayout.Label("All Points Operation");
            EditorGUILayout.BeginHorizontal(BestWidth());
            if(GUILayout.Button("X = 0"))
            {
                for (int i = 0; i < z.points.Count; i++)
                    z.points[i].SetAxisTo("x", 0);
                SceneView.RepaintAll();
            }
            if (GUILayout.Button("Y = 0"))
            {
                for (int i = 0; i < z.points.Count; i++)
                    z.points[i].SetAxisTo("y", 0);
                SceneView.RepaintAll();
            }
            if (GUILayout.Button("Z = 0"))
            {
                for (int i = 0; i < z.points.Count; i++)
                    z.points[i].SetAxisTo("z", 0);
                SceneView.RepaintAll();
            }
            EditorGUILayout.EndHorizontal();

            DrawSeparate();

            z.closeLoop = EditorGUILayout.Toggle(closeLoopGUIC, z.closeLoop, "radio");
            if (EditorGUI.EndChangeCheck())
            {
                SceneView.RepaintAll();
            }
            
        }

        void DrawPointList()
        {
            pointList.DoLayoutList();

            GUILayout.Space(-10);

            GUILayout.BeginHorizontal(BestWidth());
            if (GUILayout.Button(addPointGUIC))
            {
                switch (addPointMode)
                {
                    case NewKeyPointMode.LastKeyPoint:
                        if (z.points.Count > 0)
                            z.AddLocalPointAt(z.points[z.points.Count - 1].position);
                        else
                            z.AddLocalPointAt(Vector3.zero);
                        break;
                    case NewKeyPointMode.WorldCenter:
                        z.AddPointAt(Vector3.zero);
                        break;
                    case NewKeyPointMode.LocalCenter:
                        z.AddLocalPointAt(Vector3.zero);
                        break;
                    case NewKeyPointMode.Specified:
                        z.AddPointAt(addPos);
                        break;
                    case NewKeyPointMode.SpecifiedLocal:
                        z.AddLocalPointAt(addPos);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                selectedIndex = z.points.Count - 1;
                SceneView.RepaintAll();

            }

            GUILayout.Label("at", GUILayout.Width(20));
            EditorGUI.BeginChangeCheck();
            addPointMode = (NewKeyPointMode)EditorGUILayout.EnumPopup(addPointMode);
            GUILayout.EndHorizontal();

            if(addPointMode == NewKeyPointMode.Specified || addPointMode == NewKeyPointMode.SpecifiedLocal)
            {
                addPos = EditorGUILayout.Vector3Field("", addPos, BestWidth());
            }

            if (EditorGUI.EndChangeCheck())
            {
                SetInt("addPointMode", (int)addPointMode);
            }

        }

        void DrawRawValues()
        {
            if (GUILayout.Button(showRawValues ? "Hide raw values" : "Show raw values", BestWidth()))
                showRawValues = !showRawValues;

            if (showRawValues)
            {
                foreach (ZTBezierPoint i in z.points)
                {
                    EditorGUI.BeginChangeCheck();
                    GUILayout.BeginVertical("Box");
                    Vector3 pos = EditorGUILayout.Vector3Field("Waypoint Position", i.position);
                    Vector3 posp = EditorGUILayout.Vector3Field("Previous Handle Offset", i.handlePrev);
                    Vector3 posn = EditorGUILayout.Vector3Field("Next Handle Offset", i.handleNext);
                    GUILayout.EndVertical();
                    if (EditorGUI.EndChangeCheck())
                    {
                        i.position = pos;
                        i.handlePrev = posp;
                        i.handleNext = posn;
                        SceneView.RepaintAll();
                    }
                }
            }

        }

        protected override void OnSceneGUIRun()
        {
            base.OnSceneGUIRun();
            if (z.points.Count >= 2)
            {
                for (int i = 0; i < z.points.Count; i++)
                {
                    DrawHandles(i);
                    Handles.color = Color.white;
                }

            }
        }

        void DrawHandles(int i)
        {
            Handles.color = z.visual.handleColor;
            DrawHandleLines(i);
            DrawPrevHandle(i);
            DrawNextHandle(i);
            DrawPointHandles(i);
            DrawSelectionHandles(i);
            Handles.color = Color.white;
        }

        void DrawHandleLines(int i)
        {
            if (i < z.points.Count - 1 || z.closeLoop)
                Handles.DrawLine(z.transform.position + z.points[i].position,
                    z.transform.position + z.points[i].position +  z.points[i].handleNext);
            if (i > 0 || z.closeLoop)
                Handles.DrawLine(z.transform.position + z.points[i].position,
                    z.transform.position + z.points[i].position + z.points[i].handlePrev);
        }

        void DrawPrevHandle(int i)
        {
            if (i > 0 || z.closeLoop)
            {
                EditorGUI.BeginChangeCheck();
                Vector3 pos = Vector3.zero;
                float size = HandleUtility.GetHandleSize(z.transform.position + z.points[i].position + z.points[i].handlePrev) * 0.1f;
                if (handlePositionMode == ManipulationMode.Free)
                {
                    pos = Handles.FreeMoveHandle(z.transform.position + z.points[i].position + z.points[i].handlePrev,
                        Quaternion.identity, size, Vector3.zero, Handles.SphereHandleCap);
                }
                else
                {
                    if(selectedIndex == i)
                    {
                        Handles.SphereHandleCap(0, z.transform.position + z.points[i].position + z.points[i].handlePrev,
                            Quaternion.identity, size, EventType.Repaint);
                    }
                    else if(Event.current.button != 1)
                    {
                        if(Handles.Button(z.transform.position + z.points[i].position + z.points[i].handlePrev,
                            Quaternion.identity, size, size, Handles.CubeHandleCap))
                        {
                            SelectIndex(i);
                        }
                    }
                }
                if (EditorGUI.EndChangeCheck())
                {
                    z.points[i].handlePrev = pos - z.points[i].position - z.transform.position;
                    z.points[i].leftTangentFree = true;
                    if (z.points[i].handleStyleSmooth)
                    {
                        z.points[i].handleNext = z.points[i].handlePrev * -1;
                        z.points[i].rightTangentFree = true;
                    }
                }
            }

            
        }

        void DrawNextHandle(int i)
        {
            if (i < z.points.Count - 1 || z.closeLoop)
            {
                EditorGUI.BeginChangeCheck();
                Vector3 pos = Vector3.zero;
                float size = HandleUtility.GetHandleSize(z.transform.position + z.points[i].position + z.points[i].handleNext) * 0.1f;
                if (handlePositionMode == ManipulationMode.Free)
                {
                    pos = Handles.FreeMoveHandle(z.transform.position + z.points[i].position + z.points[i].handleNext,
                        Quaternion.identity, size, Vector3.zero, Handles.SphereHandleCap);
                }
                else
                {
                    if (selectedIndex == i)
                    {
                        Handles.SphereHandleCap(0, z.transform.position + z.points[i].position + z.points[i].handleNext,
                            Quaternion.identity, size, EventType.Repaint);
                    }
                    else if (Event.current.button != 1)
                    {
                        if (Handles.Button(z.transform.position + z.points[i].position + z.points[i].handleNext,
                            Quaternion.identity, size, size, Handles.CubeHandleCap))
                        {
                            SelectIndex(i);
                        }
                    }
                }
                if (EditorGUI.EndChangeCheck())
                {
                    z.points[i].handleNext = pos - z.points[i].position - z.transform.position;
                    z.points[i].rightTangentFree = true;
                    if (z.points[i].handleStyleSmooth)
                    {
                        z.points[i].handlePrev = z.points[i].handleNext * -1;
                        z.points[i].leftTangentFree = true;
                    }
                }
            }
        }

        void DrawPointHandles(int i)
        {
            EditorGUI.BeginChangeCheck();
            Vector3 pos = Vector3.zero;
            if(pointPositionMode == ManipulationMode.SelectAndTransform)
            {
                if (i == selectedIndex)
                    pos = Handles.PositionHandle(z.transform.position + z.points[i].position, Quaternion.identity);
            }
            else
            {
                pos = Handles.FreeMoveHandle(z.transform.position + z.points[i].position, Quaternion.identity,
                    HandleUtility.GetHandleSize(z.transform.position + z.points[i].position) * 0.2f, Vector3.zero, Handles.RectangleHandleCap);
            }
            if (EditorGUI.EndChangeCheck())
            {
                z.points[i].position = pos - z.transform.position;
                if (!z.points[i].leftTangentFree)
                    z.SetLinearTangent(i, true);
                if (!z.points[i].rightTangentFree)
                    z.SetLinearTangent(i, false);
            }
        }

        void DrawSelectionHandles(int i)
        {
            if(Event.current.button != 1 && selectedIndex != i)
            {
                if(pointPositionMode == ManipulationMode.SelectAndTransform)
                {
                    float size = HandleUtility.GetHandleSize(z.transform.position + z.points[i].position) * 0.2f;
                    if (Handles.Button(z.transform.position + z.points[i].position, Quaternion.identity, size, size, Handles.CubeHandleCap))
                    {
                        SelectIndex(i);
                        SceneView.RepaintAll();
                    }
                }
            }
        }

        void SelectIndex(int i)
        {
            selectedIndex = i;
            pointList.index = i;
            Repaint();
        }
    }
}
#endif

