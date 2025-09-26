using Game;
using GameMaker;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
public class PopupConsoleSetParameter001 : PageMachineButtonBase
{
    public TextMeshProUGUI tmpTitle, tmpParamName1, tmpTip1;

    public Button btnClose, btnConfirm;

    public CompInputController compInput1;


    Func<string, string> checkParam1Func;

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
            if (argDic.ContainsKey("checkParam1Func"))
            {
                checkParam1Func = (Func<string, string>)argDic["checkParam1Func"];
            }
        }
    }


    void InitParam()
    {
        compInput1.value = "";
        tmpTip1.text = "";
        checkParam1Func = (res) => null;

    }


    void OnClickConfirm()
    {
        tmpTip1.text = "";

        string msg1 = checkParam1Func(compInput1.value);
        if (!string.IsNullOrEmpty(msg1))
        {
            TipPopupHandler.Instance.OpenPopup(msg1);
            tmpTip1.text = msg1;
            return;
        }

        string res = compInput1.value;
        PageManager.Instance.ClosePage(this, new EventData<string>("Result", res));
    }

}
