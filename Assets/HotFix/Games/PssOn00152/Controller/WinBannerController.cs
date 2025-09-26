using GameMaker;
using PssOn00152;
using Sirenix.OdinInspector;
using SlotMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using _gameSoundHelper = PssOn00152.GameSoundHelper;
using _soundKey = PssOn00152.SoundKey;
using _contentBB = PssOn00152.ContentBlackboard;
//ChinaTown100
public class WinBannerController : CorBehaviour
{

    public bool isShowTotalWin = false;
    public GameObject goTurn, goTotalWin, goEffect,
        goFreeSpin, goFreeSpinAdd, goFreeSpinMax;
    

    Transform tfmTotalWinAnchor , tfmFreeSpinAnchor;
    Text txtTotalWin;


    MessageDelegates onWinEventDelegates;

    MessageDelegates onContentEventDelegates;


    const string COR_SHOW_TOTAL_WIN = "COR_SHOW_TOTAL_WIN";
    const string COR_FREE_SPIN_ADD = "COR_FREE_SPIN_ADD";
    private void Awake()
    {
        tfmTotalWinAnchor = goTotalWin.transform.Find("Anchor");
        txtTotalWin = tfmTotalWinAnchor.Find("Text").GetComponent<Text>();

        tfmFreeSpinAnchor = goFreeSpin.transform.Find("Anchor");


        onWinEventDelegates = new MessageDelegates
        (
            new Dictionary<string, EventDelegate>
            {
                { SlotMachineEvent.PrepareTotalWinCredit, OnPrepareTotalWinCredit },
                { SlotMachineEvent.PrepareTotalWinCredit02, OnPrepareTotalWinCredit02 },
            }
        );

        onContentEventDelegates = new MessageDelegates
        (
            new Dictionary<string, EventDelegate>
            {
                { SlotMachineEvent.BeginBonus, OnBeginBonus },
                { SlotMachineEvent.EndBonus, OnEndBonus },
            }
        );
    }




    private void OnEnable()
    {
        EventCenter.Instance.AddEventListener<EventData>(SlotMachineEvent.ON_WIN_EVENT, onWinEventDelegates.Delegate);
        EventCenter.Instance.AddEventListener<EventData>(SlotMachineEvent.ON_CONTENT_EVEN, onContentEventDelegates.Delegate);

        //EventCenter.Instance.AddEventListener<EventData>(PanelEvent.ON_PANEL_INPUT_EVENT, OnEvnetEndScaleRoll);
        EventCenter.Instance.AddEventListener<EventData>(SlotMachineEvent.ON_SLOT_EVENT, OnEvnetEndScaleRoll);

        InitParam();
    }


    private void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener<EventData>(SlotMachineEvent.ON_WIN_EVENT, onWinEventDelegates.Delegate);
        EventCenter.Instance.RemoveEventListener<EventData>(SlotMachineEvent.ON_CONTENT_EVEN, onContentEventDelegates.Delegate);

