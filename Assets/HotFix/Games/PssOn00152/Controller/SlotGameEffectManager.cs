using GameMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using _reelSetBB = SlotMaker.ReelSettingBlackboard;
using _spinWEBB = SlotMaker.SpinWinEffectSettingBlackboard;
using _contentBB = PssOn00152.ContentBlackboard;
using _consoleBB = PssOn00152.ConsoleBlackboard02;



public enum SlotGameEffect
{
    Default,
    StopImmediately,
    AutoSpin,
    FreeSpin,
    Expectation01,
    GameIdle,
}


/// <summary>
/// 拉霸游戏特效管理
/// </summary>
/// <remarks>
/// * 管理滚轮特效
/// * 管理中奖特效
/// </remarks>
public class SlotGameEffectManager : MonoSingleton<SlotGameEffectManager>
{

    public void SetEffect(SlotGameEffect state)
    {
        SetSpinWinEffect(state);
        SetReelRunEffect(state);

    }


    void SetSpinWinEffect(SlotGameEffect state)
    {
        if (state == SlotGameEffect.GameIdle)
        {
            _spinWEBB.Instance.SelectData(_spinWEBB.SPIN_WIN_EFFECT_IDLE);
            return;
        }

        if (state == SlotGameEffect.StopImmediately)
        {
            _spinWEBB.Instance.SelectData(_spinWEBB.SPIN_WIN_EFFECT_STOP_IMMEDIATELY);
            return;
        }

        if (state == SlotGameEffect.FreeSpin)
        {
            _spinWEBB.Instance.SelectData(_spinWEBB.SPIN_WIN_EFFECT_FREE_SPIN);
            return;
        }

        if (_contentBB.Instance.isAuto)
        {
            _spinWEBB.Instance.SelectData(_spinWEBB.SPIN_WIN_EFFECT_AUTO);
            return;
        }

        _spinWEBB.Instance.SelectData(_spinWEBB.SPIN_WIN_EFFECT_DEFAULT);
    }

    void SetReelRunEffect(SlotGameEffect state)
    {
        if (state == SlotGameEffect.StopImmediately)
        {
            _reelSetBB.Instance.SelectData(_reelSetBB.REEL_SETTING_STOP);
            return;
        }

        if (state == SlotGameEffect.Expectation01)
        {
            _reelSetBB.Instance.SelectData(_reelSetBB.REEL_SETTING_SLOW_MOTION);
            return;
        }

        _reelSetBB.Instance.SelectData(_reelSetBB.REEL_SETTING_REGULAR);
    }


}
