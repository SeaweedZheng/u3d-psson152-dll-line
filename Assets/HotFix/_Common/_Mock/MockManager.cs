using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Timers;
using UnityEngine;
using JSONNode = SimpleJSON.JSONNode;
using Random = System.Random;
using GameMaker;



#region GM 指令数据局
public partial class MockManager : MonoSingleton<MockManager>
{
    [System.Serializable]
    public class MockSpecialDataInfo
    {
        public string rpcName;
        public string gmName; // "mock_gm_5kind__g105__slot_spin",
        /// <summary> 多文件的文件名 </summary>
        public List<string> flies;
        /// <summary> 多文件数据时，当前使用文件的索引</summary>
        public int index;

    }

    /// <summary> 当前已经在运行的特殊局游戏 </summary>
    public Dictionary<string, MockSpecialDataInfo> gmDataRunning = new Dictionary<string, MockSpecialDataInfo>();

    /// <summary> “特殊局数据信息”和“对应的gm指令”</summary>
    public Dictionary<string, List<MockSpecialDataInfo>> gmDataLst = new Dictionary<string, List<MockSpecialDataInfo>>();

    string mockGM = "";//"5kind";   mock_gm:5kind#mock_ko


    [Button]
    void TestShowGmDataInfo()
    {
        DebugUtils.Log(JSONNodeUtil.ObjectToJsonStr(gmDataLst));
    }

    private bool IsGMData(string fileName)
    {
        for (int i=0; i< gmDataLst.Count; i++)
        {
            var item = gmDataLst.ElementAt(i);
            for (int j=0; j< item.Value.Count; j++)
            {
                if (item.Value[j].flies.Contains(fileName))
                {
                    return true;
                }
            }
        }
        return false;
    }


    /// <summary>
    /// 获取GM-Spin指令中的Spin数据
    /// </summary>
    /// <param name="rpcKey"></param>
    /// <returns></returns>
    /// <remarks>
    /// "gm:spin#file_name:g152__RESULT_RECALL__7#spin_data:<xmp>{\"msgid\":107,\"status_code\":0,\"result\":{\"module_id\":\"BS\",\"credit\":{\"low\":1220,\"high\":0,\"unsigned\":true},\"rng\":[59,63,62,17,62],\"win_line_group\":[{\"win_line_type\":0,\"line_no\":1,\"symbol_id\":4,\"pos\":[11,31,41,12,22,32,3,13,23],\"credit\":1200,\"multiplier\":12,\"credit_long\":{\"low\":1200,\"high\":0,\"unsigned\":true}},{\"win_line_type\":0,\"line_no\":2,\"symbol_id\":7,\"pos\":[21,31,2,13,33],\"credit\":20,\"multiplier\":2,\"credit_long\":{\"low\":20,\"high\":0,\"unsigned\":true}}],\"multiplier_alone\":1,\"mulitplier_pattern\":[1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1],\"random_syb_pattern\":[],\"bonus_multiplier\":null,\"win_bonus_group\":[],\"be_locked_pattern\":[],\"position_pay\":[],\"reel_stack_pay\":[],\"golden_wild_flag\":[],\"pay_of_scatter\":[],\"capture_award\":null,\"win_line_multiple\":null,\"mystery\":null,\"jp\":null,\"overlap\":[],\"pay_of_pos\":[],\"golden_icon\":[],\"exp_wild\":[],\"pre_exp_wild\":[],\"trigger_respin_times\":null,\"push_wild\":[],\"typed_wild\":null,\"sub_result\":[],\"icon_accumulate\":null,\"scatter_type\":[],\"pre_scatter_type\":[],\"full_pay\":null,\"block_reel_index\":null,\"trigger_super_scatter\":[],\"strip_index\":null,\"cascade_result\":[],\"random_bonus_times\":null,\"bonus_multiplier_list\":[],\"bonus_multiplier_index\":null,\"col_cascade_count\":[],\"external_multiplier\":1,\"pre_no_win_acc\":null,\"no_win_acc\":null,\"respin_types\":[],\"respin_costs\":[],\"jackpot_rng\":null,\"jackpot_type\":null,\"capture_award_list\":[],\"capture_award_index\":null,\"golden_scatter_flag\":[],\"full_symbol\":null,\"pay_of_star\":[],\"collect_counter\":null,\"cur_collect_counter\":null,\"upgrade_id\":[],\"change_symbol_id\":null,\"full_symbol_pattern\":[],\"avg_bet\":null,\"trigger_bonus_total_bet\":null,\"respin_reels\":[],\"cent_in_ask\":[],\"next_strip_index\":null,\"bonus_bet_list\":[],\"last_player_opt_index\":null,\"cur_star_counts\":[],\"total_star_times\":[],\"bonus_star_times\":[],\"cur_random_prize\":[],\"pool_info\":null,\"crush_syb_pattern\":[],\"bonus_symbol_pos\":null,\"arcade_mario_bar\":null,\"race_game_data\":null,\"coin_pusher_data\":null,\"arcade_monopoly\":null,\"player_data\":null,\"village_infor\":null,\"arcade_football\":null,\"arcade_tamagotchi\":null,\"record_list\":[]},\"player_cent\":{\"low\":1012370,\"high\":0,\"unsigned\":true},\"next_module\":\"BS\",\"cur_module_play_times\":1,\"cur_module_total_times\":1,\"member_info\":null,\"ups_data\":null,\"marquee_data\":[]}</xmp>"
    /// </remarks>
    private List<string> GetGMSpinData(string rpcKey)
    {
        List<string> lst = new List<string>();
        string gm = TestManager.Instance.GetValue("gm");
        if (!string.IsNullOrEmpty(gm) && gm == "spin" && rpcKey.Contains(RPCName.slotSpin)) // &&
        {
            gm = TestManager.Instance.GetValueOnce("gm");
            lst = new List<string>()
            {
                TestManager.Instance.GetValueOnce("spin_data"),
                TestManager.Instance.GetValueOnce("file_name"),
            };
        }
        return lst;
    }


