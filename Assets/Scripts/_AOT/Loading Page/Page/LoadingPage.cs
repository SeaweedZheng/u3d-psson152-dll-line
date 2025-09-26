#define DISABLE_DELAY
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.Collections;


public class LoadingProgress
{

    /// <summary> ���ư����е�hotfix dll ������</summary>
    public const string COPY_SA_HOTFIX_DLL = "COPY_SA_HOTFIX_DLL";
    /// <summary> ���ư�����AB�������� </summary>
    public const string COPY_SA_ASSET_BUNDLE = "COPY_SA_ASSET_BUNDLE";

    public const string COPY_SA_ASSET_BACKUP = "COPY_SA_ASSET_BACKUP";

    /// <summary> ������޴��������ļ� </summary>
    public const string CHECK_COPY_TEMP_HOTFIX_FILE = "CHECK_COPY_TEMP_HOTFIX_FILE";

    /// <summary> ��������ȸ��汾 </summary>
    public const string CHECK_WEB_VERSION = "CHECK_WEB_VERSION";

    /// <summary> ����hotfix dll </summary>
    public const string DOWNLOAD_HOTFIX_DLL = "DOWNLOAD_HOTFIX_DLL";

    /// <summary> �����ȸ�AB�� </summary>
    public const string DOWNLOAD_ASSET_BUNDLE = "DOWNLOAD_ASSET_BUNDLE";

    /// <summary> ����"��Դ����" </summary>
    public const string DOWNLOAD_ASSET_BACKUP = "DOWNLOAD_ASSET_BACKUP";


    /// <summary> �������ص��ļ� </summary>
    public const string COPY_TEMP_HOTFIX_FILE = "COPY_TEMP_HOTFIX_FILE";


    /// <summary> ɾ�����õ�ab�� </summary>
    public const string DELETE_UNUSE_ASSET_BUNDLE = "DELETE_UNUSE_ASSET_BUNDLE";

    /// <summary> ɾ�����õ�hotfix dll </summary>
    public const string DELETE_UNUSE_HOTFIX_DLL = "DELETE_UNUSE_HOTFIX_DLL";

    /// <summary> ����AOT dll���ڴ� </summary>
    //public const string LOAD_AOT_DLL = "LOAD_AOT_DLL";

    /// <summary> ����Ԫ���ݸ�AOT,�����Ǹ��ȸ���dll����Ԫ����</summary>
    public const string LOAD_AOT_META_DATA = "LOAD_AOT_META_DATA";

    /// <summary> ����hotfix dll���ڴ� </summary>
    public const string LOAD_HOTFIX_DLL = "LOAD_HOTFIX_DLL";


    /// <summary> Ԥ����AB�����ڴ� </summary>
    public const string PRELOAD_ASSET_BUNDLE = "PRELOAD_ASSET_BUNDLE";

    /// <summary> Ԥ������Դ���ڴ� </summary>
    public const string PRELOAD_ASSET = "PRELOAD_ASSET";

    /// <summary> ���ӻ�̨����ȡ������ </summary>
    public const string CONNECT_MACHINE = "CONNECT_MACHINE";

    /// <summary> ��ʼ���������� </summary>
    public const string INIT_SETTINGS = "INIT_SETTINGS";

