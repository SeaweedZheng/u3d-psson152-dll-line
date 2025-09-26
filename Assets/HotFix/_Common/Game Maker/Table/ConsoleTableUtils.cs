#define SQLITE_ASYNC
using GameMaker;
using Newtonsoft.Json;
using SimpleJSON;
using System;
using System.Collections.Generic;
using UnityEngine;
using _consoleBB = PssOn00152.ConsoleBlackboard02;
using System.Threading.Tasks;




#region 删除或清除表
public static partial class ConsoleTableUtils
{

    /// <summary>
    /// 删除多个表
    /// </summary>
    public static void DeleteTables()
    {
        bool isVerChange = false;
        string cacherStr = SQLitePlayerPrefs03.Instance.GetString(ConsoleTableName.TABLE_VER_CACHER, "{}");
        if (string.IsNullOrEmpty(cacherStr))
            cacherStr = "{}";

        DebugUtils.Log($"old table ver： {cacherStr}");
        JSONNode tableVerCacher = JSONNode.Parse(cacherStr);

        DeleteTable(ConsoleTableName.TABLE_COIN_IN_OUT_RECORD, ref tableVerCacher, ref isVerChange);
        DeleteTable(ConsoleTableName.TABLE_SYS_SETTING, ref tableVerCacher, ref isVerChange);

        DeleteTable(ConsoleTableName.TABLE_SLOT_GAME_RECORD, ref tableVerCacher, ref isVerChange);

        DeleteTable(ConsoleTableName.TABLE_LOG_ERROR_RECORD, ref tableVerCacher, ref isVerChange);
        DeleteTable(ConsoleTableName.TABLE_LOG_EVENT_RECORD, ref tableVerCacher, ref isVerChange);

        DeleteTable(ConsoleTableName.TABLE_BET, ref tableVerCacher, ref isVerChange);
        DeleteTable(ConsoleTableName.TABLE_BUSINESS_DAY_RECORD, ref tableVerCacher, ref isVerChange);

        DeleteTable(ConsoleTableName.TABLE_USERS, ref tableVerCacher, ref isVerChange);
        DeleteTable(ConsoleTableName.TABLE_JACKPOT_RECORD, ref tableVerCacher, ref isVerChange);
        DeleteTable(ConsoleTableName.TABLE_ORDER_ID, ref tableVerCacher, ref isVerChange);
        DeleteTable(ConsoleTableName.TABLE_GAME, ref tableVerCacher, ref isVerChange);
        if (isVerChange)
        {
            string tableVerStr = tableVerCacher.ToString();
            DebugUtils.Log($"new table ver： {tableVerStr}");
            SQLitePlayerPrefs03.Instance.SetString(ConsoleTableName.TABLE_VER_CACHER, tableVerStr);
        }

    }


    //ref和out是用于传递参数引用的两种关键字
    //ref 参数在方法调用前必须被初始化。
    //out 参数在方法调用前不需要被初始化，且通常是由方法内部进行初始化的。
    /// <summary>
    /// 删除表
    /// </summary>
    public static void DeleteTable(string tableName, ref JSONNode tableVerCacher, ref bool isVerChange)
    {
        if (!tableVerCacher.HasKey(tableName) ||
            (string)tableVerCacher[tableName] !=
            ConsoleTableName.tableVer[tableName]
        )
        {
            isVerChange = true;
            tableVerCacher[tableName] = ConsoleTableName.tableVer[tableName];
#if !SQLITE_ASYNC   || true
            if (SQLiteHelper.Instance.CheckTableExists(tableName))
            {
                string sql = $"DROP TABLE {tableName};";
                DebugUtils.LogWarning($"delete table：{tableName} ！   upadte to version：{ConsoleTableName.tableVer[tableName]}");
                SQLiteHelper.Instance.ExecuteNonQuery(sql);//刪除表
            }
#else
           SQLiteAsyncHelper.Instance.ExecuteDropTableAsync(tableName,null); // 可以用

            // SQLiteAsyncHelper.Instance.ExecuteDropTableAsync02(tableName, null);// 可以用
#endif
        }
    }
}

#endregion





#region 创建默认表
public static partial class ConsoleTableUtils
{

