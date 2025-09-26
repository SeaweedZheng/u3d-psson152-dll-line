using GameMaker;
using SlotMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using _spinWEBB = SlotMaker.SpinWinEffectSettingBlackboard;
public class WinTipController : MonoBehaviour
{

    public GameObject goText;

    Text txtWinCredit => goText.GetComponent<Text>();



    MessageDelegates onWinEventDelegates;

    GameObject goAnchor;

    Animator atorAnimator;

    private void Start()
    {
        goAnchor = transform.Find("Animator/Anchor").gameObject;
        atorAnimator = transform.Find("Animator").GetComponent<Animator>();

        onWinEventDelegates = new MessageDelegates
         (
             new Dictionary<string, EventDelegate>
             {
                { SlotMachineEvent.SkipWinLine, OnSkipWin},
                { SlotMachineEvent.SingleWinLine, OnSingleWin},
                { SlotMachineEvent.TotalWinLine, OnSingleWin}
             }
         );

        EventCenter.Instance.AddEventListener<EventData>(SlotMachineEvent.ON_WIN_EVENT, onWinEventDelegates.Delegate);


        InitParam();
    }

    private void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener<EventData>(SlotMachineEvent.ON_WIN_EVENT, onWinEventDelegates.Delegate);

    }

    void InitParam()
    {
        goAnchor.SetActive(false);
    }

    void OnSkipWin(EventData receivedEvent = null)
    {
        goAnchor.SetActive(false);
    }

    void OnSingleWin(EventData receivedEvent = null)
    {
        if (!_spinWEBB.Instance.isShowWinCredit)
            return;

        SymbolWin sw = receivedEvent.value as SymbolWin;

        goAnchor.SetActive(true);
        atorAnimator.Play("Scale");
        SetCredit(sw.earnCredit);
    }

    public void SetCredit(long credit)
    {
        if (txtWinCredit != null)
        {
            //txtWinCredit.text = $"${credit.ToString()}"; 
            txtWinCredit.text = $"{credit.ToString()}";
        }
    }



    /*
    Coroutine _cor;
    private void OnEnable()
    {
        ClearCor();
        //_cor =  StartCoroutine(ChangeScale());
    }

    IEnumerator  ChangeScale()
    {
        yield return null;
    }

    void ClearCor()
    {
        if (_cor != null)
        {
            StopCoroutine(_cor);
        }
        _cor = null;
    }*/

}
