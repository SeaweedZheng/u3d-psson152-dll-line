using GameMaker;
using PssOn00152;
using SimpleJSON;
using SlotMaker;
using System;
using SlotDllAlgorithmG152;
using System.Collections.Generic;
using UnityEngine;
using _contentBB = PssOn00152.ContentBlackboard;
using _consoleBB = PssOn00152.ConsoleBlackboard02;
using _mainBB = GameMaker.MainBlackboard;
using Newtonsoft.Json;
using SBoxApi;
using System.Collections;
using JetBrains.Annotations;


public partial class DlllDataG152Controller : MonoSingleton<DlllDataG152Controller>
{

    public void ParseSlotSpin(long totalBet, LinkInfo res)
    {


        SboxIdeaGameState gameState = (SboxIdeaGameState)((int)res.gameState);

        List<int> LineNumbers = new List<int>();
        int lineMark = (int)res.lineMark;

        // lineMark /10000 / 100  // 3个线的数据

        int B = lineMark / 10000;
        int remainingAfterB = lineMark - B * 10000;
        int C = remainingAfterB / 100;
        int D = remainingAfterB - C * 100;

        if (B != 0)
            LineNumbers.Add(B);
        if (C != 0)
            LineNumbers.Add(C);
        if (D != 0)
            LineNumbers.Add(D);

        List<WinningLineInfo> winningLines = new List<WinningLineInfo>();

        if(res.lineNum > 0 && res.lineMark > 0 )
            for (int i = 0; i < (int)res.lineNum; i++)
            {
                LinkData node = res.linkData[i];

                int symbolNumber = node.icon;
                int hitCount = node.link;
                //int credit = node.reward;
                int lineNumber = LineNumbers[i];

                winningLines.Add(new WinningLineInfo()
                {
                    LineNumber = lineNumber,
                    SymbolNumber = symbolNumber,
                    WinCount = hitCount,
                });
            }


        bool isJackpotGrand = gameState == SboxIdeaGameState.JackpotGame && res.lottery[0]==1;

        bool isJackpotMajor = gameState == SboxIdeaGameState.JackpotGame && res.lottery[1] == 1;

        bool isJackpotMinor = gameState == SboxIdeaGameState.JackpotGame && res.lottery[2] == 1;

        bool isJackpotMini = gameState == SboxIdeaGameState.JackpotGame && res.lottery[3] == 1;

        int freeSpinTotalTimes = res.maxRound; // (int)res["maxRound"];
        int freeSpinPlayTimes = res.curRound; // (int)res["curRound"];

        //bool isFreeSpinTrigger = freeSpinPlayTimes == 0 && freeSpinTotalTimes > 0;
        //bool isFreeSpinResult = freeSpinTotalTimes > 0 && freeSpinPlayTimes == freeSpinTotalTimes;
        //bool isFreeSpin = freeSpinPlayTimes > 0 && freeSpinTotalTimes > 0;

        bool isFreeSpinTrigger = gameState == SboxIdeaGameState.FreeSpinStart;
        bool isFreeSpinResult = gameState == SboxIdeaGameState.FreeSpinEnd;
        bool isFreeSpin = gameState == SboxIdeaGameState.FreeSpin || gameState == SboxIdeaGameState.FreeSpinEnd;

        string strDeckRowCol = "";




        long creditBefore = MainBlackboardController.Instance.myRealCredit;
        long afterBetCredit = 0;
        long totalEarnCredit = 0;
        long baseGameWinCredit = 0;
        long jackpotGameCredit = 0;



        JackpotRes jpGameRes = new JackpotRes();
        _contentBB.Instance.jpGameRes  = jpGameRes;
        jpGameRes.curJackpotGrand = (float)(res.jackpotout[0] / 100);
        jpGameRes.curJackpotMajor = (float)(res.jackpotout[1] / 100);
        jpGameRes.curJackpotMinior = (float)(res.jackpotout[2] / 100);
        jpGameRes.curJackpotMini = (float)(res.jackpotout[3] / 100);


        List<SymbolInclude> symbolInclude = new List<SymbolInclude>();


        if (isJackpotMini)
        {
            jackpotGameCredit += (long)(res.jackpotlottery[3] / 100);    

            jpGameRes.jpWinLst.Add(new JackpotWinInfo()
            {
                name = "jp_jp4",
                id = 3,
                winCredit = (float)(res.jackpotlottery[3] / 100),
                whenCredit = (float)(res.jackpotold[3] / 100),
                curCredit = (float)(res.jackpotout[3] / 100),
            });

            symbolInclude = new List<SymbolInclude>();
            for (int i = 0; i < 2; i++)
            {
                symbolInclude.Add(new SymbolInclude() { symbolNumber = 12 });
            }

        }

        if (isJackpotMinor)
        {
            jackpotGameCredit += (long)(res.jackpotlottery[2] / 100);

            jpGameRes.jpWinLst.Add(new JackpotWinInfo()
            {
                name = "jp_jp3",
                id = 2,
                winCredit = (float)(res.jackpotlottery[2] / 100),
                whenCredit = (float)(res.jackpotold[2] / 100),
                curCredit = (float)(res.jackpotout[2] / 100),
            });

            symbolInclude = new List<SymbolInclude>();
            for (int i = 0; i < 3; i++)
            {
                symbolInclude.Add(new SymbolInclude() { symbolNumber = 12 });
            }
        }


        if (isJackpotMajor)
        {
            jackpotGameCredit += (long)(res.jackpotlottery[1] / 100);

            jpGameRes.jpWinLst.Add(new JackpotWinInfo()
            {
                name = "jp_jp2",
                id = 1,
                winCredit = (float)(res.jackpotlottery[1] / 100),
                whenCredit = (float)(res.jackpotold[1] / 100),
                curCredit = (float)(res.jackpotout[1] / 100),
            });

            symbolInclude = new List<SymbolInclude>();
            for (int i = 0; i < 4; i++)
            {
                symbolInclude.Add(new SymbolInclude() { symbolNumber = 12 });
            }
        }

        if (isJackpotGrand)
        {
            jackpotGameCredit += (long)(res.jackpotlottery[0] / 100);

            jpGameRes.jpWinLst.Add(new JackpotWinInfo()
            {
                name = "jp_jp1",
                id = 0,
                winCredit = (float)(res.jackpotlottery[0]/100),
                whenCredit = (float)(res.jackpotold[0] / 100),
                curCredit = (float)(res.jackpotout[0] / 100),
            });

            symbolInclude = new List<SymbolInclude>();
            for (int i = 0; i < 5; i++)
            {
                symbolInclude.Add(new SymbolInclude() { symbolNumber = 12 });
            }
        }
       

        if (isFreeSpinTrigger)
        {
            int freeSpinSymbolCount = GetFreeSpinSymbolCount(res.num);
            for (int i = 0; i < (int)freeSpinSymbolCount; i++)
            {
                symbolInclude.Add(new SymbolInclude() { symbolNumber = 11 });
            }

            strDeckRowCol = SlotDeckGenerator.Instance.GenerateGameArray(
                _contentBB.Instance.payLines,
                CustomBlackboard.Instance.symbolNumber, winningLines, new int[] { 11, 12 }, symbolInclude);
        }
        else
        {
            strDeckRowCol = SlotDeckGenerator.Instance.GenerateGameArray(
                _contentBB.Instance.payLines,
                CustomBlackboard.Instance.symbolNumber, winningLines, new int[] { 11, 12 }, symbolInclude);
        }

        //Debug.LogError($"strDeckRowCol = {strDeckRowCol}");



        if (++_mainBB.Instance.gameNumber < 0)
            _mainBB.Instance.gameNumber = 1;

        if (++_mainBB.Instance.reportId < 0)
            _mainBB.Instance.reportId = 1;

        _contentBB.Instance.curGameNumber = _mainBB.Instance.gameNumber;



        _contentBB.Instance.response = JsonConvert.SerializeObject(res);  //res.ToString();

        //Debug.LogError($"@@@ response: {_contentBB.Instance.response}");

        _contentBB.Instance.strDeckRowCol = strDeckRowCol;

        List<SymbolWin> winList = new List<SymbolWin>();

        for (int i = 0; i < LineNumbers.Count; i++)
        {
            int lineNumber = LineNumbers[i];
            int lineIndex = lineNumber - 1;

            List<int> lineInfo = _contentBB.Instance.payLines[lineIndex];
      
            LinkData lineNode = res.linkData[i];
            int credit = (int)lineNode.reward;
            int hitCount = (int)lineNode.link;
            int symbolNumber = (int)lineNode.icon;

            List<Cell> _cells = new List<Cell>();

            for (int c = 0; c < hitCount; c++)
            {
                int rowIdx = lineInfo[c];
                int colIdx = c;
                _cells.Add(new Cell(colIdx, rowIdx));
            }

            SymbolWin sw = new SymbolWin()
            {
                earnCredit = credit,
                multiplier = 1,
                lineNumber = lineNumber,
                symbolNumber = symbolNumber,
                cells = _cells,
            };
            winList.Add(sw);

            baseGameWinCredit +=  credit;
        }
        _contentBB.Instance.winList = winList;



        if (isFreeSpin && freeSpinTotalTimes != 0)
        {
            _contentBB.Instance.freeSpinAddNum =
                freeSpinTotalTimes - _contentBB.Instance.freeSpinTotalTimes;
        }
        else
            _contentBB.Instance.freeSpinAddNum = 0;

        if (isFreeSpin)
        {
            _contentBB.Instance.showFreeSpinRemainTime = _contentBB.Instance.freeSpinTotalTimes -
                                                           _contentBB.Instance.freeSpinPlayTimes - 1;
        }
        else
        {
            _contentBB.Instance.showFreeSpinRemainTime = 0;
        }

        _contentBB.Instance.freeSpinTotalTimes = freeSpinTotalTimes;
        _contentBB.Instance.freeSpinPlayTimes = freeSpinPlayTimes;

        _contentBB.Instance.isFreeSpinTrigger = isFreeSpinTrigger;
        _contentBB.Instance.isFreeSpinResult = isFreeSpinResult;

        if (isFreeSpinTrigger)
        {
            _contentBB.Instance.curReelStripsIndex = "BS";
            _contentBB.Instance.nextReelStripsIndex = "FS";
        }
        else if (isFreeSpinResult)
        {
            _contentBB.Instance.curReelStripsIndex = "FS";
            _contentBB.Instance.nextReelStripsIndex = "BS";
        }
        else if (isFreeSpin)
        {
            _contentBB.Instance.curReelStripsIndex = "FS";
            _contentBB.Instance.nextReelStripsIndex = "FS";
        }
        else
        {
            _contentBB.Instance.curReelStripsIndex = "BS";
            _contentBB.Instance.nextReelStripsIndex = "BS";
        }

        totalEarnCredit = baseGameWinCredit + jackpotGameCredit;
        _contentBB.Instance.totalEarnCredit = totalEarnCredit;
        _contentBB.Instance.jackpotGameCredit = jackpotGameCredit;
        _contentBB.Instance.baseGameWinCredit = baseGameWinCredit;


        //_contentBB.Instance.winFreeSpinTriggerOrAddCopy = null;


        _contentBB.Instance.curGameCreatTimeMS = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        _contentBB.Instance.curGameGuid = isFreeSpin
        ? $"{MainBlackboard.Instance.gameID}-{UnityEngine.Random.Range(100, 1000)}-{_contentBB.Instance.curGameCreatTimeMS}-{_mainBB.Instance.gameNumber}-{_contentBB.Instance.gameNumberFreeSpinTrigger}"
        : $"{MainBlackboard.Instance.gameID}-{UnityEngine.Random.Range(100, 1000)}-{_contentBB.Instance.curGameCreatTimeMS}-{_mainBB.Instance.gameNumber}";


        if (isFreeSpinTrigger)
            _contentBB.Instance.gameNumberFreeSpinTrigger = _mainBB.Instance.gameNumber;



        if (!isFreeSpin)
        {
            afterBetCredit = creditBefore - totalBet;
            MainBlackboardController.Instance.SetMyTempCredit(afterBetCredit, false);
        }
        else
        {
            afterBetCredit = creditBefore;
        }
        // ## MainBlackboardController.Instance.SetMyTempCredit(afterBetCredit, false); // 不同步给ui


        long creditAfter = afterBetCredit + totalEarnCredit;


        //Debug.LogWarning($"押注前分数：creditBefore = {creditBefore} 押注分数：{totalBet} 押注后分数:  afterBetCredit = {afterBetCredit}  totalEarnCredit={totalEarnCredit} ");
        //Debug.LogWarning($"本次计算 creditAfter= {afterBetCredit + totalEarnCredit}；  算法卡 creditAfter={creditAfter}");


        // 免费游戏累计总赢
        long freeSpinTotalWinCredit = 0;

        if (!isFreeSpin)
        {
            _contentBB.Instance.freeSpinTotalWinCredit = 0;
        }
        else
        {
            _contentBB.Instance.freeSpinTotalWinCredit += totalEarnCredit;
            freeSpinTotalWinCredit = _contentBB.Instance.freeSpinTotalWinCredit;
        }


        List<List<int>> deckColRow = SlotTool.GetDeckColRow02(strDeckRowCol);



        // 是否长滚动
        //bool isReelsSlowMotion = (deckColRow[0].Contains(1) && deckColRow[1].Contains(1)) ? true : false;
        //_contentBB.Instance.isReelsSlowMotion = isReelsSlowMotion;
        bool isReelsSlowMotion = false;
        _contentBB.Instance.isReelsSlowMotion = isReelsSlowMotion;


        // bonus数据
        var bonusResult = new Dictionary<int, JSONNode>();
        /*
        if (res["contents"]["bonus_result"] != null && res["contents"]["bonus_result"].Count > 0)
        {
           foreach (JSONNode item in res["contents"]["bonus_result"])
           {
               bonusResult.Add((int)item["bonus_id"],item);
           }
        }*/
        _contentBB.Instance.bonusResult = bonusResult;


        SlotGameEffectManager.Instance.SetEffect(
            isReelsSlowMotion ? SlotGameEffect.Expectation01 :
            isFreeSpin ? SlotGameEffect.FreeSpin : SlotGameEffect.Default);



        _contentBB.Instance.creditBefore = creditBefore;
        _contentBB.Instance.creditAfter = creditAfter;


        if (isFreeSpinTrigger || isFreeSpin)
        {
            Debug.LogError($"免费游戏： {_contentBB.Instance.curGameGuid} - {JsonConvert.SerializeObject(res)}  -- {SlotDllAlgorithmG152Manager.Instance.curSlotData}");
        }

    }
    public IEnumerator SetPlayerCredit(Action successCallback = null, Action<string> errorCallback = null)
    {
        bool isError = false;
        bool isNext = false;
        int totalBet = (int)_contentBB.Instance.totalBet;

        if (!_contentBB.Instance.isFreeSpin)
        {
            MachineDataManager.Instance.RequestScoreDown(totalBet, (res) =>
            {
                isNext = true; // 走上下分，传回算发卡。
            });

            yield return new WaitUntil(() => isNext == true);
            isNext = false;
        }


        if (_contentBB.Instance.totalEarnCredit > 0)
        {
            MachineDataManager.Instance.RequestScoreUp(
                (int)_contentBB.Instance.totalEarnCredit, (res) =>
                {
                    isNext = true;
                });

            yield return new WaitUntil(() => isNext == true);
            isNext = false;
        }

        SBoxPlayerAccount playerAccount = null;

        MachineDataManager.Instance.RequestGetPlayerInfo((res) =>
        {
            SBoxAccount data = (SBoxAccount)res;

            int pid = _consoleBB.Instance.pid;

            bool isOK = false;
            List<SBoxPlayerAccount> playerAccountList = data.PlayerAccountList;
            for (int i = 0; i < playerAccountList.Count; i++)
            {
                if (playerAccountList[i].PlayerId == pid)
                {
                    playerAccount = playerAccountList[i];
                    isOK = true;
                    break;
                }
            }
            if (!isOK)
            {
                DebugUtils.LogError($" SBoxPlayerAccount is null , Where PlayerId = {pid}");
                isError = true;
            }

            isNext = true;

        }, (BagelCodeError err) =>
        {
            errorCallback?.Invoke(err.msg);
            isError = true;
            isNext = true;
        });

        yield return new WaitUntil(() => isNext == true);
        isNext = false;

        if (isError) yield break;

        long creditAfter = playerAccount.Credit;

        MainBlackboardController.Instance.SetMyRealCredit(creditAfter);

        // 这里有可能会受到上下分，投退币的影响！
        //_contentBB.Instance.creditAfter = creditAfter;

        //_contentBB.Instance.winFreeSpinTriggerOrAddCopy = winFreeSpinTriggerOrAddCopy;


        successCallback?.Invoke();
    }

