using IOT;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class MiniReelSymbolSize
{
    public string symbol = "";
    public float width = -1;
    public float height = -1;
    public float fontSize = -1;
}

public class MiniReel : MonoBehaviour
{
    public float gapMS = 50f;
    public Transform tfmSymbolLst;

    public List<string> codes = new List<string>() { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

    public MiniReelSymbolSize defauktSymbolSize;

    public List<MiniReelSymbolSize> eachSymbolSize;

    public string data = "";

    protected Coroutine _cor;


    public bool isMask = true;

    void Start()
    {
        if (tfmSymbolLst == null)
        {
            tfmSymbolLst = transform.Find("Anchors");
        }
        /* RectTransform  rtfm = gameObject.GetComponent<RectTransform>();
         foreach (Transform item in tfmReel)
         {
             item.GetComponent<RectTransform>().sizeDelta = new Vector2(rtfm.sizeDelta.x, rtfm.sizeDelta.y);
             symbols.Add(item.GetComponent<MiniReelSymbol>());
         }
         tfmReel.GetComponent<VerticalLayoutGroup>().padding = new RectOffset(0, 0,(int)(-2 * rtfm.sizeDelta.y),0);*/
    }


    [Button]
    public void SetSymbol(int index)
    {
        string value = codes[index];
        SetSymbol(value);
    }
    [Button]
    public void SetSymbol(string value)
    {
        foreach (Transform item in tfmSymbolLst)
        {
            item.gameObject.SetActive(false);
        }
        GameObject cur = tfmSymbolLst.GetChild(0).gameObject;
        cur.SetActive(true);
        data = value;
        cur.transform.GetChild(0).GetComponent<MiniReelSymbol>().SetSymbol(value);
    }

    [Button]
    public void TurnOrKeepSymbol(float ms, string value, Action cb = null)
    {
        if (data == value || !codes.Contains(value))
        {
            SetSymbol(value);
            if (cb != null)
                cb();
            return;
        }

        int nowIdx = 0;
        if (codes.Contains(data))
            nowIdx = codes.IndexOf(data);

        int idx = codes.IndexOf(value);

        TurnSymbol(ms, nowIdx, idx, cb);
    }

    
    [Button]
    public void TurnSymbol(float ms, int startIdx = 0, int endIdx = -1, Action cb = null)
    {
        if (ms <= 0)
            return;

        if (_cor != null)
        {
            StopCoroutine(_cor);
            _cor = null;
        }
        _cor = StartCoroutine(_StartTurn(ms, startIdx, endIdx, cb));
    }

    IEnumerator _StartTurn(float ms, int startIdx = 0, int endIdx = -1, Action cb = null)
    {
        int nowIdx = startIdx;
        GameObject cur = null;
        int endlessLoop = 1000;
        while (true && --endlessLoop >= 0)
        {

            //实时同步数值
            data = codes[nowIdx];

            foreach (Transform item in tfmSymbolLst)
            {
                item.gameObject.SetActive(false);

                int j = nowIdx;
                foreach (Transform sy in item)
                {
                    sy.GetComponent<MiniReelSymbol>().SetSymbol(codes[j], isMask);
                    if (++j >= codes.Count)
                        j = 0;
                }
            }

            cur?.SetActive(false);
            cur = tfmSymbolLst.GetChild(0).gameObject;
            cur.SetActive(true);


            if ((endIdx == -1 && ms < 0)
            || (endIdx != -1 && ms < 0 && endIdx == nowIdx))
            {
                break;
            }

            //yield return new WaitForSeconds(gapMS / 1000f);
            //ms -= gapMS;

            //only move one row
            for (int i = 1; i < tfmSymbolLst.childCount; i++)
            {
                yield return new WaitForSeconds(gapMS / 1000f);
                ms -= gapMS;

                cur?.SetActive(false);
                cur = tfmSymbolLst.GetChild(i).gameObject;
                cur.SetActive(true);
            }

            if (++nowIdx >= codes.Count)
                nowIdx = 0;
        }

        if (endlessLoop < 0)
            Debug.LogError($"is in endless loop : mini reel");
        

        SetSymbol(nowIdx);

        if (cb != null)
        {
            cb();
        }
    }


    bool _isDirty = true;

    void Update()
    {
      if (_isDirty)
        {
            _isDirty = false;

            MiniReelSymbolSize sz = GetSymbolSize(data);
            RectTransform rtfm = transform.GetComponent<RectTransform>();
            if (_nowSymbolSize.width != sz.width 
                || _nowSymbolSize.width != sz.height 
                || _nowSymbolSize.fontSize != sz.fontSize)
            {
                _nowSymbolSize.height = sz.height;
                _nowSymbolSize.width = sz.width;
                _nowSymbolSize.fontSize = sz.fontSize;

                rtfm.sizeDelta = new Vector2(sz.width, sz.height);
                //transform.gameObject.SetActive(false);
                //transform.gameObject.SetActive(true);  //

                ReflashLayoutGroup(transform.parent);

                MiniReelSymbol[] comps = this.GetComponentsInChildren<MiniReelSymbol>();
                foreach (MiniReelSymbol comp in comps)
                {
                    comp.SetSymbolSize(sz);
                }

                if (comps.Length > 0)
                {
                    ReflashLayoutGroup(comps[0].transform.parent);
                }
            }

            _isDirty = true;
        }
    }


    MiniReelSymbolSize _nowSymbolSize = new MiniReelSymbolSize();
    private MiniReelSymbolSize GetSymbolSize(string data)
    {
        MiniReelSymbolSize sz = new MiniReelSymbolSize()
        {
            height = defauktSymbolSize.height,   
            width = defauktSymbolSize.width,
            fontSize = defauktSymbolSize.fontSize,  
        };  

        for (int i = 0; i < eachSymbolSize.Count; i++)
        {
            MiniReelSymbolSize item = eachSymbolSize[i];
            if (item.symbol == data)
            {
                if (item.width >= 0)
                {
                    sz.width = item.width;
                }
                if (item.height >= 0)
                {
                    sz.height = item.height;
                }
                if (item.fontSize >= 0)
                {
                    sz.fontSize = item.fontSize;
                }
            }
        }
        return sz;
    }

    private void ReflashLayoutGroup(Transform trm)
    {
        /*HorizontalLayoutGroup hlg = transform.parent.GetComponent<HorizontalLayoutGroup>();
        VerticalLayoutGroup vlg = transform.parent.gameObject.GetComponent<VerticalLayoutGroup>();
        GridLayoutGroup glg = transform.parent.gameObject.GetComponent<GridLayoutGroup>();
        if (hlg != null)
        {
            hlg.enabled = false;
            hlg.enabled = true;
        }
        if (vlg != null)
        {
            vlg.enabled = false;
            vlg.enabled = true;
        }*/

        LayoutGroup lg  = transform.parent.GetComponent<LayoutGroup>();
        if (lg != null)
        {
            lg.enabled = false;
            lg.enabled = true;
        }
    }

}
