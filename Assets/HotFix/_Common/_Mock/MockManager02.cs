using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Timers;
using UnityEngine;
using Random = System.Random;


public class MockManager02 : MonoSingleton<MockManager02>, ISocket
{

    Action<object> _onConnected = null;
    Action<object> _onMessage = null;
    Action<object> _onError = null;
    Action<object> _onClosed = null;

    public Action<object> onConnected { set { this._onConnected = value; } }         // 连接回调
    public Action<object> onMessage { set { this._onMessage = value; } }         // 消息回调 onMessage: (event: any)
    public Action<object> onError { set { this._onError = value; } }          // 错误回调
    public Action<object> onClosed { set { this._onClosed = value; } }           // 关闭回调





    private Action task = null;

    private bool isRun = false;
    private Queue<Action> taskQueue = new Queue<Action>();
    private void Update()
    {

        if (this.task != null)
        {
            var func = this.task;
            this.task = null;
            func.Invoke();
        }

        if (!isRun)
        {
            isRun = true;
            while (taskQueue.Count > 0)
            {
                var task = taskQueue.Dequeue();
                task.Invoke();
            }
            isRun = false;
        }
    }


























    void OnEnterGame()
    {
        TextAsset jsn = Resources.Load<TextAsset>($"GM Mock/mock_gm_g{_gameId}");
        if (jsn != null)
        {
            Debug.Log($"读取到的游戏配置数据 ： {jsn.text}");
            JSONNode node = JSONNode.Parse(jsn.text);

        }
    }





    Dictionary<int, Dictionary<string, GameData>> allData = new Dictionary<int, Dictionary<string, GameData>>();

    //public bool isKo = true;



    public void testD()
    {
        var anonymousObj = new { name = "小明", age = 30 };

        // 获取匿名对象的类型  
        Type anonymousType = anonymousObj.GetType();

        // 获取 name 字段的值  
        PropertyInfo nameProperty = anonymousType.GetProperty("name");
        string nameValue = (string)nameProperty.GetValue(anonymousObj);
        Debug.Log($"Name: {nameValue}");

        // 获取 age 字段的值  
        PropertyInfo ageProperty = anonymousType.GetProperty("age");
        int ageValue = (int)ageProperty.GetValue(anonymousObj);
        Debug.Log($"Age: {ageValue}");
    }


    /// <summary>
    /// 本地的假数据存储
    /// g8980__1747108331466__keno_spin__ko_21231.json
    /// </summary>
    /// <param name="?">int : 游戏id  （8980）</param>
    /// <param name="?">string: 协议名称 （keno_spin）</param>
    /// <param name="?">List<KeyValuePair<string, string>>>:   文件名[不带.json] - 文件数据  =  （g8980__keno_spin__80808 ，  json string） </param>
    /// <returns>String</returns>
    Dictionary<int, Dictionary<string, List<KeyValuePair<string, string>>>> jsonLst = new Dictionary<int, Dictionary<string, List<KeyValuePair<string, string>>>>();
    //private async void LoadJsonAsync(int gameid, Action<object> onComplete) {
    private void LoadJsonAsync(int gameid, Action<object> onComplete)
    {
        Dictionary<int, string> load = new Dictionary<int, string>();
        if (!this.jsonLst.ContainsKey(-1))
        {
            load.Add(-1, "mock/g/");
        }

        if (!this.jsonLst.ContainsKey(gameid) && gameid != -1)
        {
            load.Add(gameid, $"mock/g{gameid}/");
        }

        if (load.Count == 0)
        {
            return;
        }


        string res = null;
        foreach (KeyValuePair<int,string> kv in load)
        {

            int gId = kv.Key;
            //g__login__4234.json
            //g8980__keno_spin__ko_21231.json
            //g8980__keno_spin__80808.json
            TextAsset[] jsons = Resources.LoadAll<TextAsset>(kv.Value);  //
            if (jsons == null || jsons.Length == 0)
            {
                res += $"【ERR】:游戏di={gId}的数据源为空\n";
            }


            //g8980__1747108331466__keno_spin__ko_21231.json

            foreach (TextAsset json in jsons)
            {
                //string input = "apple===banana---orange+++grape";
                //string pattern = @"[__]{2,}";
                //string[] result = Regex.Split(input, pattern);

                string[] result = Regex.Split(json.name, "__");
                string rpc = result[2];

                if (!this.jsonLst.ContainsKey(gId))
                {
                    this.jsonLst.Add(gId, new Dictionary<string, List<KeyValuePair<string, string>>>());
                }

                if (!this.jsonLst[gId].ContainsKey(rpc))
                {
                    this.jsonLst[gId].Add(rpc, new List<KeyValuePair<string, string>>());
                }

                // 所有数据
                this.jsonLst[gId][rpc].Add(new KeyValuePair<string, string>(json.name, json.text));//任意数据
                

                // 特殊游戏
                if (gId != -1)
                {
                    specTurnInfo.Analysis(gId, json.name);
                }
            }

        }

        if (onComplete != null)
        {
            //onComplete.Invoke();??
            onComplete(res);
        }

    }

