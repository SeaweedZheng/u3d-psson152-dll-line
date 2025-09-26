//#define TEST_HOTFIX_0
#define NEW_VER_DLL
using HybridCLR;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;

public class Loader : MonoBehaviour
{

    IEnumerator Start()
    {
        LoadingPage.Instance.Open();

        OnLoadingBefore();

        yield return VersionCheck002.Instance.DoHotfix(null);

        Debug.Log($"autoHotfixUrl = {GlobalData.autoHotfixUrl}");
        Debug.Log($"hofixKey = {GlobalData.hotfixKey}");
        Debug.Log($"hofixVersion = {GlobalData.hotfixVersion}");

        LoadingPage.Instance.RemoveProgress(LoadingProgress.COPY_SA_HOTFIX_DLL);
        LoadingPage.Instance.RemoveProgress(LoadingProgress.COPY_SA_ASSET_BUNDLE);
        LoadingPage.Instance.RemoveProgress(LoadingProgress.CHECK_COPY_TEMP_HOTFIX_FILE);
        LoadingPage.Instance.RemoveProgress(LoadingProgress.CHECK_WEB_VERSION);
        LoadingPage.Instance.RemoveProgress(LoadingProgress.DOWNLOAD_HOTFIX_DLL);
        LoadingPage.Instance.RemoveProgress(LoadingProgress.DOWNLOAD_ASSET_BUNDLE);
        LoadingPage.Instance.RemoveProgress(LoadingProgress.COPY_TEMP_HOTFIX_FILE);
        LoadingPage.Instance.RemoveProgress(LoadingProgress.DELETE_UNUSE_ASSET_BUNDLE);
        LoadingPage.Instance.RemoveProgress(LoadingProgress.DELETE_UNUSE_HOTFIX_DLL);


        #region  �Ǳ༭��������Ҫ����Ԫ���ݣ�����dll������Ԫ���ݣ�
        LoadingPage.Instance.AddProgressCount(LoadingProgress.LOAD_AOT_META_DATA, 1);
        if (!Application.isEditor)
        {
            yield return LoadMetadataForAOTAssemblies(); // s_assetDatas ��AOT Meta ���ص������򼯶�����
        }
        #endregion
        DllHelper.Instance.AddAotMeta();

        LoadingPage.Instance.RemoveProgress(LoadingProgress.LOAD_AOT_META_DATA);


        #region �����ȸ����򼯵� "���򼯶���"��


        //List<string> hotfixDlls = DllHelper.Instance.DllNameList; //�ȸ���
        List<string> hotfixDlls = DllHelper.Instance.GetDllNameList(GlobalData.version);

        Assembly ass = null;

        LoadingPage.Instance.AddProgressCount(LoadingProgress.LOAD_HOTFIX_DLL, hotfixDlls.Count);

        for (int i = 0; i < hotfixDlls.Count; i++)
        {
            if (Application.isEditor) //��ȡ�������ȸ���dll���򼯡�
            {
                ass = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == hotfixDlls[i]);
            }
            else // ��ȡ�����ȸ���dll���򼯡�
            {
                // ���ر��أ��ȸ���dll��
                string path = PathHelper.GetDllLOCPTH(hotfixDlls[i]);
                ass = Assembly.Load(File.ReadAllBytes(path));
            }

            DllHelper.Instance.SethotUpdateAss(hotfixDlls[i], ass);  //���������

            LoadingPage.Instance.Next(LoadingProgress.LOAD_HOTFIX_DLL,  $"load hotfix dll: {hotfixDlls[i]} {i}/{hotfixDlls.Count}");
        }

        LoadingPage.Instance.RemoveProgress(LoadingProgress.LOAD_HOTFIX_DLL);

        #endregion

