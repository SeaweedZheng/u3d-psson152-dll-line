using GameMaker;
using SBoxApi;
using System;
using System.Collections.Generic;
using UnityEngine;

//using static UnityEditor.Progress;
using _DeviceInfo = GameMaker.DeviceInfo;

//ChinaTown100
namespace PssOn00152
{
    public class ConsoleBlackboard02 : MonoSingleton<ConsoleBlackboard02>
    {

        ObservableObject m_Observable;
        public ObservableObject observable
        {
            get
            {
                if (m_Observable == null)
                {
                    string[] classNamePath = this.GetType().ToString().Split('.');
                    m_Observable = new ObservableObject(classNamePath[classNamePath.Length - 1], "@console/");
                }
                return m_Observable;
            }
        }


        #region SQL数据表
        [SerializeField]
        private TableBetItem m_TableBet;

        public TableBetItem tableBet
        {
            get => observable.GetProperty(() => m_TableBet);
            set => observable.SetProperty(ref m_TableBet, value);
            //set => m_TableMachine = value;
        }

        //public List<TableButtonsItem> tableButtons;


        public List<TableUsersItem> tableUsers;



        [SerializeField]
        private TableSysSettingItem m_TableSysSetting;
        public TableSysSettingItem tableSysSetting
        {
            get => observable.GetProperty(() => m_TableSysSetting);
            set => observable.SetProperty(ref m_TableSysSetting, value);
            //set => m_TableSysSetting = value;
        }




        public TableGameItem tableGame;

        //public List<TableGameAnalysisItem> tableGameAnalysis;

        //public List<TableSlotGameRecordItem> tableGameRecord;

        #endregion


        #region 算法卡数据
        /// <summary> 当前玩家数据 </summary>
        [SerializeField]
        private SBoxPlayerAccount m_SboxPlayerInfo;
        public SBoxPlayerAccount sboxPlayerInfo
        {
            get => observable.GetProperty(() => m_SboxPlayerInfo);
            set => observable.SetProperty(ref m_SboxPlayerInfo, value);
            //set => m_TableMachine = value;
        }

        /// <summary> 机台配置 </summary>
        [SerializeField]
        private SBoxConfData m_SBoxConfData;
        public SBoxConfData sboxConfData
        {
            //get => observable.GetProperty(() => m_SBoxConfData);
            //set => observable.SetProperty(ref m_SBoxConfData, value);
            get => m_SBoxConfData;
            set => m_SBoxConfData = value;
        }

        #endregion


        #region 平台信息


        /// <summary> 好酷端口号（分机号+1） </summary>
        public int iotPort => pid + 1;


        /// <summary> 分机号 </summary>
        public int pid = 0;



        /// <summary> 代理商号(线号)  </summary>
        public string agentID
        {
            get => $"{sboxConfData.LineId}";
            set => sboxConfData.LineId = int.Parse(value);
            //get => tableMachine.agent_id;
            //set => tableMachine.agent_id = value;
        }

        /// <summary> 机台号 </summary>
        public string machineID
        {
            //get => tableMachine.machine_id;
            get => $"{sboxConfData.MachineId}";
            set => sboxConfData.MachineId = int.Parse(value);
        }

        /// <summary> 渠道号(线上app才有)</summary>
        public string channelID
        {
            get => $"{sboxConfData.LineId}";
            set => sboxConfData.LineId = int.Parse(value);

        }



        /// <summary> 硬盘版本 </summary>
        public string hardwareVer
        {
            //get => tableMachine.hardware_ver;
            //set => tableMachine.hardware_ver = value;
            get => _hardwareVer;
            set => _hardwareVer = value;
        }
        string _hardwareVer = "";



        string _algorithmVer = "";
        /// <summary> 算法卡版本 </summary>
        public string algorithmVer
        {
            get => _algorithmVer;
            set => _algorithmVer = value;
        }


        #endregion


        #region 营收信息


        /// <summary> 历史总压注 </summary>
        public long historyTotalBet
        {
            //get => sboxPlayerInfo.Bets;
            get => TableBusniessTotalRecordAsyncManager.Instance.historyTotalBet;
        }


        /// <summary> 历史总赢 </summary>
        public long historyTotalWin
        {
            //get => sboxPlayerInfo.Wins;
            get => TableBusniessTotalRecordAsyncManager.Instance.historyTotalWin;
        }

        /// <summary> 历史总压分盈利(总压注分 - 总得分) </summary>
        public long historyTotalProfitBet
        {
            get => historyTotalBet - historyTotalWin;
        }


        /// <summary> 历史总投币个数 </summary>
        /*public long historyTotalCoinInNums
        {
            get => sboxPlayerInfo.CoinIn;
        }*/


        /// <summary> 历史总投币 </summary>
        public long historyTotalCoinInCredit
        {
            //get => historyTotalCoinInNums * coinInScale;
            get => TableBusniessTotalRecordAsyncManager.Instance.historyTotalCoinInCredit;
        }

        /// <summary> 历史总退票个数 （之前是对的，CoinOut返回的是数量。这个版本缺变成了分数！） </summary>
        /// <remarks>
        /// * 存在异议，这个参数改为private，不被外面调用。<br/>
        /// </remarks>
       /* private long historyTotalCoinOutNums
        {
            get => sboxPlayerInfo.CoinOut;
        }*/


