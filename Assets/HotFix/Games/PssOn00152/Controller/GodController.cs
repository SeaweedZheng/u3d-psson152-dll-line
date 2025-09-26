using GameMaker;
using System.Collections.Generic;
using SlotMaker;
public class GodController : AnimBaseUI
{
    const string WIN = "win";
    const string IDLE = "idle";

    MessageDelegates onWinEventDelegates;
    MessageDelegates onPanelEventDelegates;
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

        onWinEventDelegates = new MessageDelegates
         (
             new Dictionary<string, EventDelegate>
             {
                //{ SlotMachineEvent.SkipWin, OnSkipWin},
                { SlotMachineEvent.TotalWinCredit, OnTotalWinCredit},
             }
         );


        EventCenter.Instance.AddEventListener<EventData>(SlotMachineEvent.ON_SLOT_EVENT, OnSlotEvent);
        EventCenter.Instance.AddEventListener<EventData>(SlotMachineEvent.ON_WIN_EVENT, onWinEventDelegates.Delegate);
        EventCenter.Instance.AddEventListener<EventData>(PanelEvent.ON_PANEL_INPUT_EVENT, onPanelEventDelegates.Delegate);
    }

    private void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener<EventData>(SlotMachineEvent.ON_SLOT_EVENT, OnSlotEvent);
        EventCenter.Instance.RemoveEventListener<EventData>(SlotMachineEvent.ON_WIN_EVENT, onWinEventDelegates.Delegate);
        EventCenter.Instance.RemoveEventListener<EventData>(PanelEvent.ON_PANEL_INPUT_EVENT, onPanelEventDelegates.Delegate);
    }

    protected override void OnValueChagne(string state)
    {
        switch (state)
        {
            case IDLE:
                Play("idle", true);
                break;
            case WIN:
                Play("win_loop", true);
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

    void OnTotalWinCredit(EventData receivedEvent = null)
    {
        state = WIN;
    }

    void OnSlotEvent(EventData receivedEvent = null)
    {
        // 滚轮开始转时，财神进入空闲状态
        if (receivedEvent.name == SlotMachineEvent.SpinSlotMachine)
        {
            state = IDLE;
        }
    }

}
