#define SQLITE_ASYNC
using Game;
using Mono.Data.Sqlite;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using TMPro;
using SimpleJSON;
using _consoleBB = PssOn00152.ConsoleBlackboard02;



namespace Console001
{
    public class PageButtomInfo
    {
        public string title;
        public long curPageIndex;
        public long totalPageCount;

        public PageButtomInfo(string title)
        {
            this.title = title;
            this.curPageIndex = 0;
            this.totalPageCount = 1;
        }
    }
    public class PageConsoleGameHistory : PageMachineButtonBase
    {
        public Button btnClose, btnPrev, btnNext;
        public TextMeshProUGUI tmpPageButtom, tmpGameTitle;

        public GameObject goMiddleJackpotRecord, goLeftGame, goRightGame, goSymbols;

        public Dropdown drpDateJackpotRecord;
        public TMP_Dropdown tmpdrpDateJackpotRecord; // TMP_Dropdown 和  Dropdown 方法一样
        TMP_Dropdown drpDateJPR => tmpdrpDateJackpotRecord;  //tmpdrpDateJackpotRecord;

        public TMP_Dropdown tmpdrpDateGameRecord;



        public PageController ctrlPage;

        List<PageButtomInfo> pageButtomInfo = new List<PageButtomInfo>()
        {
            new PageButtomInfo("Game History, Page {0} of {1}") ,
            new PageButtomInfo("Jackpot History, Page {0} of {1}"),
        };

        const string FORMAT_DATE_SECOND = "yyyy-MM-dd HH:mm:ss";
        const string FORMAT_DATE_DAY = "yyyy-MM-dd";

        private void Awake()
        {
            btnClose.onClick.AddListener(OnClickClose);

            btnNext.onClick.AddListener(OnClickNext);

            btnPrev.onClick.AddListener(OnClickPrev);

            // 添加监听器
            drpDateJPR.onValueChanged.AddListener(OnDropdownChangedJackpotRecord);
            tmpdrpDateGameRecord.onValueChanged.AddListener(OnDropdownChangedGameRecord);
            
        }

        private void OnDestroy()
        {
            ResourceManager.Instance.UnloadAssetBundle(MARK_BUNDLE_GAME_HISTORY_PGAE);
        }

        void OnClickClose()
        {
            PageManager.Instance.ClosePage(this);
        }



        [Button]
        public void CleatAllRecord()
        {

            // ====游戏记录



            // 去掉所有图片
            Transform tfmSymbols = goSymbols.transform;
            foreach (Transform symble in tfmSymbols)
            {
                symble.Find("Image").GetComponent<Image>().sprite = null;
            }

            // 标题
            tmpGameTitle.text = "";

            //去掉所有数据
            Transform tfmLefGame = goLeftGame.transform;
            Transform tfmRightGame = goRightGame.transform;

            foreach (Transform tfm in tfmLefGame)
            {
                Transform tfmTarget = tfm.Find("Anchor/Text (2)");
                if (tfmTarget != null)
                {
                    TextMeshProUGUI comp = tfmTarget.GetComponent<TextMeshProUGUI>();
                    if(comp != null)
                    {
                        comp.text = "--";
                    }
                }
            }
            foreach (Transform tfm in tfmRightGame)
            {
                Transform tfmTarget = tfm.Find("Anchor/Text (2)");
                if (tfmTarget != null)
                {
                    TextMeshProUGUI comp = tfmTarget.GetComponent<TextMeshProUGUI>();
                    if (comp != null)
                    {
                        comp.text = "--";
                    }
                }
            }

            // 页角
            tmpPageButtom.text = string.Format(I18nMgr.T(pageButtomInfo[0].title), 1, 1);

            //==== 彩金记录

        }




        //private void OnEnable()
        private void OnEnable()
        {
            //OnPageChange(0);
            InitParam();
        }

        void InitParam()
        {
            CleatAllRecord();
            InitJackpotRecordInfo();
            InitGameRecordInfo();

            ctrlPage.PageSet(0, 10);
        }





        int pageIndex = 0;
        public void OnPageChange(int index)
        {
            pageIndex = index;
            ResetButtom(pageIndex);
        }


        private void ResetButtom(int index)
        {
            tmpPageButtom.text = string.Format(I18nMgr.T(pageButtomInfo[index].title),
                pageButtomInfo[index].curPageIndex + 1, pageButtomInfo[index].totalPageCount);
        }

        void OnClickNext()
        {
            if (pageIndex == 0)
            {
                OnNextGameRecord();
            }
            else if (pageIndex == 1)
            {
                OnNextJackpotRecord();
            }
            OnPageChange(pageIndex);
        }

        void OnClickPrev()
        {
            if (pageIndex == 0)
            {
                OnPrevGameRecord();
            }
            else if (pageIndex == 1)
            {
                OnPrevJackpotRecord();
            }
            OnPageChange(pageIndex);
        }




        #region 彩金记录

