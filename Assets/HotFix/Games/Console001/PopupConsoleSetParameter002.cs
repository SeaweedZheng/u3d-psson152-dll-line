using Game;
using GameMaker;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System;

public class PopupConsoleSetParameter002 : PageMachineButtonBase
{
    public TextMeshProUGUI tmpTitle, tmpParamName1, tmpTip1, tmpParamName2, tmpTip2;

    public Button btnClose, btnConfirm;

    public CompInputController compInput1, compInput2;


    Func<string, string> checkParam1Func;
    Func<string, string> checkParam2Func;
    void Awake()
    {
        btnConfirm.onClick.AddListener(OnClickConfirm);
        btnClose.onClick.AddListener(OnClickClose);
    }

    void OnClickClose()
    {
        PageManager.Instance.ClosePage(this, new EventData("Exit"));
    }


    public override void OnOpen(PageName name, EventData data)
    {
        base.OnOpen(name, data);

        InitParam();

        Dictionary<string, object> argDic = null;
        if (data != null)
        {
            argDic = (Dictionary<string, object>)data.value;
            if (argDic.ContainsKey("title"))
            {
                tmpTitle.text = (string)argDic["title"];
            }
            if (argDic.ContainsKey("paramName1"))
            {
                tmpParamName1.text = (string)argDic["paramName1"];
            }
            if (argDic.ContainsKey("paramName2"))
            {
                tmpParamName2.text = (string)argDic["paramName2"];
            }

            if (argDic.ContainsKey("checkParam1Func"))
            {
                checkParam1Func = (Func<string, string>)argDic["checkParam1Func"];
            }

            if (argDic.ContainsKey("checkParam2Func"))
            {
                checkParam2Func = (Func<string, string>)argDic["checkParam2Func"];
            }
        }
    }


    void InitParam()
    {
        compInput1.value = "";
        tmpTip1.text = "";

        compInput2.value = "";
        tmpTip2.text = "";

        checkParam1Func = (res) => null;
        checkParam2Func = (res) => null;
    }


    void OnClickConfirm()
    {
        tmpTip1.text = "";
        tmpTip2.text = "";

        string msg1 = checkParam1Func(compInput1.value);
        if (!string.IsNullOrEmpty(msg1))
        {
            TipPopupHandler.Instance.OpenPopup(msg1);
            tmpTip1.text = msg1;
            return;
        }


        string msg2 = checkParam2Func(compInput2.value);
        if (!string.IsNullOrEmpty(msg2))
        {
            TipPopupHandler.Instance.OpenPopup(msg2);
            tmpTip2.text = msg2;
            return;
        }

        List<string> list = new List<string>() { compInput1.value, compInput2.value };
        PageManager.Instance.ClosePage(this, new EventData<List<string>>("Result", list));

    }

}
