#define SQLITE_ASYNC
using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DayBusinessRecordController001 : CorBehaviour
{

    public bool isShowGameRecord = true;

    public bool isShowConiInOutRecord = true;

    public TMP_Dropdown drpDateBusinessDayRecord;

    public TextMeshProUGUI tmpTotalBet, tmpTotalWin, tmpTotalProfitBet,

        tmpTotalCoinIn, tmpTotalCoinOut, tmpTotalProfitCoinInOut,

        tmpTotalScoreUp, tmpTotalScoreDown, tmpTotalProfitlScoreUpDown,

        tmpTotalBillIn, tmpTotalPrinterOut;


    public UnityEvent<string> OnClickData;

    void Awake()
    {
        drpDateBusinessDayRecord.onValueChanged.AddListener(OnDropdownChangedDateBusinessDayRecord);
    }

    private void OnEnable()
    {
        InitParam();
    }

    void InitParam()
    {

        ClearAllUI();

        InitBusinessDayRecordInfo();
    }





    const string FORMAT_DATE_DAY = "yyyy-MM-dd";


    void ClearAllUI()
    {
        tmpTotalBet.text = "0";
        tmpTotalWin.text = "0";
        tmpTotalProfitBet.text = "0";

        tmpTotalCoinIn.text = "0";
        tmpTotalCoinOut.text = "0";
        tmpTotalProfitCoinInOut.text = "0";

        tmpTotalScoreUp.text = "0";
        tmpTotalScoreDown.text = "0";
        tmpTotalProfitlScoreUpDown.text = "0";

        tmpTotalBillIn.text = "0"; 
        tmpTotalPrinterOut.text = "0";
    }





    List<string> dropdownDateLstBusniessDayRecord;



#if !SQLITE_ASYNC
    void InitBusinessDayRecordInfo()
    {
        dropdownDateLstBusniessDayRecord = new List<string>();


        //string sql = $"select distinct date(created_at) from {tableName}";
        string sql = $"SELECT created_at FROM {ConsoleTableName.TABLE_BUSINESS_DAY_RECORD}";
        DebugUtil.Log(sql);
        List<long> date = new List<long>();
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

                if (!dropdownDateLstBusniessDayRecord.Contains(time))
                {
                    //dropdownDateLst.Add(time);
                    DebugUtil.Log($"时间搓：{timestamp} 时间 ：{time}");
                    dropdownDateLstBusniessDayRecord.Insert(0, time); //最新排在最前
                }
            }

        });


        drpDateBusinessDayRecord.options.Clear();
        if (dropdownDateLstBusniessDayRecord.Count > 0)
        {
            drpDateBusinessDayRecord.AddOptions(dropdownDateLstBusniessDayRecord);
            drpDateBusinessDayRecord.RefreshShownValue();// 最后，刷新Dropdown以应用更改
            drpDateBusinessDayRecord.value = 0;
            OnDropdownChangedDateBusinessDayRecord(0);
        }
    }

#else
    void InitBusinessDayRecordInfo()
    {
        dropdownDateLstBusniessDayRecord = new List<string>();


        //string sql = $"select distinct date(created_at) from {tableName}";
        string sql = $"SELECT created_at FROM {ConsoleTableName.TABLE_BUSINESS_DAY_RECORD}";
        DebugUtils.Log(sql);
        List<long> date = new List<long>();

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

                if (!dropdownDateLstBusniessDayRecord.Contains(time))
                {
                    //dropdownDateLst.Add(time);
                    DebugUtils.Log($"时间搓：{timestamp} 时间 ：{time}");
                    dropdownDateLstBusniessDayRecord.Insert(0, time); //最新排在最前
                }
            }



            drpDateBusinessDayRecord.options.Clear();
            if (dropdownDateLstBusniessDayRecord.Count > 0)
            {
                drpDateBusinessDayRecord.AddOptions(dropdownDateLstBusniessDayRecord);
                drpDateBusinessDayRecord.RefreshShownValue();// 最后，刷新Dropdown以应用更改
                drpDateBusinessDayRecord.value = 0;
                OnDropdownChangedDateBusinessDayRecord(0);
            }

        });



    }

