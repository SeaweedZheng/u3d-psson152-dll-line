using GameMaker;
using System.Collections;
using UnityEngine;


/// <summary>
/// ���������Ŀ�����
/// </summary>
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(PooledObject))]
public class GSSource : MonoBehaviour
{
    private AudioSource _source;
    private AudioSource source
    {
        get
        {
            if (_source == null)
                _source = GetComponent<AudioSource>();

            return _source;
        }
    }

    private bool isWaitingForCompletion = false;
    void Update()
    {
        if (isWaitingForCompletion && !source.isPlaying)
        {
            isWaitingForCompletion = false;
            OnAudioFinished();
        }
    }

    void OnAudioFinished()
    {
        Debug.Log("��Ƶ���Ž���");
        // ִ�к����߼����粥����һ����Ƶ�������¼���
    }


    /// <summary>
    /// ���ƸĶ����GSHandler
    /// </summary>
    public GSHandler Handler { get; set; }

    /// <summary> ��Դ·�� </summary>
    public string assetPath;

    public bool IsPlaying =>source.isPlaying;
        

    public bool IsFading => corFadeInOut != null;

    /// <summary> ���� </summary>
    public bool Mute
    {
        get => source.mute;
        set =>source.mute = value;
    }

    /// <summary> ��ǰ���� </summary>
    public float curVolume
    {
        get => source.volume;
        set => source.volume = value;
    }

    /// <summary> Ŀ������ </summary>
    //float targetVolume;

    float selfVolume = 0.7f;


    public GSOutType outputType = GSOutType.SoundEffect;

    public float GetTargetVolume()
    {
        switch (outputType)
        {
            case GSOutType.SoundEffect:
                return GSManager.Instance.totalVolumEff * selfVolume;
            case GSOutType.Music:
                return GSManager.Instance.totalVolumMusic * selfVolume;
            default:
                return 0f;
        }
    }


    bool isAutoRelease = true;

    /*
    public float Volume
    {
        get => source.volume;
        set =>source.volume = value;
    }
    */



    public void Initialize( AudioClip clip,GSHandler gsh)
    {
        this.Handler = gsh;
        Initialize( clip, gsh.assetPath, gsh.volume, gsh.outputType, gsh.loop);
    }

     public void Initialize( AudioClip clip, string assetPath,float volume, GSOutType outputType, bool loop)
    {
        //source.clip = Handler.clip.Load(); //��Դ�������clip

        this.assetPath = assetPath;
        this.selfVolume = volume;
        this.outputType = outputType;


        source.clip = clip;
        source.volume = GetTargetVolume();
        source.loop = loop;

        // ��������Ƶ���ŵ����ߣ������ߵͣ���
        //0.5������ 50 % ���ߣ��ͳ�Ч������
        //1.0��ԭʼ���ߣ�Ĭ��ֵ����
        //2.0����� 100 % ���ߣ�����Ч������
        source.pitch = 1.0f;
        //��λ�øк�������Ч����������Ƶ�Ŀռ仯�̶ȣ�������Ƶ�� 2D ���� 3D ���š�
        //0.0����ȫ 2D ��Ƶ�������ǿռ�λ�ã���������ƽ��̶�����
        //1.0����ȫ 3D ��Ƶ������ listener ����ƵԴ��λ�ü���������������Ч������
        //�м�ֵ����0.5������� 2D �� 3D Ч����
        source.spatialBlend = 0.0f;   
    }


    public void ResetVolume()
    {
        if (IsFading && !isFadeIn) //����ֱ�ӷ���
        {
            return;
        }
        if (IsFading) // �رյ���
        {
            StopCoroutine(corFadeInOut);
            corFadeInOut = null;
        }
        source.volume = GetTargetVolume();
    }


    /// <summary> ��ͣ </summary>
    public void Pause()
    {
        source.Pause();
    }


    /// <summary> ȡ����ͣ  </summary>
    public void UnPause()
    {
        source.UnPause();
    }



    public void Play()
    {
        if(Handler != null)
        {
            Play(Handler.delayS,
                Handler.fadeIn != null?  Handler.fadeIn.time : 0,
                Handler.fadeIn != null ? Handler.fadeIn.easeType: GSEaseType.None);
        }
        else
        {
            Play(-1, 0, GSEaseType.None);
        }
    }



    public void Play(float delay, float fadeInTimes, GSEaseType easeType)
    {
        if (delay > 0f)
            source.PlayDelayed(delay);
        else
            source.Play();

        if (corFadeInOut != null)
        {
            StopCoroutine(corFadeInOut);
            corFadeInOut = null;
        }

        if (fadeInTimes > 0f)  // ����
        {
            corFadeInOut = FadeInCo(fadeInTimes, easeType);
            StartCoroutine(corFadeInOut);
        }
        else
        {
            curVolume = GetTargetVolume();
        }
    }