        //������Ϸ
        OpenMain();
    }

    /// <summary> ����ǰ </summary>
    void OnLoadingBefore()
    {
        GameObject goReporter = GOFind.FindObjectIncludeInactive("Reporter");
        if (goReporter != null)
        {
            goReporter.SetActive(!ApplicationSettings.Instance.isRelease);
        }
    }

    private static IEnumerator LoadMetadataForAOTAssemblies()
    {

#if NEW_VER_DLL

        /// ע�⣬����Ԫ�����Ǹ�AOT dll����Ԫ���ݣ������Ǹ��ȸ���dll����Ԫ���ݡ�
        /// �ȸ���dll��ȱԪ���ݣ�����Ҫ���䣬�������LoadMetadataForAOTAssembly�᷵�ش���
        HomologousImageMode mode = HomologousImageMode.SuperSet;

        LoadingPage.Instance.AddProgressCount(LoadingProgress.LOAD_AOT_META_DATA, DllHelper.Instance.AOTMetaAssemblyFiles.Count);
        int i = 0;


        //�ӱ���Streaming����dll
        foreach (string item in AOTGenericReferences.PatchedAOTAssemblyList)
        {
            string aotDllName = $"{item}.bytes";

            UnityWebRequest req = UnityWebRequest.Get(Application.streamingAssetsPath + "/AOTMeta/" + aotDllName);

            LoadingPage.Instance.Next(LoadingProgress.LOAD_AOT_META_DATA,
                $"load streamingAssets/{aotDllName} {++i}/{AOTGenericReferences.PatchedAOTAssemblyList.Count}");

            yield return req.SendWebRequest();
            if (req.result == UnityWebRequest.Result.Success)
            {
                byte[] aotMetaBytes = req.downloadHandler.data;

                // ����assembly��Ӧ��dll�����Զ�Ϊ��hook��һ��aot���ͺ�����native���������ڣ��ý������汾����
                LoadImageErrorCode err = RuntimeApi.LoadMetadataForAOTAssembly(aotMetaBytes, mode);
                //Debug.Log($"LoadMetadataForAOTAssembly:{aotDllName}. mode:{mode} ret:{err}");
            }
            else
            {
                throw new Exception($"����AOTԪ���ݱ��� {aotDllName}");
            }
        }
        LoadingPage.Instance.RemoveProgress(LoadingProgress.LOAD_AOT_META_DATA);

#else

        /// ע�⣬����Ԫ�����Ǹ�AOT dll����Ԫ���ݣ������Ǹ��ȸ���dll����Ԫ���ݡ�
        /// �ȸ���dll��ȱԪ���ݣ�����Ҫ���䣬�������LoadMetadataForAOTAssembly�᷵�ش���
        HomologousImageMode mode = HomologousImageMode.SuperSet;

        LoadingPage.Instance.AddProgressCount(LoadingProgress.LOAD_AOT_META_DATA, DllHelper.Instance.AOTMetaAssemblyFiles.Count);
        int i = 0;

        //�ӱ���Streaming����dll
        foreach (var aotDllName in DllHelper.Instance.AOTMetaAssemblyFiles)
        {
            UnityWebRequest req = UnityWebRequest.Get(Application.streamingAssetsPath + "/AOTMeta/" + aotDllName);

            LoadingPage.Instance.Next(LoadingProgress.LOAD_AOT_META_DATA,
                $"load streamingAssets/{aotDllName} {++i}/{DllHelper.Instance.AOTMetaAssemblyFiles.Count}");

            yield return req.SendWebRequest();
            if (req.result == UnityWebRequest.Result.Success)
            {
                byte[] aotMetaBytes = req.downloadHandler.data;

                // ����assembly��Ӧ��dll�����Զ�Ϊ��hook��һ��aot���ͺ�����native���������ڣ��ý������汾����
                LoadImageErrorCode err = RuntimeApi.LoadMetadataForAOTAssembly(aotMetaBytes, mode);
                //Debug.Log($"LoadMetadataForAOTAssembly:{aotDllName}. mode:{mode} ret:{err}");
            }
            else
            {
                throw new Exception($"����AOTԪ���ݱ��� {aotDllName}");
            }
        }
        LoadingPage.Instance.RemoveProgress(LoadingProgress.LOAD_AOT_META_DATA);

#endif

    }

    private void OpenMain()
    {
        Assembly ass = DllHelper.Instance.GetAss("HotFix");
        Type t = ass.GetType("Main");
        MethodInfo m = t.GetMethod("MainStart");
        m.Invoke(null, null);
    }


}
