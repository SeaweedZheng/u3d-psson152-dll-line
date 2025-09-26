using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public partial class SoundManager : MonoBehaviour
{
    private float bgmVolumScale = 1f; //背景音乐声音大小比例
    private float effVolumScale = 0.5f; //音效声音大小比例
    private AudioSource musicAudio;   //背景音乐播放器
    private Dictionary<string, AudioClip> audioClipResDic = new Dictionary<string, AudioClip>();
    Transform thisTrans;
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
                instance = Init();
            return instance;
        }
    }
    // Start is called before the first frame update
    private static SoundManager Init()
    {
        GameObject obj = new GameObject("SoundManager");
        SoundManager soundManager = obj.AddComponent<SoundManager>();
        obj.AddComponent<AudioPool>();
        obj.AddComponent<AudioListener>();
        return soundManager;
    }
    private void Awake()
    {
        thisTrans = transform;
        //存储关闭时的音量比例
        if (PlayerPrefs.HasKey("bgmVolumScale"))
            bgmVolumScale = PlayerPrefs.GetFloat("bgmVolumScale");
        if (PlayerPrefs.HasKey("effVolumScale"))
            effVolumScale = PlayerPrefs.GetFloat("effVolumScale");

        musicAudio = gameObject.AddComponent<AudioSource>();
        musicAudio.playOnAwake = false;
        musicAudio.loop = true;
        musicAudio.volume = bgmVolumScale;
        DontDestroyOnLoad(gameObject);
    }

    //解暂停
    public void UnPause()
    {
        musicAudio.UnPause();
        AudioController.UnpauseAll();
    }
    //暂停所有声音
    public void Pause()
    {
        musicAudio.Pause();
        AudioController.PauseAll();
    }
    //停止所有声音
    public void Stop()
    {
        musicAudio.Stop();
        AudioController.StopAll();
    }
    //设置静音
    public void SetMute(bool isMute)
    {
        musicAudio.mute = isMute;
        AudioController.MuteAll();
    }
    /// <summary>
    /// set the bgm scale [0,1]
    /// </summary>
    /// <param name="scale"></param>
    public void SetBGMVolumScale(float scale)
    {
        bgmVolumScale = scale;
        musicAudio.volume = bgmVolumScale;
        PlayerPrefs.SetFloat("bgmVolumScale", bgmVolumScale);
    }
    /// <summary>
    /// set the soundeff scale [0,1]
    /// </summary>
    /// <param name="scale"></param>
    public void SetEFFVolumScale(float scale)
    {
        effVolumScale = scale;
        PlayerPrefs.SetFloat("effVolumScale", effVolumScale);
    }
    //获取背景音乐音量0-1
    public float GetBGMVolumScale()
    {
        return bgmVolumScale;
    }

    //获取音效音量0-1
    public float GetEFFVolumScale()
    {
        return effVolumScale;
    }

    //播放背景音乐
    public void PlayMusic(string assetPath, bool isLoop = false)
    {
        if (IsPlayMusic(assetPath))
            return;
        AudioClip clip = GetClipByName(assetPath);
        PlayMusic(clip);
    }

    public void StopMusic()
    {
        musicAudio.Stop();
    }
    //是否正在播放music


    /// <summary>
    /// 获取音乐名称
    /// </summary>
    /// <param name="assetPathOrName"></param>
    /// <returns></returns>
    private string GetSoundName(string assetPathOrName)
    {
        string[] str = assetPathOrName.Split('/');
        string nameNoSuffix = str[str.Length - 1];
        if (nameNoSuffix.Contains("."))
        {
            str = nameNoSuffix.Split('.');
            int suffixLength = str[str.Length - 1].Length + 1; // ".wav .mp3"
            nameNoSuffix = nameNoSuffix.Substring(0, nameNoSuffix.Length - suffixLength);
        }
        return nameNoSuffix;
    }

    private bool IsPlayMusic(string assetPathOrName)
    {
        string nameNoSuffix = GetSoundName(assetPathOrName);

        if (musicAudio.isPlaying && musicAudio.clip != null)
        {
            return musicAudio.clip.name == nameNoSuffix;
        }

        return false;
    }

    [Button]
    public void TestShowMusic()
    {
        if (musicAudio.isPlaying && musicAudio.clip != null)
        {
            Debug.Log("musicAudio = " + musicAudio.clip.name);   //musicAudio = BaseBackground
            //return musicAudio.clip.name == music;
        }
    }

    public bool IsPlaySoundEff(string assetPathOrName)
    {
        if (AudioController.IsPlaying(GetSoundName(assetPathOrName)))
            return true;
        return false;
    }

    //播放背景音乐
    private void PlayMusic(AudioClip clip, bool loop = true)
    {
        musicAudio.loop = loop;
        musicAudio.clip = clip;
        musicAudio.Play();
    }
    /*private void PlayMusic(AudioClip clip)
    {
        musicAudio.loop = true;
        musicAudio.clip = clip;
        musicAudio.Play();
    }*/

    //播放2d音效  eff  loop 是否循环
    public void PlaySoundEff(string assetPath, bool loop = false)
    {
        AudioClip clip = GetClipByName(assetPath);
        PlaySoundEff(clip, loop);
    }

    //播放2d音效  clip  loop 是否循环
    public void PlaySoundEff(AudioClip clip, bool loop = false)
    {
        if (clip == null)
            return;
        AudioController audioController = AudioPool.Instance.GetController();
        audioController.SetSourceProperties(clip, effVolumScale, 1, loop, 0);
        audioController.Play();
    }

    //播放音效N秒后停止 一般用在背景音乐逻辑
    public void PlaySoundEffAfterSecondsStop(string assetPath, float seconds)
    {
        AudioClip clip = GetClipByName(assetPath);
        if (clip == null)
        {
            return;
        }
        AudioController audioController = AudioPool.Instance.GetController();
        audioController.SetSourceProperties(clip, effVolumScale, 1, false, 0);

    }

    public void StopAllSoudEff()
    {
        AudioController.StopAll();
    }

    //停止某音效
    public void StopSoundEff(AudioClip clip)
    {
        AudioController.Stop(clip);
    }

    public void StopSoundEff(string assetPath, float seconds = 0)
    {
        if (seconds == 0)
        {
            StopSoundEff(assetPath);
        }
        else
        {
            AudioClip audioClip = GetClipByName(assetPath);

        }
    }

    public void StopSoundEff(string assetPath)
    {
        if (audioClipResDic.ContainsKey(assetPath))
        {
            AudioController.Stop(audioClipResDic[assetPath]);
        }
    }

    //设置音效音量
    public void SetSoundEffVolume(string assetPathOrName, float volume)
    {
        AudioController.SetVolume(GetSoundName(assetPathOrName), volume);
    }

    public AudioClip GetClipByName(string assetPath)
    {
        if (!audioClipResDic.ContainsKey(assetPath))
            audioClipResDic[assetPath] = ResourceManager.Instance.LoadAssetAtPath<AudioClip>(assetPath, MARK_BUNDLE_SOUND_MANAGER);
            //audioClipResDic[assetPath] = ResMgr1001.Instance.LoadSound($"{assetPath}");
        return audioClipResDic[assetPath];
    }
}


