using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.Events;
using Object = UnityEngine.Object;
using TMPro;
using SpringGUI;
using Sirenix.OdinInspector;


public class CalendarController: UIBehaviour
{
    private int _hourValue;
    public int HourValue { 
        get => _hourValue;
        set {
            _hourValue = value; 
            OnChangeSelectDateTime();
        } 
    }


    private int _minuteValue;

    public int MinuteValue { 
        get => _minuteValue; 
        set {
            _minuteValue = value;
            OnChangeSelectDateTime();
        }
    }

    private int currentDay = 0;

    private string _lastSelectDay = "";
    private string lastSelectDay
    {
        get => _lastSelectDay;
        set
        {
            _lastSelectDay = value;
            OnChangeSelectDateTime();
        }
    }

    #region click events
    public class DayClickEvent : UnityEvent<DateTime> { }
    public class MonthClickEvent : UnityEvent<DateTime> { }
    public class YearClickEvent : UnityEvent<DateTime> { }

    private DayClickEvent m_onDayClickEvent = new DayClickEvent();
    public DayClickEvent onDayClick
    {
        get { return m_onDayClickEvent; }
        set { m_onDayClickEvent = value; }
    }
    private MonthClickEvent m_onMonthClickEvent = new MonthClickEvent();
    public MonthClickEvent onMonthClick
    {
        get { return m_onMonthClickEvent; }
        set { m_onMonthClickEvent = value; }
    }
    private YearClickEvent m_onYearClickEvent = new YearClickEvent();
    public YearClickEvent onYearClick
    {
        get { return m_onYearClickEvent; }
        set { m_onYearClickEvent = value; }
    }

    public Action OnClickConfirmEvent;

    #endregion

    #region private && public members
    private DateTime m_selectDT = DateTime.Today;
    private readonly CalendarData m_calendarData = new CalendarData();
    private Transform m_Transform = null;
    [HideInInspector]
    public E_CalendarType CalendarType = E_CalendarType.Day;
    //public E_DisplayType DisplayType = E_DisplayType.Chinese;
    E_DisplayType DisplayType = E_DisplayType.Standard;
    private TextMeshProUGUI _timeButtonText = null;
    private GameObject _weeksGameObject = null;
    private GameObject _daysGameObejct = null;
    private GameObject _monthGameObject = null;
    private TextMeshProUGUI _tmpDateTip = null;
    private readonly List<DMY> _daysPool = new List<DMY>();
    private readonly List<DMY> _monthYearPool = new List<DMY>();
    private Button hourBtnAdd;
    private Button hourBtnReduce;
    private TextMeshProUGUI hourTxt;
    private Button minuteBtnAdd;
    private Button minuteBtnReduce;
    private TextMeshProUGUI minuteTxt;
    #endregion




    /// <summary> 日期栏根节点 </summary>
    public GameObject goRootWeeks;

    /// <summary> 日号栏根节点 </summary>
    public GameObject goRootDays;

    /// <summary> 月号栏根节点 </summary>
    public GameObject goRootMonths;


    /// <summary> 抬头日期下一个按钮 </summary>
    public GameObject goTitleNextButton;

    /// <summary> 抬头日期上一个按钮 </summary>
    public GameObject goTitlePrevButton;

    /// <summary> 抬头日期日期按钮 </summary>
    public GameObject goTitleContentButton;

    /// <summary> 抬头日期内容按钮 </summary>
    public GameObject goTitleContentText;



    /// <summary> 小时加键 </summary>
    public GameObject goHourAddButton;

    /// <summary> 小时减键 </summary>
    public GameObject goHourReduceButton;

    /// <summary> 分钟加键 </summary>
    public GameObject goMinuteAddButton;

    /// <summary> 分钟减键 </summary>
    public GameObject goMinuteReduceButton;

    /// <summary> 小时内容 </summary>
    public GameObject goHoutText;

    /// <summary> 分钟内容 </summary>
    public GameObject goMinuteText;

    /// <summary> 确认按钮 </summary>
    //public Button btnConfirm;


    public GameObject goDateTip;

