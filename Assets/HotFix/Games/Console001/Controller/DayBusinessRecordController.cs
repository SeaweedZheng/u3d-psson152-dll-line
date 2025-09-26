#define SQLITE_ASYNC
using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
public class DayBusinessRecordController : CorBehaviour
{

    public bool isShowGameRecord = true;

    public bool isShowConiInOutRecord = true;

    public TMP_Dropdown drpDateCoinInOut;

    public TextMeshProUGUI tmpTotalBet, tmpTotalWin, tmpTotalProfitBet,

        tmpTotalCoinIn, tmpTotalCoinOut, tmpTotalProfitCoinInOut,

        tmpTotalScoreUp, tmpTotalScoreDown, tmpTotalProfitlScoreUpDown;

    //MessageDelegates onPropertyChangedEventDelegates;


    public UnityEvent<string> OnClickData;

    void Awake()
    {
        drpDateCoinInOut.onValueChanged.AddListener(OnDropdownChangedDate);
    }

    private void OnEnable()
    {
         InitParam();       
    }

    void InitParam()
    {

        ClearAllUI();

        /*List<string> dateStrings = new List<string>
        {
            "2023-10-01",
            "2022-05-15",
            "2021-12-25",
            "2023-11-01",
            "2020-01-01"
        };

        //dateStrings = dateStrings.OrderByDescending(dateString => DateTime.ParseExact(dateString, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture)).ToList();
        
        BubbleSort(ref dateStrings);
        foreach (string item in dateStrings)
        {
            DebugUtil.Log($"@ {item}");
        }*/

        int next = 2;
        Action cb = () =>
        {
            if (--next == 0)
            {
                allDate.Clear();
                allDate.AddRange(dropdownDateLstCoinInOut);
                allDate.AddRange(dropdownDateLstGame);

                // 使用 LINQ 去重
                allDate = allDate.Distinct().ToList();


                // 排序，最新排最前
                //allDate = allDate.OrderByDescending(dateString => DateTime.ParseExact(dateString, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture)).ToList();
                BubbleSort(ref allDate);

                drpDateCoinInOut.options.Clear();
                if (allDate.Count > 0)
                {
                    drpDateCoinInOut.AddOptions(allDate);
                    drpDateCoinInOut.RefreshShownValue();// 最后，刷新Dropdown以应用更改
                    drpDateCoinInOut.value = 0;
                    OnDropdownChangedDate(0);
                }
            }
        };

        InitCoinInOutRecordInfo(cb);
        InitGameRecordInfo(cb);
    }




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
    }
     void BubbleSort(ref List<string> arr)
    {
        int n = arr.Count;
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                // 如果前一个元素小于后一个元素，则交换它们
                DateTime dj = DateTime.ParseExact(arr[j], "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                DateTime dj1 = DateTime.ParseExact(arr[j+1], "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                //if (arr[j] < arr[j + 1])
                if (dj < dj1)
                {
                    // 交换 arr[j] 和 arr[j + 1]
                    string temp = arr[j];
                    arr[j] = arr[j + 1];
                    arr[j + 1] = temp;
                }
            }
        }
    }


    void OnDropdownChangedDate(int index)
    {
        ClearAllUI();
        string item = allDate[index];
        if (dropdownDateLstCoinInOut.IndexOf(item)>=0)
        {
            OnDropdownChangedDateCoinInOut(dropdownDateLstCoinInOut.IndexOf(item));
        }

        if (dropdownDateLstGame.IndexOf(item) >=0)
        {
            OnDropdownChangedDateGame(dropdownDateLstGame.IndexOf(item));
        }

        OnClickData?.Invoke(item);
    }









    const string FORMAT_DATE_DAY = "yyyy-MM-dd";

    List<string> dropdownDateLstCoinInOut;

    #region 投退币数据





#if !SQLITE_ASYNC

    void InitCoinInOutRecordInfo(Action callback)
    {
        dropdownDateLstCoinInOut = new List<string>();

        if (!isShowConiInOutRecord)
        {
            callback?.Invoke();
            return;
        }
            
        //string sql = $"select distinct date(created_at) from {tableName}";
        string sql = $"SELECT created_at FROM {ConsoleTableName.TABLE_COIN_IN_OUT_RECORD}";
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

                if (!dropdownDateLstCoinInOut.Contains(time))
                {
                    //dropdownDateLst.Add(time);
                    DebugUtil.Log($"时间搓：{timestamp} 时间 ：{time}");
                    dropdownDateLstCoinInOut.Insert(0, time); //最新排在最前
                }
            }
            callback?.Invoke();

        });
    }
