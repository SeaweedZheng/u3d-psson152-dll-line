#define SQLITE_ASYNC
using Mono.Data.Sqlite;
using Newtonsoft.Json;
using SimpleJSON;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;


public partial class OrderIdManager : MonoSingleton<OrderIdManager>
{
    /// <summary> orderId 加密秘钥  </summary>
    string _orderIdDecryptKey = null;
    string orderIdDecryptKey{
        get{
            if (_orderIdDecryptKey == null)
                _orderIdDecryptKey = ComputeMD5Hash("i am order id");
            return _orderIdDecryptKey;
        }
   }

    /// <summary> 点单id数据 加密秘钥  </summary>
    string _orderDataDecryptKey = null;
    string orderDataDecryptKey{
        get{
            if (_orderDataDecryptKey == null)
                _orderDataDecryptKey = ComputeMD5Hash("i am order data");
            return _orderDataDecryptKey;
        }
    }


    List <OrderIdInfo> creatIds = new List<OrderIdInfo>();
    class OrderIdInfo
    {
        public string orderId;
        public string type;
        public long overtimeMS;
        public long creatTimeMS;
    }


    /// <summary>
    /// 检查id是否真实
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    public bool CheckOrderIdSign(string orderId)
    {
        if(!orderId.Contains("#"))
            return false;

        string[] itemsStrs = orderId.Split('#');

        string sign = ComputeMD5Hash($"{itemsStrs[0]}{orderIdDecryptKey}");

        return sign == itemsStrs[1];
    }

    /// <summary>
    /// 检查id是否有效可用
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    public void IsValid(string orderId , Action<bool> callback)
    {
        for(int i = 0; i<creatIds.Count; i++)
        {
            if (creatIds[i].orderId == orderId)
            {
                callback(true);
                return;
            }
        }

        // 检查本地缓存
        string hash = ComputeMD5Hash(orderId);

        string sql2 = $"SELECT * FROM {ConsoleTableName.TABLE_ORDER_ID} WHERE hash = '{hash}'"; //可以用
                                                                                                                                                                   //string sql = $"SELECT * FROM {TABLE_COIN_IN_OUT_RECORD} WHERE DATE(created_at) = '{dropdownDateLst[index]}'"; //不可以用
        DebugUtils.Log(sql2);

#if !SQLITE_ASYNC

        SQLiteHelper01.Instance.ExecuteQueryAfterOpenDB(sql2, (SqliteDataReader sdr) =>
        {
            List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();
            while (sdr.Read())
            {
                Dictionary<string, object> row = new Dictionary<string, object>();
                for (int i = 0; i < sdr.FieldCount; i++)
                {
                    row[sdr.GetName(i)] = sdr[i];
                }
                results.Add(row);
            }

            if (results.Count > 0)
            {
                /*foreach (var row in results)
                {
                    foreach (var kvp in row)
                    {
                        DebugUtil.Log($"{kvp.Key}: {kvp.Value}");
                    }
                }*/

                //results[""]
                callback(true);
            }
            else
            {
                callback(false);
            }
        });
#else
        SQLiteAsyncHelper.Instance.ExecuteQueryAsync(sql2, null, (SqliteDataReader sdr) =>
        {
            List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();
            while (sdr.Read())
            {
                Dictionary<string, object> row = new Dictionary<string, object>();
                for (int i = 0; i < sdr.FieldCount; i++)
                {
                    row[sdr.GetName(i)] = sdr[i];
                }
                results.Add(row);
            }

            if (results.Count > 0)
            {
                /*foreach (var row in results)
                {
                    foreach (var kvp in row)
                    {
                        DebugUtil.Log($"{kvp.Key}: {kvp.Value}");
                    }
                }*/

                //results[""]
                callback(true);
            }
            else
            {
                callback(false);
            }
        });
#endif






    }

