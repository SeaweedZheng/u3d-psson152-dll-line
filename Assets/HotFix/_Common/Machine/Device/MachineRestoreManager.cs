#define SQLITE_ASYNC
using GameMaker;
using Mono.Data.Sqlite;
using Sirenix.OdinInspector;
using SlotDllAlgorithmG152;
using System.Collections;
using UnityEngine;

using _consoleBB = PssOn00152.ConsoleBlackboard02;

/// <summary>
/// ��̨���ݻָ�
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
    /// <summary> �����һ�����л�װ </summary>
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
        DebugUtils.Log("�״ΰ�װ����λ���в���");
        RestoreToFactorySettings();
    }

    [Button]
    /// <summary>
    /// �������Ϸ��¼������Ͷ�˱Ҽ�¼�����ݡ�
    /// </summary>
    public void ClearRecordWhenCoding()
    {
        // ������Ϸ�ʽ�������ɰ汾������������
        //GameJackpotCreator.Instance.ResetGameJackpot();


        // �°汾�ʽ�
        SlotDllAlgorithmG152Manager.Instance.Clear();


        // ClearMachineCounter();
        DoCor(COR_CLEAR_SQL_TABLE, ClearSQLTable());
    }


    /// <summary>
    /// ���������Ŀǰ�޷������
    /// </summary>
    void ClearMachineCounter()
    {
        MachineDataManager.Instance.RequestCounter(0, 0, 1, (res) =>
        {
            int resault = (int)res;

            DebugUtils.Log($"���Ͷ����� : {resault}");

            // �������Ƕ�ף�MachineDataManager�ķ����޷��ظ����ã���Ӧ�ص��ᱻ����
            MachineDataManager.Instance.RequestCounter(1, 0, 1, (res) =>
            {
                int resault = (int)res;
                DebugUtils.Log($"����˱���� : {resault}");
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
    /// �����������������
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
            DebugUtil.LogError($"��Check Record��{dbName} is close");
            yield break;
        }

        // ���Ͷ�˱�
        tableName = ConsoleTableName.TABLE_COIN_IN_OUT_RECORD;
        if (SQLiteHelper01.Instance.CheckTableExists(tableName))
        {
            string sql = $"DELETE FROM {tableName};";
            SQLiteHelper01.Instance.ExecuteNonQuery(sql);
        }

        // �����Ϸ��¼
        tableName = ConsoleTableName.TABLE_SLOT_GAME_RECORD;
        if (SQLiteHelper01.Instance.CheckTableExists(tableName))
        {
            string sql = $"DELETE FROM {tableName};";
            SQLiteHelper01.Instance.ExecuteNonQuery(sql);
        }


        // ���ÿ��Ӫ�ռ�¼
        tableName = ConsoleTableName.TABLE_BUSINESS_DAY_RECORD;
        if (SQLiteHelper01.Instance.CheckTableExists(tableName))
        {
            string sql = $"DELETE FROM {tableName};";
            SQLiteHelper01.Instance.ExecuteNonQuery(sql);
        }


        // ����ʽ��¼
        tableName = ConsoleTableName.TABLE_JACKPOT_RECORD;
        if (SQLiteHelper01.Instance.CheckTableExists(tableName))
        {
            string sql = $"DELETE FROM {tableName};";
            SQLiteHelper01.Instance.ExecuteNonQuery(sql);
        }

        // ���������־
        tableName = ConsoleTableName.TABLE_LOG_ERROR_RECORD;
        if (SQLiteHelper01.Instance.CheckTableExists(tableName))
        {
            string sql = $"DELETE FROM {tableName};";
            SQLiteHelper01.Instance.ExecuteNonQuery(sql);
        }

        // ����¼���־
        tableName = ConsoleTableName.TABLE_LOG_EVENT_RECORD;
        if (SQLiteHelper01.Instance.CheckTableExists(tableName))
        {
            string sql = $"DELETE FROM {tableName};";
            SQLiteHelper01.Instance.ExecuteNonQuery(sql);
        }

        if (ApplicationSettings.Instance.isMock)
        {
            // �����ҽ�Ӯ�֣���ʷ��Ͷ������
            _consoleBB.Instance.sboxPlayerInfo = MachineDataManager.Instance.RequestClearPlayerAccountWhenMock();
            MainBlackboardController.Instance.SyncMyTempCreditToReal(true);

            string TEST_JP_GRAND = "TEST_JP_GRAND";
            string TEST_JP_MAJOR = "TEST_JP_MAJOR";
            string TEST_JP_MINOR = "TEST_JP_MINOR";
            string TEST_JP_MINI = "TEST_JP_MINI";
            // �����Ϸjackpot
            PlayerPrefs.SetInt(TEST_JP_GRAND, 0);
            PlayerPrefs.SetInt(TEST_JP_MAJOR, 0);
            PlayerPrefs.SetInt(TEST_JP_MINOR, 0);
            PlayerPrefs.SetInt(TEST_JP_MINI, 0);
        }

        // ɾ�����
        _mainBB.Instance.reportId = 0;
        _mainBB.Instance.gameNumber = 0;


        // ɾ����������
        EventCenter.Instance.EventTrigger<EventData>(GlobalEvent.ON_COIN_IN_OUT_EVENT,  new EventData(GlobalEvent.ClearAllOrderCache));


        Debug.Log("������б�����Ϸ��¼����");
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
            DebugUtils.LogError($"��Check Record��{dbName} is close");
            yield break;
        }

        // ���Ͷ�˱�
        tableName = ConsoleTableName.TABLE_COIN_IN_OUT_RECORD;
        SQLiteAsyncHelper.Instance.ExecuteDeleteAsync(tableName, (res) =>
        {
            isNext = true;
        });
        yield return new WaitUntil(() => isNext == true);
        isNext = false;


        // �����Ϸ��¼
        tableName = ConsoleTableName.TABLE_SLOT_GAME_RECORD;
        SQLiteAsyncHelper.Instance.ExecuteDeleteAsync(tableName, (res) =>
        {
            isNext = true;
        });
        yield return new WaitUntil(() => isNext == true);
        isNext = false;


        // ���ÿ��Ӫ�ռ�¼
        tableName = ConsoleTableName.TABLE_BUSINESS_DAY_RECORD;
        SQLiteAsyncHelper.Instance.ExecuteDeleteAsync(tableName, (res) =>
        {
            isNext = true;
        });
        yield return new WaitUntil(() => isNext == true);
        isNext = false;


        // �����Ӫ�ռ�¼
        tableName = ConsoleTableName.TABLE_BUSINESS_TOTAL_RECORD;
        SQLiteAsyncHelper.Instance.ExecuteDeleteAsync(tableName, (res) =>
        {
            isNext = true;
        });
        yield return new WaitUntil(() => isNext == true);
        isNext = false;

        //TableBusniessTotalRecordAsyncManager.Instance.OnEventClearAllOrderCache();
        TableBusniessTotalRecordAsyncManager.Instance.GetTotalBusniess();

        // ����ʽ��¼
        tableName = ConsoleTableName.TABLE_JACKPOT_RECORD;
        SQLiteAsyncHelper.Instance.ExecuteDeleteAsync(tableName, (res) =>
        {
            isNext = true;
        });
        yield return new WaitUntil(() => isNext == true);
        isNext = false;


        // ���������־
        tableName = ConsoleTableName.TABLE_LOG_ERROR_RECORD;
        SQLiteAsyncHelper.Instance.ExecuteDeleteAsync(tableName, (res) =>
        {
            isNext = true;
        });
        yield return new WaitUntil(() => isNext == true);
        isNext = false;


        // ����¼���־
        tableName = ConsoleTableName.TABLE_LOG_EVENT_RECORD;
        SQLiteAsyncHelper.Instance.ExecuteDeleteAsync(tableName, (res) =>
        {
            isNext = true;
        });
        yield return new WaitUntil(() => isNext == true);
        isNext = false;


        if (ApplicationSettings.Instance.isMock)
        {
            // �����ҽ�Ӯ�֣���ʷ��Ͷ������
            _consoleBB.Instance.sboxPlayerInfo = MachineDataManager.Instance.RequestClearPlayerAccountWhenMock();
            MainBlackboardController.Instance.SyncMyTempCreditToReal(true);

            string TEST_JP_GRAND = "TEST_JP_GRAND";
            string TEST_JP_MAJOR = "TEST_JP_MAJOR";
            string TEST_JP_MINOR = "TEST_JP_MINOR";
            string TEST_JP_MINI = "TEST_JP_MINI";
            // �����Ϸjackpot
            PlayerPrefs.SetInt(TEST_JP_GRAND, 0);
            PlayerPrefs.SetInt(TEST_JP_MAJOR, 0);
            PlayerPrefs.SetInt(TEST_JP_MINOR, 0);
            PlayerPrefs.SetInt(TEST_JP_MINI, 0);
        }

        // ɾ�����
        MainBlackboardController.Instance.ClearGameNumber();
        MainBlackboardController.Instance.ClearReportId();


        // ɾ����������
        EventCenter.Instance.EventTrigger<EventData>(GlobalEvent.ON_COIN_IN_OUT_EVENT, new EventData(GlobalEvent.ClearAllOrderCache));


        Debug.Log("������б�����Ϸ��¼����");
    }

#endif











    /// <summary>
    /// �ָ���������
    /// </summary>
    public void RestoreToFactorySettings()
    {
        // �����¼
        DoCor(COR_CLEAR_SQL_TABLE, ClearSQLTable());

        // �ָ�Ͷ�˱Ҳ�������
        _consoleBB.Instance.billInScale = DefaultSettingsUtils.Config().defBillInScale;
        _consoleBB.Instance.printOutScale = DefaultSettingsUtils.Config().defPrintOutScale;
        _consoleBB.Instance.coinInScale = DefaultSettingsUtils.Config().defCoinInScale;
        _consoleBB.Instance.coinOutScaleCreditPerTicket = DefaultSettingsUtils.Config().defCoinOutPerTicket2Credit;
        _consoleBB.Instance.coinOutScaleTicketPerCredit = DefaultSettingsUtils.Config().defCoinOutPerCredit2Ticket;
        _consoleBB.Instance.scoreUpDownScale = DefaultSettingsUtils.Config().defScoreUpDownScale;

        // Ĭ������
        _consoleBB.Instance.language = DefaultSettingsUtils.Config().defLanguage;

        // Ĭ������
        _consoleBB.Instance.passwordAdmin = DefaultSettingsUtils.Config().passwordAdmin;
        _consoleBB.Instance.passwordManager = DefaultSettingsUtils.Config().passwordManager;
        _consoleBB.Instance.passwordShift = DefaultSettingsUtils.Config().passwordShift;

        // ��¼������
        _consoleBB.Instance.errorRecordMax = DefaultSettingsUtils.Config().defMaxErrorRecord;
        _consoleBB.Instance.eventRecordMax = DefaultSettingsUtils.Config().defMaxEventRecord;
        _consoleBB.Instance.jackpotRecordMax = DefaultSettingsUtils.Config().defMaxJackpotRecord;
        _consoleBB.Instance.businiessDayRecordMax = DefaultSettingsUtils.Config().defMaxBusinessDayRecord;
    }

}
