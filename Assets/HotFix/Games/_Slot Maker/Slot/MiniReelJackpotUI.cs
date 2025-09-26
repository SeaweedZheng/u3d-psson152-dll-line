

using System;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;


public enum MathType
{
    None,

    /// <summary>�������뵽���� </summary>
    Round,

    /// <summary>����ȡ����ֱ�Ӷ���С�����֣� </summary>
    Truncate,

    /// <summary>����ȡ�������С����������</summary>
    /// <remarks>
    /// 18.87������� 18��
    /// -18.87������� -19��
    /// </remarks>
    //Floor,

    /// <summary>����ȡ������������������</summary>
    /// <remarks>
    /// 18.87������� 19��
    /// -18.87������� -18��
    /// </remarks>
    //Ceiling,
}

public class MiniReelJackpotUI : JackpotUI
{
    private MiniReelGroup miniReelGroup => goText?.GetComponent<MiniReelGroup>();


    public MathType mathType = MathType.None;


    /// <summary>  </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <remarks>
    /// * ����ֵ�������� �� ����ȡ��
    /// </remarks>
    public float GetMathData(float value)
    {
        switch (mathType)
        {
            case MathType.None:
                return value;
            case MathType.Round:
                return (float)Math.Round(value);
            case MathType.Truncate:
                return (float)Math.Truncate(value);
        }
        return -9898;
    }


    public override float nowCredit
    {
        get => miniReelGroup.nowData;
        //set => miniReelGroup.SetData(value);
    }

    public override void AddToData(string str)
    {
        AddToData(float.Parse(str));
    }
    public void AddToData(float toCredit)
    {
        toCredit = GetMathData(toCredit);

        if (miniReelGroup != null)
        {
            base.toCredit = toCredit;
            miniReelGroup.AddToData(toCredit);
            ChangeAlign(toCredit);
        }
    }
    public void AddToData(float formCredit, float toCredit)
    {
        formCredit = GetMathData(formCredit);
        toCredit = GetMathData(toCredit);

        if (miniReelGroup != null)
        {
            base.toCredit = toCredit;
            miniReelGroup.AddToData(formCredit, toCredit);
            ChangeAlign(toCredit);
        }
    }


    public void ChangeAddToData(float toCredit)
    {
        toCredit = GetMathData(toCredit);

        if (miniReelGroup != null)
        {
            base.toCredit = toCredit;
            miniReelGroup.ChangeAddToData(toCredit);
            ChangeAlign(toCredit);
        }
    }



    public void SetData(float toCredit)
    {
        toCredit = GetMathData(toCredit);

        if (miniReelGroup != null)
        {
            base.toCredit = toCredit;
            miniReelGroup.SetData(toCredit);
            ChangeAlign(toCredit);
        }
    }


    RectTransform rtfmMiniReelGroup;
    public void Awake()
    {
        rtfmMiniReelGroup = miniReelGroup.GetComponent<RectTransform>();
        oldx = rtfmMiniReelGroup.localPosition.x;
    }

    public override void Start()
    {
        /*
        miniReelGroup.SetData(0);
        yield return new WaitForSeconds(5);
        miniReelGroup.AddToData(toCredit);*/
    }


    public override void Update()
    {

    }



    #region �Զ�Ū��С  �� ����


    int lastDataLength = 0;

    float oldx = 0;
    void ChangeAlign(float data)
    {
        int curDataLength = ((int)data).ToString().Length;
        if (curDataLength == lastDataLength)
            return;
        lastDataLength = curDataLength;

        string str = data.ToString(miniReelGroup.format);
        string numberStr = str.Replace(",", "").Replace(".", "");

        float width = rtfmMiniReelGroup.sizeDelta.x;

        float def = ((8 - numberStr.Length) * width / 8) * 0.55f;
        if (def > 0)
        {
            rtfmMiniReelGroup.localPosition = new Vector3(oldx - def, rtfmMiniReelGroup.localPosition.y);
        }
    }


    #endregion
}
