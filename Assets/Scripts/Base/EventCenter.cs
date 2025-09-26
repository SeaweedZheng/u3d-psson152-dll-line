using System.Collections.Generic;
using UnityEngine.Events;
using GameMaker;
public interface IEventInfo
{

}
public class EventInfo<T> : IEventInfo
{
    public UnityAction<T> actions;
    public EventInfo(UnityAction<T> action)
    {
        actions += action;
    }
}
public class EventInfo : IEventInfo
{
    public UnityAction actions;

    public EventInfo(UnityAction action)
    {
        actions += action;
    }
}
/// <summary>
/// 事件中心 单例模式对象
/// 1.Dictionary
/// 2.委托
/// 3.观察者设计模式
/// 4.泛型
/// </summary>
public class EventCenter : BaseManager<EventCenter>
{
    //key —— 事件的名字（比如：怪物死亡，玩家死亡，通关 等等）
    //value —— 对应的是 监听这个事件 对应的委托函数们
    private Dictionary<string, IEventInfo> eventDic = new Dictionary<string, IEventInfo>();

    /// <summary>
    /// 添加事件监听
    /// </summary>
    /// <param name="name">事件的名字</param>
    /// <param name="action">准备用来处理事件 的委托函数</param>
    public void AddEventListener<T>(string name, UnityAction<T> action)
    {
        //有没有对应的事件监听
        //有的情况
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo<T>).actions += action;
        }
        //没有的情况
        else
        {
            eventDic.Add(name, new EventInfo<T>(action));
        }
    }

    /// <summary>
    /// 监听不需要参数传递的事件
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void AddEventListener(string name, UnityAction action)
    {
        //有没有对应的事件监听
        //有的情况
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo).actions += action;
        }
        //没有的情况
        else
        {
            eventDic.Add(name, new EventInfo(action));
        }
    }


    /// <summary>
    /// 移除对应的事件监听
    /// </summary>
    /// <param name="name">事件的名字</param>
    /// <param name="action">对应之前添加的委托函数</param>
    public void RemoveEventListener<T>(string name, UnityAction<T> action)
    {
        if (eventDic.ContainsKey(name))
            (eventDic[name] as EventInfo<T>).actions -= action;
    }

    /// <summary>
    /// 移除不需要参数的事件
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void RemoveEventListener(string name, UnityAction action)
    {
        if (eventDic.ContainsKey(name))
            (eventDic[name] as EventInfo).actions -= action;
    }

    /// <summary>
    /// 事件触发
    /// </summary>
    /// <param name="name">哪一个名字的事件触发了</param>
    public void EventTrigger<T>(string name, T info)
    {
        /*
        if (typeof(T) == typeof(EventData) || typeof(T).IsSubclassOf(typeof(EventData)))
        {
            EventData ed = info as EventData;
            DebugUtil.Log($"【EventData】 eventName = {name}  ;  name = {ed.name}  ;  value = {ed.value}");
        }*/
        

        //有没有对应的事件监听
        //有的情况
        if (eventDic.ContainsKey(name))
        {
            /*if (name == "ON_UI_EVENT01")
            {
                EventInfo<T> req1 = eventDic[name] as EventInfo<T>;
                if(req1 == null)
                {
                    Debug.LogWarning($" {req1} is null");
                }
                if (req1.actions == null)
                {
                    Debug.LogWarning($" {req1} is null");
                }
            }*/

            EventInfo<T> req = eventDic[name] as EventInfo<T>;
            if (req == null) {
                // EventData 类型 被自动识别为 EventData<T> 
                DebugUtils.LogError($"event name:{name}; EventTrigger<T> 方法没有 填写<T>; 例如： EventTrigger<EventData> 类型 被自动识别为 EventTrigger<EventData<T>>");
                /*
                 * EventInfo<T> req = eventDic[name] as EventInfo<T>;
                 * 【监听父类 , 用子类作为 EventTrigger<T>的T,来发送信息时就会报错！】

                 EventCenter.Instance.AddEventListener<EventData>("123", func); // 监听父类
                 EventCenter.Instance.EventTrigger<EventData>("123", new EventData<string>("1", "2"));   // 能用
                 EventCenter.Instance.EventTrigger<EventData<string>>("123", new EventData<string>("1", "2"));   //报错

                */
            }
            if (req.actions != null)
                req.actions.Invoke(info);
            //eventDic[name].Invoke(info);
        }
    }
    /// <summary>
    /// 事件触发（不需要参数的）
    /// </summary>
    /// <param name="name"></param>
    public void EventTrigger(string name)
    {
        //有没有对应的事件监听
        //有的情况
        if (eventDic.ContainsKey(name))
        {
            //eventDic[name]();
            if ((eventDic[name] as EventInfo).actions != null)
                (eventDic[name] as EventInfo).actions.Invoke();
            //eventDic[name].Invoke(info);
        }
    }
    /// <summary>
    /// 清空事件中心
    /// 主要用在 场景切换时
    /// </summary>
    public void Clear()
    {
        eventDic.Clear();
    }
}
