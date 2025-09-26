using SlotMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SlotMaker
{

    public enum SpinWinEffect
    {
        /// <summary> �߿� </summary>
        Frame,
        /// <summary> ��ʾ�� </summary>
        Line,
        /// <summary> ͼ���� </summary>
        Bigger,
        /// <summary> ���� </summary>
        Anim,
        /// <summary> ����Ӯ </summary>
        TotalWinLine,
        /// <summary> ����Ӯ </summary>
        SingleWinLine,
        /// <summary> �÷� </summary>
        Credit,
        /// <summary> ���� </summary>
        Cover,
        /// <summary> ����ԭͼ�� </summary>
        HideBaseSymbol,


        /// <summary> ����ֹͣ����ʱ�������Ž�� </summary>
        SkipAtStopImmediately


        // ShowUntilSpinButtonClick
        // totalWinLine
        // singleWinLine
        // sameSymbolWin
    }

    public class SpinWinEffectSettingBlackboard : MonoWeakSelectSingleton<SpinWinEffectSettingBlackboard>
    {
        /// <summary> Ĭ��spin�н�Ч�� </summary>
        public const string SPIN_WIN_EFFECT_DEFAULT = "Spin Win Effect Setting Default";

        /// <summary>��Ϸ����ʱ��spin�н�Ч�� </summary>
        public const string SPIN_WIN_EFFECT_IDLE = "Spin Win Effect Setting Idle";

        /// <summary>��������ͣ��spin�н�Ч�� </summary>
        public const string SPIN_WIN_EFFECT_STOP_IMMEDIATELY = "Spin Win Effect Setting Stop Immediately";

        /// <summary>�����Զ�����spin�н�Ч�� </summary>
        public const string SPIN_WIN_EFFECT_AUTO = "Spin Win Effect Setting Auto";

        /// <summary>���������Ϸ��spin�н�Ч�� </summary>
        public const string SPIN_WIN_EFFECT_FREE_SPIN = "Spin Win Effect Setting Free Spin";

        /// <summary>�ı�ͼ��</summary>
        public const string SPIN_WIN_EFFECT_CHANGE_SYMBOL = "Spin Win Effect Change Symbol";


        /// <summary>�����Ϸ����</summary>
        public const string SPIN_WIN_EFFECT_FREE_SPIN_TRIGGER  = "Spin Win Effect Free Spin Trigger";

        public List<SpinWinEffect> winEffectSetting = new List<SpinWinEffect> { 
            SpinWinEffect.Cover, SpinWinEffect.Credit, SpinWinEffect.Frame, 
            SpinWinEffect.SingleWinLine, SpinWinEffect.Bigger};


        public bool isFrame => winEffectSetting.Contains(SpinWinEffect.Frame);

        public bool isBigger => winEffectSetting.Contains(SpinWinEffect.Bigger);

        public bool isShowLine => winEffectSetting.Contains(SpinWinEffect.Line);

        public bool isHideBaseIcon => winEffectSetting.Contains(SpinWinEffect.HideBaseSymbol);

        public bool isShowCover => winEffectSetting.Contains(SpinWinEffect.Cover);

        public bool isSymbolAnim => winEffectSetting.Contains(SpinWinEffect.Anim);

        public bool isShowWinCredit => winEffectSetting.Contains(SpinWinEffect.Credit);

        public bool isTotalWin => winEffectSetting.Contains(SpinWinEffect.TotalWinLine);

        public bool isSingleWin => winEffectSetting.Contains(SpinWinEffect.SingleWinLine);

        public bool isSkipAtStopImmediately => winEffectSetting.Contains(SpinWinEffect.SkipAtStopImmediately);

        //public float minTimeS = 3f;//
        public float timeS = 0.8f;//����ʱ�� ����ʱ: 3f  # ���Ž������: 0.8f ��

    }



    /*
List<SpinWinEffect> weDefaultSpin = new List<SpinWinEffect> {SpinWinEffect.Cover, SpinWinEffect.Credit, SpinWinEffect.Frame, SpinWinEffect.SingleWin, SpinWinEffect.Bigger};
List<SpinWinEffect> weDefaultSpinStopImmediately = new List<SpinWinEffect> { SpinWinEffect.Cover, SpinWinEffect.Credit, SpinWinEffect.Frame, SpinWinEffect.SingleWin, SpinWinEffect.Bigger };
List<SpinWinEffect> weNormalSpin;
List<SpinWinEffect> weNormalSpinIdel;
List<SpinWinEffect> weNormalSpinStopImmediately;
List<SpinWinEffect> weNormalSpinAuto;
List<SpinWinEffect> weFreeSpin;
*/
}