using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Net;
using PssOn00152;
using SimpleJSON;
using System.Security.Policy;
public class ReportManager : MonoSingleton<ReportManager>
{

    private CorController _corCtrl;
    private CorController corCtrl
    {
        get
        {
            if (_corCtrl == null)
                _corCtrl = new CorController(this);
            return _corCtrl;
        }
    }

    public void ClearCor(string name) => corCtrl.ClearCor(name);
    public void DoCor(string name, IEnumerator routine) => corCtrl.DoCor(name, routine);
    public bool IsCor(string name) => corCtrl.IsCor(name);
    //public IEnumerator DoTask(Action cb, int ms) => corCtrl.DoTask(cb, ms);





    const string COR_SEND_REPORT = "COR_SEND_REPORT";


    
    public void SendData(string jsonStr , Action successCallback, Action<string> errorCallback)
    {
        //string jsonStr = "{\"key1\":\"value1\", \"key2\":\"value2\"}";
        ///string jsonStr = JSONNodeUtil.ObjectToJsonStr(data);

        // game_id == 2002


        DoCor(COR_SEND_REPORT, _SendData(jsonStr, successCallback , errorCallback));
    }

    IEnumerator _SendData(string jsonStr,Action successCallback, Action<string> errorCallback)
    {
        //string jsonStr = "{\"key1\":\"value1\", \"key2\":\"value2\"}";

        DebugUtils.Log("数据上报地址 ： " + ApplicationSettings.Instance.reportUrl);

        using (UnityWebRequest webRequest = new UnityWebRequest(ApplicationSettings.Instance.reportUrl, "POST"))
        {
            byte[] jsonToSend = new UTF8Encoding().GetBytes(jsonStr);
            webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            // Send the request and wait until it is done
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                DebugUtils.LogWarning(webRequest.error);
                errorCallback?.Invoke(webRequest.error);
            }
            else
            {
                DebugUtils.Log("Response: " + webRequest.downloadHandler.text);
                successCallback?.Invoke();
            }
        }
    }
}