    protected  void Awake()//protected override void Awake()
    {

        _tmpDateTip = goDateTip.GetComponent<TextMeshProUGUI>();    
        _timeButtonText = goTitleContentText.GetComponent<TextMeshProUGUI>();

        currentDay = DateTime.Today.Day;
        m_Transform = transform;
        _weeksGameObject = goRootWeeks;
        _daysGameObejct = goRootDays;
        _monthGameObject = goRootMonths;

        var weekPrefab = _weeksGameObject.transform.GetChild(0).gameObject;
        var dayPrefab = _daysGameObejct.transform.GetChild(0).gameObject;
        var monthPrefab = _monthGameObject.transform.GetChild(0).gameObject;

        WeekGenerator(weekPrefab, _weeksGameObject.transform);
        DayGenerator(dayPrefab, _daysGameObejct.transform);
        MonthGenerator(monthPrefab, _monthGameObject.transform);
        goTitleNextButton.GetComponent<Button>().onClick.AddListener(OnNextButtonClick);
        goTitlePrevButton.GetComponent<Button>().onClick.AddListener(OnLastButtonClick);
        goTitleContentButton.GetComponent<Button>().onClick.AddListener(OnTimeButtonClick);

        hourBtnAdd = goHourAddButton.GetComponent<Button>();
        hourBtnReduce = goHourReduceButton.GetComponent<Button>();
        minuteBtnAdd = goMinuteAddButton.GetComponent<Button>();
        minuteBtnReduce = goMinuteReduceButton.GetComponent<Button>();

        hourTxt = goHoutText.GetComponent<TextMeshProUGUI>();
        minuteTxt = goMinuteText.GetComponent<TextMeshProUGUI>();

        //bgBtnClose = transform.Find("bgBtn")?.GetComponent<Button>();
        //btnConfirm = transform.Find("Confirm").GetComponent<Button>();
        /*if (bgBtnClose != null)
        {
            bgBtnClose.onClick.AddListener(OnClickBgBtn);
        }*/
        //btnConfirm.onClick.AddListener(OnClickConfirm);
        hourBtnAdd.onClick.AddListener(OnClickHourAddBtn);
        hourBtnReduce.onClick.AddListener(OnClickHourReduceBtn);
        minuteBtnAdd.onClick.AddListener(OnClickMinuteAddBtn);
        minuteBtnReduce.onClick.AddListener(OnClickMinuteReduceBtn);
    }


    protected override void OnEnable()
    {
        base.OnEnable();
        InitTime();
    }

    public void InitTime()
    {
        //ResetShow();

        //goRootMonths.SetActive(false);

        m_selectDT = DateTime.Today;
        RefreshSelectDay();

        var now = DateTime.Now;
        HourValue = now.Hour;
        MinuteValue = now.Minute;
        hourTxt.text = GetHourStr();
        minuteTxt.text = GetMinuteStr();
        Refresh();
        RefreshTimeButtonContent();

        // 默认高亮
        List<DateTime> dateTimes = m_calendarData.Days(m_selectDT);
        for (int i = 0; i < _daysPool.Count; i++)
        {
            if (dateTimes[i].Month == m_selectDT.Month && dateTimes[i].Day == DateTime.Now.Day)
            {
                _daysPool[i].SetBGHightLight();
                break;
            }
        }

    }



    [Button]
    private void TestShowReasult()
    {
        DebugUtils.Log($"@@ 1 == {lastSelectDay}");
        DebugUtils.Log($"@@ 2 == {GetLastSelectDate()}");
    }

    void RefreshSelectDay() => lastSelectDay = m_selectDT.ToString("yyyy-MM-dd");
    /*private void ResetShow()
    {
        m_selectDT = DateTime.Today;
        RefreshSelectDay();

        var now = DateTime.Now;
        HourValue = now.Hour;
        MinuteValue = now.Minute;
        hourTxt.text = GetHourStr();
        minuteTxt.text = GetMinuteStr();
        Refresh();


        // 默认高亮
        List<DateTime> dateTimes = m_calendarData.Days(m_selectDT);
        for (int i = 0; i < _daysPool.Count; i++)
        {
            if (dateTimes[i].Month == m_selectDT.Month && dateTimes[i].Day == DateTime.Now.Day)
            {
                _daysPool[i].SetBGHightLight();
                break;
            }
        }

    }*/

    private string GetHourStr()=> _hourValue < 10 ? "0" + _hourValue : _hourValue.ToString();
    private string GetMinuteStr()=> _minuteValue < 10 ? "0" + _minuteValue : _minuteValue.ToString();
    private void OnClickHourAddBtn()
    {
        SetHourTxt(1);
    }

    private void OnClickMinuteAddBtn()
    {
        SetMinuteTxt(1);
    }
    private void OnClickHourReduceBtn()
    {
        SetHourTxt(-1);
    }

