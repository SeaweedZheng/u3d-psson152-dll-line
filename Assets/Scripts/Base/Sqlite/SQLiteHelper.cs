using UnityEngine;
using Mono.Data.Sqlite;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Data;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using System.Reflection;
using UnityEngine.Networking;

public partial class SQLiteHelper : MonoSingleton<SQLiteHelper>
{

    private SqliteConnection _connection;
    private SqliteCommand _command;
    private string _connectString;

    public string curDBName = null;




    /*public class Task
    {
        public string dbName;
        public string sql;
        public Action callback;
    }*/

    List<IEnumerator> taskLst = new List<IEnumerator>();

    Coroutine _cor;
    void ClearCor()
    {
        if (_cor != null)
            StopCoroutine(_cor);
        _cor = null;
    }


    // IEnumerator _OpenDB(string dbName = "Default.db",  Action<SqliteConnection> callBack = null)
    IEnumerator _OpenDB(string dbName, Action<SqliteConnection> callBack = null)
    {
        if (string.IsNullOrEmpty(dbName))
        {
            DebugUtils.LogError(" dbName is null ");
            yield break; 
        }

        if (_connection == null || _connection.State == ConnectionState.Closed || curDBName != dbName)
        {            
            Debug.LogWarning($"�л����ݿ�{curDBName}Ϊ{dbName}");

            // �ر����ݿ�
            CloseDB();

/*
#if UNITY_EDITOR
            _connectString = "URI=file:" + Path.Combine(Application.streamingAssetsPath, dbName);
            DebugUtil.Log($"streamingAssets�ݿ� {dbName}  url = {_connectString}");
#elif UNITY_ANDROID

            yield return copyDbFile(dbName);

#endif
*/

            if (Application.isEditor)
            {
                _connectString = "URI=file:" + Path.Combine(Application.streamingAssetsPath, dbName);
                DebugUtils.Log($"streamingAssets�ݿ� {dbName}  url = {_connectString}");
            }
            else
            {

                //yield return copyDbFile(dbName);

                // ��������ֱ�Ӵ���
                string targetDBPath = Path.Combine(Application.persistentDataPath, dbName);
                _connectString = "Data Source=" + targetDBPath;
            }


            _connection = new SqliteConnection(_connectString);
            try
            {
                if (_connection.State == ConnectionState.Closed)
                {
                    _connection.Open();
                    curDBName = dbName;
                    DebugUtils.Log($"���ݿ� {dbName} �����ѽ�������.");
                }
                else
                {
                    DebugUtils.LogError($"���ݿ� {dbName} ����ʧ��. State:{_connection.State} ");
                }
            }
            catch (Exception e)
            {
                DebugUtils.Log(e);
            }
        }

        callBack?.Invoke(_connection);

        if(taskLst.Count > 0)
        {
            IEnumerator nextTask = taskLst[0];
            taskLst.RemoveAt(0);
            yield return nextTask;
        }

        _cor = null;

    }


    IEnumerator copyDbFile(string dbName = "Default.db")
    {
        if (string.IsNullOrEmpty(dbName) )
        {
            DebugUtils.LogError(" dbName is null ");
        }
        //string targetDBPath1 = Application.persistentDataPath + $"/{dbName}";

        string targetDBPath = Path.Combine(Application.persistentDataPath, dbName);

       // DebugUtil.Log($"targetDBPath1 {targetDBPath1}  targetDBPath = {targetDBPath}");

        //connectString = "URI=file:" + targetDBPath;
        _connectString = "Data Source=" + targetDBPath;
        DebugUtils.Log($"�������ݿ� {dbName}  url = {_connectString}");

        if (!Directory.Exists(Application.persistentDataPath))
        {
            Directory.CreateDirectory(Application.persistentDataPath);
        }

        if (File.Exists(targetDBPath))
        {
            DebugUtils.Log($"���ݿ�{dbName}���� �����κβ���");
            yield break;
        }

        DebugUtils.Log($"����û�����ݿ⣬��ʼ�����������ݿ�{dbName}������");


        string sourceDBPath = Path.Combine(Application.streamingAssetsPath, dbName);  // �������������������⣿��

        // ������������
        if (File.Exists(sourceDBPath))
        {
            WWW loadWWW = new WWW(sourceDBPath);
            yield return loadWWW;
            File.WriteAllBytes(targetDBPath, loadWWW.bytes);
            DebugUtils.Log($"�������ݿ�{sourceDBPath}  �����أ� {targetDBPath}");
        }
        else{
            DebugUtils.LogError($"�������ݿ⣺{sourceDBPath} ������");
        }
    }


