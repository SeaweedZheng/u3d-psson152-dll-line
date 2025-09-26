#define SQLITE_ASYNC
using GameMaker;
using Mono.Data.Sqlite;
using Sirenix.OdinInspector;
using SlotDllAlgorithmG152;
using System.Collections;
using UnityEngine;

using _consoleBB = PssOn00152.ConsoleBlackboard02;

/// <summary>
/// 机台数据恢复
/// </summary>
public class MachineRestoreManager : MonoSingleton<MachineRestoreManager>
{


    private CorController _corCtrl;
    private CorController corCtrl
    {
        get
        {
            if (_corCtrl == null)
                _corCtrl = new CorController(this);
            return _corCtrl;
        }
    }

    public void ClearCor(string name) => corCtrl.ClearCor(name);

    public void ClearAllCor() => corCtrl.ClearAllCor();

    public void DoCor(string name, IEnumerator routine) => corCtrl.DoCor(name, routine);


    const string COR_CLEAR_SQL_TABLE = "COR_CLEAR_SQL_TABLE";


    public IEnumerator Start()
    {  
        yield return new WaitForSeconds(3f);

        if (_consoleBB.Instance.isFirstInstall)
            OnFirstInstall();
    }

    /*
    /// <summary> 软件第一次运行或安装 </summary>
    const string FIRST_INSTALL_EDITOR = "FIRST_INSTALL_EDITOR";
    const string FIRST_INSTALL = "FIRST_INSTALL";
    bool isFirstInstall
    {
        get
        {
            bool _isFirst = false;
            string key = Application.isEditor ? FIRST_INSTALL_EDITOR : FIRST_INSTALL;
            int dat = PlayerPrefs.GetInt(key, 1);
            _isFirst = dat == 1;
            if (_isFirst)
            {
                PlayerPrefs.SetInt(key, 0);
                PlayerPrefs.Save();
            }
            return _isFirst;
        }
    }*/




    public void OnFirstInstall()
    {
        DebugUtils.Log("首次安装，复位所有参数");
        RestoreToFactorySettings();
    }

    [Button]
    /// <summary>
    /// 清除“游戏记录”，“投退币记录”数据。
    /// </summary>
    public void ClearRecordWhenCoding()
    {
        // 本地游戏彩金清除【旧版本，即将废弃】
        //GameJackpotCreator.Instance.ResetGameJackpot();


        // 新版本彩金
        SlotDllAlgorithmG152Manager.Instance.Clear();


        // ClearMachineCounter();
        DoCor(COR_CLEAR_SQL_TABLE, ClearSQLTable());
    }


