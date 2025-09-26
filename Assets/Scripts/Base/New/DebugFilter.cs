using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;


/// <summary>
/// 【这个方法将弃用】
/// </summary>
public class DebugFilter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        logsFilter.Clear();
        logsFilter = GetLogsFilter(GetNodes());
    }


    List<string> logsFilter  = new List<string>();

    public bool IsFilter(string data)
    {
        for (int i=0;i<logsFilter.Count;i++)
        {
            if (data.Contains(logsFilter[i]))
            {
                return true;
            }
        }
        return false;
    }

    const string DEBUG_FILTER_VER = "DEBUG_FILTER_VER";
    
    public JObject GetNodes()
    {
        JObject defaultNodes = JObject.Parse(defaultJsonStr);
        string hash =  ComputeMD5ForStr(defaultNodes.ToString());
        if (!PlayerPrefs.HasKey(DEBUG_FILTER_VER))
        {
            PlayerPrefs.SetString("DEBUG_FILTER_VER", hash);
            PlayerPrefs.SetString("DEBUG_FILTER_DATA", defaultNodes.ToString());
        }
        else
        {
            if(PlayerPrefs.GetString("DEBUG_FILTER_VER", "" ) != hash)
            {
                PlayerPrefs.SetString("DEBUG_FILTER_VER", hash);
                PlayerPrefs.SetString("DEBUG_FILTER_DATA", defaultNodes.ToString());
            }
        }
        string jsonStr = PlayerPrefs.GetString("DEBUG_FILTER_DATA", defaultNodes.ToString());
        return JObject.Parse(jsonStr);
    }

    List<string> GetLogsFilter(JObject data)
    {
        List<string> lst = new List<string>();
        if (data["is_open"].ToObject<bool>() == false)
        {
            lst.Add(data["name"].ToObject<string>());
            return lst;
        }
        if (data.ContainsKey("children"))
        {
            JArray jArray = data["children"]as JArray;
            foreach (JToken child in jArray)
            {
                lst.AddRange(GetLogsFilter(child as JObject));
            }
        }
        return lst;
    }

    public void SetNodes(JObject data)
    {
        logsFilter.Clear();
        logsFilter = GetLogsFilter(data);

        PlayerPrefs.SetString("DEBUG_FILTER_DATA", data.ToString());
    }


    string ComputeMD5ForStr(string rawData)
    {
        // 创建一个MD5实例
        using (MD5 md5 = MD5.Create())
        {
            // 将输入字符串转换为字节数组
            byte[] inputBytes = Encoding.ASCII.GetBytes(rawData);

            // 计算哈希值
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // 将哈希值转换为十六进制字符串
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
            }

            // 返回十六进制字符串
            return sb.ToString();
        }
    }

    string defaultJsonStr = @"
{
    ""name"": ""_ALL"",
    ""is_open"": true,
    ""children"": [
        {
            ""name"": ""【EventData】"",
            ""is_open"": true,
            ""children"": [
                {
                    ""name"": ""ON_PROPERTY_CHANGED_EVENT"",
                    ""is_open"": true,
                    ""children"": []
                },
                {
                    ""name"": ""ON_WIN_EVENT"",
                    ""is_open"": true,
                    ""children"": []
                },
                {
                    ""name"": ""ON_TRIGGER_EVENT"",
                    ""is_open"": true,
                    ""children"": []
                },
                {
                    ""name"": ""ON_SLOT_DETAIL_EVENT"",
                    ""is_open"": true,
                    ""children"": []
                },
                {
                    ""name"": ""ON_PANEL_INPUT_EVENT"",
                    ""is_open"": true,
                    ""children"": []
                },
                {
                    ""name"": ""ON_CREDIT_EVENT"",
                    ""is_open"": true,
                    ""children"": []
                },
                {
                    ""name"": ""ON_SLOT_EVENT"",
                    ""is_open"": true,
                    ""children"": []
                },
                {
                    ""name"": ""MACHINE_CUSTOM_BUTTON_FOCUS_EVENT"",
                    ""is_open"": true,
                    ""children"": []
                },
                {
                    ""name"": ""RESULT_RECALL"",
                    ""is_open"": true,
                    ""children"": []
                }
            ]
        },
        {
            ""name"": ""【SBox】"",
            ""is_open"": true,
            ""children"": [
                {
                    ""name"": ""SBOX_GAME_JACKPOT"",
                    ""is_open"": true,
                    ""children"": []
                },
                {
                    ""name"": ""SBOX_GET_ACCOUNT"",
                    ""is_open"": true,
                    ""children"": []
                },
                {
                    ""name"": ""RESULT_RECALL"",
                    ""is_open"": true,
                    ""children"": []
                },
                {
                    ""name"": ""RpcNameSetPlayerInfo"",
                    ""is_open"": true,
                    ""children"": []
                },
                {
                    ""name"": ""RpcNameIsCodingActive"",
                    ""is_open"": true,
                    ""children"": []
                }
            ]
        }
    ]
}";



}
