using GameMaker;
using SlotMaker;
using System.Collections;
using System;
using UnityEngine;
using _consoleBB = PssOn00152.ConsoleBlackboard02;
using _mainBB = GameMaker.MainBlackboard;
using static UnityEngine.ParticleSystem;


/// <summary>
/// 
/// </summary>
/// <remarks>
/// * �����ʵ���
/// * �����ʱ���
/// * ���UI��ʾ���
/// </remarks>
public class MainBlackboardController : MonoSingleton<MainBlackboardController>
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


    
    const string COR_MY_CREDIT_AINM = "COR_MY_CREDIT_AINM";
    const string COR_AUTO_SYNC_MY_REAL_CREDIT = "COR_AUTO_SYNC_MY_REAL_CREDIT";

    /// <summary> �����ʱ���  </summary>
    public long myTempCredit
    {
        get => _mainBB.Instance.myCredit;
    }

    /// <summary> �����ʵ���  </summary>
    public long myRealCredit
    {
        get => _consoleBB.Instance.myCredit;
    }

    /// <summary>
    /// ��Ǯ������־λ
    /// </summary>
    /// <param name="isAnim"></param>
    public bool SyncCreditAnim(bool isAnim)
    {
        if (isAnim)
            DoCor(COR_MY_CREDIT_AINM, DoTask(() => { }, 4000));
        else
            ClearCor(COR_MY_CREDIT_AINM);

        return isAnim;
    }
  

    /// <summary>
    /// ���������ʵ���
    /// </summary>
    /// <param name="crefit"></param>
    public void SetMyRealCredit(long crefit)
    {
        _consoleBB.Instance.myCredit = crefit;
    }

    /// <summary>
    /// ������ҽ�����
    /// </summary>
    /// <param name="credit">Ҫ���ٵĽ��</param>
    /// <param name="isEvent">�Ƿ����¼�</param>
    /// <param name="isAnim">�Ƿ���Ҫ��Ǯ����</param>
    public void MinusMyTempCredit(long credit, bool isEvent = true, bool isAnim = false)
    {
        long fromCredit = _mainBB.Instance.myCredit;
        _mainBB.Instance.myCredit -= credit;

        if (isEvent)
        {
            

            /* ͣ������Ǯ�����������õ���ǰ���
            EventCenter.Instance.EventTrigger<EventData>(MetaUIEvent.ON_CREDIT_EVENT,
                new EventData<bool>(MetaUIEvent.UpdateNaviCredit, true));
            */

            // ͣ������Ǯ�����������õ���ǰ���
            EventCenter.Instance.EventTrigger<EventData>(MetaUIEvent.ON_CREDIT_EVENT,
            new EventData<UpdateNaviCredit>(MetaUIEvent.UpdateNaviCredit,
            new UpdateNaviCredit()
            {
                isAnim = SyncCreditAnim(isAnim),
                fromCredit = fromCredit,
                toCredit = _mainBB.Instance.myCredit
            }));
        }

    }



    /// <summary>
    /// ͬ����Temp��������ʵ��
    /// </summary>
    /// <returns></returns>
    /// <remarks>
    /// * ��û�С��Ӽ�Ǯ�������������У�����ͬ����<br/>
    /// * ͬ���ɹ�����֪ͨUIˢ��<br/>
    /// * MyCredit = MyUICredit + MyTempCredit<br/>
    /// </remarks>
    public bool TrySyncMyCreditToReel()
    {
        if (IsCor(COR_MY_CREDIT_AINM))
            return false;

       if (BlackboardUtils.IsBlackboard("./") ){

            if(BlackboardUtils.GetValue<bool>("./isSpin"))
            //if (BlackboardUtils.GetValue<string>("./gameState") != "Idle")
            {
                return false;
            }
        }

        SyncMyTempCreditToReal(true);// ����ͬ��
        return true;
    }

    /// <summary>
    /// �Զ�ͬ����ҽ��ȴ���Ϸ�����󣬲�ͬ����
    /// </summary>
    public void AutoSyncMyCreditToReel()
    {
        DoCor(COR_AUTO_SYNC_MY_REAL_CREDIT, _AutoSyncMyCreditToReal());
    }

    IEnumerator _AutoSyncMyCreditToReal()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (TrySyncMyCreditToReel())
                break;
        }
    }


    /// <summary>
    /// ֱ��ͬ������ʵ��ҽ��,��߲��Ŷ����߼��Ͻ��
    /// </summary>
    /// <param name="minusCredit"></param>
    public void AddOrSyncMyCreditToReal(long addCredit)
    {
        if (TrySyncMyCreditToReel())
            return;

        // my_ui_credit �ڲ��Ŷ���ʱ��my_ui_credit �߲��Ŷ������߼��Ͻ��
        AddMyTempCredit(addCredit, true, true);
    }

    /// <summary>
    /// ֱ��ͬ������ʵ��ҽ��,��߲��Ŷ����߼�ȥ���
    /// </summary>
    /// <param name="minusCredit"></param>
    public void MinusOrSyncMyCreditToReal(long minusCredit)
    {
        if (TrySyncMyCreditToReel())
            return;

        // my_ui_credit �ڲ��Ŷ���ʱ��my_ui_credit �߲��Ŷ������߼�ȥ���
        MinusMyTempCredit(minusCredit,true,true);
    }



    /// <summary>
    /// ������ҽ������
    /// </summary>
    /// <param name="credit">Ҫ���ٵĽ��</param>
    /// <param name="isEvent">�Ƿ����¼�</param>
    /// <param name="isAnim">�Ƿ���Ҫ��Ǯ����</param>
    public void AddMyTempCredit(long credit, bool isEvent = true , bool isAnim = false)
    {
        long fromCredit = _mainBB.Instance.myCredit;
        _mainBB.Instance.myCredit += credit;


        if (isEvent)
        {

            /* 
            EventCenter.Instance.EventTrigger<EventData>(MetaUIEvent.ON_CREDIT_EVENT,
                new EventData<bool>(MetaUIEvent.UpdateNaviCredit, true));
            */
            // ͣ������Ǯ�����������õ���ǰ���
            EventCenter.Instance.EventTrigger<EventData>(MetaUIEvent.ON_CREDIT_EVENT,
                new EventData<UpdateNaviCredit>(MetaUIEvent.UpdateNaviCredit,
                new UpdateNaviCredit()
                {
                    isAnim = SyncCreditAnim(isAnim),
                    fromCredit = fromCredit,    
                    toCredit = _mainBB.Instance.myCredit
                }));
        }

    }

    // ��Ǯ��ͬ��

    
    /// <summary>
    /// ͬ������ҵ���ʵ���
    /// </summary>
    /// <param name="isEvent">�Ƿ����¼�</param>
    public void SyncMyTempCreditToReal(bool isEvent)
    {
        _mainBB.Instance.myCredit = _consoleBB.Instance.myCredit;

        if (isEvent)
        {

            // ֹ֮ͣǰ�ġ���Ǯ��������ͬ������ǰ�Ľ��
            EventCenter.Instance.EventTrigger<EventData>(MetaUIEvent.ON_CREDIT_EVENT,
                new EventData<UpdateNaviCredit>(MetaUIEvent.UpdateNaviCredit,
                new UpdateNaviCredit()
                {
                    isAnim = SyncCreditAnim(false),
                    toCredit = _mainBB.Instance.myCredit
                }));
        }
    }

    /// <summary>
    /// ͬ�������µ���ҽ��
    /// </summary>
    public void SyncMyUICreditToTemp()
    {

        // ֹ֮ͣǰ�ġ���Ǯ��������ͬ������ǰ�Ľ��
        EventCenter.Instance.EventTrigger<EventData>(MetaUIEvent.ON_CREDIT_EVENT,
            new EventData<UpdateNaviCredit>(MetaUIEvent.UpdateNaviCredit,
            new UpdateNaviCredit()
            {
                isAnim = SyncCreditAnim(false),
                toCredit = _mainBB.Instance.myCredit
            }));  
    }



    /// <summary>
    /// �����ʱ���
    /// </summary>
    /// <param name="isEvent">�Ƿ����¼�ͬ�����UI���</param>
    public void SetMyTempCredit(long credit, bool isEvent = true) {

        _mainBB.Instance.myCredit = credit;

        if (isEvent)
        {

            // ֹ֮ͣǰ�ġ���Ǯ��������ͬ������ǰ�Ľ��
            EventCenter.Instance.EventTrigger<EventData>(MetaUIEvent.ON_CREDIT_EVENT,
                new EventData<UpdateNaviCredit>(MetaUIEvent.UpdateNaviCredit,
                new UpdateNaviCredit()
                {
                    isAnim = SyncCreditAnim(false),
                    toCredit = _mainBB.Instance.myCredit
                }));
 
        }
    }




    /// <summary>
    /// ��ÿ����Ϸ��š�����
    /// </summary>
    public void ClearGameNumber()
    {
        _mainBB.Instance.gameNumberNode = null;
        SQLitePlayerPrefs03.Instance.SetString(_mainBB.PARAM_GAME_NUMBER, "{}");
    }

    /// <summary>
    /// ��ÿ����Ϸ�ϱ���š�����
    /// </summary>
    public void ClearReportId()
    {
        _mainBB.Instance.reportIdNode = null;
        SQLitePlayerPrefs03.Instance.SetString(_mainBB.PARAM_REPORT_ID, "{}");
    }

}
