using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

public static class PathHelper 
{

    // ��Զ�̡�
    // ��Դ������/��Ϸƽ̨/total_version.json

    // ��Ϸƽ̨/debug/android/1/version.json
    // ��Ϸƽ̨/debug/android/1/GameRes/
    // ��Ϸƽ̨/debug/android/1/GameDll/
    // ��Ϸƽ̨/release/android/1_1_1/version.json
    // ��Ϸƽ̨/release/android/1_1_1/GameRes/
    // ��Ϸƽ̨/release/android/1_1_1/GameDll/


    // �����ء�
    // Application.persistentDataPath/HotfixTmp/version.json
    // Application.persistentDataPath/HotfixTmp/GameRes/
    // Application.persistentDataPath/HotfixTmp/GameDll/
    // Application.persistentDataPath/Hotfix/version.json
    // Application.persistentDataPath/Hotfix/GameRes/
    // Application.persistentDataPath/Hotfix/GameDll/
    // Application.persistentDataPath/total_version.json

    // �����塿
    // Application.streamingAssetsPath/total_version.json
    // Application.streamingAssetsPath/Hotfix/version.json
    // Application.streamingAssetsPath/Hotfix/GameRes/
    // Application.streamingAssetsPath/Hotfix/GameDll/





    public static string gameResDirPROJPTH => Application.dataPath + "/GameRes";
    public static string gameDllDirPROJPTH => Application.dataPath + "/HotFix";



    /* �ɰ汾���·��
    public const string versionName = "version_0.json";
    public const string totalVersionName = "total_version_0.json";
    public string hotfixDirSAPTH => Application.streamingAssetsPath;
    */

    /* �°汾���·�� */

    public const string versionName = "version.json";
    public const string totalVersionName = "total_version.json";
    public static string hotfixDirSAPTH => Application.streamingAssetsPath + "/Hotfix";


    public static string dllDirSAPTH => Path.Combine(hotfixDirSAPTH, "GameDll");

    public static string abDirSAPTH => Path.Combine(hotfixDirSAPTH, "GameRes");

    public static string totalVersionSAPTH => Path.Combine(hotfixDirSAPTH, totalVersionName);

    public static string versionSAPTH => Path.Combine(hotfixDirSAPTH, versionName);

    public static string mainfestSAPTH => Path.Combine(abDirSAPTH, mainfestBundleName);

    public static string mainfestBundleName = "GameRes";




    /// <summary> �����·����·�� ���Ƕ�̬��ȡ�ģ���</summary>
    public static string hotfixDirWEBURL => GlobalData.autoHotfixUrl;

    public static string hotfixDirLOCPTH => Application.persistentDataPath + "/Hotfix";
    /// <summary> hotfix������Դ��ʱ����Ŀ¼ </summary>
    public static string tmpHotfixDirLOCPTH => Application.persistentDataPath + "/HotfixTmp";


    /// <summary>�ܰ汾�����ļ�·�� </summary>
    public static string totalVersionWEBURL => ApplicationSettings.Instance.platformResourceServerUrl + "/" + totalVersionName;

    public static string totalVersionLOCPTH => Path.Combine(Application.persistentDataPath, totalVersionName);



    /// <summary>�ȸ��¸�·�� </summary>
    public static string versionFileWEBURL
    {
        get
        {
            if (string.IsNullOrEmpty(hotfixDirWEBURL))
                return null;
            return $"{hotfixDirWEBURL}{versionName}";
        }
    }



    public static string versionLOCPTH => Path.Combine(hotfixDirLOCPTH, versionName);
    public static string tmpVersionLOCPTH => Path.Combine(tmpHotfixDirLOCPTH, versionName);


    public static string tmpMainfestLOCPTH => Path.Combine(tmpABDirLOCPTH, mainfestBundleName);


    public static string mainfestLOCPTH => Path.Combine(abDirLOCPTH, mainfestBundleName);
    public static string mainfestWEBURL => Path.Combine(abDirWEBURL, mainfestBundleName);


    public static string tmpABDirLOCPTH => Path.Combine(tmpHotfixDirLOCPTH, "GameRes");
    //public static string abDirSAPTH => Path.Combine(hotfixDirSAPTH, "GameRes");

