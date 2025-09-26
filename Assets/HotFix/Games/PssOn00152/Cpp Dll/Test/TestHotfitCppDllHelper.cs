using Newtonsoft.Json;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using static TestApkCppDllManager;


/// <summary>
/// 热更dll-测试版本
/// </summary>
public class TestHotfitCppDllHelper : MonoSingleton<TestHotfitCppDllHelper>
{
   
    private const string DLL_NAME01 = "slot-spin";
    // 定义委托类型（与 C++ 函数签名一致）
    public delegate SlotResultNative GetSlotSpin(uint bet);
    public delegate MyStruct GetStructData();
    public void DoCappDll()
    {
        string dllPath = "";

#if UNITY_EDITOR
        dllPath = PathHelper.GetAssetBackupSAPTH("Assets/GameBackup/Cpp Dll/Test/slot-spin.dll.bytes");
        // 或 dllPath = Path.Combine(Application.streamingAssetsPath, "Hotfix/GameDll/",  "slot-spin.dll.bytes");
#else
        dllPath = PathHelper.GetAssetBackupLOCPTH("Assets/GameBackup/Cpp Dll/Test/slot-spin.dll.bytes");
#endif

        //Assembly.LoadFile(dllPath); // 这个是C++的dll 不是C#的dll !!

        Debug.Log($"== dllPath = {dllPath}");

        IntPtr _dllHandle = CppDllHotLoader.DoLoadLibrary(dllPath);

        Debug.Log($"== IntPtr = {_dllHandle}");

        // 获取函数地址
        IntPtr _getSlotSpin = CppDllHotLoader.DoGetFunctionPointer(_dllHandle, "GetSlotSpin");
        IntPtr _getStructData = CppDllHotLoader.DoGetFunctionPointer(_dllHandle, "GetStructData");

        // 转换为委托
        GetSlotSpin getSlotSpin = Marshal.GetDelegateForFunctionPointer<GetSlotSpin>(_getSlotSpin);
        GetStructData getStructData = Marshal.GetDelegateForFunctionPointer<GetStructData>(_getStructData);


        Debug.LogError($"@@@ getSlotSpin  == {JsonConvert.SerializeObject(getSlotSpin(22))}");
        Debug.LogError($"@@@ getStructData  == {JsonConvert.SerializeObject(getStructData())}");
#if false
        string dllPath = Path.Combine(Application.streamingAssetsPath, "Hotfix/GameDll/", DLL_NAME);
        byte[] dllBytes;

        // 处理不同平台的读取方式
        if (Application.platform == RuntimePlatform.Android)
        {
            // Android 需要用 UnityWebRequest 读取
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
            // Windows/macOS/Linux 直接读取
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