#endif


    const string COR_BUSINESS_DAY_RECORD_YYYYMMDD = "COR_BUSINESS_DAY_RECORD_YYYYMMDD";
    void OnDropdownChangedDateBusinessDayRecord(int index)
    {
        DoCor(COR_BUSINESS_DAY_RECORD_YYYYMMDD, CheckBusinessDayRecord(dropdownDateLstBusniessDayRecord[index]));
    }






#if !SQLITE_ASYNC
    IEnumerator CheckBusinessDayRecord(string yyyyMMdd)
    {

        string dbName = ConsoleTableName.DB_NAME; 
        string tableName = ConsoleTableName.TABLE_BUSINESS_DAY_RECORD;

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
        SqliteDataReader sdr;
        SqliteCommand command;


        string sql = $"SELECT * FROM {tableName} WHERE DATE(DATETIME(created_at / 1000, 'unixepoch', 'localtime')) = '{yyyyMMdd}';";
        command = connection.CreateCommand();
        command.CommandText = sql;
        sdr = command.ExecuteReader();
        TableBussinessDayRecordItem bussinessDayRecord = new TableBussinessDayRecordItem();
        while (sdr.Read())
        {
            bussinessDayRecord = new TableBussinessDayRecordItem()
            {
                credit_before = sdr.GetInt32(sdr.GetOrdinal("credit_before")),
                credit_after = sdr.GetInt32(sdr.GetOrdinal("credit_after")),
                total_bet_credit = sdr.GetInt32(sdr.GetOrdinal("total_bet_credit")),
                total_win_credit = sdr.GetInt32(sdr.GetOrdinal("total_win_credit")),
                total_coin_in_credit = sdr.GetInt32(sdr.GetOrdinal("total_coin_in_credit")),
                total_coin_out_credit = sdr.GetInt32(sdr.GetOrdinal("total_coin_out_credit")),
                total_score_up_credit = sdr.GetInt32(sdr.GetOrdinal("total_score_up_credit")),
                total_score_down_credit = sdr.GetInt32(sdr.GetOrdinal("total_score_down_credit")),
                total_bill_in_credit = sdr.GetInt32(sdr.GetOrdinal("total_bill_in_credit")),
                total_bill_in_as_money = sdr.GetInt32(sdr.GetOrdinal("total_bill_in_as_money")),
                total_printer_out_credit = sdr.GetInt32(sdr.GetOrdinal("total_printer_out_credit")),
                total_printer_out_as_money = sdr.GetInt32(sdr.GetOrdinal("total_printer_out_as_money")),
                created_at = sdr.GetInt32(sdr.GetOrdinal("created_at")),
            };
        }



        tmpTotalBet.text = $"{bussinessDayRecord.total_bet_credit}";

        tmpTotalWin.text = $"{bussinessDayRecord.total_win_credit}";

        tmpTotalProfitBet.text = $"{bussinessDayRecord.total_bet_credit - bussinessDayRecord.total_win_credit}";

        tmpTotalScoreDown.text = $"{bussinessDayRecord.total_score_down_credit}";

        tmpTotalScoreUp.text = $"{bussinessDayRecord.total_score_up_credit}";

        tmpTotalProfitlScoreUpDown.text = $"{bussinessDayRecord.total_score_up_credit - bussinessDayRecord.total_score_down_credit}";



        tmpTotalCoinOut.text = $"{bussinessDayRecord.total_coin_out_credit}";

        tmpTotalCoinIn.text = $"{bussinessDayRecord.total_coin_in_credit}";

        tmpTotalProfitCoinInOut.text = $"{bussinessDayRecord.total_coin_in_credit - bussinessDayRecord.total_coin_out_credit}";


        tmpTotalBillIn.text = $"{bussinessDayRecord.total_bill_in_as_money}";
        tmpTotalPrinterOut.text = $"{bussinessDayRecord.total_printer_out_as_money}";
    }