    /// <summary>
    /// 使用订单号
    /// </summary>
    /// <param name="orderId"></param>
    public bool UseOrderId(string orderId)
    {
        OrderIdInfo target = null;
        for (int i = 0; i < creatIds.Count; i++)
        {
            if (creatIds[i].orderId == orderId)
            {
                target = creatIds[i];
                break;
            }
        }

        if (target == null)
            return false;

        creatIds.Remove(target);

        OrderIdData data = new OrderIdData()
        {
            order_id = target.orderId,
            order_type = target.type,
            state = 1,
            overtime_ms = target.overtimeMS,
            created_at = target.creatTimeMS,
        };
        string strData = JsonConvert.SerializeObject(data);
        TableOrdeId  item = new TableOrdeId()
        {
            hash = ComputeMD5Hash(target.orderId),
            data = StrToBase64str(XOR(strData)),
            created_at = target.creatTimeMS,
        };


#if !SQLITE_ASYNC

        string sql = SQLiteEncryptHelper.SQLInsertTableData<TableOrdeId>(ConsoleTableName.TABLE_ORDER_ID, item);
        SQLiteHelper01.Instance.ExecuteNonQueryAfterOpenDB(sql);
#else
        //string sql = SQLiteAsyncHelper.SQLInsertTableData<TableOrdeId>(ConsoleTableName.TABLE_ORDER_ID, item);

        string sql = SQLiteEncryptHelper.SQLInsertTableData<TableOrdeId>(ConsoleTableName.TABLE_ORDER_ID, item);
        SQLiteAsyncHelper.Instance.ExecuteNonQueryAsync(sql);
#endif

        return true;
    }


    /// <summary>
    /// 订单号对应的订单完成 - 删除订单缓存
    /// </summary>
    /// <param name="orderId"></param>
    public void CompleteOrderId(string orderId)
    {

#if !SQLITE_ASYNC

        string hash = ComputeMD5Hash(orderId);
        string sql = $"DELETE FROM {ConsoleTableName.TABLE_ORDER_ID} WHERE hash = '{hash}'";
        SQLiteHelper01.Instance.ExecuteNonQueryAfterOpenDB(sql);

#else

        string hash = ComputeMD5Hash(orderId);
        string sql = $"DELETE FROM {ConsoleTableName.TABLE_ORDER_ID} WHERE hash = '{hash}'";
        SQLiteAsyncHelper.Instance.ExecuteNonQueryAsync(sql);
#endif

    }

    [Button]
    public void CompleteOrderIdHash(string orderIdHash)
    {


#if !SQLITE_ASYNC
        string sql = $"DELETE FROM {ConsoleTableName.TABLE_ORDER_ID} WHERE hash = '{orderIdHash}'";
        SQLiteHelper01.Instance.ExecuteNonQueryAfterOpenDB(sql);

#else
        string sql = $"DELETE FROM {ConsoleTableName.TABLE_ORDER_ID} WHERE hash = '{orderIdHash}'";
        SQLiteAsyncHelper.Instance.ExecuteNonQueryAsync(sql);
#endif


    }

    [Button]
    public void ShowOrderIdData(string orderId)
    {

#if !SQLITE_ASYNC

        string hash = ComputeMD5Hash(orderId);

        string sql2 = $"SELECT * FROM {ConsoleTableName.TABLE_ORDER_ID} WHERE hash = '{hash}'"; 
                                                                                                
        DebugUtil.Log(sql2);
        SQLiteHelper01.Instance.ExecuteQueryAfterOpenDB(sql2, (SqliteDataReader sdr) =>
        {
            List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();
            while (sdr.Read())
            {
                Dictionary<string, object> row = new Dictionary<string, object>();
                for (int i = 0; i < sdr.FieldCount; i++)
                {
                    row[sdr.GetName(i)] = sdr[i];
                }
                results.Add(row);
            }

            if (results.Count > 0)
            {
                /*foreach (var row in results)
                {
                    foreach (var kvp in row)
                    {
                        DebugUtil.Log($"{kvp.Key}: {kvp.Value}");
                    }
                }*/

                string plaintext = SQLiteEncryptHelper.GetPlaintext((string)results[0]["ciphertext"]);

                DebugUtil.Log($"plaintext = {plaintext}");
                JSONNode node = JSONNode.Parse(plaintext);
                DebugUtil.Log($"data = {XOR(Base64strToStr((string)node["data"]))}");
            }
            else
            {
                DebugUtil.Log($"没有订单:{orderId}的数据");
            }
        });

#else
        string hash = ComputeMD5Hash(orderId);

        string sql2 = $"SELECT * FROM {ConsoleTableName.TABLE_ORDER_ID} WHERE hash = '{hash}'";

        DebugUtils.Log(sql2);
        SQLiteAsyncHelper.Instance.ExecuteQueryAsync(sql2, null, (SqliteDataReader sdr) =>
        {
            List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();
            while (sdr.Read())
            {
                Dictionary<string, object> row = new Dictionary<string, object>();
                for (int i = 0; i < sdr.FieldCount; i++)
                {
                    row[sdr.GetName(i)] = sdr[i];
                }
                results.Add(row);
            }

            if (results.Count > 0)
            {
                /*foreach (var row in results)
                {
                    foreach (var kvp in row)
                    {
                        DebugUtil.Log($"{kvp.Key}: {kvp.Value}");
                    }
                }*/

                string plaintext = SQLiteEncryptHelper.GetPlaintext((string)results[0]["ciphertext"]);

                DebugUtils.Log($"plaintext = {plaintext}");
                JSONNode node = JSONNode.Parse(plaintext);
                DebugUtils.Log($"data = {XOR(Base64strToStr((string)node["data"]))}");
            }
            else
            {
                DebugUtils.Log($"没有订单:{orderId}的数据");
            }
        });
#endif


    }


