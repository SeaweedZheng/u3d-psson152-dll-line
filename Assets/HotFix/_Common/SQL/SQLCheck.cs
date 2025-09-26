using GameMaker;
using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _consoleBB = PssOn00152.ConsoleBlackboard02;

public class SQLCheck : CorBehaviour
{

    const string COR_CHECK_RECORD = "COR_CHECK_RECORD";
    void OnEnable()
    {
        DoCor(COR_CHECK_RECORD, CheckRecord());
    }

    void OnDisable()
    {
        ClearCor(COR_CHECK_RECORD);
    }

    IEnumerator CheckRecord()
    {

        string dbName = ConsoleTableName.DB_NAME;
        string tableName = ConsoleTableName.TABLE_COIN_IN_OUT_RECORD;
        string rowName = "created_at";


        while (true)
        {
            //yield return new WaitForSecondsRealtime(3600f); //1小时
            yield return new WaitForSecondsRealtime(600f);

            DebugUtils.Log("【Check Record】check sql record");

            bool isNext = false;


            if (!SQLiteAsyncHelper.Instance.isConnect)
            {
                DebugUtils.LogError($"【Check Record】{dbName} is close");
                yield break;
            }



            tableName = ConsoleTableName.TABLE_COIN_IN_OUT_RECORD;// "coin_in_out_record";
            SQLiteAsyncHelper.Instance.ExecuteDeleteOverflowAsync(tableName, (int)_consoleBB.Instance.coinInOutRecordMax, rowName,
                (res) =>
                {
                    isNext = true;
                });
            yield return new WaitUntil(() => isNext == true);
            isNext = false;


            tableName = ConsoleTableName.TABLE_SLOT_GAME_RECORD;// "slot_game_record";
            SQLiteAsyncHelper.Instance.ExecuteDeleteOverflowAsync(tableName, (int)_consoleBB.Instance.gameRecordMax, rowName,
            (res) =>
            {
                isNext = true;
            });
            yield return new WaitUntil(() => isNext == true);
            isNext = false;


            tableName = ConsoleTableName.TABLE_LOG_ERROR_RECORD;
            SQLiteAsyncHelper.Instance.ExecuteDeleteOverflowAsync(tableName, (int)_consoleBB.Instance.errorRecordMax, rowName,
            (res) =>
            {
                isNext = true;
            });
            yield return new WaitUntil(() => isNext == true);
            isNext = false;



            tableName = ConsoleTableName.TABLE_LOG_EVENT_RECORD;
            SQLiteAsyncHelper.Instance.ExecuteDeleteOverflowAsync(tableName, (int)_consoleBB.Instance.eventRecordMax, rowName,
            (res) =>
            {
                isNext = true;
            });
            yield return new WaitUntil(() => isNext == true);
            isNext = false;



            tableName = ConsoleTableName.TABLE_BUSINESS_DAY_RECORD;
            SQLiteAsyncHelper.Instance.ExecuteDeleteOverflowAsync(tableName, (int)_consoleBB.Instance.businiessDayRecordMax, rowName,
            (res) =>
            {
                isNext = true;
            });
            yield return new WaitUntil(() => isNext == true);
            isNext = false;


        }
    }


#if false

