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

        /// <summary> ��̨��Ϸid </summary>
        public string game_id = "2002";

        /// <summary> ��̨�� </summary>
        //public string machineId = "1004";
        public string player = "";

        /// <summary> ���� </summary>
        public int recharge = 10000;

        /// <summary> ���� </summary>
        public int withdraw = 9800;

        /// <summary> ӯ����recharge - withdraw�� </summary>
        public int profit = 200;

        /// <summary> ��Ӯ </summary>
        public int win = 500;
        /// <summary> ��ѹ </summary>
        public int bet = 300;

        /// <summary> ��ӯ�� (��Ӯ / ��ѹ = win / bet ) </summary>
        public float rtp = 1.01f;

        /// <summary> ��Ϸ�Ѷ� </summary>
        public int difficult = 95;

        /// <summary> �ܾ��� </summary>
        public int roundCount = 888;

        /// <summary> ����汾 </summary>
        public string version = ApplicationSettings.Instance.appVersion;

        public GameSenceData detail;

        public ReportTotal total = new ReportTotal();
    }

    [System.Serializable]
    public class ReportTotal
    {
        /// <summary> �� </summary>
        public int column = 5;

        /// <summary> �� </summary>
        public int row = 3;
    }



    public static class ReportDataUtils{
        public static string CreatReportData(GameSenceData detail, SBoxPlayerAccount account )
        {
            ReportData data = new ReportData();

            data.game_id = "2002"; //��̨����ү��Ӧ�ı��

            data.recharge = account.ScoreIn;
            data.withdraw = account.ScoreOut;
            data.profit = data.recharge - data.withdraw;
            data.bet = account.Bets;
            data.win = account.Wins;
            data.rtp = (float)data.win / (float)data.bet;

            data.player = $"{_consoleBB.Instance.machineID}";  //11100001
            data.detail = detail;


            //��̨�Զ������ݽڵ㣺
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