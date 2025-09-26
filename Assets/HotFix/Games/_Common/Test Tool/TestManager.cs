using GameMaker;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleJSON;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//using GameMaker;


public partial class TestManager : MonoSingleton<TestManager>
{

    public InputField inputCode;

    //public InputField inputSpin;

    public InputField inputAutoUrl;

    public Text txtServer;

    public Text txtSoftwareVersion;

    public Text txtUser;

    public InputField inputGameConfig;

    public InputField inputCustomKV;//k1:2#k2:3#k3:4

    public Toggle toggleCheckCredit;

    public GameObject goGM;

    public GameObject goPages;

    public GameObject goCustomButtons;

    public List<Transform> pops;




    GameObject goBaseBtn;


    private void Start()
    {
        goBaseBtn = transform.Find("Base Button").gameObject;

        if (inputAutoUrl != null)
        {
            inputAutoUrl.text = PlayerPrefs.GetString("TestAutoUrl", "");
            DebugUtils.Log($"【TestAutoUrl】1 = {PlayerPrefs.GetString("TestAutoUrl", "")}");
        }
        else
        {
            DebugUtils.Log($"【TestAutoUrl】2 = {PlayerPrefs.GetString("TestAutoUrl", "")}");
        }

        if (toggleCheckCredit != null)
            toggleCheckCredit.isOn = true;

    }


    public void Init(string softwareVersion)
    {
        txtSoftwareVersion.text = softwareVersion;
    }


    Coroutine _corCheckKV;
    private void OnEnable()
    {
        _corCheckKV = StartCoroutine(CheckKeyValue());
        //MessageDispatcher.Register(RPCName.enterGame, OnReceivedEnterGameHandle);
    }
    private void OnDisable()
    {
        if (_corCheckKV != null)
        {
            StopCoroutine(_corCheckKV);
            _corCheckKV = null;
        }

        //MessageDispatcher.UnRegister(RPCName.enterGame, OnReceivedEnterGameHandle);
    }


    public void LoadAssetBundleAsync(string pth, UnityAction<AssetBundle> onFinishCallback)
    {
        ResourceManager.Instance.LoadAssetBundleAsync(pth, "", (bundle) =>
        {
            onFinishCallback?.Invoke(bundle);
        });
    }
    public void LoadAsset<T>(string pth, UnityAction<T> onFinishCallback) where T : UnityEngine.Object
    {
        T asset = ResourceManager.Instance.LoadAssetAtPathOnce<T>(pth);
        onFinishCallback?.Invoke(asset);
    }


