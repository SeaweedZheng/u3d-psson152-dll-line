using Newtonsoft.Json;
using SBoxApi;
using System;
using _consoleBB = PssOn00152.ConsoleBlackboard02;

public static class MachineDataUtils
{

    /// <summary>
    /// 设置线号机台号
    /// </summary>
    /// <param name="LineId"></param>
    /// <param name="MachineId"></param>
    public static void RequestSetLineIDMachineID(int LineId, int MachineId, Action<object> successCallback, Action<BagelCodeError> errorCallback) 
    {
        string str = JsonConvert.SerializeObject(_consoleBB.Instance.sboxConfData);
        SBoxConfData req = JsonConvert.DeserializeObject<SBoxConfData>(str);
        req.LineId = LineId;
        req.MachineId = MachineId;

        MachineDataManager.Instance.RequestWriteConf(req, (res) =>
        {
            SBoxPermissionsData data = res as SBoxPermissionsData;
            if (data.result == 0)
            {
                _consoleBB.Instance.sboxConfData.LineId = LineId;
                _consoleBB.Instance.sboxConfData.MachineId = MachineId;
            }
            successCallback?.Invoke(data);
            /**/
            MachineDataManager.Instance.RequestReadConf((data) =>
            {
                DebugUtils.Log($"!!重新读取的单数 ： {JsonConvert.SerializeObject(data)}");
                SBoxConfData res = (SBoxConfData)data;
                _consoleBB.Instance.sboxConfData = res;
            }, (BagelCodeError err) =>
            {
                DebugUtils.LogError(err.msg);
            });
        }, (BagelCodeError err) =>
        {
            errorCallback?.Invoke(err);
        });
    }



    /// <summary>
    /// 设置线号机台号
    /// </summary>
    /// <param name="LineId"></param>
    /// <param name="MachineId"></param>
    public static void RequestSetCoinInCoinOutScale(int? coinInScale, int? perTicket2Credit, int? perCredit2Ticket,int? scoreUpDownScale,
        Action<object> successCallback, Action<BagelCodeError> errorCallback)
    {
        string str = JsonConvert.SerializeObject(_consoleBB.Instance.sboxConfData);
        SBoxConfData req = JsonConvert.DeserializeObject<SBoxConfData>(str);
        
        if(coinInScale!= null)
            req.CoinValue = (int)coinInScale;

        /*
        if (perCredit2Ticket != null && perCredit2Ticket > 0)
        {
            req.scoreTicket = (int)perCredit2Ticket;
            req.TicketValue = 0;
        }
        if (perTicket2Credit != null && perTicket2Credit > 0)
        {
            req.scoreTicket = 0;
            req.TicketValue = (int)perTicket2Credit;
        }*/

        if (perCredit2Ticket != null && perCredit2Ticket > 1)
        {
            req.scoreTicket = (int)perCredit2Ticket;
            req.TicketValue = 1;
        }
        if (perTicket2Credit != null && perTicket2Credit > 1)
        {
            req.scoreTicket = 1;
            req.TicketValue = (int)perTicket2Credit;
        }

        if (scoreUpDownScale != null)
        {
            req.ScoreUpUnit = (int)scoreUpDownScale;
        }

        MachineDataManager.Instance.RequestWriteConf(req, (res) =>
        {
            SBoxPermissionsData data = res as SBoxPermissionsData;

            if (data.result == 0)
            {
                _consoleBB.Instance.sboxConfData.CoinValue = req.CoinValue;
                _consoleBB.Instance.sboxConfData.scoreTicket = req.scoreTicket;
                _consoleBB.Instance.sboxConfData.TicketValue = req.TicketValue;
                _consoleBB.Instance.sboxConfData.ScoreUpUnit = req.ScoreUpUnit;
            }
            successCallback?.Invoke(data);
        }, (BagelCodeError err) =>
        {
            DebugUtils.LogError($"RequestWriteConf : {err}");
            errorCallback?.Invoke(err);
        });
    }


    public static void RequestSetIsJackpotOnLine(bool isJackpotOnLine,
        Action<object> successCallback, Action<BagelCodeError> errorCallback)
    {

        int val = isJackpotOnLine ? 1 : 0;

        string str = JsonConvert.SerializeObject(_consoleBB.Instance.sboxConfData);
        SBoxConfData req = JsonConvert.DeserializeObject<SBoxConfData>(str);

        if (req.NetJackpot == val)
        {
            errorCallback?.Invoke(new BagelCodeError() { msg = ""});
            return;
        }
        req.NetJackpot = val;

        MachineDataManager.Instance.RequestWriteConf(req, (res) =>
        {
            SBoxPermissionsData data = res as SBoxPermissionsData;
            /*if (data.result == 0)
            {
                _consoleBB.Instance.sboxConfData.NetJackpot = req.NetJackpot;
            }*/
            successCallback?.Invoke(data);
        }, (BagelCodeError err) =>
        {
            DebugUtils.LogError($"RequestWriteConf : {err}");
            errorCallback?.Invoke(err);
        });
    }

}
