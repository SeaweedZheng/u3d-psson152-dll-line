using Game;
using GameMaker;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum JackpotOnlineType
{
    Grand = 0,
    Major = 1,
    Minor =2 ,
    Mini = 3,
}
public class JackpotOnlineLevel
{
    public JackpotOnlineType ty;
    public float credit;
}
public class PopupJackpotOnLine002 : PageCorBase
{
    public Button btnConfirm;

    //public SkeletonMecanim smFG;
    //public Animator atorFG;

    public Transform tfmFG;

    SkeletonMecanim smFG => tfmFG.GetComponent<SkeletonMecanim>();
    Animator atorFG => tfmFG.GetComponent<Animator>();

    public Text txtCredit;

    public Transform tfmEffFG;
    public GameObject goBaseFG;


    const string COR_AUTO_CLOSE = "COR_AUTO_CLOSE";

    public void Awake()
    {
        btnConfirm.onClick.AddListener(OnClose);
    }
    // Update is called once per frame
    void Update()
    {
        
    }


    long fromCredit = 0, toCredit = 1000;
    JackpotOnlineType jpType = JackpotOnlineType.Grand;
    List<JackpotOnlineLevel> lst;
    public override void OnOpen(PageName name, EventData data)
    {
        base.OnOpen(name, data);


        fromCredit = 0;
        toCredit = 3000;
        jpType = JackpotOnlineType.Major;

        if (data != null && data.value != null)
        {
            Dictionary<string, object> args = data.value as Dictionary<string, object>;
            if (args.ContainsKey("toCredit"))
            {
                toCredit = (long)args["toCredit"];
                jpType = (JackpotOnlineType)((int)args["jackpotType"]);
            }
        }

        lst = new List<JackpotOnlineLevel>();

        /*
        float dif = (int)(toCredit / 5 * Random.Range(0.5f, 1f));
        float remain = toCredit - dif;
        switch (jpType)
        {
            case JackpotOnlineType.Grand:
                fromCredit = remain * 1f / 4f;
                lst.Add(new JackpotOnlineLevel()
                {
                    ty = JackpotOnlineType.Mini,
                    credit = remain * 1f / 4f,
                });
                lst.Add(new JackpotOnlineLevel()
                {
                    ty = JackpotOnlineType.Minor,
                    credit = remain * 2f / 4f,
                });
                lst.Add(new JackpotOnlineLevel()
                {
                    ty = JackpotOnlineType.Major,
                    credit = remain * 3f / 4f,
                });
                lst.Add(new JackpotOnlineLevel()
                {
                    ty = JackpotOnlineType.Grand,
                    credit = remain,
                });
                break;
            case JackpotOnlineType.Major:
                fromCredit = remain * 1f / 3f;
                lst.Add(new JackpotOnlineLevel()
                {
                    ty = JackpotOnlineType.Mini,
                    credit = remain * 1f / 3f,
                });
                lst.Add(new JackpotOnlineLevel()
                {
                    ty = JackpotOnlineType.Minor,
                    credit = remain * 2f / 3f,
                });
                lst.Add(new JackpotOnlineLevel()
                {
                    ty = JackpotOnlineType.Major,
                    credit = remain,
                });
                break;
            case JackpotOnlineType.Minor:
                fromCredit = remain * 1f / 2f;
                lst.Add(new JackpotOnlineLevel()
                {
                    ty = JackpotOnlineType.Mini,
                    credit = remain * 1f / 2f,
                });
                lst.Add(new JackpotOnlineLevel()
                {
                    ty = JackpotOnlineType.Minor,
                    credit = remain,
                });
                break;
            case JackpotOnlineType.Mini:
                fromCredit = remain;
                lst.Add(new JackpotOnlineLevel()
                {
                    ty = JackpotOnlineType.Mini,
                    credit = remain,
                });
                break;
        }*/

        //fromCredit = toCredit * 0.9f;

        fromCredit = (long)(toCredit * 0.8f);
        if (toCredit - fromCredit > 1000)
            fromCredit = toCredit - 1000;
        Debug.Log($"@@@ fromCredit：{fromCredit} toCredit：{toCredit}");
        InitParam();
    
    }

    private void InitParam()
    {

        string skinName = I18nMgr.language == I18nLang.tw || I18nMgr.language == I18nLang.cn ?  "cn" : "en";
        smFG.initialSkinName = skinName;
        smFG.Initialize(true); //重新初始化

        state = AnimJackpotState.None;

        DoMainAnim();
    }
    void OnClose()
    {
        DoMainAnim();
    }

    void DoMainAnim()
    {
        if (AnimJackpotState.Idle == state)
        {
            ClosePage();
        }
        else
        {
            DoCor(COR_CHAGNE_STATE, ChangeState002());
        }
    }

    enum AnimJackpotState
    {
        None,
        CreditAdd,
        BaseScale,
        Idle,
    }

    AnimJackpotState state = AnimJackpotState.None;



    const string COR_CHAGNE_STATE = "COR_CHAGNE_STATE";


    int curIdx = 0;
    IEnumerator AminCreditAdd001()
    {
        SetIcon(curIdx);
        while (true)
        {
            fromCredit += 1;
            if (fromCredit >= toCredit)
            {
                txtCredit.text = toCredit.ToString("N0");
                yield break;
            }
            if(curIdx < lst.Count-1 && fromCredit >= lst[curIdx+1].credit)
            {
                curIdx++;
                SetIcon(curIdx);
            }
            txtCredit.text = fromCredit.ToString("N0");
            yield return new WaitForSeconds(0.02f);
        }
    }

