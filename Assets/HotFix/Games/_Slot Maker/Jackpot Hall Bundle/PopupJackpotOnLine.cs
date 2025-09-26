using Game;
using GameMaker;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class BulbsInfo
{
    public Transform tfm;
    public int direction;
}


public class PopupJackpotOnLine : PageCorBase //   BasePageMachineButton
{
    public Button btnConfirm;
    public Transform tfmJackpot,tfmBase ,tfmButton,
        
        tfmTopBulbsLight , tfmBottomBulbsLight,
        tfmBaseBulbs;

    public TextMeshProUGUI tmpTotalWin;
    
    public GameObject goRuby, goFirework , goGlowBack;


    public CanvasRendererProperty crpCoverEnd;

    public CanvasGroup cgGlowCover;

    long fromCredit = 0, toCredit = 1000;
    //Dictionary<Transform,int> baseBulbs = new Dictionary<Transform, int>();

    List<BulbsInfo> baseBulbs = new List<BulbsInfo>();
    enum AnimJackpotState
    {
        None,
        BaseScale,
        CreditAdd,
        CreditAddEnd,
        Idle,
    }

    AnimJackpotState state = AnimJackpotState.BaseScale;


    public void Awake()
    {
        btnConfirm.onClick.AddListener(OnClose);
    }
    private void OnDisable()
    {
        ClearAllCor();
    }

    // Update is called once per frame
    void OnEnable()
    {
        //InitParam();
    }



    public override void OnOpen(PageName name, EventData data)
    {
        base.OnOpen(name, data);

        if(data != null && data.value != null)
        {
            Dictionary<string, object>  args = data.value as Dictionary<string, object>;
            if (args.ContainsKey("toCredit"))
            {
                long temp  = (long)args["toCredit"];
                toCredit = temp;
            }

            fromCredit = (long)args["fromCredit"];
            state = AnimJackpotState.None;
            DoMainAnim();
        }
        else
        {
            InitParam();
        }
    }

    private void InitParam()
    {
        state = AnimJackpotState.None;
        fromCredit = 0;
        toCredit = 1000;
        DoMainAnim();
    }
    

    public float unitJPScale = 0.0008f;
    public float gapJPScaleS = 0.002f;
    const string COR_ANIM_JP_SCALE = "COR_ANIM_JP_SCALE";
    const string COR_ANIM_BUTTON_LIGHT_TOP = "COR_ANIM_BUTTON_LIGHT_TOP";
    const string COR_ANIM_BUTTON_LIGHT_BUTTOM = "COR_ANIM_BUTTON_LIGHT_BUTTOM";

    const string COR_ANIM_BASE_LIGHT = "COR_ANIM_BASE_LIGHT";
    const string COR_ANIM_COVER_END = "COR_ANIM_COVER_END"; //CoverEnd

    IEnumerator AnimJackpotScale()
    {
        float scale = 0.97f;
        float val = unitJPScale;
        while (true)
        {
            if (scale > 1f)
                val = - unitJPScale;
            
            if (scale < 0.97f)
                val = unitJPScale;

            scale += val;
            yield return new WaitForSeconds(gapJPScaleS);
            tfmJackpot.localScale = new Vector3(scale, scale,1);
        }
    }

    public override void OnClickMachineButton(MachineButtonInfo info)
    {
        switch (info.btnKey)
        {
            case MachineButtonKey.BtnSpin:
                ShowUIAminButtonClick(btnConfirm, info);
                break;
        }
        /*
        if (info != null)
         {
             if (info.isUp)
             {
                 switch (info.btnKey)
                 {
                     case MachineButtonKey.BtnSpin:

                        OnClose();
                         break;
                 }
             }
         }*/
    }



    void OnClose()
    {
        DoMainAnim();
    }

    void DoMainAnim()
    {
        if (AnimJackpotState.Idle == state)
        {
            PageManager.Instance.ClosePage(this);
        }
        else
        {
            DoCor(COR_CHAGNE_STATE, ChangeState0());
        }
    }



