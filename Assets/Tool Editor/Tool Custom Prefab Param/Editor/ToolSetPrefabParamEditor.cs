#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;



[CustomEditor(typeof(ToolSetPrefabParam))]
public class ToolSetPrefabParamEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ToolSetPrefabParam script = target as ToolSetPrefabParam; // 绘制滚动条


        if (GUILayout.Button("一键修改节点下所有符合条件的Text的参数VerticalOverflow = Overflow"))
        {
            Debug.Log("一键修改节点下所有符合条件的Text的参数VerticalOverflow = Overflow");
            script.ChangeTextVerticalOverflow();
        }

        if (GUILayout.Button("一键修改节点下所Text的参数VerticalOverflow = Overflow"))
        {
            Debug.Log("一键修改节点下所Text的参数VerticalOverflow = Overflow");
            script.ChangeAllTextVerticalOverflow();
        }


        if (GUILayout.Button("一键修改节点下所(Include Disactive)Text的参数VerticalOverflow = Overflow"))
        {
            Debug.Log("一键修改节点下所(Include Disactive)Text的参数VerticalOverflow = Overflow");
            script.ChangeAllTextVerticalOverflowIncludeDisactive();
        }


    }
}


#endif