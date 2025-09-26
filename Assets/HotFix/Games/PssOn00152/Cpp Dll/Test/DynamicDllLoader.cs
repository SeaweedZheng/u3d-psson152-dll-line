using UnityEngine;
using System;
using System.Runtime.InteropServices;

/// <summary>
/// ���û�в���ͨ��
/// </summary>
public class DynamicDllLoader : MonoBehaviour
{
    // ����ί�����ͣ��� C++ ����ǩ��һ�£�
    public delegate int AddDelegate(int a, int b);
    public delegate void PrintMessageDelegate(string message);

    private IntPtr _dllHandle;

    void Start()
    {
        // ��̬���� DLL
        string dllPath = System.IO.Path.Combine(Application.streamingAssetsPath, "MyCppLibrary.dll");
        _dllHandle = LoadLibrary(dllPath);
        if (_dllHandle == IntPtr.Zero)
        {
            Debug.LogError("Failed to load DLL: " + dllPath);
            return;
        }

        // ��ȡ������ַ
        IntPtr addAddr = GetProcAddress(_dllHandle, "Add");
        IntPtr printAddr = GetProcAddress(_dllHandle, "PrintMessage");

        // ת��Ϊί��
        AddDelegate add = Marshal.GetDelegateForFunctionPointer<AddDelegate>(addAddr);
        PrintMessageDelegate print = Marshal.GetDelegateForFunctionPointer<PrintMessageDelegate>(printAddr);

        // ���ú���
        int result = add(5, 3);
        Debug.Log("Result from C++: " + result);
        print("Hello from C++!");

        // ж�� DLL����ѡ��ͨ������Ϸ�˳�ʱ����
        // FreeLibrary(_dllHandle);
    }

    // ���� Windows API
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr LoadLibrary(string lpFileName);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool FreeLibrary(IntPtr hModule);
}