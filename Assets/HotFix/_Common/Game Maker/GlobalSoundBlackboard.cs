

namespace GameMaker
{
    [System.Serializable]
    public class CustomDicSound : UnitySerializedDictionary<SoundKey, string> { }

    //[System.Serializable] // ����ᵼ����Դһֱ�����ò��ܱ��ͷ�
    //public class CustomDicSound : UnitySerializedDictionary<SoundKey, AudioClip>{ }

    public enum SoundKey
    {
        /// <summary> ��ע </summary>
        BetDown,
        /// <summary> ���ע </summary>
        BetMax,
        /// <summary> ��ע </summary>
        BetUp,
        /// <summary> ������� </summary>
        NormalClick,
        /// <summary> ����ҳ�� </summary>
        Tab,
        /// <summary> �رյ��� </summary>
        PopupClose,
        /// <summary> �򿪵��� </summary>
        PopupOpen,
        /// <summary> �Զ��� ��� </summary>
        SpinAutoClick,
        /// <summary> spin ��� </summary>
        SpinClick,
        /// <summary> ��̨��Ǯ </summary>
        MachineCoinIn,
    }
    public class GlobalSoundBlackboard : MonoWeakSingleton<GlobalSoundBlackboard>
    {

        //������·��
        //public string soundRootPath = "Assets/GameRes/_Common/Game Maker/Sounds";

        //�������·��(Relative road strength)
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
