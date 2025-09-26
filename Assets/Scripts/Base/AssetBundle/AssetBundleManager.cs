using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Security.Permissions;



/// <summary>
/// ��ע�⡿����ű�׼������
/// </summary>
public class AssetBundleManager : MonoBehaviour
{
    private Dictionary<string, AssetBundle> m_ab = new Dictionary<string, AssetBundle>();
    private AssetBundle soundAB;
    private static AssetBundleManager _instance;
    public static AssetBundleManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AssetBundleManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("AssetBundleManager");
                    _instance = obj.AddComponent<AssetBundleManager>();
                }
            }
            return _instance;
        }
    }

    public AssetBundle AddAssetBundle(string key, string path)
    {
        if (m_ab.ContainsKey(key) == false)
        {
            //UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(path);
            //request.SendWebRequest();
            //while (!request.isDone && string.IsNullOrEmpty(request.error))
            //{

            //}
            //AssetBundle ab = DownloadHandlerAssetBundle.GetContent(request);
            //Debug.Log("����AB�ļ� ·��Ϊ" + path);
            //AssetBundle ab = AssetBundle.LoadFromMemory(File.ReadAllBytes(path));
            AssetBundle ab = AssetBundle.LoadFromFile(path);
            m_ab.Add(key, ab);
            return ab;
        }
        else
        {
            return m_ab[key];
            //Debug.Log("�Ѿ�������AB");
        }
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="key">�ļ�����abc </param>
    /// <param name="path">·������ Application.persistentDataPath + "/GameRes" + "/" +  A/B/C/abc.unity3d </param>
    /// <param name="callback"></param>
    public void GetAssetBundleAsync(string key, string path, UnityAction<AssetBundle> callback)
    {
        StartCoroutine(AddAssetBundleAsync(key, path, callback));
    }

    public IEnumerator AddAssetBundleAsync(string key, string path, UnityAction<AssetBundle> callback)
    {
        if (m_ab.ContainsKey(key) == false)
        {
            AssetBundleCreateRequest request ;
            request = AssetBundle.LoadFromFileAsync(path);
            yield return request;
            AssetBundle ab = request.assetBundle;
            if(ab == null)
            {
                Debug.Log("�޷�����AssetBundle path = " +  path);
                yield break;
            }
            m_ab.Add(key, ab);
            callback(ab);
        }
        else
        {
            callback(m_ab[key]);
        }
        yield return null;
    }

    public void PreLoadSoundRes(UnityAction action)
    {
        StartCoroutine(IEPreLoadSoundRes(action));
    }

    private IEnumerator IEPreLoadSoundRes(UnityAction callback)
    {
        string path = Application.persistentDataPath + "/GameRes/sound/gamesound.unity3d";
        AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(path);
        yield return request;
        soundAB = request.assetBundle;
        if (soundAB == null)
        {
            yield break;
        }
        callback();
    }

    public AudioClip GetSoundAB(string name)
    {
        AudioClip clip = null;
        clip = soundAB.LoadAsset<AudioClip>(name);
        return clip;
    }

    public bool ContainsKey(string key)
    {
        return m_ab.ContainsKey(key);
    }

    public T GetAssetBundleObject<T>(string key) where T : UnityEngine.Object
    {
        // Debug.Log("��AB key name = " + key);
        if (m_ab.ContainsKey(key))
        {
            return m_ab[key].LoadAsset<T>(key);
        }
        Debug.Log("û�д�AB Name = " + key);
        return null;
    }

    public AudioClip GetAssetBundleAudio(string asset)
    {
        if (m_ab.ContainsKey("sound"))
        {
            return m_ab["sound"].LoadAsset<AudioClip>(asset);
        }
        Debug.Log("û�д����� name = " + asset);
        return null;
    }

    public void UnLoadAssetBundle()
    {
        foreach (AssetBundle ab in m_ab.Values)
        {
            Debug.Log("ж��AB :" + ab.name);
            ab.Unload(true);
        }
    }

}