    public void Record()
    {

        // 游戏场景记录
        GameSenceData gameSenceData = new GameSenceData();

        gameSenceData.respone = _contentBB.Instance.response;
        gameSenceData.reportId = _mainBB.Instance.reportId;
        gameSenceData.timeS = _contentBB.Instance.curGameCreatTimeMS / 1000;
        gameSenceData.gameNumber = _contentBB.Instance.curGameNumber;
        gameSenceData.gameNumberFreeSpinTrigger = _contentBB.Instance.isFreeSpin ? _contentBB.Instance.gameNumberFreeSpinTrigger : 0;
        gameSenceData.isFreeSpin = _contentBB.Instance.isFreeSpin;
        gameSenceData.freeSpinAddNum = _contentBB.Instance.freeSpinAddNum;

        gameSenceData.curStripsIndex = _contentBB.Instance.curReelStripsIndex;
        gameSenceData.nextStripsIndex = _contentBB.Instance.nextReelStripsIndex;
        gameSenceData.strDeckRowCol = _contentBB.Instance.strDeckRowCol;
        gameSenceData.deckRowCol = SlotTool.GetDeckRowCol(_contentBB.Instance.strDeckRowCol);

        gameSenceData.winFreeSpinTrigger = _contentBB.Instance.winFreeSpinTriggerOrAddCopy;
        gameSenceData.winList = _contentBB.Instance.winList;
        gameSenceData.freeSpinPlayTimes = _contentBB.Instance.freeSpinPlayTimes;
        gameSenceData.freeSpinTotalTimes = _contentBB.Instance.freeSpinTotalTimes;
        gameSenceData.freeSpinTotalWinCredit = _contentBB.Instance.freeSpinTotalWinCredit;
        gameSenceData.totalBet = _contentBB.Instance.totalBet;
        gameSenceData.creditBefore = _contentBB.Instance.creditBefore;
        gameSenceData.creditAfter = _contentBB.Instance.creditAfter; // 这是基础游戏+彩金【外设彩金-需要修改】
        gameSenceData.jackpotWinCredit = 0;  //【外设彩金-需要修改】
        gameSenceData.baseGameWinCredit = _contentBB.Instance.baseGameWinCredit;


        TableSlotGameRecordItem slotGameRecordItem = new TableSlotGameRecordItem()
        {
            game_type = _contentBB.Instance.isFreeSpin ? "free_spin" : _contentBB.Instance.isFreeSpinTrigger ? "free_spin_trigger" : "spin",
            game_id = ConfigUtils.curGameId,
            game_uid = _contentBB.Instance.curGameGuid,
            created_at = _contentBB.Instance.curGameCreatTimeMS,
            total_bet = _contentBB.Instance.totalBet,
            credit_before = gameSenceData.creditBefore,
        };

        // 本剧数据存入数据库
        slotGameRecordItem.credit_after = gameSenceData.creditAfter;  //【外设彩金-需要修改】
        slotGameRecordItem.base_game_win_credit = gameSenceData.baseGameWinCredit;



        // 彩金数据
        JackpotRes info = _contentBB.Instance.jpGameRes;


        // 数据修改：
        gameSenceData.jpGrand = info.curJackpotGrand;
        gameSenceData.jpMajor = info.curJackpotMajor;
        gameSenceData.jpMinor = info.curJackpotMinior;
        gameSenceData.jpMini = info.curJackpotMini;


        if (info.jpWinLst != null && info.jpWinLst.Count > 0)
        {
            JackpotWinInfo item = info.jpWinLst[0];

            gameSenceData.jpWinInfo = item;


            //int winJPCredit = (int)item.winCredit;

            //slotGameRecordItem.jackpot_win_credit = winJPCredit;
            //gameSenceData.jackpotWinCredit = winJPCredit;

            // long creditBefore = _contentBB.Instance.creditAfter;
            // long creditAfter = _contentBB.Instance.creditAfter += winJPCredit;


            int winJPCredit = (int)_contentBB.Instance.jackpotGameCredit;
            long creditAfter = _contentBB.Instance.creditAfter;

            long jpGameCreditBefore = creditAfter - winJPCredit;
            long jpGameCreditAfter = creditAfter;


            // _contentBB.Instance.creditAfter = creditAfter;
            gameSenceData.creditAfter = creditAfter;
            slotGameRecordItem.credit_after = creditAfter;

            slotGameRecordItem.jackpot_win_credit = winJPCredit;
            gameSenceData.jackpotWinCredit = winJPCredit;


            // 通知算法卡(游戏彩金得分)
            // 【废弃不用】旧版本会使用这个接口给，将游戏彩金中奖值同步给底板
            // MachineDataManager.Instance.NotifyGameJackpot(winJPCredit);


            // 游戏彩金记录
            TableJackpotRecordAsyncManager.Instance.AddJackpotRecord(item.id, item.name, winJPCredit,
            jpGameCreditBefore, jpGameCreditAfter,
            _contentBB.Instance.curGameGuid, _contentBB.Instance.curGameCreatTimeMS);


            // 额外奖上报
            DeviceBonusReport.Instance.ReportBonus(item.name, item.name, winJPCredit, -1, (msg) => { }, (err) => { });

        }

        // 每日营收统计
        TableBusniessDayRecordAsyncManager.Instance.AddTotalBetWin(

            _contentBB.Instance.curReelStripsIndex == "FS" ? 0 : _contentBB.Instance.totalBet,

            _contentBB.Instance.totalEarnCredit,// _contentBB.Instance.baseGameWinCredit + gameSenceData.jackpotWinCredit,

            _consoleBB.Instance.myCredit);

        //_contentBB.Instance.totalEarnCredit = _contentBB.Instance.baseGameWinCredit + gameSenceData.jackpotWinCredit;
        //_contentBB.Instance.jpWinLst = info.jpWinLst;

        slotGameRecordItem.scene = JsonConvert.SerializeObject(gameSenceData);
        string sql = SQLiteAsyncHelper.SQLInsertTableData<TableSlotGameRecordItem>(ConsoleTableName.TABLE_SLOT_GAME_RECORD, slotGameRecordItem);
        SQLiteAsyncHelper.Instance.ExecuteNonQueryAsync(sql);

        try
        {
            // 数据数据上报
            string str = ReportDataUtils.CreatReportData(gameSenceData, _consoleBB.Instance.sboxPlayerInfo);
            DebugUtils.Log($"数据上报成功 {str}");
            ReportManager.Instance.SendData(str, null, null);
        }
        catch (Exception ex) { }


    }