    public static void CheckOrCreatTableBet(Action<object[]> ononFinishCallback = null)
    {

#if !SQLITE_ASYNC || true

        if (!SQLiteHelper.Instance.CheckTableExists(ConsoleTableName.TABLE_BET))
        {
            string sql = SQLiteHelper.SQLCreateTable<TableBetItem>(ConsoleTableName.TABLE_BET);
            SQLiteHelper.Instance.ExecuteNonQuery(sql);

            TableBetItem[] defaultTable = TableBetItem.DefaultTable();
            foreach (TableBetItem item in defaultTable)
            {
                sql = SQLiteHelper.SQLInsertTableData<TableBetItem>(ConsoleTableName.TABLE_BET, item);
                SQLiteHelper.Instance.ExecuteNonQuery(sql);
            }

        }
        ononFinishCallback?.Invoke(new object[] { 0 });
#else
        SQLiteAsyncHelper.Instance.CheckOrCreatTableAsync<TableBetItem>(ConsoleTableName.TABLE_BET, TableBetItem.DefaultTable(), null);
#endif

    }






    public static void CheckOrCreatTableCoinInOutRecord() => CheckOrCreatTable<TableCoinInOutRecordItem>(ConsoleTableName.TABLE_COIN_IN_OUT_RECORD);









    public static void CheckOrCreatTableUsers()
    {


#if !SQLITE_ASYNC || true

        if (!SQLiteHelper.Instance.CheckTableExists(ConsoleTableName.TABLE_USERS))
        {
            string sql = SQLiteHelper.SQLCreateTable<TableUsersItem>(ConsoleTableName.TABLE_USERS);
            SQLiteHelper.Instance.ExecuteNonQuery(sql);


            TableUsersItem[] defaultTable = TableUsersItem.DefaultTable();
            foreach (TableUsersItem item in defaultTable)
            {
                sql = SQLiteHelper.SQLInsertTableData<TableUsersItem>(ConsoleTableName.TABLE_USERS, item);
                SQLiteHelper.Instance.ExecuteNonQuery(sql);
            }
        }

#else
        SQLiteAsyncHelper.Instance.CheckOrCreatTableAsync<TableUsersItem>(ConsoleTableName.TABLE_USERS, TableUsersItem.DefaultTable(), null);
#endif

    }



    public static void CheckOrCreatTableSysSetting()
    {

#if !SQLITE_ASYNC || true

        if (!SQLiteHelper.Instance.CheckTableExists(ConsoleTableName.TABLE_SYS_SETTING))
        {
            string sql = SQLiteHelper.SQLCreateTable<TableSysSettingItem>(ConsoleTableName.TABLE_SYS_SETTING);
            SQLiteHelper.Instance.ExecuteNonQuery(sql);

            TableSysSettingItem[] defaultTable = TableSysSettingItem.DefaultTable();
            foreach (TableSysSettingItem item in defaultTable)
            {
                sql = SQLiteHelper.SQLInsertTableData<TableSysSettingItem>(ConsoleTableName.TABLE_SYS_SETTING, item);
                SQLiteHelper.Instance.ExecuteNonQuery(sql);
            }
        }


#else
        SQLiteAsyncHelper.Instance.CheckOrCreatTableAsync<TableSysSettingItem>(ConsoleTableName.TABLE_SYS_SETTING, TableSysSettingItem.DefaultTable(), null);
#endif


    }



    public static void CheckOrCreatTableJackpotRecord() => CheckOrCreatTable<TableJackpotRecordItem>(ConsoleTableName.TABLE_JACKPOT_RECORD);


    public static void CheckOrCreatTableGame()
    {

#if !SQLITE_ASYNC || true

        if (!SQLiteHelper.Instance.CheckTableExists(ConsoleTableName.TABLE_GAME))
        {
            string sql = SQLiteHelper.SQLCreateTable<TableGameItem>(ConsoleTableName.TABLE_GAME);
            SQLiteHelper.Instance.ExecuteNonQuery(sql);


            TableGameItem[] defaultTable = TableGameItem.DefaultTable();
            foreach (TableGameItem item in defaultTable)
            {
                sql = SQLiteHelper.SQLInsertTableData<TableGameItem>(ConsoleTableName.TABLE_GAME, item);
                SQLiteHelper.Instance.ExecuteNonQuery(sql);
            }
        }

#else
        SQLiteAsyncHelper.Instance.CheckOrCreatTableAsync<TableGameItem>(ConsoleTableName.TABLE_GAME, TableGameItem.DefaultTable(), null);
#endif
    }