        /// <summary> 历史总退票 </summary>
        /// <remarks>
        /// * 存在异议，这个参数暂时隐藏<br/>
        /// </remarks>
        /*public long historyTotalCoinOutCredit
        {
            get {
               if (coinOutScalePerCredit2Ticket > 1) // 1分几票(最小值为1)
               {
                    return historyTotalCoinOutNums / coinOutScalePerCredit2Ticket;
               }
               else if (coinOutScalePerTicket2Credit > 1) //1票几分(最小值为1) 
               {
                    return historyTotalCoinOutNums * coinOutScalePerTicket2Credit;
               }
               else if (coinOutScalePerTicket2Credit == 1 && coinOutScalePerCredit2Ticket ==1)
               {
                    return historyTotalCoinOutNums * 1;
               }
                return -999;
            }
        }*/


        /// <summary> 历史总退票 </summary>
        public long historyTotalCoinOutCredit
        {
            //get => historyTotalCoinOutNums;
            get => TableBusniessTotalRecordAsyncManager.Instance.historyTotalCoinOutCredit;
        }




        /// <summary> 历史总投币盈利(总投币 - 总退票) </summary>
        public long historyTotalProfitCoinIn
        {
            get => historyTotalCoinInCredit - historyTotalCoinOutCredit;
        }


        /// <summary> 历史总上分 </summary>
        public long historyTotalScoreUpCredit
        {
            //get => sboxPlayerInfo.ScoreIn;
            get => TableBusniessTotalRecordAsyncManager.Instance.historyTotalScoreUpCredit;
        }

        /// <summary> 历史总下分 </summary>
        public long historyTotalScoreDownCredit
        {
            //get => sboxPlayerInfo.ScoreOut;
            get => TableBusniessTotalRecordAsyncManager.Instance.historyTotalScoreDownCredit;
        }



        /// <summary> 历史总上分盈利(总上分 - 总下分) </summary>
        public long historyTotalProfitScoreUp
        {
            get => historyTotalScoreUpCredit - historyTotalScoreDownCredit;
        }


        /*
         账单详细信息：（什么时间？）
        年月日，时分
        线号/机台号/投币比例
        主机版本（软件版本-自定义 ？？）/算法版本/难度/盈利宕机（盈利宕机单位是万，数值需要乘一万）
        */
        public string billInfoTime
        {
            get
            {
                long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(timestamp);
                DateTime localDateTime = dateTimeOffset.LocalDateTime;
                //return localDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                return localDateTime.ToString("yyyy-MM-dd");
            }
        }

        public string billInfoLineMachineNumber
        {
            get => $"{sboxConfData.LineId}/{sboxConfData.MachineId}/1:{sboxConfData.CoinValue}";
        }

        public string billInfoHardwareAlgorithmVer
        {
            get
            {

                int winLockBalance = SBoxIdea.WinLockBalance(); // 盈利当机余额

                string tempStr = winLockBalance / 10000 > 0 ?
                    $"<color=#F8DF1B>{winLockBalance / 10000}</color>" : $"<color=#FF0000>{winLockBalance / 10000}</color>";

                //Debug.LogError($"【Test】：winLockBalance: {winLockBalance};  winLockBalance 除以 10000: {winLockBalance / 10000}");

                return $"{GlobalData.hotfixVersion}/{algorithmVer}/{difficultyName}/{tempStr}";

            }
        }

        /** 旧版本
        public string difficultyName
        {
            get
            {
                if (sboxConfData.difficulty < difficultyNames.Count)
                {
                    return difficultyNames[sboxConfData.difficulty];
                }
                return "--";
            }

        }*/

        public string difficultyName
        {
            get
            {
                if (dllLevelIndex < MachineDeviceCommonBiz.Instance.levelLst.Length)
                {
                    return MachineDeviceCommonBiz.Instance.levelLst[dllLevelIndex];
                }
                return "--";
            }

        }




        /*
        public long m_RemainingPoints = 99999;
        public long remainingPoints
        {
            get => m_RemainingPoints;
            set => m_RemainingPoints = value;
        }*/

        /*
        /// <summary> 历史总盈利 </summary>
        public long m_HistoryTotalProfit = 99;
        public long historyTotalProfit
        {
            get => m_HistoryTotalProfit;
            set => m_HistoryTotalProfit = value;
        }*/
        #endregion

        #region 游戏压注列表
        [SerializeField]
        private List<long> m_BetList;
        public List<long> betList
        {
            get => observable.GetProperty(() => m_BetList);
            set => observable.SetProperty(ref m_BetList, value);
        }

        [SerializeField]
        private List<BetAllow> m_BetAllowList;
        public List<BetAllow> betAllowList
        {
            get => observable.GetProperty(() => m_BetAllowList);
            set => observable.SetProperty(ref m_BetAllowList, value);
        }
        #endregion




        #region 联网彩金

        /// <summary> 是否开启大厅彩金 </summary>
        int m_IsJackpotOnLine = 0;
        public bool isJackpotOnLine
        {
            get
            {
                try
                {
                    //m_IsJackpotOnline = tableSysSetting.is_jackpot_online;
                    if (sboxConfData == null)
                    {
                        m_IsJackpotOnLine = 0;
                    }
                    else
                    {
                        m_IsJackpotOnLine = sboxConfData.NetJackpot;
                    }
                    return m_IsJackpotOnLine == 1;
                }
                catch (Exception ex)
                {
                    m_IsJackpotOnLine = 0;
                    return m_IsJackpotOnLine == 1;
                }
            }
            set
            {
                if (sboxConfData == null)
                {
                    return;
                }
                //tableSysSetting.is_jackpot_online = value;
                sboxConfData.NetJackpot = value ? 1 : 0;
                observable.SetProperty(ref m_IsJackpotOnLine, value ? 1 : 0);
            }
        }



