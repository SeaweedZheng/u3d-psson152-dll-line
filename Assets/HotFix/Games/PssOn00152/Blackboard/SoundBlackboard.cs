using GameMaker;
// Blackboard
// Helper  // BB+���߷���
// Controller   // �¼�+BBʹ��+���߷���
namespace PssOn00152
{
    [System.Serializable]
    public class CustomDicSound : UnitySerializedDictionary<SoundKey, string> { }

    [System.Serializable]
    public class CustomDicSoundHandler : UnitySerializedDictionary<SoundKey, GSHandler> { }

    public enum SoundKey
    {
        /// <summary> ������Ϸ�������� </summary>
        RegularBG,
        /// <summary> �����Ϸ�������� </summary>
        FreeSpinBG,
        /// <summary> ����ʱ����������1 </summary>
        GodIdle1,
        /// <summary> ����ʱ����������2 </summary>
        GodIdle2,
        /// <summary> ����ʱ����������3 </summary>
        GodIdle3,
        /// <summary> ����ʱ����������4 </summary>
        GodIdle4,
        /// <summary> ����ʱ����������5 </summary>
        GodIdle5,
        /// <summary> ����ʱ����������6 </summary>
        GodIdle6,
        /// <summary> ����1ֹͣ </summary>
        ReelStop1,
        /// <summary> ��Ӯ�� </summary>
        TotalWinLine,

        /// <summary> 1��2��3�У�ÿ�����ֲ���ͼ�꣨���ֻ�����Ч���У� </summary>
        SlowMotionReal123MeetGod,
        /// <summary> 1��2��3�ж��в���ͼ�꣨���ֻ�����Ч���У� </summary>
        SlowMotionReal123HasGod,
        /// <summary>  1��2�г��ֲ���ͼ�꣬ף���� �����ֻ�����Ч���У� </summary>
        SlowMotionCongratulate,
        /// <summary>  ���ֻ��� </summary>
        SlowMotion,

        /// <summary> �����Ϸ�������棬�������� </summary>
        FreeSpinTriggerBG,
        /// <summary> �����Ϸ�������棬�������� </summary>
        FreeSpinResultBG,
        /// <summary> �����Ϸ�޸ı������� </summary>
        FreeSpinChangeSymbol,
        /// <summary> ���������Ϸ </summary>
        FreeSpinAdd,

        /// <summary> 5���� </summary>
        FiveLine,

        /// <summary> ����ϷӮǮ����ҹ��� </summary>
        WinRolling,

        /// <summary> �󽱵����������� </summary>
        PopupWinBG,
        /// <summary> �󽱵����ر�ǰ���� </summary>
        PopupWinBeforeClose,
        /// <summary> �󽱵���,ף���� </summary>
        PopupWinCongratulate01,
        /// <summary> �󽱵���,ף���� </summary>
        PopupWinCongratulate02,
        /// <summary> �󽱵����رպ�,ף���� </summary>
        PopupWinAfterCloseCongratulate01,
        /// <summary> �󽱵����رպ�,ף���� </summary>
        PopupWinAfterCloseCongratulate02,
        /// <summary> �� </summary>
        BigWin,
        /// <summary> �޽� </summary>
        MegaWin,
        /// <summary> ������ </summary>
        SuperWin,


        /// <summary> �ʽ𵯴�����ҹ��� </summary>
        JackpotFlow,
        /// <summary> �ʽ𵯴����������� </summary>
        JackpotBG,
        /// <summary> �ʽ𵯴��������� </summary>
        JackpotEnd,
    }

    public class SoundBlackboard : MonoWeakSingleton<SoundBlackboard>
    {
        //������·��
        //public string soundRootPath = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds";

        //�������·��
        public CustomDicSound soundRelativePath = new CustomDicSound()
        {
            [SoundKey.RegularBG] = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/BaseBackground.mp3",
            [SoundKey.GodIdle1] = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/os_idle_1.mp3",
            [SoundKey.GodIdle2] = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/os_idle_2.mp3",
            [SoundKey.GodIdle3] = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/os_idle_3.mp3",
            [SoundKey.GodIdle4] = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/os_idle_4.mp3",
            [SoundKey.GodIdle5] = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/os_idle_5.mp3",
            [SoundKey.GodIdle6] = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/os_idle_6.mp3",
            [SoundKey.ReelStop1] = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/S_R4Stop.mp3",
            [SoundKey.TotalWinLine] = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/WinRollingEnd.mp3",
            [SoundKey.SlowMotionReal123MeetGod] = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/mutiple.mp3",
            [SoundKey.SlowMotionReal123HasGod] = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/Trigger.mp3",
            [SoundKey.SlowMotionCongratulate] = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/os_slowmotion.mp3",
            [SoundKey.SlowMotion] = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/slowmotion2.mp3",

            [SoundKey.FiveLine] = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/FiveLine.mp3",
            [SoundKey.WinRolling] = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/WinRolling.mp3",

            [SoundKey.FreeSpinBG] = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/FeatureBackground.mp3",
            [SoundKey.FreeSpinTriggerBG] = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/FeatureBackground.mp3",
            [SoundKey.FreeSpinResultBG] = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/Checkbonus.mp3",
            [SoundKey.FreeSpinChangeSymbol] = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/change.mp3",
            [SoundKey.FreeSpinAdd] = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/ReTrigger.mp3",


            //[SoundKey.]="",
            [SoundKey.PopupWinBG] = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/Win.mp3",
            [SoundKey.PopupWinBeforeClose] = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/WinEnd.mp3",
            [SoundKey.BigWin] = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/BigWin.mp3",
            [SoundKey.MegaWin]= "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/MegaWin.mp3",
            [SoundKey.SuperWin] = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/SuperWin.mp3",
            [SoundKey.PopupWinCongratulate01]= "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/os_win_1.mp3",
            [SoundKey.PopupWinCongratulate02]= "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/os_win_2.mp3",
            [SoundKey.PopupWinAfterCloseCongratulate01] = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/os_bigwin_1.mp3",
            [SoundKey.PopupWinAfterCloseCongratulate02] = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/os_bigwin_2.mp3",

            [SoundKey.JackpotFlow]= "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/jackpot_flow.mp3",
            [SoundKey.JackpotBG]= "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/particle.mp3",
            [SoundKey.JackpotEnd]= "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/particleEnd.mp3",
        };