    public static void CheckOrCreatTableGameRecord() => CheckOrCreatTable<TableSlotGameRecordItem>(ConsoleTableName.TABLE_SLOT_GAME_RECORD);
    public static void CheckOrCreatTableLogEventRecord() => CheckOrCreatTable<TableLogRecordItem>(ConsoleTableName.TABLE_LOG_EVENT_RECORD);
    public static void CheckOrCreatTableLogErrorRecord() => CheckOrCreatTable<TableLogRecordItem>(ConsoleTableName.TABLE_LOG_ERROR_RECORD);

    public static void CheckOrCreatTableBussinessDayRecord() => CheckOrCreatTable<TableBussinessDayRecordItem>(ConsoleTableName.TABLE_BUSINESS_DAY_RECORD);
    public static void CheckOrCreatTable<T>(string tableName)
    {

#if !SQLITE_ASYNC || true
        if (!SQLiteHelper.Instance.CheckTableExists(tableName))
        {
            string sql = SQLiteHelper.SQLCreateTable<T>(tableName);
            SQLiteHelper.Instance.ExecuteNonQuery(sql);
        }
#else
        SQLiteAsyncHelper.Instance.CheckOrCreatTableAsync<T>(tableName, null, null);
#endif
    }
}

#endregion






#region 获取表数据


public static partial class ConsoleTableUtils
{


    public static void GetTableBet(Action<TableBetItem> finishCallback = null)
    {
#if !SQLITE_ASYNC
        string sql = $"SELECT* FROM {ConsoleTableName.TABLE_BET} WHERE game_id = {ConfigUtils.curGameId}";
        SQLiteHelper01.Instance.ConvertSqliteToJsonAfterOpenDB(ConsoleTableName.DB_NAME, sql, (res) =>
        {

            if (res.StartsWith("[") && res.EndsWith("]"))
            {
                res = res.Substring(1, res.Length - 2);
            }


            TableBetItem tBet = JsonConvert.DeserializeObject<TableBetItem>(res);
            _consoleBB.Instance.tableBet = tBet;
            //BlackboardUtils.SetValue<TableBetItem>("@console/tableBet", tBet);


            List<BetAllow> betAllowList = new List<BetAllow>();
            JSONNode node = JSONNode.Parse(tBet.bet_list);
            foreach (JSONNode nd in node)
            {
                BetAllow betAllow = JsonConvert.DeserializeObject<BetAllow>(nd.ToString());
                betAllowList.Add(betAllow);
            }
            _consoleBB.Instance.betAllowList = betAllowList;
            //BlackboardUtils.SetValue<List<BetAllow>>("@console/betAllowList", betAllowList);


            List<long> betList = new List<long>();
            foreach (BetAllow nd in betAllowList)
            {
                if (nd.allowed == 1)
                    betList.Add(nd.value);
            }
            _consoleBB.Instance.betList = betList;
            //BlackboardUtils.SetValue<List<long>>("@console/betList", betList);


            //DebugUtil.LogWarning($"@[Check] console/betList = {JSONNodeUtil.ObjectToJsonStr( BlackboardUtils.GetValue<List<long>>("@console/betList") )   } ");

            finishCallback?.Invoke(tBet);
        });

#else

        string sql = $"SELECT* FROM {ConsoleTableName.TABLE_BET} WHERE game_id = {ConfigUtils.curGameId}";
        SQLiteAsyncHelper.Instance.ConvertSqliteToJsonAsync(sql, (res) =>
        {
            if (res.StartsWith("[") && res.EndsWith("]"))
            {
                res = res.Substring(1, res.Length - 2);
            }


            TableBetItem tBet = JsonConvert.DeserializeObject<TableBetItem>(res);
            _consoleBB.Instance.tableBet = tBet;
            //BlackboardUtils.SetValue<TableBetItem>("@console/tableBet", tBet);


            List<BetAllow> betAllowList = new List<BetAllow>();
            JSONNode node = JSONNode.Parse(tBet.bet_list);
            foreach (JSONNode nd in node)
            {
                BetAllow betAllow = JsonConvert.DeserializeObject<BetAllow>(nd.ToString());
                betAllowList.Add(betAllow);
            }
            _consoleBB.Instance.betAllowList = betAllowList;
            //BlackboardUtils.SetValue<List<BetAllow>>("@console/betAllowList", betAllowList);

            List<long> betList = new List<long>();
            foreach (BetAllow nd in betAllowList)
            {
                if (nd.allowed == 1)
                    betList.Add(nd.value);
            }
            _consoleBB.Instance.betList = betList;
            //BlackboardUtils.SetValue<List<long>>("@console/betList", betList);

            //DebugUtil.LogWarning($"@[Check] console/betList = {JSONNodeUtil.ObjectToJsonStr( BlackboardUtils.GetValue<List<long>>("@console/betList") )   } ");

            finishCallback?.Invoke(tBet);
        });

#endif


    }

