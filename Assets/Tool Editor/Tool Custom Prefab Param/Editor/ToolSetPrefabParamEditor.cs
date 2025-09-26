#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;



[CustomEditor(typeof(ToolSetPrefabParam))]
public class ToolSetPrefabParamEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ToolSetPrefabParam script = target as ToolSetPrefabParam; // ���ƹ�����


        if (GUILayout.Button("һ���޸Ľڵ������з���������Text�Ĳ���VerticalOverflow = Overflow"))
        {
            Debug.Log("һ���޸Ľڵ������з���������Text�Ĳ���VerticalOverflow = Overflow");
            script.ChangeTextVerticalOverflow();
        }

        if (GUILayout.Button("һ���޸Ľڵ�����Text�Ĳ���VerticalOverflow = Overflow"))
        {
            Debug.Log("һ���޸Ľڵ�����Text�Ĳ���VerticalOverflow = Overflow");
            script.ChangeAllTextVerticalOverflow();
        }


        if (GUILayout.Button("һ���޸Ľڵ�����(Include Disactive)Text�Ĳ���VerticalOverflow = Overflow"))
        {
            Debug.Log("һ���޸Ľڵ�����(Include Disactive)Text�Ĳ���VerticalOverflow = Overflow");
            script.ChangeAllTextVerticalOverflowIncludeDisactive();
        }


    }
}


#endif