        /// <summary>
        /// 联网彩金分值比
        /// </summary>
        /// <remarks>
        /// 只能修改投币值，来改变该值<br/>
        /// 1 除以 币值 乘以 1000 整形 <br/>
        /// </remarks>
        public int jackpotScoreRate
        {
            get => 1 * 1000 / coinInScale;
        }



        /// <summary>
        /// 联网彩金比(千分)
        /// </summary>
        /// <remarks>
        /// 1 - 100 可调
        /// </remarks>
        const string PARAM_JACKPOT_PERCENT = "PARAM_JACKPOT_PERCENT";
        int? m_jackpotPercent;
        public int jackpotPercent
        {
            get
            {
                if (m_jackpotPercent == null)
                    m_jackpotPercent = SQLitePlayerPrefs03.Instance.GetInt(PARAM_JACKPOT_PERCENT, 5);
                return (int)m_jackpotPercent;
            }
            set
            {
                m_jackpotPercent = value;
                SQLitePlayerPrefs03.Instance.SetInt(PARAM_JACKPOT_PERCENT, value);
            }
        }







        /// <summary>
        /// 游戏彩金比(千分)
        /// </summary>
        public float jackpotGamePercent
        {
            get
            {
                if (m_jackpotGamePercent == null)
                    m_jackpotGamePercent = SQLitePlayerPrefs03.Instance.GetFloat(PARAM_JACKPOT_GAME_PERCENT, 0.04f);
                return (float)m_jackpotGamePercent;
            }
            set
            {
                m_jackpotGamePercent = value;
                SQLitePlayerPrefs03.Instance.DeleteKey(PARAM_JACKPOT_GAME_PERCENT);
                SQLitePlayerPrefs03.Instance.SetFloat(PARAM_JACKPOT_GAME_PERCENT, value);
            }
        }
        const string PARAM_JACKPOT_GAME_PERCENT = "PARAM_JACKPOT_GAME_PERCENT";
        float? m_jackpotGamePercent;

        #endregion

        #region 玩家数据

        /// <summary> 玩家分数 </summary>
        public long myCredit  //   public long myCredit
        {
            get => (long)sboxPlayerInfo.Credit;
            set => sboxPlayerInfo.Credit = (int)value;
        }


        public string passwordAdmin;

        public string passwordManager;

        public string passwordShift;
        #endregion


        public string language
        {
            get => tableSysSetting.language_number;
            set => tableSysSetting.language_number = value;
        }


        #region 声音模块


        /// <summary> 音效 </summary>
        public float sound
        {
            get => tableSysSetting.sound;
            set => tableSysSetting.sound = value;
        }

        /// <summary> 背景音乐 </summary>
        public float music
        {
            get => tableSysSetting.music;
            set => tableSysSetting.music = value;
        }

        /// <summary> 声音快捷设置 </summary>
        public int soundLevel
        {
            get
            {
                if (music <= 0)
                    return 0;
                else if (music <= 0.35f)
                    return 1;
                else if (music <= 0.65f)
                    return 2;
                else
                    return 3;
            }
            set
            {
                if (value <= 0)
                {
                    music = 0f;
                    sound = 0f;
                }
                else if (value == 1)
                {
                    music = 0.35f;
                    sound = 0.35f;
                }
                else if (value == 2)
                {
                    music = 0.65f;
                    sound = 0.65f;
                }
                else if (value >= 3)
                {
                    music = 1f;
                    sound = 1f;
                }
            }
        }

        /// <summary> 声音使能 </summary>
        public bool soundEnable
        {
            get => tableSysSetting.sound_enable == 1;
            set => tableSysSetting.sound_enable = value ? 1 : 0;
        }



        public bool isDemoVoice
        {
            get => tableSysSetting.is_demo_voice == 1;
            set => tableSysSetting.is_demo_voice = value ? 1 : 0;
        }

        #endregion





        #region 数据记录上下限

        //public readonly int maxMaxGameRecord = 50000;
        //public readonly int minMaxGameRecord = 100;

        /// <summary> 最大游戏记录局数 </summary>
        public long gameRecordMax
        {
            get => tableSysSetting.max_game_record;
            set => tableSysSetting.max_game_record = value;
        }


        //public readonly int maxMaxCoinInOutRecord = 50000;
        //public readonly int minMaxCoinInOutRecord = 100;

        /// <summary> 最投退币记录次数 </summary>
        public long coinInOutRecordMax
        {
            get => tableSysSetting.max_coin_in_out_record;
            set => tableSysSetting.max_coin_in_out_record = value;
        }


        /// <summary> 最大报警信息记录局数 </summary>
        public long errorRecordMax
        {
            get => tableSysSetting.max_error_record;
            set => tableSysSetting.max_error_record = value;
        }


        /// <summary> 最大事件信息记录局数 </summary>
        public long eventRecordMax
        {
            get => tableSysSetting.max_event_record;
            set => tableSysSetting.max_event_record = value;
        }


        /// <summary> 最大彩金记录局数 </summary>
        public long jackpotRecordMax
        {
            get => tableSysSetting.max_jackpot_record;
            set => tableSysSetting.max_jackpot_record = value;
        }

