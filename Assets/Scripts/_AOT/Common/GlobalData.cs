#define NEW_VER_DLL
using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

public static class GlobalData 
{
    /// <summary> 是否多次检查热更新 </summary>
    public const string HOTFIX_REQUEST_COUNT = "HOTFIX_REQUEST_COUNT";

    /// <summary> 首次非重复安装本包 </summary>
    public static bool isFirstInstall
    {
        get
        {
            if (_isFirstInstall == null)
            {
                if (streamingAssetsVersion == null)
                {
                    Debug.LogError("streamingAssetsVersion is null, please get streamingAssetsVersion first !");
                    return false;
                }
                else
                {
                    // 检查大版本是否发生变化。（删除数据库，恢复为默认配置）
                    string INSTAL_VER = "INSTAL_VER";
                    string lastInstallVerNumber = PlayerPrefs.GetString(INSTAL_VER, "");
                    string curInstallVerNumber = $"{ApplicationSettings.Instance.appKey}-{ApplicationSettings.Instance.appVersion}-{streamingAssetsVersion["hotfix_version"].ToObject<string>()}";
                    bool isFirst = lastInstallVerNumber != curInstallVerNumber;
                    if (isFirst)
                    {
                        PlayerPrefs.SetString(INSTAL_VER, curInstallVerNumber);
                        PlayerPrefs.Save();
                        Debug.LogWarning($"@ is first install: {curInstallVerNumber}");
                    }
                    else
                    {
                        Debug.LogWarning($"@ is not first install: {curInstallVerNumber}");
                    }
                    _isFirstInstall = isFirst;
                }
            }
            return (bool)_isFirstInstall;
        }
    }
    static bool? _isFirstInstall = null;


    /// <summary> 包内版本信息(streamingAssetsVersion) </summary>
    public static JObject streamingAssetsVersion;

    /// <summary> 总版本信息 </summary>
    public static JObject totalVersion;

    /// <summary> 版本信息（新版：dll+ab版本） </summary>
    static JObject _version;

    /**/
    public static JObject version
    {
        get => _version;
        set
        {
            //Debug.LogWarning($"@#@# 333 = {value["hotfix_version"].Value<string>()}");
            _version = value;
        }
    }

    /// <summary> 热更版本 </summary>
    public static string hotfixVersion => version["hotfix_version"].Value<string>();

    /// <summary> 热更id </summary>
    public static string hotfixKey => version["hotfix_key"].Value<string>();

    /// <summary> 安卓包内的热更版本 </summary>
    public static string installHofixVersion => streamingAssetsVersion["hotfix_version"].ToObject<string>();

#if NEW_VER_DLL
    /// <summary> 本包测试标志 </summary>
    const string APK_SELF_DEBUG = "APK_SELF_DEBUG";
    static string _autoHotfixUrl = "";

    /// <summary> 动态获取热更地址 </summary>
    public static string autoHotfixUrl
    {
        get
        {
            string target = _autoHotfixUrl;
            string debug =  PlayerPrefs.GetString(APK_SELF_DEBUG, "");
            if (!string.IsNullOrEmpty(debug))
            {
                target += $"{debug}/";
            }
            return target;
        }
        set
        {
            _autoHotfixUrl = value;
        }
    }
#else
    /// <summary> 动态获取热更地址 </summary>
    public static string autoHotfixUrl ="";
#endif




    /// <summary> 获取建议升级的版本号 </summary>
    public static string versionSuggest
    {

        get
        {
            if(totalVersion == null)
                return null;

            JArray lst = totalVersion["data"] as JArray;
            JObject curTotalVersionItem = null;
            for (int i = 0; i < lst.Count; i++)
            {
                if (lst[i]["app_key"].Value<string>() == ApplicationSettings.Instance.appKey)
                {
                    curTotalVersionItem = lst[i] as JObject;
                    break;
                }
            }

            if (curTotalVersionItem == null)
                return null;

            return curTotalVersionItem["version_suggest"].Value<string>();

        }
    }


    /// <summary>
    /// 保护apk
    /// </summary>
    /// <remarks>
    /// * 热更到更高版本，解除apk保护
    /// </remarks>
    public static bool isProtectApplication
    {
        get
        {
            if (ApplicationSettings.Instance.isUseProtectApplication)
            {
                try
                {
                    Debug.Log($" hotfixVersion: {GlobalData.hotfixVersion}   installHofixVersion: {GlobalData.installHofixVersion}");
                    Version hotfixVersion = new Version(GlobalData.hotfixVersion);
                    Version installHofixVersion = new Version(GlobalData.installHofixVersion);

                    if (installHofixVersion >= hotfixVersion)
                        Debug.LogError("应用受保护!!");
                    
                    return installHofixVersion >= hotfixVersion;// Debug.LogError("应用受保护!!");
                }
                catch (Exception e)
                {
                    return true;
                }

            }
            return false;
        }
    }
}
