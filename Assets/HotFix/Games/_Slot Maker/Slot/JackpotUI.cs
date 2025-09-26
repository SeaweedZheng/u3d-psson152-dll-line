using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JackpotUI : MonoBehaviour
{

    /// <summary>
    /// ²Ê½ð·¶Î§ÊÇ·ñÊÜÑº×¢Ó°Ïì
    /// </summary>
    public bool isEffectByBet = true;

    public float minCredit = 0;
    public float maxCredit = 100000;
    public float toCredit = 100000;


    private float m_NowCredit;
    public virtual float nowCredit
    {
        get
        {
            return m_NowCredit;
        }
        set
        {
            m_NowCredit = value;
        }
    }

    public string eventName = "";
    public bool isRandom = true;
    public GameObject goText;

    protected static readonly int gap = 2;
    protected int tempGap;
    protected float _minDataBase = 0;
    protected float _maxDataBase = 0;
    protected bool isJackpotRun = true;


    protected Text compText => goText?.GetComponent<Text>();
    protected TextMeshProUGUI compTmpugui => goText?.GetComponent<TextMeshProUGUI>();

    //protected MiniReelGroup  miniReelGroup => goText?.GetComponent<MiniReelGroup>();



   

    public virtual void AddToData(string str)
    {
        if (compTmpugui != null)
        {
            compTmpugui.text = str;
        }
        if (compText != null)
        {
            compText.text = str;
        }
    }
    public virtual void Start()
    {
        _minDataBase = minCredit;
        _maxDataBase = maxCredit;

        OnChangeRange();

        m_NowCredit = minCredit + Random.Range(0, (int)(maxCredit - minCredit));
        tempGap = gap;

        //MessageDispatcher.Register(EVTType.ON_CONTENT_EVENT, OnJackpotEvent);
        //MessageDispatcher.Register(EVTType.ON_CREDIT_EVENT, OnCreditEvent);
    }

    public virtual void OnDestroy()
    {
        //MessageDispatcher.UnRegister(EVTType.ON_CONTENT_EVENT, OnJackpotEvent);
       // MessageDispatcher.UnRegister(EVTType.ON_CREDIT_EVENT, OnCreditEvent);
    }

    protected void OnChangeRange()
    {
        if (isEffectByBet)
        {
            /*int betIndex = BlackboardUtils.FindVariable<int>("./betIndex").value;
            List<float> betListBase = BlackboardUtils.FindVariable<List<float>>("./gameNew/betListBase").value;
            int baseWager = BlackboardUtils.FindVariable<int>("./gameNew/baseWager").value;

            float time = betListBase[betIndex] / baseWager;

            minData = _minDataBase * time;
            maxData = _maxDataBase * time;*/
        }
    }

    /*
    protected void OnCreditEvent(EventData eventData)
    {
        if (eventData.name == "UpdateBetCredit")
        {
            OnChangeRange();
        }
    }

    protected void OnJackpotEvent(EventData eventData)
    {
        if (eventData.name == "JackpotRun")
        {
            isJackpotRun = true;
        }
        else if (eventData.name == "JackpotStop")
        {
            isJackpotRun = false;
        }
        else if (eventData.name == "JackpotHide")
        {
            gameObject.SetActive(false);
        }
        else if (eventData.name == "JackpotShow")
        {
            gameObject.SetActive(true);
        }
        else if (eventData.name == eventName)
        {
            _nowData = (float)eventData.value;
            //ContextUtils.SetGlobalText(compText, "TEXT_BET_CREDIT", (float)eventData.value);
            //compText.SetText($"{_nowData.ToString("N0")}");
            SetText($"{_nowData.ToString("N0")}");
        }
    }*/

    public virtual void Update()
    {

        tempGap--;
        if (tempGap <= 0)
        {
            tempGap = gap;

            if (isJackpotRun)
            {

                if (isRandom)
                {
                    m_NowCredit = minCredit + Random.Range(0, (int)(maxCredit - minCredit));
                    AddToData($"{m_NowCredit.ToString("N0")}");
                }
                else if(m_NowCredit <= toCredit)
                {
                    m_NowCredit++;
                    AddToData($"{m_NowCredit.ToString("N0")}");
                }

            }
        }
    }
}
