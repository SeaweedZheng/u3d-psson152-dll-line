using GameMaker;
using System.Collections.Generic;
using SlotMaker;

public class GodFreeSpinController : AnimBaseUI
{
    const string WIN = "State Win";
    const string IDLE = "State Idle";

    MessageDelegates onWinEventDelegates;
    MessageDelegates onPanelEventDelegates;
    MessageDelegates onSlotEventDelegates;

    void Start()
    {
        onPanelEventDelegates = new MessageDelegates
         (
             new Dictionary<string, EventDelegate>
             {
                { PanelEvent.SpinButtonClick, OnSpinButtonClick},
                //{ SlotMachineEvent.TotalWinLine, OnTotalWinLine},
             }
         );

        onSlotEventDelegates = new MessageDelegates
         (
             new Dictionary<string, EventDelegate>
             {
                { SlotMachineEvent.SpinSlotMachine, OnSpinSlotMachine},
             }
         );

        onWinEventDelegates = new MessageDelegates
         (
             new Dictionary<string, EventDelegate>
             {
                //{ SlotMachineEvent.SkipWin, OnSkipWin},
                { SlotMachineEvent.TotalWinCredit, OnTotalWinCredit},
             }
         );

        EventCenter.Instance.AddEventListener<EventData>(SlotMachineEvent.ON_WIN_EVENT, onWinEventDelegates.Delegate);
        EventCenter.Instance.AddEventListener<EventData>(PanelEvent.ON_PANEL_INPUT_EVENT, onPanelEventDelegates.Delegate);

        EventCenter.Instance.AddEventListener<EventData>(SlotMachineEvent.ON_SLOT_EVENT, onSlotEventDelegates.Delegate);
    }

    private void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener<EventData>(SlotMachineEvent.ON_WIN_EVENT, onWinEventDelegates.Delegate);
        EventCenter.Instance.RemoveEventListener<EventData>(PanelEvent.ON_PANEL_INPUT_EVENT, onPanelEventDelegates.Delegate);
        
        EventCenter.Instance.RemoveEventListener<EventData>(SlotMachineEvent.ON_SLOT_EVENT, onSlotEventDelegates.Delegate);
    }

    protected override void OnValueChagne(string state)
    {
        switch (state)
        {
            case IDLE:
                Play(IDLE, true);
                break;
            case WIN:
                Play(WIN, true);
                break;
        }
    }

    private void OnEnable()
    {
        state = IDLE;
    }
    void OnSpinButtonClick(EventData receivedEvent = null)
    {
        state = IDLE;
    }

    void OnSpinSlotMachine(EventData receivedEvent = null)
    {
        state = IDLE;
    }

    void OnTotalWinCredit(EventData receivedEvent = null)
    {
        state = WIN;
    }

}