    /// <summary>
    /// 从gm指令中获取特殊局类型，并获取对应的文件的数据
    /// </summary>
    /// <param name="rpcKey"></param>
    /// <returns></returns>
    private string GetMockGMDataFileName(string rpcKey) //"g105__slot_spin"  "g152__RESULT_RECALL__7"
    {
        if (string.IsNullOrEmpty(mockGM))
        {
            mockGM = TestManager.Instance.GetValueOnce("mock_gm");
            if (string.IsNullOrEmpty(mockGM))
            {
                return null;
            }
        }

        DebugUtils.Log($"@== 当前 mockGM = {mockGM}");

        string gmKey = $"mock_gm_{mockGM}__{rpcKey}";
        if (!gmDataLst.ContainsKey(gmKey))
        {
            mockGM = "";
            return null;
        }

        if (gmDataRunning.ContainsKey(gmKey))
        {
            gmDataRunning[gmKey].index++;

            if(gmDataRunning[gmKey].index >= gmDataRunning[gmKey].flies.Count)
            {
                gmDataRunning[gmKey].index = -1;
                gmDataRunning.Remove(gmKey);
                mockGM = "";
                return null;
            }

            string fileName = gmDataRunning[gmKey].flies[gmDataRunning[gmKey].index];
            return fileName;
        }

        var index = UnityEngine.Random.Range(0, gmDataLst[gmKey].Count);

        var target = gmDataLst[gmKey][index];

        if (target.flies.Count ==0) //没有文件
        {
            mockGM = "";
            return null;
        }
        else if (target.flies.Count == 1)//单个文件
        {
            target.index = -1;
            mockGM = "";
            return target.flies[0];
        }

        //多文件
        target.index = 0;
        gmDataRunning.Add(gmKey, target);
        return target.flies[0];

    }


    /// <summary>
    /// 获取GM指令对应的特殊局，数据文件名信息
    /// </summary>
    /// <param name="gameid"></param>
    private void AddGMDataConfig(int gameid)
    {


#if false
        TextAsset jsn = Resources.Load<TextAsset>($"GM Mock/mock_gm_g{gameid}");  //
#else
        //TextAsset jsn = ResourceManager.Instance.LoadD<TextAsset>($"Assets/GameRes/_Common/Game Maker/ABs/Datas/{152}/GM Mock/mock_gm_g{152}.json");

        string path = ConfigUtils.GetGMMockData(ConfigUtils.curGameId);
        TextAsset jsn = ResourceManager.Instance.LoadAssetAtPathOnce<TextAsset>(path);
#endif


        DebugUtils.Log(jsn.text);

        if (jsn == null)
        {
            return;
        }

        JSONNode node = JSONNode.Parse(jsn.text);

        foreach(KeyValuePair<string,JSONNode> kv in node["mock_special_data"])
        {
            string gm = kv.Key;
            string gmNickName = $"mock_gm_{gm}__g{gameid}";

            foreach (JSONNode nd in kv.Value)
            {
                string rpc = (string)nd["rpc_name"];
                string gmKey = $"{gmNickName}__{rpc}";  // "mock_gm_5kind__g105__slot_spin",

                if (!gmDataLst.ContainsKey(gmKey))
                    gmDataLst.Add(gmKey, new List<MockSpecialDataInfo>());

                List<string> files = new List<string>();
                foreach (JSONNode nd1 in nd["file"])
                {
                    files.Add(((string)nd1).Replace(".json", ""));
                }

                MockSpecialDataInfo data = new MockSpecialDataInfo()
                {
                    rpcName = rpc,
                    gmName = gmKey,
                    index = -1,
                    flies = files,
                };

                gmDataLst[gmKey].Add(data);
            }
        }
    }

}

