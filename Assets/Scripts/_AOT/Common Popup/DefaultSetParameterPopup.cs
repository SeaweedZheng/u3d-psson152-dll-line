using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class SetParameterPopupInfo
{
    public string title;
    public string btnConfirmName = "确定";
    public List<ParamInfo> paramLst;
    public Action<string> onFinishCallback;
}

public class ParamInfo
{
    public string name;
    public string value = "";
    public bool isPlaintext = true;
    public Func<string, string> tipFunc;
}

public class DefaultSetParameterPopup: DefaultBasePopup
{
    static DefaultSetParameterPopup _instance;

    public static DefaultSetParameterPopup Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new DefaultSetParameterPopup();
            }
            return _instance;
        }
    }


    CompInputDefaultController inpCtrl01, inpCtrl02;
    CompKeyboardDefaultController kbCtrl;

    Button btnClose, btnConfirm;

    Text txtTitle, txtTip01 , txtParamName01, txtTip02, txtParamName02;


    SetParameterPopupInfo info;
    public void OpenPopup(SetParameterPopupInfo info)
    {
        this.info = info;
        base.OnOpen("Common/Prefabs/Popup Set Parameter");

        if (info.paramLst.Count >=2)
        {
            goPopup.transform.Find("Anchor/Custom Input (1)").gameObject.SetActive(false);
            goPopup.transform.Find("Anchor/Custom Input (2)").gameObject.SetActive(true);

            kbCtrl = goPopup.transform.Find("Anchor/Custom Keyboard Default").GetComponent<CompKeyboardDefaultController>();
            btnClose = goPopup.transform.Find("Anchor/Custom Input (2)/Button Close").GetComponent<Button>();
            btnConfirm = goPopup.transform.Find("Anchor/Custom Input (2)/Anchor/Button Confirm").GetComponent<Button>();
            txtTitle = goPopup.transform.Find("Anchor/Custom Input (2)/Anchor/Title").GetComponent<Text>();

            txtTitle.text = info.title;
            btnConfirm.onClick.RemoveAllListeners();
            btnConfirm.onClick.AddListener(OnClickButtonConfirm02);

            // 参数1：
            inpCtrl01 = goPopup.transform.Find("Anchor/Custom Input (2)/Anchor/Input (1)").GetComponent<CompInputDefaultController>();
            txtTip01 = goPopup.transform.Find("Anchor/Custom Input (2)/Anchor/Tip (1)").GetComponent<Text>();
            txtParamName01 = goPopup.transform.Find("Anchor/Custom Input (2)/Anchor/Param Name (1)").GetComponent<Text>();

            //inpCtrl01.value = "";
            inpCtrl01.GetFocus();
            txtParamName01.text = info.paramLst[0].name;
            inpCtrl01.value = info.paramLst[0].value;
            inpCtrl01.isPlaintext = info.paramLst[0].isPlaintext;
            txtTip01.text = "";


            // 参数2：
            inpCtrl02 = goPopup.transform.Find("Anchor/Custom Input (2)/Anchor/Input (2)").GetComponent<CompInputDefaultController>();
            txtTip02 = goPopup.transform.Find("Anchor/Custom Input (2)/Anchor/Tip (2)").GetComponent<Text>();
            txtParamName02 = goPopup.transform.Find("Anchor/Custom Input (2)/Anchor/Param Name (2)").GetComponent<Text>();

            //inpCtrl02.value = "";
            txtParamName02.text = info.paramLst[1].name;
            inpCtrl02.value = info.paramLst[1].value;
            inpCtrl02.isPlaintext = info.paramLst[1].isPlaintext;
            txtTip02.text = "";


            btnClose.onClick.RemoveAllListeners();
            btnClose.onClick.AddListener(() =>
            {
                ClosePopup();
                info.onFinishCallback?.Invoke(null);
            });
        }
        else
        {
            goPopup.transform.Find("Anchor/Custom Input (1)").gameObject.SetActive(true);
            goPopup.transform.Find("Anchor/Custom Input (2)").gameObject.SetActive(false);

            kbCtrl = goPopup.transform.Find("Anchor/Custom Keyboard Default").GetComponent<CompKeyboardDefaultController>();
            btnClose = goPopup.transform.Find("Anchor/Custom Input (1)/Button Close").GetComponent<Button>();
            btnConfirm = goPopup.transform.Find("Anchor/Custom Input (1)/Anchor/Button Confirm").GetComponent<Button>();
            txtTitle = goPopup.transform.Find("Anchor/Custom Input (1)/Anchor/Title").GetComponent<Text>();

            txtTitle.text = info.title; 
            btnConfirm.onClick.RemoveAllListeners();
            btnConfirm.onClick.AddListener(OnClickButtonConfirm01);


            inpCtrl01 = goPopup.transform.Find("Anchor/Custom Input (1)/Anchor/Input (1)").GetComponent<CompInputDefaultController>();
            txtTip01 = goPopup.transform.Find("Anchor/Custom Input (1)/Anchor/Tip (1)").GetComponent<Text>();
            txtParamName01 = goPopup.transform.Find("Anchor/Custom Input (1)/Anchor/Param Name (1)").GetComponent<Text>();

            //inpCtrl01.value = "";
            inpCtrl01.GetFocus();
            txtParamName01.text = info.paramLst[0].name;
            inpCtrl01.value = info.paramLst[0].value;
            inpCtrl01.isPlaintext = info.paramLst[0].isPlaintext;
            txtTip01.text = "";



            btnClose.onClick.RemoveAllListeners();
            btnClose.onClick.AddListener(() =>
            {
                ClosePopup();
                info.onFinishCallback?.Invoke(null);
            });
        }


    }


    void OnClickButtonConfirm02()
    {
        Func<string, string> tipFunc1 = info.paramLst[0].tipFunc;
        string tipMsg1 = null;
        if (tipFunc1 != null)
        {
            tipMsg1 = tipFunc1?.Invoke(inpCtrl01.value);
        }


        Func<string, string> tipFunc2 = info.paramLst[1].tipFunc;
        string tipMsg2 = null;
        if (tipFunc2 != null)
        {
            tipMsg2 = tipFunc2?.Invoke(inpCtrl02.value);
        }


        if (!string.IsNullOrEmpty(tipMsg1))
        {
            txtTip01.text = tipMsg1;
        }
        else if (!string.IsNullOrEmpty(tipMsg2))
        {
            txtTip02.text = tipMsg2; 
        }
        else{
            ClosePopup();
            info.onFinishCallback?.Invoke($"{inpCtrl01.value}#{inpCtrl02.value}");
        }
    }

    void OnClickButtonConfirm01()
    {
        Func<string, string> tipFunc1 = info.paramLst[0].tipFunc;
        string tipMsg1 = null;
        if (tipFunc1 != null)
        {
            tipMsg1 = tipFunc1?.Invoke(inpCtrl01.value);
        }
        
        
        if(!string.IsNullOrEmpty(tipMsg1))
        {
            txtTip01.text = tipMsg1;
        }
        else
        {
            ClosePopup();
            info.onFinishCallback?.Invoke(inpCtrl01.value);
        }
    }

}
