using UnityEngine;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// * PlayerPrefsUtils定义的，在AOT中可以调用到<br/>
/// * 相比于PlayerPrefsUtils，在Blackboard对象定义的字段，会有值变事件发出。<br/>
/// * 但是使用Blackboard，必须等到Blackboard初始化结束后才能调用。
/// </remarks>
public static class PlayerPrefsUtils
{
    /// <summary> 是否链接Sas </summary>
    public static bool isUseSas
    {
        get
        {
            if (ApplicationSettings.Instance.isRelease)  //正式版先不放出去
                return false;

            int enable =PlayerPrefs.GetInt(PARAM_IS_USE_SAS, 0);
            return enable != 0;
        }
        set
        {
            PlayerPrefs.DeleteKey("PARAM_IS_CONNECT_SAS");
            PlayerPrefs.SetInt(PARAM_IS_USE_SAS, value?1:0);
            PlayerPrefs.Save();
        }
    }
    //const string PARAM_IS_CONNECT_SAS = "PARAM_IS_CONNECT_SAS";
    const string PARAM_IS_USE_SAS = "PARAM_IS_USE_SAS";

    /*
    /// <summary> 是否链接钱箱 </summary>
    public static bool isConnectMoneyBox
    {
        get
        {
            if (ApplicationSettings.Instance.isRelease)  //正式版先不放出去
                return false;


            int enable = PlayerPrefs.GetInt(PARAM_IS_CONNECT_MONEY_BOX, 0);
            return enable != 0;
        }
        set
        {
            PlayerPrefs.SetInt(PARAM_IS_CONNECT_MONEY_BOX, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
    const string PARAM_IS_CONNECT_MONEY_BOX = "PARAM_IS_CONNECT_MONEY_BOX";

    */

    /// <summary> 是否多次检查热更新 </summary>
    public static bool isCheckHotfixMultipleTimes
    {
        get
        {
            int enable = PlayerPrefs.GetInt(GlobalData.HOTFIX_REQUEST_COUNT, 1);
            return enable >1;
        }
        set
        {
            PlayerPrefs.SetInt(GlobalData.HOTFIX_REQUEST_COUNT, value ? 10 : 1);
            PlayerPrefs.Save();
        }
    }


    /// <summary> 是否使用正式版的好酷 </summary>
    /// <remarks>
    /// * 正式包，肯定是用正式服的好酷。 <br>
    /// * 测试包才允许测试正式服好酷的功能。<br>
    /// </remarks>
    public static bool isUseReleaseIot
    {
        get
        {
            if (ApplicationSettings.Instance.isRelease)  //正式包，肯定是用正式服的好酷。 
                return true;

            // 测试包，允许使用正式服的好酷。
            int enable = PlayerPrefs.GetInt(PARAM_IS_TEST_RELEASE_IOT, 0);
            return enable != 0;
        }
        set
        {
            PlayerPrefs.SetInt(PARAM_IS_TEST_RELEASE_IOT, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
    const string PARAM_IS_TEST_RELEASE_IOT = "PARAM_IS_TEST_RELEASE_IOT";






    /// <summary> 暂停免费游戏触发弹窗 </summary>
    public static bool isPauseAtPopupFreeSpinTrigger
    {
        get
        {
            if (ApplicationSettings.Instance.isRelease)  //正式包
                return false;

            int enable = PlayerPrefs.GetInt(PARAM_IS_PAUSE_AT_POPUP_FREE_SPIN_TRIGGER, 0);
            return enable != 0;
        }
        set
        {
            PlayerPrefs.SetInt(PARAM_IS_PAUSE_AT_POPUP_FREE_SPIN_TRIGGER, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
    const string PARAM_IS_PAUSE_AT_POPUP_FREE_SPIN_TRIGGER = "PARAM_IS_PAUSE_AT_POPUP_FREE_SPIN_TRIGGER";


    /// <summary> 暂停游戏彩金弹窗 </summary>
    public static bool isPauseAtPopupJackpotGame
    {
        get
        {
            if (ApplicationSettings.Instance.isRelease)  //正式包
                return false;

            int enable = PlayerPrefs.GetInt(PARAM_IS_PAUSE_AT_POPUP_JACKPOT_GAME, 0);
            return enable != 0;
        }
        set
        {
            PlayerPrefs.SetInt(PARAM_IS_PAUSE_AT_POPUP_JACKPOT_GAME, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
    const string PARAM_IS_PAUSE_AT_POPUP_JACKPOT_GAME = "PARAM_IS_PAUSE_AT_POPUP_JACKPOT_GAME";



    /// <summary> 暂停联网彩金弹窗 </summary>
    public static bool isPauseAtPopupJackpotOnline
    {
        get
        {
            if (ApplicationSettings.Instance.isRelease)  //正式包
                return false;

            int enable = PlayerPrefs.GetInt(PARAM_IS_PAUSE_AT_POPUP_JACKPOT_ONLINE, 0);
            return enable != 0;
        }
        set
        {
            PlayerPrefs.SetInt(PARAM_IS_PAUSE_AT_POPUP_JACKPOT_ONLINE, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
    const string PARAM_IS_PAUSE_AT_POPUP_JACKPOT_ONLINE = "PARAM_IS_PAUSE_AT_POPUP_JACKPOT_ONLINE";
}
