using GameMaker;
using Newtonsoft.Json.Linq;
using PssOn00152;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum DeviceState
{
    /// <summary> 下线 </summary>
    Offline,
    /// <summary> 在线 </summary>
    OnLine,
    /// <summary> 断开网络 </summary>
    Disconnect,
}
public class AppRemoteControl : MonoBehaviour
{
    public Button btnSpin, btnTicket, btnBetDown, btnBetUp, btnBetMax,
        btnTab, btnPrev, btnNext, btnExit, btnSwitch,
        btnConnectExternal, btnConnectInner;

    public PIDButtonX btnxSpin;

    public Image imgAvatar;

    public Text txtMachineName, txtCredit, txtBet,
        txtJP1, txtJP2, txtJP3, txtJP4, txtGameState,
        txtConnect;

    public Dropdown dpMachineLst;


    DeviceState targetDeviceState = DeviceState.Offline;

    private void Awake()
    {
        btnSpin.onClick.AddListener(TestA2MSendBtnSpin);
        btnTicket.onClick.AddListener(TestA2MSendBtnTicketOut);
        btnBetDown.onClick.AddListener(TestA2MSendBtnBetDown);
        btnBetUp.onClick.AddListener(TestA2MSendBtnBetUp);
        btnBetMax.onClick.AddListener(TestA2MSendBtnBetMax);
        btnTab.onClick.AddListener(TestA2MSendBtnTable);
        btnPrev.onClick.AddListener(TestA2MSendBtnPrevious);
        btnNext.onClick.AddListener(TestA2MSendBtnNext);
        btnExit.onClick.AddListener(TestA2MSendBtnExit);
        btnSwitch.onClick.AddListener(TestA2MSendBtnSwitch);


        btnxSpin.onLongClick.AddListener(OnLongClickButtonSpin);
        btnxSpin.onShortClick.AddListener(OnClickButtonSpin);

        btnConnectExternal.onClick.AddListener(OnClickConnectExternal);
        btnConnectInner.onClick.AddListener(OnClickConnectInner);

        ClearData();
        InitializeDropdown();
        // 添加选项变更监听
        dpMachineLst.onValueChanged.AddListener(OnDropdownValueChanged);



        EventCenter.Instance.AddEventListener<EventData>(GlobalEvent.ON_MQTT_REMOTE_CONTROL_EVENT, OnMqttRemoteControlAsk);
    }

