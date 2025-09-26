#define SQLITE_ASYNC
using Game;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GameMaker;
using Console001;
using Mono.Data.Sqlite;
using System;
using System.Collections;
using _consoleBB = PssOn00152.ConsoleBlackboard02;
using SBoxApi;

public class PageConsoleBusinessRecord :  PageCorBase // BasePageMachineButton
{


    public TextMeshProUGUI tmpTotalBet, tmpTotalWin, tmpTotalProfitBet,tmpRemainPoints, 

        tmpTotalCoinIn, tmpTotalCoinOut, tmpTotalProfitCoinInOut,
        tmpTotalScoreUp, tmpTotalScoreDown, tmpTotalProfitlScoreUpDown,

        tmpBillInfoTime,tmpBillInfoLineNumber, tmpBillInfoHardwareVer,


        tmpDayInOutTotalCoinIn, tmpDayInOutTotalCoinOut, tmpDayInOutTotalProfitlCoinInOut,
        tmpDayInOutTotalScoreUp, tmpDayInOutTotalScoreDown, tmpDayInOutTotalProfitlScoreUpDown,
        tmpDayInOutTotalBillIn, tmpDayInOutTotalPrinterOut,

        tmpPageButtom1,tmpPageButtom2,


        tmpTipDayInOut,tmpTipDayBusniess;




    public GameObject goContainerCoinInOut, goPageButtom1, goPageButtom2;

    public Button btnClose1,btnClose2, btnPrev, btnNext;

    public TMP_Dropdown drpDateCoinInOut;

    public PageController ctrlPage;

    //public ToggleGroup tgPage;

    MessageDelegates onPropertyChangedEventDelegates;


    void Awake()
    {

        onPropertyChangedEventDelegates = new MessageDelegates
         (
             new Dictionary<string, EventDelegate>
             {
                 { "@console/sboxPlayerInfo", OnPropertyChangeSBoxPlayerAccount},
                //{ "@console/historyTotalBet", OnPropertyChangeHistoryTotalBet},
                //{ "@console/historyTotalWin", OnPropertyChangeHistoryTotalWin},
             }
         );


        btnClose1.onClick.AddListener(OnClickClose);
        btnClose2.onClick.AddListener(OnClickClose);
        btnNext.onClick.AddListener(OnClickNext);
        btnPrev.onClick.AddListener(OnClickPrev);

        ctrlPage.pageHandle.AddListener(OnPageChange);

        drpDateCoinInOut.onValueChanged.AddListener(OnDropdownChangedDateCoinInOut);
    }

    private void OnEnable()
    {
        //先不用
        //EventCenter.Instance.AddEventListener<EventData>(ObservableObject.ON_PROPERTY_CHANGED_EVENT, onPropertyChangedEventDelegates.Delegate);

        /*if (++testNumb >= 3)
            testNumb = 0;

        if (testNumb ==0)
        {
            StartCoroutine(InitParam());
        }else if (testNumb == 1)
        {
            InitParam01();
        }
        else
        {
            Debug.Log("No InitParam");
        }*/

        StartCoroutine(InitParam());
    }


    private void OnDisable()
    {
        //先不用
        //EventCenter.Instance.RemoveEventListener<EventData>(ObservableObject.ON_PROPERTY_CHANGED_EVENT, onPropertyChangedEventDelegates.Delegate);
        ClearAllCor();
    }

    /*
    long historyTotalBet = 0;
    long historyTotalWin = 0;
    void OnPropertyChangeHistoryTotalBet(EventData res = null)
    {
        if (res == null)
            historyTotalBet = _consoleBB.Instance.historyTotalBet;
        //BlackboardUtils.GetValue<long>("@console/historyTotalBet");
        else
            historyTotalBet = (long)res.value;
        SetDataTotalWinInfo();
    }

    void OnPropertyChangeHistoryTotalWin(EventData res = null)
    {
        if (res == null)
            historyTotalWin = _consoleBB.Instance.historyTotalWin;
        //BlackboardUtils.GetValue<long>("@console/historyTotalWin");
        else
            historyTotalWin = (long)res.value;
        SetDataTotalWinInfo();
    }*/


    void OnPropertyChangeSBoxPlayerAccount(EventData res = null)
    {
        SBoxPlayerAccount sboxPlayerInfo;
        if (res == null)
            sboxPlayerInfo = _consoleBB.Instance.sboxPlayerInfo;
        else
            sboxPlayerInfo = (SBoxPlayerAccount)res.value;

        SetDataTotalWinInfo();
        SetDataTotalCoinInOutScoreUpDown();
        SetDataBillInfo();
    }
    void SetDataTotalWinInfo()
    {

        tmpTotalBet.text = $"{_consoleBB.Instance.historyTotalBet}";
        tmpTotalWin.text = $"{_consoleBB.Instance.historyTotalWin}";
        tmpRemainPoints.text = $"{_consoleBB.Instance.myCredit}";
        tmpTotalProfitBet.text = $"{_consoleBB.Instance.historyTotalProfitBet}";

        /*
        tmpTotalBet.text = $"{historyTotalBet}";
        tmpTotalWin.text = $"{historyTotalWin}";
        tmpRemainPoints.text = $"{_consoleBB.Instance.myCredit}";
        tmpTotalProfitBet.text = $"{historyTotalBet - historyTotalWin}";
        */
    }

    /// <summary>
    /// 设置账单信息
    /// </summary>
    void SetDataBillInfo()
    {
        tmpBillInfoTime.text = _consoleBB.Instance.billInfoTime;
        tmpBillInfoLineNumber.text = _consoleBB.Instance.billInfoLineMachineNumber;
        tmpBillInfoHardwareVer.text = _consoleBB.Instance.billInfoHardwareAlgorithmVer;
    }


