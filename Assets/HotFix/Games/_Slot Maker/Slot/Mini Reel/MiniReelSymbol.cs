using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[Serializable]
public class SymbolImageInfo
{
    [SerializeField]
    public string symbol;
    [SerializeField]
    public Sprite spr;
}


public class MiniReelSymbol : MonoBehaviour
{

    public List<SymbolImageInfo> symbolImageInfo = new List<SymbolImageInfo>();

    public Material frontBlur;

    public GameObject goSymbol;

    private Text txtSymbol => goSymbol?.GetComponent<Text>();
    private TextMeshProUGUI tmpSymbol => goSymbol?.GetComponent<TextMeshProUGUI>();
    private Image imgSymbol => goSymbol?.GetComponent<Image>();

    void Start()
    {
        //TextMeshPro tmp;
        if (goSymbol == null)
        {
            goSymbol = gameObject;
        }
    }

    public virtual void SetSymbol(string str, bool isMask = false)
    {

        if (imgSymbol != null)
        {
            Sprite spr = null;
            for (int i = 0; i < symbolImageInfo.Count; i++)
            {
                if (symbolImageInfo[i].symbol == str)
                {
                    spr = symbolImageInfo[i].spr;
                    break;
                }
            }
            if (spr != null)
            {
                imgSymbol.sprite = spr;
                imgSymbol.material = isMask ? frontBlur : null;
            }
            else
            {
                DebugUtils.LogError($"can not find image({str})");
            }

        }
        else if (tmpSymbol != null)
        {
            tmpSymbol.text = str;
        } else if (txtSymbol  != null)
        {
            txtSymbol.text = str;
        }
    }

    public void SetSymbolSize(MiniReelSymbolSize sz)
    {
        RectTransform rtfm = transform.GetComponent<RectTransform>();
        if (rtfm.sizeDelta.x != sz.width || rtfm.sizeDelta.y != sz.height)
        {
            rtfm.sizeDelta = new UnityEngine.Vector2(sz.width, sz.height);
        }

        if (tmpSymbol != null)
        {
            tmpSymbol.fontSize = sz.fontSize;
        }
        else if (txtSymbol != null)
        {
            txtSymbol.fontSize = (int)sz.fontSize;
        }
    }
}
