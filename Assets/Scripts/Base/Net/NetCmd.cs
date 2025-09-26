//服务器发送给客户端的消息
public enum S2C_CMD
{
    S2C_HeartHeat = 1000,                      //心跳
    S2C_WinJackpot,                            //获得彩金
    S2C_Error,                                 //错误
}
//客户端发送给服务器的消息
public enum C2S_CMD
{
    C2S_HeartHeat = 2000,                      //心跳
    C2S_Login,                                 //登录
    C2S_JackBet,                               //下注
    C2S_ReceiveJackpot,                        //领取彩金
}