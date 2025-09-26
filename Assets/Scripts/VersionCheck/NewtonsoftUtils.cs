using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

public static class NewtonsoftUtils 
{

     

    public static Dictionary<string, object> DeserializeObjectToDic(string json)
    {
        JObject jObject = JObject.Parse(json);
        // �� JObject ת��Ϊ Dictionary<string, object>
        Dictionary<string, object> dictionary = JObjectToDictionary(jObject);
        return dictionary;
    }
    public static List<object> DeserializeObjectToArray(string json)
    {
        JArray jArray = JArray.Parse(json);
        // �� JObject ת��Ϊ Dictionary<string, object>
        List<object> lst = JArrayToList(jArray);
        return lst;
    }


    static Dictionary<string, object> JObjectToDictionary(JObject jObject)
    {
        var dictionary = new Dictionary<string, object>();
        foreach (var property in jObject.Properties())
        {
            var key = property.Name;
            var value = property.Value;

            if (value is JObject nestedObject)
            {
                // ���ֵ��Ƕ�׵� JObject���ݹ�ת��
                dictionary[key] = JObjectToDictionary(nestedObject);
            }
            else if (value is JArray jArray)
            {
                // ���ֵ�� JArray������ת��Ϊ��������
                dictionary[key] = JArrayToList(jArray);
            }
            else
            {
                // ��ֵͨ��ֱ��ת��
                dictionary[key] = value.ToObject<object>();
            }
        }
        return dictionary;
    }

    private static List<object> JArrayToList(JArray jArray)
    {
        List<object> array = new List<object>();
        foreach (var item in jArray)
        {
            if (item is JObject nestedInArray)
            {
                // �����е�Ԫ������� JObject���ݹ�ת��
                array.Add(JObjectToDictionary(nestedInArray));
            }
            else if (item is JArray nestedJArray)
            {
                // ����Ƕ�׵� JArray
                array.Add(JArrayToList(nestedJArray));
            }
            else
            {
                array.Add(item.ToObject<object>());
            }
        }
        return array;
    }
    /*
    private static object ProcessJArray(JArray jArray)
    {
        var array = new List<object>();
        foreach (var item in jArray)
        {
            if (item is JObject nestedInArray)
            {
                // �����е�Ԫ������� JObject���ݹ�ת��
                array.Add(JObjectToDictionary(nestedInArray));
            }
            else if (item is JArray nestedJArray)
            {
                // ����Ƕ�׵� JArray
                array.Add(ProcessJArray(nestedJArray));
            }
            else
            {
                array.Add(item.ToObject<object>());
            }
        }
        return array.ToArray();
    }*/





    /// <summary>
    ///  ������ã�������
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"> JArray �� JObject</param>
    /// <param name="path">0/a/b   �� </param>
    /// <returns></returns>
    public static T GerValue<T>(JToken obj, string path = null)
    {
        JToken target = obj;
        if (!string.IsNullOrEmpty(path))
        {
            List<string> lst = new List<string>(path.Split('/'));
            // string key = lst[lst.Count - 1];
            // lst.RemoveAt(lst.Count - 1);
            while (lst.Count > 0)
            {
                string k = lst[0];
                lst.RemoveAt(0);
                if (target is Newtonsoft.Json.Linq.JArray)
                {
                    target = ((IList<JToken>)target)[int.Parse(k)];
                }
                else if (target is Newtonsoft.Json.Linq.JObject)
                {
                    target = ((IDictionary<string, JToken>)target)[k];
                }
            }
        }
        return target.Value<T>();
    }

    public static T GerValueFormObject<T>(string json, string path = null)
    {
        JObject jObject = JObject.Parse(json);
        return GerValue<T>(jObject, path);
    }
    public static T GerValueFormArray<T>(string json, string path = null)
    {
        JArray jArray = JArray.Parse(json);
        return GerValue<T>(jArray, path);
    }





    static string GetDllFileHash(Dictionary<string, object> node, string key)
    {
        if (node.ContainsKey("hotfix_dll") && node["hotfix_dll"] is Newtonsoft.Json.Linq.JObject)
        {
            IDictionary<string, JToken> hotfixDllInfo = node["hotfix_dll"] as IDictionary<string, JToken>;
            if (hotfixDllInfo.ContainsKey(key) && hotfixDllInfo[key] is Newtonsoft.Json.Linq.JObject)
            {
                IDictionary<string, JToken> targetInfo = hotfixDllInfo[key] as IDictionary<string, JToken>;
                JToken jt = targetInfo["hash"];
                return jt.Value<string>();
            }
        }
        return null;
    }


