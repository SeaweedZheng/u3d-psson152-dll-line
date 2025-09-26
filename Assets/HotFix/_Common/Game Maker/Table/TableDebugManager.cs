using GameMaker;
using Mono.Data.Sqlite;
using Newtonsoft.Json;
using SimpleJSON;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;


public class TableDebugManager : MonoSingleton<TableDebugManager>
{

    /*
"table_last_data_bussiness_day_record":{
        "event_type": "ON_TOOL_EVENT",
"event_name": "ShowTableLastData",
"event_data": "{\"msg\":\"null\",\"table_name\":\"bussiness_day_record\",\"last_count\":7,}",		
"nick_name": "显示'每日营收数据表'前7条数据",
"des": ""
},
 */

    void Start()
    {
        EventCenter.Instance.AddEventListener<EventData>(GlobalEvent.ON_TOOL_EVENT, OnToolEvent);
    }


    protected  override void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener<EventData>(GlobalEvent.ON_TOOL_EVENT, OnToolEvent);

        base.OnDestroy();
    }


    void OnToolEvent(EventData res)
    {
        if (res.name == GlobalEvent.ShowTableLastData)
        {

            Debug.Log($"@#@#  string data = {(string)res.value}");

            JSONNode nod = JSONNode.Parse((string)res.value);
            GetTableLastData((string)nod["table_name"], (int)nod["last_count"]);
        }
    }

    [Button]
    public void GetTableLastData(string tableName = "bussiness_day_record", int count = 10)
    {

        SQLiteHelper.Instance.OpenDB(ConsoleTableName.DB_NAME, (connect) =>
        {
            // SQL查询语句，选择最近的10条记录
            string selectQuery = $" SELECT * FROM {tableName} ORDER BY ID DESC LIMIT {count}";


            using (SqliteCommand selectCommand = new SqliteCommand(selectQuery, connect))
            {
                using (IDataReader reader = selectCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var record = new Dictionary<string, object>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            record[reader.GetName(i)] = reader.GetValue(i);
                        }

                        string createdAtUTC = "";
                        string createdAtLOC = "";
                        if (record.ContainsKey("created_at"))
                        {
                            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds((long)record["created_at"]);
                            DateTime utcDateTime = dateTimeOffset.UtcDateTime;
                            DateTime localDateTime = dateTimeOffset.LocalDateTime;
                            //createdAtLOC = localDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                            createdAtLOC = localDateTime.ToString("yyyyMMddHHmmss");
                            createdAtUTC = utcDateTime.ToString("yyyyMMddHHmmss");
                        }

                        string json = JsonConvert.SerializeObject(record);
                        Debug.Log($" utc-{createdAtUTC} loc-{createdAtLOC} : {json}");
                    }
                }
            }

        });
    }




    /// <summary>
    /// 这个先不用，AOT缺少程序引用 (IDbCommand)
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="count"></param>
    public void GetTableLastData001(string tableName = "bussiness_day_record", int count = 10)
    {

        SQLiteHelper.Instance.OpenDB(ConsoleTableName.DB_NAME, (connect) =>
        {
            // SQL查询语句，选择最近的10条记录
            string selectQuery = $" SELECT * FROM {tableName} ORDER BY ID DESC LIMIT {count}";


            using (IDbCommand dbCommand = connect.CreateCommand())
            {
                dbCommand.CommandText = selectQuery;
                using (IDataReader reader = dbCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var record = new Dictionary<string, object>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);
                            object value = reader.GetValue(i);
                            record[columnName] = value;
                        }

                        string createdAtUTC = "";
                        string createdAtLOC = "";
                        if (record.ContainsKey("created_at"))
                        {
                            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds((long)record["created_at"]);
                            DateTime utcDateTime = dateTimeOffset.UtcDateTime;
                            DateTime localDateTime = dateTimeOffset.LocalDateTime;
                            //createdAtLOC = localDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                            createdAtLOC = localDateTime.ToString("yyyyMMddHHmmss");
                            createdAtUTC = utcDateTime.ToString("yyyyMMddHHmmss");
                        }

                        string json = JsonConvert.SerializeObject(record);
                        Debug.Log($" utc-{createdAtUTC} loc-{createdAtLOC} : {json}");
                    }
                }
            }
        });
    }




}
