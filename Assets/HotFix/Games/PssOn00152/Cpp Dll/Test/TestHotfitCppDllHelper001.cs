using Newtonsoft.Json;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class TestHotfitCppDllHelper001 : MonoSingleton<TestHotfitCppDllHelper001>
{


    // ������C++�ṹ��ƥ���C#�ṹ��
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct MyStruct
    {
        public int id;
        public float value;

        //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        //public string name;

        public IntPtr name; // ������ IntPtr�������� PtrToStringAnsi
    }


    // ����ί�����ͣ��� C++ ����ǩ��һ�£�
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

        //Assembly.LoadFile(dllPath); // �����C++��dll ����C#��dll !!


        Debug.Log($"==��Test Dll�� dllPath = {dllPath}");

        IntPtr _dllHandle = CppDllHotLoader.DoLoadLibrary(dllPath);

        try
        {
            Debug.Log($"==��Test Dll��  IntPtr = {_dllHandle}");

            // ��ȡ������ַ
            IntPtr _ptrGetMyStruct = CppDllHotLoader.DoGetFunctionPointer(_dllHandle, "GetMyStruct");
            IntPtr _ptrGetVersion = CppDllHotLoader.DoGetFunctionPointer(_dllHandle, "GetVersion");
            IntPtr _ptrFreeMyStruct = CppDllHotLoader.DoGetFunctionPointer(_dllHandle, "FreeMyStruct");
            IntPtr _ptrFreeVersion = CppDllHotLoader.DoGetFunctionPointer(_dllHandle, "FreeVersion");
            IntPtr _ptrGetID = CppDllHotLoader.DoGetFunctionPointer(_dllHandle, "GetID");

            // ת��Ϊί��
            GetMyStruct funcGetMyStruct = Marshal.GetDelegateForFunctionPointer<GetMyStruct>(_ptrGetMyStruct);
            GetVersion funcGetVersion = Marshal.GetDelegateForFunctionPointer<GetVersion>(_ptrGetVersion);
            var funcFreeMyStruct = Marshal.GetDelegateForFunctionPointer<FreeMyStructDelegate>(_ptrFreeMyStruct);
            var funcFreeVersion = Marshal.GetDelegateForFunctionPointer<FreeStringDelegate>(_ptrFreeVersion);
            GetID funcGetID = Marshal.GetDelegateForFunctionPointer<GetID>(_ptrGetID);


            int id = funcGetID();
            Debug.Log($"@@@��Test Dll��  GetID  == {id}");


            IntPtr versionPtr = funcGetVersion();
            string ver = Marshal.PtrToStringAnsi(versionPtr); // ��ָ�룬��ȡ�ַ���

            MyStruct myStruct = funcGetMyStruct();
            string name = Marshal.PtrToStringAnsi(myStruct.name);  //  ��ָ�룬��ȡ�ַ���


            Debug.Log($"@@@��Test Dll��  getVersion  == {ver}");
            //Debug.LogError($"@@@ GetMyStruct  == {JsonConvert.SerializeObject(funcGetMyStruct())}");
            Debug.Log($"@@@ ��Test Dll�� GetMyStruct  == {JsonConvert.SerializeObject(myStruct)}   name: {name}");


            // ��MyStruct���ṹ�岻��Ҫ�ͷţ����ǽṹ��������ָ�루�磺�ַ���������Ҫ�ͷš�
            funcFreeMyStruct(ref myStruct);// �����ͷţ�
            funcFreeVersion(versionPtr); // �����ͷţ�
        }
        finally
        {
            CppDllHotLoader.DoFreeLibrary(_dllHandle);
        }


    }
}
