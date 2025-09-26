using GameMaker;
using SBoxApi;
using SimpleJSON;
using System.Security.Principal;
using static System.Net.WebRequestMethods;
using _consoleBB = PssOn00152.ConsoleBlackboard02;
namespace PssOn00152
{

    [System.Serializable]
    public class ReportData
    {

        ///game_log/send

        /// <summary> 后台游戏id </summary>
        public string game_id = "2002";

        /// <summary> 机台号 </summary>
        //public string machineId = "1004";
        public string player = "";

        /// <summary> 总上 </summary>
        public int recharge = 10000;

        /// <summary> 总下 </summary>
        public int withdraw = 9800;

        /// <summary> 盈利（recharge - withdraw） </summary>
        public int profit = 200;

        /// <summary> 总赢 </summary>
        public int win = 500;
        /// <summary> 总压 </summary>
        public int bet = 300;

        /// <summary> 总盈亏 (总赢 / 总压 = win / bet ) </summary>
        public float rtp = 1.01f;

        /// <summary> 游戏难度 </summary>
        public int difficult = 95;

        /// <summary> 总局数 </summary>
        public int roundCount = 888;

        /// <summary> 软件版本 </summary>
        public string version = ApplicationSettings.Instance.appVersion;

        public GameSenceData detail;

        public ReportTotal total = new ReportTotal();
    }

    [System.Serializable]
    public class ReportTotal
    {
        /// <summary> 列 </summary>
        public int column = 5;

        /// <summary> 行 </summary>
        public int row = 3;
    }



    public static class ReportDataUtils{
        public static string CreatReportData(GameSenceData detail, SBoxPlayerAccount account )
        {
            ReportData data = new ReportData();

            data.game_id = "2002"; //后台财神爷对应的编号

            data.recharge = account.ScoreIn;
            data.withdraw = account.ScoreOut;
            data.profit = data.recharge - data.withdraw;
            data.bet = account.Bets;
            data.win = account.Wins;
            data.rtp = (float)data.win / (float)data.bet;

            data.player = $"{_consoleBB.Instance.machineID}";  //11100001
            data.detail = detail;


            //后台自定义数据节点：
            string str = JSONNodeUtil.ObjectToJsonStr(data);
            JSONNode node = JSONNode.Parse(str);
            int hasFree = detail.curStripsIndex == "BS" && detail.nextStripsIndex == "FS" ? 1 : 0;
            node["detail"].Add("has_free", hasFree);
            node["detail"].Add("is_free", detail.curStripsIndex == "FS"?1:0);
            node["detail"].Add("row", 3);
            node["detail"].Add("column", 5);
            return node.ToString();
        }

     }
}