    /*public static void GetTableButtons(Action<List<TableButtonsItem>> finishCallback = null)
    {

        string sql = $"SELECT* FROM {ConsoleTableName.TABLE_BUTTONS}";
        SQLiteHelper02.Instance.ConvertSqliteToJsonAfterOpenDB(ConsoleTableName.DB_NAME, sql, (res) =>
        {

            JSONNode node = JSONNode.Parse(res);

            List<TableButtonsItem> lst = new List<TableButtonsItem>();
            foreach (JSONNode nd in node)
            {
                TableButtonsItem temp = JsonConvert.DeserializeObject<TableButtonsItem>(nd.ToString());
                lst.Add(temp);
            }

            _consoleBB.Instance.tableButtons = lst;
            //BlackboardUtils.SetValue<List<TableButtonsItem>>("@console/tableButtons", lst);

            finishCallback?.Invoke(lst);

        });
    }*/


    /*public static void GetTableCoinInOutRecord(Action<List<TableCoinInOutRecordItem>> finishCallback = null)
    {

        //string sql = $"SELECT* FROM {ConsoleTableName.TABLE_COIN_IN_OUT_RECORD} WHERE game_id = {ConfigUtils.curGameId}";
        string sql = $"SELECT* FROM {ConsoleTableName.TABLE_COIN_IN_OUT_RECORD}";
        SQLiteHelper02.Instance.ConvertSqliteToJsonAfterOpenDB(ConsoleTableName.DB_NAME, sql, (res) =>
        {

            JSONNode node = JSONNode.Parse(res);

            List<TableCoinInOutRecordItem> lst = new List<TableCoinInOutRecordItem>();
            foreach (JSONNode nd in node)
            {
                TableCoinInOutRecordItem temp = JsonConvert.DeserializeObject<TableCoinInOutRecordItem>(nd.ToString());
                lst.Add(temp);
            }

            _consoleBB.Instance.tableCoinInOutRecord = lst;
            //BlackboardUtils.SetValue<List<TableCoinInOutRecordItem>>("@console/tableCoinInOutRecord", lst);

            finishCallback?.Invoke(lst);
        });
    }*/



