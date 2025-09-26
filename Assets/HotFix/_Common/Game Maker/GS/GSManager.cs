using GameMaker;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 声音管理类
/// </summary>
public class GSManager : MonoSingleton<GSManager>
{
    public ObjectPool pool; // 每个pool的子对象有GSSource组件


    const string MARK_BUNDLE_SOUND_MANAGER = "MARK_BUNDLE_SOUND_MANAGER";


    private Dictionary<string, AudioClip> audioClipResDic = new Dictionary<string, AudioClip>();



    private float? _totalVolumMusic = null; //背景音乐声音大小比例
    private float? _totalVolumEff = null; //音效声音大小比例
    public float totalVolumMusic
    {
        get
        {
            if(_totalVolumMusic == null)
            {
                _totalVolumMusic = PlayerPrefs.GetFloat(TOTAL_VOLUM_MUSIC, 1f);
            }
            return (float)_totalVolumMusic;
        }
        set
        {
            _totalVolumMusic = value;
            PlayerPrefs.SetFloat(TOTAL_VOLUM_MUSIC, value);
        }
    }
    const string TOTAL_VOLUM_MUSIC = "TOTAL_VOLUM_MUSIC";

    public float totalVolumEff
    {
        get
        {
            if (_totalVolumEff == null)
            {
                _totalVolumEff = PlayerPrefs.GetFloat(TOTAL_VOLUM_EFF, 1f);
            }
            return (float)_totalVolumEff;
        }
        set
        {
            _totalVolumEff = value;
            PlayerPrefs.SetFloat(TOTAL_VOLUM_EFF, value);
        }
    }
    const string TOTAL_VOLUM_EFF = "TOTAL_VOLUM_EFF";




    public void SetTotalVolumMusic(float volum)
    {
        totalVolumMusic = volum;
        foreach (Transform tfm in pool.transform)
        {
            if (tfm.gameObject.active)
            {
                GSSource gss = tfm.GetComponent<GSSource>();
                if (gss.outputType == GSOutType.Music)
                    gss.ResetVolume();
            }
        }
    }

    public void SetTotalVolumEfft(float volum)
    {
        totalVolumEff = volum;
        foreach (Transform tfm in pool.transform)
        {
            if (tfm.gameObject.active)
            {
                GSSource gss = tfm.GetComponent<GSSource>();
                if (gss.outputType == GSOutType.SoundEffect)
                    gss.ResetVolume();
            }
        }
    }






    //解暂停
    public void UnPause()
    {
        foreach (Transform tfm in pool.transform)
        {
            if (tfm.gameObject.active)
            {
                tfm.GetComponent<GSSource>().UnPause();
            }
        }
    }
    //暂停所有声音
    public void Pause()
    {
        foreach (Transform tfm in pool.transform)
        {
            if (tfm.gameObject.active)
            {
                tfm.GetComponent<GSSource>().Pause();
            }
        }
    }
    //停止所有声音
    public void Stop()
    {
        foreach (Transform tfm in pool.transform)
        {
            if (tfm.gameObject.active)
            {
                tfm.GetComponent<GSSource>().Stop();
            }
        }
    }
    //设置静音
    public void SetMute(bool isMute)
    {
        foreach (Transform tfm in pool.transform)
        {
            if (tfm.gameObject.active)
            {
                tfm.GetComponent<GSSource>().Mute = isMute;
            }
        }
    }

    public void StopMusic()
    {
        foreach (Transform tfm in pool.transform)
        {
            if (tfm.gameObject.active)
            {
                GSSource gss = tfm.GetComponent<GSSource>();
                if (gss.outputType == GSOutType.Music)
                {
                    gss.Stop();
                }
            }
        }
    }
    public void StopSoundEff()
    {
        foreach (Transform tfm in pool.transform)
        {
            if (tfm.gameObject.active)
            {
                GSSource gss = tfm.GetComponent<GSSource>();
                if (gss.outputType == GSOutType.SoundEffect)
                {
                    gss.Stop();
                }
            }
        }
    }
    public void StopSound(string assetPath)
    {
        foreach (Transform tfm in pool.transform)
        {
            if (tfm.gameObject.active)
            {
                GSSource gss = tfm.GetComponent<GSSource>();
                if (gss.assetPath == assetPath)
                {
                    gss.Stop();
                }
            }
        }
    }


