using System;
using GameMaker;
using UnityEngine;

public class DebugUtils 
{
    private static DebugUtils instance;
    private bool openDebug = true;
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
            Instance.openDebug = (bool)res.value;
        }
    }


    public static void Log(object msg)
    {
        //return;
        if (Instance.openDebug == false)
            return;

        //DebugFilterDynamics.Instance.AnalysisDebug($"{msg}");
        //if (!DebugFilterDynamics.Instance.IsShowDebug($"{msg}"))  return;

        Debug.Log(msg);
    }

    public static void LogWarning(object msg)
    {
        if (Instance.openDebug == false)
            return;

        //DebugFilterDynamics.Instance.AnalysisDebug($"{msg}");
        //if (!DebugFilterDynamics.Instance.IsShowDebug($"{msg}")) return;

        Debug.LogWarning(msg);
    }

    public static void LogError(object msg)
    {
        Debug.LogError(msg);
    }

    public static void LogException(Exception exception)
    {
        Debug.LogException(exception);
    }

}
