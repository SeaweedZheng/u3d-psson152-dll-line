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
    /// ֻ��鰴ť���������ж��Ƿ�Ҫ������Ч��
    /// </summary>
    /// <param name="animName"></param>
    protected virtual void OnLongClickPrepare(string animName)
    {
        //DebugUtil.Log($"@== name = {animName}");

        isInLongClick = true;
        ShowLongClickEffect(animName);
    }


    /// <summary>
    /// ����Ƿ�Ҫ���ų�����Ч
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

        //��ťһֱ�����У���������stop�� ״̬��Ҫ���ų�����Ч
        if (isInLongClick && changeSpinState == SpinButtonState.Stop)
        {
            ShowLongClickEffect(changeSpinState);
            return;
        }


        // ��ť�л�״̬���������Ч
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

    /// <summary> �رա�Ԥ�Զ��桱���� </summary>
    void ClosePrepareAnim()
    {
        if (abui != null)
        {
            abui.state = AnimBaseUI.STOP;
        }
    }




    /// <summary>
    ///  ��Bug��
    /// </summary>
    /// <remarks>
    /// * ��ҳ�泤��spin��ť���Ҳ��š�Ԥ�Զ��桱������
    /// * ������������棬����Ʊ���������Ʊ��
    /// * ��ʱ��̨��ť�¼�������Ʊ����ҳ���ȡ������Spin��ť�ղ�����ť����ʱ�䣬һֱ���ڡ�Ԥ�Զ��桱������
    /// </remarks>
    void OnEventPageOnTopChange(EventData res)
    {
        if (res.name == GlobalEvent.PageOnTopChange)
        {
            bool isAuto = BlackboardUtils.GetValue<bool>("./isAuto");
            string changeSpinState = BlackboardUtils.GetValue<string>("./btnSpinState");

            //PageName pageName = (PageName)res.value;
            //Debug.LogWarning($"��Spin��ҳ���л��� ��ť״̬: {changeSpinState}  �Ƿ��Զ�: {isAuto}   ҳ�棺 {pageName} ");

            // ��ť�л�״̬���������Ч
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
