using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class DebugFilterInfo
{
    /// <summary> 日志开头多少个字符</summary>
    public int prefixLength = 12;

    /// <summary> 日子队列保存多少个日子</summary>
    public int prefixCount = 60;

    /// <summary> 日志队列</summary>
    public List<string> queue = new List<string>();
}


/// <summary>
/// 动态过滤
/// </summary>
public partial class DebugFilterDynamics : MonoSingleton<DebugFilterDynamics>
{

    /// <summary>
    /// 日志过滤
    /// </summary>
    /// <remarks>
    /// * 增加三个过滤影响不大。</br>
    /// * 还不如增加条件过滤
    /// </remarks>
   /* public List<DebugFilterInfo> filterLst = new List<DebugFilterInfo>()
   {

       new DebugFilterInfo()  // 【Sbox】
       {
           prefixLength = 10,
       },
       new DebugFilterInfo() // 【Sbox】 rpc 
       {
           prefixLength = 20,
       },
       new DebugFilterInfo() // 【Sbox】 rpc  123
       {
           prefixLength = 30,
       },

   };*/



    public List<DebugFilterInfo> filterLst = new List<DebugFilterInfo>()
   {
       new DebugFilterInfo() // 【Sbox】 rpc  123
       {
           prefixLength = 50,
       },

   };


    Dictionary<string, bool> target = new Dictionary<string, bool>();


    /// <summary> 大于等于多少条开始保存 </summary>
    int numGetTarget = 3;

    public void AnalysisDebug(string log)
    {
        if (!isUseDebugFilter)
            return;

        for (int i =0; i< filterLst.Count; i++)
        {
            var item = filterLst[i];
            string result1 = log.Length >= item.prefixLength ? log.Substring(0, item.prefixLength) : log;

            item.queue.Insert(0, result1);

            if (item.queue.Count > item.prefixCount)
                item.queue.RemoveRange(item.prefixCount, item.queue.Count - item.prefixCount); // 只包里前item.prefixCount 个元素
            
            if (!target.Keys.Contains(result1))
            {
                int count = item.queue.Count(s => s == result1);
                if(count >= numGetTarget)
                    target.Add(result1,true);
            }  
        }
    }



    /// <summary>
    /// 动态 或 固定 的日志过滤器
    /// </summary>
    Dictionary<string, bool> targetFilter
    {
        get
        {
            if (_targetFilter == null)
                _targetFilter = isUseFixedFilter ? fixedFilter : cacheFilter;
            return _targetFilter;
        }
        set
        {
            _targetFilter = value;
        }
    }
    Dictionary<string, bool> _targetFilter;

    public bool IsShowDebug(string log)
    {
        // 开启日志过滤功能？

        if (!isUseDebugFilter)
            return true;

        if (targetFilter["IS_OPEN_ALL"])
            return true;
        else if (targetFilter["IS_CLOSE_ALL"])
            return false;
        else
        {
            for (int i = 0; i < filterLst.Count; i++)
            {
                var item = filterLst[i];
                string result1 = log.Length >= item.prefixLength ? log.Substring(0, item.prefixLength) : log;

                if (targetFilter.ContainsKey(result1)){
                    return targetFilter[result1];
                } 
            }

            return true;
        }
    }


    /// <summary>
    /// 日志排序
    /// </summary>
    /// <remarks>
    /// * ab、 abcd、 aabb、 abc、 ac、 a、.......  这些字符串  将其排序为   a、 aabb、 ab、 abc、 abcd、 ac  ......
    /// * 增加这个方法，也不好用。因为日志太乱了！排序无意义
    /// </remarks>
    void Sort()
    {
        List<string> keys = new List<string>(target.Keys.ToArray());

        keys.Sort((x, y) =>
        {
            // 首先比较字符集是否相同
            bool sameChars = AreCharSetsEqual(x, y);
            if (sameChars)
            {
                // 字符集相同则按长度升序
                return x.Length - y.Length;
            }
            else
            {
                // 字符集不同则按字符集排序
                return String.Concat(x.OrderBy(c => c))
                             .CompareTo(String.Concat(y.OrderBy(c => c)));
            }
        });

        Dictionary<string, bool> dic = new Dictionary<string, bool>();
        for (int i=0; i< keys.Count; i++)
        {
            dic.Add(keys[i], target[keys[i]]);
        }

        target = dic;
    }
    static bool AreCharSetsEqual(string a, string b)
    {
        return new HashSet<char>(a).SetEquals(new HashSet<char>(b));
    }


