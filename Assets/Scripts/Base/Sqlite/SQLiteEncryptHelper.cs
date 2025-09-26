using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using System;


/// <summary>
/// 数据库加密
/// </summary>
public static class SQLiteEncryptHelper 
{

    public const string TYPE_INT = "INTEGER";
    public const string TYPE_STRING = "TEXT";
    public const string TYPE_FLOAT = "REAL";
    //布尔值，存 0 或 1 （"INTEGER" 类型）


    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="plaintext"></param>
    /// <returns></returns>
    public static string GetCiphertext(string plaintext)
    {
        string ciphertext = AesManager.Instance.TryLocalEncrypt(plaintext);
        return ciphertext;
    }
    public static string GetCiphertext<T>(T obj)
    {
        Type type = typeof(T);
        FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        Dictionary<string, object> keyValues = new Dictionary<string, object>();
        foreach (FieldInfo finfo in fieldInfos)
        {
            object resObj = finfo.GetValue(obj);

            if (finfo.Name == "id")
                continue;

            keyValues.Add(finfo.Name, resObj);
        }
        string plaintext = JsonConvert.SerializeObject(keyValues);
        Debug.Log($"@@ plaintext = {plaintext}");
        return GetCiphertext(plaintext);
    }

    public static string GetCiphertext(Dictionary<string, object> dicKeyValue)
    {
        string plaintext = JsonConvert.SerializeObject(dicKeyValue);
        Debug.Log($"@@ plaintext = {plaintext}");
        return GetCiphertext(plaintext);
    }

    /// <summary>
    /// 解密
    /// </summary>
    /// <param name="ciphertext"></param>
    /// <returns></returns>
    public static string GetPlaintext(string ciphertext)
    {
        string plaintext = AesManager.Instance.TryLocalDecrypt(ciphertext);
        return plaintext;
    }
    public static T GetPlaintext<T>(string ciphertext)
    {
        string plaintext = GetPlaintext(ciphertext);
        return JsonConvert.DeserializeObject<T>(plaintext);
    }




    /// <summary>
    /// 
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="columns"></param>
    /// <param name="columnType"></param>
    /// <returns></returns>
    public static string SQLCreateTable(string tableName, Dictionary<string, string> dicKeyType)
    {
        List<string> columns = new List<string>(dicKeyType.Keys.ToArray());
        List<string> columnType = new List<string>(dicKeyType.Values.ToArray());

        //columns.Add("ciphertext");
        //columnType.Add(TYPE_STRING);
        if (!columns.Contains("ciphertext"))
        {
            columns.Add("ciphertext");
            columnType.Add(TYPE_STRING);
        }
        else
        {
            int index = columns.IndexOf("ciphertext");
            columnType[index] = TYPE_STRING;
        }


        StringBuilder cmdSrt = new StringBuilder(20);

        //根据参数进行创建表格SQL命令字符串拼接
        //cmdSrt.Append($"CREATE TABLE {tableName}(");
        cmdSrt.Append($"CREATE TABLE {tableName}(id INTEGER PRIMARY KEY,");
        for (int i = 0; i < columns.Count; i++)
        {
            if (i > 0)
            {
                cmdSrt.Append(",");
            }

            cmdSrt.Append($"{columns[i]} {columnType[i]}");
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
                Debug.LogError($" type: {finfo.FieldType} is not allow");
            }
        }

        //columns.Add("ciphertext");
        //columnType.Add(TYPE_STRING);
        if (!columns.Contains("ciphertext"))
        {
            columns.Add("ciphertext");
            columnType.Add(TYPE_STRING);
        }
        else
        {
            int index = columns.IndexOf("ciphertext");
            columnType[index] = TYPE_STRING;
        }


        StringBuilder cmdSrt = new StringBuilder(20);

        //根据参数进行创建表格SQL命令字符串拼接
        //cmdSrt.Append($"CREATE TABLE {tableName}(");
        cmdSrt.Append($"CREATE TABLE {tableName}(id INTEGER PRIMARY KEY,");
        for (int i = 0; i < columns.Count; i++)
        {
            if (i > 0)
            {
                cmdSrt.Append(",");
            }

            cmdSrt.Append($"{columns[i]} {columnType[i]}");
        }

        cmdSrt.Append(")");

        return cmdSrt.ToString();
    }




    /// <summary>
    /// "INSERT INTO test_student_info (name, numb, age, height) VALUES ('小韩', 1, 12, 15.9)";
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="keys"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public static string SQLInsertTableData(string tableName, Dictionary<string, object> dicKeyValue)
    {

        StringBuilder cmdSrt = new StringBuilder(20);

        //根据参数进行创建表格SQL命令字符串拼接
        cmdSrt.Append($"INSERT INTO {tableName}(");

        //string[] keys = dicKeyValue.Keys.ToArray();
        //object[] values = dicKeyValue.Values.ToArray();
        List<string> keys = new List<string>(dicKeyValue.Keys.ToArray());
        List<object> values = new List<object>(dicKeyValue.Values.ToArray());
        string ciphertext = GetCiphertext(dicKeyValue);
        //keys.Add("ciphertext");
        //values.Add(ciphertext);

        if (!keys.Contains("ciphertext"))
        {
            keys.Add("ciphertext");
            values.Add(ciphertext);
        }
        else
        {
            int index = keys.IndexOf("ciphertext");
            values[index] = ciphertext;
        }



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
        string ciphertext = GetCiphertext<T>(obj);

        //keys.Add("ciphertext");
        //values.Add(ciphertext);
        if (!keys.Contains("ciphertext"))
        {
            keys.Add("ciphertext");
            values.Add(ciphertext);
        }
        else
        {
            int index = keys.IndexOf("ciphertext");
            values[index] = ciphertext;
        }



        StringBuilder cmdSrt = new StringBuilder(20);

        //根据参数进行创建表格SQL命令字符串拼接
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

        string ciphertext = GetCiphertext<T>(obj);

        //keys.Add("ciphertext");
        //values.Add(ciphertext);
        if (!keys.Contains("ciphertext"))
        {
            keys.Add("ciphertext");
            values.Add(ciphertext);
        }
        else
        {
            int index = keys.IndexOf("ciphertext");
            values[index] = ciphertext;
        }


        StringBuilder cmdSrt = new StringBuilder(20);

        //根据参数进行创建表格SQL命令字符串拼接
        cmdSrt.Append($"UPDATE {tableName} SET ");

        // "UPDATE test_student_info SET numb = 4, height = 159 WHERE name = '小红33'";

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
