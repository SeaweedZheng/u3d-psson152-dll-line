using UnityEngine;
using Mono.Data.Sqlite;
using System.IO;
using System.Text;
using System;
using System.Collections.Generic;
using System.Collections;
using SimpleJSON;
using Sirenix.OdinInspector;


/// <summary>
/// 【这个版本将弃用】
/// </summary>
/// <remarks>
/// * 在上个版本添加了加密的功能
/// </remarks>
public partial class SQLitePlayerPrefs02 : MonoSingleton<SQLitePlayerPrefs02>
{

    //string filePath = Path.Combine(Application.persistentDataPath, "SQLitePlayerPrefs02.db");
    private static SqliteConnection connection;
    private static SqliteCommand command;
    private static SqliteDataReader dataReader;

    private string connectString;

    private bool _isReady = false;
    public bool isReady
    {
        get
        {
            return _isReady;
        }
    }

    private void Awake()
    {

#if UNITY_EDITOR
        connectString = "URI=file:" + Path.Combine(Application.streamingAssetsPath, "SQLitePlayerPrefs02.db");
        _isReady = true;
        DebugUtils.Log($"db url = {connectString}");
#elif UNITY_ANDROID
        StartCoroutine(copyDbFile());
#endif

    }

    /*IEnumerator copyDbFile()
    {
        string dataSandBoxPath = Application.persistentDataPath + "/SQLitePlayerPrefs02.db";
        if (!Directory.Exists(Application.persistentDataPath))
        {
            Directory.CreateDirectory(Application.persistentDataPath);
        }

        WWW loadWWW = new WWW(Path.Combine(Application.streamingAssetsPath, "SQLitePlayerPrefs02.db"));
        DebugUtil.Log(Path.Combine(Application.streamingAssetsPath, "SQLitePlayerPrefs02.db"));
        yield return loadWWW;
        File.WriteAllBytes(dataSandBoxPath, loadWWW.bytes);
        connectString = "URI=file:" + dataSandBoxPath;
        _isReady = true;
        DebugUtil.Log($"db url = {connectString}");
    }*/

    IEnumerator copyDbFile()
    {
        string dataSandBoxPath = Application.persistentDataPath + "/SQLitePlayerPrefs02.db";
        connectString = "URI=file:" + dataSandBoxPath;
        DebugUtils.Log($"db url = {connectString}");

        if (!Directory.Exists(Application.persistentDataPath))
        {
            Directory.CreateDirectory(Application.persistentDataPath);
        }

        if (!File.Exists(dataSandBoxPath))
        {
            string defaultDbPath = Path.Combine(Application.streamingAssetsPath, "SQLitePlayerPrefs02.db");

            if (File.Exists(defaultDbPath))
            {
                WWW loadWWW = new WWW(defaultDbPath);
                DebugUtils.Log(defaultDbPath);
                yield return loadWWW;
                File.WriteAllBytes(dataSandBoxPath, loadWWW.bytes);
                DebugUtils.Log($"使用默认 Application.streamingAssetsPath 的db库");
            }
        }
        _isReady = true;
    }