public partial class SoundManager : MonoBehaviour {

    const string MARK_BUNDLE_SOUND_MANAGER = "MARK_BUNDLE_SOUND_MANAGER";

    //播放2d音效  eff  loop 是否循环
    public void PlaySoundEffX(string assetPath, bool loop = false)
    {
        if (!audioClipResDic.ContainsKey(assetPath))
            audioClipResDic[assetPath] = ResourceManager.Instance.LoadAssetAtPath<AudioClip>(assetPath, MARK_BUNDLE_SOUND_MANAGER);
        //ResMgr1001.Instance.LoadSound($"{name}");

        if (audioClipResDic[assetPath] == null)
            Debug.LogError($"AudioClip is null;  path = {assetPath}");

        PlaySoundEff(audioClipResDic[assetPath], loop);
    }

    public void PlaySoundEffSingle(string assetPath, bool loop = false)
    {
        StopSoundEff(assetPath);
        PlaySoundEffX(assetPath,loop);
    }

    public void PlayMusicX(string assetPath, bool loop = true)
    {
        if (!audioClipResDic.ContainsKey(assetPath))
            audioClipResDic[assetPath] = ResourceManager.Instance.LoadAssetAtPath<AudioClip>(assetPath, MARK_BUNDLE_SOUND_MANAGER);
        //ResMgr1001.Instance.LoadSound($"{name}");

        if (audioClipResDic[assetPath] == null)
            Debug.LogError($"AudioClip is null;  path = {assetPath}");

        PlayMusic(audioClipResDic[assetPath], loop);
    }
    public void PlayMusicSingle(string assetPath, bool loop = false)
    {
        StopMusic();
        PlayMusicX(assetPath, loop);
    }


    public bool IsPlaySound(string assetPathOrName)
    {
        string nameNoSuffix = GetSoundName(assetPathOrName);

        // 背景音乐
        if (musicAudio.clip != null
            && musicAudio.isPlaying
            && musicAudio.clip.name == nameNoSuffix
            && musicAudio.gameObject.active)
            return true;

        // 音效
        if (AudioController.IsPlaying(nameNoSuffix))
            return true;

        return false;
    }
}
