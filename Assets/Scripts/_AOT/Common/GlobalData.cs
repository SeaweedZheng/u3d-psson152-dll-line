#define NEW_VER_DLL
using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

public static class GlobalData 
{
    /// <summary> �Ƿ��μ���ȸ��� </summary>
    public const string HOTFIX_REQUEST_COUNT = "HOTFIX_REQUEST_COUNT";

    /// <summary> �״η��ظ���װ���� </summary>
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
                    // ����汾�Ƿ����仯����ɾ�����ݿ⣬�ָ�ΪĬ�����ã�
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


    /// <summary> ���ڰ汾��Ϣ(streamingAssetsVersion) </summary>
    public static JObject streamingAssetsVersion;

    /// <summary> �ܰ汾��Ϣ </summary>
    public static JObject totalVersion;

    /// <summary> �汾��Ϣ���°棺dll+ab�汾�� </summary>
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

    /// <summary> �ȸ��汾 </summary>
    public static string hotfixVersion => version["hotfix_version"].Value<string>();

    /// <summary> �ȸ�id </summary>
    public static string hotfixKey => version["hotfix_key"].Value<string>();

    /// <summary> ��׿���ڵ��ȸ��汾 </summary>
    public static string installHofixVersion => streamingAssetsVersion["hotfix_version"].ToObject<string>();

#if NEW_VER_DLL
    /// <summary> �������Ա�־ </summary>
    const string APK_SELF_DEBUG = "APK_SELF_DEBUG";
    static string _autoHotfixUrl = "";

    /// <summary> ��̬��ȡ�ȸ���ַ </summary>
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
    /// <summary> ��̬��ȡ�ȸ���ַ </summary>
    public static string autoHotfixUrl ="";
#endif




    /// <summary> ��ȡ���������İ汾�� </summary>
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
    /// ����apk
    /// </summary>
    /// <remarks>
    /// * �ȸ������߰汾�����apk����
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
                        Debug.LogError("Ӧ���ܱ���!!");
                    
                    return installHofixVersion >= hotfixVersion;// Debug.LogError("Ӧ���ܱ���!!");
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