#else
    void InitCoinInOutRecordInfo(Action callback)
    {
        dropdownDateLstCoinInOut = new List<string>();

        if (!isShowConiInOutRecord)
        {
            callback?.Invoke();
            return;
        }

        //string sql = $"select distinct date(created_at) from {tableName}";
        string sql = $"SELECT created_at FROM {ConsoleTableName.TABLE_COIN_IN_OUT_RECORD}";
        DebugUtils.Log(sql);
        List<long> date = new List<long>();
        SQLiteAsyncHelper.Instance.ExecuteQueryAsync(sql, null,(SqliteDataReader sdr) =>
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

                if (!dropdownDateLstCoinInOut.Contains(time))
                {
                    //dropdownDateLst.Add(time);
                    DebugUtils.Log($"时间搓：{timestamp} 时间 ：{time}");
                    dropdownDateLstCoinInOut.Insert(0, time); //最新排在最前
                }
            }
            callback?.Invoke();

        });
    }
#endif










    void OnDropdownChangedDateCoinInOut(int index)
    {
        DoCor(COR_IN_OUT_RECORD_YYYYMMDD, CheckInOutRecord(dropdownDateLstCoinInOut[index]));
    }

    const string COR_IN_OUT_RECORD_YYYYMMDD = "COR_IN_OUT_RECORD_YYYYMMDD";



#if !SQLITE_ASYNC

    IEnumerator CheckInOutRecord(string yyyyMMdd)
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
        tmpTotalScoreUp.text = $"{totalScoreUp}";


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
        tmpTotalScoreDown.text = $"{totalScoreDown}";

        tmpTotalProfitlScoreUpDown.text = $"{totalScoreUp - totalScoreDown}";

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
        tmpTotalCoinIn.text = $"{totalCoinIn}";

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
        tmpTotalCoinOut.text = $"{totalCoinOut}";

        tmpTotalProfitCoinInOut.text = $"{totalCoinIn - totalCoinOut}";
    }


#else
    IEnumerator CheckInOutRecord(string yyyyMMdd)
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
        SQLiteAsyncHelper.Instance.ExecuteQueryAsync(sql, null,(dataReader) =>
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
            tmpTotalScoreDown.text = $"{totalScoreDown}";

            tmpTotalProfitlScoreUpDown.text = $"{totalScoreUp - totalScoreDown}";
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
            tmpTotalCoinIn.text = $"{totalCoinIn}";
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
            tmpTotalCoinOut.text = $"{totalCoinOut}";

            tmpTotalProfitCoinInOut.text = $"{totalCoinIn - totalCoinOut}";
            isNext = true;
        });
        yield return new WaitUntil(() => isNext == true);
        isNext = false;

    }


#endif






    #endregion



    List<string> allDate = new List<string>();

    #region 游戏数据


    List<string> dropdownDateLstGame;
#if !SQLITE_ASYNC

    void InitGameRecordInfo(Action callback)
    {

        dropdownDateLstGame = new List<string>();

        if (!isShowGameRecord)
        {
            callback?.Invoke();
            return;
        }


        //string sql = $"select distinct date(created_at) from {tableName}";
        string sql = $"SELECT created_at FROM {ConsoleTableName.TABLE_SLOT_GAME_RECORD}";

        DebugUtil.Log(sql);
        List<long> date = new List<long>();

        SQLiteHelper01.Instance.ExecuteQueryAfterOpenDB(sql, (SqliteDataReader sdr) =>
        {

            try
            {
                while (sdr.Read())
                {
                    long d = sdr.GetInt64(0);
                    date.Add(d);
                }
            }
            catch{}

            foreach (long timestamp in date)
            {
                //DebugUtil.Log($"时间搓：{timestamp}");
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(timestamp);
                //DateTime utcDateTime = dateTimeOffset.UtcDateTime;
                //string time = utcDateTime.ToString(FORMAT_DATE_DAY);

                DateTime localDateTime = dateTimeOffset.LocalDateTime;
                string time = localDateTime.ToString(FORMAT_DATE_DAY);

                if (!dropdownDateLstGame.Contains(time))
                {
                    //dropdownDateLstGame.Add(time);
                    DebugUtil.Log($"时间搓：{timestamp} 时间 ：{time}");
                    dropdownDateLstGame.Insert(0, time); //最新排在最前
                }
            }

            callback?.Invoke();

        });
    }
