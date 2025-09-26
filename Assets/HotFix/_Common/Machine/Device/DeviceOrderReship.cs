using System.Collections;
using UnityEngine;
using System;

/// <summary>
/// 订单补发
/// </summary>
/// <remarks>
/// * 发现正常投退币操作，则退出循环检查本地订单缓存的操作。
/// * 在正常投退操作完成后延时20秒，开始循环检查本地订单缓存。
/// </remarks>
public class DeviceOrderReship : MonoSingleton<DeviceOrderReship>
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

    public void ClearAllCor() => corCtrl.ClearAllCor();
    public void ClearCor(string name) => corCtrl.ClearCor(name);
    public void DoCor(string name, IEnumerator routine) => corCtrl.DoCor(name, routine);
    public bool IsCor(string name) => corCtrl.IsCor(name);
    public IEnumerator DoTask(Action cb, int ms) => corCtrl.DoTask(cb, ms);
    public IEnumerator DoTaskRepeat(Action cb, int ms) => corCtrl.DoTaskRepeat(cb, ms);





    const string COR_RESHIP_ORDER_ONCE = "COR_RESHIP_ORDER_ONCE";
    const string COR_DELAY_RESHIP_ORDER_REPEAT = "COR_DELAY_RESHIP_ORDER_REPEAT";

    /// <summary> 重复补发订单 </summary>
    public void DelayReshipOrderRepeat() =>  DoCor(COR_DELAY_RESHIP_ORDER_REPEAT, ReshipOrderRepeat());
    


    /// <summary> 停止重复补发订单 </summary>
    public void ClearReshipOrderRepeat() =>  ClearCor(COR_DELAY_RESHIP_ORDER_REPEAT);
    


    void Start()
    {
        DoCor(COR_RESHIP_ORDER_ONCE, ReshipOrderOnce());
        DelayReshipOrderRepeat();
    }

    protected override void OnDestroy()
    {
        ClearAllCor();
        base.OnDestroy();
    }

    IEnumerator ReshipOrderRepeat()
    {
        yield return new WaitForSecondsRealtime(25f); // 延时读算法卡
        yield return ReshipOrderRepeat01();
    }

    IEnumerator ReshipOrderRepeat01()
    {
        while (true)
        {
            DebugUtils.LogWarning($"【OrderReship】:开始检查订单补发 time:{Time.unscaledTime}");

            yield return DeviceCoinOut.Instance.ReshipOrde();

            yield return DeviceCoinIn.Instance.ReshipOrde();

            yield return DeviceMoneyBox.Instance.ReshipOrde();

            yield return new WaitForSecondsRealtime(20f); 
        }
    }


    IEnumerator ReshipOrderOnce()
    {
        yield return new WaitForSeconds(2); 

        DebugUtils.LogWarning($"【OrderReship】:开始检查订单补发 time:{Time.unscaledTime}");

        yield return DeviceCoinOut.Instance.ReshipOrde();

        yield return DeviceCoinIn.Instance.ReshipOrde();

        yield return DeviceMoneyBox.Instance.ReshipOrde();
    }


}
