using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;



public static partial class FileUtils 
{
    public static void WriteFile(string path, byte[] data)
    {
        FileInfo fi = new FileInfo(path);
        DirectoryInfo dir = fi.Directory;
        if (!dir.Exists)
        {
            dir.Create();
        }
        FileStream fs = fi.Create();
        fs.Write(data, 0, data.Length);
        fs.Flush();
        fs.Close();
    }



    public static string ComputeMD5ForStr(string rawData)
    {
        // ����һ��MD5ʵ��
        using (MD5 md5 = MD5.Create())
        {
            // �������ַ���ת��Ϊ�ֽ�����
            byte[] inputBytes = Encoding.ASCII.GetBytes(rawData);

            // �����ϣֵ
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // ����ϣֵת��Ϊʮ�������ַ���
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
            }

            // ����ʮ�������ַ���
            return sb.ToString();
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    /// <remarks>
    /// * �����ҪƵ�����ø÷������ɿ���ʹ�� StringBuilder ������ʮ�������ַ���������Ƶ�����ַ���ƴ�Ӳ������Ӷ�������ܡ�
    /// </remarks>
    public static string CalculateMD5_01(byte[] data)
    {

        try
        {
            // ����MD5ʵ��
            using (MD5 md5 = MD5.Create())
            {
                // ����MD5��ϣֵ
                byte[] hashBytes = md5.ComputeHash(data);

                // ���ֽ�����ת��Ϊʮ�������ַ���
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"����MD5��ʱ��������: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    /// <remarks>
    /// * CalculateMD5 �� CalculateMD5_01 ������һ��
    /// * CalculateMD5_01 �ȽϺ����ܡ�
    /// </remarks>
    public static string CalculateMD5(byte[] data)
    {
        try
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(data);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"����MD5��ʱ��������: {ex.Message}");
            return null;
        }
    }


    public static string CalculateFileMD5(string filePath)
    {
        try
        {
            // ����ļ��Ƿ����
            if (!File.Exists(filePath))
            {
                Debug.LogError($"�ļ� {filePath} �����ڣ�");
                return null;
            }

            // ��ȡ�ļ�����Ϊ�ֽ�����
            byte[] fileBytes = File.ReadAllBytes(filePath);

            // ����MD5ʵ��
            using (MD5 md5 = MD5.Create())
            {
                // ����MD5��ϣֵ
                byte[] hashBytes = md5.ComputeHash(fileBytes);

                // ���ֽ�����ת��Ϊʮ�������ַ���
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"����MD5��ʱ��������: {ex.Message}");
            return null;
        }
    }

    static int chunkSize = 1024 * 1024; // 1MB ���С


    /// <summary>
    /// ���ļ� - �ֿ�У��
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static string CalculateChunkedMD5(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                Debug.LogError($"�ļ� {filePath} �����ڣ�");
                return null;
            }

            using (FileStream fileStream = File.OpenRead(filePath))
            using (MD5 md5 = MD5.Create())
            {
                byte[] buffer = new byte[chunkSize];
                int bytesRead;
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    byte[] chunk = new byte[bytesRead];
                    Array.Copy(buffer, chunk, bytesRead);
                    md5.TransformBlock(chunk, 0, chunk.Length, null, 0);
                }
                md5.TransformFinalBlock(new byte[0], 0, 0);
                byte[] hashBytes = md5.Hash;
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"����ֿ� MD5 ��ϣֵʱ��������: {ex.Message}");
            return null;
        }
    }

    public static uint CalculateFileCRC32(string filePath) => CRC32Calculator.CalculateFileCRC32(filePath);
}





public class CRC32Calculator
{
    // CRC32 ����ʽ����׼�� CRC32 ����ʽֵ
    private const uint Polynomial = 0xEDB88320;
    // CRC �����ڴ洢Ԥ�ȼ���õ� CRC ֵ
    private static readonly uint[] CrcTable = new uint[256];