    public int gameId
    {
        get => _gameId;
    }

    private int _gameId = -1;

    private System.Timers.Timer _timer = null;


    private void ClearTime()
    {
        if (this._timer != null)
        {
            this._timer.Stop();
            this._timer.Dispose();
            this._timer = null;
            this.task = null;
            this.taskQueue = new Queue<Action>();
            isRun = false;
        }
    }

    public bool Connect(NetConnectOptions options)
    {
        Debug.Log(" 【链接 mock socket】");


        this.ClearTime();
        this._timer = new System.Timers.Timer(500);
        this._timer.AutoReset = false; // 是否重复执行
        this._timer.Enabled = true; //是否执行Elapsed事件
        this._timer.Elapsed += (object sender, ElapsedEventArgs e) => {
            this.ClearTime();
            //this._Connect();  //这里是子线程，不能使用LoadAll加载资源
            this.task = () =>
            {
                this._Connect();
            };
        };
        this._timer.Start();

        return true;
    }

    public void _Connect()
    {
        this.jsonLst = new Dictionary<int, Dictionary<string, List<KeyValuePair<string, string>>>>();

        this.LoadJsonAsync(-1, (res) =>
        {
            if (res != null)
            {
                Debug.LogError(res);
                if (this._onError != null) this._onError(res);
                return;
            }
            if (this._onConnected != null) this._onConnected(null);
        }); //加载公共包
    }

    public void Close()
    {
        this.jsonLst = new Dictionary<int, Dictionary<string, List<KeyValuePair<string, string>>>>();
        if (this._onClosed != null) this._onClosed(null);
    }


    /// <summary> 当前本轮游戏的编号（时间戳） </summary>
    string curSpecTurnNumber = "";

    /// <summary> 当前本轮游戏，的第几包数据 </summary>
    int curSpecTurnIndex = -1;