        //EventCenter.Instance.RemoveEventListener<EventData>(PanelEvent.ON_PANEL_INPUT_EVENT, OnEvnetEndScaleRoll);
        EventCenter.Instance.RemoveEventListener<EventData>(SlotMachineEvent.ON_SLOT_EVENT, OnEvnetEndScaleRoll);
    }

    void InitParam()
    {

        goTotalWin.SetActive(false);
        goTurn.SetActive(true);
    }


    void OnEvnetEndScaleRoll(EventData data) {

        if (data.name == PanelEvent.SpinButtonClick || data.name == SlotMachineEvent.SpinSlotMachine)
        {
            if (IsCor(COR_SHOW_TOTAL_WIN) && stateTotalWin == TotalWinState.ScaleRoll)
            {
                DoCor(COR_SHOW_TOTAL_WIN, ShowTotalWinScaleRollEnd(totalWinCredit));
            }
            /*else if (IsCor(COR_SYNC_TOTAL_WIN_CREDIT) && stateTotalWin == TotalWinState.ScaleRoll)
            {

            }*/
        }
    }

    long totalWinCredit = 0;


    #region 特效：同步滚动的totalwin金额
    void OnPrepareTotalWinCredit02(EventData receivedEvent)
    {
        PrepareTotalWinCredit02 res = receivedEvent.value as PrepareTotalWinCredit02;

        DoCor(COR_SYNC_TOTAL_WIN_CREDIT, ShowTotalWinScaleRoll02(res.credit, res.isEndToCredit));
    }

    const string COR_SYNC_TOTAL_WIN_CREDIT = "COR_SYNC_TOTAL_WIN_CREDIT";
    IEnumerator ShowTotalWinScaleRoll02(long totalWin,bool isEndToTotalWinCredit)
    {
        if (stateTotalWin ==  TotalWinState.ScaleRoll && isEndToTotalWinCredit)
            stateTotalWin = TotalWinState.ScaleRollEnd;

        switch (stateTotalWin)
        {
            case TotalWinState.None:
                {
                    stateTotalWin = TotalWinState.ScaleRoll;
                    goTotalWin.SetActive(true);
                    goTurn.SetActive(false);
                    goEffect.SetActive(false);
                    _gameSoundHelper.Instance.PlaySoundSingle(_soundKey.WinRolling);
                    txtTotalWin.text = (totalWin).ToString();
                }
                break;
            case TotalWinState.ScaleRoll:
                {
                    txtTotalWin.text = (totalWin).ToString();
                }
                break;
            case TotalWinState.ScaleRollEnd:
                {
                    txtTotalWin.text = (totalWin).ToString();
                    _gameSoundHelper.Instance.StopSound(_soundKey.WinRolling);

                    goEffect.SetActive(true);
                    //渐变
                    float scale = 1f;
                    while (scale < 1.2f)
                    {
                        tfmTotalWinAnchor.localScale = new Vector3(scale, scale, 1f);
                        scale += 0.1f;
                        yield return new WaitForSeconds(0.04f);
                    }

                    //yield return new WaitForSeconds(0.2f);

                    //渐变
                    scale = 1.2f;
                    while (scale >= 1f)
                    {
                        tfmTotalWinAnchor.localScale = new Vector3(scale, scale, 1f);
                        scale -= 0.1f;
                        yield return new WaitForSeconds(0.05f);
                    }

                    tfmTotalWinAnchor.localScale = new Vector3(1f, 1f, 1f);

                    yield return new WaitForSeconds(1.5f);

                    goTotalWin.SetActive(false);
                    goTurn.SetActive(true);
                    goEffect.SetActive(false);
                    stateTotalWin = TotalWinState.None;
                }
                break;
        }
    }

    #endregion


    void OnPrepareTotalWinCredit(EventData receivedEvent)
    {
        PrepareTotalWinCredit res = (PrepareTotalWinCredit)receivedEvent.value;

        if (!isShowTotalWin)
            return;

        goEffect.SetActive(false);

        totalWinCredit = res.totalWinCredit;

        if (res.isAddToCredit)
        {
            DoCor(COR_SHOW_TOTAL_WIN, ShowTotalWinScaleRoll(res.totalWinCredit));
        }
        else
        {
            DoCor(COR_SHOW_TOTAL_WIN, ShowTotalWin(res.totalWinCredit));
        }

    }


    [Button]
    void TestTotalWinLine()
    {
        DoCor(COR_SHOW_TOTAL_WIN, ShowTotalWinScaleRoll(1456L));
    }
    IEnumerator ShowTotalWin(long totalWin)
    {
        stateTotalWin = TotalWinState.Scale;

        goTotalWin.SetActive(true);
        goTurn.SetActive(false);

        txtTotalWin.text = totalWin.ToString(); // 不要千分号  totalWin.ToString("N0"); // totalWin.ToString("N2");
        goEffect.SetActive(true);

        _gameSoundHelper.Instance.PlaySoundSingle(_soundKey.TotalWinLine);

        //渐变
        float scale = 1f;
        while (scale < 1.2f)
        {
            tfmTotalWinAnchor.localScale = new Vector3(scale, scale, 1f);
            scale += 0.1f;
            yield return new WaitForSeconds(0.04f);
        }

        //yield return new WaitForSeconds(0.2f);

        //渐变
        scale = 1.2f;
        while (scale >= 1f)
        {
            tfmTotalWinAnchor.localScale = new Vector3(scale, scale, 1f);
            scale -= 0.1f;
            yield return new WaitForSeconds(0.05f);
        }

        tfmTotalWinAnchor.localScale = new Vector3(1f, 1f, 1f);

        yield return new WaitForSeconds(1.5f);

        goTotalWin.SetActive(false);
        goTurn.SetActive(true);
        goEffect.SetActive(false);
        stateTotalWin = TotalWinState.None;
    }



    #region 数字带滚动

    IEnumerator ShowTotalWinScaleRollEnd(long totalWin)
    {
        stateTotalWin = TotalWinState.ScaleRollEnd;

        _gameSoundHelper.Instance.StopSound(_soundKey.WinRolling);
        goTotalWin.SetActive(true);
        goTurn.SetActive(false);
        txtTotalWin.text = totalWin.ToString(); // 不要千分号  totalWin.ToString("N0"); // totalWin.ToString("N2");
        tfmTotalWinAnchor.localScale = new Vector3(1f, 1f, 1f);
        goEffect.SetActive(true);

        _gameSoundHelper.Instance.PlaySoundSingle(_soundKey.TotalWinLine);

        yield return new WaitForSeconds(0.5f);

        goTotalWin.SetActive(false);
        goTurn.SetActive(true);
        goEffect.SetActive(false);
        stateTotalWin = TotalWinState.None;
    }


    IEnumerator ShowTotalWinScaleRoll(long totalWin)
    {
        stateTotalWin = TotalWinState.ScaleRoll;

        goTotalWin.SetActive(true);
        goTurn.SetActive(false);
        goEffect.SetActive(false);

        _gameSoundHelper.Instance.PlaySoundSingle(_soundKey.WinRolling);

        long fromCredit = (long)(totalWin * 0.5);

        if (totalWin - fromCredit > 100)
            fromCredit = totalWin - 100;

        while (true)
        {
            fromCredit++;
            if (fromCredit < totalWin)
                txtTotalWin.text = (fromCredit).ToString(); // 不要千分号   (fromCredit).ToString("N0");
            else
                break;
            yield return new WaitForSeconds(0.01f);

        }
        _gameSoundHelper.Instance.StopSound(_soundKey.WinRolling);


        txtTotalWin.text = (totalWin).ToString(); // 不要千分号    (totalWin).ToString("N0");
        goEffect.SetActive(true);

        

        _gameSoundHelper.Instance.PlaySoundSingle(_soundKey.TotalWinLine);
        //渐变
        float scale = 1f;
        while (scale < 1.2f)
        {
            tfmTotalWinAnchor.localScale = new Vector3(scale, scale, 1f);
            scale += 0.1f;
            yield return new WaitForSeconds(0.04f);
        }

        //yield return new WaitForSeconds(0.2f);

        //渐变
        scale = 1.2f;
        while (scale >= 1f)
        {
            tfmTotalWinAnchor.localScale = new Vector3(scale, scale, 1f);
            scale -= 0.1f;
            yield return new WaitForSeconds(0.05f);
        }

        tfmTotalWinAnchor.localScale = new Vector3(1f, 1f, 1f);

        yield return new WaitForSeconds(1.5f);

        goTotalWin.SetActive(false);
        goTurn.SetActive(true);
        goEffect.SetActive(false);
        stateTotalWin = TotalWinState.None;
    }

    TotalWinState stateTotalWin;
    enum TotalWinState
    {
        None,
        Scale,
        ScaleRoll,
        ScaleRollEnd,
    }

