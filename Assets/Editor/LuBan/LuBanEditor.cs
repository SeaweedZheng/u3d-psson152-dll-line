using UnityEditor;
using UnityEngine;
using System.Diagnostics;
using System.IO;

public class LuBanEditor
{
    static string folderPath = Application.dataPath;

    
    [MenuItem("LuBan/�������ñ�")]
    private static void RunBatchScript()
    {
        string batchFilePath = Path.GetDirectoryName(folderPath);
        // ָ��.bat�ļ���·��
        //batchFilePath = $"{batchFilePath}\\gen.bat";
        batchFilePath = $"{batchFilePath}\\gen_temp_02.bat";


        string disk = batchFilePath.Split(':')[0];
        string Arguments = $"/{disk} \"" + batchFilePath + "\"";
        UnityEngine.Debug.Log($"batchFilePath = {batchFilePath} || {Arguments}");

        // ����ļ��Ƿ����
        if (System.IO.File.Exists(batchFilePath))
        {

            // ����.bat�ļ�
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

            // �ȴ����̽���
            process.WaitForExit();
            //UnityEngine.Debug.Log($"���̽���");
            AssetDatabase.Refresh();
        }
        else
        {
            UnityEngine.Debug.LogError("Batch file not found at: " + batchFilePath);
        }
    }


    [MenuItem("LuBan/�����ñ��ļ���")]
    public static void OpenFolder()
    {
        string projectPaht = Path.GetDirectoryName(folderPath);
        projectPaht = $"{projectPaht}\\LuBan\\DataTables\\Datas";
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.FileName = "explorer"; // ʹ��Windows���ļ���Դ������
        startInfo.Arguments = projectPaht; // �����ļ���·����Ϊ����

        try
        {
            Process.Start(startInfo.FileName, startInfo.Arguments);
        }
        catch (System.ComponentModel.Win32Exception)
        {
            // �������
            UnityEngine.Debug.LogError("�޷���ָ�����ļ���");
        }
    }


}