    int GetFreeSpinSymbolCount(int totalFreeSpinCount)
    {
        switch (totalFreeSpinCount)
        {
            case 6:
                return 3;
            case 9:
                return 4;
            case 12:
                return 5;
            default:
                {
                    Debug.LogError($"没费次数有误: {totalFreeSpinCount}");
                    return 1;
                }

        }
    }




}



public partial class DlllDataG152Controller
{




    void OnEnable()
    {
        EventCenter.Instance.AddEventListener<EventData>(GlobalEvent.ON_GM_EVENT, OnGMEvent);
    }

    void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener<EventData>(GlobalEvent.ON_GM_EVENT, OnGMEvent);
    }

    void OnGMEvent(EventData res)
    {
        if (ApplicationSettings.Instance.isMock == false) return;

        if (res.id != 152) return;

        switch (res.name)
        {
            case GlobalEvent.GMFreeSpin:
                nextSpin = SpinDataType.FreeSpin;
                break;
            case GlobalEvent.GMBigWin:
                nextSpin = SpinDataType.BigWin;
                break;
            case GlobalEvent.GMJp1:
                nextSpin = SpinDataType.Jp1;
                break;
            case GlobalEvent.GMJp2:
                nextSpin = SpinDataType.Jp2;
                break;
            case GlobalEvent.GMJp3:
                nextSpin = SpinDataType.Jp3;
                break;
            case GlobalEvent.GMJp4:
                nextSpin = SpinDataType.Jp4;
                break;
            case GlobalEvent.GMJpOnline:
                //nextSpin = SpinDataType.JpOnline;
                break;
        }

    }



    public enum SpinDataType
    {
        None,
        Normal,
        FreeSpin,
        BigWin,
        Jp1,
        Jp2,
        Jp3,
        Jp4,
        JpOnline,
        //Bonus1Ball,
    };


    SpinDataType nextSpin = SpinDataType.None;




    private Dictionary<SpinDataType, List<string[]>> spinDatas = new Dictionary<SpinDataType, List<string[]>>()
    {
        [SpinDataType.FreeSpin] = new List<string[]>()
            {
               new string[]
                {
                    "Assets/GameRes/_Common/Game Maker/_Mock/Resources/g152_dll/g152__slot_spin__free_0.json",
                    "Assets/GameRes/_Common/Game Maker/_Mock/Resources/g152_dll/g152__slot_spin__free_1.json",
                    "Assets/GameRes/_Common/Game Maker/_Mock/Resources/g152_dll/g152__slot_spin__free_2.json",
                    "Assets/GameRes/_Common/Game Maker/_Mock/Resources/g152_dll/g152__slot_spin__free_3.json",
                    "Assets/GameRes/_Common/Game Maker/_Mock/Resources/g152_dll/g152__slot_spin__free_4.json",
                    "Assets/GameRes/_Common/Game Maker/_Mock/Resources/g152_dll/g152__slot_spin__free_5.json",
                    "Assets/GameRes/_Common/Game Maker/_Mock/Resources/g152_dll/g152__slot_spin__free_6.json",
                    "Assets/GameRes/_Common/Game Maker/_Mock/Resources/g152_dll/g152__slot_spin__free_7.json",
                    "Assets/GameRes/_Common/Game Maker/_Mock/Resources/g152_dll/g152__slot_spin__free_8.json",
                    "Assets/GameRes/_Common/Game Maker/_Mock/Resources/g152_dll/g152__slot_spin__free_9.json",
                },
            },
        [SpinDataType.Normal] = new List<string[]>()
            {
                new string[] { "Assets/GameRes/_Common/Game Maker/_Mock/Resources/g152_dll/g152__slot_spin_0.json"},
                new string[] { "Assets/GameRes/_Common/Game Maker/_Mock/Resources/g152_dll/g152__slot_spin_1.json" },
                new string[] { "Assets/GameRes/_Common/Game Maker/_Mock/Resources/g152_dll/g152__slot_spin_2.json" },
            },
        [SpinDataType.Jp1] = new List<string[]>()
            {
                new string[] { "Assets/GameRes/_Common/Game Maker/_Mock/Resources/g152_dll/g152__slot_spin__jp1.json" },
            },
        [SpinDataType.Jp2] = new List<string[]>()
            {
                new string[] { "Assets/GameRes/_Common/Game Maker/_Mock/Resources/g152_dll/g152__slot_spin__jp2.json" },
            },
        [SpinDataType.Jp3] = new List<string[]>()
            {
                new string[] { "Assets/GameRes/_Common/Game Maker/_Mock/Resources/g152_dll/g152__slot_spin__jp3.json" },
            },
        [SpinDataType.Jp4] = new List<string[]>()
            {
                new string[] { "Assets/GameRes/_Common/Game Maker/_Mock/Resources/g152_dll/g152__slot_spin__jp4.json" },
            },
        [SpinDataType.BigWin] = new List<string[]>()
            {
                new string[] { "Assets/HotFix/Games/_Mock/Resources/g152/g152__slot_spin__Bigwin_0.json" },
                new string[] { "Assets/HotFix/Games/_Mock/Resources/g152/g152__slot_spin__Bigwin_1.json" },
                new string[] { "Assets/HotFix/Games/_Mock/Resources/g152/g152__slot_spin__Bigwin_2.json" },
                new string[] { "Assets/HotFix/Games/_Mock/Resources/g152/g152__slot_spin__Bigwin_3.json" },
            }

    };

    Queue<string> curDatas = new Queue<string>();

    public void RequestSlotSpinFromMock(long totalBet, Action<JSONNode> successCallback,
    Action<BagelCodeError> errorCallback)
    {
        tasks.Enqueue(() =>
        {
            if (curDatas.Count == 0)
            {
                /*  随机数据
                int dataIndex = UnityEngine.Random.Range(0, spinDatas.Count);
                List<string[]> target = nextSpin != SpinDataType.None?
                    spinDatas[nextSpin] : spinDatas.ElementAt(dataIndex).Value;
                nextSpin = SpinDataType.None;
                */
                List<string[]> target = null;
                target = nextSpin != SpinDataType.None ? spinDatas[nextSpin] : spinDatas[SpinDataType.Normal];
                nextSpin = SpinDataType.None;

                string[] strs = target[UnityEngine.Random.Range(0, target.Count)];
                curDatas = new Queue<string>(strs);  // 会改变引用数据  
            }

            string path = curDatas.Dequeue();
            int resourcesIndex = path.IndexOf("Resources/");
            string remainingPath = path.Substring(resourcesIndex + "Resources/".Length);
            remainingPath = remainingPath.Split('.')[0];

            try
            {
                TextAsset jsn = Resources.Load<TextAsset>(remainingPath);
                Debug.LogWarning($"<color=yellow>mock down</color>: 使用数据: {remainingPath} data:{jsn.text}");
                if (jsn != null && jsn.text != null)
                {
                    JSONNode res = JSON.Parse(jsn.text);
                    successCallback?.Invoke(res);
                }
                else
                {
                    BagelCodeError err = new BagelCodeError() { code = 404, msg = $"找不到数据: {path}" };
                    errorCallback?.Invoke(err);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"数据报错： {remainingPath}");
            }
        });
    }


    bool isDirty = true;
    Queue<Action> tasks = new Queue<Action>();
    void Update()
    {
        if (isDirty)
        {
            isDirty = false;

            while (tasks.Count > 0)
            {
                Action task = tasks.Dequeue();
                task.Invoke();
            }

            isDirty = true;
        }
    }


}