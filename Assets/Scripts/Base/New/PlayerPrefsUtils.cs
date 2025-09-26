using UnityEngine;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// * PlayerPrefsUtils����ģ���AOT�п��Ե��õ�<br/>
/// * �����PlayerPrefsUtils����Blackboard��������ֶΣ�����ֵ���¼�������<br/>
/// * ����ʹ��Blackboard������ȵ�Blackboard��ʼ����������ܵ��á�
/// </remarks>
public static class PlayerPrefsUtils
{
    /// <summary> �Ƿ�����Sas </summary>
    public static bool isUseSas
    {
        get
        {
            if (ApplicationSettings.Instance.isRelease)  //��ʽ���Ȳ��ų�ȥ
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
    /// <summary> �Ƿ�����Ǯ�� </summary>
    public static bool isConnectMoneyBox
    {
        get
        {
            if (ApplicationSettings.Instance.isRelease)  //��ʽ���Ȳ��ų�ȥ
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

    /// <summary> �Ƿ��μ���ȸ��� </summary>
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


    /// <summary> �Ƿ�ʹ����ʽ��ĺÿ� </summary>
    /// <remarks>
    /// * ��ʽ�����϶�������ʽ���ĺÿᡣ <br>
    /// * ���԰������������ʽ���ÿ�Ĺ��ܡ�<br>
    /// </remarks>
    public static bool isUseReleaseIot
    {
        get
        {
            if (ApplicationSettings.Instance.isRelease)  //��ʽ�����϶�������ʽ���ĺÿᡣ 
                return true;

            // ���԰�������ʹ����ʽ���ĺÿᡣ
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






    /// <summary> ��ͣ�����Ϸ�������� </summary>
    public static bool isPauseAtPopupFreeSpinTrigger
    {
        get
        {
            if (ApplicationSettings.Instance.isRelease)  //��ʽ��
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


    /// <summary> ��ͣ��Ϸ�ʽ𵯴� </summary>
    public static bool isPauseAtPopupJackpotGame
    {
        get
        {
            if (ApplicationSettings.Instance.isRelease)  //��ʽ��
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



    /// <summary> ��ͣ�����ʽ𵯴� </summary>
    public static bool isPauseAtPopupJackpotOnline
    {
        get
        {
            if (ApplicationSettings.Instance.isRelease)  //��ʽ��
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
