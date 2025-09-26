using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICustomBlackboard
{
    /// <summary> 图标宽 </summary>
    public float symbolWidth { get; }

    /// <summary> 图标高 </summary>
    public float symbolHeight { get; }

    /// <summary> 列 </summary>
    public int column { get; }

    /// <summary> 行 </summary>
    public int row { get; }

    public float reelMaxOffsetY { get; }

    /// <summary> 说明页 </summary>
    //## public string[] payTable { get; }

    /// <summary> 通过图标索引，获取图标真实编号 </summary>
    public List<int> symbolNumber { get; }

    /// <summary> 所有图标个数 </summary>
    public int symbolCount { get; }

    /// <summary> 预制体名称 - 图标中奖特效</summary>
    public Dictionary<string, string> symbolHitEffect { get; }

    /// <summary> 特殊图标 </summary>
    public List<int> specialHitSymbols { get; }

    /// <summary> 特效图标 - 预制体名称</summary>
    public Dictionary<string, string> symbolAppearEffect { get; }

    /// <summary> 预制体名称 - 图标中奖特效</summary>
    public Dictionary<string, string> symbolExpectationEffect { get; }

    /// <summary> 边框特效</summary>
    public string borderEffect { get; }

    /// <summary> 图片 - 默认图标</summary>
    //## public Dictionary<string, string> symbolIcon { get; }

}
