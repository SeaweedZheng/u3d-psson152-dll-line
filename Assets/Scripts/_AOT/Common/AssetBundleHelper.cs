using System;
using System.Collections;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

/// <summary>
/// ������ű����á�
/// </summary>
/// <remarks>
/// # �������<br/>
/// * ��ʹ��AssetBundle������Դʱ�����ʹ��ͬ�����صķ�ʽ����ʹ��Ϸ�ڼ�����Դ�Ĺ����г��ֿ���������Ϊͬ�����ػ��������̵߳�ִ�У�ֱ����Դ������ɲ��ܼ���ִ�к������߼���<br/>
/// * ���첽����������ں�̨������Դ���أ������������̵߳�ִ�У��������ͬʱ���ض����Դ��Ҳ�ᵼ����Ϸ��֡��Ӱ����Ϸ�������ԡ�<br/>
/// </remarks>
public class AssetBundleHelper  
{
    MonoBehaviour mon;
    public AssetBundleHelper(MonoBehaviour mon)
    {
        this.mon = mon;
    }

    public void LoadAssetBundleAsync(string url, Action<AssetBundle> OnfinishCallback)=>
        mon.StartCoroutine(LoadAssetBundleAsyncIE(url, OnfinishCallback));
    

    /// <summary>
    /// ����1�����AssetBundle�첽���صĿ��ٺ͵�֡����:
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    /// <remarks>
    /// # �������<br/>
    /// * Unity3D�ṩ���첽����AssetBundle�Ľӿڣ����ǿ���������Щ�ӿ���ʵ����Դ���첽���ء�
    /// * �ڼ�����Դʱ������ʹ��UnityWebRequest�������������󣬲�ͨ��AssetBundle.LoadFromMemoryAsync���������ص���Դ���ص��ڴ��С�
    /// * ͨ�����ַ�ʽ�������ں�̨������Դ�ļ��أ������������̵߳�ִ�У��Ӷ����⿨������ķ�����
    /// </remarks>
    public IEnumerator LoadAssetBundleAsyncIE(string url, Action<AssetBundle> OnfinishCallback)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        AssetBundleCreateRequest assetBundleRequest = AssetBundle.LoadFromMemoryAsync(request.downloadHandler.data);
        yield return assetBundleRequest;
        AssetBundle assetBundle = assetBundleRequest.assetBundle;
        // ��Դ������ɺ���߼�����
        OnfinishCallback?.Invoke(assetBundle);
    }


    class AssetBundleTask
    {
        public Thread thread;
        public AssetBundle assetBundle;
        public ManualResetEvent loadCompleteEvent = new ManualResetEvent(false);

        public void LoadAssetBundleThread(object url)
        {
            UnityWebRequest request = UnityWebRequest.Get((string)url);
            request.SendWebRequest();
            while (!request.isDone)
            {
                Thread.Sleep(100);
            }
            assetBundle = AssetBundle.LoadFromMemory(request.downloadHandler.data);
            loadCompleteEvent.Set();
        }
    }

    public void LoadLoadAssetBundleAsync2(string url, Action<AssetBundle> OnfinishCallback) 
        => mon.StartCoroutine(LoadAssetBundleAsyncIE2(url, OnfinishCallback));


    /// <summary>
    /// ����2��ʹ�ö��̼߳�����Դ
    /// </summary>
    /// <param name="url"></param>
    /// <param name="OnfinishCallback"></param>
    /// <returns></returns>
    /// <remarks>
    /// # �������<br/>
    /// * ����ʹ��Unity3D�ṩ���첽���ؽӿ��⣬���ǻ�����ͨ�����߳���������Դ���Ӷ���һ�������Դ���ص�Ч�ʡ�
    /// �ڶ��̼߳�����Դʱ�����Խ���Դ�ļ��غͽ�ѹ���Ȳ������ں�̨�߳��н��У�����Ӱ�����̵߳�ִ�С��ڼ�����ɺ��ٽ���Դ���ݸ����߳̽��к������߼�����
    /// </remarks>
    public IEnumerator LoadAssetBundleAsyncIE2(string url, Action<AssetBundle> OnfinishCallback)
    {
        AssetBundleTask task = new AssetBundleTask();

        task.loadCompleteEvent.Reset();
        task.thread = new Thread(task.LoadAssetBundleThread);
        task.thread.Start(url);
        yield return new WaitUntil(() => task.loadCompleteEvent.WaitOne(0));
        // ��Դ������ɺ���߼�����
        OnfinishCallback?.Invoke(task.assetBundle);
    }


    /*
     * 
    Thread thread;
    AssetBundle assetBundle;
    ManualResetEvent loadCompleteEvent = new ManualResetEvent(false);

    void LoadAssetBundleThread(object url)
    {
        UnityWebRequest request = UnityWebRequest.Get((string)url);
        request.SendWebRequest();
        while (!request.isDone)
        {
            Thread.Sleep(100);
        }
        assetBundle = AssetBundle.LoadFromMemory(request.downloadHandler.data);
        loadCompleteEvent.Set();
    }

    IEnumerator LoadAssetBundleAsync(string url)
    {
        loadCompleteEvent.Reset();
        thread = new Thread(LoadAssetBundleThread);
        thread.Start(url);
        yield return new WaitUntil(() => loadCompleteEvent.WaitOne(0));
        // ��Դ������ɺ���߼�����
    }
    */




    class WriteTask
    {
        string path;
        byte[] bytes;

        public WriteTask(string path, byte[] bytes)
        {
            this.path = path;
            this.bytes = bytes;
        }

        public Thread thread;
        public ManualResetEvent loadCompleteEvent = new ManualResetEvent(false);
        public void WriteThread(object url)
        {
            File.WriteAllBytes(path, bytes);
            loadCompleteEvent.Set();
        }
    }


    public IEnumerator WriteAsyncIE(string path, byte[] bytes, Action<bool> OnfinishCallback = null)
    {
        WriteTask task = new WriteTask(path, bytes);
        task.loadCompleteEvent.Reset();
        task.thread = new Thread(task.WriteThread);
        task.thread.Start();
        yield return new WaitUntil(() => task.loadCompleteEvent.WaitOne(0));
        // ��Դ������ɺ���߼�����
        OnfinishCallback?.Invoke(true);
    }




    public  async Task WriteAllBytesAsync(string filePath, byte[] bytes, Action<bool> onFinishCallback)
    {
        // ʹ�� Task.Run ���ں�̨�߳���ִ��ͬ���� File.WriteAllBytes ����
        await Task.Run(() =>
        {
            try
            {
                File.WriteAllBytes(filePath, bytes);
                Debug.Log($"Data written to file successfully: {filePath}");
                onFinishCallback.Invoke(true);
            }
            catch (Exception e)
            {
                onFinishCallback.Invoke(false);
                // �����ﴦ���쳣�������¼��־���׳�һ���µ��쳣
                Debug.LogError($"Error writing data to file: {e.Message}");
                throw; // ��ѡ�������׳��쳣���Ա�����߿��Դ�����
            }
        });
    }
    /*
     ����Unity�����߳��ǵ��̵߳ģ����Ҹ�������Ⱦ������ģ���GUI���µȣ���������߳��Ͻ��г�ʱ���ͬ������������ļ���I/O���������ܻᵼ��֡���½�����ˣ����������ж�ص���̨�߳�ͨ����һ�������⡣
    */

}