    Dictionary<string, string> customKV = new Dictionary<string, string>();
    IEnumerator CheckKeyValue()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            string agrs = inputCustomKV.text ?? "";
            inputCustomKV.text = "";
            if (!string.IsNullOrEmpty(agrs))
            {
                string _agrs = agrs;
                List<string> xmpContents = new List<string>();
                if (agrs.Contains("</xmp>"))
                {
                    string pattern = @"<xmp>(.*?)<\/xmp>";
                    Regex regex = new Regex(pattern, RegexOptions.Singleline);

                    MatchCollection matches = regex.Matches(agrs);

                    foreach (Match match in matches)
                    {
                        xmpContents.Add(match.Groups[1].Value);
                    }

                    for (int i=0;i< xmpContents.Count;i++)
                    {
                        _agrs = _agrs.Replace($"<xmp>{xmpContents[i]}</xmp>", $"@@{0}");
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

                for (int i=0; i< xmpContents.Count; i++)
                {
                    string Ta = $"@@{i}";
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

                string res = "==@ [flags]";
                foreach (KeyValuePair<string, string> item in customKV)
                {
                    res += $" {item.Key} : {item.Value};";
                }
                DebugUtils.Log(res);
            }
        }
    }

    public bool HasKey(string key)
    {
        return customKV.ContainsKey(key);
    }

    public bool HasKeyOnce(string key)
    {
        bool isHas = customKV.ContainsKey(key);
        customKV.Remove(key);
        return isHas;
    }

    public string GetValue(string key)
    {
        if (!customKV.ContainsKey(key))
        {
            return "";
        }
        return customKV[key];
    }
    public string GetValueOnce(string key)
    {
        string res = "";
        if (customKV.ContainsKey(key))
        {
            res = customKV[key];
            //flags[key] = "";
            customKV.Remove(key);
        }
        return res;
    }



    /*
    public string getSpin()
    {
        if (inputSpin == null)
            return "";

        string res = inputSpin.text ?? "";
        inputSpin.text = "";
        return res;
    }*/




    public void SetCode(int code)
    {
        inputCode.text = code.ToString();
    }
    public void SetCode(string codeStr)
    {
        inputCode.text = codeStr;
    }


    public int getCode()
    {
        if (inputCode == null)
            return 0;

        string res = inputCode.text ?? "";
        inputCode.text = "";

        if (res == "")
            return 0;

        if(int.TryParse(res, out int result) == false)
        {
            DebugUtils.Log("【TestManager】code格式错误,必须为数字");
            return 0;
        }

        return result;
    }



    private string[] GetList(string res)
    {
        string[] lstStrs = res.Replace(" ", "").Split(',') ?? new string[] { };
        return lstStrs;
    }

    private T[] GetList<T>(string res)
    {
        string[] lstStrs = GetList(res);

        List<T> temp = new List<T>();
        for (int i = 0; i < lstStrs.Length; i++)
        {
            string cur = lstStrs[i];
            if (cur == "" || cur == null)
                continue;
            if (typeof(T) == typeof(int))
            {
                if (int.TryParse(cur, out int result) == true)
                {
                    (temp as List<int>).Add(result);
                }
                else
                {
                    DebugUtils.Log("【TestManager】list格式错误");
                    temp.Clear();
                    break;
                }
            }
            else if (typeof(T) == typeof(string))
            {
                (temp as List<string>).Add(cur);
            }
        }
        return temp.ToArray();
    }
    public string ToList(int[] list)
    {
        return string.Join(",", list);
    }


    #region 自动连接服务器
    public string GetAutoUrl()
    {
        if (inputAutoUrl == null)
            return "";
        String res = inputAutoUrl.text ?? "";
        PlayerPrefs.SetString("autoUrl", res);
        return res;
    }
    #endregion


    #region 游戏配置
    public string GetGameConfig()
    {
        string res = null;
        if (inputGameConfig == null)
        {
            res = "";
        }
        else
        {
            res = inputGameConfig.text ?? "";
            inputGameConfig.text = "";
        }

        if (res == "")
        {
            TextAsset jsnGameCfg = Resources.Load<TextAsset>("game_config");
            res = PlayerPrefs.GetString("gameConfig", jsnGameCfg.text);
        }
        PlayerPrefs.SetString("gameConfig", res);
        PlayerPrefs.Save();
        return res;
    }

    public void ClearGameConfig()
    {
        inputGameConfig.text = "";
        PlayerPrefs.DeleteKey("gameConfig");
        PlayerPrefs.Save();
    }
    #endregion





    public bool isCheckCredit
    {
        get
        {
#if !UNITY_EDITOR
            return true;
#endif
            if (toggleCheckCredit == null)
            {
                return true;
            }
            return toggleCheckCredit.isOn;
        }
    }




    public bool isTestSpin => customKV.ContainsKey("spin");


    
    public void GetTestSpinData(Action<JSONNode> responseCallback)
    {
        StartCoroutine(_getResponseData(GetValueOnce("spin"), responseCallback));
    }


    private IEnumerator _getResponseData(string resStr, Action<JSONNode> responseCallback)
    {
        //string spinRes = TestManager.Instance.getSpin();
        yield return new WaitForSeconds(0.2f);

       try
        {
            SimpleJSON.JSONNode dataDict = SimpleJSON.JSONNode.Parse(resStr as string);
            SimpleJSON.JSONNode res = dataDict.HasKey("protocol_key") ? dataDict["data"] : dataDict;
            //res.Add("is_custom_reels",true);
            JSONNode node = JSON.Parse("{}");
            node.Add("is_custom_reels", true);
            res.Add("user_config", node);
            if (responseCallback != null)
            {
                responseCallback(res);
            }
        }
        catch (Exception e)
        {
            DebugUtils.LogError($"【ERR】 data = {resStr}");
            DebugUtils.LogException(e);
        }
    }

    public void SetServer(string text)
    {
        if (txtServer == null)
            return;
        txtServer.text = text.Replace("https://", "").Replace("http://", "");
    }


    public void SetUserInfo(string text)
    {
        if (txtUser == null)
            return;
        txtUser.text = text;
    }





    private static MD5 md5Hash = MD5.Create();
    private static string GetHash(string source)
    {
        if (string.IsNullOrEmpty(source)) return null;

        byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(source));
        StringBuilder sBuilder = new StringBuilder();
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }
        return sBuilder.ToString();
    }



    bool _toolActive = true;
    public bool toolActive => _toolActive;
    public void SetToolActive(bool active)
    {
        _toolActive = true;
        goBaseBtn.SetActive(active);
        GameObject go = transform.Find("Anchor").gameObject;
        go.SetActive(false);
    }


    /// <summary>
    /// 打开或关闭操作面板
    /// </summary>
    public void OnClickBase()
    {
        GameObject go = transform.Find("Anchor").gameObject;

        go.SetActive(!go.active);
    }


    public const string CUSTOM_BUTTON = "CUSTOM_BUTTON";
    public const string PAGES = "PAGES";
    public void SetKV(string key,string value)
    {
        if (!customKV.ContainsKey(key))
            customKV.Add(key, value);
        else
            customKV[key] = value;
    }


    public void OnClickPages()
    {
        if (goPages == null || !HasKey(PAGES))
            return;

        GameObject goPop = ChangePop("Pop Pages");

        if (goPop != null && goPop.active)
        {

            string str = GetValue(PAGES);

            JSONNode _gmNode = JSONNode.Parse(str);

            JSONNode gmNode = JSONNode.Parse("{}");
            foreach (KeyValuePair<string, JSONNode> item in _gmNode)
            {
                if (item.Key.StartsWith("//"))
                    continue;
                gmNode.Add(item.Key, item.Value);
            }

            Transform tfmContent = goPop.transform.Find("Anchor/Scroll View/Viewport/Content");
            for (int i = tfmContent.childCount; i < gmNode.Count; i++)
            {
                Transform tfmNew = Instantiate(tfmContent.GetChild(tfmContent.childCount - 1));
                //tfmNew.parent = tfmContent;
                tfmNew.SetParent(tfmContent);
                tfmNew.localScale = Vector3.one;
                tfmNew.localPosition = Vector3.zero;
            }

            foreach (Transform tfm in tfmContent)
            {
                tfm.GetComponent<Button>().onClick.RemoveAllListeners();
            }

            for (int i = gmNode.Count; i < tfmContent.childCount; i++)
            {
                tfmContent.GetChild(i).gameObject.SetActive(false);
            }

            int idx = 0;
            foreach (KeyValuePair<string, JSONNode> item in gmNode)
            {
                Transform tfm = tfmContent.GetChild(idx);
                tfm.name = item.Key;
                tfm.gameObject.SetActive(true);

                string showName = item.Value.HasKey("nick_name") ? (string)item.Value["nick_name"] : item.Key;
                tfm.Find("Text").GetComponent<Text>().text = showName;

                tfm.GetComponent<Button>().onClick.AddListener(() =>
                {
                    OnClickPageItem((string)item.Value["page_name"], (string)item.Value["data"]);
                });

                idx++;
            }
        }
    }

    private void OnClickPageItem(string pageName,string data)
    {
        //DebugUtil.Log($"page  name = {name}  data = ：{data} ");

        if (!string.IsNullOrEmpty(data))
        {
            EventCenter.Instance.EventTrigger<EventData>(GlobalEvent.ON_TOOL_EVENT,
               new EventData<Dictionary<string,object>>(GlobalEvent.PageButton, 
                    new Dictionary<string, object>()
                    {
                        ["pageName"] = pageName,
                        ["pageData"] = data
                    }
                )
              );
            //EventCenter.Instance.EventTrigger<EventData<string>>(GlobalEvent.ON_TOOL_EVENT, new EventData<string>(name, data));
        }
        ChosePop();
        OnClickBase();
    }



    public void OnClickCustomButons()
    {
        if (goCustomButtons == null || !HasKey(CUSTOM_BUTTON))
            return;

        GameObject goPop = ChangePop("Pop Custom Buttons");

        if (goPop != null && goPop.active)
        {

            string str = GetValue(CUSTOM_BUTTON);

            JSONNode _gmNode = JSONNode.Parse(str);

            JSONNode gmNode = JSONNode.Parse("{}");
            foreach (KeyValuePair<string, JSONNode> item in _gmNode)
            {
                if (item.Key.StartsWith("//"))
                    continue;
                gmNode.Add(item.Key, item.Value);
            }

            Transform tfmContent = goPop.transform.Find("Anchor/Scroll View/Viewport/Content");

            float itemheight = tfmContent.GetChild(0).GetComponent<RectTransform>().sizeDelta.y;
            //float height = gmNode.Count * itemheight;
            RectTransform rtfmContent = tfmContent.GetComponent<RectTransform>();
            rtfmContent.sizeDelta = new Vector2(rtfmContent.sizeDelta.x, gmNode.Count * itemheight);

            for (int i = tfmContent.childCount; i < gmNode.Count; i++)
            {
                Transform tfmNew = Instantiate(tfmContent.GetChild(tfmContent.childCount - 1));
                //tfmNew.parent = tfmContent;
                tfmNew.SetParent(tfmContent);
                tfmNew.localScale = Vector3.one;
                tfmNew.localPosition = Vector3.zero;
            }

            foreach (Transform tfm in tfmContent)
            {
                tfm.GetComponent<Button>().onClick.RemoveAllListeners();
            }



            for (int i = gmNode.Count; i < tfmContent.childCount; i++)
            {
                tfmContent.GetChild(i).gameObject.SetActive(false);
            }

            int idx = 0;
            foreach (KeyValuePair<string, JSONNode> item in gmNode)
            {
                Transform tfm = tfmContent.GetChild(idx);
                tfm.name = item.Key;
                tfm.gameObject.SetActive(true);

                string showName = item.Value.HasKey("nick_name") ? (string)item.Value["nick_name"] : item.Key;
                tfm.Find("Text").GetComponent<Text>().text = showName;

                tfm.GetComponent<Button>().onClick.AddListener(() =>
                {
                    OnClickCustomButtonItem((string)item.Value["event_type"], (string)item.Value["event_name"], (string)item.Value["event_data"]);
                });

                idx++;
            }
        }
    }

    private void OnClickCustomButtonItem(string eventType, string eventName,string eventData)
    {
        //DebugUtil.Log($"page  name = {name}  data = ：{data} ");
        EventCenter.Instance.EventTrigger<EventData>(eventType, new EventData<string>(eventName, eventData));

        /*
        ChosePop();
        OnClickBase();
        */
    }






    #region 按钮点击事件



    [Button]
    public void OnClickGMBase()
    {
        if (goGM == null)
            return;
        GameObject goPop = ChangePop("Pop GM");

        if (goPop != null && goPop.active)
        {

            string keyDataGM = $"DATA_GM_{ConfigUtils.curGameId}"; // json存入字典的 key编号

            string str = GetValue(keyDataGM);

            if (string.IsNullOrEmpty(str))
            {
                LoadAsset<TextAsset>(ConfigUtils.curGameGMURL, (asset) =>
                {
                    SetKV(keyDataGM, asset.text);

                    CreatGMPop(goPop, asset.text);

                });
            }
            else
            {
                CreatGMPop(goPop, str);
            }
        }
    }
    void CreatGMPop(GameObject goPop, string jsn)
    {
        JSONNode _gmNode = JSONNode.Parse(jsn);

        int gameId = (int)_gmNode["game_id"];

        JSONNode gmNode = JSONNode.Parse("{}");
        foreach (KeyValuePair<string, JSONNode> item in _gmNode["gm_event"])
        {
            if (item.Key.StartsWith("//"))
                continue;
            gmNode.Add(item.Key, item.Value);
        }

        Transform tfmContent = goPop.transform.Find("Anchor/Scroll View/Viewport/Content");
        for (int i = tfmContent.childCount; i < gmNode.Count; i++)
        {
            Transform tfmNew = Instantiate(tfmContent.GetChild(tfmContent.childCount - 1));
            //tfmNew.parent = tfmContent;
            tfmNew.SetParent(tfmContent);
            tfmNew.localScale = Vector3.one;
            tfmNew.localPosition = Vector3.zero;
        }

        foreach (Transform tfm in tfmContent)
        {
            tfm.GetComponent<Button>().onClick.RemoveAllListeners();
        }

        for (int i = gmNode.Count; i < tfmContent.childCount; i++)
        {
            tfmContent.GetChild(i).gameObject.SetActive(false);
        }


        int idx = 0;
        foreach (KeyValuePair<string, JSONNode> item in gmNode)
        {
            Transform tfm = tfmContent.GetChild(idx);
            tfm.name = item.Key;
            tfm.gameObject.SetActive(true);

            string showName = item.Value.HasKey("nick_name") ? (string)item.Value["nick_name"] : item.Key;
            tfm.Find("Text").GetComponent<Text>().text = showName;


            string name = (string)item.Value["event_name"];
            string val = (string)item.Value["value"];
            tfm.GetComponent<Button>().onClick.AddListener(() =>
            {
                EventData data = new EventData<string>(name, gameId, val);
                EventCenter.Instance.EventTrigger<EventData>(GlobalEvent.ON_GM_EVENT, data);
                OnClickBase();
            });

            idx++;
        }
    }



