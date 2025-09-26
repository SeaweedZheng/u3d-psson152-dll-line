using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeColorSettings
{
    #region 弹窗配色
    /// <summary> 弹窗背景选中高亮颜色（亮绿: 00FD8D）</summary>
    public static readonly Color HIGHLIGHT_ITEM_BG = new Color(0f / 255f, 253f / 255f, 141f / 255f, 255f / 255f);

    /// <summary> 弹窗背景默认颜色（深暗绿: 09221F）</summary>
    public static readonly Color NORMAL_ITEM_BG = new Color(9f / 255f, 34f / 255f, 31f / 255f, 255f / 255f);

    /// <summary> 弹窗前景默认颜色（浅暗绿: 143733）</summary>
    public static readonly Color NORMAL_POPUP_BG = new Color(20f / 255f, 55f / 255f, 51f / 255f, 255f / 255f);

    #endregion

    #region 按钮颜色
    /// <summary> 灰色按钮关闭（深灰色：333333）</summary>
    public static readonly Color DISABLE_BUTTOM_BG = new Color(51f / 255f, 51f / 255f, 51f / 255f, 255f / 255f);

    /// <summary> 灰色按钮高亮（亮灰色：808080）</summary>
    public static readonly Color HIGHLIGHT_BUTTOM_BG = new Color(128f / 255f, 128f / 255f, 128f / 255f, 255f / 255f);

    /// <summary> 灰色按钮正常（正常灰色：606165）</summary>
    public static readonly Color NORMAL_BUTTOM_BG = new Color(96f / 255f, 97f / 255f, 101f / 255f, 255f / 255f);


    /// <summary> 键盘绿色按钮正常（色：59FF00）</summary>
    public static readonly Color NORMAL_BUTTON_GREEN_BG = new Color(89f / 255f, 255f / 255f, 0f / 255f, 255f / 255f);

    /// <summary> 键盘黄色按钮正常（色：FFEB00）</summary>
    public static readonly Color NORMAL_BUTTON_YELLOW_BG = new Color(255f / 255f, 235f / 255f, 0f / 255f, 255f / 255f);

    /// <summary> 键盘红色按钮正常（色：FF344D）</summary>
    public static readonly Color NORMAL_BUTTON_RED_BG = new Color(255f / 255f, 52f / 255f, 77f / 255f, 255f / 255f);

    /// <summary> 设置按钮正常颜色（浅粉：B8A8B2）</summary>
    public static readonly Color NORMAL_BUTTON_PINK_BG = new Color(184f / 255f, 168f / 255f, 178f / 255f, 255f / 255f);

    /// <summary> 设置按钮关闭颜色（深粉：6A6668）</summary>
    public static readonly Color DISABLE_BUTTON_PINK_BG = new Color(106f / 255f, 102f / 255f, 104f / 255f, 255f / 255f);

    // C8C8C8
    #endregion

    #region 滑动条

    /// <summary> 滑动条冒颜色（色：FFFFFF）</summary>
    public static readonly Color SLIDER_HANDLE_COLOR = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);

    /// <summary> 滑动条填充粉色（浅粉：F9B5C2）</summary>
    public static readonly Color SLIDER_FILL_COLOR = new Color(249f / 255f, 181f / 255f, 194f / 255f, 255f / 255f);

    /// <summary> 滑动条背景颜色（深粉：C8C8C8）</summary>
    public static readonly Color SLIDER_BG_COLOR = new Color(200f / 255f, 200f / 255f, 200f / 255f, 255f / 255f);

    #endregion


    #region 字体配色
    /// <summary> 常用字体默认颜色（白色：FFFFFF）</summary>
    public static readonly Color NORMAL_TXT = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);

    /// <summary> 常用字体不使能颜色（灰色：808080）</summary>
    public static readonly Color DISABLE_TXT = new Color(128f / 255f, 128f / 255f, 128f / 255f, 255f / 255f);


    /// <summary> 提示字绿色（绿：4AFF00）</summary>
    public static readonly Color TIP_GREEN_TEXY = new Color(74f / 255f, 255f / 255f, 0f / 255f, 255f / 255f);

    /// <summary> 提示字黄色（黄：FFFF00）</summary>
    public static readonly Color TIP_YELLOW_TEXY = new Color(255f / 255f, 255f / 255f, 0f / 255f, 255f / 255f);

    #endregion


    #region 控台配色
    /// <summary> 控台背景颜色（灰色：333333）</summary>
    public static readonly Color PAGE_CONSOLE_BG = new Color(51f / 255f, 51f / 255f, 51f / 255f, 255f / 255f);

    /// <summary> 控台前景颜色（浅蓝：347591）</summary>
    public static readonly Color PAGE_CONSOLE_FG = new Color(52f / 255f, 117f / 255f, 145f / 255f, 255f / 255f);

    /// <summary> 控台底部前景颜色（浅绿：114550）</summary>
    public static readonly Color PAGE_CONSOLE_BUTTON_BG = new Color(17f / 255f, 69f / 255f, 80f / 255f, 255f / 255f);

    /// <summary> 控台前景颜色（灰色：4A4C4B）</summary>
    public static readonly Color PAGE_CONSOLE_TITLE_BG = new Color(74f / 255f, 76f / 255f, 75f / 255f, 255f / 255f);

    /// <summary> 控台按钮选择高亮颜色（高亮绿：68FFD6）</summary>
    public static readonly Color PAGE_CONSOLE_TITLE_HIGHLIGHT_TXT = new Color(104f / 255f, 255f / 255f, 214f / 255f, 255f / 255f);


    #endregion


}
