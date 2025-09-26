using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System;
using Mono.Data.Sqlite;
using UnityEngine;
using System.IO;
using System.Text;
using SimpleJSON;
using System.Threading.Tasks;
using Sirenix.OdinInspector;



/// <summary>
/// 
/// </summary>
/// <remarks>
/// * ���ϸ��汾����˼��ܵĹ���
/// * ������̣߳��������ui����
/// 
/// * Unity ���̷߳������ƣ�Unity �� UI �� GameObject �������������߳̽��У���˻ص�����ͨ��EnqueueToMainThread�����ص����߳�ִ��
/// * �߳�ͬ����
/// * д����ʹ�õ������У�ȷ��˳��ִ��
/// * ��������д���п�ʱִ�У������ͻ
/// * ʹ��lock��֤�̰߳�ȫ
/// * �쳣����
/// * �������ݿ������������ try-catch ��
/// * ������Ϣͨ�����̻߳ص���ʾ
/// * �������ڹ���
/// * ʹ�õ���ģʽ�������ݿ�����
/// * ��Ӧ���˳�ʱ��ȷ�ر����ݿ�����
/// * 
/// * ͨ�����ַ�ʽ��������� Unity ��ʹ�� SQLite3 �������ݿ�����������������̣߳��Ӷ����⻭�濨�١�
/// </remarks>
public partial class SQLitePlayerPrefs03 : MonoSingleton<SQLitePlayerPrefs03>
{


    private Thread dbThread;
    private Queue<Action> writeQueue = new Queue<Action>();
    private Queue<Action> readQueue = new Queue<Action>();
    private Queue<Action> mainThreadQueue = new Queue<Action>();
    private bool isRunning = false;
    private IDbConnection dbConnection = null;




    // ��ʼ�����ݿ�
    public void Initialize(string databaseName = "SQLitePlayerPrefs03.db")
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
            // string databaseName = dbName as string;

            string databaseName = "SQLitePlayerPrefs03.db";

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
        }
    }

    // ִ�зǲ�ѯ������д������
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
                    Debug.LogError("ִ�зǲ�ѯʧ��: " + e.Message);
                }

                if (callback != null)
                    EnqueueToMainThread(() => callback(success));
            });
        }
    }

    // ִ�в�ѯ��������������
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

public partial class SQLitePlayerPrefs03 : MonoSingleton<SQLitePlayerPrefs03>
{
    Dictionary<string, object> tempKV = new Dictionary<string, object>();

    public bool isInit = false;

    string TableName = "PlayerPrefs";

    IEnumerator Start()
    {

        bool isNext = false;

        Initialize();

        yield return new WaitUntil(() => dbConnection != null);

        CreateTable(TableName, new string[] { "key", "value", "type", "ciphertext" }, new string[] { "TEXT", "TEXT", "TEXT", "TEXT" }, (isSuccess) =>
        {
            if (!isSuccess)
            {
                Debug.LogError($"creat table file: {TableName}");
            }
            isNext = isSuccess;
        });

        yield return new WaitUntil(() => isNext == true);
        isNext = false;

        string query = $"SELECT * FROM {TableName}";

        ExecuteQueryAsync(query, (dataTable) =>
        {
            if (dataTable.Rows.Count != 0)
            {
                Debug.Log($"�� '{TableName}' ���в�ѯ�� {dataTable.Rows.Count} ����¼:");

                // ���������У���̬����ֶ�����ֵ
                foreach (DataRow row in dataTable.Rows)
                {
                    /*string rowInfo = "��¼: ";
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        rowInfo += $"{column.ColumnName} = {row[column]}, ";
                    }
                    Debug.Log(rowInfo);*/

                    string plaintext = GetPlaintext((string)row["ciphertext"]);
                    JSONNode node = JSONNode.Parse(plaintext);
                    string type = (string)node["type"];
                    string key = (string)node["key"];
                    string res = (string)node["value"];
                    switch (type)
                    {
                        case "int":
                            tempKV.Add(key, int.Parse(res));
                            break;
                        case "float":
                            tempKV.Add(key, float.Parse(res));
                            break;
                        case "string":
                            tempKV.Add(key, (string)res);
                            break;
                        default:
                            DebugUtils.LogError($"���ݸ�ʽ���� key: {key} type: {type}");
                            break;
                    }
                }
            }

            isNext = true;
        });

        yield return new WaitUntil(() => isNext == true);
        isNext = false;

        isInit = true;
        DebugUtils.LogWarning($"���ݿ��ʼ����ɣ� {TableName}");
    }


