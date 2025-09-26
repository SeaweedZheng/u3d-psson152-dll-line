using GameMaker;
// Blackboard
// Helper  // BB+工具方法
// Controller   // 事件+BB使用+工具方法
namespace PssOn00152
{
    [System.Serializable]
    public class CustomDicSound : UnitySerializedDictionary<SoundKey, string> { }

    [System.Serializable]
    public class CustomDicSoundHandler : UnitySerializedDictionary<SoundKey, GSHandler> { }

    public enum SoundKey
    {
        /// <summary> 正常游戏背景音乐 </summary>
        RegularBG,
        /// <summary> 免费游戏背景音乐 </summary>
        FreeSpinBG,
        /// <summary> 空闲时，财神声音1 </summary>
        GodIdle1,
        /// <summary> 空闲时，财神声音2 </summary>
        GodIdle2,
        /// <summary> 空闲时，财神声音3 </summary>
        GodIdle3,
        /// <summary> 空闲时，财神声音4 </summary>
        GodIdle4,
        /// <summary> 空闲时，财神声音5 </summary>
        GodIdle5,
        /// <summary> 空闲时，财神声音6 </summary>
        GodIdle6,
        /// <summary> 滚轮1停止 </summary>
        ReelStop1,
        /// <summary> 总赢线 </summary>
        TotalWinLine,

        /// <summary> 1、2、3列，每当出现财神图标（滚轮缓动特效才有） </summary>
        SlowMotionReal123MeetGod,
        /// <summary> 1、2、3列都有财神图标（滚轮缓动特效才有） </summary>
        SlowMotionReal123HasGod,
        /// <summary>  1、2列出现财神图标，祝贺语 （滚轮缓动特效才有） </summary>
        SlowMotionCongratulate,
        /// <summary>  滚轮缓动 </summary>
        SlowMotion,

        /// <summary> 免费游戏触发界面，背景音乐 </summary>
        FreeSpinTriggerBG,
        /// <summary> 免费游戏结束界面，背景音乐 </summary>
        FreeSpinResultBG,
        /// <summary> 免费游戏修改背景音乐 </summary>
        FreeSpinChangeSymbol,
        /// <summary> 增加免费游戏 </summary>
        FreeSpinAdd,

        /// <summary> 5连线 </summary>
        FiveLine,

        /// <summary> 主游戏赢钱，金币滚动 </summary>
        WinRolling,

        /// <summary> 大奖弹窗背景音乐 </summary>
        PopupWinBG,
        /// <summary> 大奖弹窗关闭前音乐 </summary>
        PopupWinBeforeClose,
        /// <summary> 大奖弹窗,祝贺语 </summary>
        PopupWinCongratulate01,
        /// <summary> 大奖弹窗,祝贺语 </summary>
        PopupWinCongratulate02,
        /// <summary> 大奖弹窗关闭后,祝贺语 </summary>
        PopupWinAfterCloseCongratulate01,
        /// <summary> 大奖弹窗关闭后,祝贺语 </summary>
        PopupWinAfterCloseCongratulate02,
        /// <summary> 大奖 </summary>
        BigWin,
        /// <summary> 巨奖 </summary>
        MegaWin,
        /// <summary> 超级大奖 </summary>
        SuperWin,


        /// <summary> 彩金弹窗，金币滚落 </summary>
        JackpotFlow,
        /// <summary> 彩金弹窗，背景音乐 </summary>
        JackpotBG,
        /// <summary> 彩金弹窗结束音乐 </summary>
        JackpotEnd,
    }

    public class SoundBlackboard : MonoWeakSingleton<SoundBlackboard>
    {
        //声音根路径
        //public string soundRootPath = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds";

        //声音相对路劲
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