    public static void GetTableUsers(Action<List<TableUsersItem>> finishCallback = null)
    {

#if !SQLITE_ASYNC

        string sql = $"SELECT* FROM {ConsoleTableName.TABLE_USERS}";
        SQLiteHelper01.Instance.ConvertSqliteToJsonAfterOpenDB(ConsoleTableName.DB_NAME, sql, (res) =>
        {

            JSONNode node = JSONNode.Parse(res);

            List<TableUsersItem> lst = new List<TableUsersItem>();
            foreach (JSONNode nd in node)
            {
                TableUsersItem temp = JsonConvert.DeserializeObject<TableUsersItem>(nd.ToString());
                lst.Add(temp);
            }

            _consoleBB.Instance.tableUsers = lst;
            //BlackboardUtils.SetValue<List<TableUsersItem>>("@console/tableUsers", lst);

            finishCallback?.Invoke(lst);
        });

#else
        string sql = $"SELECT* FROM {ConsoleTableName.TABLE_USERS}";
        SQLiteAsyncHelper.Instance.ConvertSqliteToJsonAsync(sql, (res) =>
        {
            JSONNode node = JSONNode.Parse(res);

            List<TableUsersItem> lst = new List<TableUsersItem>();
            foreach (JSONNode nd in node)
            {
                TableUsersItem temp = JsonConvert.DeserializeObject<TableUsersItem>(nd.ToString());
                lst.Add(temp);
            }

            _consoleBB.Instance.tableUsers = lst;
            //BlackboardUtils.SetValue<List<TableUsersItem>>("@console/tableUsers", lst);

            finishCallback?.Invoke(lst);

        });
#endif

    }



    public static void GetTableSysSetting(Action<TableSysSettingItem> finishCallback = null)
    {

#if !SQLITE_ASYNC

        string sql = $"SELECT* FROM {ConsoleTableName.TABLE_SYS_SETTING}";
        SQLiteHelper01.Instance.ConvertSqliteToJsonAfterOpenDB(ConsoleTableName.DB_NAME, sql, (res) =>
        {

            if (res.StartsWith("[") && res.EndsWith("]"))
            {
                res = res.Substring(1, res.Length - 2);
            }

            TableSysSettingItem temp = JsonConvert.DeserializeObject<TableSysSettingItem>(res);
            _consoleBB.Instance.tableSysSetting = temp;
            //BlackboardUtils.SetValue<TableSysSettingItem>("@console/tableSysSetting", temp);


            finishCallback?.Invoke(temp);
        });

#else
        string sql = $"SELECT* FROM {ConsoleTableName.TABLE_SYS_SETTING}";
        SQLiteAsyncHelper.Instance.ConvertSqliteToJsonAsync(sql, (res) =>
        {

            if (res.StartsWith("[") && res.EndsWith("]"))
            {
                res = res.Substring(1, res.Length - 2);
            }

            TableSysSettingItem temp = JsonConvert.DeserializeObject<TableSysSettingItem>(res);
            _consoleBB.Instance.tableSysSetting = temp;
            //BlackboardUtils.SetValue<TableSysSettingItem>("@console/tableSysSetting", temp);


            finishCallback?.Invoke(temp);
        });
#endif
    }


    public static void GetTableGame(Action<TableGameItem> finishCallback = null)
    {


#if !SQLITE_ASYNC

        string sql = $"SELECT* FROM {ConsoleTableName.TABLE_GAME} WHERE game_id = {ConfigUtils.curGameId}";
        SQLiteHelper01.Instance.ConvertSqliteToJsonAfterOpenDB(ConsoleTableName.DB_NAME, sql, (res) =>
        {

            if (res.StartsWith("[") && res.EndsWith("]"))
            {
                res = res.Substring(1, res.Length - 2);
            }

            TableGameItem temp = JsonConvert.DeserializeObject<TableGameItem>(res);
            _consoleBB.Instance.tableGame = temp;
            //BlackboardUtils.SetValue<TableGameItem>("@console/tableGame", temp);

            finishCallback?.Invoke(temp);
        });
#else
        string sql = $"SELECT* FROM {ConsoleTableName.TABLE_GAME} WHERE game_id = {ConfigUtils.curGameId}";
        SQLiteAsyncHelper.Instance.ConvertSqliteToJsonAsync(sql, (res) =>
        {
            if (res.StartsWith("[") && res.EndsWith("]"))
            {
                res = res.Substring(1, res.Length - 2);
            }

            TableGameItem temp = JsonConvert.DeserializeObject<TableGameItem>(res);
            _consoleBB.Instance.tableGame = temp;
            //BlackboardUtils.SetValue<TableGameItem>("@console/tableGame", temp);

            finishCallback?.Invoke(temp);

        });
#endif


    }

    public static void CheckOrCreatTableOrderId() => CheckOrCreatTable<TableOrdeId>(ConsoleTableName.TABLE_ORDER_ID);

}

 #endregion
