using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
/*
public class CorBehaviour : MonoBehaviour
{
    private Dictionary<string, Coroutine> _corDic = new Dictionary<string, Coroutine>();
    protected void ClearCor(string name)
    {
        if (_corDic.ContainsKey(name))
        {
            if (_corDic[name] != null)
                StopCoroutine(_corDic[name]);
            _corDic.Remove(name);
        }
    }
    protected void ClearAllCor()
    {
        foreach (var kv in _corDic)
        {
            if (kv.Value != null)
                StopCoroutine(kv.Value);
        }
        _corDic.Clear();
    }
    protected void DoCor(string name, IEnumerator routine)
    {
        ClearCor(name);
        if (gameObject.active)//gameObject.activeSelf
        {
            //_corDic.Add(name, StartCoroutine(routine));
            _corDic.Add(name, StartCoroutine(_routine(name, routine )));
        }
        else
        {
            // Debug.Log("active is false, can not use coroutine");
        }

    }

    IEnumerator _routine(string name, IEnumerator routine, Action callBack=null)
    {
        yield return routine;

        if (_corDic.ContainsKey(name))
            _corDic.Remove(name);
    }

    protected bool IsCor(string name)=>_corDic.ContainsKey(name);
    
}*/


public class CorBehaviour : MonoBehaviour
{

    private CorController _corCtrl; 
    private CorController corCtrl
    {
        get
        {
            if(_corCtrl == null)
            {
                _corCtrl = new CorController(this);
            }
            return _corCtrl;
        }
    }
    
    public void ClearCorStartsWith(string prefix) => corCtrl.ClearCorStartsWith(prefix);
    public void ClearCor(string name) => corCtrl.ClearCor(name);
    
    public void ClearAllCor() => corCtrl.ClearAllCor();

    public void DoCor(string name, IEnumerator routine) => corCtrl.DoCor(name, routine);

    public bool IsCor(string name) => corCtrl.IsCor(name);


    public IEnumerator DoTaskRepeat(Action cb, int ms) => corCtrl.DoTaskRepeat(cb, ms);

    public IEnumerator DoTask(Action cb, int ms) => corCtrl.DoTask(cb, ms);



    /// <summary>
    /// </summary>
    /// <param name="timeS"></param>
    /// <returns></returns>
    /// <remark>
    /// 这个受到Time.timeScale的影响，会变快变慢，停止。
    /// </remark>
    public IEnumerator DoWaitForSeconds(float timeS) =>corCtrl.DoWaitForSeconds(timeS);

    /// <summary>
    /// </summary>
    /// <param name="timeS"></param>
    /// <returns></returns>
    /// <remark>
    /// 这个不受到Time.timeScale的影响。
    /// </remark>
    public IEnumerator DoWaitForSecondsRealtime(float timeS) => corCtrl.DoWaitForSecondsRealtime(timeS);

}

public class CorController
{

    MonoBehaviour _mon;
    private Dictionary<string, Coroutine> _corDic = new Dictionary<string, Coroutine>();

    public CorController(MonoBehaviour monoBehaviour) { _mon = monoBehaviour; }


    public void ClearCorStartsWith(string prefix)
    {
        List<string> keyList = _corDic.Keys.ToList();
        for (int i = 0; i < keyList.Count; i++)
        {
            string key = keyList[i];
            if(key.StartsWith(prefix))
            {
                if (_corDic[key] != null)
                    _mon.StopCoroutine(_corDic[key]);
                _corDic.Remove(key);
            }
        }
    }


    public void ClearCor(string name)
    {
        if (_corDic.ContainsKey(name))
        {
            if (_corDic[name] != null)
                _mon.StopCoroutine(_corDic[name]);
            _corDic.Remove(name);
        }
    }
    public void ClearAllCor()
    {
        foreach (var kv in _corDic)
        {
            if (kv.Value != null)
                _mon.StopCoroutine(kv.Value);
        }
        _corDic.Clear();
    }
    public void DoCor(string name, IEnumerator routine)
    {
        ClearCor(name);
        if (_mon.gameObject.active)//_mon.gameObject.activeSelf
        {
            //_corDic.Add(name, _mon.StartCoroutine(routine));
            _corDic.Add(name, _mon.StartCoroutine(_routine(name, routine)));
        }
        else
        {
            // Debug.Log("active is false, can not use coroutine");
        }

    }

    IEnumerator _routine(string name, IEnumerator routine, Action callBack = null)
    {
        yield return routine;

        if (_corDic.ContainsKey(name))
            _corDic.Remove(name);
    }

    public bool IsCor(string name) => _corDic.ContainsKey(name);


    public IEnumerator DoTaskRepeat(Action cb, int ms = 0)
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(ms / 1000f);
            if (cb != null)
                cb();
        }
    }


    public IEnumerator DoTask(Action cb, int ms = 0)
    {
        yield return new WaitForSecondsRealtime(ms / 1000f);
        if (cb != null)
            cb();    
    }


    /// <summary>
    /// </summary>
    /// <param name="timeS"></param>
    /// <returns></returns>
    /// <remark>
    /// 这个受到Time.timeScale的影响，会变快变慢，停止。
    /// </remark>
    public IEnumerator DoWaitForSeconds(float timeS)
    {
        float targetRunTimeS = Time.time + timeS;
        while (true)
        {
            if (Time.time >= targetRunTimeS)
            {
                yield break;
            }
            yield return null;
        }
    }

    public IEnumerator DoWaitForSeconds02(float timeS)
    {
        float delayRunTimeS = 0f;
        while (true)
        {
            delayRunTimeS += Time.deltaTime;
            if (delayRunTimeS >= timeS)
            {
                yield break;
            }
            yield return null;
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="timeS"></param>
    /// <returns></returns>
    /// <remark>
    /// 这个不受到Time.timeScale的影响。
    /// </remark>
    public IEnumerator DoWaitForSecondsRealtime(float timeS)
    {
        //float targetTimMS = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + timeS * 1000;

        float targetTimeS = Time.unscaledTime + timeS;
        while (true)
        {
            //if (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() >= targetTimMS)
            if(Time.unscaledTime >= targetTimeS)
            {
                yield break;
            }
            yield return null;
        }
    }

}