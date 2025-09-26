using System.Collections;
using UnityEngine;
using System;

/// <summary>
/// ��������
/// </summary>
/// <remarks>
/// * ��������Ͷ�˱Ҳ��������˳�ѭ����鱾�ض�������Ĳ�����
/// * ������Ͷ�˲�����ɺ���ʱ20�룬��ʼѭ����鱾�ض������档
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

    /// <summary> �ظ��������� </summary>
    public void DelayReshipOrderRepeat() =>  DoCor(COR_DELAY_RESHIP_ORDER_REPEAT, ReshipOrderRepeat());
    


    /// <summary> ֹͣ�ظ��������� </summary>
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
        yield return new WaitForSecondsRealtime(25f); // ��ʱ���㷨��
        yield return ReshipOrderRepeat01();
    }

    IEnumerator ReshipOrderRepeat01()
    {
        while (true)
        {
            DebugUtils.LogWarning($"��OrderReship��:��ʼ��鶩������ time:{Time.unscaledTime}");

            yield return DeviceCoinOut.Instance.ReshipOrde();

            yield return DeviceCoinIn.Instance.ReshipOrde();

            yield return DeviceMoneyBox.Instance.ReshipOrde();

            yield return new WaitForSecondsRealtime(20f); 
        }
    }


    IEnumerator ReshipOrderOnce()
    {
        yield return new WaitForSeconds(2); 

        DebugUtils.LogWarning($"��OrderReship��:��ʼ��鶩������ time:{Time.unscaledTime}");

        yield return DeviceCoinOut.Instance.ReshipOrde();

        yield return DeviceCoinIn.Instance.ReshipOrde();

        yield return DeviceMoneyBox.Instance.ReshipOrde();
    }


}
