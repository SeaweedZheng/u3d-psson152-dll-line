using System.Net.Sockets;
using System.Collections.Generic;
//服务器信息
public class ServerInfo
{
    public string IP { get; set; }
    public int port { get; set; }
}

//服务器收到的数据结构(TOOD 此处可能需要优化)
public class SrvMsgData
{
    public Socket mSocket { get; set; }
    public string mData { get; set; }
}

//服务器收到的websocket数据结构
public class WSSrvMsgData
{
    public WebSockets.ClientConnection Client { get; set; }
    public string Data { get; set; }
}

//消息体
public class MsgInfo
{
    public int cmd { get; set; }        //协议
    public int id { get; set; }         //这里一般都是机台ID
    public string jsonData; //
}

//C2S_JackBet
public class JackBetInfo
{
    public int seat;                           // 分机号/座位号                   
    public int bet;                            // 当前的押分,为了避免丢失小数，需要乘以100，硬件读取这个值会除以100后使用
    public int betPercent;                     // 押分比例，目前拉霸默认值传1，同样需要乘以100          
    public int scoreRate;                      // 分值比，1分多少钱，需要乘以1000再往下传
    public int JPPercent;                      // 分机彩金百分比，每次押分贡献给彩金的比例。需要乘以1000再往下传
}

//S2C_WinJackpot
public class WinJackpotInfo
{
    public int macId;
    public int seat;
    /// <summary> 中多少个币 </summary>
    public int win;
    public int jackpotId;
    public long orderId;
    /// <summary> 毫秒时间戳 </summary>
    public long time;
}

//S2C_Error
public class ErrorInfo
{
    public int errCode;
    public string errString;
}