    /// <summary> 是否使用过滤 </summary>
    public  bool isUseDebugFilter
    {
        get
        {
            if (ApplicationSettings.Instance.isRelease)  //正式版先不放出去
                return false;

            if(_isUseDebugFilter == null)
            {
                int enable = PlayerPrefs.GetInt(PARAM_IS_USE_DEBUG_FILTER, 0);
                _isUseDebugFilter  = enable != 0;
            }

            return (bool)_isUseDebugFilter;
        }
        set
        {
            _isUseDebugFilter = value;
            PlayerPrefs.SetInt(PARAM_IS_USE_DEBUG_FILTER, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
    const string PARAM_IS_USE_DEBUG_FILTER = "PARAM_IS_USE_DEBUG_FILTER";
    public  bool? _isUseDebugFilter = null;


    /// <summary>
    /// 过滤配置缓存
    /// </summary>
    Dictionary<string, bool> cacheFilter
    {
        get
        {
            if (_cacheFilter == null)
            {
                Dictionary<string, bool> defauleFilter = new Dictionary<string, bool>()
                {
                    ["IS_OPEN_ALL"] = true,
                    ["IS_ClOSE_ALL"] = false,
                };
                string temp = PlayerPrefs.GetString(DEBUG_FILTER_CACHE, JsonConvert.SerializeObject(defauleFilter));
                _cacheFilter = JsonConvert.DeserializeObject<Dictionary<string, bool>>(temp);
            }
            return _cacheFilter;
        }
        set
        {
            _cacheFilter = value;
            PlayerPrefs.SetString(DEBUG_FILTER_CACHE, JsonConvert.SerializeObject(_cacheFilter));
        }
    }
    const string DEBUG_FILTER_CACHE = "DEBUG_FILTER_CACHE";
    Dictionary<string, bool> _cacheFilter;



    [Button]
    void ChangeIsUseDebugFilter()
    {
        isUseDebugFilter = !isUseDebugFilter;
        Debug.Log($"IsUseDebugFilter = {isUseDebugFilter}");
    }
    [Button]
    public void SaveCache()
    {

        //  Sort();   // 先不排序

        Dictionary<string, bool> filter = new Dictionary<string, bool>()
        {
            ["IS_OPEN_ALL"] = true,
            ["IS_ClOSE_ALL"] = false,
        };

        foreach (var pair in target)
        {
            filter[pair.Key] = pair.Value; // 若键存在则覆盖，不存在则添加
        }

        cacheFilter = filter;

    }

    [Button] 
    void ShowCache()
    {
        Debug.Log("cacheFilter = " + JsonConvert.SerializeObject(cacheFilter));
    }


    [Button] 
    void ClearCache()
    {
        Dictionary<string, bool> filter = new Dictionary<string, bool>()
        {
            ["IS_OPEN_ALL"] = true,
            ["IS_ClOSE_ALL"] = false,
        };
        cacheFilter = filter;

    }


}



/// <summary>
/// 静态过滤
/// </summary>
public partial class DebugFilterDynamics : MonoSingleton<DebugFilterDynamics>
{

    /// <summary> 是否使用固定过滤器 </summary>
    bool isUseFixedFilter = true;

    /*

    {
        "IS_OPEN_ALL": true,
        "IS_ClOSE_ALL": false,
        "【多语言】：键重复： ": true,
        "==@ 【SBox】rpc up: SBOX_GET_AC": true,
        "==@ 【SBox】rpc down: SBOX_GET": true,
        "【sqlite async helper】 updateQuery = UPDATE bussine": true,
        "【sqlite async helper】New data inserted": true,
        "==@ 【SBox】rpc up: RpcNameIsCo": true,
        "==@ 【SBox】rpc down: RpcNameI": true,
        "check code; isActive = True": true,
        "sql = UPDATE PlayerPrefs SET value='{\"flag\":421437": true,
        "==@ 【SBox】rpc up: RpcNameScor": true,
        "sql = UPDATE PlayerPrefs SET value='{\"PlayerId\":0,": true,
        "==@ 【SBox】rpc down: RpcNameS": true,
        "==@下行 SlotSpin :{\"result\":0,\"freeTimes\":0,\"curFree": true,
        "数据上报成功 {\"game_id\":\"2002\",\"player\":\"11109003\",\"rech": true,
        "数据上报地址 ： http://192.168.3.152/api/game_log/caishen": true,
        "Response: {\"code\":1,\"msg\":\"wrong params\"}": true,
        "sql = UPDATE PlayerPrefs SET value='{\"mJackpotBase": true,
        "is play sound : BaseBackground = True": true
    } 
     */

    const string fixedFilterJsonStr = @"{
            ""IS_OPEN_ALL"": true,
            ""IS_ClOSE_ALL"": false,
            ""【多语言】：键重复： "": true,
            ""==@ 【SBox】rpc up: SBOX_GET_AC"": true,
            ""==@ 【SBox】rpc down: SBOX_GET"": true,
            ""【sqlite async helper】 updateQuery = UPDATE bussine"": true,
            ""【sqlite async helper】New data inserted"": true,
            ""==@ 【SBox】rpc up: RpcNameIsCo"": true,
            ""==@ 【SBox】rpc down: RpcNameI"": true,
            ""check code; isActive = True"": true,
            ""sql = UPDATE PlayerPrefs SET value='{\""flag\"":421437"": true,
            ""==@ 【SBox】rpc up: RpcNameScor"": true,
            ""sql = UPDATE PlayerPrefs SET value='{\""PlayerId\"":0,"": true,
            ""==@ 【SBox】rpc down: RpcNameS"": true,
            ""==@下行 SlotSpin :{\""result\"":0,\""freeTimes\"":0,\""curFree"": true,
            ""数据上报成功 {\""game_id\"":\""2002\"",\""player\"":\""11109003\"",\""rech"": true,
            ""数据上报地址 ： http://192.168.3.152/api/game_log/caishen"": true,
            ""Response: {\""code\"":1,\""msg\"":\""wrong params\""}"": true,
            ""sql = UPDATE PlayerPrefs SET value='{\""mJackpotBase"": true,
            ""is play sound : BaseBackground = True"": true
        }";




    Dictionary<string, bool> fixedFilter
    {
        get
        {
            if (_fixedFilter == null)
            {
                _fixedFilter = JsonConvert.DeserializeObject<Dictionary<string, bool>>(fixedFilterJsonStr);
            }
            return _fixedFilter;
        }
        set
        {
            _fixedFilter = value;
        }
    }
    Dictionary<string, bool> _fixedFilter = null;





    public string GetFilterJsonStr()
    {
       return  JsonConvert.SerializeObject(targetFilter, Formatting.Indented);
    }
    public void SaveFilterJsonStr(string jsonStr)
    {
        Dictionary<string, bool> temp = JsonConvert.DeserializeObject<Dictionary<string, bool>>(jsonStr);
        targetFilter = temp;
    }
}