    /// <summary>
    /// 历史总上下分、总投退币
    /// </summary>
    void SetDataTotalCoinInOutScoreUpDown()
    {
        tmpTotalCoinIn.text = $"{ _consoleBB.Instance.historyTotalCoinInCredit}";
        tmpTotalCoinOut.text = $"{ _consoleBB.Instance.historyTotalCoinOutCredit}";
        tmpTotalProfitCoinInOut.text = $"{_consoleBB.Instance.historyTotalProfitCoinIn}";

        tmpTotalScoreUp.text = $"{_consoleBB.Instance.historyTotalScoreUpCredit}";
        tmpTotalScoreDown.text = $"{_consoleBB.Instance.historyTotalScoreDownCredit}";
        tmpTotalProfitlScoreUpDown.text = $"{_consoleBB.Instance.historyTotalProfitScoreUp}";
    }


    void InitCoinInOutPage()
    {
        foreach (Transform tfm  in goContainerCoinInOut.transform)
            tfm.gameObject.SetActive(false);
        InitCoinInOutRecordInfo();
    }

    void InitBussinessPageTotalData()
    {
        //刷新总赢总输数据
        //OnPropertyChangeHistoryTotalBet();  //待优化
        //OnPropertyChangeHistoryTotalWin(); //待优化
        //SetDataBillInfo();

        OnPropertyChangeSBoxPlayerAccount();
    }

    int testNumb = 0;

    IEnumerator InitParam()
    {
        Debug.Log("IEnumerator InitParam");

        // 这里不能使用！
        //ctrlPage.PageSet(0);
        //tgPage.transform.GetChild(0).GetComponent<Toggle>().isOn = true;
        //tgPage.transform.GetChild(0).GetComponent<Toggle>().OnSubmit(null);

        tmpDayInOutTotalCoinIn.text = "0";
        tmpDayInOutTotalCoinOut.text = "0";
        tmpDayInOutTotalProfitlCoinInOut.text = "0";
        tmpDayInOutTotalScoreUp.text = "0";
        tmpDayInOutTotalScoreDown.text = "0";
        tmpDayInOutTotalProfitlScoreUpDown.text = "0";


        tmpTipDayBusniess.text = string.Format(I18nMgr.T("[Note]: Only retain the most recent {0} business day data."), _consoleBB.Instance.businiessDayRecordMax);
        tmpTipDayInOut.text = string.Format(I18nMgr.T("[Note]: Only retain the most recent {0} In-Out data."), _consoleBB.Instance.coinInOutRecordMax);


        yield return new WaitForSeconds(0.1f);

        InitCoinInOutPage();
        //InitBussinessPageTotalData();



        bool isNext = false;
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
                    _consoleBB.Instance.sboxPlayerInfo = playerAccountList[i];

                    MainBlackboardController.Instance.SyncMyTempCreditToReal(false); //同步玩家金币
                    break;
                }
            }
            isNext = true;
        }, (BagelCodeError err) =>
        {
            isNext = true;
            DebugUtils.Log(err.msg);
        });
        yield return new WaitUntil(() => isNext == true);

        // 这里直接读取算法卡数据，不数据表统计
        // StartCoroutine(CheckBussinesRecordTotalCoinInOutScoreUpDown());
        OnPropertyChangeSBoxPlayerAccount();


        ctrlPage.PageSet(0,10);
    }



    /*void InitParam01()
    {

        Debug.Log("void InitParam");

        // 这里不能使用！
        //ctrlPage.PageSet(0);
        //tgPage.transform.GetChild(0).GetComponent<Toggle>().isOn = true;
        //tgPage.transform.GetChild(0).GetComponent<Toggle>().OnSubmit(null);

        tmpDayInOutTotalCoinIn.text = "0";
        tmpDayInOutTotalCoinOut.text = "0";
        tmpDayInOutTotalProfitlCoinInOut.text = "0";
        tmpDayInOutTotalScoreUp.text = "0";
        tmpDayInOutTotalScoreDown.text = "0";
        tmpDayInOutTotalProfitlScoreUpDown.text = "0";


        tmpTipDayBusniess.text = string.Format(I18nMgr.T("[Note]: Only retain the most recent {0} business day data."), _consoleBB.Instance.businiessDayRecordMax);
        tmpTipDayInOut.text = string.Format(I18nMgr.T("[Note]: Only retain the most recent {0} In-Out data."), _consoleBB.Instance.coinInOutRecordMax);


        InitCoinInOutPage();
        //InitBussinessPageTotalData();

        // 这里直接读取算法卡数据，不数据表统计
        // StartCoroutine(CheckBussinesRecordTotalCoinInOutScoreUpDown());
        OnPropertyChangeSBoxPlayerAccount();
        MachineDataManager.Instance.RequestGetPlayerInfo((res) =>
        {
            SBoxAccount data = (SBoxAccount)res;
            int pid = _consoleBB.Instance.pid;
            List<SBoxPlayerAccount> playerAccountList = data.PlayerAccountList;
            for (int i = 0; i < playerAccountList.Count; i++)
            {
                if (playerAccountList[i].PlayerId == pid)
                {
                    _consoleBB.Instance.sboxPlayerInfo = playerAccountList[i];

                    MainBlackboardController.Instance.SyncMyCreditToReal(false); //同步玩家金币
                    return;
                }
            }
        }, (BagelCodeError err) =>
        {
            DebugUtil.Log(err.msg);
        });

        ctrlPage.PageSet(0, 10);
    }*/




    void OnClickClose()=> PageManager.Instance.ClosePage(this);


    List<PageButtomInfo> pageButtomInfo = new List<PageButtomInfo>()
        {
            new PageButtomInfo("Business Record") ,
            new PageButtomInfo("Coin In-Out History, Page {0} of {1}"),
        };


    int pageIndex = 0;
    public void OnPageChange(int index)
    {
        pageIndex = index;
        ChanageButtonUI(pageIndex);
    }

    public void ChanageButtonUI(int pageIndex)
    {
        if (pageIndex == 0)
        {
            goPageButtom2.SetActive(false);
            tmpPageButtom1.text = I18nMgr.T(pageButtomInfo[pageIndex].title);
        }else if (pageIndex == 1)
        {
            goPageButtom2.SetActive(true);
            tmpPageButtom2.text = string.Format(I18nMgr.T(pageButtomInfo[pageIndex].title),
                pageButtomInfo[pageIndex].curPageIndex + 1, pageButtomInfo[pageIndex].totalPageCount);
        }
    }


    #region 营收数据统计

    [Button]
    private void TestGetData()
    {
        /*
        //string sql = "SELECT SUM(credit) AS total_credit FROM coin_in_out_record WHERE in_out = 1;";
        string sql = "SELECT SUM(credit) AS total_credit FROM coin_in_out_record WHERE in_out = 1 AND device_type = 'score_up';";
        SQLiteHelper01.Instance.ExecuteQueryAfterOpenDB(sql, (SqliteDataReader sdr) =>
        {
            while (sdr.Read())
            {
                long d = sdr.GetInt64(0);

                DebugUtil.Log($"@@@ credit = {d}");
                tmpTotalScoreUp.text = $"{d}";
            }
        });

        sql = "SELECT SUM(credit) AS total_credit FROM coin_in_out_record WHERE in_out = 0 AND device_type = 'score_down';";
        SQLiteHelper01.Instance.ExecuteQueryAfterOpenDB(sql, (SqliteDataReader sdr) =>
        {
            while (sdr.Read())
            {
                long d = sdr.GetInt64(0);

                DebugUtil.Log($"@@@ credit = {d}");
                tmpTotalScoreDown.text = $"{d}";
            }
        });
        */

    }






