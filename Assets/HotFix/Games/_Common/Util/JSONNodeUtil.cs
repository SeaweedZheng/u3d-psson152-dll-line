using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using UnityEngine;
using GameMaker;
public enum CompareMethod
{
    EqualTo,
    GreaterThan,
    LessThan,
    GreaterOrEqualTo,
    LessOrEqualTo
}
public partial class JSONNodeUtil
{

    private static JSONNode GetTargetNode(JSONNode node, string keyPath = "kkk/aaa/__0/xxxx/__7/88")
    {
        string[] itemsStrs = keyPath.Split('/') ?? new string[] { };

        if (itemsStrs.Length == 0)
            return null;

        JSONNode target = node;
        foreach (string itemStr in itemsStrs)
        {
            bool isIndex = false;
            int index = -1;
            if (itemStr.StartsWith("__"))
            {
                try
                {
                    //int.TryParse(itemStr, out index);
                    index = int.Parse(itemStr.Replace("__", ""));
                    isIndex = index >= 0;
                }
                catch
                {
                    isIndex = false;
                    index = -1;
                }
            }
            if (isIndex)
            {
                if (index < target.Count)
                    target = target[index];
                else
                    return null;
            }
            else if (target.HasKey(itemStr))
            {
                target = target[itemStr];
            }
            else
            {
                return null;
            }
        }
        return target;
    }



    public static string SetOrCreatKeyValue(string jsonStr, string keyPath, object value)
    {

        JSONNode node = JSONNode.Parse(jsonStr);
        string[] itemsStrs = keyPath.Split('/') ?? new string[] { };

        JSONNode temp = node;
        for (int i = 0; i < itemsStrs.Length; i++)
        {
            string nd = itemsStrs[i];
            if (i == itemsStrs.Length - 1)
            {
                JSONNode t = JSONNode.Parse(string.Format("{{\"value\":{0}}}", value));
                temp.Add(nd, t["value"]);
            }
            else
            {
                if (!temp.HasKey(nd))
                {
                    temp.Add(nd, JSONNode.Parse("{}"));
                }
                temp = temp[nd];
            }
        }
        return node.ToString();
    }
    public static void SetOrCreatKeyValue(JSONNode node, string keyPath, object value)
    {
        string[] itemsStrs = keyPath.Split('/') ?? new string[] { };

        JSONNode temp = node;
        for (int i = 0; i < itemsStrs.Length; i++)
        {
            string nd = itemsStrs[i];
            if (i == itemsStrs.Length - 1)
            {
                JSONNode t = JSONNode.Parse(string.Format("{{\"value\":{0}}}", value));
                temp.Add(nd, t["value"]);
            }
            else
            {
                if (!temp.HasKey(nd))
                {
                    temp.Add(nd, JSONNode.Parse("{}"));
                }
                temp = temp[nd];
            }
        }
    }




    public static void SetOrCreatListValue(JSONNode node, string keyPath, object value)
    {
        string[] itemsStrs = keyPath.Split('/') ?? new string[] { };

        JSONNode temp = node;
        for (int i = 0; i < itemsStrs.Length; i++)
        {
            string nd = itemsStrs[i];
            if (i == itemsStrs.Length - 1)
            {
                /* 可以不加！
                 if (!temp.HasKey(nd))
                {
                    temp.Add(nd, JSONNode.Parse("[]"));
                }*/
                temp[nd].Clear();
                JSONNode t = JSONNode.Parse(string.Format("{{\"value\":{0}}}", value));
                temp[nd].Add(t["value"]);
            }
            else
            {
                if (!temp.HasKey(nd))
                {
                    temp.Add(nd, JSONNode.Parse("{}"));
                }
                temp = temp[nd];
            }
        }
    }


    public static void SetOrCreatListValues(JSONNode node, string keyPath, List<object> values)
    {
        string[] itemsStrs = keyPath.Split('/') ?? new string[] { };

        JSONNode temp = node;
        for (int i = 0; i < itemsStrs.Length; i++)
        {
            string nd = itemsStrs[i];
            if (i == itemsStrs.Length - 1)
            {
                /* 可以不加！
                 if (!temp.HasKey(nd))
                {
                    temp.Add(nd, JSONNode.Parse("[]"));
                }*/
                temp[nd].Clear();
                foreach (object item in values)
                {
                    JSONNode t = JSONNode.Parse(string.Format("{{\"value\":{0}}}", item));
                    temp[nd].Add(t["value"]);
                }
            }
            else
            {
                if (!temp.HasKey(nd))
                {
                    temp.Add(nd, JSONNode.Parse("{}"));
                }
                temp = temp[nd];
            }
        }
    }

    public static JSONNode GetOrCreatValue(JSONNode node, string keyPath, object value)
    {
        JSONNode nd = GetValue(node, keyPath);
        if (nd == null)
        {
            SetOrCreatKeyValue(node, keyPath, value);
        }
        nd = GetValue(node, keyPath);
        return nd;
    }