    public static void TestJson()
    {

        /*
         * 
         string json = @"{
            ""name"": ""John Doe"",
            ""age"": 30,
            ""isStudent"": false,
            ""hobbies"": [""reading"", ""swimming"", ""running""]
        }";
*/
        // ����һ�� JSON �ַ���
        string json = @"{
            ""updated_at"": 1740655990369,
            ""hotfix_info_id"": ""xxxxxxx"",
            ""hotfix_version"": ""1.1.1"",
            ""asset_bundle"": {
                ""asset_bundle_dir"": ""./GameRes/"",
                ""manifest"": {
                    ""file"": ""./GameRes/GameRes"",
                    ""hash"": ""xxxxxx1""
                }
            },
            ""hotfix_dll"": {
                ""HotFix"": {
                    ""file"": ""./GameDll/HotFix.dll.bytes"",
                    ""hash"": ""xxxxxx2""
                },
                ""Base"": {
                    ""file"": ""./GameDll/Base.dll.bytes"",
                    ""hash"": ""xxxxxx3""
                },
                ""LuBan"": {
                    ""file"": ""./GameDll/LuBan.dll.bytes"",
                    ""hash"": ""xxxxxx4""
                },
                ""Sqlite"": {
                    ""file"": ""./GameDll/Sqlite.dll.bytes"",
                    ""hash"": ""xxxxxx""
                },
                ""ConstData"": {
                    ""file"": ""./GameDll/ConstData.dll.bytes"",
                    ""hash"": ""xxxxxx5""
                }
            },
            ""hobbies"": [""reading"", ""swimming"", ""running""]
        }";

        string json0 = @"[""reading"", ""swimming"", ""running""]";
        string json1 = "[\"reading\", \"swimming\", \"running\"]";

        try
        {
            // ʹ�� JsonConvert.DeserializeObject ������ JSON �ַ���ת��Ϊ Dictionary<string, object>
            Dictionary<string, object> dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
           
            Debug.Log(" ==0== " + GetDllFileHash(dictionary, "Base"));

            /* �����ֵ䲢�����ֵ��*/
            foreach (KeyValuePair<string, object> pair in dictionary)
            {
                Debug.Log($"Key: {pair.Key}," +
                    $" tp: {pair.Value.GetType()} " +
                    $"- is JObject: {pair.Value is Newtonsoft.Json.Linq.JObject} " +
                    $"- is JArray: {pair.Value is Newtonsoft.Json.Linq.JArray}" +
                    $"- is dic: {pair.Value is Dictionary<string, object>}" +
                    $"- is str: {pair.Value is string}  " +
                    $"- is lst: {pair.Value is List<object>}" +
                    $"- is long: {pair.Value is long} " +
                    $"Value: {pair.Value} ");
            }

            JObject jObject = JObject.Parse(json);
            //JArray jObject1 = JArray.Parse(json);

            Debug.Log(GerValue<string>(jObject, "hobbies/0"));

            Debug.Log(GerValue<string>(jObject, "hotfix_dll/Sqlite/hash"));

            Debug.Log(GerValue<long>(jObject, "updated_at"));


            Dictionary<string, object>  dic = NewtonsoftUtils.DeserializeObjectToDic(json);


            Debug.Log( " -1- " +
              (string) ((dic["asset_bundle"] as Dictionary<string, object>)["manifest"] as Dictionary<string, object>)["file"]
            );
            Debug.Log(" -2- " +
              (string)((dic["hobbies"] as List<object>)[1])
            );


            Debug.Log(NewtonsoftUtils.GerValueFormObject<long>(json, "updated_at"));
            Debug.Log(NewtonsoftUtils.GerValueFormObject<string>(json, "hotfix_dll/Sqlite/hash"));
            Debug.Log(NewtonsoftUtils.GerValueFormArray<string>(json1, "1"));
            Debug.Log(NewtonsoftUtils.GerValueFormArray<string>(json0, "2"));


        }
        catch (JsonException ex)
        {
            // ���� JSON �����쳣
            Debug.Log($"JSON ��������: {ex.Message}");
        }
    }

}