#if !SQLITE_ASYNC

    /// <summary>
    /// 历史总投退币、总上下分
    /// </summary>
    /// <returns></returns>
    IEnumerator CheckBussinesRecordTotalCoinInOutScoreUpDown()
    {

        string dbName = ConsoleTableName.DB_NAME;
        string tableName = ConsoleTableName.TABLE_COIN_IN_OUT_RECORD;


        bool isNext = false;
        SqliteConnection connection = null;

        SQLiteHelper01.Instance.OpenDB(dbName, (conect) =>
        {
            connection = conect;
            isNext = true;
        });

        yield return new WaitUntil(() => isNext == true);
        isNext = false;

        if (connection == null || connection.State != System.Data.ConnectionState.Open)
        {
            DebugUtil.LogError($"【Check Record】{dbName} is close");
            yield break;
        }
        SqliteDataReader dataReader;
        SqliteCommand command;


        // 上下分

        string sql = $"SELECT SUM(credit) AS total_credit FROM {tableName} WHERE in_out = 1 AND device_type = 'score_up';";

        string sqlScoreUp = $"SELECT SUM(credit) AS total_credit FROM {tableName} WHERE in_out = 1 AND (device_type = 'score_up' OR device_type = 'money_box_qrcode_in' OR device_type = 'sas_ticket_in' );";

        command = connection.CreateCommand();
        command.CommandText = sqlScoreUp;// sql;
        dataReader = command.ExecuteReader();
        long totalScoreUp = 0;
        while (dataReader.Read())
        {
            try
            {
                totalScoreUp = dataReader.GetInt64(0);
            }
            catch
            {
                totalScoreUp = 0;
            }
        }
        tmpTotalScoreUp.text = $"{totalScoreUp}";



        sql = $"SELECT SUM(credit) AS total_credit FROM {tableName} WHERE in_out = 0 AND device_type = 'score_down';";

        string sqlScoreDown = $"SELECT SUM(credit) AS total_credit FROM {tableName} WHERE in_out = 0 AND (device_type = 'score_down' OR device_type = 'money_box_qrcode_out' OR device_type = 'sas_ticket_out');";

        command = connection.CreateCommand();
        command.CommandText = sqlScoreDown;// sql;
        dataReader = command.ExecuteReader();
        long totalScoreDown = 0;
        while (dataReader.Read())
        {
            try
            {
                totalScoreDown = dataReader.GetInt64(0);
            }
            catch
            {
                totalScoreDown = 0;
            }
        }
        tmpTotalScoreDown.text = $"{totalScoreDown}";

        tmpTotalProfitlScoreUpDown.text = $"{totalScoreUp - totalScoreDown}";



        // 投退币

        sql = $"SELECT SUM(credit) AS total_credit FROM {tableName} WHERE in_out = 1 AND device_type != 'score_up';";

        string sqlCoinIn = $"SELECT SUM(credit) AS total_credit FROM {tableName} WHERE in_out = 1 AND device_type != 'score_up' AND device_type != 'money_box_qrcode_in' AND device_type != 'sas_ticket_in';";

        command = connection.CreateCommand();
        command.CommandText = sqlCoinIn;// sql;
        dataReader = command.ExecuteReader();
        long totalCoinIn = 0;
        while (dataReader.Read())
        {
            try
            {
                totalCoinIn = dataReader.GetInt64(0);
            }
            catch
            {
                totalCoinIn = 0;
            }
        }
        tmpTotalCoinIn.text = $"{totalCoinIn}";

        sql = $"SELECT SUM(credit) AS total_credit FROM {tableName} WHERE in_out = 0 AND device_type != 'score_down';";

        string sqlCoinOut = $"SELECT SUM(credit) AS total_credit FROM {tableName} WHERE in_out = 0 AND device_type != 'score_down' AND device_type != 'money_box_qrcode_out' AND device_type != 'sas_ticket_out';";

        command = connection.CreateCommand();
        command.CommandText = sqlCoinOut;// sql;
        dataReader = command.ExecuteReader();
        long totalCoinOut = 0;
        while (dataReader.Read())
        {
            try
            {
                totalCoinOut = dataReader.GetInt64(0);
            }
            catch
            {
                totalCoinOut = 0;
            }
        }
        tmpTotalCoinOut.text = $"{totalCoinOut}";

        tmpTotalProfitCoinInOut.text = $"{totalCoinIn - totalCoinOut}";



        //压分总盈利

        tableName = ConsoleTableName.TABLE_SLOT_GAME_RECORD;

        sql = $"SELECT SUM(total_bet) AS total_total_bet FROM {tableName}";
        command = connection.CreateCommand();
        command.CommandText = sql;
        dataReader = command.ExecuteReader();
        long totalBet = 0;
        while (dataReader.Read())
        {
            try
            {
                totalBet = dataReader.GetInt64(0);
            }
            catch
            {
                totalBet = 0;
            }
        }
        tmpTotalBet.text = $"{totalBet}";

        sql = $"SELECT SUM(total_win_credit) AS total_total_win_credit FROM {tableName}";
        command = connection.CreateCommand();
        command.CommandText = sql;
        dataReader = command.ExecuteReader();
        long totalWin = 0;
        while (dataReader.Read())
        {
            try
            {
                totalWin = dataReader.GetInt64(0);
            }
            catch
            {
                totalWin = 0;
            }
        }
        tmpTotalWin.text = $"{totalWin}";

        tmpTotalProfitBet.text = $"{totalBet - totalWin}";

    }
