#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
public class AssetPathInfo : Editor
{

    /// <summary>
    /// ��ȡԤ������Դ·����
    /// </summary>
    /// <param name="gameObject"></param>
    /// <returns></returns>
    public static string GetPrefabAssetPath(GameObject gameObject)
    {
#if UNITY_EDITOR
        // Project�е�Prefab��Asset����Instance
        if (UnityEditor.PrefabUtility.IsPartOfPrefabAsset(gameObject))
        {
            // Ԥ������Դ��������
            return UnityEditor.AssetDatabase.GetAssetPath(gameObject);
        }

        // Scene�е�Prefab Instance��Instance����Asset
        if (UnityEditor.PrefabUtility.IsPartOfPrefabInstance(gameObject))
        {
            // ��ȡԤ������Դ
            var prefabAsset = UnityEditor.PrefabUtility.GetCorrespondingObjectFromOriginalSource(gameObject);
            return UnityEditor.AssetDatabase.GetAssetPath(prefabAsset);
        }

        // PrefabMode�е�GameObject�Ȳ���InstanceҲ����Asset
        var prefabStage = UnityEditor.Experimental.SceneManagement.PrefabStageUtility.GetPrefabStage(gameObject);
        if (prefabStage != null)
        {
            // Ԥ������Դ��prefabAsset = prefabStage.prefabContentsRoot
            return prefabStage.prefabAssetPath;
        }
#endif

        // ����Ԥ����
        return null;
    }


}

#endif