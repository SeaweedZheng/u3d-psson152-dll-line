using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreateLocalText : MonoBehaviour
{
    // ָ��Prefab��·��
    private const string PREFAB_PATH = "Assets/GameRes/Games/LocalText.prefab";

    // ��Ӳ˵��Unity�˵�����Create��
    [MenuItem("GameObject/UI/LocalText", false, 100)]
    public static void Create()
    {
        // ȷ����ǰ��ѡ�е�GameObject
        GameObject parent = Selection.activeGameObject;
        if (parent == null)
        {
            Debug.LogError("No active GameObject selected to create LocalText as a child.");
            return;
        }

        // ����Prefab
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PREFAB_PATH);
        if (prefab == null)
        {
            Debug.LogError("Prefab not found at path: " + PREFAB_PATH);
            return;
        }

        // ʵ����Prefab����Ϊ�ӽڵ���ӵ�parent��
        GameObject instance = Instantiate(prefab, parent.transform);
        instance.transform.name = "LocalText";
        // ���Ը�����Ҫ����instance���������ԣ���λ�á���ת��

        // ����Unity�༭������ʾ�µ�GameObject
        EditorUtility.SetDirty(instance);
        SceneView.lastActiveSceneView.Focus();
    }
}
