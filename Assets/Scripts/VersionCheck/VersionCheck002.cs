#define NEW_VER_DLL
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;




public class VersionCheck002 : MonoBehaviour
{

    static VersionCheck002 instance;
    public static VersionCheck002 Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(VersionCheck002)) as VersionCheck002;
            }
            return instance;
        }
    }





    const string HOTFIX_STATE = "HOTFIX_STATE";
    const string HotfixStateCompleted = "Completed";
    const string HotfixStateCopying = "Copying";
    const string HotfixDownloading = "HotfixDownloading";
    const string HotfixDownloadFail = "HotfixDownloadFail";
    /// <summary> �ܰ汾����-�ӽڵ����� </summary>

    //byte[] versionDataRemoteByte;
    JObject versionFileRemoteNode;


    //bool hasInternet = false;



    /// <summary> �ȸ��ظ�������� </summary>
    int hotfixRequestCount
    {
        get
        {
            // �ܵ�����ʱĬ�Ͽ�������
            if (GlobalData.isFirstInstall && ApplicationSettings.Instance.isUseProtectApplication)
            {
                // �ɰ��Ѿ������ֶ�  HOTFIX_REQUEST_COUNT
                // ��һ��װ��ʱ��Ҫǿ�Ʊ��棬��������
                PlayerPrefs.SetInt(GlobalData.HOTFIX_REQUEST_COUNT, 10);  
                PlayerPrefs.Save();
                return 10;
            }

            int defaultCount = ApplicationSettings.Instance.isUseProtectApplication ? 10 : 1;
            if (!PlayerPrefs.HasKey(GlobalData.HOTFIX_REQUEST_COUNT))
                PlayerPrefs.SetInt(GlobalData.HOTFIX_REQUEST_COUNT, defaultCount);
            int count = PlayerPrefs.GetInt(GlobalData.HOTFIX_REQUEST_COUNT, defaultCount);
            return count < 1 ? 1 : count;
        }
    }



    /// <summary> ��ȡԶ����Դ���ݵ�hash /// </summary>
    string GetWebAssetBackupHash(string nodeName) => versionFileRemoteNode["asset_backup"][nodeName]["hash"].ToObject<string>();

    /// <summary> ��ȡ����dll�ļ���hash  </summary>
    string GetLocalAssetBackupHash(string nodeName) => GlobalData.version["asset_backup"][nodeName]["hash"].ToObject<string>();

    /// <summary>
    /// ��ȡԶ��dll�ļ���hash
    /// </summary>
    /// <param name="dllName"></param>
    /// <returns></returns>
    string GetWebHotfixDllHash(string dllName) => versionFileRemoteNode["hotfix_dll"][dllName]["hash"].ToObject<string>();



    /// <summary>
    /// ��ȡ����dll�ļ���hash
    /// </summary>
    /// <param name="dllName"></param>
    /// <returns></returns>
    string GetLocalHotfixDllHash(string dllName) => GlobalData.version["hotfix_dll"][dllName]["hash"].ToObject<string>();



    void GetLocalVersionInfo()
    {
        GlobalData.version = JObject.Parse(File.ReadAllText(PathHelper.versionLOCPTH));
        GlobalData.totalVersion = null;
        GlobalData.autoHotfixUrl = "";

        if (File.Exists(PathHelper.totalVersionLOCPTH))
        {
            GlobalData.totalVersion = JObject.Parse(File.ReadAllText(PathHelper.totalVersionLOCPTH));

            JObject targetTotalVersionItem = null;
            JArray lst = GlobalData.totalVersion["data"] as JArray;
            for (int i = 0; i < lst.Count; i++)
            {
                string appKey = lst[i]["app_key"].ToObject<string>();
                if (appKey == ApplicationSettings.Instance.appKey)
                {
                    targetTotalVersionItem = lst[i] as JObject;
                    break;
                }
            }
            if (targetTotalVersionItem != null)
            {
                GlobalData.autoHotfixUrl = FileUtils.GetDirWebUrl(PathHelper.totalVersionWEBURL, targetTotalVersionItem["hotfix_url"].ToObject<string>());
            }
            else
            {
                Debug.LogWarning($"cant not find app key:{ApplicationSettings.Instance.appKey} in local total version");
            }
        }
    }


    /// <summary>
    /// ��鲢�ȸ�����Դ
    /// </summary>
    /// <param name="callback"></param>
    /// <returns></returns>
    public IEnumerator DoHotfix(UnityAction callback)
    {
        ClearRemoteInfo();

        // ��ȡ���ڰ汾
        yield return GetStreamingAssetsVersion();

        // �༭����ʹ���ȸ�
        if (!ApplicationSettings.Instance.IsUseHotfix())
        {
            yield return FileUtils.ReadStreamingAsset<string>(
                PathHelper.versionSAPTH,
                (obj) =>
                {
                    GlobalData.version = JObject.Parse((string)obj);
                }, null);
            Debug.Log($"is use streaming assets version");
            // �ص�
            callback?.Invoke();
            yield break;
        }

        // �ж��Ƿ����״ΰ�װ
        bool isFirstInstall = GlobalData.isFirstInstall;

        // �״�װ�������ļ�
        if (isFirstInstall || !File.Exists(PathHelper.versionLOCPTH))
        {
            yield return CopyStreamingAssetsToPersistentDataPath();
        }

        LoadingPage.Instance.AddProgressCount(LoadingProgress.CHECK_COPY_TEMP_HOTFIX_FILE, 1);
        LoadingPage.Instance.Next(LoadingProgress.CHECK_COPY_TEMP_HOTFIX_FILE, $"check cache : temp hotfix file");

        // ���Ҫ�������ļ�
        yield return CopyTempWebHotfixFileToTargetDir();

        LoadingPage.Instance.RemoveProgress(LoadingProgress.CHECK_COPY_TEMP_HOTFIX_FILE);

        // ��ȡ���������ļ�
        GetLocalVersionInfo();


        /// ����ȡ�汾��Ϣ��������󣬱����̨�ϵ��wifiҪ��ʱ�����ӡ�
        bool isGetWebVersionSuccess = false;
        int count = hotfixRequestCount;
        int i = 0;
        while (true)
        {
            yield return GetWebTotalVersionAndVersion(() => isGetWebVersionSuccess = true, () => isGetWebVersionSuccess = false);

            if (isGetWebVersionSuccess || --count < 1)
                break;
            else {
                Debug.LogWarning($"getting web version files fails, count: {++i}  time: {Time.unscaledTime}");
                yield return new WaitForSecondsRealtime(2.5f);
            }
        }

        Debug.Log($"is get web version file = {isGetWebVersionSuccess}");


        if (!isGetWebVersionSuccess)
        {
            // �ص�
            callback?.Invoke();
            yield break;
        }
        else
        {

            string localVersionVER = GlobalData.version["hotfix_version"].Value<string>();
            string webVersionVER = versionFileRemoteNode["hotfix_version"].Value<string>();
            Debug.Log($"local version file ver:{localVersionVER}  --  web version file ver:{webVersionVER}");
            int[] localVersions = GetVersions(localVersionVER);
            int[] serverVersions = GetVersions(webVersionVER);

            if (localVersions[0] == serverVersions[0])
            {

                if (localVersions[1] != serverVersions[1] || localVersions[2] != serverVersions[2])
                {
                    PlayerPrefs.SetString(HOTFIX_STATE, HotfixDownloading);

                    if (PlayerPrefs.GetString(HOTFIX_STATE) != HotfixDownloadFail)
                        yield return StartDownLoadAllHotfixDll(); //���ذ�Dll

                    if (PlayerPrefs.GetString(HOTFIX_STATE) != HotfixDownloadFail)
                        yield return HotfixABResources(); //����AB �� mainfest�ļ�

                    if (PlayerPrefs.GetString(HOTFIX_STATE) != HotfixDownloadFail)
                        yield return StartDownLoadAllAssetBackup(); //���� ������Դ
                    

                    if (PlayerPrefs.GetString(HOTFIX_STATE) != HotfixDownloadFail)
                    {
                        // д��汾�ļ�
                        //FileUtils.WriteAllBytes(PathHelper.tmpVersionLOCPTH, versionDataRemoteByte);
                        FileUtils.WriteAllText(PathHelper.tmpVersionLOCPTH, versionFileRemoteNode.ToString());
                        //Debug.Log($"@@ д��汾���ݣ�{versionFileRemoteNode.ToString()}");


                        LoadingPage.Instance.AddProgressCount(LoadingProgress.COPY_TEMP_HOTFIX_FILE, 1);
                        LoadingPage.Instance.Next(LoadingProgress.COPY_TEMP_HOTFIX_FILE, $"copy cache : temp hotfix file");

                        // ���Ҫ�������ļ�
                        PlayerPrefs.SetString(HOTFIX_STATE, HotfixStateCopying);
                        yield return CopyTempWebHotfixFileToTargetDir();


                        LoadingPage.Instance.RemoveProgress(LoadingProgress.COPY_TEMP_HOTFIX_FILE);


                        // ���»�ȡ���������ļ�
                        GlobalData.version = JObject.Parse(File.ReadAllText(PathHelper.versionLOCPTH));

                        // ɾ�������ļ�
                        yield return DeleteUnuseABAndManifest();


                        // ɾ������dll
                        yield return DeleteUnuseHotfixDll();

                    }

                }
                else
                {
                    Debug.Log("no need for hotfix");
                }

                // �ص�
                callback?.Invoke();
                yield break;
            }
            else // ��·���汾�Ŵ��ڵ�ǰ���汾��
            {
                // �ص� - ���� - ������Ҫ���ظ��µ�app��װ��
                Debug.LogError("The local master version number and the remote master version number are not equal");
                callback?.Invoke();
                yield break;
            }
        }

    }



    #region ɾ���ò�����ab���� dll����
    /// <summary>
    /// ��������Ԥ���壬����Ԥ������
    /// </summary>
    /// <param name="rootFolderPath"></param>
    List<string> GetTargetFilePath(string rootFolderPath, string extension = ".png")
    {
        List<string> paths = new List<string>();
        foreach (string path in Directory.GetFiles(rootFolderPath))
        {
            //��ȡ�����ļ����а�����׺Ϊ .prefab ��·��
            if (extension == ".*") // path System.IO.Path.GetExtension(path) != ".meta"
            {
                if (!path.EndsWith(".meta"))
                    paths.Add(path);
            }
            else if (path.EndsWith(extension))
            {
                paths.Add(path);
            }
        }
        if (Directory.GetDirectories(rootFolderPath).Length > 0)  //���������ļ���
        {
            foreach (string path in Directory.GetDirectories(rootFolderPath))
            {
                paths.AddRange(GetTargetFilePath(path, extension));
            }
        }
        return paths;
    }

    List<string> GetUnuseAB()
    {

        string manifestAssetName = "AssetBundleManifest"; // ���� manifest �ļ�����Դ����
        AssetBundle manifestBundle = AssetBundle.LoadFromFile(PathHelper.mainfestLOCPTH);
        AssetBundleManifest manifest = manifestBundle.LoadAsset<AssetBundleManifest>(manifestAssetName);
        manifestBundle.Unload(false);

        string[] allAssetBundleNames = manifest.GetAllAssetBundles();

        List<string> bundlePathLst = new List<string>();
        foreach (string assetBundleName in allAssetBundleNames)
        {
            string pth1 = Path.Combine(PathHelper.abDirLOCPTH, assetBundleName);
            bundlePathLst.Add(pth1.Replace("\\", "/"));
        }

        // ����ab�����ǲ���".unity3d"��β�ġ���"GameRes" �� ��GameRes.manifest����
        List<string> targetPathLst = new List<string>();  //��ȡ��ͨ��·�� xxx.unity3d  ��  xxx.unity3d.manifest
        targetPathLst.AddRange(GetTargetFilePath(PathHelper.abDirLOCPTH, ".unity3d"));
        //targetPathLst.AddRange(GetTargetFilePath(abDirLOCPTH, ".unity3d.manifest"));

        for (int i = 0; i < targetPathLst.Count; i++)
        {
            targetPathLst[i] = targetPathLst[i].Replace("\\", "/");
        }

        List<string> unusePths = new List<string>();
        foreach (string pth002 in targetPathLst)
        {
            /* string tempPth = pth002.EndsWith(".unity3d.manifest") ? pth002.Replace(".unity3d.manifest", ".unity3d") : pth002;
             if (!bundlePathLst.Contains(tempPth))
             {
                 unusePths.Add(pth002);
             }*/
            if (!bundlePathLst.Contains(pth002))
            {
                unusePths.Add(pth002);
            }
        }

        return unusePths;
    }

    /// <summary>
    /// ɾ�����õ�ab����manifest�ļ�
    /// </summary>
    IEnumerator DeleteUnuseABAndManifest()
    {
        List<string> unusePths = GetUnuseAB();

        LoadingPage.Instance.AddProgressCount(LoadingProgress.DELETE_UNUSE_ASSET_BUNDLE, unusePths.Count);

        int i = 0;
        foreach (string pth in unusePths)
        {
            i++;
            if (File.Exists(pth))
            {
                LoadingPage.Instance.Next(LoadingProgress.DELETE_UNUSE_ASSET_BUNDLE, $"delete unuse ab {i}/{unusePths.Count}: {pth}  ");

                Debug.Log($"delete unuse ab {i}/{unusePths.Count}: {pth}");
                File.Delete(pth);
                yield return null;
            }
        }

        LoadingPage.Instance.Next(LoadingProgress.DELETE_UNUSE_ASSET_BUNDLE, $"delete unuse ab folder");

        yield return DeleteUnuseABFolderAndMeta();

        LoadingPage.Instance.RemoveProgress(LoadingProgress.DELETE_UNUSE_ASSET_BUNDLE);
    }


    /// <summary>
    /// ɾ�����õ��ļ���
    /// </summary>
    /// <returns></returns>
    IEnumerator DeleteUnuseABFolderAndMeta()
    {

        List<string> allSubDirectories = GetAllSubFolders(PathHelper.abDirLOCPTH);

        // ���ļ���·�����ַ����ȴӳ����̽�������Խ����·������Խǰ�棩
        allSubDirectories.Sort((a, b) => b.Length - a.Length);

        /*
        // ����������ļ��е�����
        foreach (string directory in allSubDirectories)
            Debug.Log(directory);
        */

        // ����������Ŀ¼·��
        foreach (string directory in allSubDirectories)
        {
            if (ShouldDeleteDirectory(directory))
            {
                DeleteDirectoryAndMeta(directory);
                yield return null;
            }
        }
    }

    static List<string> GetAllSubFolders(string directoryPath)
    {
        List<string> allFolders = new List<string>();
        try
        {
            // ��ȡ��ǰĿ¼�µ��������ļ���
            string[] subDirectories = Directory.GetDirectories(directoryPath);
            foreach (string subDirectory in subDirectories)
            {
                // ����ǰ���ļ�����ӵ�����б���
                allFolders.Add(subDirectory);
                // �ݹ���ø÷�������ȡ��ǰ���ļ����µ��������ļ���
                allFolders.AddRange(GetAllSubFolders(subDirectory));
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"����Ŀ¼ {directoryPath} ʱ����: {e.Message}");
        }
        return allFolders;
    }


    /// <summary>
    /// �����·�����ڣ���û���Ӽ��ļ��У����Ӽ��ļ����� .meta�ļ��� ������Ŀ¼����������ڶ�ɾ��
    /// </summary>
    /// <param name="directoryPath"></param>
    static bool ShouldDeleteDirectory(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            return false;
        }

        string[] subDirectories = Directory.GetDirectories(directoryPath);
        if (subDirectories.Length > 0)
        {
            return false;
        }

        string[] files = Directory.GetFiles(directoryPath);
        foreach (string file in files)
        {
            if (Path.GetExtension(file).ToLower() != ".meta")
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// ɾ��Ŀ¼����Ŀ¼��.meta�ļ�����Ŀ¼�����������
    /// </summary>
    /// <param name="directoryPath"></param>
    static void DeleteDirectoryAndMeta(string directoryPath)
    {
        try
        {
            // ɾ��Ŀ¼��������
            Directory.Delete(directoryPath, true);

            // ɾ����Ӧ�� .meta �ļ�
            string metaFilePath = directoryPath + ".meta";
            if (File.Exists(metaFilePath))
            {
                File.Delete(metaFilePath);
            }

            Debug.Log($"delete unuse dir: {directoryPath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"delete unuse dir: {directoryPath} error: {e.Message}");
        }
    }


    private IEnumerator DeleteUnuseHotfixDll()
    {

        JObject hotfixDlls = GlobalData.version["hotfix_dll"] as JObject;

        List<string> targetPathLst = new List<string>();  //��ȡ��ͨ��·�� xxx.unity3d  ��  xxx.unity3d.manifest
        targetPathLst.AddRange(GetTargetFilePath(PathHelper.hotfixDirLOCPTH, ".dll.bytes"));

        int idx = targetPathLst.Count - 1;
        while (idx >= 0)
        {
            string[] pths = targetPathLst[idx].Replace("\\", "/").Split('/');
            string name = pths[pths.Length - 1].Replace(".dll.bytes", "");

            if (hotfixDlls.ContainsKey(name))
            {
                targetPathLst.RemoveAt(idx);
            }
            idx--;
        }

        LoadingPage.Instance.AddProgressCount(LoadingProgress.DELETE_UNUSE_HOTFIX_DLL, targetPathLst.Count);
        int i = 0;
        foreach (string s in targetPathLst)
        {
            i++;
            if (File.Exists(s))
            {
                LoadingPage.Instance.Next(LoadingProgress.DELETE_UNUSE_HOTFIX_DLL, $"delete unuse dll {i}/{targetPathLst.Count}: {s}  ");

                Debug.Log($"delete unuse dll {i}/{targetPathLst.Count}: {s}");

                File.Delete(s);
                yield return null;
            }
        }

        LoadingPage.Instance.RemoveProgress(LoadingProgress.DELETE_UNUSE_HOTFIX_DLL);

    }



    #endregion





    #region ���������ļ�



    private IEnumerator GetStreamingAssetsVersion()
    {
        // ��������dll
        GlobalData.streamingAssetsVersion = null;
        yield return FileUtils.ReadStreamingAsset<string>(PathHelper.versionSAPTH, (obj) =>
        {
            GlobalData.streamingAssetsVersion = JObject.Parse((string)obj);
        }, (err) =>
        {
            throw new System.Exception(err);
        });
    }


    private IEnumerator CopyStreamingAssetsToPersistentDataPath()
    {
        // ɾ��Ŀ¼
        if (Directory.Exists(PathHelper.tmpHotfixDirLOCPTH))
        {
            yield return FileUtils.DeleteDirectoryAsync(PathHelper.tmpHotfixDirLOCPTH);
        }
        // ɾ��Ŀ¼
        if (Directory.Exists(PathHelper.hotfixDirLOCPTH))
        {
            yield return FileUtils.DeleteDirectoryAsync(PathHelper.hotfixDirLOCPTH);
        }


        // �ȼ���manifest�ļ�����ȡ����ab��Դ
        using (var manifestWWW = new UnityWebRequest(PathHelper.mainfestSAPTH))
        {
            manifestWWW.downloadHandler = new DownloadHandlerAssetBundle(PathHelper.mainfestSAPTH, 0);
            yield return manifestWWW.SendWebRequest();

            if (manifestWWW.isNetworkError || manifestWWW.isHttpError)
            {
                throw new Exception("StreamingAssets��û��manifest�ļ���������version�ļ�");
            }
            else
            {
                // ����ab��Դ���־�Ŀ¼
                var manifestAB = DownloadHandlerAssetBundle.GetContent(manifestWWW);
                var abManifest = manifestAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                manifestAB.Unload(false);
                string[] abList = abManifest.GetAllAssetBundles();
                int totalCount = abList.Length;// һ���ǰ汾�����ļ���һ����manifest�ļ�
                int completedCount = 0;

                LoadingPage.Instance.AddProgressCount(LoadingProgress.COPY_SA_ASSET_BUNDLE, totalCount);

                foreach (var abName in abList)
                {
                    string srcPath = PathHelper.GetAssetBundleSAPTH(abName);
                    string tarPath = PathHelper.GetAssetBundleLOCPTH(abName);
                    Debug.Log($"{srcPath} - {tarPath}");
                    completedCount++;

                    LoadingPage.Instance.Next(LoadingProgress.COPY_SA_ASSET_BUNDLE,
                        $"copy assetbundle to cache: {abName} {completedCount}/{totalCount}");

                    Debug.Log(string.Format("copy asset bundle {0}/{1}, bundle:{2}", completedCount, totalCount, abName));

                    yield return FileUtils.CopyStreamingAssetToLocal(srcPath, tarPath);

                }
                LoadingPage.Instance.Next(LoadingProgress.COPY_SA_ASSET_BUNDLE,
                    $"copy manifest to cache: {PathHelper.mainfestBundleName}");

                Debug.Log("copy manifest");
                yield return FileUtils.CopyStreamingAssetToLocal(PathHelper.mainfestSAPTH, PathHelper.mainfestLOCPTH);

            }
        }

        LoadingPage.Instance.RemoveProgress(LoadingProgress.COPY_SA_ASSET_BUNDLE);


        /* ��������dll
        JObject versionSAObj = null;
        yield return FileUtils.ReadStreamingAsset<string>(PathHelper.versionSAPTH, (obj) =>
        {
            versionSAObj = JObject.Parse((string)obj); 

        }, (err) =>
        {
            throw new System.Exception(err);
        });*/

        JObject versionSAObj = GlobalData.streamingAssetsVersion;

        if (versionSAObj != null)
        {

            JObject hotfixDll = versionSAObj["hotfix_dll"] as JObject;

            LoadingPage.Instance.AddProgressCount(LoadingProgress.COPY_SA_HOTFIX_DLL, hotfixDll.Count);

            int i = 0;
            foreach (KeyValuePair<string, JToken> kv in hotfixDll)
            {
                string saPth = PathHelper.GetDllSAPTH(kv.Key);
                string tarPath = PathHelper.GetDllLOCPTH(kv.Key);

                i++;
                LoadingPage.Instance.Next(LoadingProgress.COPY_SA_HOTFIX_DLL,
                    $"copy dll to cache: {kv.Key} {i}/{hotfixDll.Count}");

                Debug.Log(string.Format("copy dll {0}/{1}, dll:{2}", i, hotfixDll.Count, kv.Key));

                yield return FileUtils.CopyStreamingAssetToLocal(saPth, tarPath);
            }

            //LoadingPage.Instance.Next(LoadingProgress.COPY_SA_HOTFIX_DLL,  $"copy version file to cache: {PathHelper.versionName}");
            //Debug.Log("copy version");
            //yield return FileUtils.CopyStreamingAssetToLocal(PathHelper.versionSAPTH, PathHelper.versionLOCPTH);
        }

        LoadingPage.Instance.RemoveProgress(LoadingProgress.COPY_SA_HOTFIX_DLL);


        if (versionSAObj != null)
        {
            JObject assetBackup = versionSAObj["asset_backup"] as JObject;

            LoadingPage.Instance.AddProgressCount(LoadingProgress.COPY_SA_ASSET_BACKUP, assetBackup.Count);

            int i = 0;
            foreach (KeyValuePair<string, JToken> kv in assetBackup)
            {
                string saPth = PathHelper.GetAssetBackupSAPTH(kv.Key); //PathHelper.GetDllSAPTH(kv.Key);
                string tarPath = PathHelper.GetAssetBackupLOCPTH(kv.Key); // PathHelper.GetDllLOCPTH(kv.Key);

                i++;
                LoadingPage.Instance.Next(LoadingProgress.COPY_SA_ASSET_BACKUP,
                    $"copy asset backup to cache: {kv.Key} {i}/{assetBackup.Count}");

                Debug.Log(string.Format("copy asset backup {0}/{1}, dll:{2}", i, assetBackup.Count, kv.Key));

                yield return FileUtils.CopyStreamingAssetToLocal(saPth, tarPath);
            }
        }
        LoadingPage.Instance.RemoveProgress(LoadingProgress.COPY_SA_ASSET_BACKUP);

        if (versionSAObj != null)
        {
            //LoadingPage.Instance.Next(LoadingProgress.COPY_SA_ASSET_BACKUP, $"copy version file to cache: {PathHelper.versionName}");
            LoadingPage.Instance.RefreshProgressUIMsg($"copy version file to cache: {PathHelper.versionName}");
            Debug.Log("copy version");
            yield return FileUtils.CopyStreamingAssetToLocal(PathHelper.versionSAPTH, PathHelper.versionLOCPTH);
        }
        else
        {
            Debug.LogError("the version in StreamingAssets can not find!! ");
        }
    }

    #endregion



    #region ������Դ����

    /// <summary>
    /// �鿴�Ƿ������ȸ���������Դ����
    /// </summary>
    /// <returns></returns>
    private IEnumerator CopyTempWebHotfixFileToTargetDir()
    {

        // �Ƿ����ȸ����ļ���Ҫ����
        if (PlayerPrefs.HasKey(HOTFIX_STATE) && PlayerPrefs.GetString(HOTFIX_STATE) == HotfixStateCopying)
        {
            //��ʼ����
            yield return FileUtils.CopyDirectoryAsync(PathHelper.tmpHotfixDirLOCPTH, PathHelper.hotfixDirLOCPTH);

            PlayerPrefs.SetString(HOTFIX_STATE, HotfixStateCompleted);
        }

        // ɾ������
        if (Directory.Exists(PathHelper.tmpHotfixDirLOCPTH))
        {
            yield return FileUtils.DeleteDirectoryAsync(PathHelper.tmpHotfixDirLOCPTH);
        }

    }

    #endregion



    #region ���Զ�˰汾

    /// <summary>
    /// ɾ��Զ�̵Ĳ���
    /// </summary>
    void ClearRemoteInfo()
    {
        versionFileRemoteNode = null;
        //versionDataRemoteByte = null;
    }


    private IEnumerator GetWebTotalVersionAndVersion(UnityAction onSuccessCallback, UnityAction onErrorCallback)
    {

        string tvUrl = PathHelper.totalVersionWEBURL + $"?t={DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";
        Debug.Log($"download total version�� {tvUrl}");

        LoadingPage.Instance.AddProgressCount(LoadingProgress.CHECK_WEB_VERSION, 2);
        LoadingPage.Instance.Next(LoadingProgress.CHECK_WEB_VERSION, $"get web total version");

        using (UnityWebRequest reqTotalVersion = UnityWebRequest.Get(tvUrl))
        {
            yield return reqTotalVersion.SendWebRequest();

            if (reqTotalVersion.result == UnityWebRequest.Result.Success)
            {
                string tvStr = reqTotalVersion.downloadHandler.text;

                GlobalData.totalVersion = JObject.Parse(tvStr);

                // �����汾�ļ�
                FileUtils.WriteAllBytes(PathHelper.totalVersionLOCPTH, reqTotalVersion.downloadHandler.data);

                JObject targetTotalVersionItem = null;
                if (GlobalData.totalVersion != null)
                {
                    JArray lst = GlobalData.totalVersion["data"] as JArray;
                    for (int i = 0; i < lst.Count; i++)
                    {
                        string appKey = lst[i]["app_key"].ToObject<string>();
                        if (appKey == ApplicationSettings.Instance.appKey)
                        {
                            targetTotalVersionItem = lst[i] as JObject;
                            break;
                        }
                    }
                }

                if (targetTotalVersionItem != null)
                {
                    GlobalData.autoHotfixUrl = FileUtils.GetDirWebUrl(PathHelper.totalVersionWEBURL, targetTotalVersionItem["hotfix_url"].ToObject<string>());

                    LoadingPage.Instance.Next(LoadingProgress.CHECK_WEB_VERSION, $"get web version");

                    yield return GetWebVersion(onSuccessCallback, onErrorCallback);
                }
                else
                {
                    Debug.LogError($"web total version file cant not find node at app_key: {ApplicationSettings.Instance.appKey}");
                    onErrorCallback?.Invoke();
                }
            }
            else//�������汾�ļ�����ʧ��
            {
                Debug.LogError(reqTotalVersion.error);
                Debug.Log("û������ֱ�Ӹ����ļ�");
                onErrorCallback?.Invoke();
            }
        }
        LoadingPage.Instance.RemoveProgress(LoadingProgress.CHECK_WEB_VERSION);
    }


    private IEnumerator GetWebVersion(UnityAction onSuccessCallback, UnityAction onErrorCallback)
    {

        string vUrl = PathHelper.versionFileWEBURL + $"?t={DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";
        Debug.Log($"remote version url�� {vUrl}");
        using (UnityWebRequest reqVersion = UnityWebRequest.Get(vUrl))
        {
            yield return reqVersion.SendWebRequest();

            if (reqVersion.result == UnityWebRequest.Result.Success)
            {
                string jsonStr = reqVersion.downloadHandler.text;

                //versionDataRemoteByte = reqVersion.downloadHandler.data;

                versionFileRemoteNode = JObject.Parse(jsonStr);

                Debug.Log($"web version file ; ver: {versionFileRemoteNode["hotfix_version"].ToObject<string>()}");

                //Debug.Log($"version data :  {jsonStr}");
                onSuccessCallback?.Invoke();
            }
            else//�������汾�ļ�����ʧ��
            {
                Debug.LogError(reqVersion.error);
                Debug.Log("û������ֱ�Ӹ����ļ�");
                onErrorCallback?.Invoke();
            }

        }
    }




    #endregion





    #region Զ�����ر�����Դ
    private IEnumerator DownLoadRemoteAssetBackupOnce(string nodeName)
    {
        if (PlayerPrefs.GetString(HOTFIX_STATE) == HotfixDownloadFail)
            yield break;

        string hashRemote = GetWebAssetBackupHash(nodeName);
        string assetBackupDownloadUrl = PathHelper.GetAssetBackupWEBURL(nodeName);

        // cdn ����
        UnityWebRequest req = UnityWebRequest.Get(assetBackupDownloadUrl); //ApplicationSettings.Instance.hotfixUrl

        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            string md5 = FileUtils.CalculateMD5(req.downloadHandler.data);

            if (md5 == hashRemote)
            {
                string writePath = PathHelper.GetTempAssetBackupLOCPTH(nodeName);
                FileUtils.WriteAllBytes(writePath, req.downloadHandler.data);

                //����������ء� s_assetDatas[dallame] = req.downloadHandler.data;
                yield break;
            }
        }

        // ��cdn����
        UnityWebRequest req01 = UnityWebRequest.Get(assetBackupDownloadUrl + $"?t={DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}");
        yield return req01.SendWebRequest();
        if (req01.result == UnityWebRequest.Result.Success)
        {
            string md5 = FileUtils.CalculateMD5(req01.downloadHandler.data);

            if (md5 == hashRemote)
            {
                string writePath = PathHelper.GetTempAssetBackupLOCPTH(nodeName);
                FileUtils.WriteAllBytes(writePath, req01.downloadHandler.data);

                //����������ء� s_assetDatas[dallame] = req01.downloadHandler.data;

                yield break;
            }
        }

        PlayerPrefs.SetString(HOTFIX_STATE, HotfixDownloadFail);
        throw new Exception($"down dall {nodeName} is fail");
    }




    private IEnumerator StartDownLoadAllAssetBackup()
    {
        JObject localBackupNode = GlobalData.version["asset_backup"] as JObject;
        JObject serverBackupNode = versionFileRemoteNode["asset_backup"] as JObject;


        LoadingPage.Instance.AddProgressCount(LoadingProgress.DOWNLOAD_ASSET_BACKUP, serverBackupNode.Count);

        int i = 0;
        //����Դ����������DLLд�뵽����
        foreach (KeyValuePair<string, JToken> item in serverBackupNode)
        {

            string name = item.Key;
            if (!localBackupNode.ContainsKey(name)
                || (localBackupNode[name]["hash"].ToObject<string>() != serverBackupNode[name]["hash"].ToObject<string>()))
            {

                LoadingPage.Instance.Next(LoadingProgress.DOWNLOAD_ASSET_BACKUP, $"download asset backup: {name}  {i + 1}/{serverBackupNode.Count}");

                Debug.Log($"download asset backup: {name}  {i + 1}/{serverBackupNode.Count}");

                yield return DownLoadRemoteAssetBackupOnce(name);
            }
            i++;
        }

        LoadingPage.Instance.RemoveProgress(LoadingProgress.DOWNLOAD_ASSET_BACKUP);

    }





    #endregion















    #region Զ����Դ����
    private IEnumerator DownLoadRemoteHotfixDllOnce(string dallame)
    {
        if (PlayerPrefs.GetString(HOTFIX_STATE) == HotfixDownloadFail)
            yield break;

        string hashRemote = GetWebHotfixDllHash(dallame);
        string hotfixDllDownloadUrl = PathHelper.GetDllWEBURL(dallame);

        // cdn ����
        UnityWebRequest req = UnityWebRequest.Get(hotfixDllDownloadUrl); //ApplicationSettings.Instance.hotfixUrl

        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            string md5 = FileUtils.CalculateMD5(req.downloadHandler.data);

            if (md5 == hashRemote)
            {
                string writePath = PathHelper.GetTempDllLOCPTH(dallame);
                FileUtils.WriteAllBytes(writePath, req.downloadHandler.data);

                //����������ء� s_assetDatas[dallame] = req.downloadHandler.data;
                yield break;
            }
        }

        // ��cdn����
        UnityWebRequest req01 = UnityWebRequest.Get(hotfixDllDownloadUrl + $"?t={DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}");
        yield return req01.SendWebRequest();
        if (req01.result == UnityWebRequest.Result.Success)
        {
            string md5 = FileUtils.CalculateMD5(req01.downloadHandler.data);

            if (md5 == hashRemote)
            {
                string writePath = PathHelper.GetTempDllLOCPTH(dallame);
                FileUtils.WriteAllBytes(writePath, req01.downloadHandler.data);

                //����������ء� s_assetDatas[dallame] = req01.downloadHandler.data;

                yield break;
            }
        }

        PlayerPrefs.SetString(HOTFIX_STATE, HotfixDownloadFail);
        throw new Exception($"down dall {dallame} is fail");
    }



    private IEnumerator StartDownLoadAllHotfixDll()
    {
        JObject localHotfixNode = GlobalData.version["hotfix_dll"] as JObject;
        JObject serverHotfixNode = versionFileRemoteNode["hotfix_dll"] as JObject;

#if NEW_VER_DLL

        LoadingPage.Instance.AddProgressCount(LoadingProgress.DOWNLOAD_HOTFIX_DLL, serverHotfixNode.Count);

        int i = 0;
        //����Դ����������DLLд�뵽����
        foreach (KeyValuePair<string, JToken> item in serverHotfixNode)
        {

            string name = item.Key;
            if (!localHotfixNode.ContainsKey(name)
                || (localHotfixNode[name]["hash"].ToObject<string>() != serverHotfixNode[name]["hash"].ToObject<string>()))
            {

                LoadingPage.Instance.Next(LoadingProgress.DOWNLOAD_HOTFIX_DLL, $"download hotfix dll: {name}  {i + 1}/{serverHotfixNode.Count}");

                Debug.Log($"download hotfix dll: {name}  {i + 1}/{serverHotfixNode.Count}");

                yield return DownLoadRemoteHotfixDllOnce(name);
            }
            i++;
        }

        LoadingPage.Instance.RemoveProgress(LoadingProgress.DOWNLOAD_HOTFIX_DLL);
#else
        List<string> dlls = DllHelper.Instance.DllNameList;
        //List<string> dlls = DllHelper.Instance.GetDllNameList(versionFileRemoteNode);

        LoadingPage.Instance.AddProgressCount(LoadingProgress.DOWNLOAD_HOTFIX_DLL, dlls.Count);

        //����Դ����������DLLд�뵽����
        for (int i = 0; i < dlls.Count; i++)
        {
            string name = dlls[i];
            if (!localHotfixNode.ContainsKey(name)
                || (localHotfixNode[name]["hash"].ToObject<string>() != serverHotfixNode[name]["hash"].ToObject<string>()))
            {

                LoadingPage.Instance.Next(LoadingProgress.DOWNLOAD_HOTFIX_DLL, $"download hotfix dll: {name}  {i+1}/{dlls.Count}");

                Debug.Log($"download hotfix dll: {name}  {i + 1}/{dlls.Count}");

                yield return DownLoadRemoteHotfixDllOnce(name);
            }
        }

        LoadingPage.Instance.RemoveProgress(LoadingProgress.DOWNLOAD_HOTFIX_DLL);
#endif

    }



    // �ȸ�AB��Դ
    private IEnumerator HotfixABResources()
    {
        using (UnityWebRequest req = UnityWebRequest.Get(PathHelper.mainfestWEBURL))
        {
            yield return req.SendWebRequest();
            if (req.isNetworkError || req.isHttpError)
            {
                Debug.LogError(req.error);
                yield break;
            }
            else
            {
                JObject bundleHash = versionFileRemoteNode["asset_bundle"]["bundle_hash"] as JObject;

                var serverManifestAB = AssetBundle.LoadFromMemory(req.downloadHandler.data);
                AssetBundleManifest serverManifest = serverManifestAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                serverManifestAB.Unload(false);

                List<string> downloadFiles;

                if (File.Exists(PathHelper.mainfestLOCPTH))
                {
                    var localManifestAB = AssetBundle.LoadFromFile(PathHelper.mainfestLOCPTH);
                    AssetBundleManifest localManifest = localManifestAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                    localManifestAB.Unload(false);
                    downloadFiles = GetDownFileName(localManifest, serverManifest);
                }
                else
                {
                    // ����û��manifest�ļ�������AB��Դȫ���ӷ���������
                    downloadFiles = new List<string>(serverManifest.GetAllAssetBundles());
                }

                yield return DownloadABFiles(downloadFiles, bundleHash);

                //��������manifest�����ػ���
                FileUtils.WriteAllBytes(PathHelper.tmpMainfestLOCPTH, req.downloadHandler.data);

                yield return new WaitForEndOfFrame();
            }
        }
    }


    // �õ���Ҫ���ص�AB�ļ���
    private List<string> GetDownFileName(AssetBundleManifest localManifest, AssetBundleManifest serverManifest)
    {
        List<string> tempList = new List<string>();
        var localHashCode = localManifest.GetHashCode();
        var serverHashCode = serverManifest.GetHashCode();
        if (localHashCode != serverHashCode)
        {
            string[] localABList = localManifest.GetAllAssetBundles();
            string[] serverABList = serverManifest.GetAllAssetBundles();
            Dictionary<string, Hash128> localHashDic = new Dictionary<string, Hash128>();
            foreach (var iter in localABList)
            {
                localHashDic.Add(iter, localManifest.GetAssetBundleHash(iter));
            }
            foreach (var iter in serverABList)
            {
                //���֮ǰҲ�����˴�AB ��Աȹ�ϣ�����Ƿ��и���
                if (localHashDic.ContainsKey(iter))
                {
                    Hash128 serverHash = serverManifest.GetAssetBundleHash(iter);
                    if (serverHash != localHashDic[iter])
                    {
                        tempList.Add(iter);
                    }
                }
                //֮ǰ��������һ�����µ�
                else
                {
                    tempList.Add(iter);
                }
            }
        }
        return tempList;
    }


    // ������Ҫ���µ��ļ�
    private IEnumerator DownloadABFiles(List<string> downloadFiles, JObject hash)
    {
       // int completedCount = 0;
        int totalCount = downloadFiles.Count;
        if (totalCount == 0)
        {
            Debug.Log("û����Ҫ���µ�AB��Դ");
            yield break;
        }
        else
        {
            LoadingPage.Instance.AddProgressCount(LoadingProgress.DOWNLOAD_ASSET_BUNDLE, totalCount);
            int i = 0;

            foreach (var iter in downloadFiles)
            {
                i++;
                LoadingPage.Instance.Next(LoadingProgress.DOWNLOAD_ASSET_BUNDLE, $"download ab: {iter} {i}/{totalCount}");
                Debug.Log($"download ab: {iter} {i}/{totalCount}");

                yield return DownloadABFileOnce(iter,hash);
            }
        }

        LoadingPage.Instance.RemoveProgress(LoadingProgress.DOWNLOAD_ASSET_BUNDLE);
    }

    private IEnumerator DownloadABFileOnce(string iter, JObject hash)
    {
        if (PlayerPrefs.GetString(HOTFIX_STATE) == HotfixDownloadFail)
            yield break;
        
        string path = Path.Combine(PathHelper.abDirWEBURL, iter);
        string downloadPath = Path.Combine(PathHelper.tmpABDirLOCPTH, iter);
        //uint calculatedHash = 0;
        string calculatedHash = "";

        using (UnityWebRequest webReq = UnityWebRequest.Get(path))
        {
            yield return webReq.SendWebRequest();
            if (webReq.result == UnityWebRequest.Result.Success)
            {
                byte[] result = webReq.downloadHandler.data;
                FileUtils.WriteAllBytes(downloadPath, result);

                // ���һֱ�ڱ�
                //AssetBundle ab = AssetBundle.LoadFromFile(downloadPath);
                //AssetBundle ab = AssetBundle.LoadFromMemory(result);
                //int dat = ab.GetHashCode();


                // ���ֻ���ڱ༭����ʹ��
                //UnityEditor.BuildPipeline.GetCRCForAssetBundle(downloadPath, out calculatedHash);// ���㱾�ر����AB����CRCֵ

                calculatedHash = FileUtils.CalculateMD5(result);

                yield return new WaitForEndOfFrame();
            }
            else
            {
                Debug.LogError($"download bundle: {iter}  error: {webReq.error}");
                yield return null;
            }
        }
        // �����������
        if (hash != null && hash.ContainsKey(iter) && hash[iter].ToObject<string>() != calculatedHash)
        {   
            using (UnityWebRequest webReq = UnityWebRequest.Get($"{path}?t={DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}"))
            {
                yield return webReq.SendWebRequest();
                if (webReq.result == UnityWebRequest.Result.Success)
                {
                    byte[] result = webReq.downloadHandler.data;
                    FileUtils.WriteAllBytes(downloadPath, result);
                    //yield return new WaitForEndOfFrame();

                    yield break;
                }
                else
                {

                    PlayerPrefs.SetString(HOTFIX_STATE, HotfixDownloadFail);
                    //throw new Exception($"download bundle: {iter} is fail");

                    Debug.LogError($"download bundle: {iter}  error: {webReq.error}");
                    yield return null;
                }
            }
        }


    }

#endregion


    private int[] GetVersions(string version)
    {
        string[] str = version.Split('.');

        List<int> vers = new List<int>();
        for (int i = 0; i < str.Length; i++)
        {
            vers.Add(int.Parse(str[i]));
        }
        // �������汾�źʹΰ汾��
        return vers.ToArray();
    }





}
