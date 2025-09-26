using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System.IO;

/// <summary>
/// 
/// </summary>
/// <remark>
/// * ab������Сд����games/a/b/c/efg.unity3d 
/// * һ����Դ��Ӧһ��ab��������Դ����Сд���Ҳ�����׺��: efg ; ����Դ����ԭ��-��д���Ҳ�����׺��: Efg 
/// * ��Դ·������Assets/GameRes/Games/A/B/C/Efg.prefab  ��Ӧ��Դ���� games/a/b/c/efg.unity3d 
/// 
/// # ���
/// * ��Դ·������"Assets/GameRes/Games/PssOn00152/Prefabs/SpriteIcon.prefab"
/// * ��Դ����ab������"games/psson00152/prefabs/spriteicon.unity3d"
/// * ������Դ����·���� Application.persistentDataPath + "/GameRes" + "/" + "games/psson00152/prefabs/spriteicon.unity3d"
/// * ����Դ��ab��������ʱ��ʹ�õ����ƣ�"spriteicon"
/// </remark>
public class AssetBundleManager02 : MonoSingleton<AssetBundleManager02>
{

    /// <summary>  �洢�Ѿ����ص� AB �� ��k:v == ab������ab����</summary>
    private Dictionary<string, AssetBundle> loadedAssetBundles = new Dictionary<string, AssetBundle>();

    /// <summary>  �洢ÿ�� AB �������ü��� </summary>
    private Dictionary<string, int> referenceCounts = new Dictionary<string, int>();


    AssetBundleManifest _assetBundleManifest;
    /// <summary>  �洢 AssetBundleManifest </summary>
    private AssetBundleManifest assetBundleManifest
    {
        get
        {
            if (_assetBundleManifest == null)
                LoadManifestBundle();
            return _assetBundleManifest;
        }
    }



    /// <summary> ab����·�� </summary>
    // string abRootFolder => Application.persistentDataPath + "/GameRes";
    string abRootFolder => PathHelper.abDirLOCPTH;

    /// <summary> ��Manifest�ļ����� </summary>
    string manifestBundleName => "GameRes";
    private void LoadManifestBundle()
    {
        _assetBundleManifest = null;

        string localManifestPath = Path.Combine(abRootFolder, manifestBundleName);

        if (File.Exists(localManifestPath))
        {

            AssetBundle manifestBundle = AssetBundle.LoadFromFile(localManifestPath);
            if (manifestBundle != null)
            {
                // �Ӽ��ص� AB ����ʹ�� LoadAsset ������ȡ AssetBundleManifest ����
                _assetBundleManifest = manifestBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                //���� Unload(false) ����ж�� AB �����������Ѽ��ص� AssetBundleManifest ����
                manifestBundle.Unload(false);
            }
            else
            {
                DebugUtils.LogError("Failed to load manifest bundle.");
            }
        }
        else
        {
            DebugUtils.LogError("can not find manifest bundle.");
        }
    }




