#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;

public class MiniBeizerDragListenerEditor : EditorWindow
{
    [MenuItem("Window/Seaweed/MiniBeizer 对象拖动监听")]
    public static void Init()
    {
        // 打开窗口
        MiniBeizerDragListenerEditor window = (MiniBeizerDragListenerEditor)EditorWindow.GetWindow(typeof(MiniBeizerDragListenerEditor));
        window.Show();
    }

    void OnEnable()
    {
        // 注册拖动更新和拖动执行的回调
        SceneView.duringSceneGui += DuringSceneGUI;
    }

    void OnDisable()
    {
        // 取消注册拖动更新和拖动执行的回调
        SceneView.duringSceneGui -= DuringSceneGUI;
    }

    void DuringSceneGUI(SceneView sceneView)
    {
        // 检查是否有物体被拖动
        if (Event.current.type == EventType.DragUpdated)
        {
            // 可以在这里设置允许拖动的物体类型
            DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
        }
       /* else if (Event.current.type == EventType.DragPerform)
        {
            // 执行拖动操作
            DragAndDrop.AcceptDrag();

            // 获取拖动的物体
            foreach (var objectReferences in DragAndDrop.objectReferences)
            {
                GameObject gameObject = objectReferences as GameObject;
                if (gameObject != null)
                {
                    // 你可以在这里处理被拖动的物体
                    DebugUtil.Log("Dragged GameObject: " + gameObject.name);
                }
            }
        }*/
        else if (Event.current.type == EventType.MouseDrag && Event.current.button == 0 )
        {
            /* 射线
            Event e = Event.current;
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // 在这里处理被拖动的物体
                DebugUtil.Log("Mouse dragged over " + hit.transform.gameObject.name);
            }
            */

            /*
            // 获取当前场景
            Scene currentScene = SceneManager.GetActiveScene();
            // 打印当前场景的名称
            DebugUtil.Log("当前场景名称: " + currentScene.name);
            */


            // 获取[Scene窗口]当前选择的物体
            GameObject selectedObject = Selection.activeGameObject;
            if (selectedObject != null)
            {
                DebugUtils.Log("Selected Object: " + selectedObject.name);

                MiniBezier bz = selectedObject.transform.parent.GetComponent<MiniBezier>();
                if (bz != null)
                {
                    bz.DrawCurve();
                }
            }
            else
            {
                DebugUtils.Log("No object selected.");
            }



            DebugUtils.Log($"@ == i am MouseDrag");
        }
        DebugUtils.Log( $"@ == {Enum.GetName(typeof(EventType), Event.current.type)}" );
    }

}
#endif