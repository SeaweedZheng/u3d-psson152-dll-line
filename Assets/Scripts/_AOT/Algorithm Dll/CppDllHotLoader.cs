using System;
using System.Runtime.InteropServices;


public static  class CppDllHotLoader   //NativeLibraryLoader
{
    // Windows平台使用kernel32
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
    [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
    static extern IntPtr LoadLibrary(string path);

    [DllImport("kernel32", SetLastError = true)]
    static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

    [DllImport("kernel32", SetLastError = true)]
    static extern bool FreeLibrary(IntPtr hModule);

    // Android/Linux平台使用libdl
#elif UNITY_ANDROID || UNITY_STANDALONE_LINUX
    [DllImport("libdl")]
    static extern IntPtr dlopen(string path, int flags);
 
    [DllImport("libdl")]
    static extern IntPtr dlsym(IntPtr handle, string symbol);
 
    [DllImport("libdl")]
    static extern int dlclose(IntPtr handle);
 
    // 定义dlopen的flags（RTLD_NOW=0x2）
    private const int RTLD_NOW = 0x2;
#endif


    /// <summary>
    /// 跨平台加载动态库（DLL/SO）
    /// </summary>
    public static IntPtr DoLoadLibrary(string path)
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        return LoadLibrary(path);
#elif UNITY_ANDROID || UNITY_STANDALONE_LINUX
        return dlopen(path, RTLD_NOW);
#else
        Debug.LogError("Unsupported platform for dynamic library loading.");
        return IntPtr.Zero;
#endif
    }

    /// <summary>
    /// 跨平台获取函数指针
    /// </summary>
    public static IntPtr DoGetFunctionPointer(IntPtr libraryHandle, string functionName)
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        return GetProcAddress(libraryHandle, functionName);
#elif UNITY_ANDROID || UNITY_STANDALONE_LINUX
        return dlsym(libraryHandle, functionName);
#else
        return IntPtr.Zero;
#endif
    }

    /// <summary>
    /// 跨平台卸载动态库
    /// </summary>
    public static bool DoFreeLibrary(IntPtr libraryHandle)
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        return FreeLibrary(libraryHandle);
#elif UNITY_ANDROID || UNITY_STANDALONE_LINUX
        return dlclose(libraryHandle) == 0;
#else
        return false;
#endif
    }
}
