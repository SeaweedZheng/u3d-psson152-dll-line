using GameMaker;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PayTableLineSegments01 : PayTableLineSegments
{

    public string lineDes = "LINE {0}";
    public GameObject goLineNumber;

    public void SetUILineNumber()
    {

        int number = lineIdx + 1;
        string title = string.Format(I18nMgr.T(lineDes), (number));

        if (goLineNumber != null)
        {
            Text txtComp = goLineNumber.GetComponent<Text>();
            if (txtComp != null)
            {
                txtComp.text = title;
            }

            TextMeshProUGUI tmpComp = goLineNumber.GetComponent<TextMeshProUGUI>();
            if (tmpComp != null)
            {
                tmpComp.text = title;
            }
        }
    }

    protected override void OnEnable()
    {
        if (isHaveInit)
            return;

        string number = this.name.Split('(')[1].Split(')')[0];   //(1)
        lineIdx = int.Parse(number) - 1;
        SetUILineNumber();

        base.OnEnable();
    }
}