#if false // 旧版本GM
    public void OnClickGMBase()
    {
        if (goGM == null)
            return;
        GameObject goPop = ChangePop("Pop GM");

        if (goPop != null && goPop.active)
        {

#if false
            TextAsset jsn8 = Resources.Load<TextAsset>($"GM/gm_{ConfigUtils.curGameId}");  //GM/gm_gameid.json
#else
            TextAsset jsn8 = ResourceManager.Instance.LoadAssetAtPathOnce<TextAsset>(HotfixSettings.Instance.GetGM(ConfigUtils.curGameId));
#endif

            DebugUtils.Log(jsn8.text);

            //JSONNode gmNode = JSONNodeUtil.GetValue(JSONNode.Parse(jsn8.text),"gm");


            JSONNode _gmNode = JSONNode.Parse(jsn8.text)["gm"];

            JSONNode gmNode = JSONNode.Parse("{}");
            foreach (KeyValuePair<string, JSONNode> item in _gmNode)
            {
                if (item.Key.StartsWith("//"))
                    continue;
                gmNode.Add(item.Key,item.Value);
            }

            Transform tfmContent = goPop.transform.Find("Anchor/Scroll View/Viewport/Content");
            for (int i = tfmContent.childCount; i < gmNode.Count; i++)
            {
                Transform tfmNew = Instantiate(tfmContent.GetChild(tfmContent.childCount-1));
                //tfmNew.parent = tfmContent;
                tfmNew.SetParent(tfmContent);
                tfmNew.localScale = Vector3.one;
                tfmNew.localPosition = Vector3.zero;
            }

            foreach (Transform tfm in tfmContent)
            {
                tfm.GetComponent<Button>().onClick.RemoveAllListeners();
            }

            for (int i = gmNode.Count; i <tfmContent.childCount ; i++)
            {
                tfmContent.GetChild(i).gameObject.SetActive(false);
            }

            int idx = 0;
            foreach (KeyValuePair<string,JSONNode> item in gmNode)
            {
                Transform tfm = tfmContent.GetChild(idx);
                tfm.name = item.Key;
                tfm.gameObject.SetActive(true);

                string showName = item.Value.HasKey("nick_name") ? (string)item.Value["nick_name"] : item.Key;
                tfm.Find("Text").GetComponent<Text>().text = showName;

                tfm.GetComponent<Button>().onClick.AddListener(() =>
                {
                    OnClickGMItem((string)item.Value["data"]);
                });

                idx++;
            }
        }
    }



    public void OnClickStateButton()
    {
        //if (goState == null)
        //    return;
        GameObject goPop = ChangePop("Pop Stats");
    }

    private void OnClickGMItem(string data)
    {
        //DebugUtil.Log($"GM指令：{data} ");
        if (!string.IsNullOrEmpty(data))
        {
            inputCustomKV.text = data;
        }
        ChosePop();
    }

