using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System;
public class OpenStreamingAssetsButton
{

#if UNITY_EDITOR

    [MenuItem("NewBuild/通过.bat打开StreamingAssets文件夹")]
    private static void RunBatToOpenStreamingAssetsFolder()
    {

        string batFilePath = System.IO.Path.Combine(Application.dataPath, "open_streaming_assets.bat");

        //UnityEngine.Debug.Log(batFilePath1);
        try
        {
            Process.Start(batFilePath);
        }
        catch (System.Exception ex)
        {
            UnityEngine.Debug.LogError($"运行.bat 文件时出错: {ex.Message}");
        }
    }
#endif

}