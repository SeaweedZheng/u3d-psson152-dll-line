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
    /// <summary> �� </summary>
    public string app_key;
    /// <summary> ���� </summary>
    public string app_name;
    /// <summary> ���汾 </summary>
    public string app_version;
    /// <summary> ������ </summary>
    public string app_type;
    /// <summary> app�������� </summary>
    public bool app_enable;
    /// <summary> �ȸ��°汾 </summary>
    public string hotfix_version;
    /// <summary> ����·�����ȶ�·�� �� ����·���� </summary>
    public string hotfix_url;
    /// <summary> ���� </summary>
    public string des;
    /// <summary> �޸�ʱ�� </summary>
    public long updated_at = 0;
    /// <summary> ������ </summary>
    public string agent;
    /// <summary> ����ϵͳ </summary>
    public string system;
    /// <summary> �Ƿ��̨�� </summary>
    public bool is_machine;
    /// <summary> app���½��� </summary>
    public string app_update_suggest;
}