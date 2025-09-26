using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System.Linq;

//主工程使用
public class DllHelper
{
    /// <summary> 需要补充元数据 【这个即将弃用】</summary>
    /// <remarks>
    /// * AOTGenericReferences.PatchedAOTAssemblyList 已经记录所有需要补充元数据<br>
    /// </remarks>
    public List<string> AOTMetaAssemblyFiles { get; } = new List<string>()
    {
        /*
        "mscorlib.dll.bytes",
        "System.dll.bytes",
        "System.Core.dll.bytes",
        "UnityEngine.JSONSerializeModule.dll.bytes",
        "UnityEngine.CoreModule.dll.bytes",
        */

		"CSRedisCore.dll.bytes",
        "DOTween.dll.bytes",
        "Newtonsoft.Json.dll.bytes",
        "SelfAOT.dll.bytes",
        "System.Buffers.dll.bytes",
        "System.Core.dll.bytes",
        "System.Memory.dll.bytes",
        "System.Runtime.CompilerServices.Unsafe.dll.bytes",
        "System.dll.bytes",
        "UnityEngine.AndroidJNIModule.dll.bytes",
        "UnityEngine.AssetBundleModule.dll.bytes",
        "UnityEngine.CoreModule.dll.bytes",
        "UnityEngine.JSONSerializeModule.dll.bytes",
        "mscorlib.dll.bytes",
        "zxing.unity.dll.bytes",
    };


    /// <summary> 需要热更的dll (排序先后有要求)</summary>
    public  List<string> DllNameList = new List<string>() { "LuBan", "Base", "HotFix" };



    /// <summary> 
    /// 针对后期热更dll个数变多的情况
    /// </summary>
    public List<string> GetDllNameList(JObject node)
    {
        List<string> lst = DllNameList;
        try
        {
            if (node.ContainsKey("hotfix_dll_load_order"))
            {
                JArray arr = node["hotfix_dll_load_order"] as JArray;
                if (arr.Count > 0)
                {
                    lst = new List<string>();
                    foreach (JToken item in arr)
                    {
                        lst.Add(item.ToObject<string>());
                    }
                }
            }

        }
        catch (Exception ex)
        {
            Debug.LogError($"节点hotfix_dll_load_order 解析错误：{node.ToString()}");
        }

        return lst;
    }

    /// <summary>
    /// 通过直接调用接口，补充Aot元数据
    /// </summary>
    public void AddAotMeta()
    {
        /*   */

        Dictionary<int,float> aot1 = new Dictionary<int,float>();  // 这样 Dictionary<int, string>.Keys 还是不能用
        Debug.Log($"@ aot Dictionary<int,float>.Keys {aot1.Keys.Count} {aot1.Keys.Contains(1)}");
        Debug.Log($"@ aot Dictionary<int,float>.Values {aot1.Values.Count} {aot1.Keys.Contains(1)}");


        /*
        Dictionary<int, string> aot2 = new Dictionary<int, string>();
        Debug.Log($"@ aot Dictionary<int,string>.Keys {aot2.Keys.Count}");
        Debug.Log($"@ aot Dictionary<int,string>.Values {aot2.Values.Count}");
        */
    }

    private Dictionary<string, Assembly> DllDic = new Dictionary<string, Assembly>();
    private Assembly hotUpdateAss;
    private static DllHelper _instance;
    public static DllHelper Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new DllHelper();
            }
            return _instance;
        }
    }

    public void SethotUpdateAss(string name, Assembly ass)
    {
        if(!DllDic.ContainsKey(name))
        {
            DllDic.Add(name, ass);
        }
    }

    public Assembly GetAss(string AssName)
    {
        if(DllDic.ContainsKey(AssName))
        {
            return DllDic[AssName];
        }
        return null;
    }

    public MethodInfo GetMethod(string AssName, string className, string methodName)
    {
        if (DllDic.ContainsKey(AssName))
        {
            Type type = DllDic[AssName].GetType(className);
            if (type != null)
                return type.GetMethod(methodName);
            else
                Debug.LogError("错误");
        }
        return null;
    }

    private Type[] GetAllTypes()
    {
        return hotUpdateAss.GetTypes();
    }
}