    public int Send(object message)
    {
        JSONNode node = JSONNode.Parse(message as string);

        var rpcName = (string)node["protocol_key"];

        string res = $"\"protocol_key\":\"{rpcName}\"," + "\"data\":{\"err\":404}}";
        string fileName = null;
        if (rpcName == RPCName.enterGame)
        {

            this._gameId = node["data"]["game_id"];

            OnEnterGame();

            this._ConnectGame(this._gameId, (err) =>
            {
                if (err != null)
                    return;

                var message_game = this.jsonLst[this._gameId];

                //string rpcKey = $"g{this._gameId}__{rpcName}";

                if (message_game.ContainsKey(rpcName))
                {
                    var lst = message_game[rpcName];

                    if (lst.Count > 1)
                    {
                        Random random = new Random();
                        var index = random.Next(0, lst.Count);
                        res = lst[index].Value;
                        fileName = lst[index].Key;
                    }
                    else
                    {
                        res = lst[0].Value;
                        fileName = lst[0].Key;
                    }
                }

                this.ClearTime();
                this._timer = new System.Timers.Timer(this._respondTimeMS);
                this._timer.AutoReset = false; // 是否重复执行
                this._timer.Enabled = true; //是否执行Elapsed事件
                this._timer.Elapsed += (object sender, ElapsedEventArgs e) => {
                    this.ClearTime();
                    /*this.task = () =>
                    {
                        if (fileName != null)
                            Debug.Log($"@【mock 下行】  rpcKey = {rpcKey} , fileName =  {fileName}");
                        if (this._onMessage != null)
                            this._onMessage(res);
                    };*/
                    taskQueue.Enqueue(() =>
                    {
                        if (fileName != null)
                            Debug.Log($"@【mock 下行】 , fileName =  {fileName}");
                        if (this._onMessage != null)
                            this._onMessage(res);
                    });

                };
                this._timer.Start();
            });

        }
        else
        {
            if (rpcName == RPCName.lobby)
            {
                this._gameId = -1;
            }

            var message_hall = this.jsonLst[-1];
            var message_game = this.jsonLst[this._gameId];


            // 使用大厅数据
            if (message_hall.ContainsKey(rpcName))
            {
                var lst = message_hall[rpcName];
                if (lst.Count > 1)
                {
                    Random random = new Random();
                    var index = random.Next(0, lst.Count);
                    res = lst[index].Value;
                    fileName = lst[index].Key;
                }
                else
                {
                    res = lst[0].Value;
                    fileName = lst[0].Key;
                }
            }

            // 优先使用游戏数据
            if (message_game.ContainsKey(rpcName))
            {
                var lst = message_game[rpcName];
                if (!string.IsNullOrEmpty(curSpecTurnNumber))
                {
                    string name = specTurnInfo.specTurn[_gameId][curSpecTurnNumber][curSpecTurnIndex++];

                    if(curSpecTurnIndex == specTurnInfo.specTurn[_gameId][curSpecTurnNumber].Count - 1)
                    {
                        curSpecTurnIndex = 1;
                        curSpecTurnNumber = "";
                    }

                    KeyValuePair<string, string> kv = lst.Find(item => item.Key == name);

                    if (!string.IsNullOrEmpty(kv.Key))
                    {
                        res = kv.Value;
                        fileName = kv.Key;
                    }
                }
                else
                {
                    if (lst.Count > 1)
                    {
                        do
                        {
                            Random random = new Random();
                            var index = random.Next(0, lst.Count);
                            res = lst[index].Value;
                            fileName = lst[index].Key;
                        } while (specTurnInfo.isSpecTurn(fileName));
                    }
                    else
                    {
                        res = lst[0].Value;
                        fileName = lst[0].Key;
                    }
                }

            }


            this.ClearTime();
            this._timer = new System.Timers.Timer(this._respondTimeMS);
            this._timer.AutoReset = false; // 是否重复执行
            this._timer.Enabled = true; //是否执行Elapsed事件
            this._timer.Elapsed += (object sender, ElapsedEventArgs e) => {
                this.ClearTime();
                /*this.task = () =>
                {
                    if(fileName != null)
                        Debug.Log($"@【mock 下行】  rpcKey = {rpcKey} , fileName =  {fileName}");
                    if (this._onMessage != null)
                        this._onMessage(res);
                };*/
                taskQueue.Enqueue(() =>
                {
                    if (fileName != null)
                        Debug.Log($"@【mock 下行】  , fileName =  {fileName}");
                    if (this._onMessage != null)
                        this._onMessage(res);
                });

            };
            this._timer.Start();


        }

        return 1;
    }

