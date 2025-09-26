using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPool : MonoSingleton<AudioPool>
{
    #region Public Fields
    public bool AutoKillIdleControllers = true; // 是否自动删除多余的播放控制器
    public float ControllerIdleKillDuration = 5f; // 删除播放控制器的时间要求
    public float IdleCheckInterval = 1f; // 检查周期
    public int MinimumNumberOfControllers = 3;  // 最小保有的播放控制器数量
    #endregion

    #region Private Fields
    private List<AudioController> audioPool;
    private Coroutine idleCheckCoroutine;
    private WaitForSecondsRealtime idleCheckIntervalWaitTime;
    private AudioController tempController;
    #endregion

    private void OnEnable()
    {
        if (AutoKillIdleControllers)
        {
            StartIdleCheckInterval();
        }
    }

    private void Awake()
    {
        audioPool = new List<AudioController>();
    }

    private void OnDisable()
    {
        StopIdleCheckInterval();
    }

    // -- Static Methods
    public void ClearPool(bool keepMinCount = false)
    {
        if (keepMinCount)
        {
            RemoveNullsFromThePool();
            if (audioPool.Count <= MinimumNumberOfControllers)
            {

            }
            else
            {
                int killCount = 0;
                for (int i = audioPool.Count - 1; i >= MinimumNumberOfControllers; i--)
                {
                    AudioController ctrl = audioPool[i];
                    audioPool.Remove(ctrl);
                    ctrl.Kill();
                    killCount++;
                }
            }
        }
        else
        {
            AudioController.KillAll();
            audioPool.Clear();
        }
    }

    public AudioController GetController()
    {
        RemoveNullsFromThePool();
        AudioController ctrl;
        if (audioPool.Count > 0)
        {
            ctrl = audioPool[0];
            audioPool.Remove(ctrl);
            ctrl.gameObject.SetActive(true);
        }
        else
        {
            ctrl = AudioController.GetController();
            ctrl.transform.SetParent(Instance.transform);
        }
        return ctrl;
    }

    public void PopulatePool(int count)
    {
        RemoveNullsFromThePool();
        if (count >= 1)
        {
            for (int i = 0; i < count; i++)
            {
                PutController(AudioController.GetController());
            }
        }
    }

    public void PutController(AudioController ctrl)
    {
        if (ctrl != null)
        {
            ctrl.gameObject.SetActive(false);
            ctrl.transform.SetParent(Instance.transform);
            if (!audioPool.Contains(ctrl))
            {
                audioPool.Add(ctrl);
            }
        }
    }

    private void RemoveNullsFromThePool()
    {
        IEnumerable<AudioController> tempSet = audioPool;
        audioPool = new List<AudioController>();
        foreach (AudioController ctrl in tempSet)
        {
            if (ctrl != null)
            {
                audioPool.Add(ctrl);
            }
        }
    }

    // -- Private Methods

    private void StartIdleCheckInterval()
    {
        idleCheckIntervalWaitTime = new WaitForSecondsRealtime(IdleCheckInterval < 0f ? 0f : IdleCheckInterval);
        idleCheckCoroutine = StartCoroutine(KillIdleControllers());
    }

    private void StopIdleCheckInterval()
    {
        if (idleCheckCoroutine != null)
        {
            StopCoroutine(idleCheckCoroutine);
            idleCheckCoroutine = null;
        }
    }

    // -- Coroutine
    private IEnumerator KillIdleControllers()
    { // 定时检查播放控制器进行适当的删除
        while (AutoKillIdleControllers)
        {
            yield return idleCheckIntervalWaitTime;
            RemoveNullsFromThePool();
            int minControllerCount = MinimumNumberOfControllers > 0 ? MinimumNumberOfControllers : 0;
            float controllerKillDuration = ControllerIdleKillDuration > 0f ? ControllerIdleKillDuration : 0f;
            if (audioPool.Count > minControllerCount)
            {
                for (int i = audioPool.Count - 1; i >= minControllerCount; i--)
                {
                    tempController = audioPool[i];
                    if (tempController.idleTime >= controllerKillDuration)
                    {
                        audioPool.Remove(tempController);
                        tempController.Kill();
                    }
                }
            }
        }
        idleCheckCoroutine = null;
    }
}