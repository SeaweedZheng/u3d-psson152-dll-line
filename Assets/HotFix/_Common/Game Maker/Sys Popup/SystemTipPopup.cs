using Game;
using GameMaker;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TipItemInfo
{
    public string msg;
    public float timeS;
    public GameObject go;
}
public class SystemTipPopup : PageCorBase, ITipPopup
{
    public GameObject goBase;

    RectTransform rfmBase;
    Transform tfmBase, tfmLayout, tfmClone;

    float gap, height;
    private void Awake()
    {
        tfmBase = goBase.transform;
        rfmBase = goBase.GetComponent<RectTransform>();
        tfmLayout = tfmBase.Find("Layout").GetComponent<Transform>();
        tfmClone = tfmLayout.GetChild(0);

        gap = 2;
        height = tfmClone.GetComponent<RectTransform>().sizeDelta.y;
    }

    private void OnEnable()
    {
        foreach (Transform chd in tfmLayout)
        {
            chd.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        ClearCor(COR_AUOT_CLOSE);
    }

    public override void OnOpen(PageName name, EventData data)
    {
        base.OnOpen(name, data);

        string msg = (string)data.value;
        ShowTip(msg);
    }


    const string COR_AUOT_CLOSE = "COR_AUOT_CLOSE";
    //const string COR_SCALE = "COR_SCALE";

    List<TipItemInfo> tipItems = new List<TipItemInfo>();



    public bool Contains(string msg)
    {
        for (int i = 0; i < tfmLayout.childCount; i++)
        {
            Transform tfm = tfmLayout.GetChild(i);
            if (tfm.gameObject.active
                //&& tfm.Find("Contents Layout/Text").GetComponent<TextMeshProUGUI>().text == msg
                && tfm.Find("Contents Layout/Text").GetComponent<Text>().text == msg  
                )
                return true;
        }
        return false;
    }



    [Button]
    public void ShowTip(string msg = "1234")
    {
        AddItem(msg);
    }

    float targetY = 0;

    bool isDirty = false;


    public void UpdatePos()
    {   
        if (isDirty)
        {
            List<Transform> items = new List<Transform>();
            for (int i = tfmLayout.childCount - 1; i >= 0; i--)
            {
                Transform tfm = tfmLayout.GetChild(i);
                if (tfm.gameObject.active)
                    items.Add(tfm);
            }

            if (items.Count > 0)
            {
                int i = 0;
                while (++i < items.Count)
                {
                    float targetY = (gap + height) * i;
                    items[i].localPosition = new Vector3(items[i].localPosition.x, targetY, 0);
                }
            }

            isDirty = false;
        }
    }
    public void AddItem(string msg)
    {

        Transform tfmTarget = null;
        for (int i = 0; i < tfmLayout.childCount; i++)
        {
            Transform tfm = tfmLayout.GetChild(i);
            if (!tfm.gameObject.active)
                tfmTarget = tfm;
        }
        if (tfmTarget == null)
        {
            tfmTarget = GameObject.Instantiate(tfmClone);
        }

        tfmTarget.SetParent(tfmLayout);
        tfmTarget.SetSiblingIndex(tfmLayout.childCount - 1);
        tfmTarget.gameObject.SetActive(true);
        tfmTarget.localPosition = new Vector3(0, 0, 0);
        //tfmTarget.localScale = Vector3.one;

        tfmTarget.localScale = new Vector3(0.2f,0.2f,1);
        scaleItems.Add(tfmTarget);

        //if (!IsCor(COR_SCALE))  DoCor(COR_SCALE, ItemScale());
     
        try
        {

            //Text 不支持<br> ; TextMeshProUGUI 支持<br>；但是 Text支持所有字体。TextMeshProUGUI不支持，要导入。

            // tfmTarget.Find("Contents Layout/Text").GetComponent<TextMeshProUGUI>().text = msg;
            tfmTarget.Find("Contents Layout/Text").GetComponent<Text>().text = msg;
        }
        catch
        {
            DebugUtils.LogError($"name: {tfmTarget.name}");
        }

        tipItems.Add(new TipItemInfo()
        {
            msg = msg,
            go = tfmTarget.gameObject,
            timeS = UnityEngine.Time.unscaledTime    //DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), // UnityEngine.Time.time,
        });

        isDirty = true;
        UpdatePos();

        if (!IsCor(COR_AUOT_CLOSE))
            DoCor(COR_AUOT_CLOSE, CheckItemRemove01());
    }




    List<Transform> scaleItems = new List<Transform>();
    /*IEnumerator ItemScale()
    {
        while (scaleItems.Count>0)
        {

            yield return DoWaitForSecondsRealtime(0.01f);
            int i = scaleItems.Count;
            while (--i >= 0)
            {
                float scale = scaleItems[i].localScale.x + 0.1f;
                if (scale >= 1)
                {
                    scaleItems[i].localScale = Vector3.one;
                    scaleItems.Remove(scaleItems[i]);
                }
                else{
                    scaleItems[i].localScale = new Vector3(scale,scale,1);
                }
            }
        }
    }*/


    /*
    private void Update()
    {
        ScaleUpdate();
    }*/

    private void FixedUpdate()
    {
        ScaleUpdate();
    }


    bool isDirtyScaleUpdate = true;
    void ScaleUpdate()
    {

        if (isDirtyScaleUpdate)
        {
            isDirtyScaleUpdate = false;
      
            //List<Transform> temp = new List<Transform>(scaleItems); //避免多处地方同时修改该scaleItems
            int i = scaleItems.Count; 
            
            while (--i >= 0)
            {

                // 【待解决】增长时间拉长时，会有bug（好像是子级排序有问题）
                // Time.unscaledDeltaTime; //上一帧到当前帧所经历的实际时间 
                float scale = scaleItems[i].localScale.x + 3f * Time.unscaledDeltaTime;
                //Debug.Log($"i = {i} {temp[i].name}  x = {temp[i].localScale.x}  + {0.5f * Time.unscaledDeltaTime}  = {scale}");
                if (scale >= 1)
                {
                    scaleItems[i].localScale = Vector3.one;
                    scaleItems.Remove(scaleItems[i]);
                }
                else
                {
                    scaleItems[i].localScale = new Vector3(scale, scale, 1);
                }
            }

            /* 【这个方案有问题-暂时不用】
            float scale1 = 0;
            List <Transform> temp = new List<Transform>(scaleItems);
            if (temp.Count > 0)
            {
                for (int j = temp.Count -1; j>=1; j--)
                {
                    scale1 = temp[j - 1].localScale.x;
                    temp[j].localScale = new Vector3(scale1, scale1, 1);
                }
                scale1 = temp[0].localScale.x + 0.8f * Time.unscaledDeltaTime;
                if (scale1 >= 1)
                {
                    temp[0].localScale = Vector3.one;
                    scaleItems.Remove(temp[0]);
                    temp.Remove(temp[0]);
                }
                else
                {
                    temp[0].localScale = new Vector3(scale1, scale1, 1);
                }
            }*/


            isDirtyScaleUpdate = true;
        }
        
    }



    
    IEnumerator CheckItemRemove01()
    {

        while (tipItems.Count >0)
        {
            while (tipItems.Count > 8)// 限制最多的个数
            {
                TipItemInfo target = tipItems[0];
                target.go.SetActive(false);
                tipItems.Remove(target);
            }

            yield return DoWaitForSecondsRealtime(0.5f);//每0.5f秒次删除一个

            float nowTimeS = UnityEngine.Time.unscaledTime;// DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(); //UnityEngine.Time.time;

            /*
            int i = tipItems.Count;
            TipItemInfo targetRemove = null;
            while (--i >= 0)
            {
                if (targetRemove == null || targetRemove.timeS >= tipItems[i].timeS)
                {
                    targetRemove = tipItems[i];
                }
            }
            if (targetRemove != null &&  nowTimeS - targetRemove.timeMS > 2f)
            {
                targetRemove.go.SetActive(false);
                tipItems.Remove(targetRemove); //出现2秒后开始删除
            }*/
            TipItemInfo targetRemove = tipItems[0];
            if (targetRemove != null && nowTimeS - targetRemove.timeS > 2f)
            {
                targetRemove.go.SetActive(false);
                tipItems.Remove(targetRemove); //出现2秒后开始删除
            }
        }
        ClearCor(COR_AUOT_CLOSE);
        PageManager.Instance.ClosePage(this);
    }


}


