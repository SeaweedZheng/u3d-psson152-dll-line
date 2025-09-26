using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json.Linq;


/// <summary>
/// 图片加载器
/// </summary>
/// <remarks>
/// * 加载缓存图片<br/>
/// * 加载网络图片本保存到本地缓存<br/>
/// * 加载Resources图片<br/>
/// * 加载编辑器图片<br/>
/// * 加载ab包中的图片<br/>
/// * 加载本地图片<br/>
/// * 设置缓存图片保留天数<br/>
/// </remarks>
public class ImageLoader : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }



    /// <summary>
    /// 加载图片
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
                // 加载本地图片
                byte[] fileData = File.ReadAllBytes(localFilePath);
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(fileData);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                onSuccessCallback?.Invoke(sprite);
                Debug.LogWarning($"使用本地缓存图片: {localFilePath}");
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
                            // 下载失败后，默认使用本地
                            byte[] fileData = File.ReadAllBytes(localFilePath);
                            Texture2D texture = new Texture2D(2, 2);
                            texture.LoadImage(fileData);
                            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                            onSuccessCallback?.Invoke(sprite);
                            Debug.LogWarning($"使用本地缓存图片: {localFilePath}");
                        }
                        else
                        {
                            Debug.LogError(www.error);
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"下载图片 ： {path}");

                        Texture2D texture = DownloadHandlerTexture.GetContent(www);
                        // 在这里可以将纹理应用到Sprite上
                        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                        //Debug.LogWarning("Loaded https image: " + sprite.name);

                        if (path.StartsWith("https://") || path.StartsWith("http://"))
                        {
                            // 保存图片到本地
                            byte[] data = texture.EncodeToPNG();
                            // 检查并创建目录
                            string directory = Path.GetDirectoryName(localFilePath);
                            if (!Directory.Exists(directory))
                            {
                                Directory.CreateDirectory(directory);
                            }
                            File.WriteAllBytes(localFilePath, data);
                            ChangeCacheInfo(flieNameMD5);
                            Debug.LogWarning("从网络加载并保存图片到本地");
                        }

                        onSuccessCallback?.Invoke(sprite);
                    }
                }
            }

        }
        else if (path.StartsWith("Assets/") &&  path.Contains("/Resources/"))
        {
            int index = path.IndexOf("/Resources/");
            // 加载Resources文件夹内的资源
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
            // 加载本地缓存的ab包资源
            if (ApplicationSettings.Instance.IsUseHotfix())
            {
                string bundleName = GetBundleName(path);
                string pathLoc = PathHelper.abDirLOCPTH + "/" + bundleName;
                AssetBundle targetBundle = AssetBundle.LoadFromFile(pathLoc); //同步

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
                // 加载编辑器资源
                Sprite sprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(path);
                onSuccessCallback?.Invoke(sprite);
#endif
            }

        }
        else
        {
           // 本地图片
           if (File.Exists(path))
            {
                Debug.LogWarning($"加载本地图片： {path}");
                // 加载本地图片
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
            result = result.Substring(prefixToRemove.Length);  //去掉 "assets/gameres/"
        }
        string[] str = result.Split('/');
        string fileNameSuffix = str[str.Length - 1];
        if (fileNameSuffix.Contains("."))
        {
            string[] str01 = fileNameSuffix.Split('.');
            int leg = str01[str01.Length - 1].Length + 1;  //".png 或 .prefab"
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






    #region 图片缓存天数
    const string IMAGE_CACHE_INFO = "IMAGE_CACHE_INFO";
    /// <summary> 保留天数 </summary>
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
