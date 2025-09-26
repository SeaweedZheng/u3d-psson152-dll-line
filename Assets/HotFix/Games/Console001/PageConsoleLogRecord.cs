#define SQLITE_ASYNC
using Console001;
using Game;
using Mono.Data.Sqlite;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
public class PageConsoleLogRecord : PageMachineButtonBase
{

    public GameObject goEventContain, goErrorContain;

    public Button btnClose, btnPrev, btnNext;

    public TextMeshProUGUI tmpPageButtom, tmpStacktraceEvnet, tmpStacktraceError;

    public Text txtStacktraceEvnet, txtStacktraceError;


    public TMP_Dropdown drpDateEvent, drpDateError;

    public PageController ctrPage;

    List<PageButtomInfo> pageButtomInfo = new List<PageButtomInfo>()
    {
        new PageButtomInfo("Event Record, Page {0} of {1}"),
        new PageButtomInfo("Warniing History, Page {0} of {1}"),
    };

    private void Awake()
    {
        btnClose.onClick.AddListener(OnClickClose);
        btnNext.onClick.AddListener(OnClickNext);
        btnPrev.onClick.AddListener(OnClickPrev);

        // 添加监听器
        drpDateEvent.onValueChanged.AddListener(OnDropdownChangedDateEvent);
        drpDateError.onValueChanged.AddListener(OnDropdownChangedDateError);

        ctrPage.pageHandle.AddListener(OnPageChange);
    }

    private void OnEnable()
    {
        InitParam();
    }


    private void InitParam()
    {
        ClearEventSelect();
        ClearErrorSelect();

        InitLogEventInfo();
        InitLogErrorInfo();

        ctrPage.PageSet(0, 10);
    }


    void OnClickClose()=>PageManager.Instance.ClosePage(this);

    const string FORMAT_DATE_DAY = "yyyy-MM-dd";
    const int PER_PAGE_NUM = 14;
    int pageIndex = 0;


    public void OnPageChange(int index)
    {
        pageIndex = index;
        tmpPageButtom.text = string.Format(I18nMgr.T(pageButtomInfo[index].title),
            pageButtomInfo[index].curPageIndex + 1, pageButtomInfo[index].totalPageCount);
    }




    int fromIdxEvent = 0;
    List<TableLogRecordItem> resLogEventRecord;
    List<string> dropdownEventDateLst;

    int fromIdxError = 0;
    List<TableLogRecordItem> resLogErrorRecord;
    List<string> dropdownErrorDateLst;


#if !SQLITE_ASYNC
    void InitLogEventInfo()
    {

        //string sql = $"select distinct date(created_at) from {tableName}";
        string sql = $"SELECT created_at FROM {ConsoleTableName.TABLE_LOG_EVENT_RECORD}";

        DebugUtil.Log(sql);
        List<long> date = new List<long>();
        dropdownEventDateLst = new List<string>();
        SQLiteHelper01.Instance.ExecuteQueryAfterOpenDB(sql, (SqliteDataReader sdr) =>
        {
            while (sdr.Read())
            {
                long d = sdr.GetInt64(0);
                date.Add(d);
            }
            foreach (long timestamp in date)
            {
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(timestamp);
                //DateTime utcDateTime = dateTimeOffset.UtcDateTime;
                //string time = utcDateTime.ToString(FORMAT_DATE_DAY);

                DateTime localDateTime = dateTimeOffset.LocalDateTime;
                string time = localDateTime.ToString(FORMAT_DATE_DAY);

                if (!dropdownEventDateLst.Contains(time))
                {
                    //dropdownEventDateLst.Add(time);
                    dropdownEventDateLst.Insert(0, time);
                    DebugUtil.Log($"时间搓：{timestamp} 时间 ：{time}");
                }
            }

            drpDateEvent.options.Clear();
            if (dropdownEventDateLst.Count > 0)
            {
                drpDateEvent.AddOptions(dropdownEventDateLst);
                drpDateEvent.RefreshShownValue();// 最后，刷新Dropdown以应用更改
                drpDateEvent.value = 0;
                OnDropdownChangedDateEvent(0);
            }
        });
    }



#else
    void InitLogEventInfo()
    {

        //string sql = $"select distinct date(created_at) from {tableName}";
        string sql = $"SELECT created_at FROM {ConsoleTableName.TABLE_LOG_EVENT_RECORD}";

        DebugUtils.Log(sql);
        List<long> date = new List<long>();
        dropdownEventDateLst = new List<string>();
        SQLiteAsyncHelper.Instance.ExecuteQueryAsync(sql, null, (SqliteDataReader sdr) =>
        {
            while (sdr.Read())
            {
                long d = sdr.GetInt64(0);
                date.Add(d);
            }
            foreach (long timestamp in date)
            {
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(timestamp);
                //DateTime utcDateTime = dateTimeOffset.UtcDateTime;
                //string time = utcDateTime.ToString(FORMAT_DATE_DAY);

                DateTime localDateTime = dateTimeOffset.LocalDateTime;
                string time = localDateTime.ToString(FORMAT_DATE_DAY);

                if (!dropdownEventDateLst.Contains(time))
                {
                    //dropdownEventDateLst.Add(time);
                    dropdownEventDateLst.Insert(0, time);
                    DebugUtils.Log($"时间搓：{timestamp} 时间 ：{time}");
                }
            }

            drpDateEvent.options.Clear();
            if (dropdownEventDateLst.Count > 0)
            {
                drpDateEvent.AddOptions(dropdownEventDateLst);
                drpDateEvent.RefreshShownValue();// 最后，刷新Dropdown以应用更改
                drpDateEvent.value = 0;
                OnDropdownChangedDateEvent(0);
            }
        });
    }


#endif



#if !SQLITE_ASYNC