    IEnumerator copyDbFile02(string dbName = "Default.db")
    {
        DebugUtils.Log("copyDbFile02");

        string sourceDBPath = Path.Combine(Application.streamingAssetsPath, dbName);
        string targetDBPath = Path.Combine(Application.persistentDataPath, dbName);

        //connectString = "URI=file:" + targetDBPath;
        _connectString = "Data Source=" + targetDBPath;
        DebugUtils.Log($"�������ݿ� {dbName}  url = {_connectString}");

        if (File.Exists(targetDBPath))
        {
            DebugUtils.Log($"���ݿ�{ dbName }���� �����κβ���");
            yield break;
        }

        UnityWebRequest request = UnityWebRequest.Get(sourceDBPath);
        yield return request.SendWebRequest();
        if (request.isDone)
        {
            byte[] data = request.downloadHandler.data;
            File.WriteAllBytes(targetDBPath, data);
            DebugUtils.Log($"�������ݿ�{sourceDBPath}  �����أ� {targetDBPath}");
        }

        request.Dispose();
    }


    public void OpenDB(Action<SqliteConnection> callBack)
    {
        if (string.IsNullOrEmpty(curDBName))
        {
            DebugUtils.LogError("curDBName is null");
            return;
        }
        OpenDB(curDBName, callBack);
    }

    //public void OpenDB(string dbName = "Default.db", Action<SqliteConnection> callBack = null)
    public void OpenDB(string dbName, Action<SqliteConnection> callBack = null)
    {
        IEnumerator task = _OpenDB(dbName, callBack);

        if (_cor != null)
        {
            taskLst.Add(task);
            return;
        }

        _cor = StartCoroutine(task);
    }


    protected override void OnDestroy()
    {

        ClearCor();
        CloseDB();
        taskLst.Clear();
        base.OnDestroy();
    }

    public void CloseDB()
    {
        if (_connection != null && _connection.State == System.Data.ConnectionState.Open)
        {
            _connection.Close();
        }
        _connection = null;
    }




    public SqliteDataReader ExecuteQuery(string sql, string dbName = null)
    {
        sql = sql.Trim();
        if (_connection == null)
        {
            DebugUtils.LogError("connection is null");
            return null;
        }
        else if (dbName != null && dbName != curDBName)
        {
            DebugUtils.LogError($"dbName:{dbName} is not cur DB;  cur DB��{curDBName}");
            return null;
        }
        else if (sql.StartsWith("CREATE TABLE")
            || sql.StartsWith("INSERT INTO")
            || sql.StartsWith("UPDATE")
            || sql.StartsWith("DELETE")
            || sql.StartsWith("DROP"))
        {
            DebugUtils.LogError($"�������롢���¡�ɾ�� ��ҵ��ʹ�� api: ExecuteNonQuery");
            return null;
        }

        _command = _connection.CreateCommand();
        _command.CommandText = sql;
        SqliteDataReader dataReader = _command.ExecuteReader();
        return dataReader;
    }
    public void ExecuteQueryAfterOpenDB(string dbName, string sql, Action<SqliteDataReader> callBack)
    {
        OpenDB(dbName, (connection) =>
        {
            callBack?.Invoke(ExecuteQuery(sql, dbName));
        });
    }
    public void ExecuteQueryAfterOpenDB(string sql, Action<SqliteDataReader> callBack)=> ExecuteQueryAfterOpenDB(curDBName, sql, callBack);
    




