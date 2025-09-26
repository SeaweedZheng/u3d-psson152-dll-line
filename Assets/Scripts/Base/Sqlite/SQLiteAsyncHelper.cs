using Mono.Data.Sqlite;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System;
using UnityEngine;
using System.IO;
using System.Collections;
using System.Reflection;
using System.Text;
using System.Linq;
using Newtonsoft.Json;





public partial class SQLiteAsyncHelper : MonoSingleton<SQLiteAsyncHelper>
{
    public bool isInit = false;

    // string TableName = "PlayerPrefs";
    IEnumerator Start()
    {

       // bool isNext = false;

        Initialize();

        //yield return new WaitUntil(() => dbConnection != null);
        yield return new WaitUntil(() => dbConnection != null && dbConnection.State == ConnectionState.Open);

        isInit = true;
    }


    /*   
    Coroutine corInit = null;
    void Init()
    {
        if (corInit != null)
            StopCoroutine(corInit);
        corInit = StartCoroutine(Connect());
    }

    IEnumerator Connect()
    {
        while (isEnableReconnect) { 

            isConnect = false;
            Initialize();

            float lastTimeS = Time.unscaledTime;
            yield return new WaitUntil(() => ( dbConnection != null && dbConnection.State == ConnectionState.Open) 
            || Time.unscaledTime - lastTimeS > 5f);

            if (Time.unscaledTime - lastTimeS <= 5f)
            {
                isConnect = true;

                while (isConnect)
                {
                    yield return null;
                }
            }
        }
    }

    bool isEnableReconnect = true;
    bool isConnect = false;
    */

}

public partial class SQLiteAsyncHelper : MonoSingleton<SQLiteAsyncHelper>
{

    public static string databaseName => ApplicationSettings.Instance.dbName; // "PssOn00152.db";

    private Thread dbThread;
    private Queue<Action> writeQueue = new Queue<Action>();
    private Queue<Action> readQueue = new Queue<Action>();
    private Queue<Action> mainThreadQueue = new Queue<Action>();


    /// <summary> isEanbleDB </summary>
    private bool isRunning = false;
    // private IDbConnection dbConnection = null;
    private SqliteConnection dbConnection = null;

    public bool isConnect => dbConnection!= null && dbConnection.State == ConnectionState.Open;


    // ��ʼ�����ݿ�
    public void Initialize()
    {
        if (isRunning) return;

        isRunning = true;
        dbThread = new Thread(DatabaseThread);
        dbThread.IsBackground = true;
        dbThread.Start(databaseName);
    }


    // ���ݿ��̷߳���
    private void DatabaseThread(object dbName)
    {
        try
        {


#if UNITY_EDITOR
            string connectionString = "URI=file:" + Path.Combine(Application.streamingAssetsPath, databaseName) +
                                     ";Version=3;Pooling=true;Max Pool Size=100;";
#elif UNITY_ANDROID

        string dataSandBoxPath = Path.Combine(Application.persistentDataPath, databaseName);
        if (!Directory.Exists(Application.persistentDataPath))
        {
            Directory.CreateDirectory(Application.persistentDataPath);
        }

        string connectionString = "URI=file:" + dataSandBoxPath +
                                     ";Version=3;Pooling=true;Max Pool Size=100;";
#endif


            dbConnection = new SqliteConnection(connectionString);
            dbConnection.Open();

            while (isRunning)
            {
                // ����д����
                if (writeQueue.Count > 0)
                {
                    Action writeAction;
                    lock (writeQueue)
                    {
                        writeAction = writeQueue.Dequeue();
                    }
                    writeAction.Invoke();

                    // д������ȴ�һС��ʱ�䣬����������
                    Thread.Sleep(10);
                }

                // ���������
                if (readQueue.Count > 0 && writeQueue.Count == 0)
                {
                    Action readAction;
                    lock (readQueue)
                    {
                        readAction = readQueue.Dequeue();
                    }
                    readAction.Invoke();
                }

                Thread.Sleep(1);
            }
        }
        catch (Exception e)
        {
            if (isRunning || !string.IsNullOrEmpty(e.Message))
            {
                Debug.LogError("���ݿ��̴߳���: " + e.Message);
                EnqueueToMainThread(() => { Debug.LogError("���ݿ����ʧ��: " + e.Message); });
            }
        }
        finally
        {
            CloseDatabase();
            //��������˳���Ϸ�������رգ���������
        }
    }


    /// <summary>
    /// ִ�зǲ�ѯ������д������
    /// </summary>
    /// <param name="query"></param>
    /// <param name="callback"></param>
    public void ExecuteNonQueryAsync(string query, Action<bool> callback = null)
    {
        lock (writeQueue)
        {
            writeQueue.Enqueue(() =>
            {
                bool success = false;
                try
                {
                    using (IDbCommand cmd = dbConnection.CreateCommand())
                    {
                        cmd.CommandText = query;
                        cmd.ExecuteNonQuery();
                        success = true;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"ִ�зǲ�ѯʧ��, error: {e.Message}  sql: {query}");
                }

                if (callback != null)
                    EnqueueToMainThread(() => callback(success));
            });
        }
    }


