using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using _consoleBB = PssOn00152.ConsoleBlackboard02;

public class DeviceRemoteControl : MonoSingleton<DeviceRemoteControl>
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
    public IEnumerator DoTask(Action cb, int ms) => corCtrl.DoTask(cb, ms);
    public IEnumerator DoTaskRepeat(Action cb, int ms) => corCtrl.DoTaskRepeat(cb, ms);

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void CheckMqttRemoteControl()
    {
        if (_consoleBB.Instance.isUseRemoteControl)
        {
            string[] addr = _consoleBB.Instance.remoteControlSetting.Split(':');
            MqttRemoteController.Instance.Init(addr[0], int.Parse(addr[1]), MqttAppType.IsGameApp, _consoleBB.Instance.machineID,
                _consoleBB.Instance.remoteControlAccount, _consoleBB.Instance.remoteControlPassword);
            //MqttRemoteButtonController.Instance.Init("192.168.3.174", 1883, MqttMachineTypeEnum.IsGameApp , _consoleBB.Instance.machineID);

            CheckMqttRemoteControlConnect();
        }
        else
        {
            MqttRemoteController.Instance.Close();
            ClearCor(COR_CHECK_REMOTE_CONTROL_CONNECT);
        }
    }


    const string COR_CHECK_REMOTE_CONTROL_CONNECT = "COR_CHECK_REMOTE_CONTROL_CONNECT";
    void CheckMqttRemoteControlConnect()
    {
        _CheckMqttRemoteControlConnect();
        DoCor(COR_CHECK_REMOTE_CONTROL_CONNECT, DoTaskRepeat(_CheckMqttRemoteControlConnect, 5000));
    }
    void _CheckMqttRemoteControlConnect()
    {
        if (_consoleBB.Instance.isConnectRemoteControl != MqttRemoteController.Instance.isConnected)
        {
            _consoleBB.Instance.isConnectRemoteControl = MqttRemoteController.Instance.isConnected;
        }

        if (_consoleBB.Instance.isUseRemoteControl == false)
        {
            ClearCor(COR_CHECK_REMOTE_CONTROL_CONNECT);
        }
    }

}
