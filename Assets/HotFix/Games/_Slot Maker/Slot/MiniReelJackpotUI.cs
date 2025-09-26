

using System;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;


public enum MathType
{
    None,

    /// <summary>四舍五入到整数 </summary>
    Round,

    /// <summary>向零取整（直接丢弃小数部分） </summary>
    Truncate,

    /// <summary>向下取整（向更小的整数方向）</summary>
    /// <remarks>
    /// 18.87，结果是 18。
    /// -18.87，结果是 -19。
    /// </remarks>
    //Floor,

    /// <summary>向上取整（向更大的整数方向）</summary>
    /// <remarks>
    /// 18.87，结果是 19。
    /// -18.87，结果是 -18。
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
    /// * 将数值四舍五入 或 向零取整
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



    #region 自动弄大小  和 居中


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