        /// <summary> 最大彩金记录局数 </summary>
        public long businiessDayRecordMax
        {
            get => tableSysSetting.max_businiess_day_record;
            set => tableSysSetting.max_businiess_day_record = value;
        }




        #endregion

        #region 投退币倍率设置



        /// <summary> 上下分比例，1次等于几分（注意：上下分比例使用，投币比例） </summary>
        public int scoreUpDownScale
        {
            get => sboxConfData.ScoreUpUnit;
            set => sboxConfData.ScoreUpUnit = value;
            //get => sboxConfData.CoinValue;//
            //set => sboxConfData.CoinValue = value;//sboxConfData.ScoreUpUnit = value;
            //  get => (int)tableSysSetting.credit_up_down_scale;
            //  set => tableSysSetting.credit_up_down_scale = value;
        }

        /// <summary>
        /// 长按时短按的10倍
        /// </summary>
        public int scoreUpScaleLongClick
        {
            get => sboxConfData.ScoreUpUnit * 10;
            set { }
            //get => (int)tableSysSetting.credit_up_long_click_scale;
            //set => tableSysSetting.credit_up_long_click_scale = value;
        }


        /// <summary> 1币几分 </summary>
        public int coinInScale
        {
            get => sboxConfData.CoinValue;
            set => sboxConfData.CoinValue = value;
        }

        /// <summary>  1票几分(最小值为1) </summary>
        public int coinOutScaleCreditPerTicket
        {
            get => sboxConfData.TicketValue;
            set => sboxConfData.TicketValue = value;
        }

        /// <summary>  1分几票(最小值为1) </summary>
        public int coinOutScaleTicketPerCredit
        {
            get => sboxConfData.scoreTicket;
            set => sboxConfData.scoreTicket = value;
        }

        /*
        /// <summary> 1钞几分 </summary>
        public int billInScale
        {
            get => (int)tableSysSetting.bills_in_scale;
            set => tableSysSetting.bills_in_scale = value;
        }
        
        /// <summary>  打印1钞，对应几分 </summary>
        public int printOutScale
        {
            get => (int)tableSysSetting.printer_out_scale;
            set => tableSysSetting.printer_out_scale = value;
        }    
         
        */

        /// <summary> 1钞几分 </summary>
        public int billInScale
        {
            get => coinInScale;
            set => coinInScale = value;
        }



        public int printOutScale = 1;  //待完成

        #endregion


        #region 设备列表


        public List<_DeviceInfo> suppoetBillers
        {
            /*get
            {
                List<_DeviceInfo> deviceInfo = new List<_DeviceInfo>();

                foreach (TableDevicesItem item in this.tableDevices)
                {
                    if (item.device_type != "biller")
                        continue;

                    TableSupportDevicesItem target = null;
                    for (int i = 0; i < tableSupportDevices.Count; i++)
                    {
                        if (item.table_support_devices_id == tableSupportDevices[i].id)
                        {
                            target = tableSupportDevices[i];
                            break;
                        }
                    }

                    deviceInfo.Add(new _DeviceInfo()
                    {
                        number = int.Parse(item.device_number) ,
                        model = target.device_model ?? "--",
                        manufacturer = target.manufacturer ?? "--",
                    });
                }
                return deviceInfo;
            }*/
            get
            {
                List<_DeviceInfo> deviceInfo = new List<_DeviceInfo>();
                for (int i = 0; i < sboxBillerList.Count; i++)
                {
                    string[] str = sboxBillerList[i].Split(':');
                    deviceInfo.Add(new _DeviceInfo()
                    {
                        number = i,
                        model = str[1] ?? "--",
                        manufacturer = str[0] ?? "--",
                    });
                }
                return deviceInfo;
            }
        }


        public List<_DeviceInfo> suppoetPrinters
        {
            /*get
            {
                List<_DeviceInfo> deviceInfo = new List<_DeviceInfo>();

                foreach (TableDevicesItem item in this.tableDevices)
                {
                    if (item.device_type != "printer")
                        continue;

                    TableSupportDevicesItem target = null;
                    for (int i = 0; i < tableSupportDevices.Count; i++)
                    {
                        if (item.table_support_devices_id == tableSupportDevices[i].id)
                        {
                            target = tableSupportDevices[i];
                            break;
                        }
                    }

                    deviceInfo.Add(new _DeviceInfo()
                    {
                        number = int.Parse(item.device_number) ,
                        model = target.device_model ?? "--",
                        manufacturer = target.manufacturer ?? "--",
                    });
                }
                return deviceInfo;
            }*/

            get
            {
                List<_DeviceInfo> deviceInfo = new List<_DeviceInfo>();
                for (int i = 0; i < sboxPrinterList.Count; i++)
                {
                    string[] str = sboxPrinterList[i].Split(':');
                    deviceInfo.Add(new _DeviceInfo()
                    {
                        number = i,
                        model = str[1] ?? "--",
                        manufacturer = str[0] ?? "--",
                    });
                }
                return deviceInfo;
            }
        }

        /// <summary> 已选纸钞机型号 </summary>
        public int selectBillerNumber
        {
            get => tableSysSetting.select_biller_number;
            set => tableSysSetting.select_biller_number = value;
        }

        /// <summary> 已选打印机型号 </summary>
        public int selectPrinterNumber
        {
            get => tableSysSetting.select_printer_number;
            set => tableSysSetting.select_printer_number = value;
        }