    /// <summary>
    /// ִ�зǲ�ѯ������д������
    /// </summary>
    /// <param name="query"></param>
    /// <param name="parameters"></param>
    /// <param name="callback"></param>
    public void ExecuteNonQueryAsync(string query, Dictionary<string, object> parameters, Action<bool> callback = null)
    {
        lock (writeQueue)
        {
            writeQueue.Enqueue(() =>
            {
                bool success = false;
                try
                {
                    /*using (IDbCommand cmd = dbConnection.CreateCommand())
                    {
                        cmd.CommandText = query;
                        cmd.ExecuteNonQuery();
                        success = true;
                    }*/


                    using (var command = new SqliteCommand(query, dbConnection))
                    {

                        foreach (KeyValuePair<string, object> kv in parameters)
                        {
                            command.Parameters.AddWithValue(kv.Key, kv.Value);
                        }
                        /*
                        command.Parameters.AddWithValue("@startTimestamp", startTimestamp);
                        command.Parameters.AddWithValue("@endTimestamp", endTimestamp);
                        command.Parameters.AddWithValue("@createdAt", timestamp);
                        command.Parameters.AddWithValue("@credit", credit);
                        command.Parameters.AddWithValue("@creditBefore", creditBefore);
                        command.Parameters.AddWithValue("@creditAfter", creditAfter);
                        */

                        /*
                        1. rowsAffected > 0
                        * ��ʾ SQL �������Ӱ����һ�����ݡ���ʱ��һ���жϣ�
                        * rowsAffected == 1 ˵��ִ����INSERT������������һ���¼�¼������־����� "New data inserted"
                        * ���� ˵��ִ����UPDATE���������������м�¼������־����� "Data updated"
                        2. rowsAffected <= 0
                        * ��ʾ SQL ���û��Ӱ���κ����ݡ�ͨ�������ڣ�
                        * ������ƥ�䣨��WHERE�Ӿ�δ�ҵ���¼��
                        * �����ظ����ݵ���Լ����ֹ
                        * SQL �߼�����
                        */
                        int rowsAffected = command.ExecuteNonQuery();

                        DebugUtils.Log(rowsAffected > 0 ?
                            (rowsAffected == 1 ? "��sqlite async helper��New data inserted" : "��sqlite async helper��Data updated") :
                            "��sqlite async helper��No operation performed");

                        success = rowsAffected > 0;

                    }

                }
                catch (Exception e)
                {
                     Debug.LogError($"ִ�зǲ�ѯʧ��, error: {e.Message}  sql: {query}");
                }

                if (callback != null)
                    EnqueueToMainThread(() => callback(success));

            });
        }
    }