#endregion

/// <summary>
/// 设定假数据
/// </summary>
public partial class MockManager : MonoSingleton<MockManager>, ISocket
{


    /// <summary> 当前执行特殊局，到第几个文件 </summary>
    Dictionary<string, int> designatedRpcIdx = new Dictionary<string, int>();


    /// <summary> 脚本写死的特殊局文件名数组 </summary>
    Dictionary<string, List<string>> designatedRpc = new Dictionary<string, List<string>>
    {
        { "g39__slot_spin",
            new List<string>{
                "g39__slot_spin__ko_SuperMegaWin_1712632840000",
                //"g39__slot_spin__ko_BigWin_1712647744580",
                //"g39__slot_spin__ko_SuperBigWin_1712647935793",
                //"g39__slot_spin__ko_BigWheel_1712648713321",
                //"g39__slot_spin__ko_1712581969773",
                //"g39__slot_spin__ko_1712582409715",
                //"g39__slot_spin__ko_1712583306724",
                //"g39__slot_spin__ko_1712583486348"
            }
        },
        { "g21__slot_spin",
            new List<string>{
                "g21__slot_spin__ko_WheelBonus_1712660948440",

                /*
                "g21__slot_spin__ko_fp0_2157_1711539772157",
                "g21__slot_spin__ko_fp1_2157_1711539783770",
                "g21__slot_spin__ko_fp2_2157_1711539787354",
                "g21__slot_spin__ko_fp3_2157_1711539792971",
                "g21__slot_spin__ko_fp4_2157_1711539794657",
                "g21__slot_spin__ko_fp5_2157_1711539796381",
                "g21__slot_spin__ko_fp6_2157_1711539800114",
                "g21__slot_spin__fp7_2157_1711539803942",
                "g21__slot_spin__fp8_2157_1711539805617",
                "g21__slot_spin__fp9_2157_1711539807426",
                "g21__slot_spin__fp10_2157_1711539811166",
                "g21__slot_spin__fp11_2157_1711539816774",
                "g21__slot_spin__fp12_2157_1711539822409",
                "g21__slot_spin__fp13_2157_ko_1711539825887"
                */

                //"g21__slot_spin__ko_MiniJackpot_1712714065647",
                //"g21__slot_spin__ko_WheelBonus_MegaWin_1712667997205",

                //"g21__slot_spin__ko_GrandJackpot_1712718100657",
                //"g21__slot_spin__ko_MinorJackpot_1712718888493",
            }
        },
        { "g93__slot_spin",
            new List<string>{
                    //"g93__slot_spin__ko_FreeGame_1712722136850",
                    "g93__slot_spin__ko_fp666_0_17132647776460",
                    "g93__slot_spin__ko_fp666_1_17132647776460",
                    "g93__slot_spin__ko_fp666_2_17132647776460",
                    "g93__slot_spin__ko_fp666_3_17132647776460",
                    "g93__slot_spin__ko_fp666_4_17132647776460",
             }
        },
        { "g7__slot_spin",
            new List<string>{
                    "g7__slot_spin__ko_DiceGame_1712721072136",
             }
        },
        { "g40__slot_spin",
            new List<string>{
                    "g40__slot_spin__ko_fp88962_0_1712735188962",
                    "g40__slot_spin__fp88962_1_1712735204138",
                    "g40__slot_spin__fp88962_2_1712735206062",
                    "g40__slot_spin__fp88962_3_1712735208008",
                    "g40__slot_spin__fp88962_4_1712735218631",
                    "g40__slot_spin__fp88962_5_1712735229216",
                    "g40__slot_spin__fp88962_6_1712735235716",
                    "g40__slot_spin__fp88962_7_1712735237696",
                    "g40__slot_spin__ko_fp88962_8_1712735239614",
                    "g40__slot_spin__fp88962_9_1712735250774",
                    "g40__slot_spin__fp88962_10_1712735354546",
             }
        },
        { "g37__slot_spin",
            new List<string>{
                    "g37__slot_spin__ko_Big_1712802479683",

                    /*"g37__slot_spin__ko_fp98080_0_1712741698080",
                    "g37__slot_spin__ko_fp98080_1_1712741861451",
                    "g37__slot_spin__ko_fp98080_2_1712741867822",
                    "g37__slot_spin__ko_fp98080_3_1712741872997",
                    "g37__slot_spin__ko_fp98080_4_1712741878089",
                    "g37__slot_spin__ko_fp98080_5_1712741880224",
                    "g37__slot_spin__ko_fp98080_6_1712741885420",*/
             }
        },
        { "g28__slot_spin",
            new List<string>{
                    "g28__slot_spin__ko_EpicWin_1712807892872",
                    "g28__slot_spin__ko_EpicWin_1712803902181",
                    "g28__slot_spin__ko_fp33829_0_1712807233829",
                    "g28__slot_spin__ko_fp33829_1_1712807422676",
                    "g28__slot_spin__ko_fp33829_2_1712807424438",
                    "g28__slot_spin__ko_fp33829_3_1712807426173",
                    "g28__slot_spin__ko_fp33829_4_1712807427917",
                    "g28__slot_spin__ko_fp33829_5_1712807433742",
                    "g28__slot_spin__ko_fp33829_6_1712807435475",
                    "g28__slot_spin__fp33829_7_1712807437241",
                    "g28__slot_spin__fp33829_8_1712807441041",
                    "g28__slot_spin__fp33829_9_1712807442753",
                    "g28__slot_spin__fp33829_10_1712807444494"
             }
        },
        { "g33__slot_spin",
            new List<string>{
                    "g33__slot_spin__ko_SuperMegaWin_1712807892872",
             }
        },
        { "g43__slot_spin",
            new List<string>{
                    "g43__slot_spin__ko_EpicWin_1712824314281",
                    "g43__slot_spin__ko_SuperMegaWin_1712823463785",
                    "g43__slot_spin__ko_MegaWin_1712807892872",
                    "g43__slot_spin__ko_SuperMegaWin_1712822164229",
                    "g43__slot_spin__ko_SuperBigWin_1712822102110",
             }
        },
        { "g105__slot_spin",
            new List<string>{
                   "g105__slot_spin__ko_1712907230816",
                   //  "g105__slot_spin__ko_change_1712907230816",
             }
        },
        { "g92__slot_spin", //小猪
            new List<string>{
                    "g92__slot_spin__ko_MinGame_1712912499719",
             }
        },
        { "g116__slot_spin",
            new List<string>{
                    "g116__slot_spin__ko_MinGame_1712914970499",

                   /* "g116__slot_spin__ko_fp54510_0_1712916054510",
                    "g116__slot_spin__fp54510_1_1712917680714",
                    "g116__slot_spin__fp54510_2_1712917684923",
                    "g116__slot_spin__fp54510_3_1712917686881",
                    "g116__slot_spin__ko_fp54510_4_1712917690770",
                    "g116__slot_spin__fp54510_5_1712917696209",
                    "g116__slot_spin__ko_fp54510_6_1712917700108",
                    "g116__slot_spin__ko_fp54510_7_1712917706063",
                    "g116__slot_spin__fp54510_8_1712917712986",
                    "g116__slot_spin__fp54510_9_1712917716877",
                    "g116__slot_spin__fp54510_10_1712917720798",
                    "g116__slot_spin__fp54510_11_1712917724716",
                    "g116__slot_spin__ko_fp54510_12_1712917728623",*/
             }
        },
        { "g149__slot_spin",
            new List<string>{
                    /*
                     * 免费游戏
                    "g149__slot_spin__ko_FreeGame_fp25245_0_1712996695031",
                    "g149__slot_spin__ko_FreeGame_fp25245_1_1712996695031",
                    "g149__slot_spin__ko_FreeGame_fp25245_2_1712996695031",
                    "g149__slot_spin__ko_FreeGame_fp25245_3_1712996695031",
                    "g149__slot_spin__ko_FreeGame_fp25245_4_1712996695031",
                    "g149__slot_spin__ko_FreeGame_fp25245_5_1712996695031",
                    "g149__slot_spin__ko_FreeGame_fp25245_6_1712996695031",
                    "g149__slot_spin__ko_FreeGame_fp25245_7_1712996695031",
                    "g149__slot_spin__ko_FreeGame_fp25245_8_1712996695031",*/


                    /* 选择免费游戏 */

                    "g149__slot_spin__ko_FreeGameSelect_fp60297_0_1713168660297",
                    "g149__slot_spin__ko_fp60297_1_1713168894410",
                    "g149__slot_spin__fp60297_2_1713168901079",
                    "g149__slot_spin__fp60297_3_1713168902854",
                    "g149__slot_spin__fp60297_4_1713168904626",
                    "g149__slot_spin__fp60297_5_1713168908435",
                    "g149__slot_spin__fp60297_6_1713168912328",
                    "g149__slot_spin__fp60297_7_1713168914083",
                    "g149__slot_spin__fp60297_8_1713168920863",
             }

        },
        { "g142__slot_spin",
            new List<string>{
                    "g142__slot_spin__ko_MiniGame_1713167472052",
             }
        },
        { "g183__slot_spin",
            new List<string>{
                    "g183__slot_spin__ko_fp16821_0_1713173316821",
                    "g183__slot_spin__ko_fp16821_1_1713173444961",
                    "g183__slot_spin__ko_fp16821_2_1713173447170",
                    "g183__slot_spin__ko_fp16821_3_1713173449376",
                    "g183__slot_spin__ko_fp16821_4_1713173453609",
                    "g183__slot_spin__fp16821_5_1713173458870",
                    "g183__slot_spin__ko_fp16821_6_1713173464091",
                    "g183__slot_spin__fp16821_7_1713173471367",
                    "g183__slot_spin__ko_fp16821_8_1713173482645",
             }
        },
        { "g103__slot_spin",
            new List<string>{
                    /*"g103__slot_spin__ko_MiniGame_1713236744938",*/

                    
                    "g103__slot_spin__ko_fp34505_0_1713236134506",
                    "g103__slot_spin__ko_fp34505_1_1713236196681",
                    "g103__slot_spin__ko_fp34505_2_1713236200697",
                    "g103__slot_spin__ko_fp34505_3_1713236204659",
                    "g103__slot_spin__ko_fp34505_4_1713236208663",
                    "g103__slot_spin__ko_fp34505_5_1713236212685",
                    "g103__slot_spin__ko_fp34505_6_1713236216810",
                    "g103__slot_spin__ko_fp34505_7_1713236220754",
                    "g103__slot_spin__fp34505_8_1713236224796",
                    "g103__slot_spin__fp34505_9_1713236228723",
                    "g103__slot_spin__fp34505_10_1713236232743",

             }
        },
        { "g65__slot_spin",
            new List<string>{
                    "g65__slot_spin__ko_Big_01",
             }
        }

    };


}