    const string COR_CHAGNE_STATE = "COR_CHAGNE_STATE";

    IEnumerator ChangeState0()
    {

        while (state != AnimJackpotState.Idle)
        {
            switch (state)
            {
                case AnimJackpotState.None:
                    state = AnimJackpotState.BaseScale;

                    crpCoverEnd.alpha = 0f;
                    cgGlowCover.alpha = 1;
                    goRuby.SetActive(false);
                    goFirework.SetActive(false);
                    goGlowBack.SetActive(true);
                    tmpTotalWin.text = "0";


                    DoCor(COR_ANIM_BASE_LIGHT, AminBaseBulbs());  //不释放
                    yield return AnimBaseScale();

                    break;
                case AnimJackpotState.BaseScale:
                    state = AnimJackpotState.CreditAdd;

 

                    tfmBase.localScale = new Vector3(1, 1, 1);
                    cgGlowCover.alpha = 0;
                    goRuby.SetActive(true);

                    DoCor(COR_ANIM_JP_SCALE, AnimJackpotScale()); //不释放
                    DoCor(COR_ANIM_BUTTON_LIGHT_TOP, AminButtonLightTop());
                    DoCor(COR_ANIM_BUTTON_LIGHT_BUTTOM, AminButtonLightButtom());


                    yield return AminCreditAdd();
                    break;

                case AnimJackpotState.CreditAdd:
                    state = AnimJackpotState.CreditAddEnd;
    
                    // 关闭灯：
                    ClearCor(COR_ANIM_BUTTON_LIGHT_TOP);
                    tfmTopBulbsLight.gameObject.SetActive(false);
                    ClearCor(COR_ANIM_BUTTON_LIGHT_BUTTOM);
                    tfmBottomBulbsLight.gameObject.SetActive(false);

                    tmpTotalWin.text = toCredit.ToString(); // 不要千分号   toCredit.ToString("N0");


                    DoCor(COR_ANIM_COVER_END, AminCoverEnd());

                    yield return AminCreditAddEnd();
                    break;

                case AnimJackpotState.CreditAddEnd:
                    state = AnimJackpotState.Idle;

                    ClearCor(COR_ANIM_COVER_END);
                    crpCoverEnd.alpha = 0f;

                    tfmButton.localScale = new Vector3(1, 1, 1);
                    goGlowBack.SetActive(false);
                    goRuby.SetActive(false);
                    goFirework.SetActive(true);

                    break;
            }
        }

    }
    





    /// <summary>
    /// 整体放大
    /// </summary>
    /// <returns></returns>
    IEnumerator AnimBaseScale()//double fromCredit, double toCredit)
    {

        float scale = 0.3f;
        tfmBase.localScale = new Vector3(scale, scale, 1);
        while (scale < 1f)
        {
            scale += 0.05f;
            tfmBase.localScale = new Vector3(scale, scale, 1);
            yield return new WaitForSeconds(0.02f);
        }

        while (scale < 1.2f)
        {
            scale += 0.01f;
            tfmBase.localScale = new Vector3(scale, scale, 1);
            yield return new WaitForSeconds(0.02f);
        }


        scale = 1.2f;
        while (scale > 1)
        {
            scale -= 0.05f;
            tfmBase.localScale = new Vector3(scale, scale, 1);
            yield return new WaitForSeconds(0.02f);
        }
        scale = 1f;
        tfmBase.localScale = new Vector3(scale, scale, 1);
    }

    float bigLightScale = 1.5f;