    /// <summary>
    /// 清除码表（码表目前无法清除）
    /// </summary>
    void ClearMachineCounter()
    {
        MachineDataManager.Instance.RequestCounter(0, 0, 1, (res) =>
        {
            int resault = (int)res;

            DebugUtils.Log($"清除投币码表 : {resault}");

            // 这里必须嵌套，MachineDataManager的方法无法重复调用，响应回调会被覆盖
            MachineDataManager.Instance.RequestCounter(1, 0, 1, (res) =>
            {
                int resault = (int)res;
                DebugUtils.Log($"清除退币码表 : {resault}");
            });
        });

        /*yield return new WaitForSeconds(0.2f);
        MachineDataManager.Instance.RequestCounter(0, 0, 1, (res) =>
        {
            int resault = (int)res;
        });*/
    }



#if !SQLITE_ASYNC
    /// <summary>
    /// 【这个方法将废弃】
    /// </summary>
    /// <returns></returns>
    IEnumerator ClearSQLTable()
    {

        string dbName = ConsoleTableName.DB_NAME; 
        string tableName = ConsoleTableName.TABLE_COIN_IN_OUT_RECORD; 
        string rowName = "created_at";

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

        // 清除投退币
        tableName = ConsoleTableName.TABLE_COIN_IN_OUT_RECORD;
        if (SQLiteHelper01.Instance.CheckTableExists(tableName))
        {
            string sql = $"DELETE FROM {tableName};";
            SQLiteHelper01.Instance.ExecuteNonQuery(sql);
        }

        // 清除游戏记录
        tableName = ConsoleTableName.TABLE_SLOT_GAME_RECORD;
        if (SQLiteHelper01.Instance.CheckTableExists(tableName))
        {
            string sql = $"DELETE FROM {tableName};";
            SQLiteHelper01.Instance.ExecuteNonQuery(sql);
        }


        // 清除每日营收记录
        tableName = ConsoleTableName.TABLE_BUSINESS_DAY_RECORD;
        if (SQLiteHelper01.Instance.CheckTableExists(tableName))
        {
            string sql = $"DELETE FROM {tableName};";
            SQLiteHelper01.Instance.ExecuteNonQuery(sql);
        }


        // 清除彩金记录
        tableName = ConsoleTableName.TABLE_JACKPOT_RECORD;
        if (SQLiteHelper01.Instance.CheckTableExists(tableName))
        {
            string sql = $"DELETE FROM {tableName};";
            SQLiteHelper01.Instance.ExecuteNonQuery(sql);
        }

        // 清除报错日志
        tableName = ConsoleTableName.TABLE_LOG_ERROR_RECORD;
        if (SQLiteHelper01.Instance.CheckTableExists(tableName))
        {
            string sql = $"DELETE FROM {tableName};";
            SQLiteHelper01.Instance.ExecuteNonQuery(sql);
        }

        // 清掉事件日志
        tableName = ConsoleTableName.TABLE_LOG_EVENT_RECORD;
        if (SQLiteHelper01.Instance.CheckTableExists(tableName))
        {
            string sql = $"DELETE FROM {tableName};";
            SQLiteHelper01.Instance.ExecuteNonQuery(sql);
        }

        if (ApplicationSettings.Instance.isMock)
        {
            // 清掉玩家金额，赢分，历史总投退数据
            _consoleBB.Instance.sboxPlayerInfo = MachineDataManager.Instance.RequestClearPlayerAccountWhenMock();
            MainBlackboardController.Instance.SyncMyTempCreditToReal(true);

            string TEST_JP_GRAND = "TEST_JP_GRAND";
            string TEST_JP_MAJOR = "TEST_JP_MAJOR";
            string TEST_JP_MINOR = "TEST_JP_MINOR";
            string TEST_JP_MINI = "TEST_JP_MINI";
            // 清掉游戏jackpot
            PlayerPrefs.SetInt(TEST_JP_GRAND, 0);
            PlayerPrefs.SetInt(TEST_JP_MAJOR, 0);
            PlayerPrefs.SetInt(TEST_JP_MINOR, 0);
            PlayerPrefs.SetInt(TEST_JP_MINI, 0);
        }

        // 删除编号
        _mainBB.Instance.reportId = 0;
        _mainBB.Instance.gameNumber = 0;


        // 删除订单缓存
        EventCenter.Instance.EventTrigger<EventData>(GlobalEvent.ON_COIN_IN_OUT_EVENT,  new EventData(GlobalEvent.ClearAllOrderCache));


        Debug.Log("清掉所有本地游戏记录数据");
    }

#else

