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
        EditorGUIUtility.systemCopyBuffer = path; // 复制路径到剪贴板
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
    /// 获取 Transform 的层级路径，跳过前 levelsToSkip 个父级，并用 "/" 连接剩余层级
    /// </summary>
    /// <param name="transform">当前 Transform</param>
    /// <param name="levelsToSkip">要跳过的父级层级数</param>
    /// <returns>拼接后的路径（如 "Anchor/ButtonClose"）</returns>
    static string GetHierarchyPathWithSkip(Transform transform, int levelsToSkip)
    {
        List<Transform> hierarchyList = new List<Transform>();

        // 1. 将当前 Transform 及其父级存入 List（父级在前）
        Transform current = transform;
        while (current != null)
        {
            hierarchyList.Insert(0, current); // 父级插入到列表开头
            current = current.parent;
        }

        // 2. 跳过前 levelsToSkip 个层级
        if (levelsToSkip > 0 && hierarchyList.Count > levelsToSkip)
        {
            hierarchyList.RemoveRange(0, levelsToSkip);
        }

        // 3. 用 "/" 连接剩余层级
        string path = string.Join("/", hierarchyList.ConvertAll(t => t.name));
        return path;
    }
}