    /// <summary>
    /// ������
    /// sqlCmd:CREATE TABLE tableName (column1 datatype1,column2 datatype2,column3 ///datatype3,.....columnN datatypeN)
    /// </summary>
    /// <param name="tableName"> �������</param>
    /// <param name="columns"></param>
    /// <param name="columnType">����</param>
    public void CreateTable(string tableName, string[] columns, string[] columnType, Action<bool> callback = null)
    {
        //����ֶ������ֶ����ͳ��Ȳ�һ�����ܴ������
        if (columns.Length != columnType.Length)
        {
            DebugUtils.LogError("Colum's Length != ColumType's Length");
            return;
        }

        StringBuilder cmdSrt = new StringBuilder(20);

        //���ݲ������д������SQL�����ַ���ƴ��
        cmdSrt.Append($"CREATE  TABLE IF NOT EXISTS {tableName}(");

        for (int i = 0; i < columns.Length; i++)
        {
            if (i > 0)
            {
                cmdSrt.Append(",");
            }

            cmdSrt.Append($"{columns[i]} {columnType[i]}");
        }

        cmdSrt.Append(")");

        //ִ������
        ExecuteNonQueryAsync(cmdSrt.ToString(), callback);
    }



    /// <summary>
    /// ����в�������
    /// sqlCmd:INSERT INTO tableName VALUES(value1, value2, value3,...valueN)
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="insertDatas"></param>
    public void InsertTableData(string tableName, string[] insertDatas, Action<bool> callback = null)
    {
        if (insertDatas.Length == 0)
        {
            DebugUtils.LogError("Values's length == 0");
        }

        StringBuilder cmdSrt = new StringBuilder(20);

        //���ݲ������д������SQL�����ַ���ƴ��
        cmdSrt.Append($"INSERT INTO {tableName} VALUES(");

        for (int i = 0; i < insertDatas.Length; i++)
        {
            if (i > 0)
            {
                cmdSrt.Append(",");
            }

            //cmdSrt.Append($"'{insertDatas[i].ToString()}'");
            cmdSrt.Append($"{insertDatas[i].ToString()}");
        }

        cmdSrt.Append(")");

        string sql = cmdSrt.ToString();
#if UNITY_EDITOR
        DebugUtils.Log($"sql = {sql}");
#endif

        //ִ�в�����������
        ExecuteNonQueryAsync(sql, callback);

    }

    /// <summary>
    /// ����и�������
    /// sqlCmd:UPDATE tableName SET column1 = value1, column2 = value2...., columnN = valueN
    /// UPDATE table1 SET Age = 11, WHERE Id = 1;
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="updateDatas"></param>
    public void UpdateTableData(string tableName, string[] updateDatas, string[] where = null, Action<bool> callback = null)
    {
        if (updateDatas.Length == 0)
            DebugUtils.LogError("Values's length == 0");

        StringBuilder cmdSrt = new StringBuilder(20);

        //���ݲ������д������SQL�����ַ���ƴ��
        cmdSrt.Append($"UPDATE {tableName} SET ");

        for (int i = 0; i < updateDatas.Length; i += 2)
        {
            if (i > 0)
            {
                cmdSrt.Append(",");
            }
            //cmdSrt.Append($"'{updateDatas[i]}'='{updateDatas[i + 1]}'"); 
            cmdSrt.Append($"{updateDatas[i]}={updateDatas[i + 1]}");
        }

        if (where != null)
        {
            string whereStr = "";
            if (where != null)
            {
                for (int i = 0; i < where.Length; i += 2)
                {
                    if (i > 0)
                    {
                        whereStr += (",");
                    }
                    //whereStr += $"'{where[i]}'='{where[i + 1]}'";
                    whereStr += $"{where[i]}={where[i + 1]}";
                }
            }
            cmdSrt.Append($" WHERE {whereStr}");
        }


        string sql = cmdSrt.ToString();

#if UNITY_EDITOR
        DebugUtils.Log($"sql = {sql}");
#endif

        //ִ�и�����������
        ExecuteNonQueryAsync(sql, callback);

    }


