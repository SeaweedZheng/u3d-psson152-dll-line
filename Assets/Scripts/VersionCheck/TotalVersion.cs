using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TotalVersion
{
    public long updated_at;
    public List<TotalVersionItem> data;
}

[System.Serializable]
public class TotalVersionItem
{
    /// <summary> 键 </summary>
    public string app_key;
    /// <summary> 包名 </summary>
    public string app_name;
    /// <summary> 包版本 </summary>
    public string app_version;
    /// <summary> 包类型 </summary>
    public string app_type;
    /// <summary> app允许游玩 </summary>
    public bool app_enable;
    /// <summary> 热更新版本 </summary>
    public string hotfix_version;
    /// <summary> 下载路劲（先对路劲 或 绝对路劲） </summary>
    public string hotfix_url;
    /// <summary> 描述 </summary>
    public string des;
    /// <summary> 修改时间 </summary>
    public long updated_at = 0;
    /// <summary> 代理商 </summary>
    public string agent;
    /// <summary> 运行系统 </summary>
    public string system;
    /// <summary> 是否机台包 </summary>
    public bool is_machine;
    /// <summary> app更新建议 </summary>
    public string app_update_suggest;
}