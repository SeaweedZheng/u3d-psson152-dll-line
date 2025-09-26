using Newtonsoft.Json;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class TestHotfitCppDllHelper001 : MonoSingleton<TestHotfitCppDllHelper001>
{


    // 定义与C++结构体匹配的C#结构体
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct MyStruct
    {
        public int id;
        public float value;

        //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        //public string name;

        public IntPtr name; // 必须是 IntPtr，才能用 PtrToStringAnsi
    }


    // 定义委托类型（与 C++ 函数签名一致）
    public delegate MyStruct GetMyStruct();
    public delegate IntPtr GetVersion();
    public delegate void FreeMyStructDelegate(ref MyStruct obj);
    public delegate void FreeStringDelegate(IntPtr str);
    public delegate int GetID();

    public void DoCappDll()
    {
        string dllPath = "";

#if UNITY_ANDROID && !UNITY_EDITOR
        dllPath = PathHelper.GetAssetBackupLOCPTH("Assets/GameBackup/Cpp Dll/CMake/Android/libs/armeabi-v7a/libmylib.so");
#else       
        dllPath = PathHelper.GetAssetBackupSAPTH("Assets/GameBackup/Cpp Dll/CMake/x86_64/mylib.dll.bytes");
#endif

        //Assembly.LoadFile(dllPath); // 这个是C++的dll 不是C#的dll !!


        Debug.Log($"==【Test Dll】 dllPath = {dllPath}");

        IntPtr _dllHandle = CppDllHotLoader.DoLoadLibrary(dllPath);

        try
        {
            Debug.Log($"==【Test Dll】  IntPtr = {_dllHandle}");

            // 获取函数地址
            IntPtr _ptrGetMyStruct = CppDllHotLoader.DoGetFunctionPointer(_dllHandle, "GetMyStruct");
            IntPtr _ptrGetVersion = CppDllHotLoader.DoGetFunctionPointer(_dllHandle, "GetVersion");
            IntPtr _ptrFreeMyStruct = CppDllHotLoader.DoGetFunctionPointer(_dllHandle, "FreeMyStruct");
            IntPtr _ptrFreeVersion = CppDllHotLoader.DoGetFunctionPointer(_dllHandle, "FreeVersion");
            IntPtr _ptrGetID = CppDllHotLoader.DoGetFunctionPointer(_dllHandle, "GetID");

            // 转换为委托
            GetMyStruct funcGetMyStruct = Marshal.GetDelegateForFunctionPointer<GetMyStruct>(_ptrGetMyStruct);
            GetVersion funcGetVersion = Marshal.GetDelegateForFunctionPointer<GetVersion>(_ptrGetVersion);
            var funcFreeMyStruct = Marshal.GetDelegateForFunctionPointer<FreeMyStructDelegate>(_ptrFreeMyStruct);
            var funcFreeVersion = Marshal.GetDelegateForFunctionPointer<FreeStringDelegate>(_ptrFreeVersion);
            GetID funcGetID = Marshal.GetDelegateForFunctionPointer<GetID>(_ptrGetID);


            int id = funcGetID();
            Debug.Log($"@@@【Test Dll】  GetID  == {id}");


            IntPtr versionPtr = funcGetVersion();
            string ver = Marshal.PtrToStringAnsi(versionPtr); // 从指针，获取字符串

            MyStruct myStruct = funcGetMyStruct();
            string name = Marshal.PtrToStringAnsi(myStruct.name);  //  从指针，获取字符串


            Debug.Log($"@@@【Test Dll】  getVersion  == {ver}");
            //Debug.LogError($"@@@ GetMyStruct  == {JsonConvert.SerializeObject(funcGetMyStruct())}");
            Debug.Log($"@@@ 【Test Dll】 GetMyStruct  == {JsonConvert.SerializeObject(myStruct)}   name: {name}");


            // 【MyStruct】结构体不需要释放，但是结构体里面有指针（如：字符串）就需要释放。
            funcFreeMyStruct(ref myStruct);// 必须释放！
            funcFreeVersion(versionPtr); // 必须释放！
        }
        finally
        {
            CppDllHotLoader.DoFreeLibrary(_dllHandle);
        }


    }
}
