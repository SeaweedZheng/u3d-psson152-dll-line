

namespace GameMaker
{
    [System.Serializable]
    public class CustomDicSound : UnitySerializedDictionary<SoundKey, string> { }

    //[System.Serializable] // 这个会导致资源一直被引用不能被释放
    //public class CustomDicSound : UnitySerializedDictionary<SoundKey, AudioClip>{ }

    public enum SoundKey
    {
        /// <summary> 减注 </summary>
        BetDown,
        /// <summary> 最大注 </summary>
        BetMax,
        /// <summary> 加注 </summary>
        BetUp,
        /// <summary> 正常点击 </summary>
        NormalClick,
        /// <summary> 帮助页面 </summary>
        Tab,
        /// <summary> 关闭弹窗 </summary>
        PopupClose,
        /// <summary> 打开弹窗 </summary>
        PopupOpen,
        /// <summary> 自动玩 点击 </summary>
        SpinAutoClick,
        /// <summary> spin 点击 </summary>
        SpinClick,
        /// <summary> 机台进钱 </summary>
        MachineCoinIn,
    }
    public class GlobalSoundBlackboard : MonoWeakSingleton<GlobalSoundBlackboard>
    {

        //声音根路径
        //public string soundRootPath = "Assets/GameRes/_Common/Game Maker/Sounds";

        //声音相对路劲(Relative road strength)
        public CustomDicSound soundRelativePath = new CustomDicSound()
        {
            [SoundKey.BetDown] = "Assets/GameRes/_Common/Game Maker/Sounds/UI_Betting_Down.wav",  // './images/logo.png';
            [SoundKey.BetMax] = "Assets/GameRes/_Common/Game Maker/Sounds/UI_Betting_Max.wav",
            [SoundKey.BetUp] = "Assets/GameRes/_Common/Game Maker/Sounds/UI_Betting_Up.wav",
            [SoundKey.NormalClick] = "Assets/GameRes/_Common/Game Maker/Sounds/UI_Button_Normal.wav",
            [SoundKey.Tab] = "Assets/GameRes/_Common/Game Maker/Sounds/UI_Button_Tab.wav",
            [SoundKey.PopupClose] = "Assets/GameRes/_Common/Game Maker/Sounds/UI_Popup_Close.wav",
            [SoundKey.PopupOpen] = "Assets/GameRes/_Common/Game Maker/Sounds/UI_Popup_Open.wav",
            [SoundKey.SpinAutoClick] = "Assets/GameRes/_Common/Game Maker/Sounds/UI_Spin_Auto.wav",
            [SoundKey.SpinClick] = "Assets/GameRes/_Common/Game Maker/Sounds/UI_Spin_Start.wav",
            [SoundKey.MachineCoinIn] = "Assets/GameRes/_Common/Game Maker/Sounds/Machine_Coin_In.wav",
        };
    }
}