    private int _respondTimeMS = 500;
    private void _ConnectGame(int id, Action<object> callback)
    {
        if (this.jsonLst.ContainsKey(id))
        {
            if (callback != null) callback(null);
        }
        else
        {
            this.LoadJsonAsync(id, (res) =>
            {
                if (res != null)
                {
                    Debug.LogError(res);
                    if (callback != null) callback(res);
                    return;
                }
                if (callback != null) callback(null);
            });

        }
    }



    public List<object> InterceptHttpPost(string method)
    {
        var rpcName = method.Replace("/", "__");

        TextAsset jsn8 = Resources.Load<TextAsset>("tempdata/keno_play_response_v1");
        if (jsn8 != null && jsn8.text != null)
        {
            //Debug.Log($"【MockManager02】： {rpcName} = {jsn8.text}");
            return new List<object> { null, jsn8.text };
        }
        else
        {
            return new List<object> { $"【MockManager02】：找不到文件 {rpcName}" };
        }

    }
    public IEnumerator HttpPost(string url, string method, Dictionary<string, string> post_param, System.Action<string> callback)
    {

        yield return null;

        var rpcName = method.Replace("/", "__");

        TextAsset jsn8 = Resources.Load<TextAsset>("tempdata/keno_play_response_v1");
        if (jsn8 != null && jsn8.text != null)
        {
            Debug.Log($"【MockManager02】： {rpcName} = {jsn8.text}");
            callback?.Invoke(jsn8.text); // 调用回调函数，传递响应数据
        }
        else
        {
            Debug.LogError($"【MockManager02】：找不到文件 {rpcName}");
        }
    }


    /// <summary>
    /// 特殊轮数据
    /// </summary>
    SpecTurnDataInfo specTurnInfo = new SpecTurnDataInfo();
}


/// <summary>
/// 
/// </summary>
/// <remarks>
/// * 普通游戏、特殊游戏（单局小游戏、多局小游戏、单局免费游戏、多局免费游戏）、中奖游戏、
/// * ko 本局大奖
/// * spec 特殊游戏
/// </remarks>
public  class SpecTurnDataInfo
{


    /// <summary>
    /// 本地的假数据存储
    /// g31__1747129782087_0__slot_spin__bonus3103_0__spec__ko.json
    /// </summary>
    /// <param name="?">int : 游戏id  （31）</param>
    /// <param name="?">string: 那一轮  1747129782087 </param>
    /// <param name="?">List<string>:   文件名[不带.json]  g31__1747129782087_0__slot_spin__bonus3103_0__spec__ko</param>
    /// <returns>String</returns>
    public Dictionary<int, Dictionary<string, List<string>>> specTurn;


    public  void Analysis(int gameId, string fileName)
    {
        if (fileName.Contains("__ko")|| fileName.Contains("__spec"))
        {

            string[] result = Regex.Split(fileName, "__");
            //string turn = result[2]; // 1747130101002_0
            string[] turnNumber = Regex.Split(result[2], "_"); // "1747130101002" "0"

            if (!specTurn.ContainsKey(gameId))
            {
                specTurn.Add(gameId, new Dictionary<string, List<string>>());
            }

            if (specTurn[gameId].ContainsKey(turnNumber[0]))
            {
                specTurn[gameId].Add(turnNumber[0], new List<string>());
            }

            if (!specTurn[gameId][turnNumber[0]].Contains(fileName))
            {
                specTurn[gameId][turnNumber[0]].Insert( int.Parse(turnNumber[1]),fileName);
            }
        }
    }

    public bool isSpecTurn(string fileName)
    {
        string[] result = Regex.Split(fileName, "__");
        int gameId = int.Parse(result[1].Replace("g",""));
        //string turn = result[2]; // 1747130101002_0
        string[] turnNumber = Regex.Split(result[2], "_"); // "1747130101002" "0"

        return specTurn[gameId].ContainsKey(turnNumber[0]);
    }

}