    public static bool IsFileExistsInStreamingAssets(string fileName)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        return File.Exists(filePath);
    }






    public SqliteDataReader temp_GetData(string sqlCmd = "SELECT * FROM Customers")
    {
        // 创建连接字符串
        string connectionString = connectString;//$"URI=file:{filePath}";

        // 创建SqliteConnection对象
        using (var connection = new SqliteConnection(connectionString))
        {
            // 打开连接
            connection.Open();

            //执行其他命令

            command = connection.CreateCommand();

            //指定要执行的SQL命令，比如查询、插入、更新或删除等
            command.CommandText = sqlCmd;
            //ExecuteReader用于执行语句并返回结果的方法
            //如果不返回结果可以使用command.ExecuteNonQuery
            dataReader = command.ExecuteReader();


            // 关闭连接
            connection.Close();

            return dataReader;
        }
    }






    /// <summary>
    /// 执行sql命令并返回结果
    /// </summary>
    /// <param name="sql"></param>
    /// <returns></returns>
    private  SqliteDataReader ExcuteSql(string sqlCmd)
    {
        //创建命令
        command = connection.CreateCommand();

        //指定要执行的SQL命令，比如查询、插入、更新或删除等
        command.CommandText = sqlCmd;

        //ExecuteReader用于执行语句并返回结果的方法
        //如果不返回结果可以使用command.ExecuteNonQuery
        dataReader = command.ExecuteReader();

        return dataReader;
    }



    private int ExecuteNonQuery(string sqlQuery)
    {
        using (command = connection.CreateCommand())
        {
            command.CommandText = sqlQuery;
            int rowsAffected = command.ExecuteNonQuery();
            return rowsAffected;
        }
    }



    /// <summary>
    /// 连接数据库
    /// </summary>
    /// <param name="path">数据库路径</param>
    public  void OpenSQLiteFile(string path = null)
    {
        try
        {
            //根据数据库路径连接数据库
            //connection = new SqliteConnection($"URI=file:{path}");
            connection = new SqliteConnection(connectString);

            //打开数据库
            connection.Open();

            DebugUtils.Log("SQLiteFile Open...");
        }
        catch (System.Exception e)
        {
            //捕获异常，如果数据库连接失败则捕获异常
            DebugUtils.LogError(e.Message);
        }
    }


    /// <summary>
    /// 清空表
    /// </summary>
    /// <param name="tableName"></param>
    public void ClearTable(string tableName)
    {
        StringBuilder cmdSrt = new StringBuilder(20);

        cmdSrt.Append($"TRUNCATE TABLE {tableName};");

        ExcuteSql(cmdSrt.ToString());
    }


    /// <summary>
    /// 创建表
    /// sqlCmd:CREATE TABLE tableName (column1 datatype1,column2 datatype2,column3 ///datatype3,.....columnN datatypeN)
    /// </summary>
    /// <param name="tableName"> 表的名称</param>
    /// <param name="columns"></param>
    /// <param name="columnType">类型</param>
    public void CreateTable(string tableName, string[] columns, string[] columnType)
    {
        //如果字段名和字段类型长度不一致则不能创建表格
        if (columns.Length != columnType.Length)
        {
            DebugUtils.LogError("Colum's Length != ColumType's Length");
            return;
        }

        StringBuilder cmdSrt = new StringBuilder(20);

        //根据参数进行创建表格SQL命令字符串拼接
        cmdSrt.Append($"CREATE TABLE {tableName}(");

        for (int i = 0; i < columns.Length; i++)
        {
            if (i > 0)
            {
                cmdSrt.Append(",");
            }

            cmdSrt.Append($"{columns[i]} {columnType[i]}");
        }

        cmdSrt.Append(")");

        //执行命令
        ExcuteSql(cmdSrt.ToString());
    }


    /// <summary>
    /// 检查表是否存在
    /// sqlCmd:SELECT COUNT(*) FROM sqlite_master WHERE type ='table' and name=tableName
    /// </summary>
    public  bool CheckTableExists(string tableName)
    {
        string sql = $"SELECT COUNT(*) FROM sqlite_master WHERE type ='table' and name='{tableName}';";

        //创建命令
        command = connection.CreateCommand();
        command.CommandText = sql;

        //获取返回结果
        int count = Convert.ToInt32(command.ExecuteScalar());

        //如果结果为1则表示存在该表格
        bool isExists = count == 1;

        return isExists;
    }


    /// <summary>
    /// 向表中插入数据
    /// sqlCmd:INSERT INTO tableName VALUES(value1, value2, value3,...valueN)
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="insertDatas"></param>
    public  void InsertTableData(string tableName, string[] insertDatas)
    {
        if (insertDatas.Length == 0)
        {
            DebugUtils.LogError("Values's length == 0");
        }

        StringBuilder cmdSrt = new StringBuilder(20);

        //根据参数进行创建表格SQL命令字符串拼接
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

        //执行插入数据命令
        ExcuteSql(sql);

    }

    /// <summary>
    /// 向表中更新数据
    /// sqlCmd:UPDATE tableName SET column1 = value1, column2 = value2...., columnN = valueN
    /// UPDATE table1 SET Age = 11, WHERE Id = 1;
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="updateDatas"></param>
    public void UpdateTableData(string tableName, string[] updateDatas, string[] where = null)
    {
        if (updateDatas.Length == 0)
            DebugUtils.LogError("Values's length == 0");

        StringBuilder cmdSrt = new StringBuilder(20);

        //根据参数进行创建表格SQL命令字符串拼接
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

        //执行更新数据命令
        ExcuteSql(sql);

    }


    /// <summary>
    /// 获取表中所有的数据 
    /// sqlCmd:SELECT * FROM tableName
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="tableName"></param>
    /// <returns></returns>
    public  List<Dictionary<string, T>> GetTableAllData<T>(string tableName, string[] where = null)
    {
        List<Dictionary<string, T>> dataList = new List<Dictionary<string, T>>(20);

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

        //查询命令
        string sql = where == null ?  $"SELECT * FROM {tableName}" : $"SELECT * FROM {tableName} WHERE {whereStr}";

#if UNITY_EDITOR
        DebugUtils.Log($"sql = {sql}");
#endif

        using (var reader = ExcuteSql(sql))
        {
            //每条读取数据
            while (reader.Read())
            {
                Dictionary<string, T> dataDic = new Dictionary<string, T>(5);

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    //获取名称
                    string key = reader.GetName(i);
                    //获取Value
                    object value = reader.GetValue(i);
                    dataDic.Add(key, (T)value);
                }

                dataList.Add(dataDic);
            }
        }

        return dataList;
    }

}

