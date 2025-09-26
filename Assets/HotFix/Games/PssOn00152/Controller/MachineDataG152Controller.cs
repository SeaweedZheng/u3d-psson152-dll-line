using GameMaker;
using PssOn00152;
using SimpleJSON;
using SlotMaker;
using System;
using System.Collections.Generic;
using UnityEngine;
using _contentBB = PssOn00152.ContentBlackboard;
using _consoleBB = PssOn00152.ConsoleBlackboard02;
using _mainBB = GameMaker.MainBlackboard;
using Newtonsoft.Json;




/// <summary>
/// 这个即将弃用
/// </summary>
public class MachineDataG152Controller : MonoSingleton<MachineDataG152Controller>
{

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
        Bonus1Ball,
    };
    public enum SBoxGameState
    {
        GSNormal = 0,

        GSStart = 1,
        /// <summary> 普通局且不中线 </summary>
        GSEnd = 2,
        /// <summary> 赢线 </summary>
        GSWinline = 3,
        /// <summary> 免费游戏 </summary>
        GSFreeGame = 4,
        /// <summary> 送球 </summary>
        GSBonus = 5,
        /// <summary> 中了中小彩金 </summary>
        GSJpSmalm = 6,
        /// <summary> 中了大彩金 (弃用)</summary>
        GSJpMajor = 7,
        /// <summary> 中了巨大彩金 (弃用)</summary>
        GSJpGrand = 8,

        GSOperater = 9
    }

    public void ParseSlotSpin(long totalBet, JSONNode res)
    {
        SBoxGameState gameState = (SBoxGameState)((int)res["gameState"]);

        List<int> LineNumbers = new List<int>();
        int lineMark = (int)res["lineMark"];

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

        for (int i = 0; i < res["lineData"].Count; i++)
        {
            JSONNode node = res["lineData"][i];

            int symbolNumber = node["icon"];
            int hitCount = node["link"];
            //int credit = node["reward"];
            int lineNumber = LineNumbers[i];

            winningLines.Add(new WinningLineInfo()
            {
                LineNumber = lineNumber,
                SymbolNumber = symbolNumber,
                WinCount = hitCount,
            });
        }



        bool isJackpotGrand = gameState == SBoxGameState.GSJpGrand;

        bool isJackpotMajor = gameState == SBoxGameState.GSJpMajor;

        bool isJackpotMinMinor = gameState == SBoxGameState.GSJpSmalm;


        bool isBonusBall = gameState == SBoxGameState.GSBonus;

        int freeSpinTotalTimes = (int)res["maxRound"];
        int freeSpinPlayTimes = (int)res["curRound"];

        bool isFreeSpinTrigger = freeSpinPlayTimes == 0 && freeSpinTotalTimes > 0;

        bool isFreeSpinResult = freeSpinTotalTimes > 0 && freeSpinPlayTimes == freeSpinTotalTimes;

        bool isFreeSpin = freeSpinPlayTimes > 0 && freeSpinTotalTimes > 0;

        string strDeckRowCol = "";



        //_contentBB.Instance.jpWinLst = new List<JackpotWinInfo>();

        List<SymbolInclude> symbolInclude = new List<SymbolInclude>();

        if (isJackpotGrand)
        {
            List<JackpotWinInfo> jpWinLst = new List<JackpotWinInfo>();
            _contentBB.Instance.jpWinLst.Add(new JackpotWinInfo()
            {
                name = "grand",
                id = 0,
                winCredit = (int)res["num"],
                whenCredit = 3000,//(int)res["maxRound"],
                curCredit = 3000,//(int)res["maxRound"],
            });

            symbolInclude = new List<SymbolInclude>();
            for (int i = 0; i < 5; i++)
            {
                symbolInclude.Add(new SymbolInclude() { symbolNumber = 12 });
            }

            strDeckRowCol = SlotDeckGenerator.Instance.GenerateGameArray(
                _contentBB.Instance.payLines,
                CustomBlackboard.Instance.symbolNumber, winningLines, new int[] { 11, 10, 12 }, symbolInclude);
        }
        else if (isJackpotMajor)
        {
            int winCredit = (int)res["num"];
            List<JackpotWinInfo> jpWinLst = new List<JackpotWinInfo>();
            _contentBB.Instance.jpWinLst.Add(new JackpotWinInfo()
            {
                name = "major",
                id = 1,
                winCredit = (int)res["num"],
                whenCredit = 3000,//(int)res["maxRound"],
                curCredit = 3000,//(int)res["maxRound"],
            });

 
            symbolInclude = new List<SymbolInclude>();
            for (int i = 0; i < 4; i++)
            {
                symbolInclude.Add(new SymbolInclude() { symbolNumber = 12 });
            }
            

            strDeckRowCol = SlotDeckGenerator.Instance.GenerateGameArray(
                _contentBB.Instance.payLines,
                CustomBlackboard.Instance.symbolNumber, winningLines, new int[] { 11, 10, 12 }, symbolInclude);
        }
        else if (isJackpotMinMinor)
        {
            int winCredit = (int)res["num"];
            List<JackpotWinInfo> jpWinLst = new List<JackpotWinInfo>();
            if (winCredit == 500)
            {
                _contentBB.Instance.jpWinLst.Add(new JackpotWinInfo()
                {
                    name = "mini",
                    id = 4,
                    winCredit = 500,
                    whenCredit = 500,
                    curCredit = 500,
                });
            }
            else if (winCredit == 1000)
            {
                _contentBB.Instance.jpWinLst.Add(new JackpotWinInfo()
                {
                    name = "minor",
                    id = 3,
                    winCredit = 1000,
                    whenCredit = 1000,
                    curCredit = 1000,
                });
            }

            symbolInclude = new List<SymbolInclude>();
            for (int i = 0; i < 3; i++)
            {
                symbolInclude.Add(new SymbolInclude() { symbolNumber = 12 });
            }

            /*if (!isJackpotGrand && !isJackpotMajor)
            {
                symbolInclude = new List<SymbolInclude>();
                for (int i = 0; i < 3; i++)
                {
                    symbolInclude.Add(new SymbolInclude() { symbolNumber = 12 });
                }
            }*/

            strDeckRowCol = SlotDeckGenerator.Instance.GenerateGameArray(
                _contentBB.Instance.payLines,
                CustomBlackboard.Instance.symbolNumber, winningLines, new int[] { 11, 10, 12 }, symbolInclude);

        }
        else if (isBonusBall)
        {
            //List<SymbolInclude> symbolInclude = new List<SymbolInclude>();

            for (int i = 0; i < (int)res["num"]; i++)
            {
                symbolInclude.Add(new SymbolInclude() { symbolNumber = 11 });
            }

            strDeckRowCol = SlotDeckGenerator.Instance.GenerateGameArray(
                _contentBB.Instance.payLines,
                CustomBlackboard.Instance.symbolNumber, winningLines, new int[] { 11, 10, 12 }, symbolInclude);
        }
        else if (isFreeSpinTrigger)
        {
            //List<SymbolInclude> symbolInclude = new List<SymbolInclude>();

            for (int i = 0; i < (int)res["num"]; i++)
            {
                symbolInclude.Add(new SymbolInclude() { symbolNumber = 10 });
            }

            strDeckRowCol = SlotDeckGenerator.Instance.GenerateGameArray(
                _contentBB.Instance.payLines,
                CustomBlackboard.Instance.symbolNumber, winningLines, new int[] { 11, 10, 12 }, symbolInclude);
        }
        else
        {
            strDeckRowCol = SlotDeckGenerator.Instance.GenerateGameArray(
                _contentBB.Instance.payLines,
                CustomBlackboard.Instance.symbolNumber, winningLines, new int[] { 11, 10, 12 }, symbolInclude);
        }

        long creditBefore = MainBlackboardController.Instance.myRealCredit;

        long afterBetCredit = 0;
        long totalEarnCredit = 0;

        if (++_mainBB.Instance.gameNumber < 0)
            _mainBB.Instance.gameNumber = 1;

        _contentBB.Instance.response = res.ToString();

        _contentBB.Instance.strDeckRowCol = strDeckRowCol;

        List<SymbolWin> winList = new List<SymbolWin>();
        for (int i = 0; i < LineNumbers.Count; i++)
        {
            int lineNumber = LineNumbers[i];
            int lineIndex = lineNumber - 1;

            List<int> lineInfo = _contentBB.Instance.payLines[lineIndex];
            //int[] lineInfo = _contentBB.Instance.payLines[lineIndex];

            JSONNode lineNode = res["lineData"][i];
            int credit = lineNode["reward"];
            int hitCount = lineNode["link"];
            int symbolNumber = lineNode["icon"];

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

            totalEarnCredit += credit;
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

        _contentBB.Instance.totalEarnCredit = totalEarnCredit;
        _contentBB.Instance.baseGameWinCredit = totalEarnCredit;


        //_contentBB.Instance.winFreeSpinTriggerOrAddCopy = null;


        _contentBB.Instance.curGameCreatTimeMS = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        _contentBB.Instance.curGameGuid = isFreeSpin
        ? $"{MainBlackboard.Instance.gameID}-{UnityEngine.Random.Range(100, 1000)}-{_contentBB.Instance.curGameCreatTimeMS}-{_mainBB.Instance.gameNumber}-{_contentBB.Instance.gameNumberFreeSpinTrigger}"
        : $"{MainBlackboard.Instance.gameID}-{UnityEngine.Random.Range(100, 1000)}-{_contentBB.Instance.curGameCreatTimeMS}-{_mainBB.Instance.gameNumber}";
        if (!isFreeSpin)
        {
            afterBetCredit = creditBefore - totalBet;
        }
        else
        {
            afterBetCredit = creditBefore;
        }
        // ## MainBlackboardController.Instance.SetMyTempCredit(afterBetCredit, false); // 不同步给ui



        long creditAfter = afterBetCredit + totalEarnCredit;

        if (res.HasKey("creditAfter"))
        {
            creditAfter = res["creditAfter"];
        }
        // ## MainBlackboardController.Instance.SetMyRealCredit(creditAfter);

        Debug.LogWarning(
            $"押注前分数：creditBefore = {creditBefore} 押注分数：{totalBet} 押注后分数:  afterBetCredit = {afterBetCredit}  totalEarnCredit={totalEarnCredit} ");
        Debug.LogWarning($"本次计算 creditAfter= {afterBetCredit + totalEarnCredit}；  算法卡 creditAfter={creditAfter}");


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
        bool isReelsSlowMotion = (deckColRow[0].Contains(1) && deckColRow[1].Contains(1)) ? true : false;
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


        /*
        if (_contentBB.Instance.bonusResult.Count >0 )
        {
           _contentBB.Instance.targetSlotGameEffect = SlotGameEffect.Expectation02;
        }
        else
        {
           _contentBB.Instance.targetSlotGameEffect = isReelsSlowMotion ? SlotGameEffect.Expectation01 :
               isFreeSpin ? SlotGameEffect.FreeSpin : SlotGameEffect.Default;
        }
        */

        // _contentBB.Instance.targetSlotGameEffect = SlotGameEffect.Default;
        //SlotGameEffectManager.Instance.SetEffect(_contentBB.Instance.targetSlotGameEffect);





        SlotGameEffectManager.Instance.SetEffect(
            isReelsSlowMotion ? SlotGameEffect.Expectation01 :
            isFreeSpin ? SlotGameEffect.FreeSpin : SlotGameEffect.Default);


    }




    public void Record()
    {

        // 游戏场景记录
        GameSenceData gameSenceData = new GameSenceData();

        if (++_mainBB.Instance.reportId < 0)
            _mainBB.Instance.reportId = 1;

        gameSenceData.respone = _contentBB.Instance.response;
        gameSenceData.reportId = _mainBB.Instance.reportId;
        gameSenceData.timeS = _contentBB.Instance.curGameCreatTimeMS / 1000;
        gameSenceData.gameNumber = _mainBB.Instance.gameNumber;
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


            int winJPCredit = (int)item.winCredit;

            slotGameRecordItem.jackpot_win_credit = winJPCredit;
            gameSenceData.jackpotWinCredit = winJPCredit;


            long creditBefore = _contentBB.Instance.creditAfter;
            long creditAfter = _contentBB.Instance.creditAfter += winJPCredit;

            _contentBB.Instance.creditAfter = creditAfter;
            gameSenceData.creditAfter = creditAfter;
            slotGameRecordItem.credit_after = creditAfter;


            // 通知算法卡
            MachineDataManager.Instance.NotifyGameJackpot(winJPCredit);
            _consoleBB.Instance.myCredit += winJPCredit;


            // 游戏彩金记录
            TableJackpotRecordAsyncManager.Instance.AddJackpotRecord(item.id, item.name, winJPCredit,
            creditBefore, creditAfter,
            _contentBB.Instance.curGameGuid, _contentBB.Instance.curGameCreatTimeMS);


            // 额外奖上报
            DeviceBonusReport.Instance.ReportBonus(item.name, item.name, winJPCredit, -1, (msg) => { }, (err) => { });

        }

        // 每日营收统计
        TableBusniessDayRecordAsyncManager.Instance.AddTotalBetWin(
            _contentBB.Instance.curReelStripsIndex == "FS" ? 0 : _contentBB.Instance.totalBet,
         _contentBB.Instance.baseGameWinCredit + gameSenceData.jackpotWinCredit, _consoleBB.Instance.myCredit);


        _contentBB.Instance.totalEarnCredit = _contentBB.Instance.baseGameWinCredit + gameSenceData.jackpotWinCredit;
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
            case GlobalEvent.GMBonus1:
                nextSpin = SpinDataType.Bonus1Ball;
                break;
        }

    }


    SpinDataType nextSpin = SpinDataType.None;




    private Dictionary<SpinDataType, List<string[]>> spinDatas = new Dictionary<SpinDataType, List<string[]>>()
    {
        [SpinDataType.FreeSpin] = new List<string[]>()
            {
               new string[]
                {
                    "Assets/HotFix/Games/Mock/Resources/g152/g152__slot_spin__free_0.json",
                    "Assets/HotFix/Games/Mock/Resources/g152/g152__slot_spin__free_1.json",
                    "Assets/HotFix/Games/Mock/Resources/g152/g152__slot_spin__free_2.json",
                    "Assets/HotFix/Games/Mock/Resources/g152/g152__slot_spin__free_3.json",
                    "Assets/HotFix/Games/Mock/Resources/g152/g152__slot_spin__free_4.json",
                    "Assets/HotFix/Games/Mock/Resources/g152/g152__slot_spin__free_5.json",
                    "Assets/HotFix/Games/Mock/Resources/g152/g152__slot_spin__free_6.json",
                },
            },
        [SpinDataType.Normal] = new List<string[]>()
            {
                new string[] { "Assets/HotFix/Games/Mock/Resources/g152/g152__slot_spin__null_0.json" },
                new string[] { "Assets/HotFix/Games/Mock/Resources/g152/g152__slot_spin__win_1.json" },
                new string[] { "Assets/HotFix/Games/Mock/Resources/g152/g152__slot_spin__win_2.json" },
            },
        [SpinDataType.Jp1] = new List<string[]>()
            {
                new string[] { "Assets/HotFix/Games/Mock/Resources/g152/g152__slot_spin__jackpot_grand.json" },
            },
        [SpinDataType.Jp2] = new List<string[]>()
            {
                new string[] { "Assets/HotFix/Games/Mock/Resources/g152/g152__slot_spin__jackpot_major.json" },
            },
        [SpinDataType.Jp3] = new List<string[]>()
            {
                new string[] { "Assets/HotFix/Games/Mock/Resources/g152/g152__slot_spin__jackpot_minor.json" },
            },
        [SpinDataType.Jp4] = new List<string[]>()
            {
                new string[] { "Assets/HotFix/Games/Mock/Resources/g152/g152__slot_spin__jackpot_mini.json" },
            },
        [SpinDataType.Bonus1Ball] = new List<string[]>()
            {
                new string[] { "Assets/HotFix/Games/Mock/Resources/g152/g152__slot_spin__ball_0.json" },
                new string[] { "Assets/HotFix/Games/Mock/Resources/g152/g152__slot_spin__ball_1.json" },
                new string[] { "Assets/HotFix/Games/Mock/Resources/g152/g152__slot_spin__ball_2.json" },
                new string[] { "Assets/HotFix/Games/Mock/Resources/g152/g152__slot_spin__ball_3.json" }
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

            while (tasks.Count>0)
            {
                Action task = tasks.Dequeue();
                task.Invoke();
            }

            isDirty = true;
        }
    }

}