    private void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener<EventData>(GlobalEvent.ON_MQTT_REMOTE_CONTROL_EVENT, OnMqttRemoteControlAsk);
    }


    private void OnClickConnectInner()
    {
        strConnect = "192.168.3.174:1883";
        txtConnect.text = $"连接： {strConnect}";
        string[] addr = strConnect.Split(':');
        //MqttRemoteController.Instance.Init("192.168.3.174", 1883, MqttAppType.IsCtrlApp, "app1");
        MqttRemoteController.Instance.Init(addr[0], int.Parse(addr[1]), MqttAppType.IsCtrlApp, "app1","tester01", "123456");
    }
    private void OnClickConnectExternal()
    {
        strConnect = "broker.hivemq.com:1883";
        txtConnect.text = $"连接： {strConnect}";
        string[] addr = strConnect.Split(':');
        //MqttRemoteController.Instance.Init("192.168.3.174", 1883, MqttAppType.IsCtrlApp, "app1");
        MqttRemoteController.Instance.Init(addr[0], int.Parse(addr[1]), MqttAppType.IsCtrlApp, "app1", "tester01", "123456");
    }



    string strConnect = "";
    void Start()
    {
        OnClickConnectInner();
    }

    List<string> machineIds = new List<string>() { "NULL"};



    #region 监听设备连接
    float connectTimeS = 0f;
    void  CheckDeviceConnect(string id)
    {
        if (!string.IsNullOrEmpty(targetClientId))
        {
            if (targetClientId == id)
            {
                connectTimeS = Time.unscaledTime;
            }
        }
    }
    // 方式1：使用RGB值（范围0-1）
    Color darkGray = new Color(0.2f, 0.2f, 0.2f, 1f); // #333333    0x33 = 51  51/255 = 0.2f
    private void Update()
    {
        if (!string.IsNullOrEmpty(targetClientId))
        {
            if (Time.unscaledTime - connectTimeS > 12f)
            {
                targetDeviceState = DeviceState.Disconnect;
                imgAvatar.color = darkGray; // Color.gray; 
            }
            else
            {
                targetDeviceState = DeviceState.OnLine;
                imgAvatar.color = Color.white;
            }
        }
    }

    #endregion

    public void OnMqttRemoteControlAsk(EventData res)
    {
        if (res.name == MqttRemoteCtrlMethod.On)
        {

            string deviceId = (string)res.value;
            if (!machineIds.Contains(deviceId))
            {
                machineIds.Add(deviceId);
                AddMachineOption(deviceId);
            }

            CheckDeviceConnect(deviceId);

        }
        else if (res.name == MqttRemoteCtrlMethod.Off)
        {
            string deviceId = (string)res.value;

            if (machineIds.Contains(deviceId))
            {
                machineIds.Remove(deviceId);
                RemoveMachineOption(deviceId);
            }

            if (deviceId == targetClientId)
            {
                dpMachineLst.value = 0; // 选中第一个选项
            }
        }
        else
        {

            JObject node = JObject.Parse((string)res.value);
            JObject data = node["body"][0] as JObject;

            if (data["code"].ToObject<int>() != 0)
            {
                return;
            }

            switch (res.name)
            {
                case MqttRemoteCtrlMethod.GetErrorCode:
                    {

                    }
                    break;
                case MqttRemoteCtrlMethod.GetGameState:
                    {

                        int credit =  data["credit"].ToObject<int>();
                        txtCredit.text = $"Credit: {credit}";


                        int bet = data["bet"].ToObject<int>();
                        txtBet.text = $"Bet: {bet}";

                        int jp1 = data["jp1"].ToObject<int>();
                        int jp2 = data["jp2"].ToObject<int>();
                        int jp3 = data["jp3"].ToObject<int>();
                        int jp4 = data["jp4"].ToObject<int>();
                        txtJP1.text = $"JP1: {jp1}";
                        txtJP2.text = $"JP2: {jp2}";
                        txtJP3.text = $"JP3: {jp3}";
                        txtJP4.text = $"JP4: {jp4}";

                        string gameState = data["game_state"].ToObject<string>();
                        txtGameState.text = $"State: {gameState}";

                        try
                        {
                            bool isAuto = data["is_auto"].ToObject<bool>();
                            bool isSpin = data["is_spin"].ToObject<bool>();
                            if (isAuto)
                            {
                                btnxSpin.transform.Find("Text").GetComponent<Text>().text = "Auto";
                            }else 
                            {
                                btnxSpin.transform.Find("Text").GetComponent<Text>().text =
                                    isSpin?"Stop":"Spin";
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.LogError("这里报错了");
                            Debug.LogException(e);
                           
                        }


                    }
                    break;
                case MqttRemoteCtrlMethod.GetMachineInfo:
                    {
                        string name = data["machine_name"].ToObject<string>();
                        txtMachineName.text = $"{name}\n({targetClientId})";

                        string avatarUrl = data["avatar_url"].ToObject<string>();
                        Debug.Log($"头像数据 {avatarUrl}");
                        if(avatarUrl.StartsWith("http"))
                            StartCoroutine(DownloadImage(avatarUrl));

                    }
                    break;
            }
        }
    }





    void InitializeDropdown()
    {
        // 清空现有选项
        dpMachineLst.ClearOptions();

        // 添加选项到Dropdown
        dpMachineLst.AddOptions(machineIds);

        // 设置默认选中项
        dpMachineLst.value = 0; // 选中第一个选项
        dpMachineLst.RefreshShownValue(); // 刷新显示        // 清空现有选项
    }

    // 动态添加单个选项
    public void AddMachineOption(string machineName)
    {
        dpMachineLst.options.Add(new Dropdown.OptionData(machineName));
        dpMachineLst.RefreshShownValue();
    }

    // 动态移除选项
    public void RemoveMachineOption(string machineName)
    {
        for (int i = 0; i < dpMachineLst.options.Count; i++)
        {
            if (dpMachineLst.options[i].text == machineName)
            {
                dpMachineLst.options.RemoveAt(i);
                break;
            }
        }
        dpMachineLst.RefreshShownValue();
    }


    // 选项变更时的回调
    private void OnDropdownValueChanged(int index)
    {
        Debug.Log($"选中项变更: {dpMachineLst.options[index].text}, 索引: {index}");

        // 在这里处理选中特定项的逻辑
        switch (index)
        {
            case 0:
                {
                    ClearData();
                    MqttRemoteController.Instance.ClearTargetClientId();
                    targetClientId = null;
                }
                break;
            default:
                {
                    ClearData();
                    targetClientId = dpMachineLst.options[index].text;
                    MqttRemoteController.Instance.SetTargetClientId(targetClientId);
                    MqttRemoteController.Instance.RequestCommand(MqttRemoteCtrlMethod.GetMachineInfo,null);
                    MqttRemoteController.Instance.RequestCommand(MqttRemoteCtrlMethod.GetGameState, null);
                    UpdateGameState();
                }
                break;
        }
    }

    void ClearData()
    {
        targetDeviceState = DeviceState.Offline;
        txtMachineName.text = "Machine: --";
        txtCredit.text = "Credit: --";
        txtBet.text = "Bet: --";
        txtJP1.text = "JP1: --";
        txtJP2.text = "JP2: --";
        txtJP3.text = "JP3: --";
        txtJP4.text = "JP4: --";
        txtGameState.text = "State: --";
        imgAvatar.sprite = null;
    }




    string targetClientId = null;


    // 下载图片的协程
    private IEnumerator DownloadImage(string url)
    {
        Debug.Log($"下载图片： {url}");
        using (WWW www = new WWW(url))
        {
            yield return www;

            if (string.IsNullOrEmpty(www.error))
            {
                // 下载成功，将Texture转换为Sprite并应用到Image
                Texture2D texture = www.texture;
                Sprite sprite = TextureToSprite(texture);

                imgAvatar.sprite = sprite;

                // 可选：调整Image组件的大小以匹配Sprite
                imgAvatar.SetNativeSize();
            }
            else
            {
                Debug.LogError($"图片下载失败: {www.error}");

            }
        }
    }
    private Sprite TextureToSprite(Texture2D texture)
    {
        if (texture == null)
            return null;

        // 创建Sprite（默认使用整个Texture）
        return Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f) // 中心点
        );
    }



    void OnClickButtonSpin()
    {
        TestA2MSendBtnSpin();
    }

    void OnLongClickButtonSpin()
    {
        TestA2MSendBtnAuto();
    }




    Coroutine corUpdateGameState = null;

    void UpdateGameState()
    {
        if(corUpdateGameState != null)
            StopCoroutine(corUpdateGameState);
        corUpdateGameState = null;

        if (!string.IsNullOrEmpty(targetClientId))
        {
            corUpdateGameState = StartCoroutine(DoUpdateGameState());
        }
    }
    IEnumerator DoUpdateGameState()
    {

        yield return new WaitForSecondsRealtime(1f);

        yield return new WaitUntil(()=> targetDeviceState== DeviceState.OnLine);
        MqttRemoteController.Instance.RequestCommand(MqttRemoteCtrlMethod.GetGameState, null);

        yield return new WaitForSecondsRealtime(1.5f);

        yield return new WaitUntil(() => targetDeviceState == DeviceState.OnLine);
        MqttRemoteController.Instance.RequestCommand(MqttRemoteCtrlMethod.GetGameState, null);

        while (true)
        {
            //MqttRemoteController.Instance.RequestCommand(MqttRemoteCtrlMethod.GetGameState, null);
            yield return new WaitForSecondsRealtime(5f);

            yield return new WaitUntil(() => targetDeviceState == DeviceState.OnLine);
            MqttRemoteController.Instance.RequestCommand(MqttRemoteCtrlMethod.GetGameState, null);
        }
    }



    [Button]
    void TestA2MSendBtnTicketOut()
    {
        if (!MqttRemoteController.Instance.isConnected)
            return;
        if (string.IsNullOrEmpty(targetClientId))
            return;

        MqttRemoteController.Instance.RequestCommand(MqttRemoteCtrlMethod.BtnTicketOut, null);
    }



    [Button]
    void TestA2MSendBtnSpin()
    {
        if (!MqttRemoteController.Instance.isConnected)
            return;
        if (string.IsNullOrEmpty(targetClientId))
            return;
        MqttRemoteController.Instance.RequestCommand(MqttRemoteCtrlMethod.BtnSpin, null);
        UpdateGameState();
    }

    [Button]
    void TestA2MSendBtnAuto()
    {
        if (!MqttRemoteController.Instance.isConnected)
            return;
        if (string.IsNullOrEmpty(targetClientId))
            return;
        MqttRemoteController.Instance.RequestCommand(MqttRemoteCtrlMethod.BtnAuto, null);
        UpdateGameState();
    }

    [Button]
    void TestA2MSendBtnExit()
    {
        if (!MqttRemoteController.Instance.isConnected)
            return;
        if (string.IsNullOrEmpty(targetClientId))
            return;
        MqttRemoteController.Instance.RequestCommand(MqttRemoteCtrlMethod.BtnExit, null);
    }



    [Button]
    void TestA2MSendBtnBetUp()
    {
        if (!MqttRemoteController.Instance.isConnected)
            return;
        if (string.IsNullOrEmpty(targetClientId))
            return;
        MqttRemoteController.Instance.RequestCommand(MqttRemoteCtrlMethod.BtnBetUp, null);
    }

    [Button]
    void TestA2MSendBtnBetDown()
    {
        if (!MqttRemoteController.Instance.isConnected)
            return;
        if (string.IsNullOrEmpty(targetClientId))
            return;
        MqttRemoteController.Instance.RequestCommand(MqttRemoteCtrlMethod.BtnBetDown, null);
    }
    [Button]
    void TestA2MSendBtnBetMax()
    {
        if (!MqttRemoteController.Instance.isConnected)
            return;
        if (string.IsNullOrEmpty(targetClientId))
            return;
        MqttRemoteController.Instance.RequestCommand(MqttRemoteCtrlMethod.BtnBetMax, null);
    }

    [Button]
    void TestA2MSendBtnTable()
    {
        if (!MqttRemoteController.Instance.isConnected)
            return;
        if (string.IsNullOrEmpty(targetClientId))
            return;
        MqttRemoteController.Instance.RequestCommand(MqttRemoteCtrlMethod.BtnTable, null);
    }

    [Button]
    void TestA2MSendBtnPrevious()
    {
        if (!MqttRemoteController.Instance.isConnected)
            return;
        if (string.IsNullOrEmpty(targetClientId))
            return;
        MqttRemoteController.Instance.RequestCommand(MqttRemoteCtrlMethod.BtnPrevious, null);
    }

    [Button]
    void TestA2MSendBtnNext()
    {
        if (!MqttRemoteController.Instance.isConnected)
            return;
        if (string.IsNullOrEmpty(targetClientId))
            return;
        MqttRemoteController.Instance.RequestCommand(MqttRemoteCtrlMethod.BtnNext, null);
    }


    [Button]
    void TestA2MSendBtnSwitch()
    {
        if (!MqttRemoteController.Instance.isConnected)
            return;
        if (string.IsNullOrEmpty(targetClientId))
            return;
        MqttRemoteController.Instance.RequestCommand(MqttRemoteCtrlMethod.BtnSwitch, null);
    }






}