#else
    /// <summary>
    /// 历史总投退币、总上下分
    /// </summary>
    /// <returns></returns>
    IEnumerator CheckBussinesRecordTotalCoinInOutScoreUpDown()
    {

        string dbName = ConsoleTableName.DB_NAME;
        string tableName = ConsoleTableName.TABLE_COIN_IN_OUT_RECORD;


        bool isNext = false;

        if (!SQLiteAsyncHelper.Instance.isConnect)
        {
            DebugUtils.LogError($"【Check Record】{dbName} is close");
            yield break;
        }




        // 上下分

        string sql = $"SELECT SUM(credit) AS total_credit FROM {tableName} WHERE in_out = 1 AND device_type = 'score_up';";

        string sqlScoreUp = $"SELECT SUM(credit) AS total_credit FROM {tableName} WHERE in_out = 1 AND (device_type = 'score_up' OR device_type = 'money_box_qrcode_in' OR device_type = 'sas_ticket_in' );";
        long totalScoreUp = 0;
        SQLiteAsyncHelper.Instance.ExecuteQueryAsync(sqlScoreUp, null, (dataReader) =>
        {
            while (dataReader.Read())
            {
                try
                {
                    totalScoreUp = dataReader.GetInt64(0);
                }
                catch
                {
                    totalScoreUp = 0;
                }
            }
            tmpTotalScoreUp.text = $"{totalScoreUp}";

            isNext = true;
        });
        yield return new WaitUntil(() => isNext == true);
        isNext = false;





        sql = $"SELECT SUM(credit) AS total_credit FROM {tableName} WHERE in_out = 0 AND device_type = 'score_down';";

        string sqlScoreDown = $"SELECT SUM(credit) AS total_credit FROM {tableName} WHERE in_out = 0 AND (device_type = 'score_down' OR device_type = 'money_box_qrcode_out' OR device_type = 'sas_ticket_out');";
        long totalScoreDown = 0;
        SQLiteAsyncHelper.Instance.ExecuteQueryAsync(sqlScoreDown, null, (dataReader) =>
        {
            while (dataReader.Read())
            {
                try
                {
                    totalScoreDown = dataReader.GetInt64(0);
                }
                catch
                {
                    totalScoreDown = 0;
                }
            }
            tmpTotalScoreDown.text = $"{totalScoreDown}";

            tmpTotalProfitlScoreUpDown.text = $"{totalScoreUp - totalScoreDown}";

            isNext = true;
        });
        yield return new WaitUntil(() => isNext == true);
        isNext = false;


        // 投退币

        sql = $"SELECT SUM(credit) AS total_credit FROM {tableName} WHERE in_out = 1 AND device_type != 'score_up';";

        string sqlCoinIn = $"SELECT SUM(credit) AS total_credit FROM {tableName} WHERE in_out = 1 AND device_type != 'score_up' AND device_type != 'money_box_qrcode_in' AND device_type != 'sas_ticket_in';";
        long totalCoinIn = 0;
        SQLiteAsyncHelper.Instance.ExecuteQueryAsync(sqlCoinIn, null, (dataReader) =>
        {
            while (dataReader.Read())
            {
                try
                {
                    totalCoinIn = dataReader.GetInt64(0);
                }
                catch
                {
                    totalCoinIn = 0;
                }
            }
            tmpTotalCoinIn.text = $"{totalCoinIn}";

            isNext = true;
        });
        yield return new WaitUntil(() => isNext == true);
        isNext = false;




        sql = $"SELECT SUM(credit) AS total_credit FROM {tableName} WHERE in_out = 0 AND device_type != 'score_down';";

        string sqlCoinOut = $"SELECT SUM(credit) AS total_credit FROM {tableName} WHERE in_out = 0 AND device_type != 'score_down' AND device_type != 'money_box_qrcode_out' AND device_type != 'sas_ticket_out';";
        long totalCoinOut = 0;
        SQLiteAsyncHelper.Instance.ExecuteQueryAsync(sqlCoinOut, null, (dataReader) =>
        {
            while (dataReader.Read())
            {
                try
                {
                    totalCoinOut = dataReader.GetInt64(0);
                }
                catch
                {
                    totalCoinOut = 0;
                }
            }
            tmpTotalCoinOut.text = $"{totalCoinOut}";

            tmpTotalProfitCoinInOut.text = $"{totalCoinIn - totalCoinOut}";

            isNext = true;
        });
        yield return new WaitUntil(() => isNext == true);
        isNext = false;



        //压分总盈利

        tableName = ConsoleTableName.TABLE_SLOT_GAME_RECORD;

        long totalBet = 0;

        sql = $"SELECT SUM(total_bet) AS total_total_bet FROM {tableName}";
        SQLiteAsyncHelper.Instance.ExecuteQueryAsync(sql, null, (dataReader) =>
        {
            while (dataReader.Read())
            {
                try
                {
                    totalBet = dataReader.GetInt64(0);
                }
                catch
                {
                    totalBet = 0;
                }
            }
            tmpTotalBet.text = $"{totalBet}";

            isNext = true;
        });
        yield return new WaitUntil(() => isNext == true);
        isNext = false;


        long totalWin = 0;
        sql = $"SELECT SUM(total_win_credit) AS total_total_win_credit FROM {tableName}";
        SQLiteAsyncHelper.Instance.ExecuteQueryAsync(sql, null, (dataReader) =>
        {
            while (dataReader.Read())
            {
                try
                {
                    totalWin = dataReader.GetInt64(0);
                }
                catch
                {
                    totalWin = 0;
                }
            }
            tmpTotalWin.text = $"{totalWin}";

            tmpTotalProfitBet.text = $"{totalBet - totalWin}";

            isNext = true;
        });
        yield return new WaitUntil(() => isNext == true);
        isNext = false;



    }

