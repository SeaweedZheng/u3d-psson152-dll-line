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
/// * 玩家真实金额
/// * 玩家临时金额
/// * 玩家UI显示金额
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

    /// <summary> 玩家临时金币  </summary>
    public long myTempCredit
    {
        get => _mainBB.Instance.myCredit;
    }

    /// <summary> 玩家真实金币  </summary>
    public long myRealCredit
    {
        get => _consoleBB.Instance.myCredit;
    }

    /// <summary>
    /// 加钱动画标志位
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
    /// 设置玩家真实金币
    /// </summary>
    /// <param name="crefit"></param>
    public void SetMyRealCredit(long crefit)
    {
        _consoleBB.Instance.myCredit = crefit;
    }

    /// <summary>
    /// 当下玩家金额减少
    /// </summary>
    /// <param name="credit">要减少的金额</param>
    /// <param name="isEvent">是否发送事件</param>
    /// <param name="isAnim">是否需要加钱动画</param>
    public void MinusMyTempCredit(long credit, bool isEvent = true, bool isAnim = false)
    {
        long fromCredit = _mainBB.Instance.myCredit;
        _mainBB.Instance.myCredit -= credit;

        if (isEvent)
        {
            

            /* 停掉“加钱动画”，设置到当前金额
            EventCenter.Instance.EventTrigger<EventData>(MetaUIEvent.ON_CREDIT_EVENT,
                new EventData<bool>(MetaUIEvent.UpdateNaviCredit, true));
            */

            // 停掉“加钱动画”，设置到当前金额
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
    /// 同步“Temp金额”到“真实金额”
    /// </summary>
    /// <returns></returns>
    /// <remarks>
    /// * 在没有“加减钱动画”和玩游中，进行同步！<br/>
    /// * 同步成功，并通知UI刷新<br/>
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

        SyncMyTempCreditToReal(true);// 允许同步
        return true;
    }

    /// <summary>
    /// 自动同步玩家金额（等待游戏结束后，才同步）
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
    /// 直接同步到真实玩家金币,或边播放动画边加上金额
    /// </summary>
    /// <param name="minusCredit"></param>
    public void AddOrSyncMyCreditToReal(long addCredit)
    {
        if (TrySyncMyCreditToReel())
            return;

        // my_ui_credit 在播放动画时，my_ui_credit 边播放动画，边加上金额
        AddMyTempCredit(addCredit, true, true);
    }

    /// <summary>
    /// 直接同步到真实玩家金币,或边播放动画边减去金额
    /// </summary>
    /// <param name="minusCredit"></param>
    public void MinusOrSyncMyCreditToReal(long minusCredit)
    {
        if (TrySyncMyCreditToReel())
            return;

        // my_ui_credit 在播放动画时，my_ui_credit 边播放动画，边减去金额
        MinusMyTempCredit(minusCredit,true,true);
    }



    /// <summary>
    /// 当下玩家金额增加
    /// </summary>
    /// <param name="credit">要减少的金额</param>
    /// <param name="isEvent">是否发送事件</param>
    /// <param name="isAnim">是否需要加钱动画</param>
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
            // 停掉“加钱动画”，设置到当前金额
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

    // 加钱并同步

    
    /// <summary>
    /// 同步到玩家的真实金额
    /// </summary>
    /// <param name="isEvent">是否发送事件</param>
    public void SyncMyTempCreditToReal(bool isEvent)
    {
        _mainBB.Instance.myCredit = _consoleBB.Instance.myCredit;

        if (isEvent)
        {

            // 停止之前的“加钱动画”，同步到当前的金额
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
    /// 同步到当下的玩家金额
    /// </summary>
    public void SyncMyUICreditToTemp()
    {

        // 停止之前的“加钱动画”，同步到当前的金额
        EventCenter.Instance.EventTrigger<EventData>(MetaUIEvent.ON_CREDIT_EVENT,
            new EventData<UpdateNaviCredit>(MetaUIEvent.UpdateNaviCredit,
            new UpdateNaviCredit()
            {
                isAnim = SyncCreditAnim(false),
                toCredit = _mainBB.Instance.myCredit
            }));  
    }



    /// <summary>
    /// 玩家临时金额
    /// </summary>
    /// <param name="isEvent">是否发送事件同步玩家UI金额</param>
    public void SetMyTempCredit(long credit, bool isEvent = true) {

        _mainBB.Instance.myCredit = credit;

        if (isEvent)
        {

            // 停止之前的“加钱动画”，同步到当前的金额
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
    /// “每局游戏编号”归零
    /// </summary>
    public void ClearGameNumber()
    {
        _mainBB.Instance.gameNumberNode = null;
        SQLitePlayerPrefs03.Instance.SetString(_mainBB.PARAM_GAME_NUMBER, "{}");
    }

    /// <summary>
    /// “每局游戏上报编号”归零
    /// </summary>
    public void ClearReportId()
    {
        _mainBB.Instance.reportIdNode = null;
        SQLitePlayerPrefs03.Instance.SetString(_mainBB.PARAM_REPORT_ID, "{}");
    }

}
