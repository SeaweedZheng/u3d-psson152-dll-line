using UnityEditor;
using UnityEngine;
using System.Diagnostics;
using System.IO;

public class LuBanEditor
{
    static string folderPath = Application.dataPath;

    
    [MenuItem("LuBan/生成配置表")]
    private static void RunBatchScript()
    {
        string batchFilePath = Path.GetDirectoryName(folderPath);
        // 指定.bat文件的路径
        //batchFilePath = $"{batchFilePath}\\gen.bat";
        batchFilePath = $"{batchFilePath}\\gen_temp_02.bat";


        string disk = batchFilePath.Split(':')[0];
        string Arguments = $"/{disk} \"" + batchFilePath + "\"";
        UnityEngine.Debug.Log($"batchFilePath = {batchFilePath} || {Arguments}");

        // 检查文件是否存在
        if (System.IO.File.Exists(batchFilePath))
        {

            // 启动.bat文件
            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    //FileName ="C:\\Windows\\system32\\cmd.exe",
                    //FileName = "C:/Windows/system32/cmd.exe",
                    //Arguments = "/C \"" + batchFilePath + "\"",
                    Arguments = Arguments,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                }
            };
            process.Start();

            // 等待进程结束
            process.WaitForExit();
            //UnityEngine.Debug.Log($"进程结束");
            AssetDatabase.Refresh();
        }
        else
        {
            UnityEngine.Debug.LogError("Batch file not found at: " + batchFilePath);
        }
    }


    [MenuItem("LuBan/打开配置表文件夹")]
    public static void OpenFolder()
    {
        string projectPaht = Path.GetDirectoryName(folderPath);
        projectPaht = $"{projectPaht}\\LuBan\\DataTables\\Datas";
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.FileName = "explorer"; // 使用Windows的文件资源管理器
        startInfo.Arguments = projectPaht; // 传递文件夹路径作为参数

        try
        {
            Process.Start(startInfo.FileName, startInfo.Arguments);
        }
        catch (System.ComponentModel.Win32Exception)
        {
            // 处理错误
            UnityEngine.Debug.LogError("无法打开指定的文件夹");
        }
    }


}