#endregion


#region Free Spin


    void OnBeginBonus(EventData receivedEvent)
    {

        if((string)receivedEvent.value == "FreeSpinAdd"){
            DoCor(COR_FREE_SPIN_ADD, ShowFreeSpinAddInfo());
        }
    }
    void OnEndBonus(EventData receivedEvent)
    {
        if ((string)receivedEvent.value == "FreeSpinAdd")
        {
            ClearCor(COR_FREE_SPIN_ADD);
            goTurn.SetActive(true);
            goFreeSpin.SetActive(false);
        }
    }

    IEnumerator ShowFreeSpinAddInfo()
    {

        goTotalWin.SetActive(false);
        goTurn.SetActive(false);
        goFreeSpin.SetActive(true);

        bool isMaxFreeSpin = _contentBB.Instance.isFreeSpinMax; 
        goFreeSpinAdd.SetActive(!isMaxFreeSpin);
        goFreeSpinMax.SetActive(isMaxFreeSpin);

        //渐变
        float scale = 1f;
        while (scale < 1.2f)
        {
            tfmFreeSpinAnchor.localScale = new Vector3(scale, scale, 1f);
            scale += 0.1f;
            yield return new WaitForSeconds(0.04f);
        }

        //yield return new WaitForSeconds(0.2f);

        //渐变
        scale = 1.2f;
        while (scale >= 1f)
        {
            tfmFreeSpinAnchor.localScale = new Vector3(scale, scale, 1f);
            scale -= 0.1f;
            yield return new WaitForSeconds(0.05f);
        }

        tfmFreeSpinAnchor.localScale = new Vector3(1f, 1f, 1f);

       /* yield return new WaitForSeconds(0.8f);

        goTurn.SetActive(true);
        goFreeSpin.SetActive(false);
       */
    }

    #endregion

}