#endif







    /// <summary>
    /// 打卡或关闭某个弹窗
    /// </summary>
    void ChosePop() =>  ChangePop("");

    private GameObject ChangePop(string popName = "")
    {
        if (pops == null)
            return null;

        GameObject goPop = null;
        foreach (Transform tfmChd in pops)
        {
            if (tfmChd.name == popName)
            {
                goPop = tfmChd.gameObject;
                goPop.SetActive(!goPop.active);
            }
            else{
                tfmChd.gameObject.SetActive(false);
            }
        }

        return goPop;
    }

    [Button]
    public void OnClickSpeedX10()
    {
        Time.timeScale = 10;
    }
    [Button]
    public void OnClickSpeedX2()
    {
        Time.timeScale = 2;
    }
    [Button]
    public void OnClickSpeedX1()
    {
        Time.timeScale = 1;

        //测试：清楚激活码
        //MachineDataManager.Instance.RequestClearCodingActive(null,null);


        //DebugUtil.LogWarning("测试打印机");
        //GameObject.Find("INSTANCE/Machine Controller/Device Printer Out").GetComponent<DevicePrinterOut>().DoPrinterOut();
    }


#endregion



    /*
    Dictionary<string, object> req = new Dictionary<string, object>(TestManager.Instance.spinAgrs);
    TestManager.Instance.spinAgrs = new Dictionary<string, object>();
    req.Add("bet", betCredit);
    req.Add("extra_bet", extraBetCredit);
    */

    public Dictionary<string, object> spinAgrs = new Dictionary<string, object>();

    [Button]
    void TestAddArg(string agrs)  // xxxx:xxx#
    {
        string[] itemsStrs = agrs.Replace(" ", "").Split('#') ?? new string[] { };

        spinAgrs = new Dictionary<string, object>();
        for (int i = 0; i < itemsStrs.Length; i++)
        {
            string[] res = itemsStrs[i].Split(':') ?? new string[] { "a", "-1" };
            spinAgrs.Add(res[0], int.Parse(res[1]));
        }
    }




    [Button]
    void TestShowDoor()
    {
        Animator anim = GameObject.Find("Game Contents/Animator").GetComponent<Animator>();
        anim.SetTrigger("Door Appear");

        //MessageDispatcher.Dispatch("OnContentUIEvent", new EventData("Start Door"));
    }

    [Button]
    void TestXmp(string str)
    {
        string input = @"  
        <html>  
            <body>  
                <xmp>  
                    这是第一个 <xmp> 标签对 之间的内容。  
                </xmp>  
                <p>其他内容</p>  
                <xmp>  
                    这是第二个 <xmp> 标签对 之间的内容，注意这里不能嵌套。  
                </xmp>  
            </body>  
        </html>  
        ";
        List<string> xmpContents = new List<string>();
        string pattern = @"<xmp>(.*?)<\/xmp>";
        Regex regex = new Regex(pattern, RegexOptions.Singleline);

        MatchCollection matches = regex.Matches(input);

        foreach (Match match in matches)
        {
            xmpContents.Add(match.Groups[1].Value);
        }
        foreach (var content in xmpContents)
        {
            DebugUtils.Log($"提取的内容是： {content}");
        }
    }


}