    public static string abDirLOCPTH => Path.Combine(hotfixDirLOCPTH, "GameRes");
    public static string abDirWEBURL => Path.Combine(hotfixDirWEBURL, "GameRes");

    public static string tmpDllDirLOCPTH => Path.Combine(tmpHotfixDirLOCPTH, "GameDll");
    public static string dllDirLOCPTH => Path.Combine(hotfixDirLOCPTH, "GameDll");


    public static string GetDllWEBURL(string dllName)
    {
        if (string.IsNullOrEmpty(hotfixDirWEBURL))
            return null;
        return $"{hotfixDirWEBURL}/GameDll/{dllName}.dll.bytes";
    }

    public static string GetTempDllLOCPTH(string abName) => Path.Combine(tmpDllDirLOCPTH, $"{abName}.dll.bytes");

    public static string GetDllLOCPTH(string abName) => Path.Combine(dllDirLOCPTH, $"{abName}.dll.bytes");

    public static string GetDllSAPTH(string abName) => Path.Combine(dllDirSAPTH, $"{abName}.dll.bytes");

    public static string GetAssetBundleSAPTH(string abName) => Path.Combine(abDirSAPTH, abName);

    public static string GetAssetBundleLOCPTH(string abName) => Path.Combine(abDirLOCPTH, abName);

    public static string GetTempAssetBundleLOCPTH(string abName) => Path.Combine(tmpABDirLOCPTH, abName);




    #region ��Դ����
    public const string FOLDERGameBackup = "GameBackup";
    public static string backupDirSAPTH => Path.Combine(hotfixDirSAPTH, FOLDERGameBackup);
    public static string backupDirLOCPTH => Path.Combine(hotfixDirLOCPTH, FOLDERGameBackup);
    public static string backupDirWEBURL => Path.Combine(hotfixDirWEBURL, FOLDERGameBackup);

    public static string tmpBackupDirLOCPTH => Path.Combine(tmpHotfixDirLOCPTH, FOLDERGameBackup);
    public static string gameBackupDirPROJPTH => Application.dataPath + $"/{FOLDERGameBackup}";


    public static string GetTempAssetBackupLOCPTH(string nodeName) => Path.Combine(tmpBackupDirLOCPTH, nodeName);


    public static string GetAssetBackupWEBURL(string nodeName = "Cpp Dll/mscatch.dll.bytes") //
    {
        if (string.IsNullOrEmpty(backupDirWEBURL))
            return null;
        return $"{backupDirWEBURL}/{nodeName}";
    }

    public static string GetAssetBackupLOCPTH(string pthOrNodeName = "Assets/GameBackup/Cpp Dll/mscatch.dll.bytes")
    {
        string nodeName = GetAssetBackupNodeName(pthOrNodeName);  // Cpp Dll/mscatch.dll.bytes

        return Path.Combine(backupDirLOCPTH, nodeName);
    }

    public static string GetAssetBackupSAPTH(string pthOrNodeName = "Assets/GameBackup/Cpp Dll/mscatch.dll.bytes")
    {
        string nodeName = GetAssetBackupNodeName(pthOrNodeName);  // Cpp Dll/mscatch.dll.bytes

        return Path.Combine(backupDirSAPTH, nodeName);
    }



    public static string GetAssetBackupNodeName(string pthOrNodeName = "Assets/GameBackup/Cpp Dll/mscatch.dll.bytes") // "Cpp Dll/mscatch.dll.bytes"
    {
        pthOrNodeName = pthOrNodeName.Replace("\\", "/");

        if (pthOrNodeName.Contains(FOLDERGameBackup)) //   if (pthOrNodeName.StartsWith($"Assets/{FOLDERGameBackup}" ))
        {
            return pthOrNodeName.Substring(pthOrNodeName.IndexOf(FOLDERGameBackup) + FOLDERGameBackup.Length + 1);  // "Assets/GameBackup/Cpp Dll/mscatch.dll.bytes"
        }
        else
        {
            return pthOrNodeName; // "Cpp Dll/mscatch.dll.bytes"
        }

    }


    #endregion
}
