using System;
using _consoleBB = PssOn00152.ConsoleBlackboard02;

public class DeviceBonusReport : MonoSingleton<DeviceBonusReport>
{


    public void CheckBonusReport()
    {
        if (_consoleBB.Instance.isUseBonusReport)
        {

            string strConnect = _consoleBB.Instance.bonusReportSetting;
            string[] addr = strConnect.Split(':');

            BonusReporter.Instance.Init(addr[0], int.Parse(addr[1]),_consoleBB.Instance.machineID,
                ApplicationSettings.Instance.gameTheme, _consoleBB.Instance.pid.ToString());
        }
        else
        {
            BonusReporter.Instance.Close();
        }
    }



    public void ReportBonus(string bonusType, string bonusName, int winCredit , int asCoinInCount,Action<string> onSuccess,Action<string> onError)
    {
        int coinInCount = asCoinInCount > 0 ? asCoinInCount : (winCredit / _consoleBB.Instance.coinInScale);
        BonusReporter.Instance.Post(bonusType, bonusName , 1, coinInCount, onSuccess, onError);
    }
}
