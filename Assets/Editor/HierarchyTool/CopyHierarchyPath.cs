using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CopyHierarchyPath : EditorWindow
{
    [MenuItem("GameObject/Copy Hierarchy Path", priority = 20)]
    static void CopyPathCommand()
    {
        Transform selectedTransform = Selection.activeTransform;
        if (selectedTransform == null)
        {
            Debug.LogWarning("No GameObject selected.");
            return;
        }

        // string path = GetHierarchyPath(selectedTransform);
        string path = GetHierarchyPathWithSkip(selectedTransform,2);
        EditorGUIUtility.systemCopyBuffer = path; // ����·����������
        //Debug.Log("Copied path: " + path + " -- " + selectedTransform.root.name);
        Debug.Log("Copied path: " + path );
    }

    static string GetHierarchyPath(Transform transform)
    {
        string path = transform.name;
        Transform parent = transform.parent;

        while (parent != null)
        {
            path = parent.name + "/" + path;
            parent = parent.parent;
        }

        return path;
    }

    /// <summary>
    /// ��ȡ Transform �Ĳ㼶·��������ǰ levelsToSkip ������������ "/" ����ʣ��㼶
    /// </summary>
    /// <param name="transform">��ǰ Transform</param>
    /// <param name="levelsToSkip">Ҫ�����ĸ����㼶��</param>
    /// <returns>ƴ�Ӻ��·������ "Anchor/ButtonClose"��</returns>
    static string GetHierarchyPathWithSkip(Transform transform, int levelsToSkip)
    {
        List<Transform> hierarchyList = new List<Transform>();

        // 1. ����ǰ Transform ���丸������ List��������ǰ��
        Transform current = transform;
        while (current != null)
        {
            hierarchyList.Insert(0, current); // �������뵽�б�ͷ
            current = current.parent;
        }

        // 2. ����ǰ levelsToSkip ���㼶
        if (levelsToSkip > 0 && hierarchyList.Count > levelsToSkip)
        {
            hierarchyList.RemoveRange(0, levelsToSkip);
        }

        // 3. �� "/" ����ʣ��㼶
        string path = string.Join("/", hierarchyList.ConvertAll(t => t.name));
        return path;
    }
}