    /// <summary>
    /// * ������ˢ�±�ɾ��������Ȳ���
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="dbName"></param>
    public void ExecuteNonQuery(string sql, string dbName = null)
    {
        sql = sql.Trim();
        if (_connection == null)
        {
            DebugUtils.LogError("connection is null");
            return;
        }
        else if (dbName != null && dbName != curDBName)
        {
            DebugUtils.LogError($"dbName:{dbName} is not cur DB;  cur DB��{curDBName}");
            return;
        }
        else if (sql.StartsWith("SELECT"))
        {
            DebugUtils.LogError($"��Ѱҵ��ʹ�� api: ExecuteQuery");
            return;
        }

        DebugUtils.Log($"sql = {sql}");

        using (SqliteCommand cmd = new SqliteCommand(sql, _connection))
        {
            cmd.ExecuteNonQuery();
            /*
            if (sql.Contains("CREATE TABLE"))
                DebugUtil.Log("������ɹ�.");
            else if (sql.Contains("INSERT INTO"))
                DebugUtil.Log("���ݲ���ɹ�.");
            else if (sql.Contains("UPDATE"))
                DebugUtil.Log("���ݸ��³ɹ�.");
            else if (sql.Contains("DELETE") || sql.Contains("DROP"))
                DebugUtil.Log("����ɾ���ɹ�.");
            */
        }
    }
    public void ExecuteNonQueryAfterOpenDB(string dbName, string sql)
    {
        OpenDB(dbName, (connection) =>
        {
            ExecuteNonQuery(sql, dbName);
        });
    }
    public void ExecuteNonQueryAfterOpenDB(string sql) => ExecuteNonQueryAfterOpenDB(curDBName, sql);






    /// <summary>
    /// ���ݿ�����תjson
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="dbName"></param>
    /// <returns></returns>
    public string ConvertSqliteToJson(string sql, string dbName = null)
    {
        if (_connection == null)
        {
            DebugUtils.LogError("connection is null");
            return null;
        }
        else if (dbName != null && dbName != curDBName)
        {
            DebugUtils.LogError($"dbName:{dbName} is not cur DB;  cur DB��{curDBName}");
            return null;
        }
        else if (!sql.Contains("SELECT"))
        {
            DebugUtils.LogError($"������ SELECT ҵ��");
            return null;
        }

        var dataTable = new DataTable();
        using (SqliteCommand cmd = new SqliteCommand(sql, _connection))
        {
            using (var reader = cmd.ExecuteReader())
            {
                dataTable.Load(reader);
            }
        }
        return JsonConvert.SerializeObject(dataTable, Formatting.Indented);
    }

    public void ConvertSqliteToJsonAfterOpenDB(string dbName, string sql, Action<string> callBack)
    {
        OpenDB(dbName, (connection) =>
        {
            /*
            var dataTable = new DataTable();
            using (SqliteCommand cmd = new SqliteCommand(sql, connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    dataTable.Load(reader);
                }
            }
            callBack.Invoke( JsonConvert.SerializeObject(dataTable, Formatting.Indented));
            */
            callBack.Invoke(ConvertSqliteToJson(sql, dbName));
        });
    }
    public void ConvertSqliteToJsonAfterOpenDB(string sql, Action<string> callBack) => ConvertSqliteToJsonAfterOpenDB(curDBName, sql, callBack);

}