        const int perPageNumJackpotRecord = 10;
        int fromIdxJackpotRecord = 0;
        List<string> dropdownDateLstJackpotRecord;
        List<TableJackpotRecordItem> resJackpotRecord;




        void ClearJackpotRecord()
        {
            foreach (Transform chd in goMiddleJackpotRecord.transform)
            {
                chd.gameObject.SetActive(false);
            }
        }





#if !SQLITE_ASYNC

        void OnDropdownChangedJackpotRecord(int index)
        {

            // 根据选中的值进行相应的处理
            //DebugUtil.Log($"Selected item index: {drpDateJPR.value}  index:{index}");

            //string sql2 = "SELECT * FROM your_table_name WHERE DATE(DATETIME(created_at / 1000, 'unixepoch', 'localtime')) = '2024-12-10'";
            string sql2 = $"SELECT * FROM {ConsoleTableName.TABLE_JACKPOT_RECORD} WHERE DATE(DATETIME(created_at / 1000, 'unixepoch', 'localtime')) = '{dropdownDateLstJackpotRecord[index]}'"; //可以用

            SQLiteHelper01.Instance.ExecuteQueryAfterOpenDB(sql2, (SqliteDataReader sdr) =>
            {

                resJackpotRecord = new List<TableJackpotRecordItem>();
                while (sdr.Read())
                {
                    //string d = sdr.ge(0);
                    //date.Add(d);

                    //d.date = sdr.GetString(sdr.GetOrdinal("All_Report1"));
                    //d.score = sdr.GetString(sdr.GetOrdinal("All_Report2"));
                    //d.jpType = sdr.GetString(sdr.GetOrdinal("All_Report21"));

                    //string res =   sdr.GetString(sdr.GetOrdinal("device_type"));
                    //DebugUtil.Log($" device_type = {res}");
                    resJackpotRecord.Insert(0,
                    new TableJackpotRecordItem()
                    {
                        user_id = sdr.GetString(sdr.GetOrdinal("user_id")),
                        game_id = sdr.GetInt64(sdr.GetOrdinal("game_id")),
                        game_uid = sdr.GetString(sdr.GetOrdinal("game_uid")),
                        jp_name = sdr.GetString(sdr.GetOrdinal("jp_name")),
                        jp_level = sdr.GetInt64(sdr.GetOrdinal("jp_level")),
                        win_credit = sdr.GetInt64(sdr.GetOrdinal("win_credit")),
                        credit_before = sdr.GetInt64(sdr.GetOrdinal("credit_before")),
                        credit_after = sdr.GetInt64(sdr.GetOrdinal("credit_after")),
                        created_at = sdr.GetInt64(sdr.GetOrdinal("created_at")),
                    });
                }

                pageButtomInfo[1].curPageIndex = 0;
                int totalPageCount = (resJackpotRecord.Count + (perPageNumJackpotRecord - 1)) / perPageNumJackpotRecord; //向上取整
                pageButtomInfo[1].totalPageCount = totalPageCount;
                fromIdxJackpotRecord = 0;
                SetUICoinInOut();
            });
        }

#else
        void OnDropdownChangedJackpotRecord(int index)
        {

            // 根据选中的值进行相应的处理
            //DebugUtil.Log($"Selected item index: {drpDateJPR.value}  index:{index}");

            //string sql2 = "SELECT * FROM your_table_name WHERE DATE(DATETIME(created_at / 1000, 'unixepoch', 'localtime')) = '2024-12-10'";
            string sql2 = $"SELECT * FROM {ConsoleTableName.TABLE_JACKPOT_RECORD} WHERE DATE(DATETIME(created_at / 1000, 'unixepoch', 'localtime')) = '{dropdownDateLstJackpotRecord[index]}'"; //可以用

            SQLiteAsyncHelper.Instance.ExecuteQueryAsync(sql2, null, (SqliteDataReader sdr) =>
            {

                resJackpotRecord = new List<TableJackpotRecordItem>();
                while (sdr.Read())
                {
                    //string d = sdr.ge(0);
                    //date.Add(d);

                    //d.date = sdr.GetString(sdr.GetOrdinal("All_Report1"));
                    //d.score = sdr.GetString(sdr.GetOrdinal("All_Report2"));
                    //d.jpType = sdr.GetString(sdr.GetOrdinal("All_Report21"));

                    //string res =   sdr.GetString(sdr.GetOrdinal("device_type"));
                    //DebugUtil.Log($" device_type = {res}");
                    resJackpotRecord.Insert(0,
                    new TableJackpotRecordItem()
                    {
                        user_id = sdr.GetString(sdr.GetOrdinal("user_id")),
                        game_id = sdr.GetInt64(sdr.GetOrdinal("game_id")),
                        game_uid = sdr.GetString(sdr.GetOrdinal("game_uid")),
                        jp_name = sdr.GetString(sdr.GetOrdinal("jp_name")),
                        jp_level = sdr.GetInt64(sdr.GetOrdinal("jp_level")),
                        win_credit = sdr.GetInt64(sdr.GetOrdinal("win_credit")),
                        credit_before = sdr.GetInt64(sdr.GetOrdinal("credit_before")),
                        credit_after = sdr.GetInt64(sdr.GetOrdinal("credit_after")),
                        created_at = sdr.GetInt64(sdr.GetOrdinal("created_at")),
                    });
                }

                pageButtomInfo[1].curPageIndex = 0;
                int totalPageCount = (resJackpotRecord.Count + (perPageNumJackpotRecord - 1)) / perPageNumJackpotRecord; //向上取整
                pageButtomInfo[1].totalPageCount = totalPageCount;
                fromIdxJackpotRecord = 0;
                SetUICoinInOut();
            });
        }


#endif