    void OnDropdownChangedDateEvent(int index)
    {
        // 根据选中的值进行相应的处理
        DebugUtil.Log($"Selected item index: {drpDateEvent.value}  index:{index}");

        //string sql2 = "SELECT * FROM your_table_name WHERE DATE(DATETIME(created_at / 1000, 'unixepoch', 'localtime')) = '2024-12-10'";
        string sql2 = $"SELECT * FROM {ConsoleTableName.TABLE_LOG_EVENT_RECORD} WHERE DATE(DATETIME(created_at / 1000, 'unixepoch', 'localtime')) = '{dropdownEventDateLst[index]}'"; //可以用
                                                                                                                                                                 
        //DebugUtil.Log(sql2);
        SQLiteHelper01.Instance.ExecuteQueryAfterOpenDB(sql2, (SqliteDataReader sdr) =>
        {
            resLogEventRecord = new List<TableLogRecordItem>();
            while (sdr.Read())
            {
                resLogEventRecord.Insert(0,
                new TableLogRecordItem()
                {
                    log_type = sdr.GetString(sdr.GetOrdinal("log_type")),
                    log_content = sdr.GetString(sdr.GetOrdinal("log_content")),
                    log_stacktrace = sdr.GetString(sdr.GetOrdinal("log_stacktrace")),
                    log_tag = sdr.GetString(sdr.GetOrdinal("log_tag")),
                    created_at = sdr.GetInt64(sdr.GetOrdinal("created_at")),
                });
            }

            pageButtomInfo[0].curPageIndex = 0;
            int totalPageCount = (resLogEventRecord.Count + (PER_PAGE_NUM - 1)) / PER_PAGE_NUM; //向上取整
            pageButtomInfo[0].totalPageCount = totalPageCount;
            fromIdxEvent = 0;
            SetUIEvent();
        });
    }



#else
    void OnDropdownChangedDateEvent(int index)
    {
        // 根据选中的值进行相应的处理
        DebugUtils.Log($"Selected item index: {drpDateEvent.value}  index:{index}");

        //string sql2 = "SELECT * FROM your_table_name WHERE DATE(DATETIME(created_at / 1000, 'unixepoch', 'localtime')) = '2024-12-10'";
        string sql2 = $"SELECT * FROM {ConsoleTableName.TABLE_LOG_EVENT_RECORD} WHERE DATE(DATETIME(created_at / 1000, 'unixepoch', 'localtime')) = '{dropdownEventDateLst[index]}'"; //可以用

        //DebugUtil.Log(sql2);
        SQLiteAsyncHelper.Instance.ExecuteQueryAsync(sql2, null,(SqliteDataReader sdr) =>
        {
            resLogEventRecord = new List<TableLogRecordItem>();
            while (sdr.Read())
            {
                resLogEventRecord.Insert(0,
                new TableLogRecordItem()
                {
                    log_type = sdr.GetString(sdr.GetOrdinal("log_type")),
                    log_content = sdr.GetString(sdr.GetOrdinal("log_content")),
                    log_stacktrace = sdr.GetString(sdr.GetOrdinal("log_stacktrace")),
                    log_tag = sdr.GetString(sdr.GetOrdinal("log_tag")),
                    created_at = sdr.GetInt64(sdr.GetOrdinal("created_at")),
                });
            }

            pageButtomInfo[0].curPageIndex = 0;
            int totalPageCount = (resLogEventRecord.Count + (PER_PAGE_NUM - 1)) / PER_PAGE_NUM; //向上取整
            pageButtomInfo[0].totalPageCount = totalPageCount;
            fromIdxEvent = 0;
            SetUIEvent();
        });
    }


#endif