        public CustomDicSoundHandler soundHandlers = new CustomDicSoundHandler()
        {
            [SoundKey.RegularBG] = new GSHandler()
            {
                outputType = GSOutType.Music,
                assetPath= "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/BaseBackground.mp3",
            },

            [SoundKey.GodIdle1] = new GSHandler()
            {
                assetPath = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/os_idle_1.mp3",
            },
            [SoundKey.GodIdle2] = new GSHandler()
            {
                assetPath = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/os_idle_2.mp3",
            },
            [SoundKey.GodIdle3] = new GSHandler()
            {
                assetPath = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/os_idle_3.mp3",
            },
            [SoundKey.GodIdle4] = new GSHandler()
            {
                assetPath = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/os_idle_4.mp3",
            },
            [SoundKey.GodIdle5] = new GSHandler()
            {
                assetPath = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/os_idle_5.mp3",
            },
            [SoundKey.GodIdle6] = new GSHandler()
            {
                assetPath = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/os_idle_6.mp3",
            },
            [SoundKey.ReelStop1] = new GSHandler()
            {
                assetPath = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/S_R4Stop.mp3",
            },
            [SoundKey.TotalWinLine] = new GSHandler()
            {
                assetPath = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/WinRollingEnd.mp3",
            },
            [SoundKey.SlowMotionReal123MeetGod] = new GSHandler()
            {
                assetPath = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/mutiple.mp3",
            },
            [SoundKey.SlowMotionReal123HasGod] = new GSHandler()
            {
                assetPath = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/Trigger.mp3",
            },
            [SoundKey.SlowMotionCongratulate] = new GSHandler()
            {
                assetPath = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/os_slowmotion.mp3",
            },
            [SoundKey.SlowMotion] = new GSHandler()
            {
                assetPath = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/slowmotion2.mp3",
            },

            [SoundKey.FiveLine] = new GSHandler()
            {
                assetPath = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/FiveLine.mp3",
            },
            [SoundKey.WinRolling] = new GSHandler()
            {
                assetPath = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/WinRolling.mp3",
            },

            [SoundKey.FreeSpinBG] = new GSHandler()
            {
                outputType = GSOutType.Music,
                assetPath = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/FeatureBackground.mp3",
            },
            [SoundKey.FreeSpinTriggerBG] = new GSHandler()
            {
                assetPath = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/FeatureBackground.mp3",
            },
            [SoundKey.FreeSpinResultBG] = new GSHandler()
            {
                assetPath = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/Checkbonus.mp3",
            },
            [SoundKey.FreeSpinChangeSymbol] = new GSHandler()
            {
                assetPath = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/change.mp3",
            },
            [SoundKey.FreeSpinAdd] = new GSHandler()
            {
                assetPath = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/ReTrigger.mp3",
            },

            //[SoundKey.]="",
            [SoundKey.PopupWinBG] = new GSHandler()
            {
                outputType = GSOutType.Music,
                assetPath = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/Win.mp3",
            },
            [SoundKey.PopupWinBeforeClose] = new GSHandler()
            {
                assetPath = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/WinEnd.mp3",
            },
            [SoundKey.BigWin] = new GSHandler()
            {
                assetPath = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/BigWin.mp3",
            },
            [SoundKey.MegaWin] = new GSHandler()
            {
                assetPath = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/MegaWin.mp3",
            },
            [SoundKey.SuperWin] = new GSHandler()
            {
                assetPath = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/SuperWin.mp3",
            },
            [SoundKey.PopupWinCongratulate01] = new GSHandler()
            {
                assetPath = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/os_win_1.mp3",
            },
            [SoundKey.PopupWinCongratulate02] = new GSHandler()
            {
                assetPath = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/os_win_2.mp3",
            },
            [SoundKey.PopupWinAfterCloseCongratulate01] = new GSHandler()
            {
                assetPath = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/os_bigwin_1.mp3",
            },
            [SoundKey.PopupWinAfterCloseCongratulate02] = new GSHandler()
            {
                assetPath = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/os_bigwin_2.mp3",
            },
            [SoundKey.JackpotFlow] = new GSHandler()
            {
                assetPath = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/jackpot_flow.mp3",
            },
            [SoundKey.JackpotBG] = new GSHandler()
            {
                outputType = GSOutType.Music,
                assetPath = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/particle.mp3",
            },
            [SoundKey.JackpotEnd] = new GSHandler()
            {
                assetPath = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/particleEnd.mp3",
            },
        };






    }
}