#else
    void InitGameRecordInfo(Action callback)
    {

        dropdownDateLstGame = new List<string>();

        if (!isShowGameRecord)
        {
            callback?.Invoke();
            return;
        }


        //string sql = $"select distinct date(created_at) from {tableName}";
        string sql = $"SELECT created_at FROM {ConsoleTableName.TABLE_SLOT_GAME_RECORD}";

        DebugUtils.Log(sql);
        List<long> date = new List<long>();

        SQLiteAsyncHelper.Instance.ExecuteQueryAsync(sql, null, (SqliteDataReader sdr) =>
        {

            try
            {
                while (sdr.Read())
                {
                    long d = sdr.GetInt64(0);
                    date.Add(d);
                }
            }
            catch { }

            foreach (long timestamp in date)
            {
                //DebugUtil.Log($"时间搓：{timestamp}");
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(timestamp);
                //DateTime utcDateTime = dateTimeOffset.UtcDateTime;
                //string time = utcDateTime.ToString(FORMAT_DATE_DAY);

                DateTime localDateTime = dateTimeOffset.LocalDateTime;
                string time = localDateTime.ToString(FORMAT_DATE_DAY);

                if (!dropdownDateLstGame.Contains(time))
                {
                    //dropdownDateLstGame.Add(time);
                    DebugUtils.Log($"时间搓：{timestamp} 时间 ：{time}");
                    dropdownDateLstGame.Insert(0, time); //最新排在最前
                }
            }

            callback?.Invoke();

        });
    }
#endif






    const string COR_GAME_RECORD_YYYYMMDD = "COR_GAME_RECORD_YYYYMMDD";
    void OnDropdownChangedDateGame(int index)
    {
        DoCor(COR_GAME_RECORD_YYYYMMDD, CheckGameRecord(dropdownDateLstGame[index]));
    }


#if !SQLITE_ASYNC

    IEnumerator CheckGameRecord(string yyyyMMdd)
    {

        string dbName = ConsoleTableName.DB_NAME; 
        string tableName = ConsoleTableName.TABLE_SLOT_GAME_RECORD;

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


        string sql = $"SELECT SUM(total_bet) AS total_total_bet FROM {tableName} WHERE game_id = {ConfigUtils.curGameId}  AND DATE(DATETIME(created_at / 1000, 'unixepoch', 'localtime')) = '{yyyyMMdd}';";
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


        sql = $"SELECT SUM(total_win_credit) AS total_total_win_credit FROM {tableName} WHERE game_id = {ConfigUtils.curGameId} AND DATE(DATETIME(created_at / 1000, 'unixepoch', 'localtime')) = '{yyyyMMdd}';";
        command = connection.CreateCommand();
        command.CommandText = sql;
        dataReader = command.ExecuteReader();
        long totalWinCredit = 0;
        while (dataReader.Read())
        {
            try
            {
                totalWinCredit = dataReader.GetInt64(0);
            }
            catch
            {
                totalWinCredit = 0;
            }
        }
        tmpTotalWin.text = $"{totalWinCredit}";

        tmpTotalProfitBet.text = $"{totalBet - totalWinCredit}";

    }
#else

    IEnumerator CheckGameRecord(string yyyyMMdd)
    {

        string dbName = ConsoleTableName.DB_NAME;
        string tableName = ConsoleTableName.TABLE_SLOT_GAME_RECORD;

        bool isNext = false;



        if (!SQLiteAsyncHelper.Instance.isConnect)
        {
            DebugUtils.LogError($"【Check Record】{dbName} is close");
            yield break;
        }


        long totalBet = 0;
        string sql = $"SELECT SUM(total_bet) AS total_total_bet FROM {tableName} WHERE game_id = {ConfigUtils.curGameId}  AND DATE(DATETIME(created_at / 1000, 'unixepoch', 'localtime')) = '{yyyyMMdd}';";
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


        long totalWinCredit = 0;
        sql = $"SELECT SUM(total_win_credit) AS total_total_win_credit FROM {tableName} WHERE game_id = {ConfigUtils.curGameId} AND DATE(DATETIME(created_at / 1000, 'unixepoch', 'localtime')) = '{yyyyMMdd}';";
        SQLiteAsyncHelper.Instance.ExecuteQueryAsync(sql, null, (dataReader) =>
        {
            while (dataReader.Read())
            {
                try
                {
                    totalWinCredit = dataReader.GetInt64(0);
                }
                catch
                {
                    totalWinCredit = 0;
                }
            }
            tmpTotalWin.text = $"{totalWinCredit}";

            tmpTotalProfitBet.text = $"{totalBet - totalWinCredit}";

            isNext = true;
        });
        yield return new WaitUntil(() => isNext == true);
        isNext = false;



    }



#endif




    #endregion
}
