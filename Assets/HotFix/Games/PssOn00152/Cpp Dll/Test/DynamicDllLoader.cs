using UnityEngine;
using System;
using System.Runtime.InteropServices;

/// <summary>
/// 这个没有测试通过
/// </summary>
public class DynamicDllLoader : MonoBehaviour
{
    // 定义委托类型（与 C++ 函数签名一致）
    public delegate int AddDelegate(int a, int b);
    public delegate void PrintMessageDelegate(string message);

    private IntPtr _dllHandle;

    void Start()
    {
        // 动态加载 DLL
        string dllPath = System.IO.Path.Combine(Application.streamingAssetsPath, "MyCppLibrary.dll");
        _dllHandle = LoadLibrary(dllPath);
        if (_dllHandle == IntPtr.Zero)
        {
            Debug.LogError("Failed to load DLL: " + dllPath);
            return;
        }

        // 获取函数地址
        IntPtr addAddr = GetProcAddress(_dllHandle, "Add");
        IntPtr printAddr = GetProcAddress(_dllHandle, "PrintMessage");

        // 转换为委托
        AddDelegate add = Marshal.GetDelegateForFunctionPointer<AddDelegate>(addAddr);
        PrintMessageDelegate print = Marshal.GetDelegateForFunctionPointer<PrintMessageDelegate>(printAddr);

        // 调用函数
        int result = add(5, 3);
        Debug.Log("Result from C++: " + result);
        print("Hello from C++!");

        // 卸载 DLL（可选，通常在游戏退出时处理）
        // FreeLibrary(_dllHandle);
    }

    // 导入 Windows API
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr LoadLibrary(string lpFileName);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool FreeLibrary(IntPtr hModule);
}