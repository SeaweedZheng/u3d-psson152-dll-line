using GameMaker;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Timers;
using UnityEngine;

namespace GameMaker
{
    public partial class ObservableObject
    {
        public const string ON_PROPERTY_CHANGED_EVENT = "ON_PROPERTY_CHANGED_EVENT";

        string[] classNameArr;
        public ObservableObject(params string[] classNameArr)
        {
            List<string> list = new List<string>();
            foreach (string cls in classNameArr)
            {
                list.Add(cls.TrimEnd('/'));
            }

            //this.classNameArr = classNameLst;
            this.classNameArr = list.ToArray();
        }

        public virtual void OnPropertyChanged(string propertyName, object value)
        {
            foreach (string cls in classNameArr)
            {
                //MessageDispatcher.Dispatch(ON_PROPERTY_CHANGED, new EventData<object>($"{cls}/{propertyName}", value));
                EventCenter.Instance.EventTrigger<EventData>(ON_PROPERTY_CHANGED_EVENT, new EventData<object>($"{cls}/{propertyName}", value));
            }
        }

        public virtual void OnPropertyChanged(object value, [CallerMemberName] String propertyName = null)
        {
            //DebugUtil.Log($"@============= {propertyName}");
            OnPropertyChanged(propertyName, value);
        }
        public bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            //ClearTimer(propertyName);
            if (Equals(storage, value)) return false;
            storage = value;
            OnPropertyChanged(propertyName, value);
            return true;
        }

    }

    public partial class ObservableObject
    {
        /// <summary>
        /// ����list��dictionary��class �仯
        /// </summary>
        /// <param name="name"></param>
        public void ClearTimer(string name = null)
        {
            if (name == null)
            {
                foreach (Timer tm in checkPropertyTimer.Values)
                {
                    tm.Stop();
                    tm.Dispose();
                }
                checkPropertyTimer.Clear();
            }
            else if (checkPropertyTimer.ContainsKey(name))
            {
                checkPropertyTimer[name].Stop();
                checkPropertyTimer[name].Dispose();
                checkPropertyTimer.Remove(name);
            }
        }


        /// <summary>
        /// * �����߳�ʹ��Dictionary ʱ���������� System.IndexOutOfRangeException: Index was outside the bounds of the array
        /// * ����ʹ�� ConCurrentDictionay
        /// (������߳�dic.Add��������Խ������)[https://blog.csdn.net/a1234012340a/article/details/109285308]
        /// * ʹ���߳������ڶ�д����ʱ����������ֱ������������
        /// * ʹ���̰߳�ȫ��ConcurrentDictionary���󣬲�ʹ��TryAdd��TryUpdate����������
        /// </summary>
        private Dictionary<string, System.Timers.Timer> checkPropertyTimer = new Dictionary<string, System.Timers.Timer>();


        public T GetProperty<T>(System.Func<T> getStorage,[CallerMemberName] String propertyName = null)
        {
            if (!checkPropertyTimer.ContainsKey(propertyName))
            {
                float ms = 1000f;

                try
                {
                    checkPropertyTimer.Add(propertyName, new System.Timers.Timer(ms));
                }
                catch (Exception ex)
                {
                    DebugUtils.LogException(ex);
                    DebugUtils.LogError($"err == {propertyName}");
                }


                //string json = JsonUtility.ToJson(getStorage());
                //string json = JSONNodeUtil.ObjectToJsonStr(getStorage());
                string json = JsonConvert.SerializeObject(getStorage());

                //System.Timers.Timer checkTimer = new System.Timers.Timer(ms);
                System.Timers.Timer checkTimer = checkPropertyTimer[propertyName];
                checkTimer.AutoReset = false; // �Ƿ��ظ�ִ��
                checkTimer.Elapsed += (object sender, ElapsedEventArgs e) =>
                {

                    /*
                    //checkTimer.Stop();
                    //checkTimer.Dispose();
                    //checkTimer = null;
                    ClearTimer(propertyName);

                    // DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                    //string json1 = JsonUtility.ToJson(getStorage());
                    string json1 = JSONNodeUtil.ObjectToJsonStr(getStorage());
                    if (json1 != json)
                    {
                        //DebugUtil.Log($" {propertyName} �����仯  value = {json1}");
                        try
                        {
                            Loom.QueueOnMainThread((data) =>
                            {
                                OnPropertyChanged(propertyName, getStorage());
                            }, null);
                        }
                        catch (Exception ex)
                        {
                            DebugUtil.LogException(ex);
                        }
                    }
                    else
                    {
                        //DebugUtil.Log($" {propertyName} �������仯  value = {json1}");
                    }*/

                    Loom.QueueOnMainThread((data) =>
                    {
                        ClearTimer(propertyName);
                        //string json1 = JSONNodeUtil.ObjectToJsonStr(getStorage());
                        string json1 = JsonConvert.SerializeObject(getStorage());
                        if (json1 != json)
                        {
                            //DebugUtil.Log($" {propertyName} �����仯  value = {json1}");
                            OnPropertyChanged(propertyName, getStorage());
                        }
                        else
                        {
                            //DebugUtil.Log($" {propertyName} �������仯  value = {json1}");
                        }
                        
                    }, null);

                };
                //this._receiveMsgTimer.Enabled = true; //��ʼִ��
                checkTimer.Start();
            }

            return getStorage();
        }
    }
}


public class MyObject01 : MonoBehaviour
{
    ObservableObject m_Observable;
    public ObservableObject observable
    {
        get
        {
            if (m_Observable == null)
            {
                string[] classNamePath = this.GetType().ToString().Split('.');
                m_Observable = new ObservableObject(classNamePath[classNamePath.Length - 1], "@obj01/");
            }
            return m_Observable;
        }
    }

    [Tooltip("my real name is m_TotalBet")]
    [SerializeField]
    private long m_TotalBet = 0;
    public long totalBet
    {
        get => m_TotalBet;
        set => observable.SetProperty(ref m_TotalBet, value);
    }

    [SerializeField]
    private List<int> m_WinList;
    public List<int> winList
    {
        get => observable.GetProperty(() => m_WinList);
        set => observable.SetProperty(ref m_WinList, value);
    }
}



#if false
    /// <summary>
    /// u3d ���������̵߳������̵߳�API
    /// </summary>
    public class LoomProxy : MonoSingleton<LoomProxy>
    {
        private bool _isDirty = true;
        private Queue<Action> taskQueue = new Queue<Action>();
        private void Update()
        {
            if (_isDirty)
            {
                _isDirty = false;
                while (taskQueue.Count > 0)
                {
                    var task = taskQueue.Dequeue();
                    task.Invoke();
                }
                _isDirty = true;
            }
        }
        public void AddTask(Action fun)
        {
            taskQueue.Enqueue(fun);  
        }
    }
#endif


#if UNITY_EDITOR && false
    public class MyObject
    {

        ObservableObject m_Observable;
        ObservableObject observable
        {
            get
            {
                if (m_Observable == null)
                {
                    string[] classNamePath = this.GetType().ToString().Split('.');
                    m_Observable = new ObservableObject(classNamePath[classNamePath.Length - 1]);
                }
                return m_Observable;
            }
        }

        private string m_PropertyA;
        public string propertyA
        {
            get => m_PropertyA;
            set => observable.SetProperty(ref m_PropertyA, value);
        }
    }

#endif