public partial class SQLitePlayerPrefs02 : MonoSingleton<SQLitePlayerPrefs02>
{

    #region 加密解密
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



    private void _GetPlayerPrefsTab()
    {
        if (connection == null)
        {
            //OpenSQLiteFile(filePath);
            OpenSQLiteFile();
        }

        if (!CheckTableExists("PlayerPrefs"))
        {
            CreateTable("PlayerPrefs", new string[] { "key", "value", "type" , "ciphertext" }, new string[] { "TEXT", "TEXT", "TEXT", "TEXT" });
        }
    }

    void SetData(string key, string value , string type)
    {
        _GetPlayerPrefsTab();

        List<Dictionary<string, object>> data = GetTableAllData<object>("PlayerPrefs", new string[] { "key", $"'{key}'" });

        string ciphertext = GetCiphertext(key, value, type);

        if (data.Count > 0)
        {
            UpdateTableData("PlayerPrefs", new string[] { "value", $"'{value}'", "ciphertext", $"'{ciphertext}'" }, new string[] { "key", $"'{key}'" });
        }
        else
        {
            InsertTableData("PlayerPrefs", new string[] { $"'{key}'", $"'{value}'", $"'{type}'", $"'{ciphertext}'" });
        }
    }


    public object GetData(string key, object defaultValue , string type)
    {
        _GetPlayerPrefsTab();

        List<Dictionary<string, object>> data = GetTableAllData<object>("PlayerPrefs", new string[] { "key", $"'{key}'" });

        if (data.Count > 0)
        {
            string plaintext = GetPlaintext((string)data[0]["ciphertext"]);
            JSONNode node = JSONNode.Parse(plaintext);
            string res = (string)node["value"];
            switch (type)
            {
                case "int":
                    return int.Parse(res);
                case "float": 
                    return float.Parse(res);
                case "string":
                    return res;
                default:
                    DebugUtils.LogError($"数据格式错误 key: {key} type: {type}");
                    return null;
            }
        }
        else
        {
            string ciphertext = GetCiphertext(key, $"{defaultValue}", type);
            InsertTableData("PlayerPrefs", new string[] { $"'{key}'", $"'{defaultValue}'", $"'{type}'", $"'{ciphertext}'" });
            //InsertTableData("PlayerPrefs", new string[] { $"'{key}'", $"'{defaultValue}'", $"'float'" });
            return  defaultValue;
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    /// <remarks>
    /// <para>PlayerPrefs.GetString(string key, string defaultValue)  // 获取不到key，则返回defaultValue； 但是不会同时保存defaultValue</para>
    /// <para>SQLitePlayerPrefs02.GetString(string key, string defaultValue)  // 获取不到key，则返回defaultValue； 同时保存defaultValue</para>
    /// </remarks>
    public string GetString(string key, string defaultValue = null) => (string)GetData(key, defaultValue, "string");

    public void SetString(string key, string value) => SetData(key, value, "string");

    public float GetFloat(string key, float defaultValue) => (float)GetData(key,defaultValue,"float");

    public void SetFloat(string key, float value) => SetData(key,$"{value}", "float");

    public int GetInt(string key, int defaultValue) => (int)GetData(key,defaultValue,"int");

    public void SetInt(string key, int value) => SetData(key, $"{value}", "int");


    [Button]
    public void DeleteKey(string key)
    {
        string sqlQuery = $"DELETE FROM PlayerPrefs WHERE key = '{key}'";
        ExecuteNonQuery(sqlQuery);
    }

   /* public void Update()
    {
        if (Input.GetKeyUp(KeyCode.N) )
        {
            GetString("Test10", "{\"result\":0,\"Bets\":100,\"Wins\":2}");
            GetInt("Test11", 8000001);
            GetFloat("Test12", 1.456f);

            SetString("Test20", "{\"result\":0,\"Bets\":100,\"Wins\":2}");
            SetInt("Test21", 8000001);
            SetFloat("Test22", 1.456f);
        }

        if (Input.GetKeyUp(KeyCode.M))
        {
            float val1 = GetFloat("Test22", 2.0f);
            DebugUtil.Log(val1);
            int val2 = GetInt("Test21", 200);
            DebugUtil.Log(val2);
            string val3 = GetString("Test20", "{}");
            DebugUtil.Log(val3);
            DebugUtil.Log((int)JSONNode.Parse(val3)["Wins"]);
        }
    }*/
}
