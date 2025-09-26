using GameMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _consoleBB = PssOn00152.ConsoleBlackboard02;
public class DeviceSasReport : MonoSingleton<DeviceSasReport>
{

    MessageDelegates coinInOutEventDelegates;
    void Start()
    {
        coinInOutEventDelegates = new MessageDelegates
        (
            new Dictionary<string, EventDelegate>
            {
                            //{ PanelEvent.OnLongClickSpinButton, OnClickSpinButton},
                            //{ PanelEvent.OnClickSpinButton, OnClickSpinButton},
                            //{ PanelEvent.SpinButtonClick, OnClickSpinButton},
                            //{ PanelEvent.RedeemButtonClick,OnClickRedeemButton}

                { GlobalEvent.CoinInCompleted, OnCoinInCompleted},
                { GlobalEvent.CoinOutCompleted, OnCoinOutCompleted},

            }
        );
    }

    // Update is called once per frame
    void OnEnable()
    {
        EventCenter.Instance.AddEventListener<EventData>(GlobalEvent.ON_COIN_IN_OUT_EVENT, coinInOutEventDelegates.Delegate);
    }


    private void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener<EventData>(GlobalEvent.ON_COIN_IN_OUT_EVENT, coinInOutEventDelegates.Delegate);

    }


    void OnCoinInCompleted(EventData res)
    {
        SasCommand.Instance.SetMeterTotalCoinIn((int)_consoleBB.Instance.historyTotalCoinInCredit);
    }

    void OnCoinOutCompleted(EventData res)
    {
        SasCommand.Instance.SetMeterTotalCoinOut((int)_consoleBB.Instance.historyTotalCoinOutCredit);
    }


    void OnGameIdle(EventData res)
    {
        SasCommand.Instance.SetMeterMyCredit((int)_consoleBB.Instance.myCredit);
        SasCommand.Instance.SetMeterTotalJcakpotCredits(10001);
    }


    void OnGameIdle01(EventData res)
    {
        SasCommand.Instance.SetMeterTotalTicketInCredits(40001);
        SasCommand.Instance.SetMeterTotalTicketOutCredits(50001);
    }


    void OnAllReoprt(EventData res)
    {
        SasCommand.Instance.SetMeterMyCredit((int)_consoleBB.Instance.myCredit);
        SasCommand.Instance.SetMeterTotalCoinIn((int)_consoleBB.Instance.historyTotalCoinInCredit);
        SasCommand.Instance.SetMeterTotalCoinOut((int)_consoleBB.Instance.historyTotalCoinOutCredit);

        SasCommand.Instance.SetMeterTotalJcakpotCredits(10001);
        SasCommand.Instance.SetMeterTotalGamePlayed(20001);

        SasCommand.Instance.SetMeterTotalTicketInCredits(40001);
        SasCommand.Instance.SetMeterTotalTicketOutCredits(50001);

        SasCommand.Instance.SetMeterTotalHeadPaidCredits(30001);

        SasCommand.Instance.SetMeterTotalElectronicTransfersToMachine(60001);
        SasCommand.Instance.SetMeterTotalElectronicTransfersToHost(70001);
        SasCommand.Instance.SetMeterTotalCreditsFromBills(80001);
        SasCommand.Instance.SetMeterBillInCash(50, 333, 100);
    }


    public void OnGameStart()
    {
        if (!_consoleBB.Instance.isSasConnect) return;

        SasCommand.Instance.SetMeterGameStart();
    }

    public void OnGameEnd()
    {
        if (!_consoleBB.Instance.isSasConnect) return;

        SasCommand.Instance.SetMeterGameEnd();
    }
}