/// <summary>
/// ISocket + Loom
/// </summary>
public partial class MockManager: MonoSingleton<MockManager>, ISocket
{

    /// <summary>
    /// 只要ko的数据
    /// </summary>
    [Tooltip("只要ko的数据")]
    public bool isKo = false;

    Action<object> _onConnected = null;
    Action<object> _onMessage = null;
    Action<object> _onError = null;
    Action<object> _onClosed = null;

    public Action<object> onConnected { set { this._onConnected = value; } }         // 连接回调
    public Action<object> onMessage { set { this._onMessage = value; } }         // 消息回调 onMessage: (event: any)
    public Action<object> onError { set { this._onError = value; } }          // 错误回调
    public Action<object> onClosed { set { this._onClosed = value; } }           // 关闭回调



    #region ISocket

    /// <summary>
    /// 本地的假数据存储
    /// g8980__keno_spin__ko_21231.json
    /// </summary>
    /// <param name="?">int : 游戏id  （8980）</param>
    /// <param name="?">string: 协议名称 （keno_spin）</param>
    /// <param name="?">List<KeyValuePair<string, string>>>:   文件名[不带.json] - 文件数据  =  （g8980__keno_spin__80808 ，  json string） </param>
    /// <returns>String</returns>
    Dictionary<int, Dictionary<string, List<KeyValuePair<string, string>>>> jsonLst = new Dictionary<int, Dictionary<string, List<KeyValuePair<string, string>>>>();
    //private async void LoadJsonAsync(int gameid, Action<object> onComplete) {