    IEnumerator AminBaseScale001()
    {
        float scale = 1f;
        while (scale < 1.5)
        {
            scale += 0.05f;
            goBaseFG.transform.localScale = new Vector3(scale, scale, 1);
            yield return new WaitForSeconds(0.02f);
        }
        scale = 1.5f;
        while (scale > 1)
        {
            scale -= 0.05f;
            goBaseFG.transform.localScale = new Vector3(scale, scale, 1);
            yield return new WaitForSeconds(0.02f);
        }
        scale = 1f;
        goBaseFG.transform.localScale = new Vector3(scale, scale, 1);

        yield return new WaitForSeconds(1f);
    }


    IEnumerator ChangeState001()
    {

        while (state != AnimJackpotState.Idle)
        {
            switch (state)
            {
                case AnimJackpotState.None:
                    state = AnimJackpotState.CreditAdd;

                    yield return AminCreditAdd001();
                    break;

                case AnimJackpotState.CreditAdd:
                    state = AnimJackpotState.BaseScale;

                    txtCredit.text = toCredit.ToString("N0");
                    if (curIdx != lst.Count - 1)
                    {
                        curIdx = lst.Count - 1;
                        SetIcon(curIdx);
                    }


                    yield return AminBaseScale001();
                    break;
                case AnimJackpotState.BaseScale:
                    state = AnimJackpotState.Idle;

                    goBaseFG.transform.localScale = new Vector3(1, 1, 1);

                    break;
            }
        }
        // 自动关闭
        DoCor(COR_AUTO_CLOSE, AutoClose(6));
    }



    //const string FORMAT_1 = "N2";
    const string FORMAT_2 = "N0";
    IEnumerator AminCreditAdd002()
    {
        float startTimeS = Time.unscaledTime;
        while (Time.unscaledTime - startTimeS <4f)
        {
            fromCredit += 1;
            if (fromCredit >= toCredit)
            {
                txtCredit.text = toCredit.ToString(FORMAT_2);
                yield break;
            }
            txtCredit.text = fromCredit.ToString(FORMAT_2);
            yield return new WaitForSeconds(0.05f);
        }
        txtCredit.text = toCredit.ToString(FORMAT_2);
    }

    IEnumerator ChangeState002()
    {

        while (state != AnimJackpotState.Idle)
        {
            switch (state)
            {
                case AnimJackpotState.None:
                    state = AnimJackpotState.CreditAdd;
                    SetEffIdle(Enum.GetName(typeof(JackpotOnlineType), jpType));
                    SetIcon(jpType);

                    yield return AminCreditAdd002();
                    break;

                case AnimJackpotState.CreditAdd:
                    state = AnimJackpotState.BaseScale;

                    SetEffIn(Enum.GetName(typeof(JackpotOnlineType), jpType));
                    txtCredit.text = toCredit.ToString(FORMAT_2);

                    yield return AminBaseScale001();
                    break;
                case AnimJackpotState.BaseScale:
                    state = AnimJackpotState.Idle;

                    goBaseFG.transform.localScale = new Vector3(1, 1, 1);

                    break;
            }
        }

        if (!PlayerPrefsUtils.isPauseAtPopupJackpotOnline)
            DoCor(COR_AUTO_CLOSE, AutoClose(6));// 自动关闭
    }
    IEnumerator AutoClose(float timeM = 60f)
    {
        yield return new WaitForSeconds(timeM);
        ClosePage();
    }

    void ClosePage()=> PageManager.Instance.ClosePage(this);
    void SetEffIdle(string nameJP)
    {
        foreach (Transform tfm in tfmEffFG)
        {
            tfm.gameObject.SetActive(false);
        }

        tfmEffFG.Find($"eff_{nameJP.ToLower()}_idle").gameObject.SetActive(true);
    }

    void SetEffIn(string nameJP)
    {
        foreach (Transform tfm in tfmEffFG)
        {
            tfm.gameObject.SetActive(false);
        }
        tfmEffFG.Find($"eff_{nameJP.ToLower()}_in").gameObject.SetActive(true);
    }

    void SetIcon(JackpotOnlineType tp)
    {
        switch (tp)
        {
            case JackpotOnlineType.Grand:
                atorFG.Play("Grand");
                //GameSoundHelper.Instance.PlaySound(SoundKey.BigWin);
                break;
            case JackpotOnlineType.Major:
                atorFG.Play("Major");
                //GameSoundHelper.Instance.PlaySound(SoundKey.MegaWin);
                break;
            case JackpotOnlineType.Minor:
                atorFG.Play("Minor");
                //GameSoundHelper.Instance.PlaySound(SoundKey.SuperWin);
                break;
            case JackpotOnlineType.Mini:
                atorFG.Play("Mini");
                //GameSoundHelper.Instance.PlaySound(SoundKey.SuperWin);
                break;
        }
    }
    void SetIcon(int index) =>  SetIcon(lst[index].ty);


    #region 机台按钮事件
    public override void OnClickMachineButton(MachineButtonInfo info)
    {
        if (info != null)
        {
            switch (info.btnKey)
            {
                case MachineButtonKey.BtnSpin:
                    if (info.isUp)
                        DoMainAnim();
                    break;
            }
        }
    }
    #endregion


}