    static CRC32Calculator()
    {
        // ��ʼ�� CRC ��
        for (uint i = 0; i < 256; i++)
        {
            uint crc = i;
            for (int j = 8; j > 0; j--)
            {
                if ((crc & 1) == 1)
                    crc = (crc >> 1) ^ Polynomial;
                else
                    crc >>= 1;
            }
            CrcTable[i] = crc;
        }
    }

    /// <summary>
    /// �����ļ��� CRC32 У��ֵ
    /// </summary>
    /// <param name="filePath">�ļ���·��</param>
    /// <returns>�ļ��� CRC32 У��ֵ</returns>
    /// <remark>
    /// * �����CRC32�㷨����u3d��AB��CRC�㷨����ǲ�һ���ģ�
    /// </remark>
    public static uint CalculateFileCRC32(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                Debug.LogError($"�ļ� {filePath} �����ڣ�");
                return 0;
            }

            uint crcValue = 0xFFFFFFFF;
            using (FileStream fileStream = File.OpenRead(filePath))
            {
                int byteRead;
                while ((byteRead = fileStream.ReadByte()) != -1)
                {
                    crcValue = (crcValue >> 8) ^ CrcTable[(crcValue ^ (byte)byteRead) & 0xFF];
                }
            }
            return crcValue ^ 0xFFFFFFFF;
        }
        catch (Exception ex)
        {
            Debug.LogError($"���� CRC32 У��ֵʱ��������: {ex.Message}");
            return 0;
        }
    }


    /// <summary>
    /// תΪ8λ��16�����ַ���
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static string CalculateFileCRC32Str(string filePath)
    {

        uint crc32 = CalculateFileCRC32(filePath);

        /*
        uint crc1 = 3911944193;
        uint crc2 = 470612471;

        // ת��Ϊʮ�������ַ���
        string hexCRC1 = crc1.ToString("X8");
        string hexCRC2 = crc2.ToString("X8");

        Debug.Log($"ʮ���� CRC1: {crc1}, ʮ������ CRC1: {hexCRC1}");
        Debug.Log($"ʮ���� CRC2: {crc2}, ʮ������ CRC2: {hexCRC2}");

        Debug.Log($"�ļ� {filePath} �� CRC32 У��ֵ��: {crc32:X8} -- {crc32}");
        // ʹ�� string.Format ����
        string result2 = string.Format("CRC32 ֵ��: {0:X8}", crc32);
        Debug.Log(result2);

        */

        /*
���� crc32 ��ֵΪ 0x12345678����ôʹ�� {crc32:X8} ��ʽ�����������Ϊ "12345678"����� crc32 ��ֵΪ 0x12����ʽ�����������Ϊ "00000012"����֤�������ʮ�������ַ���ʼ���� 8 λ���ȡ�
����������{crc32:X8} �������ǽ� crc32 ����޷�����������ת��Ϊ 8 λ���ȵĴ�дʮ�������ַ������������
        */

        return crc32.ToString("X8");  //ʮ������
    }


}



public static partial class FileUtils
{
    /*
    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    /// <remarks>
    /// </remarks>
    public static string CalculateMD5(byte[] data)
    {
        try
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(data);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"����MD5��ʱ��������: {ex.Message}");
            return null;
        }
    }

    public static string CalculateFileMD5(string filePath)
    {
        try
        {
            // ����ļ��Ƿ����
            if (!File.Exists(filePath))
            {
                Debug.LogError($"�ļ� {filePath} �����ڣ�");
                return null;
            }

            // ��ȡ�ļ�����Ϊ�ֽ�����
            byte[] fileBytes = File.ReadAllBytes(filePath);

            // ����MD5ʵ��
            return CalculateMD5(fileBytes);
        }
        catch (Exception ex)
        {
            Debug.LogError($"����MD5��ʱ��������: {ex.Message}");
            return null;
        }
    }*/