    IEnumerator ClearSQLTable()
    {

        string dbName = ConsoleTableName.DB_NAME;
        string tableName = ConsoleTableName.TABLE_COIN_IN_OUT_RECORD;
        string rowName = "created_at";

        bool isNext = false;



        if (!SQLiteAsyncHelper.Instance.isConnect)
        {
            DebugUtils.LogError($"【Check Record】{dbName} is close");
            yield break;
        }

        // 清除投退币
        tableName = ConsoleTableName.TABLE_COIN_IN_OUT_RECORD;
        SQLiteAsyncHelper.Instance.ExecuteDeleteAsync(tableName, (res) =>
        {
            isNext = true;
        });
        yield return new WaitUntil(() => isNext == true);
        isNext = false;


        // 清除游戏记录
        tableName = ConsoleTableName.TABLE_SLOT_GAME_RECORD;
        SQLiteAsyncHelper.Instance.ExecuteDeleteAsync(tableName, (res) =>
        {
            isNext = true;
        });
        yield return new WaitUntil(() => isNext == true);
        isNext = false;


        // 清除每日营收记录
        tableName = ConsoleTableName.TABLE_BUSINESS_DAY_RECORD;
        SQLiteAsyncHelper.Instance.ExecuteDeleteAsync(tableName, (res) =>
        {
            isNext = true;
        });
        yield return new WaitUntil(() => isNext == true);
        isNext = false;


        // 清除总营收记录
        tableName = ConsoleTableName.TABLE_BUSINESS_TOTAL_RECORD;
        SQLiteAsyncHelper.Instance.ExecuteDeleteAsync(tableName, (res) =>
        {
            isNext = true;
        });
        yield return new WaitUntil(() => isNext == true);
        isNext = false;

        //TableBusniessTotalRecordAsyncManager.Instance.OnEventClearAllOrderCache();
        TableBusniessTotalRecordAsyncManager.Instance.GetTotalBusniess();

        // 清除彩金记录
        tableName = ConsoleTableName.TABLE_JACKPOT_RECORD;
        SQLiteAsyncHelper.Instance.ExecuteDeleteAsync(tableName, (res) =>
        {
            isNext = true;
        });
        yield return new WaitUntil(() => isNext == true);
        isNext = false;


        // 清除报错日志
        tableName = ConsoleTableName.TABLE_LOG_ERROR_RECORD;
        SQLiteAsyncHelper.Instance.ExecuteDeleteAsync(tableName, (res) =>
        {
            isNext = true;
        });
        yield return new WaitUntil(() => isNext == true);
        isNext = false;


        // 清掉事件日志
        tableName = ConsoleTableName.TABLE_LOG_EVENT_RECORD;
        SQLiteAsyncHelper.Instance.ExecuteDeleteAsync(tableName, (res) =>
        {
            isNext = true;
        });
        yield return new WaitUntil(() => isNext == true);
        isNext = false;


        if (ApplicationSettings.Instance.isMock)
        {
            // 清掉玩家金额，赢分，历史总投退数据
            _consoleBB.Instance.sboxPlayerInfo = MachineDataManager.Instance.RequestClearPlayerAccountWhenMock();
            MainBlackboardController.Instance.SyncMyTempCreditToReal(true);

            string TEST_JP_GRAND = "TEST_JP_GRAND";
            string TEST_JP_MAJOR = "TEST_JP_MAJOR";
            string TEST_JP_MINOR = "TEST_JP_MINOR";
            string TEST_JP_MINI = "TEST_JP_MINI";
            // 清掉游戏jackpot
            PlayerPrefs.SetInt(TEST_JP_GRAND, 0);
            PlayerPrefs.SetInt(TEST_JP_MAJOR, 0);
            PlayerPrefs.SetInt(TEST_JP_MINOR, 0);
            PlayerPrefs.SetInt(TEST_JP_MINI, 0);
        }

        // 删除编号
        MainBlackboardController.Instance.ClearGameNumber();
        MainBlackboardController.Instance.ClearReportId();


        // 删除订单缓存
        EventCenter.Instance.EventTrigger<EventData>(GlobalEvent.ON_COIN_IN_OUT_EVENT, new EventData(GlobalEvent.ClearAllOrderCache));


        Debug.Log("清掉所有本地游戏记录数据");
    }

#endif











    /// <summary>
    /// 恢复出厂设置
    /// </summary>
    public void RestoreToFactorySettings()
    {
        // 清楚记录
        DoCor(COR_CLEAR_SQL_TABLE, ClearSQLTable());

        // 恢复投退币参数设置
        _consoleBB.Instance.billInScale = DefaultSettingsUtils.Config().defBillInScale;
        _consoleBB.Instance.printOutScale = DefaultSettingsUtils.Config().defPrintOutScale;
        _consoleBB.Instance.coinInScale = DefaultSettingsUtils.Config().defCoinInScale;
        _consoleBB.Instance.coinOutScaleCreditPerTicket = DefaultSettingsUtils.Config().defCoinOutPerTicket2Credit;
        _consoleBB.Instance.coinOutScaleTicketPerCredit = DefaultSettingsUtils.Config().defCoinOutPerCredit2Ticket;
        _consoleBB.Instance.scoreUpDownScale = DefaultSettingsUtils.Config().defScoreUpDownScale;

        // 默认语言
        _consoleBB.Instance.language = DefaultSettingsUtils.Config().defLanguage;

        // 默认密码
        _consoleBB.Instance.passwordAdmin = DefaultSettingsUtils.Config().passwordAdmin;
        _consoleBB.Instance.passwordManager = DefaultSettingsUtils.Config().passwordManager;
        _consoleBB.Instance.passwordShift = DefaultSettingsUtils.Config().passwordShift;

        // 记录最大次数
        _consoleBB.Instance.errorRecordMax = DefaultSettingsUtils.Config().defMaxErrorRecord;
        _consoleBB.Instance.eventRecordMax = DefaultSettingsUtils.Config().defMaxEventRecord;
        _consoleBB.Instance.jackpotRecordMax = DefaultSettingsUtils.Config().defMaxJackpotRecord;
        _consoleBB.Instance.businiessDayRecordMax = DefaultSettingsUtils.Config().defMaxBusinessDayRecord;
    }

}