    /// <summary>
    /// ִ�в�ѯ��������������
    /// </summary>
    /// <param name="query"></param>
    /// <param name="callback"></param>
    public void ExecuteQueryAsync(string query, Action<DataTable> callback)
    {
        if (callback == null) return;

        lock (readQueue)
        {
            readQueue.Enqueue(() =>
            {
                DataTable dataTable = new DataTable();
                try
                {
                    using (IDbCommand cmd = dbConnection.CreateCommand())
                    {
                        cmd.CommandText = query;
                        using (IDataReader reader = cmd.ExecuteReader())
                        {
                            dataTable.Load(reader);
                            reader.Close();
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("ִ�в�ѯʧ��: " + e.Message);
                }

                EnqueueToMainThread(() => callback(dataTable));
            });
        }
    }




    /// <summary>
    /// ִ�в�ѯ��������������
    /// </summary>
    /// <param name="query"></param>
    /// <param name="parameters"></param>
    /// <param name="callback"></param>
    public void ExecuteQueryAsync(string query, Dictionary<string, object> parameters, Action<SqliteDataReader> callback)
    {
        if (callback == null) return;

        lock (readQueue)
        {
            readQueue.Enqueue(() =>
            {
                try
                {
                    using (SqliteCommand command = new SqliteCommand(query, dbConnection))
                    {
                        if(parameters != null)
                            foreach (KeyValuePair<string, object> kv in parameters)
                            {
                                command.Parameters.AddWithValue(kv.Key, kv.Value);
                            }

                        SqliteDataReader reader = command.ExecuteReader();

                        EnqueueToMainThread(() => callback(reader));
                        /*
                        using (SqliteDataReader reader = command.ExecuteReader())
                        {
                            EnqueueToMainThread(() => callback(reader));
                        }*/
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("ִ�в�ѯʧ��: " + e.Message);
                }

            });
        }
    }



    /// <summary>
    /// �����д���ò�������bug������������ˢ�£����������������
    /// </summary>
    /// <param name="selectQuery"></param>
    /// <param name="updateQuery"></param>
    /// <param name="insertQuery"></param>
    /// <param name="callback"></param>
    /// <remarks>
    /// * ʹ��transaction����������д����
    /// </remarks>
    private void ExecuteUpdateOrInsertBUG(string selectQuery, string updateQuery, string insertQuery, Action<bool> callback)
    {
        lock (readQueue)
        {
            readQueue.Enqueue(() =>
            {

                Debug.Log($"��sqlite async helper�� selectQuery = {selectQuery}");
                Debug.Log($"��sqlite async helper�� updateQuery = {updateQuery}");
                Debug.Log($"��sqlite async helper�� insertQuery = {insertQuery}");

                using (var transaction = dbConnection.BeginTransaction())
                {
                    try
                    {
                        using (SqliteCommand selectCommand = new SqliteCommand(selectQuery, dbConnection, transaction))
                        {

                            SqliteDataReader reader = selectCommand.ExecuteReader();

                            if (reader.Read()) //reader.HasRows
                            {
                                // ���ݴ��ڣ�ִ�и��²���
                                reader.Close(); // �رն�ȡ������ִ����һ������

                                using (SqliteCommand updateCommand = new SqliteCommand(updateQuery, dbConnection, transaction))
                                {
                                    int rowsAffected = updateCommand.ExecuteNonQuery();

                                    DebugUtils.Log(rowsAffected > 0 ?
                                        (rowsAffected == 1 ? "��sqlite async helper��New data inserted" : "��sqlite async helper��Data updated") :
                                        "��sqlite async helper��No operation performed");

         
                                    EnqueueToMainThread(() => callback?.Invoke(rowsAffected > 0));
                                }
                            }
                            else
                            {
                                // ���ݲ����ڣ�ִ�в������
                                reader.Close();

                                using (SqliteCommand insertCommand = new SqliteCommand(insertQuery, dbConnection, transaction))
                                {
                                    int rowsAffected = insertCommand.ExecuteNonQuery();

                                    DebugUtils.Log(rowsAffected > 0 ?
                                        (rowsAffected == 1 ? "��sqlite async helper��New data inserted" : "��sqlite async helper��Data updated") :
                                        "��sqlite async helper��No operation performed");

     
                                    EnqueueToMainThread(() => callback?.Invoke(rowsAffected > 0));
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("ִ�в�ѯʧ��: " + e.Message);
                        if (transaction != null) transaction.Rollback(); // �ع�����
                        EnqueueToMainThread(() => callback?.Invoke(false));
                        throw;
                    }
                    finally
                    {
                        // if (transaction != null) transaction.Dispose();

                    }
                }

            });
        }
    }




    public void ExecuteUpdateOrInsertAsync(string selectQuery, string updateQuery, string insertQuery, Action<bool> callback)
    {
        lock (readQueue)
        {
            readQueue.Enqueue(() =>
            {

                //Debug.Log($"��sqlite async helper�� selectQuery = {selectQuery}");
                //Debug.Log($"��sqlite async helper�� updateQuery = {updateQuery}");
                //Debug.Log($"��sqlite async helper�� insertQuery = {insertQuery}");

                try
                {
                    using (SqliteCommand selectCommand = new SqliteCommand(selectQuery, dbConnection))
                    {

                        SqliteDataReader reader = selectCommand.ExecuteReader();

                        if (reader.HasRows) //  ��   if (reader.Read())
                        {
                            // ���ݴ��ڣ�ִ�и��²���
                            reader.Close(); // �رն�ȡ������ִ����һ������

                            using (SqliteCommand updateCommand = new SqliteCommand(updateQuery, dbConnection))
                            {
                                int rowsAffected = updateCommand.ExecuteNonQuery();
                                DebugUtils.Log($"��sqlite async helper�� updateQuery = {updateQuery}");
                                DebugUtils.Log(rowsAffected > 0 ?
                                    (rowsAffected == 1 ? "��sqlite async helper��New data inserted" : "��sqlite async helper��Data updated") :
                                    "��sqlite async helper��No operation performed");

                                EnqueueToMainThread(() => callback?.Invoke(rowsAffected > 0));
                            }
                        }
                        else
                        {
                            // ���ݲ����ڣ�ִ�в������
                            reader.Close();

                            using (SqliteCommand insertCommand = new SqliteCommand(insertQuery, dbConnection))
                            {
                                int rowsAffected = insertCommand.ExecuteNonQuery();
                                DebugUtils.Log($"��sqlite async helper�� insertQuery = {insertQuery}");
                                DebugUtils.Log(rowsAffected > 0 ?
                                    (rowsAffected == 1 ? "��sqlite async helper��New data inserted" : "��sqlite async helper��Data updated") :
                                    "��sqlite async helper��No operation performed");

                                EnqueueToMainThread(() => callback?.Invoke(rowsAffected > 0));
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("ִ�в�ѯʧ��: " + e.Message);
                    Debug.LogError($"��selectQuery��: {selectQuery}  ��updateQuery��: {updateQuery}  ��insertQuery��: {insertQuery}");
                    EnqueueToMainThread(() => callback?.Invoke(false));
                    throw;
                }
                finally
                {

                }
            });
        }
    }




    /// <summary>
    /// ɾ������������
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="maxRecord"></param>
    /// <param name="rowName"></param>
    /// <param name="callback"></param>
    public void ExecuteDeleteOverflowAsync(string tableName, int maxRecord, string rowName, Action<object[]> callback)
    {
        lock (readQueue)
        {
            readQueue.Enqueue(() =>
            {

                object[] res = new object[] { 0 };
                try
                {
                    // ������ݿ�����״̬
                    // if (dbConnection.State != System.Data.ConnectionState.Open) dbConnection.Open();

                    string existsQuery = $"SELECT COUNT(*) FROM sqlite_master WHERE type ='table' and name='{tableName}';";

                    using (SqliteCommand existsCommand = new SqliteCommand(existsQuery, dbConnection))
                    {

                        int count = Convert.ToInt32(existsCommand.ExecuteScalar());
                        //������Ϊ1���ʾ���ڸñ��
                        bool isExists = count == 1;
                        if (isExists) // 
                        {

                            string countQuery = $"SELECT COUNT(*) FROM {tableName}";

                            using (SqliteCommand countCommand = new SqliteCommand(countQuery, dbConnection))
                            {
                                object result = countCommand.ExecuteScalar();
                                long rowCount = (long)result;
                                if (maxRecord < rowCount)
                                {
                                    string deleteSQL = $"DELETE FROM {tableName} WHERE {rowName} NOT IN (  SELECT {rowName} FROM (  SELECT {rowName} FROM {tableName} ORDER BY {rowName} DESC LIMIT {maxRecord} ))";

                                    using (SqliteCommand deleteCommand = new SqliteCommand(deleteSQL, dbConnection))
                                    {
                                        int rowsAffected = deleteCommand.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                        else
                        {
                            res = new object[] { 1, $"�� {tableName} ������" };
                        }
                    }

                }
                catch (Exception e)
                {
                    Debug.LogError("ִ��ɾ��ʧ��: " + e.Message + $" tableName: {tableName}  maxRecord: {maxRecord}  rowName: {rowName}");
                    res = new object[] { 1, e.Message };
                    // throw e;
                }
                finally
                {
                    EnqueueToMainThread(() => callback?.Invoke(res));
                }
            });
        }
    }




    /// <summary>
    /// ɾ��������
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="callback"></param>
    public void ExecuteDeleteAsync(string tableName, Action<object[]> callback)
    {
        lock (readQueue)
        {
            readQueue.Enqueue(() =>
            {

                object[] res = new object[] { 0 };
                try
                {
                    // ������ݿ�����״̬
                    // if (dbConnection.State != System.Data.ConnectionState.Open) dbConnection.Open();

                    string existsQuery = $"SELECT COUNT(*) FROM sqlite_master WHERE type ='table' and name='{tableName}';";

                    using (SqliteCommand existsCommand = new SqliteCommand(existsQuery, dbConnection))
                    {

                        int count = Convert.ToInt32(existsCommand.ExecuteScalar());
                        //������Ϊ1���ʾ���ڸñ��
                        bool isExists = count == 1;
                        if (isExists) // 
                        {
                            string deleteSQL = $"DELETE FROM {tableName};";

                            using (SqliteCommand deleteCommand = new SqliteCommand(deleteSQL, dbConnection))
                            {
                                int rowsAffected = deleteCommand.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            res = new object[] { 1, $"�� {tableName} ������" };
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("ִ��ɾ��ʧ��: " + e.Message + $" tableName: {tableName}");
                    res = new object[] { 1, e.Message };
                    // throw e;
                }
                finally
                {
                    EnqueueToMainThread(() => callback?.Invoke(res));
                }
            });
        }
    }





    public void CheckOrCreatTableAsync<T>(string tableName, T[] defaultValue, Action<object[]> callback)
    {

        lock (readQueue)
        {
            readQueue.Enqueue(() =>
            {

                object[] res = new object[] { 0 };
                try
                {
                    // ������ݿ�����״̬
                    // if (dbConnection.State != System.Data.ConnectionState.Open) dbConnection.Open();

                    string existsQuery = $"SELECT COUNT(*) FROM sqlite_master WHERE type ='table' and name='{tableName}';";

                    using (SqliteCommand existsCommand = new SqliteCommand(existsQuery, dbConnection))
                    {
                        int count = Convert.ToInt32(existsCommand.ExecuteScalar());
                        //������Ϊ1���ʾ���ڸñ��
                        bool isExists = count == 1;
                        if (!isExists) // 
                        {

                            // ������
                            string creatSQL = SQLCreateTable<T>(tableName);
                            using (SqliteCommand creatCommand = new SqliteCommand(creatSQL, dbConnection))
                            {
                                creatCommand.ExecuteNonQuery();
                            }

                            // ����Ĭ������
                            if (defaultValue != null && defaultValue.Length > 0)
                            {
                                foreach (T item in defaultValue)
                                {
                                    string insertSQL = SQLInsertTableData<T>(tableName, item);

                                    using (SqliteCommand insertCommand = new SqliteCommand(insertSQL, dbConnection))
                                    {
                                        insertCommand.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("ִ��ɾ��ʧ��: " + e.Message + $" tableName: {tableName}");
                    res = new object[] { 1, e.Message };
                    // throw e;
                }
                finally
                {
                    EnqueueToMainThread(() => callback?.Invoke(res));
                }
            });
        }
    }



    public void CheckOrCreatTableAsync02<T>(string tableName, T[] defaultValue, Action<object[]> callback)
    {
        lock (readQueue)
        {
            readQueue.Enqueue(() =>
            {
                object[] res = new object[] { 0 };
                try
                {
                    // ������
                    string creatSQL = SQLCreateTableIfNotExists<T>(tableName);
                    using (SqliteCommand creatCommand = new SqliteCommand(creatSQL, dbConnection))
                    {
                        creatCommand.ExecuteNonQuery();


                        // ����Ĭ������
                        if (defaultValue != null && defaultValue.Length > 0)
                        {
                            foreach (T item in defaultValue)
                            {
                                string insertSQL = SQLInsertTableData<T>(tableName, item);

                                using (SqliteCommand insertCommand = new SqliteCommand(insertSQL, dbConnection))
                                {
                                    insertCommand.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("ִ��ɾ��ʧ��: " + e.Message + $" tableName: {tableName}");
                    res = new object[] { 1, e.Message };
                    // throw e;
                }
                finally
                {
                    EnqueueToMainThread(() => callback?.Invoke(res));
                }
            });
        }
    }




    public void GetDataAsync<T>(string tableName, string selectSQL, T[] defaultValue, Action<object[]> callback)
    {
        lock (readQueue)
        {
            readQueue.Enqueue(() =>
            {
                object[] res = new object[] { 0 ,"[]" };
                try
                {
                    // ������
                    string creatSQL = SQLCreateTableIfNotExists<T>(tableName);
                    using (SqliteCommand creatCommand = new SqliteCommand(creatSQL, dbConnection))
                    {
                        creatCommand.ExecuteNonQuery();
                    }

                    var dataTable = new DataTable();
                    using (SqliteCommand cmd = new SqliteCommand(selectSQL, dbConnection))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            /*  if (reader.Read()) // �����ᵼ�¶��������ݣ���������
                                 dataTable.Load(reader);*/
                           dataTable.Load(reader);
                        }
                    }
                    string result = JsonConvert.SerializeObject(dataTable, Formatting.Indented);  

                    if(!string.IsNullOrEmpty(result) && result != "[]")
                    {
                        res = new object[] { 0, result };
                    }
                    else {
                        // ����Ĭ������
                        if (defaultValue != null && defaultValue.Length > 0)
                        {
                            foreach (T item in defaultValue)
                            {
                                string insertSQL = SQLInsertTableData<T>(tableName, item);

                                using (SqliteCommand insertCommand = new SqliteCommand(insertSQL, dbConnection))
                                {
                                    insertCommand.ExecuteNonQuery();
                                }
                            }
                            // �ٴλ�ȡ����
                            var dataTable02 = new DataTable();
                            using (SqliteCommand cmd = new SqliteCommand(selectSQL, dbConnection))
                            {
                                using (var reader = cmd.ExecuteReader())
                                {
                                    /* if (reader.Read()) // �����ᵼ�¶��������ݣ���������
                                         dataTable02.Load(reader);*/
                                    dataTable02.Load(reader);
                                }
                            }
                            string result02 = JsonConvert.SerializeObject(dataTable02, Formatting.Indented);
                            res = new object[] { 0, result02 };
                        }
                    }


                }
                catch (Exception e)
                {
                    Debug.LogError("ִ��ɾ��ʧ��: " + e.Message + $" tableName: {tableName}");
                    res = new object[] { 1, e.Message };
                    // throw e;
                }
                finally
                {
                    EnqueueToMainThread(() => callback?.Invoke(res));
                }
            });
        }
    }


/*
    // ����Ƿ���� id=0 �ļ�¼
    var checkQuery = "SELECT * FROM TableBussinessTotalRecordItem WHERE id = 0";
            using var checkCommand = new SQLiteCommand(checkQuery, connection, transaction);
using var reader = checkCommand.ExecuteReader();

if (reader.Read())
{
    // ���ڼ�¼��ת��Ϊ�������л�Ϊ JSON
    var record = ReadRecordFromReader(reader);
    return JsonSerializer.Serialize(record);
}
else
{
    // �����ڼ�¼������Ĭ��ֵ
    var defaultRecord = CreateDefaultRecord();
    InsertRecord(defaultRecord, connection, transaction);
    return JsonSerializer.Serialize(defaultRecord);
}

*/





/// <summary>
/// ɾ�����
/// </summary>
/// <param name="tableName"></param>
/// <param name="callback"></param>
/// <remarks>
/// * ����ʹ��
/// </remarks>
public void ExecuteDropTableAsync(string tableName, Action<object[]> callback)
    {

        lock (readQueue)
        {
            readQueue.Enqueue(() =>
            {

                object[] res = new object[] { 0 };
                try
                {
                    // ������ݿ�����״̬
                    // if (dbConnection.State != System.Data.ConnectionState.Open) dbConnection.Open();
                    string existsQuery = $"SELECT COUNT(*) FROM sqlite_master WHERE type ='table' and name='{tableName}';";

                    using (SqliteCommand existsCommand = new SqliteCommand(existsQuery, dbConnection))
                    {
                        int count = Convert.ToInt32(existsCommand.ExecuteScalar());
                        //������Ϊ1���ʾ���ڸñ��
                        bool isExists = count == 1;
                        Debug.Log($"is {tableName} Exists: {isExists}");
                        if (isExists) // 
                        {
                            string dropSQL = $"DROP TABLE {tableName};";   
                            Debug.Log(dropSQL);
                            using (SqliteCommand dropCommand = new SqliteCommand(dropSQL, dbConnection))
                            {
                                dropCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("ִ��ɾ��ʧ��: " + e.Message + $" tableName: {tableName}");
                    res = new object[] { 1, e.Message };
                    // throw e;
                }
                finally
                {
                    EnqueueToMainThread(() => callback?.Invoke(res));
                }
            });
        }
    }

    // DROP TABLE IF EXISTS users;


    /// <summary>
    /// ɾ�����
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="callback"></param>
    /// <remarks>
    /// * ����ʹ��
    /// </remarks>
    public void ExecuteDropTableAsync02(string tableName, Action<object[]> callback)
    {

        lock (readQueue)
        {
            writeQueue.Enqueue(() =>
            {

                object[] res = new object[] { 0 };
                try
                {
                    string dropSQL = $"DROP TABLE IF EXISTS {tableName};";
                    using (SqliteCommand dropCommand = new SqliteCommand(dropSQL, dbConnection))
                    {
                        dropCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("ִ��ɾ��ʧ��: " + e.Message + $" tableName: {tableName}");
                    res = new object[] { 1, e.Message };
                    // throw e;
                }
                finally
                {
                    EnqueueToMainThread(() => callback?.Invoke(res));
                }
            });
        }
    }


    /// <summary>
    /// ִ�в�ѯ��������������
    /// </summary>
    /// <param name="query"></param>
    /// <param name="parameters"></param>
    /// <param name="callback"></param>
    public void ExecuteTotalCount(string query, Dictionary<string, object> parameters, Action<long> callback)
    {
        if (callback == null) return;

        lock (readQueue)
        {
            readQueue.Enqueue(() =>
            {
                try
                {
                    using (SqliteCommand command = new SqliteCommand(query, dbConnection))
                    {
                        if (parameters != null)
                            foreach (KeyValuePair<string, object> kv in parameters)
                            {
                                command.Parameters.AddWithValue(kv.Key, kv.Value);
                            }

                        long totalCount = 0;
                        // ִ�в�ѯ����ȡ���
                        object result = command.ExecuteScalar();
                        // ������
                        if (result != null && result != DBNull.Value)
                        {
                            totalCount = Convert.ToInt64(result);
                        }

                        EnqueueToMainThread(() => callback(totalCount));
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("ִ�в�ѯʧ��: " + e.Message);
                }

            });
        }
    }












    // �������������̶߳���
    private void EnqueueToMainThread(Action action)
    {
        lock (mainThreadQueue)
        {
            mainThreadQueue.Enqueue(action);
        }
    }

    // �����̸߳���
    void Update()
    {
        if (mainThreadQueue.Count > 0)
        {
            Action action;
            lock (mainThreadQueue)
            {
                action = mainThreadQueue.Dequeue();
            }
            action.Invoke();
        }
    }

    // �ر����ݿ�
    public void CloseDatabase()
    {
        try
        {
            isRunning = false;
            if (dbThread != null && dbThread.IsAlive)
            {
                dbThread.Abort();
                dbThread = null;
            }

            if (dbConnection != null)
            {
                dbConnection.Close();
                dbConnection = null;
            }


            lock (writeQueue)
            {
                if (writeQueue.Count > 0)
                    Debug.LogError($"writeQueue.Count= {writeQueue.Count}");
            }
            lock (readQueue)
            {
                if (readQueue.Count > 0)
                    Debug.LogError($"readQueue.Count= {readQueue.Count}");
            }

            Debug.LogWarning("�ѹر����ݿ� ");
        }
        catch (Exception e)
        {
            Debug.LogError("�ر����ݿ�ʧ��: " + e.Message);
        }
    }

    protected override void OnDestroy()
    {
        Debug.LogWarning("�ر����ݿ� OnDestroy");
        CloseDatabase();
        base.OnDestroy();
    }

}




public partial class SQLiteAsyncHelper : MonoSingleton<SQLiteAsyncHelper>
{

    public const string TYPE_INT = "INTEGER";
    public const string TYPE_STRING = "TEXT";
    public const string TYPE_FLOAT = "REAL";
    //����ֵ���� 0 �� 1 ��"INTEGER" ���ͣ�


    /// <summary>
    /// 
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="columns"></param>
    /// <param name="columnType"></param>
    /// <returns></returns>
    public static string SQLCreateTable(string tableName, Dictionary<string, string> dicKeyType)
    {
        string[] columns = dicKeyType.Keys.ToArray();
        string[] columnType = dicKeyType.Values.ToArray();

        StringBuilder cmdSrt = new StringBuilder(20);

        //���ݲ������д������SQL�����ַ���ƴ��
        //cmdSrt.Append($"CREATE TABLE {tableName}(");
        cmdSrt.Append($"CREATE TABLE {tableName}(id INTEGER PRIMARY KEY,");
        for (int i = 0; i < columns.Length; i++)
        {
            if (i > 0)
            {
                cmdSrt.Append(",");
            }

            //cmdSrt.Append($"{columns[i]} {columnType[i]}");

            // ��Ĭ��ֵ��
            if (columnType[i] == TYPE_INT)
            {
                cmdSrt.Append($"{columns[i]} {columnType[i]} DEFAULT 0");
            }
            else if (columnType[i] == TYPE_FLOAT)
            {
                cmdSrt.Append($"{columns[i]} {columnType[i]} DEFAULT 0.0");
            }
            else
            {
                cmdSrt.Append($"{columns[i]} {columnType[i]}");
            }

        }

        cmdSrt.Append(")");

        return cmdSrt.ToString();
    }




    public static string SQLCreateTable<T>(string tableName)
    {
        Type type = typeof(T);

        FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        List<string> columns = new List<string>();
        List<string> columnType = new List<string>();

        foreach (FieldInfo finfo in fieldInfos)
        {
            //object resObj = finfo.GetValue(obj);

            if (finfo.Name == "id")
                continue;

            columns.Add(finfo.Name);
            if (finfo.FieldType == typeof(string))
            {
                columnType.Add(TYPE_STRING);
            }
            else if (finfo.FieldType == typeof(int) || finfo.FieldType == typeof(long))
            {
                columnType.Add(TYPE_INT);
            }
            else if (finfo.FieldType == typeof(float))
            {
                columnType.Add(TYPE_FLOAT);
            }
            else
            {
                DebugUtils.LogError($" type: {finfo.FieldType} is not allow");
            }
        }


        StringBuilder cmdSrt = new StringBuilder(20);

        //���ݲ������д������SQL�����ַ���ƴ��
        //cmdSrt.Append($"CREATE TABLE {tableName}(");
        cmdSrt.Append($"CREATE TABLE {tableName}(id INTEGER PRIMARY KEY,");
        for (int i = 0; i < columns.Count; i++)
        {
            if (i > 0)
            {
                cmdSrt.Append(",");
            }

            //cmdSrt.Append($"{columns[i]} {columnType[i]}");

            // ��Ĭ��ֵ��
            if (columnType[i] == TYPE_INT)
            {
                cmdSrt.Append($"{columns[i]} {columnType[i]} DEFAULT 0");
            }
            else if (columnType[i] == TYPE_FLOAT)
            {
                cmdSrt.Append($"{columns[i]} {columnType[i]} DEFAULT 0.0");
            }
            else
            {
                cmdSrt.Append($"{columns[i]} {columnType[i]}");
            }
        }

        cmdSrt.Append(")");

        return cmdSrt.ToString();
    }





    public static string SQLCreateTableIfNotExists<T>(string tableName)
    {
        Type type = typeof(T);

        FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        List<string> columns = new List<string>();
        List<string> columnType = new List<string>();

        foreach (FieldInfo finfo in fieldInfos)
        {
            //object resObj = finfo.GetValue(obj);

            if (finfo.Name == "id")
                continue;

            columns.Add(finfo.Name);
            if (finfo.FieldType == typeof(string))
            {
                columnType.Add(TYPE_STRING);
            }
            else if (finfo.FieldType == typeof(int) || finfo.FieldType == typeof(long))
            {
                columnType.Add(TYPE_INT);
            }
            else if (finfo.FieldType == typeof(float))
            {
                columnType.Add(TYPE_FLOAT);
            }
            else
            {
                DebugUtils.LogError($" type: {finfo.FieldType} is not allow");
            }
        }


        StringBuilder cmdSrt = new StringBuilder(20);

        //���ݲ������д������SQL�����ַ���ƴ��
        //cmdSrt.Append($"CREATE TABLE {tableName}(");
        cmdSrt.Append($"CREATE TABLE IF NOT EXISTS {tableName}(id INTEGER PRIMARY KEY,");
        for (int i = 0; i < columns.Count; i++)
        {
            if (i > 0)
            {
                cmdSrt.Append(",");
            }

            //cmdSrt.Append($"{columns[i]} {columnType[i]}");

            // ��Ĭ��ֵ��
            if (columnType[i] == TYPE_INT)
            {
                cmdSrt.Append($"{columns[i]} {columnType[i]} DEFAULT 0");
            }
            else if (columnType[i] == TYPE_FLOAT)
            {
                cmdSrt.Append($"{columns[i]} {columnType[i]} DEFAULT 0.0");
            }
            else
            {
                cmdSrt.Append($"{columns[i]} {columnType[i]}");
            }
        }

        cmdSrt.Append(")");

        return cmdSrt.ToString();
    }




    /// <summary>
    /// "INSERT INTO test_student_info (name, numb, age, height) VALUES ('С��', 1, 12, 15.9)";
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="keys"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public static string SQLInsertTableData(string tableName, Dictionary<string, object> dicKeyValue)
    {

        StringBuilder cmdSrt = new StringBuilder(20);

        //���ݲ������д������SQL�����ַ���ƴ��
        cmdSrt.Append($"INSERT INTO {tableName}(");

        string[] keys = dicKeyValue.Keys.ToArray();
        object[] values = dicKeyValue.Values.ToArray();
        for (int i = 0; i < keys.Length; i++)
        {
            if (i > 0)
            {
                cmdSrt.Append(",");
            }

            cmdSrt.Append($"{keys[i]}");
        }

        cmdSrt.Append(") VALUES (");


        for (int i = 0; i < values.Length; i++)
        {
            if (i > 0)
            {
                cmdSrt.Append(",");
            }

            //cmdSrt.Append($" {values[i]}");

            if (values[i] is string)
                cmdSrt.Append(" '" + values[i] + "'");
            else
                cmdSrt.Append(" " + values[i]);

        }

        cmdSrt.Append(")");

        return cmdSrt.ToString();
    }




    public static string SQLInsertTableData<T>(string tableName, T obj)
    {
        Type type = typeof(T);

        FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        List<string> keys = new List<string>();
        List<object> values = new List<object>();

        foreach (FieldInfo finfo in fieldInfos)
        {
            object resObj = finfo.GetValue(obj);

            if (finfo.Name == "id")
                continue;

            keys.Add(finfo.Name);
            values.Add(resObj);
        }


        StringBuilder cmdSrt = new StringBuilder(20);

        //���ݲ������д������SQL�����ַ���ƴ��
        cmdSrt.Append($"INSERT INTO {tableName}(");


        for (int i = 0; i < keys.Count; i++)
        {
            if (i > 0)
            {
                cmdSrt.Append(",");
            }

            cmdSrt.Append($"{keys[i]}");
        }

        cmdSrt.Append(") VALUES (");


        for (int i = 0; i < values.Count; i++)
        {
            if (i > 0)
            {
                cmdSrt.Append(",");
            }

            //cmdSrt.Append($" {values[i]}");

            if (values[i] is string)
                cmdSrt.Append(" '" + values[i] + "'");
            else
                cmdSrt.Append(" " + values[i]);

        }

        cmdSrt.Append(")");

        return cmdSrt.ToString();
    }




    public static string SQLUpdateTableData<T>(string tableName, T obj)
    {
        Type type = typeof(T);

        FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        List<string> keys = new List<string>();
        List<object> values = new List<object>();
        long id = 1;
        foreach (FieldInfo finfo in fieldInfos)
        {
            object resObj = finfo.GetValue(obj);

            if (finfo.Name == "id")
            {
                id = (long)resObj;
                continue;
            }

            keys.Add(finfo.Name);
            values.Add(resObj);
        }

        StringBuilder cmdSrt = new StringBuilder(20);

        //���ݲ������д������SQL�����ַ���ƴ��
        cmdSrt.Append($"UPDATE {tableName} SET ");

        // "UPDATE test_student_info SET numb = 4, height = 159 WHERE name = 'С��33'";

        for (int i = 0; i < keys.Count; i++)
        {
            if (i > 0)
            {
                cmdSrt.Append(",");
            }

            if (values[i] is string)
                cmdSrt.Append(keys[i] + " = '" + values[i] + "'");
            else
                cmdSrt.Append(keys[i] + " = " + values[i]);
            //cmdSrt.Append($"{keys[i]} = {values[i]}");
        }

        cmdSrt.Append($" WHERE  id = {id}");

        return cmdSrt.ToString();
    }




}



public partial class SQLiteAsyncHelper : MonoSingleton<SQLiteAsyncHelper>
{
    public void CheckTableExistsAsync(string tableName, Action<bool> callback)
    {
        lock (writeQueue)
        {
            writeQueue.Enqueue(() =>
            {
                string sql = $"SELECT COUNT(*) FROM sqlite_master WHERE type ='table' and name='{tableName}';";
                try
                {
                    using (IDbCommand command = dbConnection.CreateCommand())
                    {
                        command.CommandText = sql;
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        bool isExists = count == 1;
                        EnqueueToMainThread(() => callback?.Invoke(isExists));
                    }
                }
                catch (Exception e)
                {
                     Debug.LogError($"ִ�зǲ�ѯʧ��, error: {e.Message}  sql: {sql}");
                    EnqueueToMainThread(() => callback?.Invoke(false));
                }


            });
        }
    }




    /// <summary>
    /// ���ݿ�����תjson
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="dbName"></param>
    /// <returns></returns>
    public void ConvertSqliteToJsonAsync(string sql, Action<string> callback)
    {
        if (!sql.Contains("SELECT"))
        {
            DebugUtils.LogError($"������ SELECT ҵ��");
            return;
        }


        lock (writeQueue)
        {
            writeQueue.Enqueue(() =>
            {

                var dataTable = new DataTable();
                using (SqliteCommand cmd = new SqliteCommand(sql, dbConnection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                }
                string res = JsonConvert.SerializeObject(dataTable, Formatting.Indented);

                EnqueueToMainThread(() => callback?.Invoke(res));
            });

        }
    }
}