    /// <summary>
    /// ��ȡ��·�ļ�·��
    /// </summary>
    /// <param name="baseUriStr">�ο�·��</param>
    /// <param name="relativeUriStr">���·�������Դ������·����</param>
    /// <returns></returns>
    public static string GetFileWebUrl(string baseUriStr, string relativeUriStr)
    {
        string targetFileUrl = relativeUriStr;

        if (!targetFileUrl.StartsWith("http://") && !targetFileUrl.StartsWith("https://"))
        {
            Uri baseUri = new Uri(baseUriStr);
            Uri combinedUri = new Uri(baseUri, relativeUriStr);
            targetFileUrl = combinedUri.ToString();

            /*
            string[] paths = { 
                "/PssOn00152/debug/android/1", // http://8.138.140.180:8124/PssOn00152/debug/android/1
                "PssOn00152/debug/android/1", // http://8.138.140.180:8124/a/b/c/PssOn00152/debug/android/1
               "../PssOn00152/debug/android/1",//  http://8.138.140.180:8124/a/b/PssOn00152/debug/android/1
                "./PssOn00152/debug/android/1",//  http://8.138.140.180:8124/a/b/c/PssOn00152/debug/android/1
               "../../../PssOn00152/debug/android/1" // http://8.138.140.180:8124/PssOn00152/debug/android/1
            };                        
            foreach (string path in paths)
            {
                Uri beUri = new Uri("http://8.138.140.180:8124/a/b/c/");
                Uri curl = new Uri(beUri, path);
                Debug.Log($"�ȸ�·���� {curl.ToString()}");
            }*/

            /*
            //totalVersionUrl = http://8.138.140.180:8124/PssOn00152/total_version.json
            string[] paths = {
                "/PssOn00152/debug/android/1", // http://8.138.140.180:8124/PssOn00152/debug/android/1
                "PssOn00152/debug/android/1", // http://8.138.140.180:8124/PssOn00152/PssOn00152/debug/android/1
               "./PssOn00152/debug/android/1",//  http://8.138.140.180:8124/PssOn00152/PssOn00152/debug/android/1
               "../PssOn00152/debug/android/1" //  http://8.138.140.180:8124/PssOn00152/debug/android/1
            };
            foreach (string path in paths)
            {
                Uri beUri = new Uri(totalVersionUrl);
                Uri curl = new Uri(beUri, path);
                Debug.Log($"�ȸ�·���� {curl.ToString()}");
            }*/
        }

        return targetFileUrl;
    }


    /// <summary>
    /// ��ȡ��·Ŀ¼·��
    /// </summary>
    /// <param name="baseUriStr">�ο�·��</param>
    /// <param name="relativeUriStr">���·�������Դ������·����</param>
    /// <returns></returns>
    public static string GetDirWebUrl(string baseUriStr, string relativeUriStr)
    {
        string targetDirUrl = relativeUriStr;

        if (!targetDirUrl.StartsWith("http://") && !targetDirUrl.StartsWith("https://"))
        {
            Uri baseUri = new Uri(baseUriStr);
            Uri combinedUri = new Uri(baseUri, relativeUriStr);
            targetDirUrl = combinedUri.ToString();
        }

        if (!targetDirUrl.EndsWith("/"))
            targetDirUrl += "/";
        return targetDirUrl;
    }






    /// <summary>
    /// ����yStreamingAsset�����Դ������
    /// </summary>
    /// <param name="srcPath"></param>
    /// <param name="tarPath"></param>
    /// <returns></returns>
    public static IEnumerator CopyStreamingAssetToLocal(string srcPath, string tarPath)
    {

#if UNITY_ANDROID
        using (UnityWebRequest reqSAAsset = UnityWebRequest.Get(srcPath))
        {
            yield return reqSAAsset.SendWebRequest();

            if (reqSAAsset.result == UnityWebRequest.Result.Success)
            {
                WriteAllBytes(tarPath, reqSAAsset.downloadHandler.data);
                yield return new WaitForEndOfFrame();
            }
            else
            {
                Debug.LogError($"Copy File Fail: {srcPath} error: {reqSAAsset.error}");
                yield break;
            }
        }
#else
        byte[] bytes = File.ReadAllBytes(srcPath);
        WriteAllBytes(tarPath, bytes);
        yield return new WaitForEndOfFrame();
#endif
    }