        void SetUICoinInOut()
        {
            int lastIdx = fromIdxJackpotRecord + perPageNumJackpotRecord - 1;
            if (lastIdx > resJackpotRecord.Count - 1)
            {
                lastIdx = resJackpotRecord.Count - 1;
            }

            Transform tfmMiddle = goMiddleJackpotRecord.transform;

            foreach (Transform item in tfmMiddle)
            {
                item.gameObject.SetActive(false);
            }
            for (int i = 0; i <= lastIdx - fromIdxJackpotRecord; i++)
            {
                Transform item = tfmMiddle.GetChild(i);
                item.gameObject.SetActive(true);
                TableJackpotRecordItem res = resJackpotRecord[i + fromIdxJackpotRecord];

                item.Find("JP Name/Text").GetComponent<TextMeshProUGUI>().text = I18nMgr.T(res.jp_name);
                item.Find("Game Number/Text").GetComponent<TextMeshProUGUI>().text = res.game_uid;
                item.Find("Credit/Text").GetComponent<TextMeshProUGUI>().text = res.win_credit.ToString();  // res.win_credit.ToString("N2");  // 游戏彩金是带有小数的
                item.Find("Before Credit/Text").GetComponent<TextMeshProUGUI>().text = res.credit_before.ToString();
                item.Find("After Credit/Text").GetComponent<TextMeshProUGUI>().text = res.credit_after.ToString();

                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(res.created_at);
                //DateTime utcDateTime = dateTimeOffset.UtcDateTime;
                DateTime localDateTime = dateTimeOffset.LocalDateTime;
                item.Find("Date/Text").GetComponent<TextMeshProUGUI>().text = localDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }




#if !SQLITE_ASYNC

        void InitJackpotRecordInfo()
        {
            ClearJackpotRecord();

            //string sql = $"select distinct date(created_at) from {tableName}";
            string sql = $"SELECT created_at FROM {ConsoleTableName.TABLE_JACKPOT_RECORD}";

            List<long> date = new List<long>();
            dropdownDateLstJackpotRecord = new List<string>();
            SQLiteHelper01.Instance.ExecuteQueryAfterOpenDB(sql, (SqliteDataReader sdr) =>
            {
                while (sdr.Read())
                {
                    long d = sdr.GetInt64(0);
                    date.Add(d);
                }
                foreach (long timestamp in date)
                {
                    //DebugUtil.Log($"时间搓：{timestamp}");
                    DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(timestamp);
                    // DateTime utcDateTime = dateTimeOffset.UtcDateTime;
                    // string time = utcDateTime.ToString(FORMAT_DATE_DAY);
      

                    DateTime localDateTime = dateTimeOffset.LocalDateTime;
                    string time = localDateTime.ToString(FORMAT_DATE_DAY);
                    if (!dropdownDateLstJackpotRecord.Contains(time))
                    {
                        //dropdownDateLstJackpotRecord.Add(time);
                        dropdownDateLstJackpotRecord.Insert(0, time);
                        //DebugUtil.Log($"时间搓：{timestamp} 时间 ：{time}");
                    }
                }

                drpDateJPR.options.Clear();
                if (dropdownDateLstJackpotRecord.Count > 0)
                {
                    drpDateJPR.AddOptions(dropdownDateLstJackpotRecord);
                    drpDateJPR.RefreshShownValue();// 最后，刷新Dropdown以应用更改
                    drpDateJPR.value = 0;
                    OnDropdownChangedJackpotRecord(0);
                }
            });
        }

#else
        void InitJackpotRecordInfo()
        {
            ClearJackpotRecord();

            /*
            # 如果 created_at 存储的是毫秒级的时间戳，您需要将其除以 1000 来转换为秒：      

            SELECT DISTINCT DATE(FROM_UNIXTIME(created_at / 1000)) AS created_at_only
            FROM coin_in_out_record;
             */


            //string sql = $"select distinct date(created_at) from {tableName}";
            string sql = $"SELECT created_at FROM {ConsoleTableName.TABLE_JACKPOT_RECORD}";

            List<long> date = new List<long>();
            dropdownDateLstJackpotRecord = new List<string>();
            SQLiteAsyncHelper.Instance.ExecuteQueryAsync(sql, null, (SqliteDataReader sdr) =>
            {
                while (sdr.Read())
                {
                    long d = sdr.GetInt64(0);
                    date.Add(d);
                }
                foreach (long timestamp in date)
                {
                    //DebugUtil.Log($"时间搓：{timestamp}");
                    DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(timestamp);
                    // DateTime utcDateTime = dateTimeOffset.UtcDateTime;
                    // string time = utcDateTime.ToString(FORMAT_DATE_DAY);


                    DateTime localDateTime = dateTimeOffset.LocalDateTime;
                    string time = localDateTime.ToString(FORMAT_DATE_DAY);
                    if (!dropdownDateLstJackpotRecord.Contains(time))
                    {
                        //dropdownDateLstJackpotRecord.Add(time);
                        dropdownDateLstJackpotRecord.Insert(0, time);
                        //DebugUtil.Log($"时间搓：{timestamp} 时间 ：{time}");
                    }
                }

                drpDateJPR.options.Clear();
                if (dropdownDateLstJackpotRecord.Count > 0)
                {
                    drpDateJPR.AddOptions(dropdownDateLstJackpotRecord);
                    drpDateJPR.RefreshShownValue();// 最后，刷新Dropdown以应用更改
                    drpDateJPR.value = 0;
                    OnDropdownChangedJackpotRecord(0);
                }
            });
        }


#endif






        private void OnNextJackpotRecord()
        {
            if (fromIdxJackpotRecord + perPageNumJackpotRecord >= resJackpotRecord.Count)
                return;

            fromIdxJackpotRecord += perPageNumJackpotRecord;
            pageButtomInfo[1].curPageIndex++;
            SetUICoinInOut();
        }

        private void OnPrevJackpotRecord()
        {
            if (fromIdxJackpotRecord <= 0)
                return;

            fromIdxJackpotRecord -= perPageNumJackpotRecord;
            pageButtomInfo[1].curPageIndex--;
            if (fromIdxJackpotRecord < 0)
            {
                pageButtomInfo[1].curPageIndex = 0;
                fromIdxJackpotRecord = 0;
            }
            SetUICoinInOut();
        }



        #endregion






        #region 游戏记录

        // const int perPageNumCoinInOut = 10;
        //int idxGameRecord = 0;
        /// <summary> 游戏记录总局数 </summary>
        //long gameTotolNum = 0;
        //const string TABLE_COIN_IN_OUT_RECORD = "coin_in_out_record";
        // const string FORMAT_DATE_SECOND = "yyyy-MM-dd HH:mm:ss";
        //const string FORMAT_DATE_DAY = "yyyy-MM-dd";
        // List<string> dropdownDateLst;



        List<string> dropdownDateLstGameRecord;



#if !SQLITE_ASYNC || true

        void OnDropdownChangedGameRecord(int index)
        {

            if (index == 0)
            {
                SQLiteHelper.Instance.OpenDB(ConsoleTableName.DB_NAME, (connect) =>
                {
                    string sql = $"SELECT COUNT(*) FROM sqlite_master WHERE type ='table' and name='{ConsoleTableName.TABLE_SLOT_GAME_RECORD}';";
                    //创建命令
                    var command = connect.CreateCommand();
                    command.CommandText = sql;
                    int count = Convert.ToInt32(command.ExecuteScalar());

                    //如果结果为1则表示存在该表格
                    bool isExists = count == 1;

                    if (isExists)
                    {
                        sql = $"SELECT COUNT(*) FROM {ConsoleTableName.TABLE_SLOT_GAME_RECORD}";
                        DebugUtils.Log(sql);
                        using (var command1 = new SqliteCommand(sql, connect))
                        {
                            pageButtomInfo[0].totalPageCount = (long)command1.ExecuteScalar();
                            pageButtomInfo[0].curPageIndex = 0;

                            if (pageButtomInfo[0].totalPageCount > 0)
                                GetGameData(0);
                        }
                    }
                    else
                    {
                        DebugUtils.Log($"不存在表 {ConsoleTableName.TABLE_SLOT_GAME_RECORD} ");
                    }
                });
            }
            else
            {

                string yyyy_MM_dd = dropdownDateLstGameRecord[(int)index];

                string sql2 = $"SELECT * FROM {ConsoleTableName.TABLE_SLOT_GAME_RECORD} WHERE DATE(DATETIME(created_at / 1000, 'unixepoch', 'localtime')) = '{yyyy_MM_dd}'"; //可以用

                SQLiteHelper.Instance.ExecuteQueryAfterOpenDB(sql2, (SqliteDataReader sdr) =>
                {
                    resGameRecord = new List<TableSlotGameRecordItem>();
                    while (sdr.Read())
                    {
                        resGameRecord.Insert(0,
                        new TableSlotGameRecordItem()
                        {
                            scene = sdr.GetString(sdr.GetOrdinal("scene")),
                            game_id = sdr.GetInt32(sdr.GetOrdinal("game_id")),
                            total_bet = sdr.GetInt64(sdr.GetOrdinal("total_bet")),
                            lines_played = sdr.GetInt32(sdr.GetOrdinal("lines_played")),
                            credit_before = sdr.GetInt64(sdr.GetOrdinal("credit_before")),
                            credit_after = sdr.GetInt64(sdr.GetOrdinal("credit_after")),
                            //base_game_win_credit = sdr.GetInt64(sdr.GetOrdinal("base_game_win_credit")),
                            jackpot_win_credit = sdr.GetInt64(sdr.GetOrdinal("jackpot_win_credit")),
                            base_game_win_credit = sdr.GetInt64(sdr.GetOrdinal("base_game_win_credit")),
                            free_spin_win_credit = sdr.GetInt64(sdr.GetOrdinal("free_spin_win_credit")),
                            bonus_game_win_credit = sdr.GetInt64(sdr.GetOrdinal("bonus_game_win_credit")),
                            game_uid = sdr.GetString(sdr.GetOrdinal("game_uid")),
                            created_at = sdr.GetInt64(sdr.GetOrdinal("created_at")),
                        });
                    }

                    pageButtomInfo[0].curPageIndex = 0;
                    int totalPageCount = resGameRecord.Count;
                    pageButtomInfo[0].totalPageCount = totalPageCount;

                    GetGameRecordWhen(0);
                });

             }

            ResetButtom(pageIndex);
        }



#else
        void OnDropdownChangedGameRecord(int index)
        {

            if (index == 0) // 全部游戏
            {

                SQLiteAsyncHelper.Instance.CheckTableExistsAsync(ConsoleTableName.TABLE_SLOT_GAME_RECORD, (isExists) =>
                {

                    if (isExists)
                    {
                        string sql = $"SELECT COUNT(*) FROM {ConsoleTableName.TABLE_SLOT_GAME_RECORD}";
                        SQLiteAsyncHelper.Instance.ExecuteTotalCount(sql, null, (toalCount) =>
                        {

                            pageButtomInfo[0].totalPageCount = toalCount;
                            pageButtomInfo[0].curPageIndex = 0;

                            if (pageButtomInfo[0].totalPageCount > 0)
                                GetGameData(0);

                        });

                    }

                    ResetButtom(pageIndex);

                });

            }
            else
            {

                string yyyy_MM_dd = dropdownDateLstGameRecord[(int)index];

                string sql2 = $"SELECT * FROM {ConsoleTableName.TABLE_SLOT_GAME_RECORD} WHERE DATE(DATETIME(created_at / 1000, 'unixepoch', 'localtime')) = '{yyyy_MM_dd}'"; //可以用

                SQLiteAsyncHelper.Instance.ExecuteQueryAsync(sql2, null, (SqliteDataReader sdr) =>
                {
                    resGameRecord = new List<TableSlotGameRecordItem>();
                    while (sdr.Read())
                    {
                        resGameRecord.Insert(0,
                        new TableSlotGameRecordItem()
                        {
                            scene = sdr.GetString(sdr.GetOrdinal("scene")),
                            game_id = sdr.GetInt32(sdr.GetOrdinal("game_id")),
                            total_bet = sdr.GetInt64(sdr.GetOrdinal("total_bet")),
                            lines_played = sdr.GetInt32(sdr.GetOrdinal("lines_played")),
                            credit_before = sdr.GetInt64(sdr.GetOrdinal("credit_before")),
                            credit_after = sdr.GetInt64(sdr.GetOrdinal("credit_after")),
                            //base_game_win_credit = sdr.GetInt64(sdr.GetOrdinal("base_game_win_credit")),
                            jackpot_win_credit = sdr.GetInt64(sdr.GetOrdinal("jackpot_win_credit")),
                            base_game_win_credit = sdr.GetInt64(sdr.GetOrdinal("base_game_win_credit")),
                            free_spin_win_credit = sdr.GetInt64(sdr.GetOrdinal("free_spin_win_credit")),
                            bonus_game_win_credit = sdr.GetInt64(sdr.GetOrdinal("bonus_game_win_credit")),
                            game_uid = sdr.GetString(sdr.GetOrdinal("game_uid")),
                            created_at = sdr.GetInt64(sdr.GetOrdinal("created_at")),
                        });
                    }

                    pageButtomInfo[0].curPageIndex = 0;
                    int totalPageCount = resGameRecord.Count;
                    pageButtomInfo[0].totalPageCount = totalPageCount;

                    GetGameRecordWhen(0);


                    ResetButtom(pageIndex);
                });

            }

        }


#endif











#if !SQLITE_ASYNC || true
        void InitGameRecordInfo()
        {


            string sql = $"SELECT created_at FROM {ConsoleTableName.TABLE_SLOT_GAME_RECORD}";

            List<long> date = new List<long>();
            dropdownDateLstGameRecord = new List<string>();
            SQLiteHelper.Instance.ExecuteQueryAfterOpenDB(sql, (SqliteDataReader sdr) =>
            {
                while (sdr.Read())
                {
                    long d = sdr.GetInt64(0);
                    date.Add(d);
                }
                foreach (long timestamp in date)
                {
                    //DebugUtil.Log($"时间搓：{timestamp}");
                    DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(timestamp);
                    //DateTime utcDateTime = dateTimeOffset.UtcDateTime;
                    //string time = utcDateTime.ToString(FORMAT_DATE_DAY);

                    DateTime localDateTime = dateTimeOffset.LocalDateTime;
                    string time = localDateTime.ToString(FORMAT_DATE_DAY);

                    if (!dropdownDateLstGameRecord.Contains(time))
                    {
                        //dropdownDateLstGameRecord.Add(time);
                        dropdownDateLstGameRecord.Insert(0, time);
                        //DebugUtil.Log($"时间搓：{timestamp} 时间 ：{time}");
                    }
                }
                dropdownDateLstGameRecord.Insert(0, I18nMgr.T("All"));

                tmpdrpDateGameRecord.options.Clear();
                if (dropdownDateLstGameRecord.Count > 0)
                {
                    tmpdrpDateGameRecord.AddOptions(dropdownDateLstGameRecord);
                    tmpdrpDateGameRecord.RefreshShownValue();// 最后，刷新Dropdown以应用更改
                    tmpdrpDateGameRecord.value = 0;
                    OnDropdownChangedGameRecord(0);
                }
            });

        }



#else
        void InitGameRecordInfo()
        {


            string sql = $"SELECT created_at FROM {ConsoleTableName.TABLE_SLOT_GAME_RECORD}";

            List<long> date = new List<long>();
            dropdownDateLstGameRecord = new List<string>();
            SQLiteAsyncHelper.Instance.ExecuteQueryAsync(sql, null, (SqliteDataReader sdr) =>
            {
                while (sdr.Read())
                {
                    long d = sdr.GetInt64(0);
                    date.Add(d);
                }
                foreach (long timestamp in date)
                {
                    //DebugUtil.Log($"时间搓：{timestamp}");
                    DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(timestamp);
                    //DateTime utcDateTime = dateTimeOffset.UtcDateTime;
                    //string time = utcDateTime.ToString(FORMAT_DATE_DAY);

                    DateTime localDateTime = dateTimeOffset.LocalDateTime;
                    string time = localDateTime.ToString(FORMAT_DATE_DAY);

                    if (!dropdownDateLstGameRecord.Contains(time))
                    {
                        //dropdownDateLstGameRecord.Add(time);
                        dropdownDateLstGameRecord.Insert(0, time);
                        //DebugUtil.Log($"时间搓：{timestamp} 时间 ：{time}");
                    }
                }
                dropdownDateLstGameRecord.Insert(0, I18nMgr.T("All"));

                tmpdrpDateGameRecord.options.Clear();
                if (dropdownDateLstGameRecord.Count > 0)
                {
                    tmpdrpDateGameRecord.AddOptions(dropdownDateLstGameRecord);
                    tmpdrpDateGameRecord.RefreshShownValue();// 最后，刷新Dropdown以应用更改
                    tmpdrpDateGameRecord.value = 0;
                    OnDropdownChangedGameRecord(0);
                }
            });

        }

#endif








        TableSlotGameRecordItem curGame;
        //JSONNode gameScene;



        void OnNextGameRecord()
        {
            if (pageButtomInfo[0].curPageIndex + 1 >= pageButtomInfo[0].totalPageCount)
                return;

            pageButtomInfo[0].curPageIndex++;



            if (tmpdrpDateGameRecord.value == 0)
                GetGameData(pageButtomInfo[0].curPageIndex); //全部
            else
                GetGameRecordWhen((int)pageButtomInfo[0].curPageIndex); //日期

        }

        void OnPrevGameRecord()
        {
            if (pageButtomInfo[0].curPageIndex <= 0)
                return;

            pageButtomInfo[0].curPageIndex--;
            //GetAllGameData(pageButtomInfo[0].curPageIndex);

            if (tmpdrpDateGameRecord.value == 0)
                GetGameData(pageButtomInfo[0].curPageIndex); //全部
            else
                GetGameRecordWhen((int)pageButtomInfo[0].curPageIndex); //日期
        }


        List<TableSlotGameRecordItem> resGameRecord;

        /// <summary>
        /// 获取某天的所有游戏
        /// </summary>
        /// <param name="index"></param>
        void GetGameRecordWhen(int index)
        {
            SetUIGame(resGameRecord[index]);
        }




#if !SQLITE_ASYNC || true
        /// <summary>
        /// 获取全部游戏
        /// </summary>
        /// <param name="index"></param>
        void GetGameData(long index)
        {
            string sql = $"SELECT * FROM {ConsoleTableName.TABLE_SLOT_GAME_RECORD} LIMIT 1 OFFSET {pageButtomInfo[0].totalPageCount - 1 - index}";

            SQLiteHelper.Instance.ExecuteQueryAfterOpenDB(sql, (SqliteDataReader sdr) =>
            {

                while (sdr.Read())
                {
                    curGame = new TableSlotGameRecordItem()
                    {
                        scene = sdr.GetString(sdr.GetOrdinal("scene")),
                        game_id = sdr.GetInt32(sdr.GetOrdinal("game_id")),
                        total_bet = sdr.GetInt64(sdr.GetOrdinal("total_bet")),
                        lines_played = sdr.GetInt32(sdr.GetOrdinal("lines_played")),
                        credit_before = sdr.GetInt64(sdr.GetOrdinal("credit_before")),
                        credit_after = sdr.GetInt64(sdr.GetOrdinal("credit_after")),
                        //total_win_credit = sdr.GetInt64(sdr.GetOrdinal("total_win_credit")),
                        jackpot_win_credit = sdr.GetInt64(sdr.GetOrdinal("jackpot_win_credit")),
                        base_game_win_credit = sdr.GetInt64(sdr.GetOrdinal("base_game_win_credit")),
                        free_spin_win_credit = sdr.GetInt64(sdr.GetOrdinal("free_spin_win_credit")),
                        bonus_game_win_credit = sdr.GetInt64(sdr.GetOrdinal("bonus_game_win_credit")),
                        game_uid = sdr.GetString(sdr.GetOrdinal("game_uid")),
                        created_at = sdr.GetInt64(sdr.GetOrdinal("created_at")),
                    };
                    //gameScene = JSONNode.Parse(curGame.scene);

                }
                SetUIGame(curGame);
            });
        }



#else
        /// <summary>
        /// 获取全部游戏
        /// </summary>
        /// <param name="index"></param>
        void GetGameData(long index)
        {
            string sql = $"SELECT * FROM {ConsoleTableName.TABLE_SLOT_GAME_RECORD} LIMIT 1 OFFSET {pageButtomInfo[0].totalPageCount - 1 - index}";

            SQLiteAsyncHelper.Instance.ExecuteQueryAsync(sql, null, (SqliteDataReader sdr) =>
            {

                while (sdr.Read())
                {
                    curGame = new TableSlotGameRecordItem()
                    {
                        scene = sdr.GetString(sdr.GetOrdinal("scene")),
                        game_id = sdr.GetInt32(sdr.GetOrdinal("game_id")),
                        total_bet = sdr.GetInt64(sdr.GetOrdinal("total_bet")),
                        lines_played = sdr.GetInt32(sdr.GetOrdinal("lines_played")),
                        credit_before = sdr.GetInt64(sdr.GetOrdinal("credit_before")),
                        credit_after = sdr.GetInt64(sdr.GetOrdinal("credit_after")),
                        //total_win_credit = sdr.GetInt64(sdr.GetOrdinal("total_win_credit")),
                        jackpot_win_credit = sdr.GetInt64(sdr.GetOrdinal("jackpot_win_credit")),
                        base_game_win_credit = sdr.GetInt64(sdr.GetOrdinal("base_game_win_credit")),
                        free_spin_win_credit = sdr.GetInt64(sdr.GetOrdinal("free_spin_win_credit")),
                        bonus_game_win_credit = sdr.GetInt64(sdr.GetOrdinal("bonus_game_win_credit")),
                        game_uid = sdr.GetString(sdr.GetOrdinal("game_uid")),
                        created_at = sdr.GetInt64(sdr.GetOrdinal("created_at")),
                    };
                    //gameScene = JSONNode.Parse(curGame.scene);

                }
                SetUIGame(curGame);
            });
        }


#endif




        const string MARK_BUNDLE_GAME_HISTORY_PGAE = "MARK_BUNDLE_GAME_HISTORY_PGAE";

        Dictionary<string,Sprite> SymbolIcon = new Dictionary<string, Sprite>();
        Sprite GetSymbolIcon(int symbolIndex)
        {
            string assetPath = ConfigUtils.GetSymbolUrl(symbolIndex); 

            if (SymbolIcon.ContainsKey(assetPath))
            {
                return SymbolIcon[assetPath] ;
            }

            Texture2D texture = ResourceManager.Instance.LoadAssetAtPath<Texture2D>(assetPath, MARK_BUNDLE_GAME_HISTORY_PGAE);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            SymbolIcon.Add(assetPath, sprite);
            return sprite;
        }


        void SetUIGame(TableSlotGameRecordItem curGame)
        {
            JSONNode gameScene = JSONNode.Parse(curGame.scene);
            //DebugUtil.Log($"gameScene = {gameScene.ToString()}");

            // gameScene["strDeckRowCol"]  //"10,4,9,5,4#6,11,1,8,4#1,0,9,5,3"
            string[] res = ((string)gameScene["strDeckRowCol"]).Split('#');

            List<string> deck = new List<string>();

            foreach (string s in res) {
                deck.AddRange(s.Split(','));
            }
            for (int i=0; i<deck.Count; i++)
            {
                int symbolNumber = int.Parse(deck[i]);
                Transform symble = goSymbols.transform.GetChild(i).Find("Image");

                symble.GetComponent<Image>().sprite = GetSymbolIcon(symbolNumber);

                if (symbolNumber == 11)
                    symble.localScale =new Vector3(1.34f,1,1); //财神要宽些
                else
                    symble.localScale = Vector3.one;
            }


            //tmpGameTitle.text = $"Caishen Daddy Game Recall {pageButtomInfo[0].curPageIndex + 1} of {pageButtomInfo[0].totalPageCount}";

            tmpGameTitle.text = string.Format(I18nMgr.T("{0} Game Recall {1} of {2}"),
                I18nMgr.T(ApplicationSettings.Instance.gameTheme), pageButtomInfo[0].curPageIndex + 1, pageButtomInfo[0].totalPageCount);

            Transform tfmLefGame = goLeftGame.transform;
            Transform tfmRightGame = goRightGame.transform;


            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(curGame.created_at);
            //DateTime utcDateTime = dateTimeOffset.UtcDateTime;
            DateTime localDateTime = dateTimeOffset.LocalDateTime;
            tfmLefGame.Find("Game Date Time/Anchor/Text (2)").GetComponent<TextMeshProUGUI>().text = localDateTime.ToString("yyyy-MM-dd HH:mm:ss");
           // tfmLefGame.Find("Game Result/Anchor/Text (2)").GetComponent<TextMeshProUGUI>().text = "";
            tfmLefGame.Find("Credits Before/Anchor/Text (2)").GetComponent<TextMeshProUGUI>().text = curGame.credit_before.ToString();
            tfmLefGame.Find("Credits After/Anchor/Text (2)").GetComponent<TextMeshProUGUI>().text = curGame.credit_after.ToString();
            tfmLefGame.Find("Total Bet/Anchor/Text (2)").GetComponent<TextMeshProUGUI>().text = curGame.total_bet.ToString();

            //tfmLefGame.Find("Total Win/Anchor/Text (2)").GetComponent<TextMeshProUGUI>().text = curGame.total_win_credit.ToString();

            tfmLefGame.Find("Base Game Win/Anchor/Text (2)").GetComponent<TextMeshProUGUI>().text = curGame.base_game_win_credit.ToString();
            tfmLefGame.Find("Jackpot Win/Anchor/Text (2)").GetComponent<TextMeshProUGUI>().text = curGame.jackpot_win_credit.ToString();

            // tfmRightGame.Find("Free Spin Win/Anchor/Text (2)").GetComponent<TextMeshProUGUI>().text = curGame.free_spin_win_credit.ToString();
            //tfmRightGame.Find("Free Spin Add/Anchor/Text (2)").GetComponent<TextMeshProUGUI>().text = curGame.free_spin_win_credit.ToString();

            /*
            tfmLefGame.Find("Hit The Jackpot/Anchor/Text (2)").
                GetComponent<TextMeshProUGUI>().text = I18nMgr.T(curGame.jackpot_win_credit > 0? "True":"False");
            */
            tfmLefGame.Find("Game Number/Anchor/Text (2)").
                GetComponent<TextMeshProUGUI>().text = curGame.game_uid;



            //====================== 右边


            tfmRightGame.Find("Is Free Spin/Anchor/Text (2)").
                GetComponent<TextMeshProUGUI>().text = I18nMgr.T((bool)gameScene["isFreeSpin"]?"True":"False");

            tfmRightGame.Find("Free Spin Total Times").gameObject.SetActive(false);
            tfmRightGame.Find("Free Spin Current Round").gameObject.SetActive(false);
            tfmRightGame.Find("Free Spin Add Times").gameObject.SetActive(false);


            if (gameScene.HasKey("isFreeSpin") && (bool)gameScene["isFreeSpin"])
            {
                tfmRightGame.Find("Free Spin Total Times").gameObject.SetActive(true);
                tfmRightGame.Find("Free Spin Current Round").gameObject.SetActive(true);
                tfmRightGame.Find("Free Spin Total Times/Anchor/Text (2)").GetComponent<TextMeshProUGUI>().text = $"{(int)gameScene["freeSpinTotalTimes"]}";
                tfmRightGame.Find("Free Spin Current Round/Anchor/Text (2)").GetComponent<TextMeshProUGUI>().text = $"{(int)gameScene["freeSpinPlayTimes"]}";
            }

            if (gameScene.HasKey("freeSpinAddNum") && 0 < (int)gameScene["freeSpinAddNum"])
            {
                int num = (int)gameScene["freeSpinAddNum"];
                tfmRightGame.Find("Free Spin Add Times").gameObject.SetActive(true);
                tfmRightGame.Find("Free Spin Add Times/Anchor/Text (2)").GetComponent<TextMeshProUGUI>().text = num.ToString();
            }


        }


        #endregion

    }


}