        /*
        /// 选择打印机
        public int indexSelectPrinter
        {
            get
            {
                if (_indexSelectPrinter == null)
                    _indexSelectPrinter = SQLitePlayerPrefs02.Instance.GetInt(PARAM_INDEX_SELECT_PRINTER, 0);
                return (int)_indexSelectPrinter;
            }
            set
            {
                _indexSelectPrinter = value;
                SQLitePlayerPrefs02.Instance.SetInt(PARAM_INDEX_SELECT_PRINTER, value);
            }
        }
        const string PARAM_INDEX_SELECT_PRINTER = "PARAM_INDEX_SELECT_PRINTER";
        int? _indexSelectPrinter;*/


        /// <summary> 打印机是否复位 </summary>
        public bool isInitPrinter = false;

        /// <summary> 打印机是否复位 </summary>
        public bool isInitBiller = false;

        /// <summary> 打印机列表 </summary>
        public List<string> sboxPrinterList = new List<string>();

        /// <summary> 纸钞机列表 </summary>
        public List<string> sboxBillerList = new List<string>();

        public string selectBillerModel
        {
            /*get
            {
                TableDevicesItem target = null;
                for (int i = 0; i < this.tableDevices.Count; i++)
                {
                    if (tableDevices[i].device_number == selectBillerNumber.ToString()
                        && tableDevices[i].device_type == "biller")
                    {
                        target = tableDevices[i];
                        break;
                    }
                }
                if (target != null)
                    for (int i = 0; i < tableSupportDevices.Count; i++)
                    {
                        if (target.table_support_devices_id == tableSupportDevices[i].id)
                        {
                            return tableSupportDevices[i].device_model;
                        }
                    }
                return "--";
            }*/
            get
            {
                if (sboxBillerList.Count > 0 && sboxBillerList.Count > selectBillerNumber)
                {
                    return sboxBillerList[selectBillerNumber].Split(':')[1];
                }
                return "--";
            }
        }
        public string selectPrinterModel
        {
            /*
            get
            {
                TableDevicesItem target = null;
                for (int i = 0; i < this.tableDevices.Count; i++)
                {
                    if (tableDevices[i].device_number == selectPrinterNumber.ToString()
                        && tableDevices[i].device_type == "printer")
                    {
                        target = tableDevices[i];
                        break;
                    }
                }
                if (target != null)
                    for (int i = 0; i < tableSupportDevices.Count; i++)
                    {
                        if (target.table_support_devices_id == tableSupportDevices[i].id)
                        {
                            return tableSupportDevices[i].device_model;
                        }
                    }
                return "--";
            }*/

            get
            {
                if (sboxPrinterList.Count > 0 && sboxPrinterList.Count > selectPrinterNumber)
                {
                    return sboxPrinterList[selectPrinterNumber].Split(':')[1];
                }
                return "--";
            }
        }




        /// <summary> 是否使用打印机 </summary>
        public bool isUsePrinter
        {
            get
            {
                if (_isUsePrinter == null)
                    _isUsePrinter = 1 == SQLitePlayerPrefs03.Instance.GetInt(PARAM_IS_USE_PRINTER, 0);
                return (bool)_isUsePrinter;
            }
            set
            {
                _isUsePrinter = value;
                SQLitePlayerPrefs03.Instance.SetInt(PARAM_IS_USE_PRINTER, value ? 1 : 0);
            }
        }
        const string PARAM_IS_USE_PRINTER = "PARAM_IS_USE_PRINTER";
        bool? _isUsePrinter;


        /// <summary> 是否链接打印机 </summary>
        public bool isConnectPrinter
        {
            get
            {
                if (!isUsePrinter)
                    return false;

                // 先强制能用，后期删除
                if (DeviceUtils.IsCurSasPrinter())
                {
                    return true;
                }

                return m_IsConnectPrinter;
            }
            set
            {
                //m_IsConnectPrinter = value;
                observable.SetProperty(ref m_IsConnectPrinter, value);
            }
        }
        private bool m_IsConnectPrinter = false;






        /// <summary> 是否使用纸钞机 </summary>
        public bool isUseBiller
        {
            get
            {
                if (_isUseBiller == null)
                    _isUseBiller = 1 == SQLitePlayerPrefs03.Instance.GetInt(PARAM_IS_USE_BILLER, 0);
                return (bool)_isUseBiller;
            }
            set
            {
                _isUseBiller = value;
                SQLitePlayerPrefs03.Instance.SetInt(PARAM_IS_USE_BILLER, value ? 1 : 0);
            }
        }
        const string PARAM_IS_USE_BILLER = "PARAM_IS_USE_BILLER";
        bool? _isUseBiller;



        /// <summary> 是否链接纸钞机 </summary>
        public bool isConnectBiller
        {
            get
            {
                if (!isUseBiller)
                    return false;
                return m_IsConnectBiller;
            }
            set
            {
                //m_IsConnectBiller = value;

                observable.SetProperty(ref m_IsConnectBiller, value);
            }
        }
        private bool m_IsConnectBiller = false;
        #endregion




        /// <summary>
        /// 临时缓存，避免重复检查tableMachine数据变化
        /// </summary>
        private bool m_IsMachineActive = false;
        /*public bool isMachineActive
        {
            get => tableMachine.manufacture_state != 1;
            set => tableMachine.manufacture_state = value?0:1;
        }*/
        /// <summary> 激活状态 </summary>
        public bool isMachineActive = true;


