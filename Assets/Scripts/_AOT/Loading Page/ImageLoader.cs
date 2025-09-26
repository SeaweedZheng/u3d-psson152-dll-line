using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json.Linq;


/// <summary>
/// ͼƬ������
/// </summary>
/// <remarks>
/// * ���ػ���ͼƬ<br/>
/// * ��������ͼƬ�����浽���ػ���<br/>
/// * ����ResourcesͼƬ<br/>
/// * ���ر༭��ͼƬ<br/>
/// * ����ab���е�ͼƬ<br/>
/// * ���ر���ͼƬ<br/>
/// * ���û���ͼƬ��������<br/>
/// </remarks>
public class ImageLoader : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }



    /// <summary>
    /// ����ͼƬ
    /// </summary>
    /// <param name="path"></param>
    /// <param name="onSuccessCallback"></param>
    public void LoadImage(string path, Action<Sprite> onSuccessCallback) => StartCoroutine(_LoadImage(path,onSuccessCallback));


    // C:/Users/Administrator/Desktop/111/v2-875b04109f74c08e77d1f20e01ae3c8d_r.jpg
    // https://img1.baidu.com/it/u=884686506,881027067&fm=253&fmt=auto&app=138&f=JPEG?w=800&h=1532
    // file://C:/Users/Administrator/Desktop/111/v2-875b04109f74c08e77d1f20e01ae3c8d_r.jpg?t=1000
    IEnumerator _LoadImage(string path ,Action<Sprite> onSuccessCallback)
    {
        if (path.StartsWith("https://")|| path.StartsWith("http://") || path.StartsWith("file://"))
        {
            string flieNameMD5 = GetMD5(path);
            string localFilePath = Path.Combine(Application.persistentDataPath, "ImageCache", flieNameMD5);

            if (File.Exists(localFilePath) && isUseImageCache(flieNameMD5))
            {
                // ���ر���ͼƬ
                byte[] fileData = File.ReadAllBytes(localFilePath);
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(fileData);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                onSuccessCallback?.Invoke(sprite);
                Debug.LogWarning($"ʹ�ñ��ػ���ͼƬ: {localFilePath}");
            }
            else
            {

                string targetPth = path.Contains("?") ? 
                    $"{path}&t={DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}" : $"{path}?t={DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";
                
                using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(targetPth))
                {
                    yield return www.SendWebRequest();

                    if (www.result != UnityWebRequest.Result.Success)
                    {
                        if (File.Exists(localFilePath))
                        {
                            // ����ʧ�ܺ�Ĭ��ʹ�ñ���
                            byte[] fileData = File.ReadAllBytes(localFilePath);
                            Texture2D texture = new Texture2D(2, 2);
                            texture.LoadImage(fileData);
                            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                            onSuccessCallback?.Invoke(sprite);
                            Debug.LogWarning($"ʹ�ñ��ػ���ͼƬ: {localFilePath}");
                        }
                        else
                        {
                            Debug.LogError(www.error);
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"����ͼƬ �� {path}");

                        Texture2D texture = DownloadHandlerTexture.GetContent(www);
                        // ��������Խ�����Ӧ�õ�Sprite��
                        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                        //Debug.LogWarning("Loaded https image: " + sprite.name);

                        if (path.StartsWith("https://") || path.StartsWith("http://"))
                        {
                            // ����ͼƬ������
                            byte[] data = texture.EncodeToPNG();
                            // ��鲢����Ŀ¼
                            string directory = Path.GetDirectoryName(localFilePath);
                            if (!Directory.Exists(directory))
                            {
                                Directory.CreateDirectory(directory);
                            }
                            File.WriteAllBytes(localFilePath, data);
                            ChangeCacheInfo(flieNameMD5);
                            Debug.LogWarning("��������ز�����ͼƬ������");
                        }

                        onSuccessCallback?.Invoke(sprite);
                    }
                }
            }

        }
        else if (path.StartsWith("Assets/") &&  path.Contains("/Resources/"))
        {
            int index = path.IndexOf("/Resources/");
            // ����Resources�ļ����ڵ���Դ
            string resourcePath = path.Substring(index + "/Resources/".Length);
            if (resourcePath.Contains("."))
            {
                string[] str = resourcePath.Split('.');
                resourcePath = resourcePath.Replace($".{str[str.Length-1]}","");
            }
            Sprite sprite = Resources.Load<Sprite>(resourcePath);
            if (sprite != null)
            {
                //Debug.LogWarning("Loaded Resources image: " + sprite.name);
                onSuccessCallback?.Invoke(sprite);
            }
            else
            {
                Debug.LogError("Sprite not found in Resources: " + resourcePath);
            }
        }
        else if(path.StartsWith("Assets/"))
        {
            // ���ر��ػ����ab����Դ
            if (ApplicationSettings.Instance.IsUseHotfix())
            {
                string bundleName = GetBundleName(path);
                string pathLoc = PathHelper.abDirLOCPTH + "/" + bundleName;
                AssetBundle targetBundle = AssetBundle.LoadFromFile(pathLoc); //ͬ��

                if (targetBundle != null)
                {
                    string[] str = bundleName.Split('/');
                    string assetName = str[str.Length - 1].Replace(".unity3d", "");

                    Sprite sprite = targetBundle.LoadAsset<Sprite>(assetName);
                    targetBundle.Unload(false);

                    onSuccessCallback?.Invoke(sprite);
                }
                else
                {
                    Debug.LogError("Sprite not found in Resources: " + pathLoc);
                }
            }
            else
            {
#if UNITY_EDITOR
                // ���ر༭����Դ
                Sprite sprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(path);
                onSuccessCallback?.Invoke(sprite);
#endif
            }

        }
        else
        {
           // ����ͼƬ
           if (File.Exists(path))
            {
                Debug.LogWarning($"���ر���ͼƬ�� {path}");
                // ���ر���ͼƬ
                byte[] fileData = File.ReadAllBytes(path);
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(fileData);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                onSuccessCallback?.Invoke(sprite);
            }
            else
            {
                Debug.LogError("Sprite not found in Resources: " + path);
            }
        }
    }


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


    string GetMD5(string input)
    {
        MD5 md5 = MD5.Create();
        byte[] inputBytes = Encoding.ASCII.GetBytes(input);
        byte[] hashBytes = md5.ComputeHash(inputBytes);

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < hashBytes.Length; i++)
        {
            sb.Append(hashBytes[i].ToString("X2"));
        }
        return sb.ToString();
    }






    #region ͼƬ��������
    const string IMAGE_CACHE_INFO = "IMAGE_CACHE_INFO";
    /// <summary> �������� </summary>
    const int cacheDay = 30;
    public static bool isUseImageCache(string flieNameMD5)
    {
        if (!PlayerPrefs.HasKey(IMAGE_CACHE_INFO))
        {
            PlayerPrefs.SetString(IMAGE_CACHE_INFO, "{}");
            PlayerPrefs.Save();
        }
        string data = PlayerPrefs.GetString(IMAGE_CACHE_INFO, "{}");
        JObject nodes = JObject.Parse(data);
        Debug.Log($"image cache: {nodes.ToString()}");
        if (nodes.ContainsKey(flieNameMD5))
        {
            long timeMS = nodes[flieNameMD5].ToObject<long>();
            return (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - timeMS) / 1000 / 60 / 60 / 24 <= cacheDay;
        }
        else
        {
            return false;
        }
    }
    public static void ChangeCacheInfo(string flieNameMD5)
    {
        string data = PlayerPrefs.GetString(IMAGE_CACHE_INFO, "{}");
        JObject nodes = JObject.Parse(data);
        nodes[flieNameMD5] = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        PlayerPrefs.SetString(IMAGE_CACHE_INFO, nodes.ToString());
        PlayerPrefs.Save();
    }

    public static void ClearImageCache(string flieNameMD5)
    {
        string data = PlayerPrefs.GetString(IMAGE_CACHE_INFO, "{}");
        JObject nodes = JObject.Parse(data);
        if (nodes.ContainsKey(flieNameMD5))
        {
            nodes.Remove(flieNameMD5);
            PlayerPrefs.SetString(IMAGE_CACHE_INFO, nodes.ToString());
        }
        string localFilePath = Path.Combine(Application.persistentDataPath, "ImageCache", flieNameMD5);
        if (File.Exists(localFilePath))
            File.Delete(localFilePath);
    }
    #endregion


}
