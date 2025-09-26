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

    public Action<object> onConnected { set { this._onConnected = value; } }         // ���ӻص�
    public Action<object> onMessage { set { this._onMessage = value; } }         // ��Ϣ�ص� onMessage: (event: any)
    public Action<object> onError { set { this._onError = value; } }          // ����ص�
    public Action<object> onClosed { set { this._onClosed = value; } }           // �رջص�





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
            Debug.Log($"��ȡ������Ϸ�������� �� {jsn.text}");
            JSONNode node = JSONNode.Parse(jsn.text);

        }
    }





    Dictionary<int, Dictionary<string, GameData>> allData = new Dictionary<int, Dictionary<string, GameData>>();

    //public bool isKo = true;



    public void testD()
    {
        var anonymousObj = new { name = "С��", age = 30 };

        // ��ȡ�������������  
        Type anonymousType = anonymousObj.GetType();

        // ��ȡ name �ֶε�ֵ  
        PropertyInfo nameProperty = anonymousType.GetProperty("name");
        string nameValue = (string)nameProperty.GetValue(anonymousObj);
        Debug.Log($"Name: {nameValue}");

        // ��ȡ age �ֶε�ֵ  
        PropertyInfo ageProperty = anonymousType.GetProperty("age");
        int ageValue = (int)ageProperty.GetValue(anonymousObj);
        Debug.Log($"Age: {ageValue}");
    }


    /// <summary>
    /// ���صļ����ݴ洢
    /// g8980__1747108331466__keno_spin__ko_21231.json
    /// </summary>
    /// <param name="?">int : ��Ϸid  ��8980��</param>
    /// <param name="?">string: Э������ ��keno_spin��</param>
    /// <param name="?">List<KeyValuePair<string, string>>>:   �ļ���[����.json] - �ļ�����  =  ��g8980__keno_spin__80808 ��  json string�� </param>
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
                res += $"��ERR��:��Ϸdi={gId}������ԴΪ��\n";
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

                // ��������
                this.jsonLst[gId][rpc].Add(new KeyValuePair<string, string>(json.name, json.text));//��������
                

                // ������Ϸ
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
        Debug.Log(" ������ mock socket��");


        this.ClearTime();
        this._timer = new System.Timers.Timer(500);
        this._timer.AutoReset = false; // �Ƿ��ظ�ִ��
        this._timer.Enabled = true; //�Ƿ�ִ��Elapsed�¼�
        this._timer.Elapsed += (object sender, ElapsedEventArgs e) => {
            this.ClearTime();
            //this._Connect();  //���������̣߳�����ʹ��LoadAll������Դ
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
        }); //���ع�����
    }

    public void Close()
    {
        this.jsonLst = new Dictionary<int, Dictionary<string, List<KeyValuePair<string, string>>>>();
        if (this._onClosed != null) this._onClosed(null);
    }


    /// <summary> ��ǰ������Ϸ�ı�ţ�ʱ����� </summary>
    string curSpecTurnNumber = "";

    /// <summary> ��ǰ������Ϸ���ĵڼ������� </summary>
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
                this._timer.AutoReset = false; // �Ƿ��ظ�ִ��
                this._timer.Enabled = true; //�Ƿ�ִ��Elapsed�¼�
                this._timer.Elapsed += (object sender, ElapsedEventArgs e) => {
                    this.ClearTime();
                    /*this.task = () =>
                    {
                        if (fileName != null)
                            Debug.Log($"@��mock ���С�  rpcKey = {rpcKey} , fileName =  {fileName}");
                        if (this._onMessage != null)
                            this._onMessage(res);
                    };*/
                    taskQueue.Enqueue(() =>
                    {
                        if (fileName != null)
                            Debug.Log($"@��mock ���С� , fileName =  {fileName}");
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


            // ʹ�ô�������
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

            // ����ʹ����Ϸ����
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
            this._timer.AutoReset = false; // �Ƿ��ظ�ִ��
            this._timer.Enabled = true; //�Ƿ�ִ��Elapsed�¼�
            this._timer.Elapsed += (object sender, ElapsedEventArgs e) => {
                this.ClearTime();
                /*this.task = () =>
                {
                    if(fileName != null)
                        Debug.Log($"@��mock ���С�  rpcKey = {rpcKey} , fileName =  {fileName}");
                    if (this._onMessage != null)
                        this._onMessage(res);
                };*/
                taskQueue.Enqueue(() =>
                {
                    if (fileName != null)
                        Debug.Log($"@��mock ���С�  , fileName =  {fileName}");
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
            //Debug.Log($"��MockManager02���� {rpcName} = {jsn8.text}");
            return new List<object> { null, jsn8.text };
        }
        else
        {
            return new List<object> { $"��MockManager02�����Ҳ����ļ� {rpcName}" };
        }

    }
    public IEnumerator HttpPost(string url, string method, Dictionary<string, string> post_param, System.Action<string> callback)
    {

        yield return null;

        var rpcName = method.Replace("/", "__");

        TextAsset jsn8 = Resources.Load<TextAsset>("tempdata/keno_play_response_v1");
        if (jsn8 != null && jsn8.text != null)
        {
            Debug.Log($"��MockManager02���� {rpcName} = {jsn8.text}");
            callback?.Invoke(jsn8.text); // ���ûص�������������Ӧ����
        }
        else
        {
            Debug.LogError($"��MockManager02�����Ҳ����ļ� {rpcName}");
        }
    }


    /// <summary>
    /// ����������
    /// </summary>
    SpecTurnDataInfo specTurnInfo = new SpecTurnDataInfo();
}


/// <summary>
/// 
/// </summary>
/// <remarks>
/// * ��ͨ��Ϸ��������Ϸ������С��Ϸ�����С��Ϸ�����������Ϸ����������Ϸ�����н���Ϸ��
/// * ko ���ִ�
/// * spec ������Ϸ
/// </remarks>
public  class SpecTurnDataInfo
{


    /// <summary>
    /// ���صļ����ݴ洢
    /// g31__1747129782087_0__slot_spin__bonus3103_0__spec__ko.json
    /// </summary>
    /// <param name="?">int : ��Ϸid  ��31��</param>
    /// <param name="?">string: ��һ��  1747129782087 </param>
    /// <param name="?">List<string>:   �ļ���[����.json]  g31__1747129782087_0__slot_spin__bonus3103_0__spec__ko</param>
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