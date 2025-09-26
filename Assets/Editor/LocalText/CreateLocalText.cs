using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreateLocalText : MonoBehaviour
{
    // 指定Prefab的路径
    private const string PREFAB_PATH = "Assets/GameRes/Games/LocalText.prefab";

    // 添加菜单项到Unity菜单栏的Create下
    [MenuItem("GameObject/UI/LocalText", false, 100)]
    public static void Create()
    {
        // 确保当前有选中的GameObject
        GameObject parent = Selection.activeGameObject;
        if (parent == null)
        {
            Debug.LogError("No active GameObject selected to create LocalText as a child.");
            return;
        }

        // 加载Prefab
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PREFAB_PATH);
        if (prefab == null)
        {
            Debug.LogError("Prefab not found at path: " + PREFAB_PATH);
            return;
        }

        // 实例化Prefab并作为子节点添加到parent下
        GameObject instance = Instantiate(prefab, parent.transform);
        instance.transform.name = "LocalText";
        // 可以根据需要设置instance的其他属性，如位置、旋转等

        // 更新Unity编辑器以显示新的GameObject
        EditorUtility.SetDirty(instance);
        SceneView.lastActiveSceneView.Focus();
    }
}
