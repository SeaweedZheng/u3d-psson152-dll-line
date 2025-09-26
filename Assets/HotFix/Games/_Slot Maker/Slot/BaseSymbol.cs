using PssOn00152;
using SlotMaker;
using UnityEngine;
using UnityEngine.UI;
using _customBB = PssOn00152.CustomBlackboard;
using _spinWEBB = SlotMaker.SpinWinEffectSettingBlackboard;
//BaseIconItem BaseSymbol



public enum SymbolEffectType
{
    None = 0,
    Hit,
    Appear,
    FreeSpinTrigger,
    FreeSpinResult,
    Expectation1,
    Expectation2,
    Expectation3,
}
public partial class SymbolBase : MonoBehaviour
{
    public int number;
    public GameObject goIcon;
    public GameObject goBtn;

    
    private Image imgBase => goIcon.GetComponent<Image>();

    private SpriteRenderer sprdBase => goIcon.GetComponent<SpriteRenderer>();

    public Button btnBase => goBtn.GetComponent<Button>();

    protected virtual void Awake()
    {
        if (goIcon == null)
        {
            goIcon = gameObject;
        }

        if(goBtn == null)
        {
            goBtn = gameObject;
        }
    }


    public virtual void SetSymbolSprite(Sprite spt)
    {
        if (sprdBase != null)
        {
            sprdBase.sprite = spt;
        }else if (imgBase != null)
        {
            imgBase.sprite = spt;
            imgBase.SetNativeSize();
        }
    }

    public virtual void SetSymbolActive(bool active)
    {
        //imgIcon.enabled = active;
        goIcon?.SetActive(active);
    }

    public virtual void SetSymbolImage(int symbolNumber, bool needNativeSize = false)
    {
        DebugUtils.LogWarning("==@ ûʵ�� BaseIconItem - SetIconImage");
    }

    public virtual int GetSymbolNumber()
    {
        DebugUtils.LogWarning("==@ ûʵ�� BaseIconItem - GetIndex");
        return number;
    }

    /// <summary>
    /// ����ͼ���Ƿ���Ե����������
    /// </summary>
    /// <param name="state"></param>
    public virtual void SetBtnInteractableState(bool state)
    {
        DebugUtils.LogWarning("==@ ûʵ�� BaseIconItem - SetBtnInteractableState");
    }

    public virtual bool IsSpecailHitSymbol()
    {
        DebugUtils.LogWarning("==@ ûʵ�� BaseIconItem - IsSpecailIcon");
        return false;
    }

    /*public virtual void MoNisimulationClickIcon()
    {
        DebugUtil.LogWarning("==@ ûʵ�� BaseIconItem - MoNisimulationClickIcon");
    }*/
}
public partial class SymbolBase : MonoBehaviour
{

    /// <summary>
    /// ���ͼ����Ч
    /// </summary>
    /// <param name="goSymbolEffect"></param>
    /// <param name="isAmin"></param>
    /// <returns></returns>
    public virtual Transform AddSymbolEffect(GameObject goSymbolEffect, bool isAmin = true)
    {

        AnimBaseUI animUI = goSymbolEffect.GetComponent<AnimBaseUI>();
        if (animUI != null)
        {
            if (isAmin) 
                animUI.Play();
            else
                animUI.Pause();
        }


       Transform tfmRoot = transform.Find("Animator/Anchor");

        Transform tfm = goSymbolEffect.transform;
        //tfm.parent = tfmRoot;
        tfm.SetParent(tfmRoot);

        tfm.localScale = Vector3.one;
        //tfm.rotation = Quaternion.identity;
        tfm.localRotation = Quaternion.identity;
        tfm.localPosition = Vector3.zero;

        // �Ƿ�����ԭ��ͼ��
        if (_spinWEBB.Instance.isHideBaseIcon)
        {
            HideBaseSymbolIcon(true);
        }

        return tfm;
        // ���Ŷ���
    }


    /// <summary>
    /// ��ӱ߿���Ч
    /// </summary>
    /// <param name="borderEffect"></param>
    /// <returns></returns>
    public virtual Transform AddBorderEffect(GameObject borderEffect)
    {
        //DebugUtil.Log("==@ i am Base AddBorderEffect");

        Transform tfmRoot = transform.Find("Animator/Anchor");

        Transform tfm = borderEffect.transform;
        //tfm.parent = tfmRoot;
        tfm.SetParent(tfmRoot);

        tfm.localScale = Vector3.one;
        //tfm.rotation = Quaternion.identity;
        tfm.localRotation = Quaternion.identity;
        tfm.localPosition = Vector3.zero;

        return tfm;
        // ���Ŷ���
    }

    public virtual void ShowBiggerEffect()
    {
    }
    /// <summary>
    /// ����ͼ��ʱ���Ƿ�����ԭ�е�ͼ��
    /// </summary>
    /// <param name="isHide"></param>
    public virtual void HideBaseSymbolIcon(bool isHide)
    {
        goIcon.SetActive(!isHide);
    }
}