using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPCName
{
    /// <summary>登录</summary>
    public const string login = "login";

    /// <summary>进入大厅</summary>
    public const string lobby = "lobby";

    /// <summary>进入子游戏</summary>
    //public const string enterGame = "enter_game";
    public const string enterGame = "STRIPS_RECALL";

    /// <summary>心跳</summary>
    public const string ping = "ping";

    public static string slotSpin {
        get{
            if (ConfigUtils.curGameId == 152) //财神爷
            {
                return "RESULT_RECALL";
            }
            return "slot_spin";
        }
   } 



    /// <summary>大厅彩金</summary>
    public const string jackpotHall = "jackpotHall";

}
