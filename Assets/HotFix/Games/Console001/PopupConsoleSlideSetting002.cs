using Game;
using GameMaker;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupConsoleSlideSetting002 : PageMachineButtonBase
{
    public GameObject goContent, goClose, goButton1;
    public Slider slider;

    TextMeshProUGUI tmpTitle, tmpContetn;
    void Awake()
    {
        goClose.GetComponent<Button>().onClick.AddListener(OnClickClose);

        goButton1.GetComponent<Button>().onClick.AddListener(OnClickConfirm);

        tmpContetn = goContent.transform.GetComponent<TextMeshProUGUI>();

        slider.onValueChanged.AddListener(OnSlide);
    }

    void OnClickClose(){
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
            valueMaxLeft = (int)argDic["valueMaxLeft"];
            valueMaxRight = (int)argDic["valueMaxRight"];
            valueLeft = (int)argDic["valueLeft"];
            valueRight = (int)argDic["valueRight"];

            valueMiddle = (float)valueMaxLeft / (float)(valueMaxLeft + valueMaxRight);


            //ÉèÖÃ¹öÂÖÎ»ÖÃ
            if (valueLeft > 1)
            {
                float tmp =  (1 - ((float)valueLeft / (float)valueMaxLeft))* valueMiddle; 
                slider.value = tmp;
            }
            else if (valueRight > 1)
            {
                float tmp =  ((float)valueRight / (float)valueMaxRight)* (1- valueMiddle) + valueMiddle;
                slider.value = tmp;
            }

            SetUIContent();
        }
        //1:200
        //1:50
    }

    // 50 -0 0 - 200
    // 0-1
    string unitLeft;
    string unitRight;
    int valueLeft;
    int valueRight;
    int valueMaxLeft;
    int valueMaxRight;
    float valueMiddle;

    void OnSlide(float value)
    {
        if (value <= valueMiddle)
        {
            float valueNow = valueMiddle - value;
            float valueTotal = valueMiddle;
            valueLeft = (int)((valueNow / valueTotal) * valueMaxLeft);
            valueRight = 1;
        }
        else
        {
            float valueNow = value - valueMiddle;
            float valueTotal = 1 - valueMiddle;
            valueRight= (int)((valueNow / valueTotal) * valueMaxRight);
            valueLeft = 1;
        }
        SetUIContent();
    }

    void SetUIContent()
    {
        string value = valueLeft > 1 ? $"{valueLeft}:1" : $"1:{valueRight}";
        tmpContetn.text = $"{title}({unitLeft} : {unitRight})<br>{value}";
    }


    void OnClickConfirm()
    {
        Dictionary<string, object> res = new Dictionary<string, object>()
        {
            ["valueRight"] = valueRight,
            ["valueLeft"] = valueLeft
        };

        PageManager.Instance.ClosePage(this, new EventData<Dictionary<string, object>>("Result", res));
    }
}
