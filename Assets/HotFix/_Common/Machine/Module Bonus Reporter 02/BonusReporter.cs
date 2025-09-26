using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class BonusReporter : MonoSingleton<BonusReporter>
{
    /// <summary> ��ַ </summary>
    public string address = "shiruan.zs-sr.cn";

    /// <summary> �˿� </summary>
    public int port = 9091;

    /// <summary> ��̨�� </summary>
    public string selfClientId = "11109001";

    /// <summary> ��̨���� </summary>
    public string selfName = "tomappBB";

    /// <summary> �ֻ��� </summary>
    public string pid;

    bool isEnable = false;
    public void Init(string address, int port,string selfClientId, string selfName, string pid) {

        this.address = address;
        this.port = port;
        this.selfClientId = selfClientId;
        this.selfName = selfName;
        this.pid = pid;

        isEnable = true;
    }


    public void Close()
    {
        isEnable = false;
    }

    public string url =>$"http://{address}:{port}/dsc/AemsOrder/Act/PushGamePrize";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bonusType">��������</param>
    /// <param name="bonusName">��������</param>
    /// <param name="payType">��Ʒ����, 1:��, 2:Ʊ</param>
    /// <param name="payCount">��Ʒ����</param>
    public void Post(string bonusType, string bonusName, int payType, int payCount, System.Action<string> onSuccess,
        System.Action<string> onError)
    {
        if (isEnable == false)
            return;

        JObject data = JObject.Parse("{}");
        data.Add("MachineID", selfClientId);
        data.Add("MachineName", selfName);
        data.Add("MachCode", pid);
        data.Add("PrizeCode", bonusType);
        data.Add("PrizeName", bonusName);
        data.Add("PrizeType", payType);
        data.Add("Num", payCount);

        string jsonData = JsonConvert.SerializeObject(data, Formatting.None);

        StartCoroutine(PostRequest(
            url: url,
            jsonData: jsonData,
            onSuccess: response => {
                Debug.Log($"�����⽱�ϱ����ɹ��� response: {response}  url: {url}");
                // ������Ӧ����
                onSuccess?.Invoke(response);
            },
            onError: error => {
                Debug.LogError($"�����⽱�ϱ���ʧ�ܡ�  error: {error}  url: {url}");
                // ��ʾ����UI
                onError?.Invoke(error);
            },
            timeout: 8f // ����8�볬ʱ
        ));
    }






    // ����ʱ��POST���󣨷���JSON��Ӧ��
    public IEnumerator PostRequest(string url, string jsonData,
        System.Action<string> onSuccess,
        System.Action<string> onError,
        float timeout = 10f)
    {
        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            // �����������ͷ
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            // ���ó�ʱ
            request.timeout = (int)timeout;

            // ��������
            yield return request.SendWebRequest();


            // ������
            if (request.result == UnityWebRequest.Result.Success)
            {
                onSuccess?.Invoke(request.downloadHandler.text);
            }
            else
            {
                string errorMsg = GetErrorMessage(request, timeout);
                onError?.Invoke(errorMsg);
            }
        }
    }


    // ���д��������Ϣ��ʹ��switch��䣩
    private string GetErrorMessage(UnityWebRequest request, float timeout)
    {
        return request.result switch
        {
            UnityWebRequest.Result.ConnectionError =>
                $"���Ӵ���: {request.error} (URL: {request.url})",

            UnityWebRequest.Result.ProtocolError =>
                $"Э�����: HTTP {(int)request.responseCode} {request.error}",

            UnityWebRequest.Result.DataProcessingError =>
                $"���ݴ������: {request.error}",

            UnityWebRequest.Result.Success =>
                "����ɹ� (��Ӧ�����������)",

            _ =>
                // �ֶ���鳬ʱ��������������
                request.error == "Request timeout"
                    ? $"����ʱ: ����{timeout}������Ӧ (URL: {request.url})"
                    : $"δ֪����: {request.error} (״̬��: {request.responseCode})"
        };
    }
}
