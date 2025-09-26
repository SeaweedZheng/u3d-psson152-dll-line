using GameMaker;
using System;
using System.Collections.Generic;
using _consoleBB = PssOn00152.ConsoleBlackboard02;
using _contentBB = PssOn00152.ContentBlackboard;
using UnityEngine;
using SBoxApi;

/// <summary>
/// 旧版本彩金
/// </summary>
public class GameJackpotCreator : MonoSingleton<GameJackpotCreator>
{


    bool isInit = false;
    public void Init()
    {

        MySteryJackpot02.Instance.Init((int)_contentBB.Instance.totalBet);
        Debug.LogWarning("【MySteryJackpot】init MySteryJackpot");
        isInit = true;
    }
 

    /// <summary> 打清零码时，重置彩金  </summary>
    public void ResetGameJackpot()
    {
        MySteryJackpot02.Instance.Reset();
    }

    public void GetGameJackpotShowData(Action<JackpotRes> onCallback)
    {
        if(isInit == false)
        {
            throw new Exception("彩金还没初始化");
        }
           

        JP.JackpotInfo res = MySteryJackpot02.Instance.JackpotInitData((int)_contentBB.Instance.totalBet);

        JackpotRes jpr = GetJackpotWinInfoData(res);

        onCallback?.Invoke(jpr);
    }



    JackpotRes GetJackpotWinInfoData(JP.JackpotInfo res)
    {

        // 检查第 0 位是否为 1
        bool bit0Grand = res.lottery[0] == 1;

        // 检查第 1 位是否为 1
        bool bit1Major = res.lottery[1] == 1;

        // 检查第 2 位是否为 1
        bool bit2Minior = res.lottery[2] == 1;

        // 检查第 3 位是否为 1
        bool bit3Mini = res.lottery[3] == 1;


        JackpotRes result = new JackpotRes();

        result.curJackpotGrand = (int)res.jackpotout[0] / 100;
        result.curJackpotMajor = (int)res.jackpotout[1] / 100;
        result.curJackpotMinior = (int)res.jackpotout[2] / 100;
        result.curJackpotMini = (int)res.jackpotout[3] / 100;


        List<JackpotWinInfo> jsrs = new List<JackpotWinInfo>();
        if (bit0Grand)
        {
            JackpotWinInfo jpw0 = new JackpotWinInfo();
            jpw0.curCredit = (int)res.jackpotout[0] / 100;
            jpw0.winCredit = bit0Grand ? (int)res.jackpotlottery[0] / 100 : 0;
            jpw0.id = 0;
            jpw0.name = "jp_jp1";
            jpw0.whenCredit = (int)res.jackpotold[0] / 100;
            jsrs.Add(jpw0);
        }


        if (bit1Major)
        {
            JackpotWinInfo jpw1 = new JackpotWinInfo();
            jpw1.curCredit = (int)res.jackpotout[1] / 100;
            jpw1.winCredit = bit1Major ? (int)res.jackpotlottery[1] / 100 : 0;
            jpw1.id = 1;
            jpw1.name = "jp_jp2";
            jpw1.whenCredit = (int)res.jackpotold[1] / 100;
            jsrs.Add(jpw1);
        }


        if (bit2Minior)
        {
            JackpotWinInfo jpw2 = new JackpotWinInfo();
            jpw2.curCredit = (int)res.jackpotout[2] / 100;
            jpw2.winCredit = bit2Minior ? (int)res.jackpotlottery[2] / 100 : 0;
            jpw2.id = 2;
            jpw2.name = "jp_jp3";
            jpw2.whenCredit = (int)res.jackpotold[2] / 100;
            jsrs.Add(jpw2);
        }


        if (bit3Mini)
        {

            JackpotWinInfo jpw3 = new JackpotWinInfo();
            jpw3.curCredit = (int)res.jackpotout[3] / 100;
            jpw3.winCredit = bit3Mini ? (int)res.jackpotlottery[3] / 100 : 0;
            jpw3.id = 3;
            jpw3.name = "jp_jp4";
            jpw3.whenCredit = (int)res.jackpotold[3] / 100;
            jsrs.Add(jpw3);
        }

        result.jpWinLst = jsrs;

        return result;
    }