#endif






    #endregion



    #region 投退币记录

    const int perPageNumCoinInOut = 10;
    int fromIdxCoinInOut = 0;
    const string TABLE_COIN_IN_OUT_RECORD = "coin_in_out_record";
    const string FORMAT_DATE_SECOND = "yyyy-MM-dd HH:mm:ss";
    const string FORMAT_DATE_DAY = "yyyy-MM-dd";
    List<string> dropdownDateLst;
    List<TableCoinInOutRecordItem> resCoinInOutRecord = new List<TableCoinInOutRecordItem>();




#if !SQLITE_ASYNC

    /// <summary>
    /// 投退币历史记录
    /// </summary>
    void InitCoinInOutRecordInfo()
    {
        //string sql = $"select distinct date(created_at) from {tableName}";
        string sql = $"SELECT created_at FROM {TABLE_COIN_IN_OUT_RECORD}";

        DebugUtil.Log(sql);
        List<long> date = new List<long>();
        dropdownDateLst = new List<string>();
        SQLiteHelper01.Instance.ExecuteQueryAfterOpenDB(sql, (SqliteDataReader sdr) =>
        {
            while (sdr.Read())
            {
                long d = sdr.GetInt64(0);
                date.Add(d);
            }
            foreach (long timestamp in date)
            {
                //DebugUtil.Log($"时间搓：{timestamp}");
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(timestamp);
                //DateTime utcDateTime = dateTimeOffset.UtcDateTime;
                //string time = utcDateTime.ToString(FORMAT_DATE_DAY);

                DateTime localDateTime = dateTimeOffset.LocalDateTime;
                string time = localDateTime.ToString(FORMAT_DATE_DAY);

                if (!dropdownDateLst.Contains(time))
                {
                    //dropdownDateLst.Add(time);
                    DebugUtil.Log($"时间搓：{timestamp} 时间 ：{time}");
                    dropdownDateLst.Insert(0,time); //最新排在最前
                }
            }

            drpDateCoinInOut.options.Clear();
            if (dropdownDateLst.Count > 0)
            {
                drpDateCoinInOut.AddOptions(dropdownDateLst);
                drpDateCoinInOut.RefreshShownValue();// 最后，刷新Dropdown以应用更改
                drpDateCoinInOut.value = 0;
                OnDropdownChangedDateCoinInOut(0);
            }
        });


    }



#else
    /// <summary>
    /// 投退币历史记录
    /// </summary>
    void InitCoinInOutRecordInfo()
    {
        //string sql = $"select distinct date(created_at) from {tableName}";
        string sql = $"SELECT created_at FROM {TABLE_COIN_IN_OUT_RECORD}";

        DebugUtils.Log(sql);
        List<long> date = new List<long>();
        dropdownDateLst = new List<string>();
        SQLiteAsyncHelper.Instance.ExecuteQueryAsync(sql, null, (SqliteDataReader sdr) =>
        {
            while (sdr.Read())
            {
                long d = sdr.GetInt64(0);
                date.Add(d);
            }
            foreach (long timestamp in date)
            {
                //DebugUtil.Log($"时间搓：{timestamp}");
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(timestamp);
                //DateTime utcDateTime = dateTimeOffset.UtcDateTime;
                //string time = utcDateTime.ToString(FORMAT_DATE_DAY);

                DateTime localDateTime = dateTimeOffset.LocalDateTime;
                string time = localDateTime.ToString(FORMAT_DATE_DAY);

                if (!dropdownDateLst.Contains(time))
                {
                    //dropdownDateLst.Add(time);
                    DebugUtils.Log($"时间搓：{timestamp} 时间 ：{time}");
                    dropdownDateLst.Insert(0, time); //最新排在最前
                }
            }

            drpDateCoinInOut.options.Clear();
            if (dropdownDateLst.Count > 0)
            {
                drpDateCoinInOut.AddOptions(dropdownDateLst);
                drpDateCoinInOut.RefreshShownValue();// 最后，刷新Dropdown以应用更改
                drpDateCoinInOut.value = 0;
                OnDropdownChangedDateCoinInOut(0);
            }
        });


    }



