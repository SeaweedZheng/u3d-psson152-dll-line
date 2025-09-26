using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class BonusReporter : MonoSingleton<BonusReporter>
{
    /// <summary> 地址 </summary>
    public string address = "shiruan.zs-sr.cn";

    /// <summary> 端口 </summary>
    public int port = 9091;

    /// <summary> 机台号 </summary>
    public string selfClientId = "11109001";

    /// <summary> 机台名称 </summary>
    public string selfName = "tomappBB";

    /// <summary> 分机号 </summary>
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
    /// <param name="bonusType">奖项类型</param>
    /// <param name="bonusName">奖项名称</param>
    /// <param name="payType">奖品类型, 1:币, 2:票</param>
    /// <param name="payCount">奖品数量</param>
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
                Debug.Log($"【额外奖上报】成功。 response: {response}  url: {url}");
                // 处理响应数据
                onSuccess?.Invoke(response);
            },
            onError: error => {
                Debug.LogError($"【额外奖上报】失败。  error: {error}  url: {url}");
                // 显示错误UI
                onError?.Invoke(error);
            },
            timeout: 8f // 设置8秒超时
        ));
    }






    // 带超时的POST请求（返回JSON响应）
    public IEnumerator PostRequest(string url, string jsonData,
        System.Action<string> onSuccess,
        System.Action<string> onError,
        float timeout = 10f)
    {
        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            // 设置请求体和头
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            // 设置超时
            request.timeout = (int)timeout;

            // 发送请求
            yield return request.SendWebRequest();


            // 处理结果
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


    // 集中处理错误信息（使用switch语句）
    private string GetErrorMessage(UnityWebRequest request, float timeout)
    {
        return request.result switch
        {
            UnityWebRequest.Result.ConnectionError =>
                $"连接错误: {request.error} (URL: {request.url})",

            UnityWebRequest.Result.ProtocolError =>
                $"协议错误: HTTP {(int)request.responseCode} {request.error}",

            UnityWebRequest.Result.DataProcessingError =>
                $"数据处理错误: {request.error}",

            UnityWebRequest.Result.Success =>
                "请求成功 (不应该在这里出现)",

            _ =>
                // 手动检查超时（针对特殊情况）
                request.error == "Request timeout"
                    ? $"请求超时: 超过{timeout}秒无响应 (URL: {request.url})"
                    : $"未知错误: {request.error} (状态码: {request.responseCode})"
        };
    }
}