    /// <summary>
    /// 加载本地的假数据
    /// </summary>
    /// <param name="gameid">游戏id</param>
    /// <param name="onComplete">加载完成回调</param>
    /// <returns>void</returns>
    private void LoadJsonAsync(int gameid, Action<object> onComplete)
    {
        Dictionary<int,string> load = new Dictionary<int, string>();
        if (!this.jsonLst.ContainsKey(-1))
        {
            load.Add(-1,"mock/g/");
        }

        if (!this.jsonLst.ContainsKey(gameid) && gameid != -1)
        {
            load.Add(gameid, $"mock/g{gameid}/");
            AddGMDataConfig(gameid);
        }

        if (load.Count == 0)
        {
            return;
        }


        string res = null;
        foreach (var kv in load)
        {
            //g__login__4234.json
            //g8980__keno_spin__ko_21231.json
            //g8980__keno_spin__80808.json

            int _gameid = kv.Key;

            TextAsset[] jsons = Resources.LoadAll<TextAsset>(kv.Value);  //
            if (jsons == null || jsons.Length == 0)
            {
                res += $"【ERR】:游戏di={_gameid}的数据源为空\n";
            }

            foreach (TextAsset json in jsons)
            {
                //string input = "apple===banana---orange+++grape";
                //string pattern = @"[__]{2,}";
                //string[] result = Regex.Split(input, pattern);

                string[] result = Regex.Split(json.name, "__");  //g152__RESULT_RECALL__1
                string rpc = result[1];

                if (!this.jsonLst.ContainsKey(_gameid))
                {
                    this.jsonLst.Add(_gameid, new Dictionary<string, List<KeyValuePair<string, string>>>() );
                }

                if (!this.jsonLst[_gameid].ContainsKey(rpc))
                {
                    this.jsonLst[_gameid].Add(rpc, new List<KeyValuePair<string, string>>()); 
                }

                if(this.isKo && this.jsonLst[_gameid][rpc].Count > 0){
                    if (json.name.IndexOf("__ko") > 0)
                    {
                        this.jsonLst[_gameid][rpc].Add( new KeyValuePair<string, string>(json.name, json.text)); //只要ko数据和至少一个任意数据
                    }
                }
                else
                {
                    this.jsonLst[_gameid][rpc].Add(new KeyValuePair<string, string>(json.name, json.text));//任意数据
                }
            }   

        }

        if (onComplete != null)
        {
            //onComplete.Invoke();??
            onComplete(res);
        }

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
        }
    }

    public bool Connect(NetConnectOptions options)
    {
        DebugUtils.Log(" 【链接 mock socket】");

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
        this.jsonLst = new Dictionary<int, Dictionary<string, List<KeyValuePair<string, string>>>> ();

        this.LoadJsonAsync(-1, (res) =>
        {
            if (res != null)
            {
                DebugUtils.LogError(res);
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



    public const string KEY_RPC_NAME = "protocol_key";
    public const string KEY_RPC_DATA = "data";
    public const string KEY_RPC_CODE = "err";
    public const string KEY_RPC_MESSAGE = "msg";
    public const string KEY_RPC_SEQID = "seq_id";

    public int Send(object message)
    {
        JSONNode node = JSONNode.Parse(message as string);

        var rpcName = (string)node[KEY_RPC_NAME];

        //string res = $"\"protocol_key\":\"{rpcName}\"," +  "\"data\":{\"err\":404}}";

        int seq_id = -1;
        Match match = Regex.Match((string)message, "\"seq_id\":\\s*(\\d+)");
        if (match.Success)
        {
            string str = match.Groups[1].Value;// 提取数字  
            seq_id = int.Parse(str);
        }

        //res = string.Format("{{\"err\":408,\"msg\":\"请求超时\",\"seq_id\":{0}}}", seq_id);

        string res;
        //res = string.Format("{{\"protocol_key\":\"{0}\",\"data\":{\"err\":408,\"msg\":\"请求超时\",\"seq_id\":{1}}}}", rpcName, seq_id);  //不支持嵌套！
        string data = string.Format("{{\"err\":408,\"msg\":\"请求超时\",\"seq_id\":{0}}}", seq_id);  
        res = string.Format("{{\"protocol_key\":\"{0}\",\"data\":{1}}}", rpcName, data);

        //DebugUtil.Log(res);

        string fileName = null;

        if (rpcName == RPCName.enterGame)
        {
            this._gameId = node["data"]["game_id"];

            //进入游戏房间时，先加载游戏数据
            this._ConnectGame(this._gameId, (err) =>
            {
                if (err != null)
                    return;

                var message_game = this.jsonLst[this._gameId];
                string rpcKey = $"g{this._gameId}__{rpcName}";

                if (message_game.ContainsKey(rpcName))
                {
                    var lst = message_game[rpcName];

                    // 随机获取数据
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




                    // 获取设定的数据
                    if (designatedRpc.ContainsKey(rpcKey))
                    {

                        //KeyValuePair<string, string> kv = lst.Find(item => item.Key == designatedRpc[rpcKey]);

                        if (!designatedRpcIdx.ContainsKey(rpcKey))
                        {
                            designatedRpcIdx.Add(rpcKey, 0);
                        }
                        var idx = designatedRpcIdx[rpcKey];

                        string fileName2 = designatedRpc[rpcKey][idx];

                        KeyValuePair<string, string> kv = lst.Find(item => item.Key == fileName2);

                        if (++idx >= designatedRpc[rpcKey].Count)
                            idx = 0;
                        designatedRpcIdx[rpcKey] = idx;


                        if (!string.IsNullOrEmpty(kv.Key))
                        {
                            res = kv.Value;
                            fileName = kv.Key;
                        }
                    }
           
                }

                res = CheckResponseFormat(rpcName,res, seq_id);

                DelayResponseRPC(rpcKey, fileName, res);
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
            string rpcKey = null;

            if (rpcName == RPCName.ping)
            {
                rpcKey = $"g__{rpcName}";
            }
            else
            {
                rpcKey = this._gameId == -1 ? $"g__{rpcName}" : $"g{this._gameId}__{rpcName}";
            }


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

                if (designatedRpc.ContainsKey(rpcKey))
                {
                    if (!designatedRpcIdx.ContainsKey(rpcKey))
                    {
                        designatedRpcIdx.Add(rpcKey, 0);
                    }
                    var idx = designatedRpcIdx[rpcKey];

                    KeyValuePair<string, string> kv = lst.Find(item => item.Key == designatedRpc[rpcKey][idx]);

                    if (++idx >= designatedRpc[rpcKey].Count)
                        idx = 0;
                    designatedRpcIdx[rpcKey] = idx;

                    if (!string.IsNullOrEmpty(kv.Key))
                    {
                        res = kv.Value;
                        fileName = kv.Key;
                    }
                }
            }


            //有重复的代码以游戏房间数据优先
            if (message_game.ContainsKey(rpcName))
            {
                var lst = message_game[rpcName];


                bool isGMSpin = false; // 直接从GM中拿到spin的数据，
                bool isGMMock = false; // 直接从GM中拿到moke的特殊局数据类型

                List<string> spinInfo = GetGMSpinData(rpcKey);
                if (spinInfo.Count>0)
                {
                    res = spinInfo[0];
                    fileName = spinInfo[1];
                    isGMSpin = true;
                }
                else
                {
                    // 获取GM的数据
                    string fn1 = GetMockGMDataFileName(rpcKey);
                    if (!string.IsNullOrEmpty(fn1))
                    {
                        KeyValuePair<string, string> kv = lst.Find(item => item.Key == fn1);
                        if (!string.IsNullOrEmpty(kv.Key))
                        {
                            res = kv.Value;
                            fileName = kv.Key;
                            isGMMock = true;
                            DebugUtils.Log("==@ 使用了Mock GM");
                        }
                    }
                }

                
                if (!isGMMock && !isGMSpin)
                {
                    if (lst.Count > 1)
                    {
                        /*if (isSpecial) {
                        }
                        else
                        {
                        }*/
                        do { 
                            Random random = new Random();
                            var index = random.Next(0, lst.Count);
                            res = lst[index].Value;
                            fileName = lst[index].Key;                        
                        } while ( IsGMData(fileName)  || fileName.Contains("__spec") );  //游戏房间里， 去除特殊数据，比如大奖 ，免费游戏
                    }
                    else
                    {
 
                        res = lst[0].Value;
                        fileName = lst[0].Key;
                    }

                    if (designatedRpc.ContainsKey(rpcKey))
                    {
                        if (!designatedRpcIdx.ContainsKey(rpcKey))
                        {
                            designatedRpcIdx.Add(rpcKey, 0);
                        }
                        var idx = designatedRpcIdx[rpcKey];

                        KeyValuePair<string, string> kv = lst.Find(item => item.Key == designatedRpc[rpcKey][idx]);

                        if (++idx >= designatedRpc[rpcKey].Count) //if (idx++ >= designatedRpc[rpcKey].Count)会报错
                            idx = 0;
                        designatedRpcIdx[rpcKey] = idx;

                        if (!string.IsNullOrEmpty(kv.Key))
                        {
                            res = kv.Value;
                            fileName = kv.Key;
                        }
                    }
                }

            }

            res = CheckResponseFormat(rpcName, res, seq_id);

            DelayResponseRPC(rpcKey, fileName, res);
        }
        return 1;
    }

    private string ChangeSeqID(string res , int seqID)
    {
        string matchStr = null;
        int seq_id = -999;
        Match match = Regex.Match((string)res, "\"seq_id\":\\s*(\\d+)");
        if (match.Success)
        {
            string str = match.Groups[1].Value;// 提取数字  
            seq_id = int.Parse(str);

            matchStr = match.Groups[0].Value;
        }

        if (seq_id != seqID || seq_id == -999)
        {
            seq_id = seqID;
        }

        if (!string.IsNullOrEmpty(matchStr))
        {
            res.Replace(matchStr, $"\"seq_id\":{seq_id}");
        }

        return res;
    }

    string CheckResponseFormat(string rpcName, string res, int seqID)
    {
        JSONNode nod;
        if (!res.Contains(KEY_RPC_NAME))
        {
            nod = JSONNode.Parse("{}");
            JSONNode _data = JSONNode.Parse(res);
           // _data.Add("seq_id", seqID);
            nod.Add("protocol_key", rpcName);
            nod.Add("data", _data);
        }
        else
        {
            nod = JSONNode.Parse((string)res);
        }
        nod["data"]["seq_id"] = seqID;

        if (!nod["data"].HasKey("err"))
        {
            nod["data"]["err"] = 0;
        }

        if (!nod["data"].HasKey("msg"))
        {
            nod["data"]["msg"] = "";
        }

        return nod.ToString();
    }





    /// <summary>
    /// 延时获取下行数据
    /// </summary>
    /// <param name="rpcKey">  g8980__keno_spin </param>
    /// <param name="fileName"> g8980__keno_spin__ko_21231 </param>
    /// <param name="res"> json 字符串 </param>
    private void DelayResponseRPC(string rpcKey, string fileName, string res)
    {

        this.ClearTime();
        this._timer = new System.Timers.Timer(this._respondTimeMS);
        this._timer.AutoReset = false; // 是否重复执行
        this._timer.Enabled = true; //是否执行Elapsed事件
        this._timer.Elapsed += (object sender, ElapsedEventArgs e) => {
            this.ClearTime();
            /*this.task = () =>
            {
                if(fileName != null)
                    DebugUtil.Log($"@【mock 下行】  rpcKey = {rpcKey} , fileName =  {fileName}");
                if (this._onMessage != null)
                    this._onMessage(res);
            };*/
            taskQueue.Enqueue(() =>
            {
                if (fileName != null)
                    DebugUtils.Log($"==@【mock 下行】  rpcKey = {rpcKey} , fileName =  {fileName}");
                if (this._onMessage != null)
                    this._onMessage(res);
            });

        };
        this._timer.Start();
    }



    private int _respondTimeMS = 20;

    /// <summary>
    /// 链接游戏服务器（加载本地游戏数据）
    /// </summary>
    /// <param name="id"></param>
    /// <param name="callback"></param>
    private  void _ConnectGame(int id, Action<object> callback)
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
                    DebugUtils.LogError(res);
                    if (callback != null) callback(res);
                    return;
                }
                if (callback != null) callback(null);
            });

        }
    }


    #endregion


    #region Loom

    private Action  task = null;

    private bool _isDirty = true;
    private Queue<Action> taskQueue = new Queue<Action>();
    private void Update()
    {

        if (this.task != null)
        {
            var func = this.task;
            this.task = null;
            func.Invoke();
        }

        if (_isDirty)
        {
            _isDirty = false;
            while (taskQueue.Count > 0)
            {
                var task = taskQueue.Dequeue();
                task.Invoke();
            }
            _isDirty = true;
        }
    }

    #endregion

    /*
    public List<object> InterceptHttpPost(string method)
    {
        var rpcName = method.Replace("/", "__");

        TextAsset jsn8 = Resources.Load<TextAsset>("tempdata/keno_play_response_v1");
        if (jsn8 != null && jsn8.text != null)
        {
            //DebugUtil.Log($"【MockManager】： {rpcName} = {jsn8.text}");
            return new List<object>{ null, jsn8.text };
        }
        else
        {
            return new List<object> { $"【MockManager】：找不到文件 {rpcName}"};
        }

    }
    public IEnumerator HttpPost(string url, string method, Dictionary<string, string> post_param, System.Action<string> callback)
    {

        yield return null;

        var rpcName = method.Replace("/","__");

        TextAsset jsn8 = Resources.Load<TextAsset>("tempdata/keno_play_response_v1");
        if (jsn8 != null && jsn8.text != null)
        {
            DebugUtil.Log($"【MockManager】： {rpcName} = {jsn8.text}");
            callback?.Invoke(jsn8.text); // 调用回调函数，传递响应数据
        }
        else
        {
            DebugUtil.LogError($"【MockManager】：找不到文件 {rpcName}");
        }
    }*/



    // g152__XXXXX
    [Button]
    void TestShowData(string gameRpcName = "g152__RESULT_RECALL")
    {

        string[] result = Regex.Split(gameRpcName, "__");

        int gameId = int.Parse(result[0].Replace("g", "")) ;
        string rpcName = result[1];

        bool isFound = false;
        if (jsonLst.ContainsKey(gameId))
        {
            if (jsonLst[gameId].ContainsKey(rpcName))
            {
                isFound = true;
                List <KeyValuePair<string, string>> data = jsonLst[gameId][rpcName];
                foreach (KeyValuePair<string, string> item in data)
                {
                    DebugUtils.Log($"{item.Key} -- {item.Value}");
                }
            }
        }

        if (!isFound)
        {
            DebugUtils.LogError($"找不到数据 ： {gameRpcName}");
        }

    }


    // g152__XXXXX
    [Button]
    void TestShowAllData(int gameId = 152)
    {
        if (jsonLst.ContainsKey(gameId))
        {
            foreach (string item in jsonLst[gameId].Keys)
            {
                DebugUtils.Log($"rpc file name : {item}");
            }
        }
    }
}




/*
 
 有一个DataManager对象，
 选择使用 算法卡数据、Mock数据、网络数据。
 DataManager 输出是统一个事的数据

 算法卡数据/Mock数据/网络数据  --> 数据转换对象（g100,g152） --> DataManager --> UI
 */