    public static IEnumerator ReadStreamingAsset<T>(string srcPath , Action<object> onSuccessCallback, Action<string> onErrorCallback)
    {
#if UNITY_ANDROID
        using (UnityWebRequest reqSAAsset = UnityWebRequest.Get(srcPath))
        {
            yield return reqSAAsset.SendWebRequest();

            if (reqSAAsset.result == UnityWebRequest.Result.Success)
            {
                Type type = typeof(T);

                if (type == typeof(string))
                {
                    onSuccessCallback?.Invoke(reqSAAsset.downloadHandler.text);
                }
                else if (type == typeof(byte[]))
                {
                    onSuccessCallback?.Invoke(reqSAAsset.downloadHandler.data);
                }
                else
                {
                    Debug.LogError("T must string or byte[]");
                }
            }
            else
            {
                Debug.LogError($"Copy File Fail: {srcPath} error: {reqSAAsset.error}");
                onErrorCallback?.Invoke(reqSAAsset.error);
            }
        }
#else
        byte[] bytes = File.ReadAllBytes(srcPath);
        onSuccessCallback?.Invoke(bytes);
        yield return new WaitForEndOfFrame();
#endif
    }



    public static void ReadStreamingAssetSync<T>(string srcPath, System.Action<object> onSuccessCallback, System.Action<string> onErrorCallback)
    {
#if UNITY_ANDROID
        using (UnityWebRequest reqSAAsset = UnityWebRequest.Get(srcPath))
        {
            reqSAAsset.SendWebRequest();
            while (!reqSAAsset.isDone)
            {
                // �ȴ��������
            }

            if (reqSAAsset.result == UnityWebRequest.Result.Success)
            {
                System.Type type = typeof(T);

                if (type == typeof(string))
                {
                    onSuccessCallback?.Invoke(reqSAAsset.downloadHandler.text);
                }
                else if (type == typeof(byte[]))
                {
                    onSuccessCallback?.Invoke(reqSAAsset.downloadHandler.data);
                }
                else
                {
                    Debug.LogError("T must string or byte[]");
                }
            }
            else
            {
                Debug.LogError($"Copy File Fail: {srcPath} error: {reqSAAsset.error}");
                onErrorCallback?.Invoke(reqSAAsset.error);
            }
        }
#else
        try
        {
            byte[] bytes = File.ReadAllBytes(srcPath);
            System.Type type = typeof(T);

            if (type == typeof(string))
            {
                onSuccessCallback?.Invoke(System.Text.Encoding.UTF8.GetString(bytes));
            }
            else if (type == typeof(byte[]))
            {
                onSuccessCallback?.Invoke(bytes);
            }
            else
            {
                Debug.LogError("T must string or byte[]");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Copy File Fail: {srcPath} error: {ex.Message}");
            onErrorCallback?.Invoke(ex.Message);
        }
#endif
    }

    public static void WriteAllBytes(string path, byte[] bytes)
    {
        string directory = Path.GetDirectoryName(path);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        File.WriteAllBytes(path, bytes);
    }


    public static void WriteAllText(string path, string contents)
    {
        string directory = Path.GetDirectoryName(path);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        File.WriteAllText(path, contents);
    }

    /// <summary>
    /// ɾ��Ŀ¼�µ��ļ��Լ��ļ���
    /// </summary>
    /// <param name="directory"></param>
    /// <returns></returns>
    public static IEnumerator DeleteDirectoryAsync(string directory)
    {
        // ɾ�������ļ�
        string[] files = Directory.GetFiles(directory);
        foreach (string file in files)
        {
            try
            {
                File.Delete(file);
            }
            catch (Exception e)
            {
                Debug.LogError($"1 Failed to delete file {file}: {e.Message}");
            }
            yield return null;
        }

        // ɾ���������ļ���
        string[] directories = Directory.GetDirectories(directory);
        foreach (string dir in directories)
        {
            yield return DeleteDirectoryAsync(dir);
           /* try
            {
                Directory.Delete(dir);
            }
            catch (Exception e)
            {
                Debug.LogError($"2 Failed to delete directory {dir}: {e.Message}");
            }
            yield return null;
           */
        }

        // ɾ����ǰĿ¼
        try
        {
            Directory.Delete(directory);
        }
        catch (Exception e)
        {
            Debug.LogError($"3 Failed to delete directory {directory}: {e.Message}");
        }
    }



    /// <summary>
    /// ��������Ŀ¼�µ��ļ����ļ��е�Ŀ��Ŀ¼��
    /// </summary>
    /// <param name="sourceDir"></param>
    /// <param name="destDir"></param>
    /// <returns></returns>
    public static IEnumerator CopyDirectoryAsync(string sourceDir, string destDir)
    {
        if (!Directory.Exists(sourceDir))
        {
            Debug.LogError($"Source directory {sourceDir} does not exist.");
            yield break;
        }

        if (!Directory.Exists(destDir))
        {
            Directory.CreateDirectory(destDir);
        }

        string[] files = Directory.GetFiles(sourceDir);
        foreach (string file in files)
        {
            Debug.Log($"copy temp hotfix file: {file}");
            yield return CopyFileAsync(file, Path.Combine(destDir, Path.GetFileName(file)));
        }

        string[] directories = Directory.GetDirectories(sourceDir);
        foreach (string dir in directories)
        {
            string dirName = Path.GetFileName(dir);
            string destSubDir = Path.Combine(destDir, dirName);
            yield return CopyDirectoryAsync(dir, destSubDir);
        }
    }


    /// <summary>
    /// ���������ļ�
    /// </summary>
    /// <param name="sourceFile"></param>
    /// <param name="destFile"></param>
    /// <returns></returns>
    public static IEnumerator CopyFileAsync(string sourceFile, string destFile)
    {
        // ���Ŀ���ļ��Ƿ���ڣ����������ɾ��
        if (File.Exists(destFile))
        {
            try
            {
                File.Delete(destFile);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to delete file {destFile}: {e.Message}");
                yield break;
            }
        }

        /*
        using (FileStream sourceStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read))
        using (FileStream destStream = new FileStream(destFile, FileMode.Create, FileAccess.Write))
        {
            //byte[] buffer = new byte[4096]; //4KB
            byte[] buffer = new byte[65536]; // 64KB
            int bytesRead;
            while ((bytesRead = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                destStream.Write(buffer, 0, bytesRead);
                yield return null;
            }
        }*/


        // ��ȡ�ļ���С
        long fileSize = new FileInfo(sourceFile).Length;
        int bufferSize = (int)Math.Min(fileSize, 65536); // 64KB

        // ʹ���첽�ļ��������ļ�����
        using (FileStream sourceStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, FileOptions.Asynchronous))
        using (FileStream destStream = new FileStream(destFile, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize, FileOptions.Asynchronous))
        {
            //byte[] buffer = new byte[4096]; //4KB
            byte[] buffer = new byte[bufferSize];
            int bytesRead;
            while ((bytesRead = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                destStream.Write(buffer, 0, bytesRead);
                yield return null;
            }
        }

        /*
        System.Threading.Tasks.Task copyTask = null;
        // ��ȡ�ļ���С
        long fileSize = new FileInfo(sourceFile).Length;
        int bufferSize = (int)Math.Min(fileSize, 65536);

        // ʹ���첽�ļ��������ļ�����
        using (FileStream sourceStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, FileOptions.Asynchronous))
        using (FileStream destStream = new FileStream(destFile, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize, FileOptions.Asynchronous))
        {
            copyTask = sourceStream.CopyToAsync(destStream);
            while (!copyTask.IsCompleted)
            {
                yield return null;
            }
        }
        if (copyTask.IsFaulted)
        {
            Debug.LogError($"Failed to copy file: {copyTask.Exception.Message}");
        }
        */
    }
}