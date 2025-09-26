using System;

public interface ISocket
{
    /// <summary>���ӻص�</summary>
    Action<object> onConnected { set; }

    /// <summary>��Ϣ�ص�</summary>
    Action<object> onMessage { set; }

    /// <summary>����ص�</summary>
    Action<object> onError { set; }

    /// <summary>�رջص�</summary>
    Action<object> onClosed { set; }


    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="options">Э������</param>
    /// <returns>true ��ʼ���� / false �����쳣</returns>
    bool Connect(NetConnectOptions options);


    /// <summary>
    /// ���ݷ��ͽӿ�
    /// </summary>
    /// <param name="data">����</param>
    /// <returns>���ͳɹ�����1  / ����ʧ�ܷ���-1</returns>
    int Send(object data);


    /// <summary>
    /// �رսӿ�
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

    /// <summary>-1 ����������0���Զ�����������������Ϊ�Զ����Դ���</summary>
    public int autoReconnect = 0;

}

public enum NetNodeState
{
    /// <summary>�ѹر�</summary>
    Closed,
    /// <summary>������</summary>
    Connecting,
    /// <summary>����������֤��</summary>
    Checking,
    /// <summary>�ɴ�������</summary>
    Working,
}