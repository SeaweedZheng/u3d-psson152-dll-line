using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ReelState
{
    /// <summary> ������ </summary>
    Idle,
    /// <summary> ��ʼ���� </summary>
    StartTurn,
    /// <summary> ��ʼֹͣ���� </summary>
    StartStop,
    /// <summary> �������� </summary>
    EndStop,
}


public class ReelBase : CorBehaviour
{

    /// <summary> ���������� </summary>
    public int reelIndex = 0;

    /// <summary> ����״̬ </summary>
    public ReelState state = ReelState.Idle;

    public List<SymbolBase> symbolList;


    public virtual void SetReelDeck(string reelValue = null)
    {
        DebugUtils.LogWarning("==@ BaseReel - SetDeckReel");
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="rollTime"> ����Ȧ�� </param>
    /// <param name="action"> ��ɻص� </param>
    public virtual void StartTurn(int rollTime, UnityAction action)
    {
        DebugUtils.LogWarning("==@ BaseReel - StartRoll");
    }

    public virtual void ReelToStopOrTurnOnce(UnityAction action)
    {
        DebugUtils.LogWarning("==@ BaseReel - ReelToStopOrTurnOnce");
    }

    public virtual void SetResult(List<int> result)
    {
        DebugUtils.LogWarning("==@ BaseReel - SetResult");
    }




    /////////////////////
    public virtual void SymbolAppearEffect()
    {
        DebugUtils.LogWarning("==@ BaseReel - SpecialSymbolEffect");
    }

    public virtual void SetReelState(ReelState state = ReelState.Idle)
    {
        DebugUtils.LogWarning("==@ BaseReel - SetReelState");
    }
}
