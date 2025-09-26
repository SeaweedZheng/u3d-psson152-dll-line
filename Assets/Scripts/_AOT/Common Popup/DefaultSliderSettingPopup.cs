using System;
using UnityEngine;
using UnityEngine.UI;


public class SliderSettingPopupInfo
{
    public string title = "标题\n1:{0}"; // "标题\n0.{0}";
    public int maxValue = 100;
    public int minValue = 0;
    public int curValue = 1;
    public Func<int, string> showFunc = (val) => $"{val}";
    public Action<int?> onFinishCallback;
}
public class DefaultSliderSettingPopup:DefaultBasePopup
{
    static DefaultSliderSettingPopup _instance;

    public static DefaultSliderSettingPopup Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new DefaultSliderSettingPopup();
                
            }
            return _instance;
        }
    }


    SliderSettingPopupInfo info;

    Button btnClose, btnConfirm;

    Text txtTitle;


    Slider slider;


    int valueCur;
    int valueMax =>info.maxValue;
    int valueMin =>info.minValue;

    public void OpenPopup(SliderSettingPopupInfo info)
    {
        this.info = info;
        base.OnOpen("Common/Prefabs/Popup Slider Setting");


        valueCur = info.curValue;
        SetValue(valueCur);
        SetUIContent();
    }

    protected override void InitParam()
    {
        btnClose = goPopup.transform.Find("Popup/Button Close").GetComponent<Button>();
        btnClose.onClick.AddListener(() =>
        {
            ClosePopup();
            info.onFinishCallback?.Invoke(null);
        });


        btnConfirm = goPopup.transform.Find("Popup/Button Confirm").GetComponent<Button>();
        btnConfirm.onClick.AddListener(() =>
        {
            ClosePopup();
            info.onFinishCallback?.Invoke(valueCur);
        });

        slider = goPopup.transform.Find("Popup/Anchor/Slider").GetComponent<Slider>();
        slider.onValueChanged.AddListener(OnSlide);

        txtTitle = goPopup.transform.Find("Popup/Anchor/Text").GetComponent<Text>();
    }


    void OnSlide(float value)
    {
        if (isScriptSetSlide)
        {
            isScriptSetSlide = false;
            return;
        }

        valueCur = (int)(value * (valueMax - valueMin)) + valueMin;

        if (valueCur < valueMin)
            valueCur = valueMin;
        if (valueCur > valueMax)
            valueCur = valueMax;
       
        SetUIContent();  // 设置ui
    }

    /// <summary> 脚本设置滚轮值 </summary>
    bool isScriptSetSlide;

    void SetValue(int value)
    {
        valueCur = value;

        if (valueCur < valueMin)
            valueCur = valueMin;
        else if (valueCur > valueMax)
            valueCur = valueMax;

        //设置滚轮位置
        float tmp = (float)valueCur / ((float)valueMax - (float)valueMin);
        isScriptSetSlide = true;
        slider.value = tmp;
    }


    void SetUIContent()
    {
        string str = info.showFunc(valueCur);
        txtTitle.text = string.Format(info.title, str);

    }
}