    public static void AddOrCreatListValues(JSONNode node, string keyPath, List<object> values)
    {
        string[] itemsStrs = keyPath.Split('/') ?? new string[] { };

        JSONNode temp = node;
        for (int i = 0; i < itemsStrs.Length; i++)
        {
            string nd = itemsStrs[i];
            if (i == itemsStrs.Length - 1)
            {
                /* 可以不加！
                 if (!temp.HasKey(nd))
                {
                    temp.Add(nd, JSONNode.Parse("[]"));
                }*/
                foreach (object item in values)
                {
                    JSONNode t = JSONNode.Parse(string.Format("{{\"value\":{0}}}", item));
                    temp[nd].Add(t["value"]);
                }
            }
            else
            {
                if (!temp.HasKey(nd))
                {
                    temp.Add(nd, JSONNode.Parse("{}"));
                }
                temp = temp[nd];
            }
        }
    }


    public static List<T> GetList<T>(JSONNode node, string keyPath)
    {
        List<T> lst = new List<T>();
        JSONNode _hitNumbsNode = JSONNodeUtil.GetValue(node, keyPath);
        foreach (JSONNode item in _hitNumbsNode)
        {
            T obj = ToType<T>(item);
            lst.Add(obj);
        }
        return lst;
    }

    public static T ToType<T>(JSONNode node)
    {
        object obj = null;

        if (typeof(T) == typeof(int))
        {
            obj = (int)node;
        }
        else if (typeof(T) == typeof(long))
        {
            obj = (long)node;
        }
        else if (typeof(T) == typeof(float))
        {
            obj = (float)node;
        }
        else if (typeof(T) == typeof(double))
        {
            obj = (double)node;
        }
        else if (typeof(T) == typeof(string))
        {
            obj = (double)node;
        }
        else if (typeof(T) == typeof(bool))
        {
            obj = (bool)node;
        }
        else if (typeof(T) == typeof(object))
        {
            obj = (object)node;
        }
        else if (typeof(T).IsClass)
        {
            obj = JsonUtility.FromJson<T>(node.ToString());
        }
        else
        {
            DebugUtils.LogError($"can nor to type : {typeof(T)}  data : {node.ToString()}");
        }
        return (T)obj;
    }

    public static JSONNode GetValue(JSONNode node, string keyPath)
    {
        /*
        string[] itemsStrs = keyPath.Split('/') ?? new string[] { };
        if (itemsStrs.Length == 0)
            return false;
        
        JSONNode target = node;
        foreach (string itemStr in itemsStrs)
        {
            if (target.HasKey(itemStr))
            {
                target = target[itemStr];
            }
            else
            {
                return null;
            }
        }*/
        JSONNode target = GetTargetNode(node, keyPath);
        return target;
    }

    public static bool CheckString(string jsonStr, string keyPath, string value)
    {

        JSONNode node = JSONNode.Parse(jsonStr);

        string[] itemsStrs = keyPath.Split('/') ?? new string[] { };

        if (itemsStrs.Length == 0)
            return false;

        JSONNode target = node;
        foreach (string itemStr in itemsStrs)
        {
            if (target.HasKey(itemStr))
            {
                target = target[itemStr];
            }
            else
            {
                return false;
            }
        }
        string resStr = (string)target;
        bool res = string.Equals(resStr, value);
        return res;
    }



    public static bool CheckInt32(string jsonStr, string keyPath, int value, CompareMethod checkType)
    {

        JSONNode node = JSONNode.Parse(jsonStr);

        string[] itemsStrs = keyPath.Split('/') ?? new string[] { };

        if (itemsStrs.Length == 0)
            return false;

        JSONNode target = node;
        foreach (string itemStr in itemsStrs)
        {
            if (target.HasKey(itemStr))
            {
                target = target[itemStr];
            }
            else
            {
                return false;
            }
        }

        bool res = false;
        switch (checkType)
        {
            case CompareMethod.EqualTo:
                res = (int)target == value;
                break;
            case CompareMethod.GreaterThan:
                res = (int)target > value;
                break;
            case CompareMethod.LessThan:
                res = (int)target < value;
                break;
            case CompareMethod.GreaterOrEqualTo:
                res = (int)target >= value;
                break;
            case CompareMethod.LessOrEqualTo:
                res = (int)target <= value;
                break;
        }
        return res;
    }


    public static bool CheckBool(string jsonStr, string keyPath, bool value)
    {

        JSONNode node = JSONNode.Parse(jsonStr);

        string[] itemsStrs = keyPath.Split('/') ?? new string[] { };

        if (itemsStrs.Length == 0)
            return false;

        JSONNode target = node;
        foreach (string itemStr in itemsStrs)
        {
            if (target.HasKey(itemStr))
            {
                target = target[itemStr];
            }
            else
            {
                return false;
            }
        }

        var res = (bool)target == value;

        return res;
    }



