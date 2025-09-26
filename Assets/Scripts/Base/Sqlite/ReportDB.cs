using Mono.Data.Sqlite;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Data;
using System.Text;
using System;

public class ReportDB
{
    static ReportDB _instance;

    public static ReportDB Instance
    {
        get
        {
            if (_instance == null)
                _instance = new ReportDB();
            return _instance;
        }
    }


    SqliteConnection _dbConnection = null;

    SqliteConnection DBConnection
    {
        get
        {
            if (_dbConnection == null || _dbConnection.State != ConnectionState.Open)
            {
                CloseDB();
                OpenDB();
            }
            return _dbConnection;
        }
    }

    public void CloseDB()
    {
        if (_dbConnection != null && _dbConnection.State != System.Data.ConnectionState.Closed)
        {
            _dbConnection.Close();
        }
        _dbConnection = null;
    }
    void OpenDB()
    {
        string dbName = ApplicationSettings.Instance.dbName;
        string connectString;
        if (Application.isEditor)
        {
            connectString = "URI=file:" + Path.Combine(Application.streamingAssetsPath, dbName);
        }
        else
        {
            string targetDBPath = Path.Combine(Application.persistentDataPath, dbName);
            connectString = "Data Source=" + targetDBPath;
        }
        _dbConnection = new SqliteConnection(connectString);
        _dbConnection.Open();
    }




    public string DoSQLSelect(string sql)
    {
        string result = "";
        try
        {
            StringBuilder resultBuilder = new StringBuilder();
            SqliteDataReader reader = null;

            // ����Ƿ�ΪSELECT��ѯ
            if (sql.TrimStart().StartsWith("SELECT"))
            {
                using (var command = new SqliteCommand(sql, DBConnection))
                {
                    reader = command.ExecuteReader();

                    // �����ѯ���
                    if (reader.HasRows)
                    {
                        resultBuilder.Append("["); // JSON ���鿪ʼ

                        bool firstRow = true;
                        while (reader.Read())
                        {
                            if (!firstRow) resultBuilder.Append(",");
                            firstRow = false;

                            resultBuilder.Append("{"); // JSON ����ʼ

                            // ��ȡ������
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                if (i > 0) resultBuilder.Append(",");

                                string columnName = reader.GetName(i);
                                object value = reader.GetValue(i);

                                // ������ܵ� null ֵ
                                string jsonValue = value == DBNull.Value
                                    ? "null"
                                    : $"\"{EscapeJsonString(value.ToString())}\"";

                                resultBuilder.AppendFormat("\"{0}\":{1}", columnName, jsonValue);
                            }

                            resultBuilder.Append("}"); // JSON �������
                        }

                        resultBuilder.Append("]"); // JSON �������

                        result = resultBuilder.ToString();
                    }
                    else
                    {
                        result = "[]"; // �ս�����ؿ�����
                    } 
                }
            }
            else
            {
                throw new System.Exception($"���������SELECT");
            }
        }
        catch (System.Exception e)
        {
            result = $"{e.Message},  ==== receive sql: {sql}";
            CloseDB();
        }
        return result;
    }

    private string EscapeJsonString(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;

        return input
            .Replace("\\", "\\\\")
            .Replace("\"", "\\\"")
            .Replace("\b", "\\b")
            .Replace("\f", "\\f")
            .Replace("\n", "\\n")
            .Replace("\r", "\\r")
            .Replace("\t", "\\t");
    }


    string DoSQL(string sql)
    {
        string result = "";
        try
        {
            using (var command = new SqliteCommand(sql, DBConnection))
            {
                // ����Ƿ�ΪSELECT��ѯ
                if (sql.TrimStart().StartsWith("SELECT"))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        var results = new StringBuilder();
                        var hasRows = false;

                        // ��ȡ����
                        var columnNames = new List<string>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            columnNames.Add(reader.GetName(i));
                        }
                        results.AppendLine(string.Join("\t", columnNames));

                        // ��ȡ������
                        while (reader.Read())
                        {
                            hasRows = true;
                            var values = new List<string>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                values.Add(reader[i]?.ToString() ?? "NULL");
                            }
                            results.AppendLine(string.Join("\t", values));
                        }

                        if (!hasRows)
                        {
                            result = "��ѯ�ɹ����������Ϊ��";
                        }
                        else
                        {
                            result = results.ToString();
                        }
                    }
                }
                else
                {
                    // ִ�зǲ�ѯ���INSERT��UPDATE��DELETE�ȣ�
                    int rowsAffected = command.ExecuteNonQuery();
                    result = $"����ִ�гɹ�����Ӱ�������: {rowsAffected}";
                }
            }
        }
        catch (System.Exception e)
        {
            result = $"{e.Message},  ==== receive sql: {sql}";
            CloseDB();
        }
        return result;
    }


    public string DoStr2base64str(string str) => Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
    public string Dobase64str2Str(string base64Encoded) => Encoding.UTF8.GetString(Convert.FromBase64String(base64Encoded));
}
