using SimpleJSON;
using System;

public interface IHttp
{
    int Post(string rpcName, object data, Action<JSONNode> responseCallback, HTTPErrorCallback errorCallback);

    int Get(string rpcName, Action<JSONNode> responseCallback, HTTPErrorCallback errorCallback);

    void RemoveRequestAt(int mark);
}



class EventHandlerInfo
{
    public EventHandlerInfo(string rpcName, object data, Action<JSONNode> responseCallback, HTTPErrorCallback errorCallback, int seqID)
    {
        this.rpcName = rpcName;
        this.data = data;
        this.responseCallback = responseCallback;
        this.errorCallback = errorCallback;
        this.seqID = seqID;
    }

    public int seqID;
    public string rpcName;
    public object data;
    public Action<JSONNode> responseCallback;
    public HTTPErrorCallback errorCallback;
}

public class BagelCodeHTTPError
{
    /*
    public string url;
    public long responseCode;
    public string error;
    public ClientModels.Error errorCode;
    public object errorDetailInfo = null;
    */

    public long code;
    public string msg;
    public string response;
}


public class BagelCodeError
{
    public long code = -1;
    public string msg = "";
    public object response;
}

public delegate void HTTPErrorCallback(BagelCodeHTTPError error);