    private void OnClickMinuteReduceBtn()
    {
        SetMinuteTxt(-1);
    }

    private void SetHourTxt(int value)
    {
        HourValue += value;
        if (HourValue < 0)
        {
            HourValue = 23;
        }
        if (HourValue > 23)
        {
            HourValue = 0;
        }
        //hourTxt.text = HourValue < 10 ? "0" + HourValue : HourValue.ToString();
        hourTxt.text = GetHourStr();
    }

    private void SetMinuteTxt(int value)
    {
        MinuteValue += value;
        if (MinuteValue < 0)
        {
            MinuteValue = 59;
        }
        if (MinuteValue > 59)
        {
            MinuteValue = 0;
        }
        minuteTxt.text = GetMinuteStr();
    }

    #region operation functions
    private void OnTimeButtonClick()
    {
        if (CalendarType == E_CalendarType.Month)
            CalendarType = E_CalendarType.Year;
        if (CalendarType == E_CalendarType.Day)
        {
            CalendarType = E_CalendarType.Month;
            calendarTypeChange(false);
        }
        Refresh();
    }
    private void OnNextButtonClick()
    {
        //SetDayColor();
        if (CalendarType == E_CalendarType.Day)
            m_selectDT = m_selectDT.AddMonths(1);
        else if (CalendarType == E_CalendarType.Month)
            m_selectDT = m_selectDT.AddYears(1);
        else
            m_selectDT = m_selectDT.AddYears(12);
        Refresh();
        RefreshTimeButtonContentWithoutDay();
    }
    private void OnLastButtonClick()
    {
        //SetDayColor();
        if (CalendarType == E_CalendarType.Day)
            m_selectDT = m_selectDT.AddMonths(-1);
        else if (CalendarType == E_CalendarType.Month)
            m_selectDT = m_selectDT.AddYears(-1);
        else
            m_selectDT = m_selectDT.AddYears(-12);
        Refresh();
        RefreshTimeButtonContentWithoutDay();
    }
    #endregion

