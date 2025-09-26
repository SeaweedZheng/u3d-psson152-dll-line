using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System;
/*
public static partial class Utils
{
    public static bool ContainKey<T>(List<T> lst , Func<T,bool> funcCondition)
    {
        for (int i = 0; i < lst.Count; i++)
        {
            if (funcCondition(lst[i]))
            {
                return true;
            }
        }
        return false;
    }

    public static T GetValue<T>(List<T> lst, Func<T, bool> funcCondition) where T : class
    {
        for (int i = 0; i < lst.Count; i++)
        {
            if (funcCondition(lst[i]))
            {
                return lst[i];
            }
        }
        return null;
    }

}*/

namespace GameMaker
{
    public static partial class Utils
    {

        public static string ComputeMD5Hash(string rawData)
        {
            // 创建一个MD5实例
            using (MD5 md5 = MD5.Create())
            {
                // 将输入字符串转换为字节数组
                byte[] inputBytes = Encoding.UTF8.GetBytes(rawData);

                // 计算哈希值
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // 将哈希值转换为十六进制字符串
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public static string DoStr2base64str(string str) => Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
        public static string Dobase64str2Str(string base64Encoded) => Encoding.UTF8.GetString(Convert.FromBase64String(base64Encoded));


        public static List<string> GetContentLST(string content)
        {
            List<string> customLST = new List<string>();

            string _agrs = content;
            List<string> xmpContents = new List<string>();
            if (content.Contains("</xmp>"))
            {
                string pattern = @"<xmp>(.*?)<\/xmp>";
                Regex regex = new Regex(pattern, RegexOptions.Singleline);

                MatchCollection matches = regex.Matches(content);

                foreach (Match match in matches)
                {
                    xmpContents.Add(match.Groups[1].Value);
                }

                for (int i = 0; i < xmpContents.Count; i++)
                {
                    _agrs = _agrs.Replace($"<xmp>{xmpContents[i]}</xmp>", $"@#@#{0}");
                }
            }

            string[] itemsStrs = _agrs.Split('#') ?? new string[] { };  //k1:2#k2:3#k3:4
            customLST = new List<string>(itemsStrs);

            for (int i = 0; i < xmpContents.Count; i++)
            {
                string Ta = $"@#@#{i}";
                // reel:<xmp>4,5,6,7,8#1,2,3,4,5#1,1,1,2,2</xmp>#flag1:3
                for (int j = 0; j < customLST.Count; j++)
                {
                    if (customLST[j].Contains(Ta))
                    {
                        customLST[j] = customLST[j].Replace(Ta, xmpContents[i]);
                    }
                }

            }
            return customLST;
        }
        public static Dictionary<string, string>  GetContentKV(string content)
        {
            Dictionary<string, string> customKV = new Dictionary<string, string>();

            string _agrs = content;
            List<string> xmpContents = new List<string>();
            if (content.Contains("</xmp>"))
            {
                string pattern = @"<xmp>(.*?)<\/xmp>";
                Regex regex = new Regex(pattern, RegexOptions.Singleline);

                MatchCollection matches = regex.Matches(content);

                foreach (Match match in matches)
                {
                    xmpContents.Add(match.Groups[1].Value);
                }

                for (int i = 0; i < xmpContents.Count; i++)
                {
                    _agrs = _agrs.Replace($"<xmp>{xmpContents[i]}</xmp>", $"@#@#{0}");
                }
            }

            string[] itemsStrs = _agrs.Split('#') ?? new string[] { };  //k1:2#k2:3#k3:4

            foreach (string item in itemsStrs)
            {
                string[] kv = item.Split(':') ?? new string[] { };
                string key = kv[0];
                string value = kv.Length > 1 ? kv[1] : "";

                if (!customKV.ContainsKey(key))
                {
                    customKV.Add(key, value);
                }
                else
                {
                    customKV[key] = value;
                }
            }

            for (int i = 0; i < xmpContents.Count; i++)
            {
                string Ta = $"@#@#{i}";
                // reel:<xmp>4,5,6,7,8#1,2,3,4,5#1,1,1,2,2</xmp>#flag1:3
                for (int j = 0; j < customKV.Count; j++)
                {
                    var item = customKV.ElementAt(j);
                    if (item.Value.Contains(Ta))
                    {
                        customKV[item.Key] = customKV[item.Key].Replace(Ta, xmpContents[i]);
                    }
                }

            }
            return customKV;
            /*
            string res = "==@ [flags]";
            foreach (KeyValuePair<string, string> item in customKV)
            {
                res += $" {item.Key} : {item.Value};";
            }
            DebugUtil.Log(res);
            */

        }

    }
}



