#define SQLITE_ASYNC
using System;
using System.Text;
using UnityEngine;



/// <summary>
/// 日志系统
/// </summary>
/// <remark>
/// * 脚本用来筛选存储日志到本地
/// * 接入数据上报接口bugly
/// * 
/// </remark>
public class LogManager : MonoSingleton<LogManager>
{
    void Start()
    {
        Application.logMessageReceived += OnLogCallBack;
    }

    protected override void OnDestroy()
    {
        Application.logMessageReceived -= OnLogCallBack;
        base.OnDestroy();
    }

    const string MYSELF_LOG = "【logMgr】";
    const string SAVE_LOG = "【Log】";
    /// <summary>
    /// 打印日志回调
    /// </summary>
    /// <param name="condition">日志文本</param>
    /// <param name="stackTrace">调用堆栈</param>
    /// <param name="type">日志类型</param>
    private void OnLogCallBack(string condition, string stackTrace, LogType type)
    {
        if (isApplicationQuit ||  condition.StartsWith(MYSELF_LOG))
            return;

        switch (type)
        {
            case LogType.Exception:
            case LogType.Error:
                {
                    try
                    {



#if !SQLITE_ASYNC

                        string sql = SQLiteHelper01.SQLInsertTableData<TableLogRecordItem>(
                            ConsoleTableName.TABLE_LOG_ERROR_RECORD,
                            new TableLogRecordItem()
                            {
                                log_type = Enum.GetName(typeof(LogType), type),
                                log_content = DoStr2base64str(condition ?? "--"),
                                log_stacktrace = DoStr2base64str(stackTrace ?? "--"),
                                log_tag = "",
                                created_at = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                            });
                        SQLiteHelper01.Instance.ExecuteNonQueryAfterOpenDB(ConsoleTableName.DB_NAME, sql);
#else
                        string sql = SQLiteAsyncHelper.SQLInsertTableData<TableLogRecordItem>(
                            ConsoleTableName.TABLE_LOG_ERROR_RECORD,
                            new TableLogRecordItem()
                            {
                                log_type = Enum.GetName(typeof(LogType), type),
                                log_content = DoStr2base64str(condition ?? "--"),
                                log_stacktrace = DoStr2base64str(stackTrace ?? "--"),
                                log_tag = "",
                                created_at = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                            });
                        SQLiteAsyncHelper.Instance.ExecuteNonQueryAsync(sql);

#endif


                    }
                    catch (Exception e) {
                        Debug.LogWarning($"{MYSELF_LOG}condition = {condition}");
                        Debug.LogWarning($"{MYSELF_LOG}stackTrace = {stackTrace}");
                        Debug.LogError($"{MYSELF_LOG}{e}");              
                    }
                }
                break;
            case LogType.Warning:
            case LogType.Log:
                {
                    try
                    {
                        if (condition != null && condition.StartsWith(SAVE_LOG))
                        {

#if !SQLITE_ASYNC

                            string sql = SQLiteHelper01.SQLInsertTableData<TableLogRecordItem>(
                                ConsoleTableName.TABLE_LOG_EVENT_RECORD,
                                new TableLogRecordItem()
                                {
                                    log_type = Enum.GetName(typeof(LogType), type),
                                    log_content = DoStr2base64str(condition.Substring(SAVE_LOG.Length)),
                                    log_stacktrace = DoStr2base64str(stackTrace ?? "--"),
                                    log_tag = "",
                                    created_at = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                                });
                            SQLiteHelper01.Instance.ExecuteNonQueryAfterOpenDB(ConsoleTableName.DB_NAME, sql);
#else
                            string sql = SQLiteAsyncHelper.SQLInsertTableData<TableLogRecordItem>(
                                ConsoleTableName.TABLE_LOG_EVENT_RECORD,
                                new TableLogRecordItem()
                                {
                                    log_type = Enum.GetName(typeof(LogType), type),
                                    log_content = DoStr2base64str(condition.Substring(SAVE_LOG.Length)),
                                    log_stacktrace = DoStr2base64str(stackTrace ?? "--"),
                                    log_tag = "",
                                    created_at = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                                });
                            SQLiteAsyncHelper.Instance.ExecuteNonQueryAsync(sql);
#endif                  


                        }
                    }
                    catch (Exception e){ Debug.LogError($"{MYSELF_LOG}{e}"); }
                }
                break;
        }

    }

    bool isApplicationQuit = false;
    void OnApplicationQuit()
    {
        isApplicationQuit = true;
    }

    public string DoStr2base64str(string str) => Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
    public string Dobase64str2Str(string base64Encoded) => Encoding.UTF8.GetString(Convert.FromBase64String(base64Encoded));
}