    #region days && weeks && months generator
    private void WeekGenerator(GameObject weekPrefab, Transform parent)
    {
        foreach (Transform tfm in parent)
        {
            tfm.gameObject.SetActive(false);
        }
        for (int i = parent.childCount; i <7; i++)
        {
            GameObject week = prefabGenerator(weekPrefab, parent);
        }
        for (int i = 0; i < 7; i++)
        {
            Transform tfm = parent.GetChild(i);
            tfm.gameObject.SetActive(true);
            tfm.GetComponent<Text>().text = I18nMgr.T(getWeekName(i.ToString()));
        }
        /*for (int i = 0; i < 7; i++)
        {
            GameObject week = prefabGenerator(weekPrefab, parent);
            week.GetComponent<TextMeshProUGUI>().text = getWeekName(i.ToString());
        }
        Destroy(weekPrefab);
        */
        }
    private void DayGenerator(GameObject dayPrefab, Transform parent)
    {
        foreach (Transform tfm in parent)
        {
            tfm.gameObject.SetActive(false);
        }
        for (int i = parent.childCount; i < 42; i++)
        {
            GameObject week = prefabGenerator(dayPrefab, parent);
        }
        for (int i = 0; i < 42; i++)
        {
            GameObject day = parent.GetChild(i).gameObject;
            day.SetActive(true);

            DMY dmy = day.AddComponent<DMY>();
            dmy.Init(day.transform.Find("Text").gameObject, day);

            day.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (dmy.GetTxtColor() == Color.gray) return;
                m_selectDT = dmy.CurrentDateTime;
                currentDay = dmy.CurrentDateTime.Day;
                onDayClick.Invoke(dmy.CurrentDateTime);
                Refresh();
                SetDayColor(dmy);
                RefreshTimeButtonContent();
                //lastSelectDay = _timeButtonText.text;
                RefreshSelectDay();
            });
            _daysPool.Add(dmy);
        }

        /*
        for (int i = 0; i < 42; i++)
        {
            GameObject day = prefabGenerator(dayPrefab, parent);
            DMY dmy = day.AddComponent<DMY>();
            day.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (dmy.GetTxtColor() == Color.gray) return;
                m_selectDT = dmy.CurrentDateTime;
                currentDay = dmy.CurrentDateTime.Day;
                onDayClick.Invoke(dmy.CurrentDateTime);
                Refresh();
                SetDayColor(dmy);
                RefreshTimeButtonContent();
                lastSelect = _timeButtonText.text;
            });
            _daysPool.Add(dmy);
        }
        Destroy(dayPrefab);
        */
    }

    private void SetDayColor(DMY dMY = null)
    {
        for (int i = 0; i < _daysPool.Count; i++)
        {

            if (dMY != null && _daysPool[i] == dMY)
            {
                _daysPool[i].SetBGHightLight();
            }
            else
            {
                _daysPool[i].SetBGDefault();
            }
        }
    }

    private void MonthGenerator(GameObject monthPrefab, Transform parent)
    {
        foreach (Transform tfm in parent)
        {
            tfm.gameObject.SetActive(false);
        }
        for (int i = parent.childCount; i < 12; i++)
        {
            GameObject month = prefabGenerator(monthPrefab, parent);
        }

        for (int i = 0; i < 12; i++)
        {

            GameObject month = parent.GetChild(i).gameObject;
            month.SetActive(true);

            DMY dmy = month.AddComponent<DMY>();
            dmy.Init(month.transform.Find("Text").gameObject, month);

            month.GetComponent<Button>().onClick.AddListener(() =>
            {
                m_selectDT = dmy.CurrentDateTime;
                if (CalendarType == E_CalendarType.Month)
                {
                    CalendarType = E_CalendarType.Day;
                    calendarTypeChange(true);
                    onMonthClick.Invoke(dmy.CurrentDateTime);
                }
                if (CalendarType == E_CalendarType.Year)
                {
                    CalendarType = E_CalendarType.Month;
                    onYearClick.Invoke(dmy.CurrentDateTime);
                }
                Refresh();
                RefreshTimeButtonContentWithoutDay(); //新加
            });
            _monthYearPool.Add(dmy);
        }
        /*for (int i = 0; i < 12; i++)
        {
            GameObject month = prefabGenerator(monthPrefab, parent);
            DMY dmy = month.AddComponent<DMY>();
            month.GetComponent<Button>().onClick.AddListener(() =>
            {
                m_selectDT = dmy.CurrentDateTime;
                if (CalendarType == E_CalendarType.Month)
                {
                    CalendarType = E_CalendarType.Day;
                    calendarTypeChange(true);
                    onMonthClick.Invoke(dmy.CurrentDateTime);
                }
                if (CalendarType == E_CalendarType.Year)
                {
                    CalendarType = E_CalendarType.Month;
                    onYearClick.Invoke(dmy.CurrentDateTime);
                }
                Refresh();
            });
            _monthYearPool.Add(dmy);
        }
        Destroy(monthPrefab);*/
    }
    private GameObject prefabGenerator(GameObject prefab, Transform parent)
    {
        GameObject go = Object.Instantiate(prefab);
        go.transform.SetParent(parent);
        go.transform.localScale = Vector3.one;
        return go;
    }
    private string getWeekName(string weekName)
    {
        switch (DisplayType)
        {
            case E_DisplayType.Standard:
                switch (weekName)
                {
                    case "0":
                        return "Sun";
                    case "1":
                        return "Mon";
                    case "2":
                        return "Tue";
                    case "3":
                        return "Wed";
                    case "4":
                        return "Thu";
                    case "5":
                        return "Fri";
                    case "6":
                        return "Sat";
                    default:
                        return "";
                }
            case E_DisplayType.Chinese:
                switch (weekName)
                {
                    case "0":
                        return "日";
                    case "1":
                        return "一";
                    case "2":
                        return "二";
                    case "3":
                        return "三";
                    case "4":
                        return "四";
                    case "5":
                        return "五";
                    case "6":
                        return "六";
                    default:
                        return "";
                }
            default:
                return "";
        }

    }
    private void calendarTypeChange(bool isDays)
    {
        _weeksGameObject.SetActive(isDays);
        _daysGameObejct.SetActive(isDays);
        _monthGameObject.SetActive(!isDays);
    }
    #endregion

    #region refresh calendar all component
    private void Refresh()
    {
        RefreshCalendar();
        //RefreshTimeButtonContent();
    }

    private void RefreshTimeButtonContent()
    {
        switch (CalendarType)
        {
            case E_CalendarType.Day:
                if (DisplayType == E_DisplayType.Standard) _timeButtonText.text = m_selectDT.ToString("yyyy-MM-dd");
                else _timeButtonText.text = m_selectDT.Year + "年" + m_selectDT.Month + "月" + m_selectDT.Day + "日";
                break;
            case E_CalendarType.Month:
                if (DisplayType == E_DisplayType.Standard) _timeButtonText.text = m_selectDT.Year + "-" + m_selectDT.Month;
                else _timeButtonText.text = m_selectDT.Year + "年" + m_selectDT.Month + "月";
                break;
            case E_CalendarType.Year:
                if (DisplayType == E_DisplayType.Standard) _timeButtonText.text = m_selectDT.Year.ToString();
                else _timeButtonText.text = m_selectDT.Year + "年";
                break;
        }
    }

    private void RefreshTimeButtonContentWithoutDay()
    {
        switch (CalendarType)
        {
            case E_CalendarType.Day:
                if (DisplayType == E_DisplayType.Standard) _timeButtonText.text = m_selectDT.ToString("yyyy-MM");
                else _timeButtonText.text = m_selectDT.Year + "年" + m_selectDT.Month + "月" + m_selectDT.Day + "日";
                break;
            case E_CalendarType.Month:
                if (DisplayType == E_DisplayType.Standard) _timeButtonText.text = m_selectDT.Year + "-" + m_selectDT.Month;
                else _timeButtonText.text = m_selectDT.Year + "年" + m_selectDT.Month + "月";
                break;
            case E_CalendarType.Year:
                if (DisplayType == E_DisplayType.Standard) _timeButtonText.text = m_selectDT.Year.ToString();
                else _timeButtonText.text = m_selectDT.Year + "年";
                break;
        }
    }


    public string GetLastSelectDate()
    {
        return string.Format("{0} {1}:{2}:00", lastSelectDay, GetHourStr(), GetMinuteStr());
    }


    public long GetLastSelectTimestamp()
    {
        string strDate = GetLastSelectDate(); // "2022-03-15 10:30:00";
        DateTimeOffset date = DateTimeOffset.Parse(strDate);
        long timeStamp = date.ToUnixTimeMilliseconds(); // date.ToUnixTimeSeconds();
        return timeStamp;
    }


    public string GetLastSelectDay()
    {
        return lastSelectDay;
    }

 /*   public string GetDateTxt()
    {
        return _timeButtonText.text;
    }

    public string GetHourTxt()
    {
        return hourTxt.text;
    }

    public string GetMinuteTxt()
    {
        return minuteTxt.text;
    }
*/




    private void RefreshCalendar()
    {
        if (CalendarType == E_CalendarType.Day) RefreshDays(m_calendarData.Days(m_selectDT));
        else if (CalendarType == E_CalendarType.Month) RefreshMonths(m_calendarData.Months(m_selectDT));
        else RefreshYears(m_calendarData.Years(m_selectDT));
    }
    private void RefreshDays(List<DateTime> dateTimes)
    {
        for (int i = 0; i < _daysPool.Count; i++)
        {
            _daysPool[i].SetDay(dateTimes[i], DisplayType, dateTimes[i].Month == m_selectDT.Month);
        }
    }
    private void RefreshMonths(List<DateTime> dateTimes)
    {
        for (int i = 0; i < _monthYearPool.Count; i++)
            //_monthYearPool[i].SetMonth(dateTimes[i], DisplayType);
            _monthYearPool[i].SetMonth(dateTimes[i], (res) => I18nMgr.T(res));
    }
    private void RefreshYears(List<DateTime> dateTimes)
    {
        for (int i = 0; i < _monthYearPool.Count; i++)
            _monthYearPool[i].SetYear(dateTimes[i], DisplayType);
    }
    #endregion



   /* [FormerlySerializedAs("OnChangeSelectDateTime")]
    [SerializeField]
    private UnityAction<string> m_OnChangeSelectDateTime;

    public UnityAction<string> onChangeSelectDateTime
    {
        get => m_OnChangeSelectDateTime;
        set => m_OnChangeSelectDateTime = value;
    }*/


    private void OnChangeSelectDateTime()
    {
        //m_OnChangeSelectDateTime?.Invoke(GetLastSelectDate());
        //DebugUtil.Log($"time  =  {_timeButtonText.text}");
        string res = I18nMgr.T("Selected Date & Time: {0}");
        //DebugUtil.LogError($"@@@@@@@ === {res}");
        _tmpDateTip.text = string.Format(res, GetLastSelectDate());
        //_tmpDateTip.text = $"Selected Data & Time: {GetLastSelectDate()}"; 
    }
}