public partial class SQLiteHelper : MonoSingleton<SQLiteHelper>
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
            }else if (columnType[i] == TYPE_FLOAT)
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
            }else if (finfo.FieldType == typeof(int) || finfo.FieldType == typeof(long))
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




    /// <summary>
    /// "INSERT INTO test_student_info (name, numb, age, height) VALUES ('С��', 1, 12, 15.9)";
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="keys"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public static string SQLInsertTableData(string tableName, Dictionary<string,object> dicKeyValue)
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

            if (finfo.Name == "id") {
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

        for (int i = 0; i < keys.Count; i ++)
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



public partial class SQLiteHelper : MonoSingleton<SQLiteHelper>
{


    public bool CheckTableExists(string tableName, string dbName = null)
    {
        if (_connection == null)
        {
            DebugUtils.LogError("connection is null");
            return false;
        }
        else if (dbName != null && dbName != curDBName)
        {
            DebugUtils.LogError($"dbName:{dbName} is not cur DB;  cur DB��{curDBName}");
            return false;
        }

        string sql = $"SELECT COUNT(*) FROM sqlite_master WHERE type ='table' and name='{tableName}';";

        //��������
        _command = _connection.CreateCommand();
        _command.CommandText = sql;
        int count = Convert.ToInt32(_command.ExecuteScalar());

        //������Ϊ1���ʾ���ڸñ��
        bool isExists = count == 1;

        return isExists;
    }

    /// <summary>
    /// ��ȡ�������
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    public long GetTableRowCount(string tableName)
    {
        long rowCount = 0;

        string sql = $"SELECT COUNT(*) FROM {tableName}";

        using (var command = new SqliteCommand(sql, _connection))
        {
            object res = command.ExecuteScalar();
            DebugUtils.Log($"res = {res}");
            rowCount = (long)res;
            DebugUtils.Log($"Table '{tableName}' has {rowCount} rows.");
        }
        return rowCount;
    }

    /// <summary>
    /// �������
    /// </summary>
    [Button]
    void TestCreatTable( )
    {
        SQLiteHelper.Instance.OpenDB(ApplicationSettings.Instance.dbName,  (connect) =>
        {
            if (!SQLiteHelper.Instance.CheckTableExists("test_student_info"))
            {
                string sql = "CREATE TABLE test_student_info(id INTEGER PRIMARY KEY, numb INTEGER, name TEXT, age INTEGER, height REAL, created_at INTEGER)";
                SQLiteHelper.Instance.ExecuteNonQuery(sql);
            }
        });
    }

    /// <summary>
    /// ˢ������
    /// </summary>
    [Button]
    void TestUpdateTable()
    {
        string sql = "UPDATE test_student_info SET numb = 4, height = 159 WHERE name = 'С��33'";
        SQLiteHelper.Instance.ExecuteNonQueryAfterOpenDB(ApplicationSettings.Instance.dbName, sql);
    }

    /// <summary>
    /// ��������
    /// </summary>
    [Button]
    void TestInsertTable(string name="С��33", int numb = 2, int age = 10 ,float height = 158.5f)
    {
        //string sql = "INSERT INTO test_student_info (name, numb, age, height) VALUES ('С��2', 2, 10, 158.5)";
        string sql = "INSERT INTO test_student_info (name, numb, age, height, created_at) VALUES ('@name', @numb, @age, @height, @created_at)";

        long tim = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        sql = sql.Replace("@name", name)
             .Replace("@numb", numb.ToString())
             .Replace("@age", age.ToString())
             .Replace("@height", height.ToString())
             .Replace("@created_at", tim.ToString());


        DebugUtils.Log($"sql = {sql}");
        SQLiteHelper.Instance.ExecuteNonQueryAfterOpenDB(ApplicationSettings.Instance.dbName, sql);

    }
    [Button]
    void TestInsertTable02(string name = "С��33", int numb = 2, int age = 10, float height = 158.5f)
    {
        long tim = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        string sql = @"INSERT INTO test_student_info 
                    (name, numb, age, height, created_at)
                    VALUES (:name, :numb, :age, :height, :created_at)";

        using (var command = new SqliteCommand(sql, _connection))
        {

            // �����������������ֵ
            command.Parameters.AddWithValue(":name", name);
            command.Parameters.AddWithValue(":numb", numb);
            command.Parameters.AddWithValue(":age", age);
            command.Parameters.AddWithValue(":height", height);
            command.Parameters.AddWithValue(":created_at", tim);

            // ִ������
            command.ExecuteNonQuery();




            // ��ȡ����ʾ��
            sql = "SELECT * FROM test_student_info WHERE name = :name";
            using (var command1 = new SqliteCommand(sql, _connection))
            {
                command1.Parameters.AddWithValue(":name", name);

                using (var reader = command1.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DebugUtils.Log($"ID: {reader.GetInt32(0)}, Numb: {reader.GetInt32(1)}, Name: {reader.GetString(2)}");
                    }
                }
            }



        }





    }

    /// <summary>
    /// ��Ѱ����
    /// </summary>
    [Button]
    void TestSELECTName(string name = "С��33")
    {
        /*string sql = "SELECT* FROM test_student_info WHERE name = 'С��33' AND age = 10";*/
        string sql = "SELECT* FROM test_student_info WHERE name = '@name'";
        sql = sql.Replace("@name", name);
        SQLiteHelper.Instance.ExecuteQueryAfterOpenDB(ApplicationSettings.Instance.dbName, sql, (reader) =>
        {
            while (reader.Read())
            {
                //string textData = reader.GetString(reader.GetOrdinal("height"));
                float height = reader.GetFloat(reader.GetOrdinal("height"));
                DebugUtils.Log($"{name}����� = {height}");
            }
        });

    }


    /// <summary>
    /// �������á���ձ��е����ݵ�������ṹ
    /// </summary>
    [Button]
    void TestClearTable()
    {
        /* TRUNCATE TABLE ����� SQLite �в�֧��
        string sql = "TRUNCATE TABLE test_student_info";*/

        string sql = "DELETE FROM test_student_info;";
        DebugUtils.Log($"sql = {sql}");
        SQLiteHelper.Instance.ExecuteNonQueryAfterOpenDB(ApplicationSettings.Instance.dbName, sql);

    }


    /// <summary>
    /// �������á�ɾ����
    /// </summary>
    [Button]
    void TestDeleteTable()
    {
        string sql = "DROP TABLE test_student_info;";
        DebugUtils.Log($"sql = {sql}");
        SQLiteHelper.Instance.ExecuteNonQueryAfterOpenDB(ApplicationSettings.Instance.dbName, sql);
    }


    /// <summary>
    /// ɾ��������
    /// </summary>
    [Button]
    void TestDeleteData(string name = "С��33")
    {
        string sql = "DELETE FROM test_student_info WHERE name = '@name'";
        sql = sql.Replace("@name", name);
        DebugUtils.Log($"sql = {sql}");
        SQLiteHelper.Instance.ExecuteNonQueryAfterOpenDB(ApplicationSettings.Instance.dbName, sql);
    }



    /// <summary>
    /// �Ƿ���ڱ�
    /// </summary>
    /// <param name="tableName"></param>
    [Button]
    void TestIsTable(string tableName = "test_student_info")
    {
        SQLiteHelper.Instance.OpenDB(ApplicationSettings.Instance.dbName, (connect) =>
        {
            if (!SQLiteHelper.Instance.CheckTableExists(tableName))
            {
                DebugUtils.Log($"�����ڱ� {tableName} ");
            }
            else
            {
                DebugUtils.Log($"���ڱ� {tableName} ");
            }
        });
    }


    /// <summary>
    /// �鿴�����������
    /// </summary>
    /// <param name="tableName"></param>
    [Button]
    void TestGetTableRowCount(string tableName = "slot_game_record")
    {
        SQLiteHelper.Instance.OpenDB(ApplicationSettings.Instance.dbName, (connection) =>
        {
            if (SQLiteHelper.Instance.CheckTableExists(tableName))
            {
                string sql = $"SELECT COUNT(*) FROM {tableName}";
                DebugUtils.Log(sql);
                using (var command = new SqliteCommand(sql, connection))
                {
                    object res = command.ExecuteScalar();
                    DebugUtils.Log($"res = {res}");
                    long  rowCount = (long)res;
                    DebugUtils.Log($"Table '{tableName}' has {rowCount} rows.");
                }
            }
            else
            {
                DebugUtils.Log($"�����ڱ� {tableName} ");
            }
        });
    }


    /// <summary>
    /// ������תjson�ַ���
    /// </summary>
    void TestTable2Json()
    {
        OpenDB((connection) =>
        {
            string sql = "SELECT* FROM test_student_info WHERE name = 'С��'";
            var dataTable = new DataTable();
            using (SqliteCommand cmd = new SqliteCommand(sql, connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    dataTable.Load(reader);
                }
            }

            DebugUtils.Log($" data to json :{JsonConvert.SerializeObject(dataTable, Formatting.Indented)}");
        });
    }


    /// <summary>
    /// �������µ�n������
    /// </summary>
    /// <param name="remainRow"></param>
    [Button]
    void TestDeleteTableRemainRow(int remainRow = 3)
    {

        //"DELETE FROM your_table WHERE created_at NOT IN (  SELECT created_at FROM (  SELECT created_at FROM your_table ORDER BY created_at DESC LIMIT 10 ))";

        string tableName = "test_student_info";
        string rowName = "created_at";
        string sql = $"DELETE FROM {tableName} WHERE {rowName} NOT IN (  SELECT {rowName} FROM (  SELECT {rowName} FROM {tableName} ORDER BY {rowName} DESC LIMIT {remainRow} ))";

        DebugUtils.Log($"sql = {sql}");
        SQLiteHelper.Instance.ExecuteNonQueryAfterOpenDB(ApplicationSettings.Instance.dbName, sql);

    }
}