    void SetUIEvent()
    {
        int lastIdx = fromIdxEvent + PER_PAGE_NUM - 1;
        if (lastIdx > resLogEventRecord.Count - 1)
        {
            lastIdx = resLogEventRecord.Count - 1;
        }

        Transform tfmMiddle = goEventContain.transform;

        foreach (Transform item in tfmMiddle)
        {
            item.gameObject.SetActive(false);
        }
        for (int i = 0; i <= lastIdx - fromIdxEvent; i++)
        {
            Transform item = tfmMiddle.GetChild(i);
            item.gameObject.SetActive(true);
            TableLogRecordItem res = resLogEventRecord[i + fromIdxEvent];

            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(res.created_at);
            //DateTime utcDateTime = dateTimeOffset.UtcDateTime;
            DateTime localDateTime = dateTimeOffset.LocalDateTime;
            item.Find("Time/Text").GetComponent<TextMeshProUGUI>().text = localDateTime.ToString("yyyy-MM-dd HH:mm:ss");

            item.Find("Tag/Text").GetComponent<TextMeshProUGUI>().text = $"--{res.log_tag}";
            string content = Encoding.UTF8.GetString(Convert.FromBase64String(res.log_content));
            item.Find("Content/Text").GetComponent<TextMeshProUGUI>().text = content;
            Button btn = item.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                foreach (Transform tfm in tfmMiddle)
                    tfm.Find("BG").gameObject.SetActive(false);
                item.Find("BG").gameObject.SetActive(true);
                 /*tmpStacktraceEvnet.text =
                    $"[content]:\n\n{content}\n\n[stacktrace]:\n\n" +
                    Encoding.UTF8.GetString(Convert.FromBase64String(res.log_stacktrace));*/

               txtStacktraceEvnet.text =
                $"[content]:\n\n{content}\n\n[stacktrace]:\n\n" +
                Encoding.UTF8.GetString(Convert.FromBase64String(res.log_stacktrace));
            });
        }
    }








#if !SQLITE_ASYNC

    void InitLogErrorInfo()
    {

        //string sql = $"select distinct date(created_at) from {tableName}";
        string sql = $"SELECT created_at FROM {ConsoleTableName.TABLE_LOG_ERROR_RECORD}";

        DebugUtil.Log(sql);
        List<long> date = new List<long>();
        dropdownErrorDateLst = new List<string>();
        SQLiteHelper01.Instance.ExecuteQueryAfterOpenDB(sql, (SqliteDataReader sdr) =>
        {
            while (sdr.Read())
            {
                long d = sdr.GetInt64(0);
                date.Add(d);
            }
            foreach (long timestamp in date)
            {
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(timestamp);
                //DateTime utcDateTime = dateTimeOffset.UtcDateTime;
                //string time = utcDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                //string time = utcDateTime.ToString(FORMAT_DATE_DAY);

                DateTime localDateTime = dateTimeOffset.LocalDateTime;
                string time = localDateTime.ToString(FORMAT_DATE_DAY);

                if (!dropdownErrorDateLst.Contains(time))
                {
                    //dropdownErrorDateLst.Add(time);
                    dropdownErrorDateLst.Insert(0, time);
                    DebugUtil.Log($"时间搓：{timestamp} 时间 ：{time}");
                }
            }

            drpDateError.options.Clear();
            if (dropdownErrorDateLst.Count > 0)
            {
                drpDateError.AddOptions(dropdownErrorDateLst);
                drpDateError.RefreshShownValue();// 最后，刷新Dropdown以应用更改
                drpDateError.value = 0;
                OnDropdownChangedDateError(0);
            }
        });
    }