    public static bool HasKey(string jsonStr, string keyPath)
    {
        return HaskKey(JSONNode.Parse(jsonStr), keyPath);

        JSONNode node = JSONNode.Parse(jsonStr);

        string[] itemsStrs = keyPath.Split('/') ?? new string[] { };

        if (itemsStrs.Length == 0)
            return false;

        JSONNode target = node;
        for (int i = 0; i < itemsStrs.Length; i++)
        {
            string itemStr = itemsStrs[i];
            if (target.HasKey(itemStr))
            {

                target = target[itemStr];
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    public static bool HaskKey(JSONNode node, string keyPath)
    {
        JSONNode target = GetTargetNode(node, keyPath);
        return target != null;
    }

    public void ForeachJSONNodeList(JSONNode node, Action<JSONNode> cb)
    {
        /*
        for (int j=0; j< node.Count; j++) // 可以用
        {
            JSONNode nod = node[j];
            cb(nod);
        }*/

        foreach (JSONNode nod in node) // 可以用
        {
            cb(nod);
        }
    }

    public void ForeachJSONNodeKeyValue(JSONNode node, Action<string, JSONNode> cb)
    {
        foreach (KeyValuePair<string, JSONNode> kv in node)
        {
            cb(kv.Key, kv.Value);
        }
    }



    public object GetJSONNodeValueAs<T>(JSONNode node, string keyPath)
    {
        string[] itemsStrs = keyPath.Split('/') ?? new string[] { };

        if (itemsStrs.Length == 0)
            return null;

        JSONNode target = node;
        foreach (string itemStr in itemsStrs)
        {
            if (target.HasKey(itemStr))
            {
                target = target[itemStr];
            }
            else
            {
                return null;
            }
        }

        /*
        if (typeof(T) == typeof(int))
        {
            return (int)target;
        }
        else if (typeof(T) == typeof(long))
        {
            return (long)target;
        }
        else if (typeof(T) == typeof(string))
        {
            return (string)target;
        }
        else if (typeof(T) == typeof(bool))
        {
            return (bool)target;
        }
        else if (typeof(T) == typeof(float))
        {
            return (float)target;
        }

        return target;
        */


        T obj = ToType<T>(target);
        return obj;
    }
}


public partial class JSONNodeUtil
{


    private static bool IsList(Type type)
    {
        // 检查类型是否实现了IList<T>接口，并且具有泛型定义  
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
    }

    private static bool IsDictionary(Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>);
    }


    static object _GetData01(object obj)
    {
        JSONNode node = null;
        if (obj == null)
        {
            return null;
        }
        else if (IsList(obj.GetType()))
        {
            node = JSONNode.Parse("[]");
            for (int i = 0; i < ((IList)obj).Count; i++)
            {
                //((IList)obj)[i].ToString() + ",";
                node.Add((JSONNode)_GetData01(((IList)obj)[i]));
            }
        }
        else if (IsDictionary(obj.GetType()))
        {
            node = JSONNode.Parse("{}");
            foreach (DictionaryEntry unit in ((IDictionary)obj))
            {
                //unit.Key + ":" + unit.Value + ",";
                node.Add($"{unit.Key}", (JSONNode)_GetData01(unit.Value));
            }
        }
        else
        {
            if (obj != null && !(obj is string) && obj.GetType().IsClass)
            {
                /* DebugUtil.LogWarning($"报错！data是类的对象 : {obj.GetType().Name}");
                 string str = JsonUtility.ToJson(obj);
                 return JSONNode.Parse(str);*/

                node = JSONNode.Parse("{}");
                Type type = obj.GetType();
                // 获取类的所有字段（包括私有字段可以使用 BindingFlags.NonPublic）  
                FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                // 也可以使用GetProperties获取属性  
                //PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                foreach (var field in fields)
                {
                    object nextObj = field.GetValue(obj);
                    //nextObjType = field.FieldType;
                    string nextObjName = field.Name;

                    node.Add(nextObjName, (JSONNode)_GetData01(nextObj));

                   
                    /*try
                    {
                        node.Add(nextObjName, (JSONNode)_GetData01(nextObj));
                    }
                    catch (Exception ex)
                    {
                        DebugUtil.LogError($"报错日志 : {nextObjName}");
                        DebugUtil.LogException(ex);
                    }*/

                }

                return node;
            }

            object _res = obj is string ? $"\"{obj}\"" : obj;
            //object _res = obj is string ? obj.ToString() : obj;

            JSONNode temp = JSONNode.Parse(string.Format("{{\"value\":{0}}}", _res));
            return temp["value"];

        }
        return node;
    }

    public static string LstDicToJsonStr(Dictionary<object, object> obj)
    {
        return ObjectToJsonStr(obj);
    }

    public static string LstDicToJsonStr(List<object> obj)
    {
        return ObjectToJsonStr(obj);
    }


    public static string ObjectToJsonStr(object obj)
    {
        if(obj == null)
            return "null";
        JSONNode temp = (JSONNode)_GetData01(obj);
        return temp.ToString();
    }
}