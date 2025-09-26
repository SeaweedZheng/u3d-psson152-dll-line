#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;

public class MiniBeizerDragListenerEditor : EditorWindow
{
    [MenuItem("Window/Seaweed/MiniBeizer �����϶�����")]
    public static void Init()
    {
        // �򿪴���
        MiniBeizerDragListenerEditor window = (MiniBeizerDragListenerEditor)EditorWindow.GetWindow(typeof(MiniBeizerDragListenerEditor));
        window.Show();
    }

    void OnEnable()
    {
        // ע���϶����º��϶�ִ�еĻص�
        SceneView.duringSceneGui += DuringSceneGUI;
    }

    void OnDisable()
    {
        // ȡ��ע���϶����º��϶�ִ�еĻص�
        SceneView.duringSceneGui -= DuringSceneGUI;
    }

    void DuringSceneGUI(SceneView sceneView)
    {
        // ����Ƿ������屻�϶�
        if (Event.current.type == EventType.DragUpdated)
        {
            // �������������������϶�����������
            DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
        }
       /* else if (Event.current.type == EventType.DragPerform)
        {
            // ִ���϶�����
            DragAndDrop.AcceptDrag();

            // ��ȡ�϶�������
            foreach (var objectReferences in DragAndDrop.objectReferences)
            {
                GameObject gameObject = objectReferences as GameObject;
                if (gameObject != null)
                {
                    // ����������ﴦ���϶�������
                    DebugUtil.Log("Dragged GameObject: " + gameObject.name);
                }
            }
        }*/
        else if (Event.current.type == EventType.MouseDrag && Event.current.button == 0 )
        {
            /* ����
            Event e = Event.current;
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // �����ﴦ���϶�������
                DebugUtil.Log("Mouse dragged over " + hit.transform.gameObject.name);
            }
            */

            /*
            // ��ȡ��ǰ����
            Scene currentScene = SceneManager.GetActiveScene();
            // ��ӡ��ǰ����������
            DebugUtil.Log("��ǰ��������: " + currentScene.name);
            */


            // ��ȡ[Scene����]��ǰѡ�������
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