    /// <summary>
    /// ��ȡ�������е����� 
    /// sqlCmd:SELECT * FROM tableName
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="tableName"></param>
    /// <returns></returns>
    public void GetTableAllData(string tableName, string[] where, Action<List<Dictionary<string, object>>> callback)
    {
        List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();

        string whereStr = "";
        if (where != null)
        {
            for (int i = 0; i < where.Length; i += 2)
            {
                if (i > 0)
                {
                    whereStr += (",");
                }
                whereStr += $"{where[i]}={where[i + 1]}";
            }
        }

        //��ѯ����
        string sql = where == null ? $"SELECT * FROM {tableName}" : $"SELECT * FROM {tableName} WHERE {whereStr}";

#if UNITY_EDITOR
        DebugUtils.Log($"sql = {sql}");
#endif

        ExecuteQueryAsync(sql, (dataTable) =>
        {
            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                Debug.Log($"û���ҵ���¼: {sql}");
                callback.Invoke(null);
                return;
            }

            foreach (DataRow row in dataTable.Rows)
            {
                Dictionary<string, object> dataDic = new Dictionary<string, object>();
                //string rowInfo = "";
                foreach (DataColumn column in dataTable.Columns)
                {
                    //rowInfo += $"{column.ColumnName}: {row[column]}, ";
                    dataDic.Add(column.ColumnName, row[column]);
                }
                //Debug.Log(rowInfo);
                dataList.Add(dataDic);
            }
            callback.Invoke(dataList);
        });
    }
}


public partial class SQLitePlayerPrefs03 : MonoSingleton<SQLitePlayerPrefs03>
{
    private int ExecuteNonQuery(string sqlQuery)
    {
        using (IDbCommand command = dbConnection.CreateCommand())
        {
            command.CommandText = sqlQuery;
            int rowsAffected = command.ExecuteNonQuery();
            return rowsAffected;
        }
    }
}
public partial class SQLitePlayerPrefs03 : MonoSingleton<SQLitePlayerPrefs03>
{

    #region ���ܽ���
    public string GetCiphertext(string key, string value, string type)
    {
        JSONNode node = JSONNode.Parse("{}");
        node["key"] = key;
        node["value"] = value;
        node["type"] = type;
        return GetCiphertext(node.ToString());
    }

    public string GetCiphertext(string plaintext)
    {
        string ciphertext = AesManager.Instance.TryLocalEncrypt(plaintext);
        return ciphertext;
    }

    public string GetPlaintext(string ciphertext)
    {
        string plaintext = AesManager.Instance.TryLocalDecrypt(ciphertext);
        return plaintext;
    }

    #endregion


    void SetData(string key, string value, string type)
    {

        object obj = null;
        switch (type)
        {
            case "int":
                obj = int.Parse(value);
                break;
            case "float":
                obj = float.Parse(value);
                break;
            case "string":
                obj = (string)value;
                break;
            default:
                DebugUtils.LogError($"���ݸ�ʽ���� key: {key} type: {type}");
                break;
        };


        if (isInit == false)
        {
            Debug.LogError("db init has not finish!!");
            return;
        }

        //List<Dictionary<string, object>> data = 

        string ciphertext = GetCiphertext(key, value, type);

        if (tempKV.ContainsKey(key))
        {
            tempKV[key] = obj;

            UpdateTableData("PlayerPrefs", new string[] { "value", $"'{value}'", "ciphertext", $"'{ciphertext}'" }, new string[] { "key", $"'{key}'" });
        }
        else
        {
            tempKV.Add(key, obj);

            GetTableAllData("PlayerPrefs", new string[] { "key", $"'{key}'" }, (List<Dictionary<string, object>> data) =>
            {
                if (data != null && data.Count > 0)
                {
                    UpdateTableData("PlayerPrefs", new string[] { "value", $"'{value}'", "ciphertext", $"'{ciphertext}'" }, new string[] { "key", $"'{key}'" });
                }
                else
                {
                    InsertTableData("PlayerPrefs", new string[] { $"'{key}'", $"'{value}'", $"'{type}'", $"'{ciphertext}'" });
                }
            });
        }
    }



