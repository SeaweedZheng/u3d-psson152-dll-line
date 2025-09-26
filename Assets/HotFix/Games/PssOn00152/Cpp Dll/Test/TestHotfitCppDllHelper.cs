using Newtonsoft.Json;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using static TestApkCppDllManager;


/// <summary>
/// �ȸ�dll-���԰汾
/// </summary>
public class TestHotfitCppDllHelper : MonoSingleton<TestHotfitCppDllHelper>
{
   
    private const string DLL_NAME01 = "slot-spin";
    // ����ί�����ͣ��� C++ ����ǩ��һ�£�
    public delegate SlotResultNative GetSlotSpin(uint bet);
    public delegate MyStruct GetStructData();
    public void DoCappDll()
    {
        string dllPath = "";

#if UNITY_EDITOR
        dllPath = PathHelper.GetAssetBackupSAPTH("Assets/GameBackup/Cpp Dll/Test/slot-spin.dll.bytes");
        // �� dllPath = Path.Combine(Application.streamingAssetsPath, "Hotfix/GameDll/",  "slot-spin.dll.bytes");
#else
        dllPath = PathHelper.GetAssetBackupLOCPTH("Assets/GameBackup/Cpp Dll/Test/slot-spin.dll.bytes");
#endif

        //Assembly.LoadFile(dllPath); // �����C++��dll ����C#��dll !!

        Debug.Log($"== dllPath = {dllPath}");

        IntPtr _dllHandle = CppDllHotLoader.DoLoadLibrary(dllPath);

        Debug.Log($"== IntPtr = {_dllHandle}");

        // ��ȡ������ַ
        IntPtr _getSlotSpin = CppDllHotLoader.DoGetFunctionPointer(_dllHandle, "GetSlotSpin");
        IntPtr _getStructData = CppDllHotLoader.DoGetFunctionPointer(_dllHandle, "GetStructData");

        // ת��Ϊί��
        GetSlotSpin getSlotSpin = Marshal.GetDelegateForFunctionPointer<GetSlotSpin>(_getSlotSpin);
        GetStructData getStructData = Marshal.GetDelegateForFunctionPointer<GetStructData>(_getStructData);


        Debug.LogError($"@@@ getSlotSpin  == {JsonConvert.SerializeObject(getSlotSpin(22))}");
        Debug.LogError($"@@@ getStructData  == {JsonConvert.SerializeObject(getStructData())}");
#if false
        string dllPath = Path.Combine(Application.streamingAssetsPath, "Hotfix/GameDll/", DLL_NAME);
        byte[] dllBytes;

        // ����ͬƽ̨�Ķ�ȡ��ʽ
        if (Application.platform == RuntimePlatform.Android)
        {
            // Android ��Ҫ�� UnityWebRequest ��ȡ
            UnityWebRequest request = UnityWebRequest.Get(dllPath);
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to load DLL: " + request.error);
                yield break;
            }

            dllBytes = request.downloadHandler.data;
        }
        else
        {
            // Windows/macOS/Linux ֱ�Ӷ�ȡ
            if (!File.Exists(dllPath))
            {
                Debug.LogError("DLL file not found at: " + dllPath);
                yield break;
            }

            dllBytes = File.ReadAllBytes(dllPath);
        }

        Assembly dllAssembly = Assembly.Load(dllBytes);
        Debug.Log("DLL loaded successfully: " + dllAssembly.FullName);
        
        yield return null;
#endif



        CppDllHotLoader.DoFreeLibrary(_dllHandle);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
