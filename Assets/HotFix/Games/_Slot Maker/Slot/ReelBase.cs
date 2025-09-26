using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ReelState
{
    /// <summary> 空闲中 </summary>
    Idle,
    /// <summary> 开始滚动 </summary>
    StartTurn,
    /// <summary> 开始停止滚动 </summary>
    StartStop,
    /// <summary> 滚动结束 </summary>
    EndStop,
}


public class ReelBase : CorBehaviour
{

    /// <summary> 滚轮索引号 </summary>
    public int reelIndex = 0;

    /// <summary> 滚轮状态 </summary>
    public ReelState state = ReelState.Idle;

    public List<SymbolBase> symbolList;


    public virtual void SetReelDeck(string reelValue = null)
    {
        DebugUtils.LogWarning("==@ BaseReel - SetDeckReel");
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="rollTime"> 滚动圈数 </param>
    /// <param name="action"> 完成回调 </param>
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
