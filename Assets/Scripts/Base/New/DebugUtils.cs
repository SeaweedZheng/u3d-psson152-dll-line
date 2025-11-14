using System;
using GameMaker;
using UnityEngine;

public class DebugUtils 
{
    private static DebugUtils instance;
    private bool openDebugLog = true;
    public static DebugUtils Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DebugUtils();
                EventCenter.Instance.RemoveEventListener<EventData>("ON_PROPERTY_CHANGED_EVENT", OnPropertyChangedEventIsDebug);
                EventCenter.Instance.AddEventListener<EventData>("ON_PROPERTY_CHANGED_EVENT", OnPropertyChangedEventIsDebug);
            }
            return instance;
        }
    }

    public static void OnPropertyChangedEventIsDebug(EventData res)
    {
        if (res.name == "@console/isDebug")
        {
            Instance.openDebugLog = (bool)res.value;
        }
    }


    public static void Log(object msg)
    {
        //return;
        if (Instance.openDebugLog == false)
            return;

        //DebugFilterDynamics.Instance.AnalysisDebug($"{msg}");
        //if (!DebugFilterDynamics.Instance.IsShowDebug($"{msg}"))  return;

        Debug.Log(msg);
    }
    public static void LogFormat(string format, params object[] args)
    {
        if (Instance.openDebugLog == false)
            return;
        Debug.LogFormat(format, args);
    }


    public static void LogWarning(object msg)
    {
        if (Instance.openDebugLog == false)
            return;

        //DebugFilterDynamics.Instance.AnalysisDebug($"{msg}");
        //if (!DebugFilterDynamics.Instance.IsShowDebug($"{msg}")) return;

        Debug.LogWarning(msg);
    }

    public static void LogError(object msg)
    {
        Debug.LogError(msg);
    }
    public static void LogErrorFormat(string format, params object[] args)
    {
        Debug.LogErrorFormat(format, args);
    }
    public static void LogException(Exception exception)
    {
        Debug.LogException(exception);
    }




    const string SAVE_LOG = "【Log】";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="type"></param>
    /// <remarks>
    /// * 不受日志开关的影响。
    /// </remarks>
    public static void Save(object msg, LogType type = LogType.Log)
    {
        try
        {
            string str = (string)msg;

            if (!str.StartsWith(SAVE_LOG))
                str = $"{SAVE_LOG}{str}";

            switch (type)
            {
                case LogType.Log:
                    {
                        Debug.Log(str);
                    }
                    break;
                case LogType.Warning:
                    {
                        Debug.LogWarning(str);
                    }
                    break;
            }
        }
        catch (Exception e) { }
    }


}