    /// <summary>
    /// 【这个方法将废弃】
    /// </summary>
    /// <returns></returns>
    IEnumerator CheckRecordOld()
    {

        string dbName = ConsoleTableName.DB_NAME;
        string tableName = ConsoleTableName.TABLE_COIN_IN_OUT_RECORD; 
        string rowName = "created_at";


        while (true)
        {
            //yield return new WaitForSecondsRealtime(3600f); //1小时
            yield return new WaitForSecondsRealtime(600f); 

            DebugUtil.Log("【Check Record】check sql record");

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
                continue;
            }

            tableName = ConsoleTableName.TABLE_COIN_IN_OUT_RECORD;// "coin_in_out_record";
            if (SQLiteHelper01.Instance.CheckTableExists(tableName))
            {
                string sql = $"SELECT COUNT(*) FROM {tableName}";
                //DebugUtil.Log(sql);
                using (var command = new SqliteCommand(sql, connection))
                {
                    object res = command.ExecuteScalar();
                    //DebugUtil.Log($"res = {res}");
                    long rowCount = (long)res;
                    //DebugUtil.Log($"Table '{tableName}' has {rowCount} rows.");

                    long maxRecord = _consoleBB.Instance.coinInOutRecordMax;

                    if (maxRecord < rowCount)
                    {
                        DebugUtil.Log("【Check Record】delete coin_in_out_record");

                        sql = $"DELETE FROM {tableName} WHERE {rowName} NOT IN (  SELECT {rowName} FROM (  SELECT {rowName} FROM {tableName} ORDER BY {rowName} DESC LIMIT {maxRecord} ))";
                        //DebugUtil.Log($"sql = {sql}");
                        SQLiteHelper01.Instance.ExecuteNonQuery(sql);
                    }
                }
            }


            tableName = ConsoleTableName.TABLE_SLOT_GAME_RECORD;// "slot_game_record";
            if (SQLiteHelper01.Instance.CheckTableExists(tableName))
            {
                string sql = $"SELECT COUNT(*) FROM {tableName}";
                //DebugUtil.Log(sql);
                using (var command = new SqliteCommand(sql, connection))
                {
                    object res = command.ExecuteScalar();
                    //DebugUtil.Log($"res = {res}");
                    long rowCount = (long)res;
                    //DebugUtil.Log($"Table '{tableName}' has {rowCount} rows.");

                    long maxRecord = _consoleBB.Instance.gameRecordMax;

                    if (maxRecord < rowCount)
                    {
                        DebugUtil.Log("【Check Record】delete slot_game_record");

                        sql = $"DELETE FROM {tableName} WHERE {rowName} NOT IN (  SELECT {rowName} FROM (  SELECT {rowName} FROM {tableName} ORDER BY {rowName} DESC LIMIT {maxRecord} ))";

                        SQLiteHelper01.Instance.ExecuteNonQuery(sql);
                    }
                }
            }

            tableName = ConsoleTableName.TABLE_LOG_ERROR_RECORD;
            if (SQLiteHelper01.Instance.CheckTableExists(tableName))
            {
                string sql = $"SELECT COUNT(*) FROM {tableName}";
                //DebugUtil.Log(sql);
                using (var command = new SqliteCommand(sql, connection))
                {
                    object res = command.ExecuteScalar();
                    //DebugUtil.Log($"res = {res}");
                    long rowCount = (long)res;
                    //DebugUtil.Log($"Table '{tableName}' has {rowCount} rows.");

                    long maxRecord = _consoleBB.Instance.errorRecordMax;

                    if (maxRecord < rowCount)
                    {
                        DebugUtil.Log("【Check Record】delete slot_game_record");

                        sql = $"DELETE FROM {tableName} WHERE {rowName} NOT IN (  SELECT {rowName} FROM (  SELECT {rowName} FROM {tableName} ORDER BY {rowName} DESC LIMIT {maxRecord} ))";

                        SQLiteHelper01.Instance.ExecuteNonQuery(sql);
                    }
                }
            }


            tableName = ConsoleTableName.TABLE_LOG_EVENT_RECORD;
            if (SQLiteHelper01.Instance.CheckTableExists(tableName))
            {
                string sql = $"SELECT COUNT(*) FROM {tableName}";
                //DebugUtil.Log(sql);
                using (var command = new SqliteCommand(sql, connection))
                {
                    object res = command.ExecuteScalar();
                    //DebugUtil.Log($"res = {res}");
                    long rowCount = (long)res;
                    //DebugUtil.Log($"Table '{tableName}' has {rowCount} rows.");

                    long maxRecord = _consoleBB.Instance.eventRecordMax;

                    if (maxRecord < rowCount)
                    {
                        DebugUtil.Log("【Check Record】delete slot_game_record");

                        sql = $"DELETE FROM {tableName} WHERE {rowName} NOT IN (  SELECT {rowName} FROM (  SELECT {rowName} FROM {tableName} ORDER BY {rowName} DESC LIMIT {maxRecord} ))";

                        SQLiteHelper01.Instance.ExecuteNonQuery(sql);
                    }
                }
            }




            tableName = ConsoleTableName.TABLE_BUSINESS_DAY_RECORD;
            if (SQLiteHelper01.Instance.CheckTableExists(tableName))
            {
                string sql = $"SELECT COUNT(*) FROM {tableName}";
                //DebugUtil.Log(sql);
                using (var command = new SqliteCommand(sql, connection))
                {
                    object res = command.ExecuteScalar();
                    //DebugUtil.Log($"res = {res}");
                    long rowCount = (long)res;
                    //DebugUtil.Log($"Table '{tableName}' has {rowCount} rows.");

                    long maxRecord = _consoleBB.Instance.businiessDayRecordMax;

                    if (maxRecord < rowCount)
                    {
                        DebugUtil.Log("【Check Record】delete coin_in_out_record");

                        sql = $"DELETE FROM {tableName} WHERE {rowName} NOT IN (  SELECT {rowName} FROM (  SELECT {rowName} FROM {tableName} ORDER BY {rowName} DESC LIMIT {maxRecord} ))";
                        //DebugUtil.Log($"sql = {sql}");
                        SQLiteHelper01.Instance.ExecuteNonQuery(sql);
                    }
                }
            }

        }
    }


#endif


}
