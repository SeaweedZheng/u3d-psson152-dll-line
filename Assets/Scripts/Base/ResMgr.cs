using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 【之后弃用】
/// </summary>
public class ResMgr : BaseManager<ResMgr>
{
    public Dictionary<string, AssetBundle> bundleDic = new Dictionary<string, AssetBundle>();


    //同步加载资源
    public T Load<T>(string name) where T : Object
    {
        T res = Resources.Load<T>(name);
        //如果对象是一个GameObject类型的 我把他实例化后 再返回出去 外部 直接使用即可
        if (res is GameObject)
            return Object.Instantiate(res);
        else//TextAsset AudioClip
            return res;
    }
    //异步加载资源
    public void LoadAsync<T>(string name, UnityAction<T> callback) where T : Object
    {
        //开启异步加载的协程
        MonoMgr.Instance.StartCoroutine(ReallyLoadAsync(name, callback));
    }

    //真正的协同程序函数  用于 开启异步加载对应的资源
    private IEnumerator ReallyLoadAsync<T>(string name, UnityAction<T> callback) where T : Object
    {
        ResourceRequest r = Resources.LoadAsync<T>(name);
        yield return r;
        if (r.asset is GameObject)
            callback(Object.Instantiate(r.asset) as T);
        else
            callback(r.asset as T);
    }

    //加载AB包
    public void LoadAssetBundle(string bundleName, string objName, UnityAction<Object> callback)
    {
        if (bundleDic.TryGetValue(bundleName, out AssetBundle bundle))
        {
            var obj = bundle.LoadAsset<Object>(objName);
            callback?.Invoke(obj);
        }
        else
            MonoMgr.Instance.StartCoroutine(LoadAssetBundleFromMemoryAsync(bundleName, objName, callback));
    }

    private IEnumerator LoadAssetBundleFromMemoryAsync(string bundleName, string objName, UnityAction<Object> callback)
    {
        AssetBundle bundle;

        AssetBundle[] loadedABs = Resources.FindObjectsOfTypeAll<AssetBundle>();
        if (loadedABs != null)
        {
            foreach (var item in loadedABs)
            {

                if (item.name == bundleName)
                {
                    bundle = item;
                    var obj = bundle.LoadAsset<Object>(objName);
                    callback?.Invoke(obj);
                    yield break;
                }
            }
        }
        AssetBundleCreateRequest createRequest = AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(AssetBundlePath + "/AssetBundles"));
        yield return createRequest;
        if (createRequest != null)
        {
            bundle = createRequest.assetBundle;
            AssetBundleManifest manifest = bundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            string[] names = manifest.GetAllAssetBundles();
            for (int i = 0; i < names.Length; i++)
            {
                createRequest = AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(AssetBundlePath + "/" + names[i]));
                yield return createRequest;
                if (createRequest != null)
                {
                    bundle = createRequest.assetBundle;
                    bundleDic[names[i]] = bundle;
                    if (names[i] == bundleName)
                    {
                        var obj = bundle.LoadAsset<GameObject>(objName);
                        callback?.Invoke(obj);
                    }
                }
            }
        }
    }

    public string AssetBundlePath
    {
        get
        {
            string path = Application.dataPath + "/StreamingAssets/AssetBundles";
            if (!Application.isEditor)
                path = Application.persistentDataPath;
            return path;
        }
    }
}
