using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class DebugFilterInfo
{
    /// <summary> ��־��ͷ���ٸ��ַ�</summary>
    public int prefixLength = 12;

    /// <summary> ���Ӷ��б�����ٸ�����</summary>
    public int prefixCount = 60;

    /// <summary> ��־����</summary>
    public List<string> queue = new List<string>();
}


/// <summary>
/// ��̬����
/// </summary>
public partial class DebugFilterDynamics : MonoSingleton<DebugFilterDynamics>
{

    /// <summary>
    /// ��־����
    /// </summary>
    /// <remarks>
    /// * ������������Ӱ�첻��</br>
    /// * ������������������
    /// </remarks>
   /* public List<DebugFilterInfo> filterLst = new List<DebugFilterInfo>()
   {

       new DebugFilterInfo()  // ��Sbox��
       {
           prefixLength = 10,
       },
       new DebugFilterInfo() // ��Sbox�� rpc 
       {
           prefixLength = 20,
       },
       new DebugFilterInfo() // ��Sbox�� rpc  123
       {
           prefixLength = 30,
       },

   };*/



    public List<DebugFilterInfo> filterLst = new List<DebugFilterInfo>()
   {
       new DebugFilterInfo() // ��Sbox�� rpc  123
       {
           prefixLength = 50,
       },

   };


    Dictionary<string, bool> target = new Dictionary<string, bool>();


    /// <summary> ���ڵ��ڶ�������ʼ���� </summary>
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
                item.queue.RemoveRange(item.prefixCount, item.queue.Count - item.prefixCount); // ֻ����ǰitem.prefixCount ��Ԫ��
            
            if (!target.Keys.Contains(result1))
            {
                int count = item.queue.Count(s => s == result1);
                if(count >= numGetTarget)
                    target.Add(result1,true);
            }  
        }
    }



    /// <summary>
    /// ��̬ �� �̶� ����־������
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
        // ������־���˹��ܣ�

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
    /// ��־����
    /// </summary>
    /// <remarks>
    /// * ab�� abcd�� aabb�� abc�� ac�� a��.......  ��Щ�ַ���  ��������Ϊ   a�� aabb�� ab�� abc�� abcd�� ac  ......
    /// * �������������Ҳ�����á���Ϊ��־̫���ˣ�����������
    /// </remarks>
    void Sort()
    {
        List<string> keys = new List<string>(target.Keys.ToArray());

        keys.Sort((x, y) =>
        {
            // ���ȱȽ��ַ����Ƿ���ͬ
            bool sameChars = AreCharSetsEqual(x, y);
            if (sameChars)
            {
                // �ַ�����ͬ�򰴳�������
                return x.Length - y.Length;
            }
            else
            {
                // �ַ�����ͬ���ַ�������
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


    /// <summary> �Ƿ�ʹ�ù��� </summary>
    public  bool isUseDebugFilter
    {
        get
        {
            if (ApplicationSettings.Instance.isRelease)  //��ʽ���Ȳ��ų�ȥ
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
    /// �������û���
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

        //  Sort();   // �Ȳ�����

        Dictionary<string, bool> filter = new Dictionary<string, bool>()
        {
            ["IS_OPEN_ALL"] = true,
            ["IS_ClOSE_ALL"] = false,
        };

        foreach (var pair in target)
        {
            filter[pair.Key] = pair.Value; // ���������򸲸ǣ������������
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
/// ��̬����
/// </summary>
public partial class DebugFilterDynamics : MonoSingleton<DebugFilterDynamics>
{

    /// <summary> �Ƿ�ʹ�ù̶������� </summary>
    bool isUseFixedFilter = true;

    /*

    {
        "IS_OPEN_ALL": true,
        "IS_ClOSE_ALL": false,
        "�������ԡ������ظ��� ": true,
        "==@ ��SBox��rpc up: SBOX_GET_AC": true,
        "==@ ��SBox��rpc down: SBOX_GET": true,
        "��sqlite async helper�� updateQuery = UPDATE bussine": true,
        "��sqlite async helper��New data inserted": true,
        "==@ ��SBox��rpc up: RpcNameIsCo": true,
        "==@ ��SBox��rpc down: RpcNameI": true,
        "check code; isActive = True": true,
        "sql = UPDATE PlayerPrefs SET value='{\"flag\":421437": true,
        "==@ ��SBox��rpc up: RpcNameScor": true,
        "sql = UPDATE PlayerPrefs SET value='{\"PlayerId\":0,": true,
        "==@ ��SBox��rpc down: RpcNameS": true,
        "==@���� SlotSpin :{\"result\":0,\"freeTimes\":0,\"curFree": true,
        "�����ϱ��ɹ� {\"game_id\":\"2002\",\"player\":\"11109003\",\"rech": true,
        "�����ϱ���ַ �� http://192.168.3.152/api/game_log/caishen": true,
        "Response: {\"code\":1,\"msg\":\"wrong params\"}": true,
        "sql = UPDATE PlayerPrefs SET value='{\"mJackpotBase": true,
        "is play sound : BaseBackground = True": true
    } 
     */

    const string fixedFilterJsonStr = @"{
            ""IS_OPEN_ALL"": true,
            ""IS_ClOSE_ALL"": false,
            ""�������ԡ������ظ��� "": true,
            ""==@ ��SBox��rpc up: SBOX_GET_AC"": true,
            ""==@ ��SBox��rpc down: SBOX_GET"": true,
            ""��sqlite async helper�� updateQuery = UPDATE bussine"": true,
            ""��sqlite async helper��New data inserted"": true,
            ""==@ ��SBox��rpc up: RpcNameIsCo"": true,
            ""==@ ��SBox��rpc down: RpcNameI"": true,
            ""check code; isActive = True"": true,
            ""sql = UPDATE PlayerPrefs SET value='{\""flag\"":421437"": true,
            ""==@ ��SBox��rpc up: RpcNameScor"": true,
            ""sql = UPDATE PlayerPrefs SET value='{\""PlayerId\"":0,"": true,
            ""==@ ��SBox��rpc down: RpcNameS"": true,
            ""==@���� SlotSpin :{\""result\"":0,\""freeTimes\"":0,\""curFree"": true,
            ""�����ϱ��ɹ� {\""game_id\"":\""2002\"",\""player\"":\""11109003\"",\""rech"": true,
            ""�����ϱ���ַ �� http://192.168.3.152/api/game_log/caishen"": true,
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