        /// <summary> 软件第一次运行或安装 </summary>
        public bool isFirstInstall
        {
            get
            {
                if (m_IsFirstInstall == null)
                {
                    const string FIRST_INSTALL_EDITOR = "FIRST_INSTALL_EDITOR";
                    const string FIRST_INSTALL = "FIRST_INSTALL";

                    m_IsFirstInstall = false;
                    string key = Application.isEditor ? FIRST_INSTALL_EDITOR : FIRST_INSTALL;
                    int dat = PlayerPrefs.GetInt(key, 1);
                    m_IsFirstInstall = dat == 1;
                    if ((bool)m_IsFirstInstall)
                    {
                        PlayerPrefs.SetInt(key, 0);
                        PlayerPrefs.Save();
                    }
                }
                return (bool)m_IsFirstInstall;
            }
        }
        bool? m_IsFirstInstall = null;


        /// <summary> 是否清零打码 </summary>
        public bool isResetCode => historyTotalBet == 0 && historyTotalWin == 0
            && historyTotalScoreUpCredit == 0 && historyTotalScoreDownCredit == 0
            && historyTotalCoinInCredit == 0 && historyTotalCoinOutCredit == 0;


        #region 调试参数
        /// <summary> 是否显示调试信息 </summary>
        public bool isDebug
        {
            get
            {
                m_IsDebug = tableSysSetting.is_debug == 1;
                return m_IsDebug;
            }
            set
            {
                if (m_IsDebug != value)
                    tableSysSetting.is_debug = value ? 1 : 0;
                observable.SetProperty(ref m_IsDebug, value);
            }
        }
        bool m_IsDebug = true;


        /// <summary> 是否显示跟新的内容 </summary>
        public bool isUpdateInfo
        {
            get => tableSysSetting.is_update_info == 1;
            set => tableSysSetting.is_update_info = value ? 1 : 0;
        }

        /// <summary> 是否显示小时日志界面 </summary>
        public bool enableReporterPage
        {
            get => tableSysSetting.enable_reporter_page == 1;
            set => tableSysSetting.enable_reporter_page = value ? 1 : 0;
        }

        /// <summary> 是否显示测试工具 </summary>
        public bool enableTestTool
        {
            get => tableSysSetting.enable_test_tool == 1;
            set => tableSysSetting.enable_test_tool = value ? 1 : 0;
        }


        #endregion


        /// <summary> 当前权限  1：普通密码权限，2：管理员密码权限，3：超级管理员密码权限</summary>
        public int curPermissions = -1;

        /// <summary> 当前是超级管理员身份？ </summary>
        public bool isCurAdministrator => curPermissions == 3;

        /// <summary>
        /// 是否使用iot
        /// </summary>
        public bool isUseIot
        {
            get
            {
                if (_isUseIot == null)
                    _isUseIot = 1 == SQLitePlayerPrefs03.Instance.GetInt(PARAM_IS_USE_IOT, 0);
                return (bool)_isUseIot;
            }
            set
            {
                _isUseIot = value;
                SQLitePlayerPrefs03.Instance.SetInt(PARAM_IS_USE_IOT, value ? 1 : 0);
            }
        }
        const string PARAM_IS_USE_IOT = "PARAM_IS_USE_IOT";
        bool? _isUseIot;


        /// <summary> 好酷是否链接 </summary>
        public bool isConnectIot
        {

            get
            {
                if (_isUseIot == false)
                {
                    return false;
                }
                return _isConnectIot;
            }
            set
            {
                observable.SetProperty(ref _isConnectIot, value);
            }
        }
        bool _isConnectIot = true;




        public int iotAccessMethods
        {
            get
            {
                if (_iotAccessMethods == null)
                    _iotAccessMethods = SQLitePlayerPrefs03.Instance.GetInt(PARAM_IOT_ACCESS_METHODS, 1);
                return (int)_iotAccessMethods;
            }
            set
            {
                _iotAccessMethods = value;
                SQLitePlayerPrefs03.Instance.SetInt(PARAM_IOT_ACCESS_METHODS, value);
            }
        }
        const string PARAM_IOT_ACCESS_METHODS = "PARAM_IOT_ACCESS_METHODS";
        int? _iotAccessMethods;

        public Dictionary<int, string> iotAccessMethodsLst = new Dictionary<int, string>()
        {
            [1] = "WeChat Official Account",
            [2] = "Mini Program",
            [3] = "Battle Platform",
        };

        public string selectIOTAccessMethod => iotAccessMethodsLst[iotAccessMethods];


        public bool isUseRemoteControl
        {
            get
            {
                if (_isUseRemoteControl == null)
                    _isUseRemoteControl = 1 == SQLitePlayerPrefs03.Instance.GetInt(PARAM_IS_USE_REMOTE_CONTROL, 0);
                return (bool)_isUseRemoteControl;
            }
            set
            {
                _isUseRemoteControl = value;
                SQLitePlayerPrefs03.Instance.SetInt(PARAM_IS_USE_REMOTE_CONTROL, value ? 1 : 0);
            }
        }
        const string PARAM_IS_USE_REMOTE_CONTROL = "PARAM_IS_USE_REMOTE_CONTROL";
        bool? _isUseRemoteControl;