    public void GetGameJackpotAfterSpin(int totalBet, bool isFreeSpin, Action<JackpotRes> onCallback)
    {
        if (isInit == false)
        {
            throw new Exception("彩金还没初始化");
        }

        if (isFreeSpin)
        {
            /*
            JP.JackpotInfo res01 = MySteryJackpot.Instance.JackpotInitData();
            JackpotRes jpr01 = GetJackpotWinInfoData(res01);
            onCallback?.Invoke(jpr01);*/
            GetGameJackpotShowData(onCallback);
            return;
        }


        MachineDataManager.Instance.RequestGetPlayerInfo((res) =>
        {
            SBoxAccount data = (SBoxAccount)res;
            int pid = _consoleBB.Instance.pid;
            List<SBoxPlayerAccount> playerAccountList = data.PlayerAccountList;
            for (int i = 0; i < playerAccountList.Count; i++)
            {
                if (playerAccountList[i].PlayerId == pid)
                {
                    //_consoleBB.Instance.sboxPlayerInfo = playerAccountList[i];
                    //MainBlackboardController.Instance.SyncMyTempCreditToReal(false); //同步玩家金币
                    _consoleBB.Instance.sboxPlayerInfo.Bets = playerAccountList[i].Bets;
                    _consoleBB.Instance.sboxPlayerInfo.Wins = playerAccountList[i].Wins;
                    break;
                }
            }
        }, (BagelCodeError err) =>
        {
            DebugUtils.Log(err.msg);
        });




        JP.BetInfo betInfo = new JP.BetInfo();
        betInfo.bet = totalBet;
        betInfo.betPercent = 1;
        betInfo.ScoreRate = 1;
        betInfo.seatId = 0;
        betInfo.JpPercent = _consoleBB.Instance.jackpotGamePercent;//0.01;  // 0.05

        if((float)_consoleBB.Instance.historyTotalWin / (_consoleBB.Instance.historyTotalBet == 0 ? 1f : (float)_consoleBB.Instance.historyTotalBet) < 0.92)
        {
            betInfo.JpPercent *= 2;
        }

        JP.JackpotInfo res = MySteryJackpot02.Instance.JackpotFlexiblePercentExtModify(betInfo);

        JackpotRes jpr = GetJackpotWinInfoData(res);

        onCallback?.Invoke(jpr);
    }





    /*
    float timeS = 0;
    void UpdataPlayerAccount()
    {
        if(Time.unscaledTime - timeS > 10f)
        {
            timeS = Time.unscaledTime;

            MachineDataManager.Instance.RequestGetPlayerInfo((res) =>
            {
                //Debug.LogError($"【Test】玩家数据  SBoxAccount：{JsonConvert.SerializeObject(res)}");

                SBoxAccount data = (SBoxAccount)res;
                int pid = _consoleBB.Instance.pid;
                List<SBoxPlayerAccount> playerAccountList = data.PlayerAccountList;
                for (int i = 0; i < playerAccountList.Count; i++)
                {
                    if (playerAccountList[i].PlayerId == pid)
                    {
                        //_consoleBB.Instance.sboxPlayerInfo = playerAccountList[i];
                        //MainBlackboardController.Instance.SyncMyTempCreditToReal(false); //同步玩家金币
                        _consoleBB.Instance.sboxPlayerInfo.Bets = playerAccountList[i].Bets;
                        _consoleBB.Instance.sboxPlayerInfo.Wins = playerAccountList[i].Wins;
                        break;
                    }
                }
            }, (BagelCodeError err) =>
            {
                DebugUtil.Log(err.msg);
            });

        }
    }*/

}


