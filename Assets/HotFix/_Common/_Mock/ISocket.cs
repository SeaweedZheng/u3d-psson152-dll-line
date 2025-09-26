using System;

public interface ISocket
{
    /// <summary>连接回调</summary>
    Action<object> onConnected { set; }

    /// <summary>消息回调</summary>
    Action<object> onMessage { set; }

    /// <summary>错误回调</summary>
    Action<object> onError { set; }

    /// <summary>关闭回调</summary>
    Action<object> onClosed { set; }


    /// <summary>
    /// 连接网络
    /// </summary>
    /// <param name="options">协议名称</param>
    /// <returns>true 开始链接 / false 链接异常</returns>
    bool Connect(NetConnectOptions options);


    /// <summary>
    /// 数据发送接口
    /// </summary>
    /// <param name="data">数据</param>
    /// <returns>发送成功返回1  / 发送失败返回-1</returns>
    int Send(object data);


    /// <summary>
    /// 关闭接口
    /// </summary>
    /// <returns>void</returns>
    void Close();
}

[Serializable]
public class NetConnectOptions
{
    public NetConnectOptions(string url, int autoReconnect = 0)
    {
        this.url = url;
        this.autoReconnect = autoReconnect;
    }

    public string url;

    /// <summary>-1 永久重连，0不自动重连，其他正整数为自动重试次数</summary>
    public int autoReconnect = 0;

}

public enum NetNodeState
{
    /// <summary>已关闭</summary>
    Closed,
    /// <summary>连接中</summary>
    Connecting,
    /// <summary>断线重连验证中</summary>
    Checking,
    /// <summary>可传输数据</summary>
    Working,
}