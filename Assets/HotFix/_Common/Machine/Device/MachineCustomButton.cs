using System.Collections.Generic;
using System;
using UnityEngine;
public enum MachineButtonType
{
    /// <summary> ���ư�ť </summary>
    Regular,
    /// <summary> ���ư�ť </summary>
    Light,
}

[System.Serializable]
public class MachineCustomButton
{
    /* public const string SOUND_DEFAULT = "UI_Button_Normal";
     public Dictionary<MachineButtonKey, string> dicBtnAndSound = new Dictionary<MachineButtonKey, string>()
     {
         [MachineButtonKey.BtnPre] = SOUND_DEFAULT,
         [MachineButtonKey.BtnNext] = SOUND_DEFAULT,
         [MachineButtonKey.BtnSpin] = SOUND_DEFAULT,
     };*/

    public MachineCustomButton()
    {
    }

    /// <summary> ��û�̨��ť�۽� </summary>
    public const string MACHINE_CUSTOM_BUTTON_FOCUS_EVENT = "MACHINE_CUSTOM_BUTTON_FOCUS_EVENT";

    /// <summary> ��ť���� </summary>
    public MachineButtonType btnType = MachineButtonType.Regular;

    /// <summary> ���ư�ť�б�</summary>
    public List<MachineButtonKey> btnShowLst = new List<MachineButtonKey>()
    {
        MachineButtonKey.BtnPre,
        MachineButtonKey.BtnNext,
        MachineButtonKey.BtnSpin,
    };

    /// <summary> �Ƿ���ʾ��ť�� </summary>
   // public bool isShowBtn = true;

    /// <summary> �Ƶĸ��� </summary>
    public int numlightBtn = 0;

    /// <summary> ��� </summary>
    string _mark = null;
    public string mark
    {
        get{
            if (string.IsNullOrEmpty(_mark))
                _mark = Guid.NewGuid().ToString();
            return _mark;
        }
        set => _mark = value;
    }

    /// <summary> �Ƿ����ȴ��� </summary>
    public bool isPriority = false;
}