    void SetData02(string key, string value, string type)
    {
        if (isInit == false)
        {
            Debug.LogError("db init has not finish!!");
            return;
        }

        object obj = null;
        switch (type)
        {
            case "int":
                obj = int.Parse(value);
                break;
            case "float":
                obj = float.Parse(value);
                break;
            case "string":
                obj = (string)value;
                break;
            default:
                DebugUtils.LogError($"���ݸ�ʽ���� key: {key} type: {type}");
                break;
        };



        //List<Dictionary<string, object>> data = 

        string ciphertext = GetCiphertext(key, value, type);

        if (tempKV.ContainsKey(key))
        {
            tempKV[key] = obj;

            UpdateTableData("PlayerPrefs", new string[] { "value", $"'{value}'", "ciphertext", $"'{ciphertext}'" }, new string[] { "key", $"'{key}'" });

            // UpdateTableData("PlayerPrefs", new string[] { "value", $"'{value}'",  "type", $"'{type}'", "ciphertext", $"'{ciphertext}'" }, new string[] { "key", $"'{key}'" });
        }
        else
        {
            tempKV.Add(key, obj);

            InsertTableData("PlayerPrefs", new string[] { $"'{key}'", $"'{value}'", $"'{type}'", $"'{ciphertext}'" });
        }
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    /// <remarks>
    /// * ���ص���װ���첽����
    /// </remarks>
    public async Task<object> GetData(string key, object defaultValue, string type)
    {
        if (isInit == false)
        {
            Debug.LogError("db init has not finish!!");
            return null;
        }

        if (tempKV.ContainsKey(key))
        {
            return tempKV[key];
        }
        else
        {
            var tcs = new TaskCompletionSource<object>();

            GetTableAllData("PlayerPrefs", new string[] { "key", $"'{key}'" }, (List<Dictionary<string, object>> data) =>
            {
                if (data != null && data.Count > 0)
                {
                    string plaintext = GetPlaintext((string)data[0]["ciphertext"]);
                    JSONNode node = JSONNode.Parse(plaintext);
                    string res = (string)node["value"];
                    switch (type)
                    {
                        case "int":
                            tempKV.Add(key, int.Parse(res));
                            break;
                        case "float":
                            tempKV.Add(key, float.Parse(res));
                            break;
                        case "string":
                            tempKV.Add(key, (string)res);
                            break;
                        default:
                            DebugUtils.LogError($"���ݸ�ʽ���� key: {key} type: {type}");
                            break;
                    }
                }
                else
                {
                    tempKV.Add(key, defaultValue);
                    string ciphertext = GetCiphertext(key, $"{defaultValue}", type);
                    InsertTableData("PlayerPrefs", new string[] { $"'{key}'", $"'{defaultValue}'", $"'{type}'", $"'{ciphertext}'" });
                }

                tcs.SetResult(tempKV[key]);
            });

            return await tcs.Task;
        }

    }


    public object GetData02(string key, object defaultValue, string type)
    {
        if (isInit == false)
        {
            Debug.LogError("db init has not finish!!");
            return null;
        }

        if (tempKV.ContainsKey(key))
        {
            return tempKV[key];
        }
        else
        {
            tempKV.Add(key, defaultValue);

            string ciphertext = GetCiphertext(key, $"{defaultValue}", type);
            InsertTableData("PlayerPrefs", new string[] { $"'{key}'", $"'{defaultValue}'", $"'{type}'", $"'{ciphertext}'" });

            return defaultValue;
        }

    }


    /// <summary>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    /// <remarks>
    /// <para>PlayerPrefs.GetString(string key, string defaultValue)  // ��ȡ����key���򷵻�defaultValue�� ���ǲ���ͬʱ����defaultValue</para>
    /// <para>SQLitePlayerPrefs02.GetString(string key, string defaultValue)  // ��ȡ����key���򷵻�defaultValue�� ͬʱ����defaultValue</para>
    /// </remarks>

    /*
        public async Task<string> GetString(string key, string defaultValue = null)
        {
             object data = await GetData(key, defaultValue, "string");
             return (string)data;
        }

        public void SetString(string key, string value) => SetData(key, value, "string");

        public async Task<float> GetFloat(string key, float defaultValue)
        {
             object data = await GetData(key, defaultValue, "float");
             return (float) data;
        }
        public void SetFloat(string key, float value) => SetData(key, $"{value}", "float");

        public async Task<int> GetInt(string key, int defaultValue)
        {
             object data = await GetData(key, defaultValue, "int");
             return (int) data;
        }

        public void SetInt(string key, int value) => SetData(key, $"{value}", "int");


        */

    public string GetString(string key, string defaultValue = null) => (string)GetData02(key, defaultValue, "string");

    public void SetString(string key, string value) => SetData02(key, value, "string");

    public float GetFloat(string key, float defaultValue) => (float)GetData02(key, defaultValue, "float");
    public void SetFloat(string key, float value) => SetData02(key, $"{value}", "float");

    public int GetInt(string key, int defaultValue) => (int)GetData02(key, defaultValue, "int");
    public void SetInt(string key, int value) => SetData02(key, $"{value}", "int");


    [Button]
    public void DeleteKey(string key)
    {
        string sqlQuery = $"DELETE FROM PlayerPrefs WHERE key = '{key}'";
        ExecuteNonQuery(sqlQuery);

        if (tempKV.ContainsKey(key))
            tempKV.Remove(key);
    }

    [Button]
    public void DeleteKeyAsync(string key)
    {
        string sqlQuery = $"DELETE FROM PlayerPrefs WHERE key = '{key}'";
        ExecuteNonQueryAsync(sqlQuery);

        if (tempKV.ContainsKey(key))
            tempKV.Remove(key);
    }
    
}