        public string remoteControlSetting
        {
            get
            {
                if (string.IsNullOrEmpty(_remoteControlSetting))
                    _remoteControlSetting = SQLitePlayerPrefs03.Instance.GetString(PARAM_REMOTE_CONTROL_SETTING, "192.168.3.174:1883");
                return _remoteControlSetting;
            }
            set
            {
                _remoteControlSetting = value;
                SQLitePlayerPrefs03.Instance.SetString(PARAM_REMOTE_CONTROL_SETTING, value);
            }
        }
        const string PARAM_REMOTE_CONTROL_SETTING = "PARAM_REMOTE_CONTROL_SETTING";
        string _remoteControlSetting = null;


        public string remoteControlAccount
        {
            get
            {
                if (string.IsNullOrEmpty(_remoteControlAccount))
                    _remoteControlAccount = SQLitePlayerPrefs03.Instance.GetString(PARAM_REMOTE_CONTROL_ACCOUNT, "tester01");
                return _remoteControlAccount;
            }
            set
            {
                _remoteControlAccount = value;
                SQLitePlayerPrefs03.Instance.SetString(PARAM_REMOTE_CONTROL_ACCOUNT, value);
            }
        }
        const string PARAM_REMOTE_CONTROL_ACCOUNT = "PARAM_REMOTE_CONTROL_ACCOUNT";
        string _remoteControlAccount = null;


        public string remoteControlPassword
        {
            get
            {
                if (string.IsNullOrEmpty(_remoteControlPassword))
                    _remoteControlPassword = SQLitePlayerPrefs03.Instance.GetString(PARAM_REMOTE_CONTROL_PASSWORD, "123456");
                return _remoteControlPassword;
            }
            set
            {
                _remoteControlPassword = value;
                SQLitePlayerPrefs03.Instance.SetString(PARAM_REMOTE_CONTROL_PASSWORD, value);
            }
        }
        const string PARAM_REMOTE_CONTROL_PASSWORD = "PARAM_REMOTE_CONTROL_PASSWORD";
        string _remoteControlPassword = null;



        public bool isConnectRemoteControl
        {

            get
            {
                if (_isUseRemoteControl == false)
                {
                    return false;
                }
                return _isConnectRemoteControl;
            }
            set
            {
                observable.SetProperty(ref _isConnectRemoteControl, value);
            }
        }
        bool _isConnectRemoteControl = true;







        /// <summary>
        /// 是否使用即中即退
        /// </summary>
        public bool isCoinOutImmediately
        {
            get
            {
                if (_isCoinOutImmediately == null)
                    _isCoinOutImmediately = 1 == SQLitePlayerPrefs03.Instance.GetInt(PARAM_IS_COIN_OUT_IMMEDIATELY, 0);
                return (bool)_isCoinOutImmediately;
            }
            set
            {
                _isCoinOutImmediately = value;
                SQLitePlayerPrefs03.Instance.SetInt(PARAM_IS_COIN_OUT_IMMEDIATELY, value ? 1 : 0);
            }
        }
        const string PARAM_IS_COIN_OUT_IMMEDIATELY = "PARAM_IS_COIN_OUT_IMMEDIATELY";
        bool? _isCoinOutImmediately;





        /// <summary> 是否链接钱箱 </summary>
        public bool isConnectMoneyBox
        {
            get
            {
                if (ApplicationSettings.Instance.isRelease)  //正式版先不放出去
                    return false;

                if (m_isConnectMoneyBox == null)
                    m_isConnectMoneyBox = 1 == SQLitePlayerPrefs03.Instance.GetInt(PARAM_IS_CONNECT_MONEY_BOX, 0);

                return (bool)m_isConnectMoneyBox;
            }
            set
            {

                if (ApplicationSettings.Instance.isRelease)  //正式版先不放出去
                    return;

                int val = value ? 1 : 0;
                SQLitePlayerPrefs03.Instance.SetInt(PARAM_IS_CONNECT_MONEY_BOX, val);

                observable.SetProperty(ref m_isConnectMoneyBox, value);
            }
        }
        const string PARAM_IS_CONNECT_MONEY_BOX = "PARAM_IS_CONNECT_MONEY_BOX";
        bool? m_isConnectMoneyBox;





        /// <summary>
        /// sas链接配置
        /// </summary>
        public string sasConnection
        {
            get
            {
                if (string.IsNullOrEmpty(_sasConnection))
                    _sasConnection = SQLitePlayerPrefs03.Instance.GetString(PARAM_SAS_CONNECTION, "192.168.3.234:6379");
                return _sasConnection;
            }
            set
            {
                _sasConnection = value;
                SQLitePlayerPrefs03.Instance.SetString(PARAM_SAS_CONNECTION, value);
            }
        }
        const string PARAM_SAS_CONNECTION = "PARAM_SAS_CONNECTION";
        string _sasConnection = null;

        //   RedisMgr.Instance.Init("192.168.3.234",6379, "sas@2024", 0, 50, "", (eventId,data) =>



        /*
        /// <summary>
        /// sas账号
        /// </summary>
        public string sasAccount
        {
            get
            {
                if (string.IsNullOrEmpty(_sasAccount))
                    _sasAccount = SQLitePlayerPrefs03.Instance.GetString(PARAM_SAS_ACCOUNT, "192.168.3.174:1883");
                return _sasAccount;
            }
            set
            {
                _sasAccount = value;
                SQLitePlayerPrefs03.Instance.SetString(PARAM_SAS_ACCOUNT, value);
            }
        }
        const string PARAM_SAS_ACCOUNT = "PARAM_SAS_ACCOUNT";
        string _sasAccount = null;
        */


