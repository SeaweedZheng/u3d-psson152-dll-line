using Game;
using GameMaker;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupConsoleSlideSetting001 : PageMachineButtonBase
{
    public GameObject goContent, goClose, goButton1,goButtonKeyboard;
    public Slider slider;

    TextMeshProUGUI tmpTitle, tmpContetn;
    void Awake()
    {
        goClose.GetComponent<Button>().onClick.AddListener(OnClickClose);

        goButton1.GetComponent<Button>().onClick.AddListener(OnClickConfirm);

        goButtonKeyboard.GetComponent<Button>().onClick.AddListener(OnClickKeyboard);

        tmpContetn = goContent.transform.GetComponent<TextMeshProUGUI>();

        slider.onValueChanged.AddListener(OnSlide);
    }

    void OnClickClose()
    {
        PageManager.Instance.ClosePage(this, new EventData("Exit"));
    }

    string title;
    public override void OnOpen(PageName name, EventData data)
    {
        base.OnOpen(name, data);

        //InitParam();
        Dictionary<string, object> argDic = null;
        if (data != null)
        {
            argDic = (Dictionary<string, object>)data.value;
            title = (string)argDic["title"];
            unitLeft = (string)argDic["unitLeft"];
            unitRight = (string)argDic["unitRight"];
            valueMax = (int)argDic["valueMax"];
            valueMin = (int)argDic["valueMin"];


            DebugUtils.Log($"@@@@ {(int)argDic["valueCur"]}");
            //…Ë÷√πˆ¬÷Œª÷√
            SetValue((int)argDic["valueCur"]);
            SetUIContent();
        }
        //1:200
        //1:50
    }

    // 50 -0 0 - 200
    // 0-1
    string unitLeft;
    string unitRight;
    int valueCur;
    int valueMax;
    int valueMin;
    void OnSlide(float value)
    {
        if (isScriptSetSlide)
        {
            isScriptSetSlide = false;
            return;
        }

        valueCur = (int)(value * valueMax);

        if (valueCur < valueMin)
            valueCur = valueMin;

        SetUIContent();
    }


    bool isScriptSetSlide = false;
    void SetValue(int value)
    {
        valueCur = value;

        if (valueCur < valueMin)
            valueCur = valueMin;
        else if (valueCur > valueMax)
            valueCur = valueMax;

        //…Ë÷√πˆ¬÷Œª÷√
        float tmp = (float)valueCur / (float)valueMax;
        isScriptSetSlide = true;
        slider.value = tmp;
    }

    void SetUIContent()
    {
        string value = $"1:{valueCur}";
        tmpContetn.text = $"{title}({unitLeft} : {unitRight})<br>{value}";
        //tmpContetn.text = string.Format("{0}({1} : {2})<br>{3}", title, unitLeft, unitRight,value);
    }


    void OnClickConfirm()
    {
        Dictionary<string, object> res = new Dictionary<string, object>()
        {
            ["valueCur"] = valueCur
        };

        PageManager.Instance.ClosePage(this, new EventData<Dictionary<string, object>>("Result", res));
    }


    async void OnClickKeyboard()
    {
        EventData res = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleKeyboard002,
            new EventData<Dictionary<string, object>>("",
                new Dictionary<string, object>()
                {
                    ["title"] = I18nMgr.T("Input Custom Value"),
                    ["isPlaintext"] = true,
                }));

        if (res.value != null)
        {
            bool isErr = true;
            try
            {
                int val = int.Parse((string)res.value);
                if (val >= valueMin && val <= valueMax)
                {
                    isErr = false;
                    SetValue(val);
                    SetUIContent();
                }
            }
            catch{}

            if (isErr)
                TipPopupHandler.Instance.OpenPopup(string.Format(I18nMgr.T("The {0} must be between {1} and {2}"),I18nMgr.T(title), valueMin, valueMax));
            //TipPopupHandler.Instance.OpenPopup($"The input value must be between {valueMin} and {valueMax}");

        }

    }

}