#if false
/// <summary>
/// 编辑json
/// </summary>
public partial class TestManager : MonoSingleton<TestManager>
{
    string[] lines;
    int curLineIndex;
    string curValue;

    Action<string> onJaonEditorSaveCallback;


    public void OnUseEditPop()
    {

        GameObject goPop = ChangePop("Pop Json Editor");

        if (goPop != null && goPop.active)
        {

        }
    }

    [Button]
    void TestJsonPop()
    {
        OnUseEditPop();
        OpenJsonPop();
    }


    public void OpenJsonPop(string jsonStr = null, Action<string> onSaveCallback = null)
    {
        jsonEditorBtnSave.onClick.RemoveAllListeners();
        jsonEditorBtnSave.onClick.AddListener(() =>
        {
            string result = string.Join("", lines);
            JObject obj = JObject.Parse(result);
            //onSaveCallback?.Invoke(JsonConvert.SerializeObject(obj)); //没有格式化
            onSaveCallback?.Invoke(obj.ToString()); //有格式化

            Debug.Log(obj.ToString());
        });


        string json1 = @"
            {""msgtype"":""resp"",""method"":""Report"",""seq_id"":10,""body"":[{""code"":0,""msg"":"""",""game_state"":""Idle"",""type"":""SqlSelect"",""data"":""输入命令非SELECT,  ==== receive sql: 214313\r\n"",""is_close"":false,""lst"":[0,1,2,3]}]}
            ";
        // 反序列化为动态对象
        JObject obj = JObject.Parse( !string.IsNullOrEmpty(jsonStr) ? jsonStr : json1);

        string result = obj.ToString();
        Debug.Log(result);


        // 按换行符拆分（处理 \n 和 \r\n）
        lines = result.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);




        GameObject goClone = tfmJsonPopContent.GetChild(0).gameObject;
        

        float itemheight = tfmJsonPopContent.GetChild(0).GetComponent<RectTransform>().sizeDelta.y;
        //float height = gmNode.Count * itemheight;
        RectTransform rtfmContent = tfmJsonPopContent.GetComponent<RectTransform>();
        rtfmContent.sizeDelta = new Vector2(rtfmContent.sizeDelta.x, lines.Length * itemheight);


        for (int i = tfmJsonPopContent.childCount; i < lines.Length; i++)
        {
            Transform tfmNew =  GameObject.Instantiate(goClone).transform;
            tfmNew.SetParent(tfmJsonPopContent);

            tfmNew.localPosition = Vector3.zero;
            tfmNew.localScale = Vector3.one;
            tfmNew.localRotation = Quaternion.identity;
        }

        foreach (Transform tfm in tfmJsonPopContent)
        {
            tfm.gameObject.SetActive(false);
        }

        for (int i=0; i<lines.Length; i++)
        {
            Transform tfm = tfmJsonPopContent.GetChild(i);
            tfm.gameObject.SetActive(true);
            tfm.Find("Text").GetComponent<Text>().text = lines[i];
            Button btnComp = tfm.GetComponent<Button>();
            btnComp.onClick.RemoveAllListeners();

            int index = i;
            btnComp.onClick.AddListener(() =>
            {
                OnLineClick(index);
            });
        }
        /* 遍历每一行
        foreach (string line in lines)
        {
            Debug.Log("## " + line);
        }*/
    }

    Transform tfmJsonPopContent => transform.Find("Anchor/Json Editor/Pop Json Editor/Nodes/Viewport/Content");
    Text jsonEditorKeyName => transform.Find("Anchor/Json Editor/Pop Json Editor/Editor/Top (1)/KEY").GetComponent<Text>();
    Text jsonEditorKey => transform.Find("Anchor/Json Editor/Pop Json Editor/Editor/Top (1)/KEY (1)").GetComponent<Text>();
    InputField jsonEditorValueInp => transform.Find("Anchor/Json Editor/Pop Json Editor/Editor/Top (2)/KEY (2)/InputField").GetComponent<InputField>();
    Button jsonEditorBtnConfirm => transform.Find("Anchor/Json Editor/Pop Json Editor/Editor/Top (2)/KEY (2)/Button Modify").GetComponent<Button>();
    Button jsonEditorBtnSoftKeyboard => transform.Find("Anchor/Json Editor/Pop Json Editor/Editor/Top (2)/KEY (2)/Button Soft Keyboard").GetComponent<Button>();
    Button jsonEditorBtnSave => transform.Find("Anchor/Json Editor/Pop Json Editor/Buttom/Button Save").GetComponent<Button>();


    void OnLineClick(int index)
    {
        jsonEditorBtnSoftKeyboard.onClick.RemoveAllListeners();
        jsonEditorBtnConfirm.onClick.RemoveAllListeners();

        string targetStr = lines[index];
        if (targetStr.EndsWith("[") || targetStr.EndsWith("]") || targetStr.EndsWith("{") || targetStr.EndsWith("}"))
            return;


        curLineIndex = index;

        if (targetStr.EndsWith(","))
            targetStr = targetStr.TrimEnd(','); // 去除末尾的所有逗号

        string[] strs = targetStr.Split(':');

        if (strs.Length>1) // 键值对
        {
            jsonEditorKeyName.text = "KEY:";
            jsonEditorKey.text = strs[0].Trim();
            curValue = strs[1].Trim();
            jsonEditorValueInp.text = curValue;
            // strs
        }
        else  // 数组
        {
            int idx = 0;
            for (int j = index-1; j>=0; j--)
            {
                if (lines[j].EndsWith("["))
                {
                    break;
                }
                else
                {
                    idx++;
                }
            }

            jsonEditorKeyName.text = "INDEX:";
            jsonEditorKey.text = $"{idx}";
            curValue = strs[0].Trim();
            jsonEditorValueInp.text = curValue;

        }

        jsonEditorBtnConfirm.onClick.AddListener(() =>
        {
            OnEditorValue(jsonEditorValueInp.text);
        });

        jsonEditorBtnSoftKeyboard.onClick.AddListener(() =>
        {

            SetParameterPopupInfo inf = new SetParameterPopupInfo()
            {
                title = "设置节点数值",
                paramLst = new List<ParamInfo>()
                {
                    new ParamInfo()
                    {
                        name = $"{jsonEditorKeyName.text}{jsonEditorKey.text}:",
                        value = curValue,
                        tipFunc = (res) =>
                        {
                            if(res.Length < 8)
                                return "输入值长度必须大于8";
                            return null;
                        }
                    },
                     new ParamInfo()
                    {
                        name = $"test2:",
                        value = "10",
                        tipFunc = (res) =>
                        {
                            try
                            {
                                int temp = int.Parse(res);
                            }catch (Exception ex) {
                                return "输入值必须是数字";
                            }
                            return null;
                        }
                    }
                },
                onFinishCallback = (string res) =>
                {
                    if (res != null)
                    {
                        jsonEditorValueInp.text = res;
                    }
                }
            };
            SetParameterPopupHandler.Instance.OpenPopup(inf);

        });
        Debug.Log("## " + lines[index]);

    }


    void OnEditorValue(string inpValue)
    {
        //string oldStr = lines[curLineIndex];
        lines[curLineIndex] = lines[curLineIndex].Replace(curValue, inpValue);
        curValue = inpValue;
        //if (oldStr != lines[curLineIndex]) curValue = inpValue;

        Transform tfm = tfmJsonPopContent.GetChild(curLineIndex);
        tfm.Find("Text").GetComponent<Text>().text = lines[curLineIndex];
    }

}
#endif


public partial class TestManager : MonoSingleton<TestManager>
{
    [Button]
    void GetBatches()
    {
        // 获取当前场景
        Scene currentScene = SceneManager.GetActiveScene();

        // 获取当前场景中的所有游戏对象
        GameObject[] rootGameObjects = currentScene.GetRootGameObjects();

        int batches = 0;
        foreach (var rootObject in rootGameObjects)
        {
            Renderer[] rds = rootObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer rd in rds)
            {
                //batches += rd.batchedMaterials.Length;
            }
        }
        DebugUtils.Log($"Batches : {batches}");
    }


    [Button]
    void TestShowJson()
    {
        string json = @"{
            ""msgtype"": ""resp"",
            ""method"": ""Report"",
            ""seq_id"": 10,
            ""body"": [{
                ""code"": 0,
                ""msg"": """",
                ""game_state"": ""Idle"",
                ""type"": ""SqlSelect"",
                ""data"": ""输入命令非SELECT,  ==== receive sql: 214313\r\n"",
                ""is_close"": false,
                ""lst"": [0, 1, 2, 3]
            }]
        }";


        string json1 = @"
            {""msgtype"":""resp"",""method"":""Report"",""seq_id"":10,""body"":[{""code"":0,""msg"":"""",""game_state"":""Idle"",""type"":""SqlSelect"",""data"":""输入命令非SELECT,  ==== receive sql: 214313\r\n"",""is_close"":false,""lst"":[0,1,2,3]}]}
            ";
        // 反序列化为动态对象
        JObject obj = JObject.Parse(json1);

        string result = obj.ToString();
        Debug.Log(result);


        // 按换行符拆分（处理 \n 和 \r\n）
        string[] lines = result.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

        // 遍历每一行
        foreach (string line in lines)
        {
            Debug.Log("## " + line);
        }

    }



    [Button]
    void TestDefaultJsonEditPop()
    {
#if !false

        string data = @"{
            ""msgtype"": ""resp"",
            ""method"": ""Report"",
            ""seq_id"": 10,
            ""body"": [
                {
                    ""code"": 0,
                    ""msg"": """",
                    ""game_state"": ""Idle"",
                    ""type"": ""SqlSelect"",
                    ""data"": ""输入命令非SELECT,  ==== receive sql: 214313\r\n"",
                    ""is_close"": false,
                    ""lst"": [0, 1, 2, 3]
                },
                {
                    ""code"": 1,
                    ""msg"": """",
                    ""game_state"": ""Idle"",
                    ""type"": ""SqlSelect"",
                    ""data"": ""输入命令非SELECT,  ==== receive sql: 214313\r\n"",
                    ""is_close"": false,
                    ""lst"": [0, 1, 2, 3]
                },
                {
                    ""code"": 2,
                    ""msg"": """",
                    ""game_state"": ""Idle"",
                    ""type"": ""SqlSelect"",
                    ""data"": ""输入命令非SELECT,  ==== receive sql: 214313\r\n"",
                    ""is_close"": false,
                    ""lst"": [0, 1, 2, 3]
                },
                {
                    ""code"": 3,
                    ""msg"": """",
                    ""game_state"": ""Idle"",
                    ""type"": ""SqlSelect"",
                    ""data"": ""输入命令非SELECT,  ==== receive sql: 214313\r\n"",
                    ""is_close"": false,
                    ""lst"": [0, 1, 2, 3]
                },
                {
                    ""code"": 4,
                    ""msg"": """",
                    ""game_state"": ""Idle"",
                    ""type"": ""SqlSelect"",
                    ""data"": ""输入命令非SELECT,  ==== receive sql: 214313\r\n"",
                    ""is_close"": false,
                    ""lst"": [0, 1, 2, 3]
                }
            ]
        }
        ";
        DefaultJsonEditPopup.Instance.OpenPopup(new JsonEditPopupInfo()
        {
            title = "修改设置json数据",
            data = data,
            onFinishCallback = (string res) =>
            {
                Debug.Log($"修改结果： {res}");
            }
        });
#endif
    }