#else
    void InitLogErrorInfo()
    {

        //string sql = $"select distinct date(created_at) from {tableName}";
        string sql = $"SELECT created_at FROM {ConsoleTableName.TABLE_LOG_ERROR_RECORD}";

        DebugUtils.Log(sql);
        List<long> date = new List<long>();
        dropdownErrorDateLst = new List<string>();
        SQLiteAsyncHelper.Instance.ExecuteQueryAsync(sql, null, (SqliteDataReader sdr) =>
        {
            while (sdr.Read())
            {
                long d = sdr.GetInt64(0);
                date.Add(d);
            }
            foreach (long timestamp in date)
            {
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(timestamp);
                //DateTime utcDateTime = dateTimeOffset.UtcDateTime;
                //string time = utcDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                //string time = utcDateTime.ToString(FORMAT_DATE_DAY);

                DateTime localDateTime = dateTimeOffset.LocalDateTime;
                string time = localDateTime.ToString(FORMAT_DATE_DAY);

                if (!dropdownErrorDateLst.Contains(time))
                {
                    //dropdownErrorDateLst.Add(time);
                    dropdownErrorDateLst.Insert(0, time);
                    DebugUtils.Log($"时间搓：{timestamp} 时间 ：{time}");
                }
            }

            drpDateError.options.Clear();
            if (dropdownErrorDateLst.Count > 0)
            {
                drpDateError.AddOptions(dropdownErrorDateLst);
                drpDateError.RefreshShownValue();// 最后，刷新Dropdown以应用更改
                drpDateError.value = 0;
                OnDropdownChangedDateError(0);
            }
        });
    }


#endif




#if !SQLITE_ASYNC
    void OnDropdownChangedDateError(int index)
    {
        // 根据选中的值进行相应的处理
        DebugUtil.Log($"Selected item index: {drpDateError.value}  index:{index}");

        //string sql2 = "SELECT * FROM your_table_name WHERE DATE(DATETIME(created_at / 1000, 'unixepoch', 'localtime')) = '2024-12-10'";
        string sql2 = $"SELECT * FROM {ConsoleTableName.TABLE_LOG_ERROR_RECORD} WHERE DATE(DATETIME(created_at / 1000, 'unixepoch', 'localtime')) = '{dropdownErrorDateLst[index]}'"; //可以用
                                                                                                                                                                         
        SQLiteHelper01.Instance.ExecuteQueryAfterOpenDB(sql2, (SqliteDataReader sdr) =>
        {
            resLogErrorRecord = new List<TableLogRecordItem>();
            while (sdr.Read())
            {
                resLogErrorRecord.Insert(0,
                new TableLogRecordItem()
                {
                    log_type = sdr.GetString(sdr.GetOrdinal("log_type")),
                    log_content = sdr.GetString(sdr.GetOrdinal("log_content")),
                    log_stacktrace = sdr.GetString(sdr.GetOrdinal("log_stacktrace")),
                    log_tag = sdr.GetString(sdr.GetOrdinal("log_tag")),
                    created_at = sdr.GetInt64(sdr.GetOrdinal("created_at")),
                });
            }

            pageButtomInfo[1].curPageIndex = 0;
            int totalPageCount = (resLogErrorRecord.Count + (PER_PAGE_NUM - 1)) / PER_PAGE_NUM; //向上取整
            pageButtomInfo[1].totalPageCount = totalPageCount;
            fromIdxError = 0;
            SetUIError();
        });
    }


#else

    void OnDropdownChangedDateError(int index)
    {
        // 根据选中的值进行相应的处理
        DebugUtils.Log($"Selected item index: {drpDateError.value}  index:{index}");

        //string sql2 = "SELECT * FROM your_table_name WHERE DATE(DATETIME(created_at / 1000, 'unixepoch', 'localtime')) = '2024-12-10'";
        string sql2 = $"SELECT * FROM {ConsoleTableName.TABLE_LOG_ERROR_RECORD} WHERE DATE(DATETIME(created_at / 1000, 'unixepoch', 'localtime')) = '{dropdownErrorDateLst[index]}'"; //可以用

        SQLiteAsyncHelper.Instance.ExecuteQueryAsync(sql2, null, (SqliteDataReader sdr) =>
        {
            resLogErrorRecord = new List<TableLogRecordItem>();
            while (sdr.Read())
            {
                resLogErrorRecord.Insert(0,
                new TableLogRecordItem()
                {
                    log_type = sdr.GetString(sdr.GetOrdinal("log_type")),
                    log_content = sdr.GetString(sdr.GetOrdinal("log_content")),
                    log_stacktrace = sdr.GetString(sdr.GetOrdinal("log_stacktrace")),
                    log_tag = sdr.GetString(sdr.GetOrdinal("log_tag")),
                    created_at = sdr.GetInt64(sdr.GetOrdinal("created_at")),
                });
            }

            pageButtomInfo[1].curPageIndex = 0;
            int totalPageCount = (resLogErrorRecord.Count + (PER_PAGE_NUM - 1)) / PER_PAGE_NUM; //向上取整
            pageButtomInfo[1].totalPageCount = totalPageCount;
            fromIdxError = 0;
            SetUIError();
        });
    }