    /// <summary>
    /// 按钮等流动
    /// </summary>
    /// <returns></returns>
    IEnumerator AminButtonLightTop()
    {
        tfmTopBulbsLight.gameObject.SetActive(true);

        foreach (Transform chd in tfmTopBulbsLight)
        {
            chd.localScale = new Vector3(0, 0, 1);
        }
        int index = 0; // 0-6 15
        while (true)
        {
            tfmTopBulbsLight.GetChild(index).localScale = new Vector3(bigLightScale, bigLightScale, 1);
            tfmTopBulbsLight.GetChild((14 - index)).localScale = new Vector3(bigLightScale, bigLightScale, 1);
            for (int i= index - 1; i>=0; i--)
            {
                int gapIdx = index - i;
                float scale = bigLightScale  - (float)(gapIdx) *0.2f;
                tfmTopBulbsLight.GetChild(i).localScale =  new Vector3(scale, scale, 1);

                tfmTopBulbsLight.GetChild(i).localRotation = Quaternion.Euler(0, 0, 0 - gapIdx * 20);
            }
            for (int i = (14 - index) + 1; i <= 14; i++)
            {
                int gapIdx = i - (14 - index);
                float scale = bigLightScale -  (float)(gapIdx) * 0.2f;
                tfmTopBulbsLight.GetChild(i).localScale = new Vector3(scale, scale, 1);

                tfmTopBulbsLight.GetChild(i).localRotation = Quaternion.Euler(0, 0, 0 + gapIdx * 20);
            }
            yield return new WaitForSeconds(0.1f);

            if(++index == 8)
            {
                index = 0;
                foreach (Transform chd in tfmTopBulbsLight)
                {
                    chd.localScale = new Vector3(0, 0, 1);
                }
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
    /// <summary>
    /// 按钮等流动
    /// </summary>
    /// <returns></returns>
    IEnumerator AminButtonLightButtom()
    {
        tfmBottomBulbsLight.gameObject.SetActive(true);

        foreach (Transform chd in tfmBottomBulbsLight)
        {
            chd.localScale = new Vector3(0, 0, 1);
        }
        int index = 0; // 0-6 15
        while (true)
        {
            tfmBottomBulbsLight.GetChild(index).localScale = new Vector3(bigLightScale, bigLightScale, 1);
            tfmBottomBulbsLight.GetChild((14 - index)).localScale = new Vector3(bigLightScale, bigLightScale, 1);
            for (int i = index - 1; i >= 0; i--)
            {
                int gapIdx = index - i;

                float scale = bigLightScale - (float)(gapIdx) * 0.2f;
                tfmBottomBulbsLight.GetChild(i).localScale = new Vector3(scale, scale, 1);

                tfmBottomBulbsLight.GetChild(i).localRotation = Quaternion.Euler(0, 0, 0 - gapIdx * 20);
            }
            for (int i = (14 - index) + 1; i <= 14; i++)
            {
                int gapIdx = i - (14 - index);

                float scale = bigLightScale - (float)(gapIdx) * 0.2f;
                tfmBottomBulbsLight.GetChild(i).localScale = new Vector3(scale, scale, 1);

                tfmBottomBulbsLight.GetChild(i).localRotation = Quaternion.Euler(0, 0, 0 + gapIdx * 20);
            }
            yield return new WaitForSeconds(0.1f);

            if (++index == 8)
            {
                index = 0;
                foreach (Transform chd in tfmBottomBulbsLight)
                {
                    chd.localScale = new Vector3(0, 0, 1);
                }
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    /// <summary>
    /// 加钱动画
    /// </summary>
    /// <returns></returns>
    IEnumerator AminCreditAdd()
    {

        long credit = fromCredit;
        do
        {
            credit += 10;
            if (credit > toCredit)
                credit = toCredit;

            tmpTotalWin.text = credit.ToString(); // 不要千分号   credit.ToString("N0");

            yield return new WaitForSeconds(0.08f);

        } while (credit < toCredit);

    }


    /// <summary>
    /// 加钱结束动画
    /// </summary>
    /// <returns></returns>
    IEnumerator AminCreditAddEnd()
    {

        float scale = 1f;
        tfmButton.localScale = new Vector3(scale, scale, 1);
        while (scale < 1.4f)
        {
            scale += 0.05f;
            tfmButton.localScale = new Vector3(scale, scale, 1);
            yield return new WaitForSeconds(0.02f);
        }

        while (scale < 1.5f)
        {
            scale += 0.002f;
            tfmButton.localScale = new Vector3(scale, scale, 1);
            yield return new WaitForSeconds(0.02f);
        }

        scale = 1.5f;
        while (scale > 1)
        {
            scale -= 0.05f;
            tfmButton.localScale = new Vector3(scale, scale, 1);
            yield return new WaitForSeconds(0.02f);
        }
        scale = 1f;
        tfmButton.localScale = new Vector3(scale, scale, 1);

    }


    /// <summary>
    /// 加钱结束爆光动画
    /// </summary>
    /// <returns></returns>
    IEnumerator AminCoverEnd()
    {
        crpCoverEnd.alpha = 0.8f;
        while (crpCoverEnd.alpha > 0f)
        {
            crpCoverEnd.alpha -= 0.01f;
            yield return new WaitForSeconds(0.02f);
        }
        crpCoverEnd.alpha = 0f;
    }



    Dictionary<string, int> test = new Dictionary<string, int>()
    {
        ["--1--"] = 101,
        ["--2--"] = 102,
        ["--3--"] = 103,
    };


    /// <summary>
    /// 背景灯光闪烁动画
    /// </summary>
    /// <returns></returns>
    IEnumerator AminBaseBulbs()
    {



        /*int j = test.Count;
        while (--j >= 0)
        {
            KeyValuePair<string, int> kv = test.ElementAt(j); ///ElementAt 使用后会报错

            DebugUtil.Log($"@@ = {kv.Key} = {kv.Value}");
        }*/

        yield return null;

        /*
        foreach (Transform item in tfmBaseBulbs)
        {
            item.localScale = new Vector3(0f, 0f, 1);
        }

        float lastRunTimeS = 0;

        while (true)
        {
            int i = baseBulbs.Count;
            while (--i >= 0)
            {   
                KeyValuePair<Transform,int> kv = baseBulbs.ElementAt(i);

                kv.Key.localScale = new Vector3(
                    kv.Key.localScale.x + kv.Value * 0.1f, kv.Key.localScale.x + kv.Value * 0.1f, 1);

                if (kv.Key.localScale.x >2f)
                {
                    baseBulbs[kv.Key] = -1;
                }
                if (kv.Key.localScale.x < 0f)
                {
                    baseBulbs.Remove(kv.Key);
                }
            }
            yield return new WaitForSeconds(0.02f);

            if (Time.time - lastRunTimeS > 0.5f)
            {
                lastRunTimeS = Time.time;
                int idx = UnityEngine.Random.Range(0, tfmBaseBulbs.childCount);
                Transform tfm = tfmBaseBulbs.GetChild(idx);
                if (!baseBulbs.ContainsKey(tfm))
                {
                    baseBulbs.Add(tfm, 1);
                }
            }
        }*/


        foreach (Transform item in tfmBaseBulbs)
        {
            item.localScale = new Vector3(0f, 0f, 1);
        }

        float lastRunTimeS = 0;

        while (true)
        {
            int i = baseBulbs.Count;
            while (--i >= 0)
            {
                BulbsInfo item = baseBulbs[i];

                item.tfm.localScale = new Vector3(
                    item.tfm.localScale.x + item.direction * 0.1f, item.tfm.localScale.x + item.direction * 0.1f, 1);

                if (item.tfm.localScale.x > 2f)
                {
                    baseBulbs[i].direction = -1;
                }
                if (item.tfm.localScale.x < 0f)
                {
                    baseBulbs.RemoveAt(i);
                }
            }
            yield return new WaitForSeconds(0.02f);

            if (Time.time - lastRunTimeS > 0.5f)
            {
                lastRunTimeS = Time.time;
                int idx = UnityEngine.Random.Range(0, tfmBaseBulbs.childCount);
                Transform _tfm = tfmBaseBulbs.GetChild(idx);

                bool isHave = false;
                for (int j=0; j<baseBulbs.Count; j++)
                {
                    if (baseBulbs[j].tfm == _tfm)
                    {
                        isHave = true;
                        break;
                    }
                }
                if (!isHave)
                {
                    baseBulbs.Add( new BulbsInfo()
                    {
                        tfm = _tfm,
                        direction = 1,
                    });
                }
            }
        }
    }

}
