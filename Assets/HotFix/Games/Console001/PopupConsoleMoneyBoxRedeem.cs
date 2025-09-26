using Game;
using GameMaker;
using MoneyBox;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using _consoleBB = PssOn00152.ConsoleBlackboard02;

public class PopupConsoleMoneyBoxRedeem : PageMachineButtonBase
{
    public Button btnClose , btnConfirm;

    public Transform btnSelectAnchor;

    public Button btnInputMoney;

    public TextMeshProUGUI tmpMyCredit, tmpRedeemRate;

    Text txtInputMoney;

    void Awake()
    {
        btnClose.onClick.AddListener(OnClickClose);
        btnConfirm.onClick.AddListener(OnClickConfirm);

        btnInputMoney.onClick.AddListener(OnclickInputMoney);

        txtInputMoney = btnInputMoney.transform.Find("Text").GetComponent<Text>();
    }
    void Start()
    {
        
    }


  

    List<int> btnSelectDatas = new List<int>() { 100, 200, 500, 1000 };
    private void OnEnable()
    {
        btnSelectDatas.Clear();
        foreach (int coinInNum in MoneyBoxModel.Instance.coinInNumLst)
        {
            int credit = coinInNum * _consoleBB.Instance.coinInScale;
            btnSelectDatas.Add(credit);
        }

        GameObject goClone = btnSelectAnchor.GetChild(0).gameObject;
        for (int i = btnSelectAnchor.childCount; i< btnSelectDatas.Count; i++ )
        {
            GameObject goNew = GameObject.Instantiate(goClone);
            goNew.transform.SetParent(btnSelectAnchor);
        }
        foreach (Transform btn in btnSelectAnchor)
        {
            btn.gameObject.SetActive(false);
        }
        for (int i = 0; i < btnSelectDatas.Count; i++)
        {
            Transform tfmChd = btnSelectAnchor.GetChild(i);
            string name = $"No.{btnSelectDatas[i]}";
            tfmChd.name = name;
            tfmChd.Find("Text").GetComponent<TextMeshProUGUI>().text = $"{btnSelectDatas[i]}"; 
            tfmChd.gameObject.SetActive(true);
            Button btn = tfmChd.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => { OnClickButtonSelect(name); });

            btn.interactable = _consoleBB.Instance.myCredit >= btnSelectDatas[i];
        }
    }



    public override void OnOpen(PageName name, EventData data)
    {
        base.OnOpen(name, data);

        txtInputMoney.text = "";
        tmpMyCredit.text = $"{_consoleBB.Instance.myCredit}";
        float redeemRate = MoneyBoxModel.Instance.moneyPerCoinIn;
        tmpRedeemRate.text = $"{redeemRate}";
    }



    void OnClickClose()
    {
        PageManager.Instance.ClosePage(this, new EventData("Exit"));
    }


    int inputCredit;
    void OnClickButtonSelect(string value)
    {
        inputCredit = int.Parse(value.Replace("No.",""));
        
        txtInputMoney.text = inputCredit.ToString();
    }


    void OnClickConfirm()
    {
        MachineDeviceCommonBiz.Instance.DoMoneyOut(inputCredit);
    }


    async void OnclickInputMoney()
    {

        EventData res = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleKeyboard001,
        new EventData<Dictionary<string, object>>("",
            new Dictionary<string, object>()
            {
                ["title"] = I18nMgr.T("Redeem Game Credits"),
                ["isPlaintext"] = true,
            }));

        if (res.value != null)
        {
            try
            {
                // Credit out
                int creditOut = int.Parse((string)res.value);  // (long)res.value;

                if (creditOut > _consoleBB.Instance.myCredit)
                    creditOut = (int)_consoleBB.Instance.myCredit;

                MoneyBoxMoneyInfo info = DeviceUtils.GetMoneyOutInfo(creditOut);
                txtInputMoney.text = info.asCredit.ToString();
            }
            catch { }

        }

    }

}