    bool IsPlaySound(string assetPath)
    {
        foreach (Transform tfm in pool.transform)
        {
            if (tfm.gameObject.active )
            {
                GSSource gss = tfm.GetComponent<GSSource>();
                if (gss.assetPath == assetPath)
                    return true;
            }
        }
        return false;
    }

    public int GetCount(string assetPath)
    {
        int count = 0;
        foreach (Transform tfm in pool.transform)
        {
            if (tfm.gameObject.active)
            {
                GSSource gss = tfm.GetComponent<GSSource>();
                if (gss.assetPath == assetPath)
                {
                    count++;
                }
            }
        }
        return count;   
    }



    public void PlayMusic(string assetPath, float volume, bool loop = true)
    {
        if (!audioClipResDic.ContainsKey(assetPath))
            audioClipResDic[assetPath] = ResourceManager.Instance.LoadAssetAtPath<AudioClip>(assetPath, MARK_BUNDLE_SOUND_MANAGER);

        GSSource gss = GetSource();
        gss.Initialize( audioClipResDic[assetPath], assetPath, volume, GSOutType.Music, loop);
        gss.Play();
    }


    public void PlaySound(GSHandler gsh)
    {
        switch (gsh.playingType)
        {
            case GSPlayingType.Independent:
                break;
            case GSPlayingType.FirstOnly:
                {
                    if(IsPlaySound(gsh.assetPath))
                        return;
                }
                break;
            case GSPlayingType.LastOnly:
                {
                    StopSound(gsh.assetPath);
                }
                break;
            case GSPlayingType.CountLimit:
                {
                    if (GetCount(gsh.assetPath) >= gsh.countLimit)
                        return;
                }
                break;
        }

        if (!audioClipResDic.ContainsKey(gsh.assetPath))
            audioClipResDic[gsh.assetPath] = ResourceManager.Instance.LoadAssetAtPath<AudioClip>(gsh.assetPath, MARK_BUNDLE_SOUND_MANAGER);

        GSSource source = GetSource(); //绑定自己和GSSource
        source.Initialize(audioClipResDic[gsh.assetPath],gsh);
        source.Play();
    }
    public void PlayMusicSingle(string assetPath, float volume, bool loop = false)
    {
        StopMusic();
        PlayMusic(assetPath, volume, loop);
    }


    public void PlaySoundEffSingle(string assetPath, float volume, bool loop = false)
    {
        StopSound(assetPath);
        PlaySoundEff(assetPath, volume, loop);
    }

    public void PlaySoundEff(string assetPath, float volume, bool loop = false)
    {

        if (!audioClipResDic.ContainsKey(assetPath))
        audioClipResDic[assetPath] = ResourceManager.Instance.LoadAssetAtPath<AudioClip>(assetPath, MARK_BUNDLE_SOUND_MANAGER);

        GSSource gss = GetSource();
        gss.Initialize( audioClipResDic[assetPath], assetPath,volume,GSOutType.SoundEffect, loop);
        gss.Play();
    }



    public GSSource GetSource()
    {
        GSSource gss = pool.GetObject().GetComponent<GSSource>();
        gss.transform.SetParent(pool.transform, false);
        //gss.startUseTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        return gss;
    }



    [Button]
    public void TestPlaySound(float volume = 0.7f)
    {
        GSHandler gsh = new GSHandler()
        {
            assetPath = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/BaseBackground.mp3",
            fadeIn = new GSFadeInOut()
            {
                easeType = GSEaseType.Linear,
                time = 3f,
            },
            fadeOut = new GSFadeInOut()
            {
                easeType = GSEaseType.Linear,
                time = 3f,
            },
            volume = volume,
        };

        Instance.PlaySound(gsh);
    }

    [Button]
    public void TestStopSound()
    {
        GSHandler gsh = new GSHandler()
        {
            assetPath = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Sounds/BaseBackground.mp3",
            fadeIn = new GSFadeInOut()
            {
                easeType = GSEaseType.Linear,
                time = 3f,

            },
            fadeOut = new GSFadeInOut()
            {
                easeType = GSEaseType.Linear,
                time = 3f,
            },
            //volume =   
        };

        Instance.StopSound(gsh.assetPath);
    }

    [Button]
    public void SetVolume(float volume = 1f)
    {
        SetTotalVolumEfft(volume);
        SetTotalVolumMusic(volume);
    }
}

