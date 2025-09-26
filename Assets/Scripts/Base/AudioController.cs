using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{

    #region Static Properties
    public const string NAME_AUDIO_CONTROLLER = "AudioController";
    public const float IDLE_TIMEOUT_LENGTH = 30f;
    private static List<AudioController> controllers = new List<AudioController>();
    #endregion

    public AudioSource source { get; private set; }

    public bool inUse { get; private set; }
    public float playProgress { get; private set; }
    public bool isPause { get; private set; }
    public bool isMute { get; private set; }
    public float lastPlayTime { get; private set; }
    public bool isPlaying { get; private set; }
    public bool autoPause { get; private set; }
    public bool muted { get; private set; }
    public bool paused { get; private set; }
    public float idleTime
    {
        get
        {
            return Time.realtimeSinceStartup - lastPlayTime;
        }
    }

    private void Reset()
    {
        ResetController();
    }

    private void Awake()
    {
        controllers.Add(this);
        source = gameObject.GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
        ResetController();
    }

    private void Update()
    {
        if (isMute || isPause || source.isPlaying)
        {
            lastPlayTime = Time.realtimeSinceStartup;
        }
        if (isMute != muted)
        {
            source.mute = isMute;
            muted = isMute;
        }
        if (isPause != paused)
        {
            if (isPause)
            {
                if (source.isPlaying)
                {
                    source.Pause();
                }
            }
            else
            {
                source.UnPause();
            }
            paused = isPause;
        }
        UpdatePlayProgress();
        if (playProgress >= 1f)
        {
            Stop();
            playProgress = 0f;
        }
        else
        {
            autoPause = inUse && isPlaying && !source.isPlaying && playProgress > 0f;
            if (inUse && !autoPause && !source.isPlaying && !isPause && isMute)
            {
                Stop();
            }
        }
        if (idleTime > IDLE_TIMEOUT_LENGTH)
        {
            Kill();
        }
    }

    private void OnDestroy()
    {
        controllers.Remove(this);
    }

    // -- Public Methods

    public void Kill()
    {
        source.Stop();
        Destroy(gameObject);
    }

    public void Mute()
    {
        isMute = true;
    }

    public void Pause()
    {
        isPause = true;
    }

    public void Play()
    {
        inUse = true;
        isPause = false;
        isPlaying = true;
        source.Play();
    }

    public void Play(float afterStop)
    {
        inUse = true;
        isPause = false;
        isPlaying = true;
        source.Play();
    }


    private IEnumerator AfterStop(float  time)
    {
        yield return new WaitForSeconds(time);
        while(true)
        {
            source.volume -= 0.01f;
            if (source.volume < 0)
            {
                source.volume = 0;
                yield break;
            }
        }
    }

    public void SetupAudioMixerGroup(AudioMixerGroup group)
    {
        if (group != null)
        {
            source.outputAudioMixerGroup = group;
        }
    }

    public void SetSourceProperties(AudioClip clip, float volume, float pitch, bool loop, float spatialBlend)
    {
        if (clip != null)
        {
            source.clip = clip;
            source.volume = volume;
            source.pitch = pitch;
            source.loop = loop;
            source.spatialBlend = spatialBlend;
        }
        else
        {
            Stop();
        }
    }

    public void Stop()
    {
        Unpause();
        Unmute();
        source.Stop();
        AudioPool.Instance.PutController(this);
        ResetController();
    }

    public void Unmute()
    {
        isMute = false;
    }

    public void Unpause()
    {
        isPause = false;
    }

    // -- Private Methods

    private void UpdatePlayProgress()
    {
        if (source != null && source.clip != null)
        {
            playProgress = Mathf.Clamp01(source.time / source.clip.length);
        }
    }

    private void ResetController()
    {
        inUse = false;
        isPause = false;
        lastPlayTime = Time.realtimeSinceStartup;
    }

    // -- Static Methods

    public static AudioController GetController()
    {
        AudioController controller = new GameObject(NAME_AUDIO_CONTROLLER, typeof(AudioSource), typeof(AudioController)).GetComponent<AudioController>();
        return controller;
    }

    public static void SetVolume(string clipNameNoSuffix, float volume)
    {
        RemoveNullsFromList();
        for (int i = 0; i < controllers.Count; i++)
        {
            if (controllers[i].source.clip != null && controllers[i].source.clip.name == clipNameNoSuffix)
            {
                controllers[i].source.volume = volume;
            }
        }
    }

    public static bool IsPlaying(string clipNameNoSuffix)
    {
        RemoveNullsFromList();
        for (int i = 0; i < controllers.Count; i++)
        {
            if (controllers[i].source.clip != null
                && controllers[i].source.isPlaying
                && controllers[i].source.clip.name == clipNameNoSuffix
                && controllers[i].gameObject.active)
            {
                return true;
            }
        }
        return false;
    }

    public static void KillAll()
    {
        RemoveNullsFromList();
        for (int i = 0; i < controllers.Count; i++)
        {
            controllers[i].Kill();
        }
    }

    public static void MuteAll()
    {
        RemoveNullsFromList();
        for (int i = 0; i < controllers.Count; i++)
        {
            controllers[i].Mute();
        }
    }

    public static void PauseAll()
    {
        RemoveNullsFromList();
        for (int i = 0; i < controllers.Count; i++)
        {
            controllers[i].Pause();
        }
    }

    public static void Stop(AudioClip clip)
    {
        for (int i = 0; i < controllers.Count; i++)
        {
            if (controllers[i].source.clip == clip)
            {
                controllers[i].Stop();
            }
        }
    }

    public static void StopAll()
    {
        RemoveNullsFromList();
        for (int i = 0; i < controllers.Count; i++)
        {
            controllers[i].Stop();
        }
    }

    public static void UnmuteAll()
    {
        RemoveNullsFromList();
        for (int i = 0; i < controllers.Count; i++)
        {
            controllers[i].Unmute();
        }
    }

    public static void UnpauseAll()
    {
        RemoveNullsFromList();
        for (int i = 0; i < controllers.Count; i++)
        {
            controllers[i].Unpause();
        }
    }

    public static void RemoveNullsFromList()
    {
        IEnumerable<AudioController> tempSet = controllers;
        controllers = new List<AudioController>();
        foreach (AudioController ctrl in tempSet)
        {
            if (ctrl != null)
            {
                controllers.Add(ctrl);
            }
        }
    }
}