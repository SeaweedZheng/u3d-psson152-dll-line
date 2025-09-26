using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;






public partial class AssetBundleBuilder03 : EditorWindow
{

    [MenuItem("NewBuild/����DLL��StreamingAssetsĿ¼GameDll �²����ĺ�׺��")]
    public static void CopyDllAndReName002()
    {

        CopyDll();

        // ˢ�°汾��
        UpdateVersionData__002();

        AssetDatabase.Refresh();
    }



    static void CopyDll()
    {
        string toDirPath = PathHelper.dllDirSAPTH;

        // ɾ�������ļ�
        ClearHotfixDll_002(toDirPath);


        if (Directory.Exists(toDirPath) == false)  // �ж��Ƿ���ڣ������ڴ���
        {
            Directory.CreateDirectory(toDirPath); // �����ļ���·�������ļ���
        }


        string dataPath = Application.dataPath;
        string projectRootPath = Directory.GetParent(dataPath).FullName;
        string targetPath = Path.Combine(projectRootPath, "HybridCLRData/HotUpdateDlls/" + EditorUserBuildSettings.activeBuildTarget.ToString());//D:\work\u3d-po\HybridCLRData/HotUpdateDlls/Android
        List<string> list = DllHelper.Instance.DllNameList;
        for (int i = 0; i < list.Count; i++)
        {
            string sourcePath = Path.Combine(targetPath, list[i] + ".dll");
            string destinaltionPaht = Path.Combine(toDirPath, list[i] + ".dll.bytes");
            if (File.Exists(sourcePath))
            {
                try
                {
                    File.Copy(sourcePath, destinaltionPaht, overwrite: true);
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
            }
            else
            {
                Debug.Log("Ҫ�������ļ�������" + sourcePath);
            }
        }
    }




    [MenuItem("NewBuild/���1001")]
    public static void BuildPigSlotGameResource002()
    {
        CopyDll();
        CopyAssetBackup();

        string toPath = PathHelper.abDirSAPTH;


        if (Directory.Exists(toPath) == false)  // �ж��Ƿ���ڣ������ڴ���
        {
            Directory.CreateDirectory(toPath); // �����ļ���·�������ļ���
        }

        GetNopkDir();

        ResetABName_002();

        MarkResourceNameEx_002();

        MarkLuBanJson_003();

        //MarkLuBanJson_002();

        MarkResoueceABs_002();

        MarkResourceSounds_002();
        BuildPipeline.BuildAssetBundles(toPath, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);


        ClearUnuseABAndManifest_002();
        ClearUnuseFolderAndMeta_002();

        // ˢ�°汾��
        UpdateVersionData__002();

        AssetDatabase.Refresh();

        Debug.Log("��ϲ,����ɹ�");
    }



    [MenuItem("NewBuild/������ԴStreamingAssetsĿ¼GameBackup �²��޸İ汾�ļ�")]
    public static void AssetBackup()
    {
        CopyAssetBackup();

        // ˢ�°汾��
        UpdateVersionData__002();

        AssetDatabase.Refresh();
    }
    public static void CopyAssetBackup()
    {
        string toDirPath = PathHelper.backupDirSAPTH;

        // �ݹ�ɾ���ļ��м�����������
        if (Directory.Exists(toDirPath))
        {
            Directory.Delete(toDirPath, recursive: true);
        }

        if (Directory.Exists(toDirPath) == false)  // �ж��Ƿ���ڣ������ڴ���
        {
            Directory.CreateDirectory(toDirPath); // �����ļ���·�������ļ���
        }

        string folderPthLst = PathHelper.gameBackupDirPROJPTH; //E:/work4/u3d-dll-po/Assets

        List<string> targetPathLst = new List<string>();
        targetPathLst.AddRange(GetTargetFilePath(PathHelper.gameBackupDirPROJPTH, ".*"));

        foreach (var pth in targetPathLst)
        {
            string sourcePath = pth;
            string destinaltionPaht = PathHelper.GetAssetBackupSAPTH(pth);
           if (File.Exists(sourcePath))
            {
                try
                {
                    string destDirectory = Path.GetDirectoryName(destinaltionPaht);
                    if (destDirectory == null)
                    {
                        Debug.LogError("�����޷�����Ŀ��Ŀ¼·����");
                        return;
                    }

                    if (!Directory.Exists(destDirectory))
                    {
                        Directory.CreateDirectory(destDirectory);
                    }

                    File.Copy(sourcePath, destinaltionPaht, overwrite: true);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
            else
            {
                Debug.Log("Ҫ�������ļ�������" + sourcePath);
            }
            Debug.Log(destinaltionPaht);
        }
    }





    [MenuItem("NewBuild/�����ԡ�-��ʾ����·��")]
    private static void TestShowLocalPath()
    {
        // ɾ�������ļ�
        Debug.Log(Application.persistentDataPath);
    }




    /*
    [MenuItem("NewBuild/�������ab����")]
    public static void ClearAllAbName()
    {
        ResetABName_002();
    }*/


    /*
    static void MarkLuBanJson_002()
    {
        string luBanJsonPath = Application.dataPath + "/GameRes/LuBan/GenerateDatas/bytes";
        foreach (string path in Directory.GetFiles(luBanJsonPath))
        {
            if (System.IO.Path.GetExtension(path) == ".json")
            {
                string p = path.Substring(path.IndexOf("Assets"));
                string bundelName = path.Substring(path.IndexOf("LuBan"));//LuBan/GenerateDatas/bytes/xxxx.json
                if (bundelName.EndsWith("json"))
                {
                    bundelName = bundelName.Replace("json", "unity3d");
                }
                AssetImporter.GetAtPath(p).assetBundleName = bundelName;
            }
        }
    }
    */

    static void MarkLuBanJson_003()
    {
        string targetDir = PathHelper.gameResDirPROJPTH;

        List<string> folderPthLst = GetAllFolderPath(targetDir, "\\LuBan");
        List<string> targetPathLst = new List<string>();
        foreach (var pth in folderPthLst)
        {
            targetPathLst.AddRange(GetTargetFilePath(pth, ".json"));
        }
        foreach (string pth in targetPathLst)
        {
            //Debug.Log($"@ ABs =={pth}");
            SetBundleName(pth);
        }
    }





    /// <summary>
    /// ABs�ļ����µ������ļ������
    /// </summary>
    static void MarkResoueceABs_002()
    {

        string rootFolderPth = PathHelper.gameResDirPROJPTH; // Application.dataPath + "/GameRes";
        List<string> folderPthLst = GetAllFolderPath(rootFolderPth, "\\ABs");  //ԭ��ABs

        List<string> targetPathLst = new List<string>();
        foreach (var pth in folderPthLst)
        {
            targetPathLst.AddRange(GetTargetFilePath(pth, ".*"));
        }

        foreach (string pth in targetPathLst)
        {
            //Debug.Log($"@ ABs =={pth}");
            SetBundleName(pth);
        }
    }

    /// <summary>
    /// ɾ��·���µ�����ab����
    /// </summary>
    /// <param name="folderPath"></param>
    private static void ResetABName_002()//(string folderPath = "GameRes")
    {
        List<string> assetPaths = new List<string>();
        // ��ȡָ���ļ��е�����·��
        //string fullFolderPath = Path.Combine(Application.dataPath, folderPath);

        string fullFolderPath = PathHelper.gameResDirPROJPTH;

        if (!Directory.Exists(fullFolderPath))
        {
            Debug.LogError($"ָ�����ļ��� {fullFolderPath} �����ڡ�");
            return;
        }

        // ��ȡָ���ļ����µ������ļ�
        string[] allFiles = Directory.GetFiles(fullFolderPath, "*", SearchOption.AllDirectories);

        foreach (string filePath in allFiles)
        {
            //string p = filePath.Substring(filePath.IndexOf("Assets"));//Assets/GameRes/Games\666.prefab
            //AssetImporter.GetAtPath(p).assetBundleName = null;


            if (filePath.EndsWith(".cs") || filePath.EndsWith(".meta"))
            {
                continue;
            }

            // ���ļ��ľ���·��ת��Ϊ����� Assets Ŀ¼��·��
            string assetPath = "Assets" + filePath.Replace(Application.dataPath, "").Replace('\\', '/');
            // ��ȡ����Դ�ĵ�����
            AssetImporter importer = AssetImporter.GetAtPath(assetPath);
            if (importer != null && importer.assetBundleName != null)
            {
                importer.assetBundleName = null;
            }
        }
    }
    static void MarkResourceNameEx_002()
    {

        string targetDir = PathHelper.gameResDirPROJPTH; // Application.dataPath + "/GameRes";
        //##ResetAssetBundleName_002(targetDir);

        List<string> folderPthLst = GetAllFolderPath(targetDir, "\\Prefabs");
        List<string> targetPathLst = new List<string>();
        foreach (var pth in folderPthLst)
        {
            targetPathLst.AddRange(GetTargetFilePath(pth, ".prefab"));
        }
        foreach (string pth in targetPathLst)
        {
            //Debug.Log($"@ ABs =={pth}");
            SetBundleName(pth);
        }
    }

    /*
    private static void ResetAssetBundleName_002(string dirPath)
    {
        foreach (string path in Directory.GetFiles(dirPath))
        {
            //��ȡ�����ļ����а�����׺Ϊ .prefab ��·��
            if (System.IO.Path.GetExtension(path) == ".prefab")
            {
                string p = path.Substring(path.IndexOf("Assets"));//Assets/GameRes/Games\666.prefab
                AssetImporter.GetAtPath(p).assetBundleName = null;
            }
        }

        if (Directory.GetDirectories(dirPath).Length > 0)  //���������ļ���
        {
            foreach (string path in Directory.GetDirectories(dirPath))
            {
                ResetAssetBundleName_002(path);
            }
        }
    }*/


    /*
    static void MarkResourceDatas_002()
    {
        string rootFolderPth = PathHelper.gameResDirPROJPTH; // Application.dataPath + "/GameRes";
        List<string> folderPthLst = GetAllFolderPath(rootFolderPth, "\\Datas");  //ԭ��Datas

        List<string> targetPathLst = new List<string>();
        foreach (var pth in folderPthLst)
        {
            targetPathLst.AddRange(GetTargetFilePath(pth, ".json"));
        }

        foreach (string pth in targetPathLst)
        {
            //Debug.Log($"@=={pth}");
            SetBundleName(pth);
        }
    }*/

    /// <summary>
    /// �������Sounds�ļ���������
    /// </summary>
    static void MarkResourceSounds_002()
    {
        string rootFolderPth = PathHelper.gameResDirPROJPTH; // Application.dataPath + "/GameRes";
        List<string> folderPthLst = GetAllFolderPath(rootFolderPth, "\\Sounds");  //ԭ��Datas

        List<string> targetPathLst = new List<string>();
        foreach (var pth in folderPthLst)
        {
            targetPathLst.AddRange(GetTargetFilePath(pth, ".mp3"));
            targetPathLst.AddRange(GetTargetFilePath(pth, ".wav"));
        }

        foreach (string pth in targetPathLst)
        {
            //Debug.Log($"@=={pth}");
            SetBundleName(pth);
        }
    }




    /// <summary>
    /// ��������Ԥ���壬����Ԥ������
    /// </summary>
    /// <param name="rootFolderPath"></param>
    private static List<string> GetTargetFilePath(string rootFolderPath, string extension = ".png")
    {
        List<string> paths = new List<string>();
        foreach (string path in Directory.GetFiles(rootFolderPath))
        {

            string pth = path.Replace("\\", "/");

            bool isIgnore = false;
            for (int i = 0; i< nopkDir.Count; i++)
            {
                if (pth.StartsWith(nopkDir[i]))
                {
                    isIgnore = true;
                    break;
                }
            }
            if(isIgnore) continue;  //��������

            //if (extension.EndsWith(".cs") || pth.Contains("/Editor/")) continue;       

            //if (path.Contains("nopk__")) continue; //��������

            //��ȡ�����ļ����а�����׺Ϊ .prefab ��·��
            if (extension == ".*") // path System.IO.Path.GetExtension(path) != ".meta"
            {
                if (!path.EndsWith(".meta") && !path.EndsWith(".cs"))
                    paths.Add(pth);
            }
            //else if (System.IO.Path.GetExtension(path) == extension)
            else if (path.EndsWith(extension))
            {
                paths.Add(pth);
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


    static List<string> GetAllFolderPath(string pathRoot, string targetFolder)
    {
        List<string> res = new List<string>();
        string[] chdPath = Directory.GetDirectories(pathRoot);

        if (chdPath.Length > 0)  //���������ļ���
        {
            foreach (string path in chdPath)
            {
                if (path.EndsWith(targetFolder))
                {
                    res.Add(path);
                }
                res.AddRange(GetAllFolderPath(path, targetFolder));
            }
        }
        return res;
    }



    static void SetBundleName(List<string> pathLst)
    {
        foreach (var pth in pathLst)
        {
            SetBundleName(pth);
        }
    }

    static void SetBundleName(string pth)
    {
        string extension = Path.GetExtension(pth);

        string p = pth.Substring(pth.IndexOf("Assets"));
        //string bundelName = pth.Substring(pth.IndexOf("GameRes")); //  GameRes\Games/Game Maker\Datas\page.json
        string bundelName = pth.Substring(pth.IndexOf("GameRes") + "GameRes".Length + 1);  //  Games/Game Maker\Datas\page.json
        bundelName = bundelName.Replace(extension, ".unity3d");
        //Debug.Log($"{bundelName}");
        AssetImporter.GetAtPath(p).assetBundleName = bundelName;
    }



    /// <summary>
    /// ɾ�����õ�ab��
    /// </summary>
    static void ClearUnuseABAndManifest_002()
    {
        List<string> unusePths = GetUnuseAB();
        foreach (string pth in unusePths)
        {
            if (File.Exists(pth))
            {
                Debug.Log($"ɾ�����õ�ab�� : {pth}");
                File.Delete(pth);
            }
        }

    }



    static void ClearUnuseFolderAndMeta_002() // dir = Application.streamingAssetsPath
    {

        // Ҫ�������ļ��еĸ�Ŀ¼·���������� Application.dataPath Ϊ������ Assets �ļ���
        string rootDirectory = PathHelper.abDirSAPTH;
        List<string> allSubDirectories = GetAllSubFolders(rootDirectory);

        // ���ļ���·�����ַ����ȴӳ����̽�������Խ����·������Խǰ�棩
        allSubDirectories.Sort((a, b) => b.Length - a.Length);

        /*
        // ����������ļ��е�����
        foreach (string directory in allSubDirectories)
        {
            Debug.Log(directory);
        }*/

        // ����������Ŀ¼·��
        foreach (string directory in allSubDirectories)
        {
            if (ShouldDeleteDirectory(directory))
            {
                DeleteDirectoryAndMeta(directory);
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
    static List<string> GetUnuseAB()
    {
        //string manifestBundleName = "GameRes"; // ���� manifest �ļ����ڵ� AssetBundle ����
        string manifestAssetName = "AssetBundleManifest"; // ���� manifest �ļ�����Դ����

        //string assetBundlePath = Application.streamingAssetsPath + "/GameRes/" + manifestBundleName;
        string assetBundlePath = PathHelper.mainfestSAPTH;
        AssetBundle manifestBundle = AssetBundle.LoadFromFile(assetBundlePath);
        AssetBundleManifest manifest = manifestBundle.LoadAsset<AssetBundleManifest>(manifestAssetName);
        manifestBundle.Unload(false);

        string[] allAssetBundleNames = manifest.GetAllAssetBundles();

        List<string> bundlePathLst = new List<string>();
        foreach (string assetBundleName in allAssetBundleNames)
        {
            // Debug.Log("[1] AssetBundle Name: " + assetBundleName);
            // assetBundleName =  "luban/generatedatas/bytes/i18n_console001.unity3d";
            string pth1 = Path.Combine(PathHelper.abDirSAPTH, assetBundleName);
            bundlePathLst.Add(pth1.Replace("\\", "/"));

        }

        // ����ab�����ǲ���".unity3d"��β�ġ���"GameRes" �� ��GameRes.manifest����
        List<string> targetPathLst = new List<string>();  //��ȡ��ͨ��·�� xxx.unity3d  ��  xxx.unity3d.manifest
        targetPathLst.AddRange(GetTargetFilePath(PathHelper.abDirSAPTH, ".unity3d"));
        targetPathLst.AddRange(GetTargetFilePath(PathHelper.abDirSAPTH, ".unity3d.manifest"));

        for (int i = 0; i < targetPathLst.Count; i++)
        {
            targetPathLst[i] = targetPathLst[i].Replace("\\", "/");
        }

        List<string> unusePths = new List<string>();
        foreach (string pth002 in targetPathLst)
        {
            string tempPth = pth002.EndsWith(".unity3d.manifest") ? pth002.Replace(".unity3d.manifest", ".unity3d") : pth002;
            if (!bundlePathLst.Contains(tempPth))
            {
                unusePths.Add(pth002);
            }
        }

        Dictionary<string, object> jsonObject = new Dictionary<string, object>();
        jsonObject.Add("bundlePaths", bundlePathLst);
        jsonObject.Add("allPaths", targetPathLst); //һ����һ����Դ�������������Դ�����ǰ�����
        jsonObject.Add("unusePaths", unusePths);

        string res = JsonConvert.SerializeObject(jsonObject);

        string pth = PathHelper.hotfixDirSAPTH + "/test_clear_file_info.json";
        // WriteAllText(pth, res);
        File.WriteAllText(pth, res); //�Ḳ��д��

        Debug.Log($"�����Ϣ�������ļ� {pth}");

        return unusePths;
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

            Debug.Log($"��ɾ�����õ�Ŀ¼: {directoryPath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"ɾ��Ŀ¼ {directoryPath} ʱ����: {e.Message}");
        }
    }

    /// <summary>
    /// ɾ���ȸ�dll
    /// </summary>
    /// <param name="rootDirectory"></param>
    static void ClearHotfixDll_002(string rootDirectory)
    {
        List<string> targetPathLst = new List<string>();  //��ȡ��ͨ��·�� xxx.unity3d  ��  xxx.unity3d.manifest
        targetPathLst.AddRange(GetTargetFilePath(rootDirectory, ".dll.bytes"));

        for (int i = 0; i < targetPathLst.Count; i++)
        {
            File.Delete(targetPathLst[i]);
        }
    }

    /// <summary>
    /// ˢ�°汾��
    /// </summary>
    static void UpdateVersionData__002()
    {
        //string mainfestSAPTH = Application.streamingAssetsPath + "/GameRes/GameRes";
        //string hotfixDllDirSAPTH = Application.streamingAssetsPath + "/GameDll";
        // string versionSAPTH = Application.streamingAssetsPath + "/GameDll/version_0.json";

        string mainfestSAPTH = PathHelper.mainfestSAPTH;
        Debug.Log("@@@ = " + mainfestSAPTH);
        //string hotfixDllDirSAPTH = PathHelper.hotfixDllDirSAPTH;
        string versionSAPTH = PathHelper.versionSAPTH;

        JObject versionFileSA = JObject.Parse(File.ReadAllText(versionSAPTH));

        string manifestHash = FileUtils.CalculateFileMD5(mainfestSAPTH);
        versionFileSA["asset_bundle"]["manifest"]["hash"] = manifestHash;


        var localManifestAB = AssetBundle.LoadFromFile(mainfestSAPTH);
        AssetBundleManifest localManifest = localManifestAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        localManifestAB.Unload(false);
        string[] abList = localManifest.GetAllAssetBundles();
        JObject bundleCrc = new JObject();

        foreach (var iter in abList)
        {

            string pth = Path.Combine(PathHelper.abDirSAPTH, iter);


            /* crc �޷�ʹ��
            uint hash = 0;
            BuildPipeline.GetCRCForAssetBundle(pth, out hash);// ���㱾�ر����AB����CRCֵ
            */

            string hash = FileUtils.CalculateFileMD5(pth);

            bundleCrc.Add(iter, hash);
        }

        versionFileSA["asset_bundle"]["bundle_hash"] = bundleCrc;


        JObject hotfixDll = new JObject();

        List<string> targetPathLst = new List<string>();  //��ȡ��ͨ��·�� xxx.unity3d  ��  xxx.unity3d.manifest
        targetPathLst.AddRange(GetTargetFilePath(PathHelper.dllDirSAPTH, ".dll.bytes"));


        for (int i = 0; i < targetPathLst.Count; i++)
        {
            string[] pths = targetPathLst[i].Replace("\\", "/").Split('/');
            string name = pths[pths.Length - 1].Replace(".dll.bytes", "");
            string hash = FileUtils.CalculateFileMD5(targetPathLst[i]);
            JObject item = new JObject();
            item.Add("hash", hash);
            hotfixDll.Add(name, item);
        }

        versionFileSA["hotfix_dll"] = hotfixDll;

        versionFileSA["hotfix_dll_load_order"] = JArray.Parse(JsonConvert.SerializeObject(DllHelper.Instance.DllNameList));



        // ��Դ����
        JObject assetsBackup = new JObject();
        List<string> backupPathLst = new List<string>();  //��ȡ��ͨ��·�� xxx.unity3d  ��  xxx.unity3d.manifest
        backupPathLst.AddRange(GetTargetFilePath(PathHelper.backupDirSAPTH, ".*"));

        for (int i = 0; i < backupPathLst.Count; i++)
        {
            string name = PathHelper.GetAssetBackupNodeName(backupPathLst[i]);
            string hash = FileUtils.CalculateFileMD5(backupPathLst[i]);
            JObject item = new JObject();
            item.Add("hash", hash);
            assetsBackup.Add(name, item);
        }
        versionFileSA["asset_backup"] = assetsBackup;



        #region �޸İ汾��

        string hotfixVer = versionFileSA["hotfix_version"].Value<string>();
        string targetHFID = versionFileSA["hotfix_key"].Value<string>();

        string appType = ApplicationSettings.Instance.isRelease ? "release" : "debug";
        string hfIDPrefix = $"hf_{appType}_{EditorUserBuildSettings.activeBuildTarget.ToString().ToLower()}";

        //bool isCreatNode = false;

        string[] appVers = ApplicationSettings.Instance.appVersion.Split('.');
        string[] hotfixVers = hotfixVer.Split('.');

        string targetVer;

        if (!targetHFID.StartsWith(hfIDPrefix)
            || hotfixVers[0] != appVers[0]
            || hotfixVers[1] != appVers[1])
        {
            targetVer = $"{appVers[0]}.{appVers[1]}.0";

            long timeMS = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            DateTime localDateTime01 = DateTimeOffset.UtcNow.LocalDateTime;
            string yyyyMMddHHmmss = localDateTime01.ToString("yyyyMMddHHmmss");
            //hf_�汾_����_����ʱ��
            targetHFID = hfIDPrefix + "_" + yyyyMMddHHmmss;
            //isCreatNode = true;
        }
        else
        {
            int miniVer = int.Parse(hotfixVers[2]) + 1;
            targetVer = $"{hotfixVers[0]}.{hotfixVers[1]}.{miniVer}";
        }


        versionFileSA["hotfix_version"] = targetVer;
        versionFileSA["hotfix_key"] = targetHFID;
        // �޸İ汾��
        versionFileSA["updated_at"] = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        #endregion

        Debug.Log($"���°汾�ţ� {versionFileSA["hotfix_version"].Value<string>()}");
        string content = versionFileSA.ToString();
        File.WriteAllText(versionSAPTH, content);
    }



}




public partial class AssetBundleBuilder03 : EditorWindow
{


    public class NopkConfig
    {
        public string[] ignore { get; set; }
    }

    [MenuItem("NewBuild/��ǰ�ȸ��汾")]
    private static void ReadCurVersion()
    {

        string content = File.ReadAllText(PathHelper.versionSAPTH);

        JObject verObj = JObject.Parse(content);

        Debug.Log($"��ǰ�ȸ��汾�� {verObj["hotfix_version"].Value<string>()}");
    }



    [MenuItem("NewBuild/�����ԡ�-��ȡyaml�ļ�")]
    private static void TestShowYaml()
    {
        //GetNopkDir();

        Debug.Log(((int)123456.7f).ToString());
    }


    static List<string> nopkDir = new List<string>();

    public static void GetNopkDir()
    {
        nopkDir = new List<string>();

        // ��ȡ YAML �ļ�����
        string yamlFilePath = Application.dataPath + "/" + "nopk.yaml";
        string yamlContent = File.ReadAllText(yamlFilePath);

        // ���������л���
        var deserializer = new DeserializerBuilder()
           .WithNamingConvention(CamelCaseNamingConvention.Instance)
           .Build();

        // �����л� YAML ���ݵ�����
        NopkConfig node = deserializer.Deserialize<NopkConfig>(yamlContent);

        Debug.Log($" NopkConfig : {JsonConvert.SerializeObject(node)}");
        // ��������л���Ķ�����Ϣ

        //Debug.Log($" pth 000 : {Application.dataPath}");
        foreach (string item in node.ignore)
        {
            if (item.EndsWith("/"))
            {
                if (item.StartsWith("./") || item.StartsWith("../"))
                {
                    nopkDir.Add(FileUtils.GetDirWebUrl(yamlFilePath, item).Replace("file:///", "").Replace("\\", "/"));
                    //Debug.Log($" pth : {FileUtils.GetDirWebUrl(yamlFilePath, item).Replace("file:///", "").Replace("\\", "/")}");
                }
            }
        }
    }


}

public partial class AssetBundleBuilder03 : EditorWindow
{



    [MenuItem("NewBuild/�����ԡ���ӡStreamingAssets����AB����")]
    private static void TestShowStreamingAssetsABInfo()
    {
        ShowStreamingAssetsABInfo();
    }

    public static void ShowStreamingAssetsABInfo() // dir = Application.streamingAssetsPath
    {

        string manifestAssetName = "AssetBundleManifest"; // ���� manifest �ļ�����Դ����
        string assetBundlePath = PathHelper.mainfestSAPTH;
        AssetBundle manifestBundle = AssetBundle.LoadFromFile(assetBundlePath);
        AssetBundleManifest manifest = manifestBundle.LoadAsset<AssetBundleManifest>(manifestAssetName);
        manifestBundle.Unload(false);


        Dictionary<string, string> bundleNameLst = new Dictionary<string, string>();
        Dictionary<string, string> AssetNameLst = new Dictionary<string, string>();
        // ��ȡ���� AssetBundle ����
        string[] allAssetBundleNames = manifest.GetAllAssetBundles();

        int i = 0;
        foreach (string assetBundleName in allAssetBundleNames)
        {
            // Debug.Log("[1] AssetBundle Name: " + assetBundleName);

            i++;
            bundleNameLst.Add($"[{i}]", $"{assetBundleName}");

            // ���ظ� AssetBundle
            string abSAPTH =  Path.Combine(PathHelper.abDirSAPTH, assetBundleName);
            AssetBundle subAssetBundle = AssetBundle.LoadFromFile(abSAPTH);


            // ��ȡ�� AssetBundle �е�������Դ����
            string[] allAssetNames = subAssetBundle.GetAllAssetNames();
            foreach (string assetName in allAssetNames)
            {
                //Debug.Log("[2] Asset Name: " + assetName);
                AssetNameLst.Add($"#[{i}]", $"{assetName}");
            }

            // ж�ظ� AssetBundle
            subAssetBundle.Unload(false);
        }

        Dictionary<string, object> jsonObject = new Dictionary<string, object>();

        jsonObject.Add("bundleNames", bundleNameLst);
        jsonObject.Add("assetNames", AssetNameLst); //һ����һ����Դ�������������Դ�����ǰ�����

        string res = JsonConvert.SerializeObject(jsonObject);


        string localAllABJsonPath = Path.Combine(PathHelper.hotfixDirSAPTH, "test_all_ab_info.json");
        WriteAllText(localAllABJsonPath, res);
        //Debug.Log($"������ɣ�  {res}");
        Debug.Log($"����������ab����Ϣ�� {localAllABJsonPath}");
    }


    static void WriteAllText(string path, string content)
    {
        string directory = Path.GetDirectoryName(path);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        File.WriteAllText(path, content); //�Ḳ��д��

    }


    [MenuItem("NewBuild/�����ԡ���ӡҪ������ļ�")]
    private static void TestCleatFile()
    {
        GetUnuseAB();
    }









    [MenuItem("NewBuild/�����ԡ�-��ӡab��calendar.unity3d��CRC")]
    private static void TestShowAbCrc()
    {

        string iter = "games/console001/calendar bundle/prefabs/calendar.unity3d";

        string mainfestSAPTH = PathHelper.mainfestSAPTH;
        var serverManifestAB = AssetBundle.LoadFromFile(mainfestSAPTH);
        AssetBundleManifest serverManifest = serverManifestAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        serverManifestAB.Unload(false);

        Hash128 serverHash = serverManifest.GetAssetBundleHash(iter);


        Debug.Log(Application.streamingAssetsPath);
        string targetFilePth = PathHelper.GetAssetBundleSAPTH(iter);  

        //string targetFilePth = Path.Combine(Application.streamingAssetsPath, "/GameRes/games/console001/calendar bundle/prefabs/calendar.unity3d"); ����д������
        //string targetFilePth = Application.streamingAssetsPath + "/GameRes/games/console001/calendar bundle/prefabs/calendar.unity3d";
        if (File.Exists(targetFilePth))
        {

            AssetBundle ab = AssetBundle.LoadFromFile(targetFilePth);
            //AssetBundle ab = AssetBundle.LoadFromMemory(result);
            int hash = ab.GetHashCode();
            ab.Unload(false);

            // ������ɡ�������Ҫ������������У��
            uint calculatedCRC;
            // ���㱾�ر����AB����CRCֵ
            BuildPipeline.GetCRCForAssetBundle(targetFilePth, out calculatedCRC);
            Debug.Log($"BuildPipeline.GetCRCForAssetBundle: {calculatedCRC} -- AssetBundle.GetHashCode: {hash} -- Manifest.GetAssetBundleHash: {serverHash}" );

        }
        else
        {
            Debug.Log("�������ļ� ��" + targetFilePth);
        }

        /*
        Hash128 serverHash = serverManifest.GetAssetBundleHash(iter);
        uint calculatedCRC;
        BuildPipeline.GetCRCForAssetBundle(targetFilePth, out calculatedCRC);
        serverHash �� calculatedCRC ��αȽ�*/
    }

}