    public void Stop()
    {
        if(Handler != null)
        {
            Stop(
            Handler.fadeOut != null ? Handler.fadeOut.time : 0,
            Handler.fadeOut != null ? Handler.fadeOut.easeType : GSEaseType.None);
        }
        else
        {
            Stop(0, GSEaseType.None);
        }
    }

    public void Stop(float fadeOutTimes, GSEaseType easeType)
    {
        if (corFadeInOut != null)
        {
            StopCoroutine(corFadeInOut);
            corFadeInOut = null;
        }

        if (fadeOutTimes > 0f)
        {
            corFadeInOut = FadeOutCo(fadeOutTimes, easeType);
            StartCoroutine(corFadeInOut);
        }
        else
        {
            source.Stop();
        }
    }


    /// <summary>ֹͣ���ţ������GSHandler�İ󶨣�����Ԥ���</summary>
    public void Clear()
    {
        ReturnToPool();
    }

    private void ReturnToPool()
    {
        if (source != null && source.isPlaying == true) { }
        source.Stop();

        if (corFadeInOut != null)
            StopCoroutine(corFadeInOut);
        corFadeInOut = null;

        /*
        Handler.onVolumeChanged -= OnVolumeChanged;
        Handler.onMute -= OnMute;
        Handler.onStop -= Stop;
        Handler.onPause -= Pause;
        Handler.onUnPause -= UnPause;
        Handler.onClear -= Clear;

        Handler.clip.UnLoad();
        Handler.OnDestroySource(this);
        Handler = null;
        */

        Handler = null;

        if (source != null)
            source.clip = null;

    

        GetComponent<PooledObject>().ReturnToPool();
    }









    bool isFadeIn = false;
    private IEnumerator corFadeInOut;

    /// <summary>
    /// �������
    /// </summary>
    /// <param name="handler"></param>
    /// <returns></returns>
    private IEnumerator FadeInCo(float fadeInTimes, GSEaseType easeType)
    {
        isFadeIn = true;
        curVolume = 0f;
        AnimationCurve curve = null;
        switch (easeType)
        {
            case GSEaseType.Linear:
                {
                    curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
                }
                break;
            case GSEaseType.EaseInQuad:
                {
                    curve = EaseInQuad();
                }
                break;
        }
        float startVolume = curVolume;
        float volumeRange = GetTargetVolume() - startVolume;
        var startTime = UnityEngine.Time.time;
        var animationTime = fadeInTimes;

        while (true)
        {
            float deltaTime = UnityEngine.Time.time - startTime;
            curVolume = curve.Evaluate(deltaTime / animationTime) * volumeRange + startVolume;
            
            if (deltaTime >= animationTime)
                break;

            yield return null;
        }
        corFadeInOut = null;
    }


    /// <summary>
    /// ������С
    /// </summary>
    /// <param name="handler"></param>
    /// <returns></returns>
    private IEnumerator FadeOutCo(float fadeOutTimes, GSEaseType easeType)
    {
        isFadeIn = false;

        // AnimationCurve curve = GSManager.Instance.GetEaseCurve(Handler.fadeOut.easeType);
        AnimationCurve curve = null;

        switch (easeType)
        {
            case GSEaseType.Linear:
                {
                    curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
                }
                break;
            case GSEaseType.EaseInQuad:
                {
                    curve = EaseInQuad();

                    //[���뻺���仯]��ʼ�ͽ���ʱ�仯�������м�仯���
                    //curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
                }
                break;
        }

        float startVolume = curVolume;
        float volumeRange = startVolume;
        var startTime = UnityEngine.Time.time;
        var animationTime = fadeOutTimes;

        while (true)
        {
            float deltaTime = UnityEngine.Time.time - startTime;
            curVolume = startVolume - (curve.Evaluate(deltaTime / animationTime) * volumeRange);
            if (deltaTime >= animationTime)
                break;

            yield return null;
        }

        source.Stop();
        corFadeInOut = null;
    }


    AnimationCurve EaseInQuad()
    {
        return new AnimationCurve(
            new Keyframe(0f, 0f, 0f, 0f), // ��㣺����Ϊ0��ˮƽ��
            new Keyframe(1f, 1f, 2f, 0f)  // �յ㣺����Ϊ2�����٣�
        );
    }



    private void LateUpdate()
    {
        if (isAutoRelease && !source.isPlaying)
        {
            ReturnToPool();
        }
    }
}
