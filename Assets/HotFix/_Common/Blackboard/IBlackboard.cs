using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICustomBlackboard
{
    /// <summary> ͼ��� </summary>
    public float symbolWidth { get; }

    /// <summary> ͼ��� </summary>
    public float symbolHeight { get; }

    /// <summary> �� </summary>
    public int column { get; }

    /// <summary> �� </summary>
    public int row { get; }

    public float reelMaxOffsetY { get; }

    /// <summary> ˵��ҳ </summary>
    //## public string[] payTable { get; }

    /// <summary> ͨ��ͼ����������ȡͼ����ʵ��� </summary>
    public List<int> symbolNumber { get; }

    /// <summary> ����ͼ����� </summary>
    public int symbolCount { get; }

    /// <summary> Ԥ�������� - ͼ���н���Ч</summary>
    public Dictionary<string, string> symbolHitEffect { get; }

    /// <summary> ����ͼ�� </summary>
    public List<int> specialHitSymbols { get; }

    /// <summary> ��Чͼ�� - Ԥ��������</summary>
    public Dictionary<string, string> symbolAppearEffect { get; }

    /// <summary> Ԥ�������� - ͼ���н���Ч</summary>
    public Dictionary<string, string> symbolExpectationEffect { get; }

    /// <summary> �߿���Ч</summary>
    public string borderEffect { get; }

    /// <summary> ͼƬ - Ĭ��ͼ��</summary>
    //## public Dictionary<string, string> symbolIcon { get; }

}
