using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ab��Ԥ����
/// </summary>
public class PreloadAssetBundlesHelper 
{
    /// <summary> �洢��ҪԤ���ص� AB ���б� </summary>
    
     public string markBundle;
     public  List<string> preloadBundleNames = new List<string>()
     {
         "Assets/GameRes/Games/PssOn00152 (1080x1920)/Prefabs/Page Game Main.prefab",  //��Դ·������ab����
         "games/psson00152 (1080x1920)/prefabs/page game main.unity3d", // ab����
     };

     public List<string> preloadAssetAtPath= new List<string>()
     {
         "Assets/GameRes/Games/PssOn00152 (1080x1920)/Prefabs/Page Game Main.prefab",  //��Դ·����
     };
   /*  

    public string markBundle;

    public List<string> preloadBundleNames;

    public List<string> preloadAssetAtPath;*/

    public void UnloadPreloadBundle()
    {
        ResourceManager.Instance.UnloadAssetBundle(markBundle);
    }
    public void LoadPreloadBundleAsync(Action<string> onProgressCallback, Action onFinishCallback)
    {
        _LoadPreloadBundle(0, onProgressCallback, onFinishCallback);
    }

    private void _LoadPreloadBundle(int index, Action<string> onProgressCallback, Action onFinishCallback)
    {
        if (index >= preloadBundleNames.Count)
        {
            onFinishCallback?.Invoke();
            return;
        }
        string assetPathOrBundleName = preloadBundleNames[index];
        string bundleName = AssetBundleManager02.Instance.GetBundleName(assetPathOrBundleName);

        onProgressCallback?.Invoke("preload bundle : " + bundleName);

        ResourceManager.Instance.LoadAssetBundleAsync(assetPathOrBundleName, markBundle, (ab) => {
            _LoadPreloadBundle(++index, onProgressCallback, onFinishCallback);
        });
    }

    public void LoadPreloadAssetAsync(Action<string> onProgressCallback, Action onFinishCallback)
    {
        _LoadPreloadAsset(0, onProgressCallback, onFinishCallback);
    }

    private void _LoadPreloadAsset(int index, Action<string> onProgressCallback, Action onFinishCallback)
    {
        if (index >= preloadAssetAtPath.Count)
        {
            onFinishCallback?.Invoke();
            return;
        }
        string assetPath = preloadAssetAtPath[index];

        onProgressCallback?.Invoke("preload asset : " + assetPath);

        ResourceManager.Instance.LoadPreloadAssetAsync(assetPath, (obj) => {
            _LoadPreloadAsset(++index, onProgressCallback, onFinishCallback);
        });
    }

    public void UnloadPreloadAsset()
    {
        foreach (string assetPath in preloadAssetAtPath)
        {
            ResourceManager.Instance.UnloadPreloadAsset(markBundle);
        }
    }

}
