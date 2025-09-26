using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public partial class SoundManager : MonoBehaviour
{
    private float bgmVolumScale = 1f; //��������������С����
    private float effVolumScale = 0.5f; //��Ч������С����
    private AudioSource musicAudio;   //�������ֲ�����
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
        //�洢�ر�ʱ����������
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

    //����ͣ
    public void UnPause()
    {
        musicAudio.UnPause();
        AudioController.UnpauseAll();
    }
    //��ͣ��������
    public void Pause()
    {
        musicAudio.Pause();
        AudioController.PauseAll();
    }
    //ֹͣ��������
    public void Stop()
    {
        musicAudio.Stop();
        AudioController.StopAll();
    }
    //���þ���
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
    //��ȡ������������0-1
    public float GetBGMVolumScale()
    {
        return bgmVolumScale;
    }

    //��ȡ��Ч����0-1
    public float GetEFFVolumScale()
    {
        return effVolumScale;
    }

    //���ű�������
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
    //�Ƿ����ڲ���music


    /// <summary>
    /// ��ȡ��������
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

    //���ű�������
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

    //����2d��Ч  eff  loop �Ƿ�ѭ��
    public void PlaySoundEff(string assetPath, bool loop = false)
    {
        AudioClip clip = GetClipByName(assetPath);
        PlaySoundEff(clip, loop);
    }

    //����2d��Ч  clip  loop �Ƿ�ѭ��
    public void PlaySoundEff(AudioClip clip, bool loop = false)
    {
        if (clip == null)
            return;
        AudioController audioController = AudioPool.Instance.GetController();
        audioController.SetSourceProperties(clip, effVolumScale, 1, loop, 0);
        audioController.Play();
    }

    //������ЧN���ֹͣ һ�����ڱ��������߼�
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

    //ֹͣĳ��Ч
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

    //������Ч����
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

    //����2d��Ч  eff  loop �Ƿ�ѭ��
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

        // ��������
        if (musicAudio.clip != null
            && musicAudio.isPlaying
            && musicAudio.clip.name == nameNoSuffix
            && musicAudio.gameObject.active)
            return true;

        // ��Ч
        if (AudioController.IsPlaying(nameNoSuffix))
            return true;

        return false;
    }
}