#else
    IEnumerator CheckBusinessDayRecord(string yyyyMMdd)
    {

        string dbName = ConsoleTableName.DB_NAME;
        string tableName = ConsoleTableName.TABLE_BUSINESS_DAY_RECORD;

        bool isNext = false;


        if (!SQLiteAsyncHelper.Instance.isConnect)
        {
            DebugUtils.LogError($"【Check Record】{dbName} is close");
            yield break;
        }

        string sql = $"SELECT * FROM {tableName} WHERE DATE(DATETIME(created_at / 1000, 'unixepoch', 'localtime')) = '{yyyyMMdd}';";
        SQLiteAsyncHelper.Instance.ExecuteQueryAsync(sql, null, (sdr) =>
        {

            TableBussinessDayRecordItem bussinessDayRecord = new TableBussinessDayRecordItem();
            while (sdr.Read())
            {
                bussinessDayRecord = new TableBussinessDayRecordItem()
                {
                    credit_before = sdr.GetInt32(sdr.GetOrdinal("credit_before")),
                    credit_after = sdr.GetInt32(sdr.GetOrdinal("credit_after")),
                    total_bet_credit = sdr.GetInt32(sdr.GetOrdinal("total_bet_credit")),
                    total_win_credit = sdr.GetInt32(sdr.GetOrdinal("total_win_credit")),
                    total_coin_in_credit = sdr.GetInt32(sdr.GetOrdinal("total_coin_in_credit")),
                    total_coin_out_credit = sdr.GetInt32(sdr.GetOrdinal("total_coin_out_credit")),
                    total_score_up_credit = sdr.GetInt32(sdr.GetOrdinal("total_score_up_credit")),
                    total_score_down_credit = sdr.GetInt32(sdr.GetOrdinal("total_score_down_credit")),
                    total_bill_in_credit = sdr.GetInt32(sdr.GetOrdinal("total_bill_in_credit")),
                    total_bill_in_as_money = sdr.GetInt32(sdr.GetOrdinal("total_bill_in_as_money")),
                    total_printer_out_credit = sdr.GetInt32(sdr.GetOrdinal("total_printer_out_credit")),
                    total_printer_out_as_money = sdr.GetInt32(sdr.GetOrdinal("total_printer_out_as_money")),
                    created_at = sdr.GetInt32(sdr.GetOrdinal("created_at")),
                };
            }



            tmpTotalBet.text = $"{bussinessDayRecord.total_bet_credit}";

            tmpTotalWin.text = $"{bussinessDayRecord.total_win_credit}";

            tmpTotalProfitBet.text = $"{bussinessDayRecord.total_bet_credit - bussinessDayRecord.total_win_credit}";

            tmpTotalScoreDown.text = $"{bussinessDayRecord.total_score_down_credit}";

            tmpTotalScoreUp.text = $"{bussinessDayRecord.total_score_up_credit}";

            tmpTotalProfitlScoreUpDown.text = $"{bussinessDayRecord.total_score_up_credit - bussinessDayRecord.total_score_down_credit}";


            tmpTotalCoinOut.text = $"{bussinessDayRecord.total_coin_out_credit}";

            tmpTotalCoinIn.text = $"{bussinessDayRecord.total_coin_in_credit}";

            tmpTotalProfitCoinInOut.text = $"{bussinessDayRecord.total_coin_in_credit - bussinessDayRecord.total_coin_out_credit}";


            tmpTotalBillIn.text = $"{bussinessDayRecord.total_bill_in_as_money}";
            tmpTotalPrinterOut.text = $"{bussinessDayRecord.total_printer_out_as_money}";


            isNext = true;
        });
        yield return new WaitUntil(() => isNext == true);
        isNext = false;

    }


#endif







}
