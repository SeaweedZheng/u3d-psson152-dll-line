using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface ITipPopupHandel
{
    void OpenPopup(string msg);
    void OpenPopupOnce(string msg);
    void ClosePopup();
}
public class DefaultTipPopup :DefaultBasePopup , ITipPopupHandel
{
    public class TipItemInfo
    {
        public string msg;
        public float timeS;
        public GameObject go;
    }


    static DefaultTipPopup _instance;

    public static DefaultTipPopup Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new DefaultTipPopup();
            }
            return _instance;
        }
    }

    MonoHelper mono = null;
    public override void OpenPopup(string msg)
    {
        //if(!IsTop()) base.OnOpen("Common/Prefabs/Popup Default Tip");

        base.OnOpen("Common/Prefabs/Popup Default Tip");
        AddItem(msg);
    }
    public void OpenPopupOnce(string msg)
    {
        // if (!IsTop()) base.OnOpen("Common/Prefabs/Popup Default Tip");

        base.OnOpen("Common/Prefabs/Popup Default Tip");
        if (!Contains(msg))
        {
            AddItem(msg);
        }
    }



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



    Transform tfmLayout, tfmClone;
    List<Transform> scaleItems = new List<Transform>();
    List<TipItemInfo> tipItems = new List<TipItemInfo>();
    bool isDirty = false;

    float gap, height;

    protected override void InitParam()
    {
        tfmLayout = goPopup.transform.Find("Anchor/Base/Layout");

        tfmClone = tfmLayout.GetChild(0);
        height = tfmClone.GetComponent<RectTransform>().sizeDelta.y;
        gap = 2;

        foreach (Transform chd in tfmLayout)
        {
            chd.gameObject.SetActive(false);
        }

        mono = goPopup.AddComponent<MonoHelper>();
        mono.fixeUpdateHandle.AddListener(FixedUpdate);
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

        tfmTarget.localScale = new Vector3(0.2f, 0.2f, 1);
        scaleItems.Add(tfmTarget);

        //if (!IsCor(COR_SCALE))  DoCor(COR_SCALE, ItemScale());

        try
        {
            //Text 不支持<br> ; TextMeshProUGUI 支持<br>；但是 Text支持所有字体。TextMeshProUGUI不支持，要导入。
            tfmTarget.Find("Contents Layout/Text").GetComponent<Text>().text = msg;
        }
        catch
        {
            Debug.LogError($"name: {tfmTarget.name}");
        }

        tipItems.Add(new TipItemInfo()
        {
            msg = msg,
            go = tfmTarget.gameObject,
            timeS = UnityEngine.Time.unscaledTime    //DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), // UnityEngine.Time.time,
        });

        isDirty = true;
        UpdatePos();

        if (corCheckItemRemove == null)
            corCheckItemRemove = mono.StartCoroutine(CheckItemRemove01());

    }

    Coroutine corCheckItemRemove = null;



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

            isDirtyScaleUpdate = true;
        }

    }




    IEnumerator CheckItemRemove01()
    {

        while (tipItems.Count > 0)
        {
            while (tipItems.Count > 8)// 限制最多的个数
            {
                TipItemInfo target = tipItems[0];
                target.go.SetActive(false);
                tipItems.Remove(target);
            }

            yield return new WaitForSecondsRealtime(0.5f);//每0.5f秒次删除一个

            float nowTimeS = UnityEngine.Time.unscaledTime;// DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(); //UnityEngine.Time.time;

            TipItemInfo targetRemove = tipItems[0];
            if (targetRemove != null && nowTimeS - targetRemove.timeS > 2f)
            {
                targetRemove.go.SetActive(false);
                tipItems.Remove(targetRemove); //出现2秒后开始删除
            }
        }
        corCheckItemRemove = null;
        ClosePopup();
    }


}