    /// <summary> ������Ϸ(��Ϸ���ؽ���) </summary>
    public const string ENTER_GAME = "ENTER_GAME";
}
public class LoadingPage : MonoBehaviour
{
    private static LoadingPage _instance;
    public static LoadingPage Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("Page Loading").transform.GetComponent<LoadingPage>();
                //_instance = new LoadingPage();
            }
            return _instance;
        }
    }

    public Text txtMsg;

    public Image imgBG;

    public Image imgLogo;

    public Slider sldProgress;

    Dictionary<string, int> allProgress = new Dictionary<string, int>();

    Dictionary<string, int> curProgress = new Dictionary<string, int>();


    public class ShowMsgInfo
    {
        public string msg;
        public float progress = 0;
    }
    List<ShowMsgInfo> msgLst = new List<ShowMsgInfo>();


    void Awake()
    {
        Debug.Log("i am LoadingPage !!");
    }


  

    private void InitParam()
    {
        msgLst.Clear();
        isError = false;
        allProgress.Clear();
        curProgress.Clear();
        sldProgress.value = 0;
        txtMsg.text = "";

        Type t = typeof(LoadingProgress);
        var fields = t.GetFields();
        foreach (var fieldInfo in fields) { 
            allProgress.Add((string)fieldInfo.GetRawConstantValue(), 0);
            curProgress.Add((string)fieldInfo.GetRawConstantValue(), 0);
        }
    }

    /// <summary>
    /// <p>��ȡ��������ֵ</p>
    /// </summary>
    /// <returns></returns>
    /// <remarks>
    /// * ���������ļ��طֳɶ�Ρ�<br/>
    /// * ÿ�ν��ȣ��ٷֳɶ��С������<br/>
    /// </remarks>
    float GetProgressValue()
    {
        float partOne = 1f / (float)(allProgress.Count);

        float A =  (float)(allProgress.Count - curProgress.Count) * partOne;

        float B = 0f;

        foreach (KeyValuePair<string, int> kv in curProgress )
        {
            int hasDoNum = allProgress[kv.Key] - kv.Value;
            if (hasDoNum > 0)
            {
                B += (float)hasDoNum * (partOne / (float)allProgress[kv.Key]);
            }
        }
        //Debug.Log($"@ A = {A} , B = {B} , C = {A + B} , partOne = {partOne}");
        return A + B;
    }

    /// <summary>
    /// <p>���ĳ����������</p>
    /// </summary>
    /// <param name="mark">����"mark"</param>
    /// <param name="count">�ý��ȵ��������</param>
    public void AddProgressCount(string mark,int count)
    {
        //��д��(֧���ظ�У��-�ȸ��汾�ļ� - ��������������ʱ)
        if (!curProgress.ContainsKey(mark))
        {
            curProgress.Add(mark, 0);
        }
        curProgress[mark] += count;
        allProgress[mark] += count;


        /* ��д��
        if (curProgress.ContainsKey(mark))
        {
            curProgress[mark] += count;
            allProgress[mark] += count;
        }
        */
    }

    /*
    public void SetProgressCount(string mark, int count)
    {
        if (curProgress.ContainsKey(mark))
        {
            curProgress[mark] = count;
            allProgress[mark] = count;
        }
    }*/

    /// <summary>
    /// <p>ɾ��ĳ����������</p>
    /// </summary>
    /// <param name="mark">����"mark"</param>
    public void RemoveProgress(string mark)
    {
        if (curProgress.ContainsKey(mark))
            curProgress.Remove(mark);   
    }

 

    /// <summary>
    /// <p>��ʾ���ؽ��Ⱥ���Ϣ</p>
    /// </summary>
    /// <param name="mark">����"mark"</param>
    /// <param name="str">��ʾ����Ϣ</param>
    /// <remarks>
    /// * �����ǽ������ʾ��<br/>
    /// * �����κε������޸ġ�<br/>
    /// </remarks>
    public void Next(string mark, string str)
    {
        if (isError) return;

        if (curProgress.ContainsKey(mark))
        {
            if (--curProgress[mark] < 0)
                curProgress[mark] = 0;
        }
        float val = GetProgressValue();

        //sldProgress.value = val;
        //txtMsg.text = ShowStr(str, val);
        msgLst.Add(new ShowMsgInfo()
        {
            msg = CreatStr(str, val),
            progress = val,
        });

#if DISABLE_DELAY
        ShowProgressUIMsg();
#endif
    }


    bool isError = false;
    public void Error(string str)
    {
        isError = true;
        txtMsg.text = str;
    }

    public void Finish(string str)
    {
        msgLst.Add(new ShowMsgInfo()
        {
            msg = CreatStr(str,1),
            progress = 1,
        });
#if DISABLE_DELAY
        ShowProgressUIMsg();
#endif

    }


    string CreatStr(string str , float pg)
    {
        string  _pg = (pg * 100f).ToString("N1");
        return ApplicationSettings.Instance.isRelease ? $"{_pg}%" : $"{str}  ({_pg})%";
    }

    public void Open()
    {
        this.gameObject.SetActive(true);
        InitParam();
    }


    Coroutine corClose = null;
    public void Close(float delayS = -1)
    {
        if (delayS > 0)
        {
            if(corClose != null)
                StopCoroutine(corClose);
            corClose = StartCoroutine(DelayToClose(delayS));
            return;
        }
        this.gameObject.SetActive(false);
    }

    
    IEnumerator DelayToClose(float delayS)
    {
        yield return new WaitForSecondsRealtime(delayS);
        this.gameObject.SetActive(false);
    }




     
    float lastRunTimeS = 0;
    public void Update()
    {
#if DISABLE_DELAY
        return;
#endif
        if (isError)
            return;

        float nowRunTimeS = Time.unscaledTime;
        if (nowRunTimeS - lastRunTimeS > 0.2f)
        {
            lastRunTimeS = nowRunTimeS;
            ShowProgressUIMsg(); 
        }
    }


    float curShowProgress = 0f;
    string curShowMsg = "";
    void ShowProgressUIMsg()
    {
        if (isError)
            return;

        if (msgLst.Count > 0)
        {
            ShowMsgInfo msgInfo = msgLst[0];
            msgLst.RemoveAt(0);

            curShowProgress = msgInfo.progress;
            curShowMsg = msgInfo.msg;

            sldProgress.value = curShowProgress;
            txtMsg.text = curShowMsg;
        }
    }

    /// <summary>
    /// ֻ��ʾ����
    /// </summary>
    /// <param name="msg"></param>
    public void RefreshProgressUIMsg(string msg)
    {
        if (isError)
            return;

        curShowMsg = CreatStr(msg, curShowProgress);

        sldProgress.value = curShowProgress;
        txtMsg.text = curShowMsg;
    }
}