    public string CreatOrderId(string type, long overtimeMS = -1)
    {

        string orderId = Guid.NewGuid().ToString();

        string sign = ComputeMD5Hash($"{orderId}{orderIdDecryptKey}");

        orderId = $"{orderId}#{sign}";

        creatIds.Add(new OrderIdInfo()
        {
            orderId = orderId,
            type = type,
            creatTimeMS = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            overtimeMS = overtimeMS,
        });

        return orderId;
    }

    [Button]
    public string CreatAndUseOrderId(string type, long overtimeMS = -1)
    {
        string orderId = CreatOrderId(type, overtimeMS);
        DebugUtils.Log($"@@ orderId = {orderId}");
        UseOrderId(orderId);
        return orderId;
    }


    float lastRunTimeS = 0;
    bool isDirty = true;
    public void Update()
    {
        if (isDirty)
        {
            isDirty = false;
            float nowRunTimeS = Time.unscaledTime;
            if (nowRunTimeS - lastRunTimeS > 2) //每2秒检查一次
            {
                lastRunTimeS = nowRunTimeS;
                long nowTimeMS = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                int i = creatIds.Count;
                while (--i >= 0)
                {
                    
                    OrderIdInfo item = creatIds[i];
                    if (item.overtimeMS != -1
                        && nowTimeMS - item.creatTimeMS > item.overtimeMS)
                    {
                        creatIds.Remove(item);
                    }
                }
            }
            isDirty = true;
        }
    }




    string ComputeMD5Hash(string rawData)
    {
        // 将字符串转换为字节数组
        using (MD5 md5Hash = MD5.Create())
        {
            byte[] bytes = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

            // 将字节数组转换为十六进制字符串
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }

    string XOR(string input)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < input.Length; i++)
            sb.Append((char)(input[i] ^ orderDataDecryptKey[(i % orderDataDecryptKey.Length)]));

        String result = sb.ToString();
        return result;
    }


    string StrToBase64str(string originalString) 
    {
        // 将字符串转换为字节数组
        byte[] byteArray = Encoding.UTF8.GetBytes(originalString);
        // 将字节数组转换为Base64字符串
        string base64EncodedString = Convert.ToBase64String(byteArray);
        return base64EncodedString;
    }


    string Base64strToStr(string base64EncodedString)
    {
        byte[] decodedByteArray = Convert.FromBase64String(base64EncodedString);
        string decodedString = Encoding.UTF8.GetString(decodedByteArray);
        return decodedString;
    }

}





public partial class OrderIdManager : MonoSingleton<OrderIdManager>
{
    [Button]
    public  void TestStrCreatTable()
    {
        string sql = SQLiteEncryptHelper.SQLCreateTable<TableOrdeId>(ConsoleTableName.TABLE_ORDER_ID);
        DebugUtils.Log(sql);
    }




    [Button]
    public  void TestCheckOrCreatTable()
    {
        /*
        if (!SQLiteHelper01.Instance.CheckTableExists(ConsoleTableName.TABLE_ORDER_ID))
        {
            string sql = SQLiteEncryptHelper.SQLCreateTable<TableOrdeId>(ConsoleTableName.TABLE_ORDER_ID);
            DebugUtil.Log(sql);
            SQLiteHelper01.Instance.ExecuteNonQuery(sql);
        }*/
    }


    [Button]
    public void TestShowName()
    {
        DebugUtils.Log($"name = {OrderIdManager.Instance.gameObject.name}");
    }




    [Button]
    public  void TestCreatShowId()
    {
        string orderId = OrderIdManager.Instance.CreatOrderId("Test",-1);
        DebugUtils.Log(orderId);
       
        if (OrderIdManager.Instance.UseOrderId(orderId))
        {
            OrderIdManager.Instance.ShowOrderIdData(orderId);
        }
    }


  

}