#endif


    void SetUIError()
    {
        int lastIdx = fromIdxError + PER_PAGE_NUM - 1;
        if (lastIdx > resLogErrorRecord.Count - 1)
        {
            lastIdx = resLogErrorRecord.Count - 1;
        }

        Transform tfmMiddle = goErrorContain.transform;

        foreach (Transform item in tfmMiddle)
        {
            item.gameObject.SetActive(false);
        }
        for (int i = 0; i <= lastIdx - fromIdxError; i++)
        {
            Transform item = tfmMiddle.GetChild(i);
            item.gameObject.SetActive(true);
            TableLogRecordItem res = resLogErrorRecord[i + fromIdxError];

            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(res.created_at);
            //DateTime utcDateTime = dateTimeOffset.UtcDateTime;
            DateTime localDateTime = dateTimeOffset.LocalDateTime;
            item.Find("Time/Text").GetComponent<TextMeshProUGUI>().text = localDateTime.ToString("yyyy-MM-dd HH:mm:ss");

            item.Find("Tag/Text").GetComponent<TextMeshProUGUI>().text = $"--{res.log_tag}";

            string content = Encoding.UTF8.GetString(Convert.FromBase64String(res.log_content));
            item.Find("Content/Text").GetComponent<TextMeshProUGUI>().text = content;
            Button btn = item.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                foreach (Transform tfm in tfmMiddle)
                    tfm.Find("BG").gameObject.SetActive(false);
                item.Find("BG").gameObject.SetActive(true);
                
                 /*tmpStacktraceError.text =
                    $"[content]:\n\n{content}\n\n[stacktrace]:\n\n" +
                    Encoding.UTF8.GetString(Convert.FromBase64String(res.log_stacktrace)); */

                txtStacktraceError.text =
                    $"[content]:\n\n{content}\n\n[stacktrace]:\n\n" +
                    Encoding.UTF8.GetString(Convert.FromBase64String(res.log_stacktrace));
            });
        }
    }


    private void OnNextEventRecord()
    {
        if (fromIdxEvent + PER_PAGE_NUM >= resLogEventRecord.Count)
            return;
        ClearEventSelect();
        fromIdxEvent += PER_PAGE_NUM;
        pageButtomInfo[0].curPageIndex++;
        SetUIEvent();
    }

    private void OnPrevEventRecord()
    {
        if (fromIdxEvent <= 0)
            return;
        ClearEventSelect();
        fromIdxEvent -= PER_PAGE_NUM;
        pageButtomInfo[0].curPageIndex--;
        if (fromIdxEvent < 0)
        {
            pageButtomInfo[0].curPageIndex = 0;
            fromIdxEvent = 0;
        }
        SetUIEvent();
    }


    private void OnNextErrorRecord()
    {
        if (fromIdxError + PER_PAGE_NUM >= resLogErrorRecord.Count)
            return;
        ClearErrorSelect();
        fromIdxError += PER_PAGE_NUM;
        pageButtomInfo[1].curPageIndex++;
        SetUIError();
    }

    private void OnPrevErrorRecord()
    {
        if (fromIdxError <= 0)
            return;
        ClearErrorSelect();
        fromIdxError -= PER_PAGE_NUM;
        pageButtomInfo[1].curPageIndex--;
        if (fromIdxError < 0)
        {
            pageButtomInfo[1].curPageIndex = 0;
            fromIdxError = 0;
        }
        SetUIError();
    }



    private void ClearEventSelect()
    {
        tmpStacktraceEvnet.text = "";
        foreach (Transform tfm in goEventContain.transform)
            tfm.Find("BG").gameObject.SetActive(false);
    }

    private void ClearErrorSelect()
    {
        tmpStacktraceError.text = "";
        foreach (Transform tfm in goErrorContain.transform)
            tfm.Find("BG").gameObject.SetActive(false);
    }
    void OnClickNext()
    {
        if (pageIndex == 0)
        {
            OnNextEventRecord();
        }
        else if (pageIndex == 1)
        {
            OnNextErrorRecord();
        }
        OnPageChange(pageIndex);
    }

    void OnClickPrev()
    {
        if (pageIndex == 0)
        {
            OnPrevEventRecord();
        }
        else if (pageIndex == 1)
        {
            OnPrevErrorRecord();
        }
        OnPageChange(pageIndex);
    }


}
