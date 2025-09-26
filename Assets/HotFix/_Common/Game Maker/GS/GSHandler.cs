using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// ����������
/// </summary>
/// <remarks>
/// * ͬ�� GSHandler ���Բ��Ŷ�� GSSource<br/>
/// * �ṩ��������<br/>
/// * �ṩ����״̬����<br/>
/// </remarks>
[System.Serializable]
public class GSHandler 
{
    /// <summary> ����·�� </summary>
    public string assetPath;

    /// <summary> ���� </summary>
    public float volume = 0.7f;

    /// <summary> ������� </summary>
    public GSOutType outputType = GSOutType.SoundEffect;

    /// <summary> ѭ������ </summary>
    // [ShowIf("outputType", GSOutType.Music)]   
    // ���ھɰ�AOT����û�а��� ��Sirenix.OdinInspector.ShowIfAttribute�� �� �������ﲻ��ʹ�� ShowIf �������´��AOT����Ż���£�
    public bool loop = false;


    /// <summary> ���� </summary>
    public GSFadeInOut fadeIn = null;

    /// <summary> ���� </summary>
    public GSFadeInOut fadeOut = null;

    public float delayS = -1f;

    public GSPlayingType playingType = GSPlayingType.Independent;

    //[ShowIf("playingType", GSPlayingType.CountLimit)]
    // ���ھɰ�AOT����û�а��� ��Sirenix.OdinInspector.ShowIfAttribute�� �� �������ﲻ��ʹ�� ShowIf �������´��AOT����Ż���£�
    //[ConditionalShow("playingType", GSPlayingType.CountLimit)]
    public int countLimit = 0;

    /// <summary> �Զ��ͷ� </summary>
    bool isAutoRelease = true;

    //public string bundleName => "";
    //public string assetName => "";


    // ����������ͣ �ر� ����
    // ���ţ�ֹͣ����ͣ������


    UnityAction onClear;
    UnityAction onStop;
    UnityAction<bool> onMute;
    UnityAction<bool> onPause;
    //UnityAction onUnPause;

    public int UseCount { get; protected set; }

    /*
    public int UseCount { get; protected set; }
    public GSSource GetSource()
    {
        GSSource source = GSManager.Instance.GetSource();
        source.Initialize(this);
        ++UseCount;
        return source;
    }
    */


}

public enum GSOutType
{
    /// <summary> �������� </summary>
    Music,
    /// <summary> ��Ч </summary>
    SoundEffect,
};



public enum GSPlayingType
{
    /// <summary> �������� </summary>
    Independent,
    /// <summary> ֻ�����״β��ţ����У� </summary>
    FirstOnly,
    /// <summary> ���һ�β�����ֹͣ </summary>
    LastOnly,
    /// <summary> ���ͬʱ���� countLimit ������</summary>
    CountLimit
};

public enum GSEaseType
{
    None,
    /// <summary> ���Ա仯 </summary>
    Linear,
    /// <summary> ��ʼ�仯������Ȼ���𽥼��� </summary>
    EaseInQuad
};

[System.Serializable]
public class GSFadeInOut
{
    //[HorizontalGroup]
    //[HideLabel]
    public GSEaseType easeType;

    //[HorizontalGroup]
    //[HideLabel]
    //[HideIf("easeType", GSEaseType.None)]
    public float time;
}