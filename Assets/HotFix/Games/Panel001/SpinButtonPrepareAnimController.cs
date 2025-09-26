using Game;
using GameMaker;
using SlotMaker;
using System.Collections.Generic;
using UnityEngine;
//using _contentBB = PssOn00152.ContentBlackboard;
public class SpinButtonPrepareAnimController : MonoBehaviour
{
    public GameObject goStateButton;
    public GameObject goAnim;

    public float prepareLongClickTime = 0.2f;
    private float timePressed = -1f;

    public StateButton sbtn => goStateButton.GetComponent<StateButton>();
    // public Animator ator => goAnim == null ? null : goAnim.GetComponent<Animator>();
    public AnimBaseUI abui => goAnim == null ? null : goAnim.GetComponent<AnimBaseUI>();

    SoundHelper soundHelper;

    MessageDelegates onPropertyChangedEventDelegates;


    protected virtual void Start()
    {
        soundHelper = new SoundHelper();

        if (goStateButton == null)
        {
            goStateButton = gameObject;
        }

        sbtn.onClickDown.AddListener(OnClickDownStateButton);
        sbtn.onClickUp.AddListener(OnClickUpStateButton);


        onPropertyChangedEventDelegates = new MessageDelegates
        (
            new Dictionary<string, EventDelegate>
            {
                //{ "./isAuto", OnPropertyChangeIsAuto },
                { "./btnSpinState", OnPropertyBtnSpinState},
            }
        );
        EventCenter.Instance.AddEventListener<EventData>(ObservableObject.ON_PROPERTY_CHANGED_EVENT, onPropertyChangedEventDelegates.Delegate);

        EventCenter.Instance.AddEventListener<EventData>(GlobalEvent.ON_PAGE_EVENT, OnEventPageOnTopChange);
    }
    private void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener<EventData>(ObservableObject.ON_PROPERTY_CHANGED_EVENT, onPropertyChangedEventDelegates.Delegate);

        EventCenter.Instance.RemoveEventListener<EventData>(GlobalEvent.ON_PAGE_EVENT, OnEventPageOnTopChange);
    }



    bool isInLongClick = false;
    void OnClickUpStateButton(string custom)
    {
        timePressed = -1;
        isInLongClick = false;
    }

    void OnClickDownStateButton(string custom)
    {
        timePressed = Time.unscaledTime;
    }

    protected virtual void Update()
    {
        if (timePressed > 0)
        {
            if (Time.unscaledTime - timePressed >= prepareLongClickTime)
            {
                timePressed = -1;
                OnLongClickPrepare(sbtn.state);
            }
        }
    }


    /// <summary>
    /// 只检查按钮长按，不判断是否要播放特效。
    /// </summary>
    /// <param name="animName"></param>
    protected virtual void OnLongClickPrepare(string animName)
    {
        //DebugUtil.Log($"@== name = {animName}");

        isInLongClick = true;
        ShowLongClickEffect(animName);
    }


    /// <summary>
    /// 检查是否要播放长按特效
    /// </summary>
    /// <param name="animName"></param>
    void ShowLongClickEffect(string animName)
    {

        if (animName == SpinButtonState.Stop)
            soundHelper.PlaySoundEff(GameMaker.SoundKey.SpinAutoClick);

        /* if (ator!= null && ator.HasState(0, Animator.StringToHash(animName)))
         {
             ator.Play(animName);
         }*/

        if (abui != null)
        {
            abui.state = animName;
        }
    }
    
    void OnPropertyBtnSpinState(EventData receivedEvent)
    {
        string changeSpinState = (string)receivedEvent?.value;

        //按钮一直长按中，且碰到“stop” 状态，要播放长按特效
        if (isInLongClick && changeSpinState == SpinButtonState.Stop)
        {
            ShowLongClickEffect(changeSpinState);
            return;
        }


        // 按钮切换状态清除长按动效
        switch (changeSpinState)
        {
            case SpinButtonState.Stop:
            case SpinButtonState.Spin:
                ClosePrepareAnim();
                break;
            case SpinButtonState.Auto:
                break;
        }
    }

    /// <summary> 关闭“预自动玩”动画 </summary>
    void ClosePrepareAnim()
    {
        if (abui != null)
        {
            abui.state = AnimBaseUI.STOP;
        }
    }




    /// <summary>
    ///  【Bug】
    /// </summary>
    /// <remarks>
    /// * 主页面长按spin按钮，且播放“预自动玩”动画。
    /// * 点击打开其他界面，如退票界面进行退票。
    /// * 这时机台按钮事件，被退票遮罩页面获取。导致Spin按钮收不到按钮弹起时间，一直卡在“预自动玩”动画。
    /// </remarks>
    void OnEventPageOnTopChange(EventData res)
    {
        if (res.name == GlobalEvent.PageOnTopChange)
        {
            bool isAuto = BlackboardUtils.GetValue<bool>("./isAuto");
            string changeSpinState = BlackboardUtils.GetValue<string>("./btnSpinState");

            //PageName pageName = (PageName)res.value;
            //Debug.LogWarning($"【Spin】页面切换， 按钮状态: {changeSpinState}  是否自动: {isAuto}   页面： {pageName} ");

            // 按钮切换状态清除长按动效
            switch (changeSpinState)
            {
                case SpinButtonState.Stop:
                case SpinButtonState.Spin:
                    ClosePrepareAnim();
                    break;
                case SpinButtonState.Auto:
                    break;
            }
        }
    }


    /*
    void OnPropertyChangeIsAuto(EventData receivedEvent)
    {
        bool isAuto = (bool)receivedEvent.value;
        if (!isAuto)
        {
            ClosePrepareAnim();
        }
    }*/

}