        /// <summary>
        /// sas账号
        /// </summary>
        /// 
        public string sasAccount
        {
            get
            {
                if (string.IsNullOrEmpty(_sasAccount))
                    _sasAccount = SQLitePlayerPrefs03.Instance.GetString(PARAM_SAS_ACCOUNT, "sastester001");
                return _sasAccount;
            }
            set
            {
                _sasAccount = value;
                SQLitePlayerPrefs03.Instance.SetString(PARAM_SAS_ACCOUNT, value);
            }
        }
        const string PARAM_SAS_ACCOUNT = "PARAM_SAS_ACCOUNT";
        string _sasAccount = null;

        public string sasPassword
        {
            get
            {
                if (string.IsNullOrEmpty(_sasPassword))
                    _sasPassword = SQLitePlayerPrefs03.Instance.GetString(PARAM_SAS_PASSWORD, "sas@2024");
                return _sasPassword;
            }
            set
            {
                _sasPassword = value;
                SQLitePlayerPrefs03.Instance.SetString(PARAM_SAS_PASSWORD, value);
            }
        }
        const string PARAM_SAS_PASSWORD = "PARAM_SAS_PASSWORD";
        string _sasPassword = null;



        /// <summary>Sas 1钞几分 </summary>
        public int sasInOutScale
        {
            get
            {
                if (_sasInOutScale == null)
                    _sasInOutScale = SQLitePlayerPrefs03.Instance.GetInt(PARAM_SAS_IN_OUT_SCALE, 1);
                return (int)_sasInOutScale;
            }
            set
            {
                _sasInOutScale = value;
                SQLitePlayerPrefs03.Instance.SetInt(PARAM_SAS_IN_OUT_SCALE, value);
            }
        }
        const string PARAM_SAS_IN_OUT_SCALE = "PARAM_SAS_IN_OUT_SCALE";
        int? _sasInOutScale;


        /// <summary>
        /// 是否链接sas
        /// </summary>
        public bool isSasConnect
        {
            get
            {
                if (PlayerPrefsUtils.isUseSas == false)
                {
                    return false;
                }
                return _isSasConnect;
            }
            set
            {
                observable.SetProperty(ref _isSasConnect, value);
            }
        }

        bool _isSasConnect = false;



        #region 奖励结果上报

        /// <summary> 额外奖上报地址 </summary>
        public string bnousReportUrl => BonusReporter.Instance.url;


        public bool isUseBonusReport
        {
            get
            {
                if (_isUseBonusReport == null)
                    _isUseBonusReport = 1 == SQLitePlayerPrefs03.Instance.GetInt(PARAM_IS_USE_BONUS_REPORT, 0);
                return (bool)_isUseBonusReport;
            }
            set
            {
                _isUseBonusReport = value;
                SQLitePlayerPrefs03.Instance.SetInt(PARAM_IS_USE_BONUS_REPORT, value ? 1 : 0);
            }
        }
        const string PARAM_IS_USE_BONUS_REPORT = "PARAM_IS_USE_BONUS_REPORT";
        bool? _isUseBonusReport;

        public string bonusReportSetting
        {
            get
            {
                if (string.IsNullOrEmpty(_bonusReportSetting))
                    _bonusReportSetting = SQLitePlayerPrefs03.Instance.GetString(PARAM_BONUS_REPORT_SETTING, "shiruan.zs-sr.cn:9091");
                return _bonusReportSetting;
            }
            set
            {
                _bonusReportSetting = value;
                SQLitePlayerPrefs03.Instance.SetString(PARAM_BONUS_REPORT_SETTING, value);
            }
        }
        const string PARAM_BONUS_REPORT_SETTING = "PARAM_BONUS_REPORT_SETTING";
        string _bonusReportSetting = null;

        #endregion







   /*
        /// <summary> 游戏难度 最小值 </summary>
        public readonly List<string> difficultyNames =  
            new List<string>() {
             "1050", // 1.050,
       
             "1000", // 1.050,

             "995",// 0.995,

             "990",// 0.990,

             "985",// 0.985,

             "980",// 0.980,

             "975",// 0.975,

             "970",// 0.970,

             "965",// 0.965

             "960",// 0.960,
             };

     
        /// <summary>  游戏难度 </summary>
        public int difficulty
        {
            get => sboxConfData.difficulty;
            set => sboxConfData.difficulty = value;
        }
        */

        /// <summary> 游戏难度等级可以设置 </summary>
        public int dllLevelIndex
        {
            get
            {
                if (_level == null)
                {
                    _level = SQLitePlayerPrefs03.Instance.GetInt(PARAM_GMAE_LEVEL_INDEX, 0);
                }
                return (int)_level;
            }
            set
            {
                _level = value;
                SQLitePlayerPrefs03.Instance.SetInt(PARAM_GMAE_LEVEL_INDEX, value);
            }
        }
        const string PARAM_GMAE_LEVEL_INDEX = "PARAM_GMAE_LEVEL_INDEX";
        int? _level = null;



        /// <summary>  支持的语言  </summary>
        public TableSupportLanguageItem[] tableSupportLanguage => TableSupportLanguageItem.DefaultTable();

    }
}

/*
1. 上分单按： 1倍
2. 上分长按： 10倍
3. 下分单按： 1倍
4. 下分长按： 清理
 */