    [Button]
    void TestSlideSettingPop()
    {
#if !false

        DefaultSliderSettingPopup.Instance.OpenPopup(new SliderSettingPopupInfo()
        {
            title = "修改设置json数据\n1:{0}",
            curValue = 40,
            maxValue = 100,
            minValue = 20,
            onFinishCallback = (int? res) =>
            {
                Debug.Log($"修改结果： {res}");
            }
        });
#endif
    }

    int idx = 0;
    [Button]
    void TestDefaultTipPopup()
    {
#if !false

        DefaultTipPopup.Instance.OpenPopup($"{idx++}");
        DefaultTipPopup.Instance.OpenPopupOnce("99");
#endif
    }


    [Button]
    void TestDefaultSetParameterPopup()
    {
        SetParameterPopupInfo inf = new SetParameterPopupInfo()
        {
            title = "设置节点数值",
            paramLst = new List<ParamInfo>()
                {
                    new ParamInfo()
                    {
                       name = $"test1:",
                        value = "11",
                        tipFunc = (res) =>
                        {
                            if(res.Length < 8)
                                return "输入值长度必须大于8";
                            return null;
                        }
                    },
                     new ParamInfo()
                    {
                        name = $"test2:",
                        value = "10",
                        tipFunc = (res) =>
                        {
                            try
                            {
                                int temp = int.Parse(res);
                            }catch (Exception ex) {
                                return "输入值必须是数字";
                            }
                            return null;
                        }
                    }
                },
            onFinishCallback = (string res) =>
            {
                if (res != null)
                {
                    Debug.Log(res);
                }
            }
        };
        DefaultSetParameterPopup.Instance.OpenPopup(inf);
    }

    [Button]
    void TestDefaultCommonPopup()
    {
        DefaultCommonPopup.Instance.OpenPopup(new CommonPopupInfo()
        {
            type = CommonPopupType.YesNo,
            title = "测试",
            text = "内容",
            buttonText1 = "取消01",
            callback1 = () =>
            {
                Debug.Log("按钮1按下");
            },
            buttonText2 = "确定02",
            callback2 = () =>
            {
            Debug.Log("按钮2按下");
            },
        });

    }


    bool isTestOpenMask = false;
    [Button]
    void TestDefaultMaskPopup()
    {
        isTestOpenMask = !isTestOpenMask;
        if(isTestOpenMask)
            DefaultMaskPopup.Instance.OpenPopup();
        else
            DefaultMaskPopup.Instance.ClosePopup();

    }
}