    /// <summary>
    /// 
    /// </summary>
    /// <param name="bundleName">games/a/b/c/efg.unity3d</param>
    /// <returns></returns>
    private IEnumerator _LoadAssetBundleAsync(string bundleName, UnityAction<AssetBundle> callback)
    {
        // �������ü���
        if (!referenceCounts.ContainsKey(bundleName))
        {
            referenceCounts[bundleName] = 0;
        }
        referenceCounts[bundleName]++;

        // ����������
        string[] dependencies = assetBundleManifest.GetAllDependencies(bundleName);
        foreach (string dependency in dependencies)
        {
            if (!loadedAssetBundles.ContainsKey(dependency))
            {
                if (!referenceCounts.ContainsKey(dependency))
                {
                    referenceCounts[dependency] = 0;
                }
                referenceCounts[dependency]++;

                // AssetBundle dependencyBundle = AssetBundle.LoadFromFile(getRootFolder + "/" + dependency); //ͬ��

                string path = abRootFolder + "/" + dependency;
                AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(path);  // �첽
                yield return request;
                AssetBundle dependencyBundle = request.assetBundle;


                if (dependencyBundle != null)
                {
                    loadedAssetBundles.Add(dependency, dependencyBundle);
                    DebugUtils.Log("Loaded dependency: " + dependency);
                }
                else
                {
                    DebugUtils.LogError("Failed to load dependency: " + dependency);
                }
            }
            else
            {
                referenceCounts[dependency]++;
            }
        }

        // ����Ŀ�� AB ��
        if (!loadedAssetBundles.ContainsKey(bundleName))
        {
            // AssetBundle targetBundle = AssetBundle.LoadFromFile(getRootFolder + "/" + bundleName);

            string path = abRootFolder + "/" + bundleName;
            AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(path);
            yield return request;
            AssetBundle targetBundle = request.assetBundle;

            if (targetBundle != null)
            {
                loadedAssetBundles.Add(bundleName, targetBundle);
                DebugUtils.Log("Loaded asset bundle: " + bundleName);
            }
            else
            {
                DebugUtils.LogError("Failed to load asset bundle: " + bundleName);
            }
        }

        callback?.Invoke(loadedAssetBundles.ContainsKey(bundleName) ? loadedAssetBundles[bundleName] : null);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="assetPathOrBundleName">
    /// * "Assets/GameRes/Games/PssOn00152/Prefabs/SpriteIcon.prefab" �� 
    /// * "games/psson00152/prefabs/spriteicon.unity3d"
    /// </param>
    /// <returns>"games/psson00152/prefabs/spriteicon.unity3d"</returns>
    /// <remarks>
    /// suffix ����׺
    /// postfix : ǰ׺
    /// </remarks>
    public string GetBundleName(string assetPathOrBundleName)
    {
        string result = assetPathOrBundleName.ToLower();

        string prefixToRemove = "Assets/GameRes/".ToLower();

        if (result.StartsWith(prefixToRemove))
        {
            result = result.Substring(prefixToRemove.Length);  //ȥ�� "assets/gameres/"
        }
        string[] str = result.Split('/');
        string fileNameSuffix = str[str.Length - 1];
        if (fileNameSuffix.Contains("."))
        {
            string[] str01 = fileNameSuffix.Split('.');
            int leg = str01[str01.Length - 1].Length + 1;  //".png �� .prefab"
            result = result.Substring(0, result.Length - leg);
        }
        result += ".unity3d";

        return result;
    }


    public AssetBundle LoadAssetBundle(string bundleName)
    {
        // �������ü���
        if (!referenceCounts.ContainsKey(bundleName))
        {
            referenceCounts[bundleName] = 0;
        }
        referenceCounts[bundleName]++;

        // ����������
        string[] dependencies = assetBundleManifest.GetAllDependencies(bundleName);
        foreach (string dependency in dependencies)
        {
            if (!loadedAssetBundles.ContainsKey(dependency))
            {
                if (!referenceCounts.ContainsKey(dependency))
                {
                    referenceCounts[dependency] = 0;
                }
                referenceCounts[dependency]++;

                string path = abRootFolder + "/" + dependency;
                AssetBundle dependencyBundle = AssetBundle.LoadFromFile(path); //ͬ��

                if (dependencyBundle != null)
                {
                    loadedAssetBundles.Add(dependency, dependencyBundle);
                    DebugUtils.Log("Loaded dependency: " + dependency);
                }
                else
                {
                    DebugUtils.LogError("Failed to load dependency: " + dependency);
                }
            }
            else
            {
                referenceCounts[dependency]++;
            }
        }

        // ����Ŀ�� AB ��
        if (!loadedAssetBundles.ContainsKey(bundleName))
        {
            string path = abRootFolder + "/" + bundleName;
            AssetBundle targetBundle = AssetBundle.LoadFromFile(path); //ͬ��

            if (targetBundle != null)
            {
                loadedAssetBundles.Add(bundleName, targetBundle);
                DebugUtils.Log("Loaded asset bundle: " + bundleName);
            }
            else
            {
                DebugUtils.LogError("Failed to load asset bundle: " + bundleName);
            }
        }

        return loadedAssetBundles.ContainsKey(bundleName) ? loadedAssetBundles[bundleName] : null;
    }




    /// <summary>
    /// 
    /// </summary>
    /// <param name="bundleName"></param>
    /// <param name="unloadAllLoadedObjects">ж�� AssetBundle ����������ж���Ѿ��Ӹ� AssetBundle �м��ص���Դ��</param>
    public void UnloadAssetBundle(string bundleName, bool unloadAllLoadedObjects = false)
    {
        if (referenceCounts.ContainsKey(bundleName))
        {
            referenceCounts[bundleName]--;
            if (referenceCounts[bundleName] <= 0)
            {
                // ж��������
                string[] dependencies = assetBundleManifest.GetAllDependencies(bundleName);
                foreach (string dependency in dependencies)
                {
                    if (referenceCounts.ContainsKey(dependency))
                    {
                        referenceCounts[dependency]--;
                        if (referenceCounts[dependency] <= 0)
                        {
                            if (loadedAssetBundles.ContainsKey(dependency))
                            {
                                loadedAssetBundles[dependency].Unload(unloadAllLoadedObjects);
                                loadedAssetBundles.Remove(dependency);
                                referenceCounts.Remove(dependency);
                                DebugUtils.Log("Unloaded dependency: " + dependency);
                            }
                        }
                    }
                }

                // ж��Ŀ�� AB ��
                if (loadedAssetBundles.ContainsKey(bundleName))
                {
                    loadedAssetBundles[bundleName].Unload(unloadAllLoadedObjects);
                    loadedAssetBundles.Remove(bundleName);
                    referenceCounts.Remove(bundleName);
                    DebugUtils.Log("Unloaded asset bundle: " + bundleName);
                }
            }
        }
    }


    protected override void OnDestroy()
    {
        // ж�������Ѽ��ص� AB ��
        foreach (KeyValuePair<string, AssetBundle> pair in loadedAssetBundles)
        {
            pair.Value.Unload(true);
        }
        loadedAssetBundles.Clear();
        referenceCounts.Clear();

        base.OnDestroy();
    }



    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="bundleName">"games/a/b/c/efg.unity3d"</param>
    /// <param name="assetName">"efg"</param>
    /// <returns></returns>
    public T GetAssetBundleObject<T>(string bundleName, string assetName) where T : UnityEngine.Object
    {
        // DebugUtil.Log("��AB key name = " + key);
        if (loadedAssetBundles.ContainsKey(bundleName))
        {
            return loadedAssetBundles[bundleName].LoadAsset<T>(assetName);
            DebugUtils.Log($"ab����{bundleName} û����Դ �� {assetName}");
        }
        DebugUtils.Log($" ����ȡ��Դ��{assetName}ʱ��û���ȼ���ab����{bundleName}��");
        return null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="bundleName">"games/a/b/c/efg.unity3d"</param>
    /// <returns></returns>
    public T GetAssetBundleObject<T>(string bundleName) where T : UnityEngine.Object
    {
        string[] str = bundleName.Split('/');
        string assetName = str[str.Length - 1].Replace(".unity3d", "");

        if (loadedAssetBundles.ContainsKey(bundleName))
        {
            return loadedAssetBundles[bundleName].LoadAsset<T>(assetName);
        }
        DebugUtils.Log($" ����ȡ��Դ��{assetName}ʱ��û���ȼ���ab����{bundleName}��");
        return null;
    }

    public void GetAssetBundleObjectAsync<T>(string bundleName, Action<T> onFinishCallback) where T : UnityEngine.Object
    {
        StartCoroutine(_GetAssetBundleObjectAsync<T>(bundleName, onFinishCallback));
    }

    IEnumerator _GetAssetBundleObjectAsync<T>(string bundleName, Action<T> onFinishCallback = null) where T : UnityEngine.Object
    {
        string[] str = bundleName.Split('/');
        string assetName = str[str.Length - 1].Replace(".unity3d", "");
        T loadedAsset = null;
        if (loadedAssetBundles.ContainsKey(bundleName))
        {
            // 2. �첽����ָ�����͵���Դ
            AssetBundleRequest request = loadedAssetBundles[bundleName].LoadAssetAsync<T>(assetName);

            // 3. �ȴ��������
            yield return request;

            // ��ȡ���ص���Դ
            //UnityEngine.Object loadedAsset = request.asset as T;
            loadedAsset = request.asset as T;
        }
        //DebugUtil.Log($" ����ȡ��Դ��{assetName}ʱ��û���ȼ���ab����{bundleName}��");
        onFinishCallback?.Invoke(loadedAsset);
        yield return loadedAsset;
    }


    public void GetAssetBundleAsync(string bundleName, UnityAction<AssetBundle> callback)
    {
        StartCoroutine(_LoadAssetBundleAsync(bundleName, callback));
    }

}