#endif


    void OnClickNext()
    {
        if (pageIndex == 1)
        {
            OnNextCoinInOutRecord();
        }
        OnPageChange(pageIndex);
    }

    void OnClickPrev()
    {
        if (pageIndex == 1)
        {
            OnPrevCoinInOutRecord();
        }
        OnPageChange(pageIndex);
    }

    private void OnNextCoinInOutRecord()
    {
        if (fromIdxCoinInOut + perPageNumCoinInOut >= resCoinInOutRecord.Count)
            return;

        fromIdxCoinInOut += perPageNumCoinInOut;
        pageButtomInfo[1].curPageIndex++;
        SetUICoinInOut();
    }

    private void OnPrevCoinInOutRecord()
    {
        if (fromIdxCoinInOut <= 0)
            return;

        fromIdxCoinInOut -= perPageNumCoinInOut;
        pageButtomInfo[1].curPageIndex--;
        if (fromIdxCoinInOut < 0)
        {
            pageButtomInfo[1].curPageIndex = 0;
            fromIdxCoinInOut = 0;
        }
        SetUICoinInOut();
    }
    void SetUICoinInOut()
    {
        int lastIdx = fromIdxCoinInOut + perPageNumCoinInOut - 1;
        if (lastIdx > resCoinInOutRecord.Count - 1)
        {
            lastIdx = resCoinInOutRecord.Count - 1;
        }

        Transform tfmMiddle = goContainerCoinInOut.transform;

        foreach (Transform item in tfmMiddle)
        {
            item.gameObject.SetActive(false);
        }
        for (int i = 0; i <= lastIdx - fromIdxCoinInOut; i++)
        {
            Transform item = tfmMiddle.GetChild(i);
            item.gameObject.SetActive(true);
            TableCoinInOutRecordItem res = resCoinInOutRecord[i + fromIdxCoinInOut];

            item.Find("In Out/Text").GetComponent<TextMeshProUGUI>().text = I18nMgr.T(res.in_out == 1 ? "In" : "Out");

            /*
            if (res.as_money > 0)
                item.Find("Credit/Text").GetComponent<TextMeshProUGUI>().text = "$" + res.as_money.ToString();
            else
                item.Find("Credit/Text").GetComponent<TextMeshProUGUI>().text = res.credit.ToString();
            */

            item.Find("Credit/Text").GetComponent<TextMeshProUGUI>().text = res.credit.ToString();


            //item.Find("Device/Text1").GetComponent<TextMeshProUGUI>().text = GetDeviceName(res.device_type);

            item.Find("Device/Text").GetComponent<TextMeshProUGUI>().text = I18nMgr.T(res.device_type);

            item.Find("Before Credit/Text").GetComponent<TextMeshProUGUI>().text = res.credit_before.ToString();
            item.Find("After Credit/Text").GetComponent<TextMeshProUGUI>().text = res.credit_after.ToString();

            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(res.created_at);
            //DateTime utcDateTime = dateTimeOffset.UtcDateTime;
            DateTime localDateTime = dateTimeOffset.LocalDateTime;
            item.Find("Date/Text").GetComponent<TextMeshProUGUI>().text = localDateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }



#if !SQLITE_ASYNC
    void OnDropdownChangedDateCoinInOut(int index)
    {

        // 根据选中的值进行相应的处理
        DebugUtil.Log($"Selected item index: {drpDateCoinInOut.value}  index:{index}");


        //string sql2 = "SELECT * FROM your_table_name WHERE DATE(DATETIME(created_at / 1000, 'unixepoch', 'localtime')) = '2024-12-10'";
        string sql2 = $"SELECT * FROM {TABLE_COIN_IN_OUT_RECORD} WHERE DATE(DATETIME(created_at / 1000, 'unixepoch', 'localtime')) = '{dropdownDateLst[index]}'"; //可以用
                                                                                                                                                                   //string sql = $"SELECT * FROM {TABLE_COIN_IN_OUT_RECORD} WHERE DATE(created_at) = '{dropdownDateLst[index]}'"; //不可以用
        DebugUtil.Log(sql2);
        SQLiteHelper01.Instance.ExecuteQueryAfterOpenDB(sql2, (SqliteDataReader sdr) =>
        {

            resCoinInOutRecord = new List<TableCoinInOutRecordItem>();

            int i = 0;
            while (sdr.Read())
            {
                //string d = sdr.ge(0);
                //date.Add(d);

                //d.date = sdr.GetString(sdr.GetOrdinal("All_Report1"));
                //d.score = sdr.GetString(sdr.GetOrdinal("All_Report2"));
                //d.jpType = sdr.GetString(sdr.GetOrdinal("All_Report21"));

                //string res =   sdr.GetString(sdr.GetOrdinal("device_type"));
                //DebugUtil.Log($" device_type = {res}");



                /*
                 
               GetInt64：对应于 long 类型，表示 64 位有符号整数。 -9,223,372,036,854,775,808 到 9,223,372,036,854,775,807。
               GetInt32：对应于 int 类型，表示 32 位有符号整数。  -2,147,483,648 到 2,147,483,647。
               GetInt16：对应于 short 类型，表示 16 位有符号整数。-32,768 到 32,767。
                 */

                //DebugUtil.Log($"  i = {i}    value {res}");

                resCoinInOutRecord.Insert(0,
                new TableCoinInOutRecordItem()
                {
                    device_type = sdr.GetString(sdr.GetOrdinal("device_type")),
                    count = sdr.GetInt64(sdr.GetOrdinal("count")),
                    as_money = sdr.GetInt64(sdr.GetOrdinal("as_money")),
                    credit = sdr.GetInt64(sdr.GetOrdinal("credit")),
                    credit_before = sdr.GetInt64(sdr.GetOrdinal("credit_before")),
                    credit_after = sdr.GetInt64(sdr.GetOrdinal("credit_after")),
                    order_id = sdr.GetString(sdr.GetOrdinal("order_id")),
                    created_at = sdr.GetInt64(sdr.GetOrdinal("created_at")),
                    in_out = sdr.GetInt32(sdr.GetOrdinal("in_out")),
                });
            }

            pageButtomInfo[1].curPageIndex = 0;
            int totalPageCount = (resCoinInOutRecord.Count + (perPageNumCoinInOut - 1)) / perPageNumCoinInOut; //向上取整
            pageButtomInfo[1].totalPageCount = totalPageCount;
            fromIdxCoinInOut = 0;
            SetUICoinInOut();

            ChanageButtonUI(pageIndex);

            DoCor(COR_IN_OUT_RECORD_YYYYMMDD, SetInOutTotal(dropdownDateLst[index]));
        });
    }


#else

    void OnDropdownChangedDateCoinInOut(int index)
    {

        // 根据选中的值进行相应的处理
        DebugUtils.Log($"Selected item index: {drpDateCoinInOut.value}  index:{index}");


        //string sql2 = "SELECT * FROM your_table_name WHERE DATE(DATETIME(created_at / 1000, 'unixepoch', 'localtime')) = '2024-12-10'";
        string sql2 = $"SELECT * FROM {TABLE_COIN_IN_OUT_RECORD} WHERE DATE(DATETIME(created_at / 1000, 'unixepoch', 'localtime')) = '{dropdownDateLst[index]}'"; //可以用
                                                                                                                                                                  //string sql = $"SELECT * FROM {TABLE_COIN_IN_OUT_RECORD} WHERE DATE(created_at) = '{dropdownDateLst[index]}'"; //不可以用
        DebugUtils.Log(sql2);
        SQLiteAsyncHelper.Instance.ExecuteQueryAsync(sql2, null, (SqliteDataReader sdr) =>
        {

            resCoinInOutRecord = new List<TableCoinInOutRecordItem>();

            int i = 0;
            while (sdr.Read())
            {
                //string d = sdr.ge(0);
                //date.Add(d);

                //d.date = sdr.GetString(sdr.GetOrdinal("All_Report1"));
                //d.score = sdr.GetString(sdr.GetOrdinal("All_Report2"));
                //d.jpType = sdr.GetString(sdr.GetOrdinal("All_Report21"));

                //string res =   sdr.GetString(sdr.GetOrdinal("device_type"));
                //DebugUtil.Log($" device_type = {res}");



                /*
                 
               GetInt64：对应于 long 类型，表示 64 位有符号整数。 -9,223,372,036,854,775,808 到 9,223,372,036,854,775,807。
               GetInt32：对应于 int 类型，表示 32 位有符号整数。  -2,147,483,648 到 2,147,483,647。
               GetInt16：对应于 short 类型，表示 16 位有符号整数。-32,768 到 32,767。
                 */

                //DebugUtil.Log($"  i = {i}    value {res}");

                resCoinInOutRecord.Insert(0,
                new TableCoinInOutRecordItem()
                {
                    device_type = sdr.GetString(sdr.GetOrdinal("device_type")),
                    count = sdr.GetInt64(sdr.GetOrdinal("count")),
                    as_money = sdr.GetInt64(sdr.GetOrdinal("as_money")),
                    credit = sdr.GetInt64(sdr.GetOrdinal("credit")),
                    credit_before = sdr.GetInt64(sdr.GetOrdinal("credit_before")),
                    credit_after = sdr.GetInt64(sdr.GetOrdinal("credit_after")),
                    order_id = sdr.GetString(sdr.GetOrdinal("order_id")),
                    created_at = sdr.GetInt64(sdr.GetOrdinal("created_at")),
                    in_out = sdr.GetInt32(sdr.GetOrdinal("in_out")),
                });
            }

            pageButtomInfo[1].curPageIndex = 0;
            int totalPageCount = (resCoinInOutRecord.Count + (perPageNumCoinInOut - 1)) / perPageNumCoinInOut; //向上取整
            pageButtomInfo[1].totalPageCount = totalPageCount;
            fromIdxCoinInOut = 0;
            SetUICoinInOut();

            ChanageButtonUI(pageIndex);

            DoCor(COR_IN_OUT_RECORD_YYYYMMDD, SetInOutTotal(dropdownDateLst[index]));
        });
    }

#endif




    const string COR_IN_OUT_RECORD_YYYYMMDD = "COR_IN_OUT_RECORD_YYYYMMDD";





#if !SQLITE_ASYNC

    IEnumerator SetInOutTotal(string yyyyMMdd)
    {

        string dbName = ConsoleTableName.DB_NAME; 
        string tableName = ConsoleTableName.TABLE_COIN_IN_OUT_RECORD;


        bool isNext = false;
        SqliteConnection connection = null;

        SQLiteHelper01.Instance.OpenDB(dbName, (conect) =>
        {
            connection = conect;
            isNext = true;
        });

        yield return new WaitUntil(() => isNext == true);
        isNext = false;

        if (connection == null || connection.State != System.Data.ConnectionState.Open)
        {
            DebugUtil.LogError($"【Check Record】{dbName} is close");
            yield break;
        }
        SqliteDataReader dataReader;
        SqliteCommand command;


        string sql = $"SELECT SUM(credit) AS total_credit FROM {tableName} WHERE in_out = 1 AND device_type = 'score_up' AND DATE(DATETIME(created_at / 1000, 'unixepoch', 'localtime')) = '{yyyyMMdd}';";
        command = connection.CreateCommand();
        command.CommandText = sql;
        dataReader = command.ExecuteReader();
        long totalScoreUp = 0;
        while (dataReader.Read())
        {
            try
            {
                totalScoreUp = dataReader.GetInt64(0);
            }
            catch
            {
                totalScoreUp = 0;
            }
        }
        tmpDayInOutTotalScoreUp.text = $"{totalScoreUp}";


        sql = $"SELECT SUM(credit) AS total_credit FROM {tableName} WHERE in_out = 0 AND device_type = 'score_down' AND DATE(DATETIME(created_at / 1000, 'unixepoch', 'localtime')) = '{yyyyMMdd}';";
        command = connection.CreateCommand();
        command.CommandText = sql;
        dataReader = command.ExecuteReader();
        long totalScoreDown = 0;
        while (dataReader.Read())
        {
            try
            {
                totalScoreDown = dataReader.GetInt64(0);
            }
            catch
            {
                totalScoreDown = 0;
            }
        }
        tmpDayInOutTotalScoreDown.text = $"{totalScoreDown}";

        tmpDayInOutTotalProfitlScoreUpDown.text = $"{totalScoreUp - totalScoreDown}";

        sql = $"SELECT SUM(credit) AS total_credit FROM {tableName} WHERE in_out = 1 AND device_type != 'score_up' AND DATE(DATETIME(created_at / 1000, 'unixepoch', 'localtime')) = '{yyyyMMdd}';";
        command = connection.CreateCommand();
        command.CommandText = sql;
        dataReader = command.ExecuteReader();
        long totalCoinIn = 0;
        while (dataReader.Read())
        {
            try
            {
                totalCoinIn = dataReader.GetInt64(0);
            }
            catch
            {
                totalCoinIn = 0;
            }
        }
        tmpDayInOutTotalCoinIn.text = $"{totalCoinIn}";


        sql = $"SELECT SUM(credit) AS total_credit FROM {tableName} WHERE in_out = 0 AND device_type != 'score_down' AND DATE(DATETIME(created_at / 1000, 'unixepoch', 'localtime')) = '{yyyyMMdd}';";
        command = connection.CreateCommand();
        command.CommandText = sql;
        dataReader = command.ExecuteReader();
        long totalCoinOut = 0;
        while (dataReader.Read())
        {
            try
            {
                totalCoinOut = dataReader.GetInt64(0);
            }
            catch
            {
                totalCoinOut = 0;
            }
        }
        tmpDayInOutTotalCoinOut.text = $"{totalCoinOut}";

        tmpDayInOutTotalProfitlCoinInOut.text = $"{totalCoinIn - totalCoinOut}";

    }

#else



    IEnumerator SetInOutTotal(string yyyyMMdd)
    {

        string dbName = ConsoleTableName.DB_NAME;
        string tableName = ConsoleTableName.TABLE_COIN_IN_OUT_RECORD;


        bool isNext = false;

        if (!SQLiteAsyncHelper.Instance.isConnect)
        {
            DebugUtils.LogError($"【Check Record】{dbName} is close");
            yield break;
        }

        long totalScoreUp = 0;
        string sql = $"SELECT SUM(credit) AS total_credit FROM {tableName} WHERE in_out = 1 AND device_type = 'score_up' AND DATE(DATETIME(created_at / 1000, 'unixepoch', 'localtime')) = '{yyyyMMdd}';";
        SQLiteAsyncHelper.Instance.ExecuteQueryAsync(sql, null, (dataReader) =>
        {
            while (dataReader.Read())
            {
                try
                {
                    totalScoreUp = dataReader.GetInt64(0);
                }
                catch
                {
                    totalScoreUp = 0;
                }
            }
            tmpDayInOutTotalScoreUp.text = $"{totalScoreUp}";
            isNext = true;
        });
        yield return new WaitUntil(() => isNext == true);
        isNext = false;


        long totalScoreDown = 0;
        sql = $"SELECT SUM(credit) AS total_credit FROM {tableName} WHERE in_out = 0 AND device_type = 'score_down' AND DATE(DATETIME(created_at / 1000, 'unixepoch', 'localtime')) = '{yyyyMMdd}';";
        SQLiteAsyncHelper.Instance.ExecuteQueryAsync(sql, null, (dataReader) =>
        {
            while (dataReader.Read())
            {
                try
                {
                    totalScoreDown = dataReader.GetInt64(0);
                }
                catch
                {
                    totalScoreDown = 0;
                }
            }
            tmpDayInOutTotalScoreDown.text = $"{totalScoreDown}";

            tmpDayInOutTotalProfitlScoreUpDown.text = $"{totalScoreUp - totalScoreDown}";

            isNext = true;
        });
        yield return new WaitUntil(() => isNext == true);
        isNext = false;


        long totalCoinIn = 0;
        sql = $"SELECT SUM(credit) AS total_credit FROM {tableName} WHERE in_out = 1 AND device_type != 'score_up' AND DATE(DATETIME(created_at / 1000, 'unixepoch', 'localtime')) = '{yyyyMMdd}';";
        SQLiteAsyncHelper.Instance.ExecuteQueryAsync(sql, null, (dataReader) =>
        {
            while (dataReader.Read())
            {
                try
                {
                    totalCoinIn = dataReader.GetInt64(0);
                }
                catch
                {
                    totalCoinIn = 0;
                }
            }
            tmpDayInOutTotalCoinIn.text = $"{totalCoinIn}";

            isNext = true;
        });
        yield return new WaitUntil(() => isNext == true);
        isNext = false;


        long totalCoinOut = 0;
        sql = $"SELECT SUM(credit) AS total_credit FROM {tableName} WHERE in_out = 0 AND device_type != 'score_down' AND DATE(DATETIME(created_at / 1000, 'unixepoch', 'localtime')) = '{yyyyMMdd}';";
        SQLiteAsyncHelper.Instance.ExecuteQueryAsync(sql, null, (dataReader) =>
        {
            while (dataReader.Read())
            {
                try
                {
                    totalCoinOut = dataReader.GetInt64(0);
                }
                catch
                {
                    totalCoinOut = 0;
                }
            }
            tmpDayInOutTotalCoinOut.text = $"{totalCoinOut}";

            tmpDayInOutTotalProfitlCoinInOut.text = $"{totalCoinIn - totalCoinOut}";

            isNext = true;
        });
        yield return new WaitUntil(() => isNext == true);
        isNext = false;

    }

#endif





    #endregion
}
