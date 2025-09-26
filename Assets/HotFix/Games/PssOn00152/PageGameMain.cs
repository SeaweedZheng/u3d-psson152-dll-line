using System.Collections.Generic;
using UnityEngine;
using GameMaker;
using SlotMaker;
using UnityEngine.UIElements;
using System;
using Sirenix.OdinInspector;
using Game;
using SimpleJSON;
using System.Collections;
using UnityEngine.UI;
using Newtonsoft.Json;
using SlotDllAlgorithmG152;

using _contentBB = PssOn00152.ContentBlackboard;
using _reelSetBB = SlotMaker.ReelSettingBlackboard;
using _consoleBB = PssOn00152.ConsoleBlackboard02;
using _mainBB = GameMaker.MainBlackboard;
using _spinWEBB = SlotMaker.SpinWinEffectSettingBlackboard;
using SBoxApi;







//ChinaTown100
namespace PssOn00152
{
    public enum ReelsTurnType
    {
        Normal,
        Once,
    }

    public partial class PageGameMain : PageCorBase  //BasePageSingleton<GamePage>
    {

        public MiniReelJackpotUI uiJPGrandCtrl, uiJPMajorCtrl, uiJPMinorCtrl, uiJPMiniCtrl;

        public SlotMachineController slotMachineCtrl;

        public GameObject goSlotCover, goEffect5Kind, goExpectation, goBgRegular, goBgFreeSpin, goAnchorMain, goPanelFreeSpin;

        MessageDelegates onPropertyChangedEventDelegates;
        MessageDelegates onPanelInputEventDelegates;
        MessageDelegates onSlotDetailEventDelegates;
        MessageDelegates onSlotEventDelegates;



        /// <summary>  start game </summary>
        const string COR_GAME_ONCE = "COR_GAME_ONCE";
        const string COR_GAME_AUTO = "COR_GAME_AUTO";
        const string COR_GAME_IDLE = "COR_GAME_IDLE";
        const string COR_EFFECT_SLOW_MOTION = "COR_EFFECT_SLOW_MOTION";
        /// <summary> 滚轮转动 </summary>
        const string COR_REELS_TURN = "COR_REELS_TURN";



        // int gameNumberFreeSpinTrigger = 0;



        const float timeSingleWin = 0.4f;
        const float timeTotalWin = 0.6f;

        //GameSenceData gameSenceData;

        //TableSlotGameRecordItem slotGameRecordItem;

        /// <summary> 大厅彩金 </summary>
        int? seqidJpHall;
        int? seqidNet;

        bool tipCoinIn = false;

        /// <summary> 压注后的金额 </summary>
        //long creditAfterBet = 0;


        protected void Awake()
        {

            onPropertyChangedEventDelegates = new MessageDelegates
             (
                 new Dictionary<string, EventDelegate>
                 {
                    { "./grandJP", OnPropertyChangeGrandJP},
                    { "./majorJP", OnPropertyChangeMajorJP},
                    { "./minorJP", OnPropertyChangeMinorJP},
                    { "./miniJP",  OnPropertyChangeMiniJP},

                     //{ "ContentBlackboard/totalBet", OnPropertyChangeTotalBet },
                     //{ "ContentBlackboard/betList",  OnPropertyChangeBetList },
                     //{ "MainBlackboard/soundVolume",  OnPropertyChangeBetList }
                    { "./totalBet", OnPropertyChangeTotalBet },
                    {"./btnSpinState",OnPropertyChangeBtnSpinState}
                 }
             );

            onPanelInputEventDelegates = new MessageDelegates
             (
                 new Dictionary<string, EventDelegate>
                 {
                     //{ PanelEvent.OnLongClickSpinButton, OnClickSpinButton},
                     //{ PanelEvent.OnClickSpinButton, OnClickSpinButton},
                     { PanelEvent.SpinButtonClick, OnClickSpinButton},
                     { PanelEvent.RedeemButtonClick,OnClickRedeemButton}
                 }
             );

            onSlotDetailEventDelegates = new MessageDelegates
             (
                 new Dictionary<string, EventDelegate>
                 {
                     {SlotMachineEvent.PrepareStoppedReel, OnEventPrepareStoppedReel},
                 }
             );

            onSlotEventDelegates = new MessageDelegates
             (
                 new Dictionary<string, EventDelegate>
                 {
                     {SlotMachineEvent.StoppedSlotMachine, OnEventStoppedSlotMachine},
                 }
             );
        }


        protected void Start()
        {
            goSlotCover?.SetActive(false);

            // 初始化外设算法卡
            GameJackpotCreator.Instance.Init();
        }

        protected void OnEnable()
        {
            EventCenter.Instance.AddEventListener<EventData>(ObservableObject.ON_PROPERTY_CHANGED_EVENT, onPropertyChangedEventDelegates.Delegate);
            EventCenter.Instance.AddEventListener<EventData>(PanelEvent.ON_PANEL_INPUT_EVENT, onPanelInputEventDelegates.Delegate);
            EventCenter.Instance.AddEventListener<EventData>(SlotMachineEvent.ON_SLOT_DETAIL_EVENT, onSlotDetailEventDelegates.Delegate);
            EventCenter.Instance.AddEventListener<EventData>(SlotMachineEvent.ON_SLOT_EVENT, onSlotEventDelegates.Delegate);

            //EventCenter.Instance.AddEventListener<EventData>(SlotMachineEvent.ON_SLOT_EVENT, onSlotEventDelegates.Delegate);

            EventCenter.Instance.AddEventListener<string>(RPCName.jackpotHall, OnRpcJackpotHall);

            EventCenter.Instance.AddEventListener<EventData>(GlobalEvent.ON_DEVICE_EVENT, OnInitGameJackpot);

            EventCenter.Instance.AddEventListener<EventData>(GlobalEvent.ON_COIN_IN_OUT_EVENT, OnCoinInOutEvent);

            InitParam();
        }




        protected void OnDisable()
        {
            //base.OnDisable();

            if (seqidJpHall != null)
                JackpotOnLineManager.Instance.RemoveRequestAt((int)seqidJpHall);
            seqidJpHall = null;

            if (seqidNet != null)
                NetManager.Instance.RemoveRequestAt((int)seqidNet);
            seqidNet = null;

            if (seqidMachine != null)
                MachineDataManager.Instance.RemoveRequestAt(((int)seqidMachine));
            seqidMachine = null;

            EventCenter.Instance.RemoveEventListener<EventData>(ObservableObject.ON_PROPERTY_CHANGED_EVENT, onPropertyChangedEventDelegates.Delegate);
            EventCenter.Instance.RemoveEventListener<EventData>(PanelEvent.ON_PANEL_INPUT_EVENT, onPanelInputEventDelegates.Delegate);
            EventCenter.Instance.RemoveEventListener<EventData>(SlotMachineEvent.ON_SLOT_DETAIL_EVENT, onSlotDetailEventDelegates.Delegate);
            EventCenter.Instance.RemoveEventListener<EventData>(SlotMachineEvent.ON_SLOT_EVENT, onSlotEventDelegates.Delegate);
            EventCenter.Instance.RemoveEventListener<string>(RPCName.jackpotHall, OnRpcJackpotHall);

            EventCenter.Instance.RemoveEventListener<EventData>(GlobalEvent.ON_DEVICE_EVENT, OnInitGameJackpot);

            EventCenter.Instance.RemoveEventListener<EventData>(GlobalEvent.ON_COIN_IN_OUT_EVENT, OnCoinInOutEvent);
        }

        /*protected override void OnDestroy()
        {
            base.OnDestroy();
        }*/



        void OnPropertyChangeGrandJP(EventData receivedEvent = null) => OnPropertyChangeJcakpotUI(() => uiJPGrandCtrl, receivedEvent);
        void OnPropertyChangeMajorJP(EventData receivedEvent = null) => OnPropertyChangeJcakpotUI(() => uiJPMajorCtrl, receivedEvent);
        void OnPropertyChangeMinorJP(EventData receivedEvent = null) => OnPropertyChangeJcakpotUI(() => uiJPMinorCtrl, receivedEvent);
        void OnPropertyChangeMiniJP(EventData receivedEvent = null) => OnPropertyChangeJcakpotUI(() => uiJPMiniCtrl, receivedEvent);

        void OnPropertyChangeJcakpotUI(Func<MiniReelJackpotUI> func, EventData receivedEvent = null)
        {
            MiniReelJackpotUI comp = func();
            JackpotInfo jp = receivedEvent != null ? (JackpotInfo)receivedEvent.value : null;
            if (comp != null && jp != null)
            {
                if (comp.toCredit.ToString("N0") != jp.curCredit.ToString("N0"))
                {
                    comp.AddToData(jp.curCredit);
                }
            }
        }


        TextAsset _txtaGameTnfo;
        TextAsset txtaGameTnfo
        {
            get
            {
                if (_txtaGameTnfo == null)
                    _txtaGameTnfo = ResourceManager.Instance.LoadAssetAtPathOnce<TextAsset>(ConfigUtils.curGameInfoURL);
                return _txtaGameTnfo;
            }
        }

        void OnPropertyChangeTotalBet(EventData receivedEvent = null)
        {
            long totalBet = receivedEvent != null ? (long)receivedEvent.value : -1;
            if (totalBet == -1)
            {
                List<long> betList = _consoleBB.Instance.betList;
                totalBet = betList[_contentBB.Instance.betIndex];
            }


            // 通知算法卡押注金额已经改变
            MachineDataManager.Instance.RequestSetPlayerBets(_consoleBB.Instance.myCredit, totalBet, (res) =>
            {
                int result = (int)res;
                if (result != 0)
                    DebugUtils.LogError($"set total bet for machine is err :{result}");
            });

            // 通知彩金
            InitGameJackpot();


            //TextAsset txtaGameTnfo = ResourceManager.Instance.LoadAssetAtPathOnce<TextAsset>(HotfixSettings.Instance.GetGameInfo(ConfigUtils.curGameId));


            JSONNode nodePayTable = JSONNode.Parse(txtaGameTnfo.text);
            JSONNode node = nodePayTable["symbol_paytable"];
            List<PayTableSymbolInfo> payTable = new List<PayTableSymbolInfo>();
            long scale = totalBet / 50;
            foreach (KeyValuePair<string, JSONNode> kv in node)
            {
                long x5 = 0;
                long x4 = 0;
                long x3 = 0;

                if (kv.Value.HasKey("x3"))
                    x3 = long.Parse(kv.Value["x3"]) * scale;
                if (kv.Value.HasKey("x4"))
                    x4 = long.Parse(kv.Value["x4"]) * scale;
                if (kv.Value.HasKey("x5"))
                    x5 = long.Parse(kv.Value["x5"]) * scale;

                int symbolNumber = int.Parse(kv.Key.Replace("s", ""));
                payTable.Add(new PayTableSymbolInfo()
                {
                    symbol = symbolNumber,
                    x5 = x5,
                    x4 = x4,
                    x3 = x3,
                });
            };
            _contentBB.Instance.payTableSymbolWin = payTable;


            node = nodePayTable["symbol_paytable_multiple"];
            List<PayTableSymbolInfo> symbolWinMultiple = new List<PayTableSymbolInfo>();
            foreach (KeyValuePair<string, JSONNode> kv in node)
            {
                long x5 = 0;
                long x4 = 0;
                long x3 = 0;

                if (kv.Value.HasKey("x3"))
                    x3 = long.Parse(kv.Value["x3"]) * scale;
                if (kv.Value.HasKey("x4"))
                    x4 = long.Parse(kv.Value["x4"]) * scale;
                if (kv.Value.HasKey("x5"))
                    x5 = long.Parse(kv.Value["x5"]) * scale;

                int symbolNumber = int.Parse(kv.Key.Replace("s", ""));
                symbolWinMultiple.Add(new PayTableSymbolInfo()
                {
                    symbol = symbolNumber,
                    x5 = x5,
                    x4 = x4,
                    x3 = x3,
                });
            };
            _contentBB.Instance.payTableSymbolWinMultiple = symbolWinMultiple;




            //payTableSymbolWinMultiple

        }


        //改变机台按钮灯
        void OnPropertyChangeBtnSpinState(EventData res)
        {
            //string state = (string)res.value;

            if (!PageManager.Instance.IsTop(this))
                return;
            ChangeSpinBtnLight();
        }


        void ChangeSpinBtnLight()
        {
            List<MachineButtonKey> lst = new List<MachineButtonKey>();
            if (_contentBB.Instance.btnSpinState == SpinButtonState.Stop || _contentBB.Instance.btnSpinState == SpinButtonState.Auto)
            {
                lst.Add(MachineButtonKey.BtnSpin);
            }
            if (_contentBB.Instance.btnSpinState == SpinButtonState.Stop)
            {
                lst.Add(MachineButtonKey.BtnTicketOut);
            }
            machineButtonEventDispatcher.ChangeButtonShow(lst);

            /*
            switch (_contentBB.Instance.btnSpinState)
            {
                //case "Normal":
                case SpinButtonState.Spin:
                    machineButtonEventDispatcher.ChangeButtonShow(new List<MachineButtonKey>() { MachineButtonKey.BtnTicketOut });
                    break;
                case SpinButtonState.Auto:
                case SpinButtonState.Stop:
                    machineButtonEventDispatcher.ChangeButtonShow(new List<MachineButtonKey>() { MachineButtonKey.BtnTicketOut, MachineButtonKey.BtnSpin });
                    break;
            }*/
        }

        #region 机台按钮事件
        public override void OnTop()
        {
            ChangeSpinBtnLight();
            //游戏置顶时，检查下激活状态
            EventCenter.Instance.EventTrigger<EventData>(MachineUIEvent.ON_MACHINE_UI_EVENT, new EventData(MachineUIEvent.CheckCodeActive));

        }
        #endregion


        void OnClickSpinButton(EventData receivedEvent = null)
        {

            // string clickType = receivedEvent.value as string;

            bool isLongClick = (bool)receivedEvent.value;


            //string btnSpinState = receivedEvent.value as string;

            switch (_contentBB.Instance.btnSpinState)
            {
                //case "Normal":
                case SpinButtonState.Stop:
                    //if (_contentBB.Instance.isSpin"))  return; // 已经开始玩直接退出
                    if (_contentBB.Instance.isSpin) return; // 已经开始玩直接退出

                    _contentBB.Instance.isSpin = true; // _contentBB.Instance.isSpin", true);

                    Action successCallback = () =>
                    {
                        _contentBB.Instance.isSpin = false;
                        _contentBB.Instance.btnSpinState = SpinButtonState.Stop;
                        DebugUtils.Log("游戏结束");
                    };

                    if (isLongClick)
                    {
                        _contentBB.Instance.isAuto = true;
                        _contentBB.Instance.btnSpinState = SpinButtonState.Auto;
                        StartGameAuto(successCallback, StopGameWhenError);//开始玩
                    }
                    else
                    {
                        _contentBB.Instance.btnSpinState = SpinButtonState.Spin;
                        StartGameOnce(successCallback, StopGameWhenError);//开始玩
                    }


                    break;

                case SpinButtonState.Spin:

                    // 已经在游戏时，去停止游戏
                    if (!_contentBB.Instance.isSpin) return; // 已经停止直接退出


                    slotMachineCtrl.isStopImmediately = true;  // 去停止游戏  

                    //BlackboardUtils.Invoke("@customData/SelectData", new object[] { "CustomData Reels Regular Stop Immediately" });
                    //_reelSetBB.Instance.SelectData(REEL_SETTING_STOP);

                    SlotGameEffectManager.Instance.SetEffect(SlotGameEffect.StopImmediately);

                    /*
                    //停止游戏
                    // wait slot machine stop callback
                    if (true)  // 去停止游戏  
                    {
                        _contentBB.Instance.isSpin", false);
                        BlackboardUtils.SetValue("./btnSpinState", SpinButtonState.Stop); //btnSpin.SetState(SpinButtonState.Stop);
                    }*/
                    break;
                case SpinButtonState.Auto:
                    //停止自动玩
                    _contentBB.Instance.isSpin = true;
                    _contentBB.Instance.isAuto = false;
                    _contentBB.Instance.btnSpinState = SpinButtonState.Spin;
                    break;
            }
        }


        void OnClickRedeemButton(EventData receivedEvent = null)
        {
            if (_contentBB.Instance.isSpin)
            {
                TipPopupHandler.Instance.OpenPopupOnce(I18nMgr.T("<size=24>Cannot coin out during the game period</size>"));
                return;
            }
            PageManager.Instance.OpenPage(PageName.Console001PopuoConsoleMoneyBoxRedeem);
        }

        private void StopGameWhenError(string msg)
        {
            _contentBB.Instance.isSpin = false;
            _contentBB.Instance.isAuto = false;
            _contentBB.Instance.btnSpinState = SpinButtonState.Stop;
            _contentBB.Instance.gameState = GameState.Idle;



            // 有好酷优先用好酷
            if (_consoleBB.Instance.isUseIot && tipCoinIn)
            {
                tipCoinIn = false;

                if (!DeviceIOTPayment.Instance.isIOTConneted)
                {
                    TipPopupHandler.Instance.OpenPopupOnce(string.Format(I18nMgr.T("IOT connection failed [{0}]"), Code.DEVICE_IOT_MQTT_NOT_CONNECT));
                }
                else if (!DeviceIOTPayment.Instance.isIOTSignInGetQRCode)
                {
                    TipPopupHandler.Instance.OpenPopupOnce(string.Format(I18nMgr.T("IOT connection failed [{0}]"), Code.DEVICE_IOT_NOT_SIGN_IN));
                }
                else
                {
                    DeviceIOTPayment.Instance.DoQrCoinIn();
                }
                return;
            }
            else
            {
                string massage = I18nMgr.T(msg);
                TipPopupHandler.Instance.OpenPopupOnce(I18nMgr.T(msg));
            }

        }



        bool isEffectSlowMotion = false;
        bool isStoppedSlotMachine = false;

        void OnEventPrepareStoppedReel(EventData receivedEvent)
        {
            if (_contentBB.Instance.isReelsSlowMotion)
            {
                int colIndex = (int)receivedEvent.value;
                if (colIndex == 1)
                {
                    isEffectSlowMotion = true;
                }
            }
        }

        void OnEventStoppedSlotMachine(EventData receivedEvent)
        {
            isStoppedSlotMachine = true;
        }




        void InitParam()
        {
            _contentBB.Instance.totalBet = _consoleBB.Instance.betList[0];

            _contentBB.Instance.winMultipleList = new List<WinMultiple>();
            //TextAsset jsn = ResourceManager.Instance.LoadAssetAtPathOnce<TextAsset>(HotfixSettings.Instance.GetGameInfo(ConfigUtils.curGameId));
            JSONNode gameInfoNode = JSONNode.Parse(txtaGameTnfo.text);
            foreach (KeyValuePair<string, JSONNode> kv in gameInfoNode["win_level_multiple"])
            {
                WinMultiple item = new WinMultiple();
                item.winLevelType = WinLevelType.None;
                item.multiple = (long)kv.Value;
                switch (kv.Key)
                {
                    case "big":
                        item.winLevelType = WinLevelType.Big;
                        break;
                    case "mega":
                        item.winLevelType = WinLevelType.Mega;
                        break;
                    case "super":
                        item.winLevelType = WinLevelType.Super;
                        break;
                    case "ultra":
                        item.winLevelType = WinLevelType.Ultra;
                        break;
                    case "ultimate":
                        item.winLevelType = WinLevelType.Ultimate;
                        break;
                }
                _contentBB.Instance.winMultipleList.Add(item);
            }

            List<List<int>> payLines = new List<List<int>>();
            foreach (JSONNode line in gameInfoNode["pay_lines"])
            {
                List<int> line001 = new List<int>();
                foreach (JSONNode col in line)
                {
                    line001.Add((int)col);
                }
                payLines.Add(line001);
            }
            _contentBB.Instance.payLines = payLines;


            MainBlackboardController.Instance.SyncMyUICreditToTemp();


            OnPropertyChangeTotalBet();


 
            // 发彩金
            /*
            SasCommand.Instance.SetMeterMyCredit((int)_consoleBB.Instance.myCredit);
            SasCommand.Instance.SetMeterTotalCoinIn((int)_consoleBB.Instance.historyTotalCoinInCredit);
            SasCommand.Instance.SetMeterTotalCoinOut((int)_consoleBB.Instance.historyTotalCoinOutCredit);
            SasCommand.Instance.SetMeterTotalJcakpotCredits(10001);
            SasCommand.Instance.SetMeterTotalGamePlayed(20001);
            SasCommand.Instance.SetMeterTotalHeadPaidCredits(30001);
            SasCommand.Instance.SetMeterTotalTicketInCredits(40001);
            SasCommand.Instance.SetMeterTotalTicketOutCredits(50001);
            SasCommand.Instance.SetMeterTotalElectronicTransfersToMachine(60001);
            SasCommand.Instance.SetMeterTotalElectronicTransfersToHost(70001);
            SasCommand.Instance.SetMeterTotalCreditsFromBills(80001);
            SasCommand.Instance.SetMeterBillInCash(50, 333, 100);*/



            try
            {
                //DllAlgorithmManager.Instance.Init();

                SlotDllAlgorithmG152Manager.Instance.Init((int)_contentBB.Instance.totalBet);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }

            // 初始化彩金
            InitGameJackpot();


        }


        #region 游戏彩金初始化和获取
        const string INIT_GAME_JACKPOT = "INIT_GAME_JACKPOT";

        void OnInitGameJackpot(EventData res)
        {
            if (res.name == GlobalEvent.CodeCompleted)
            {
                InitGameJackpot();
                slotMachineCtrl.SkipWinLine(false);
            }
        }


        /// <summary>
        /// 方法卡彩金值
        /// </summary>
        void InitGameJackpot() => DoCor(INIT_GAME_JACKPOT, _InitGameJackpot());
        IEnumerator _InitGameJackpot()
        {
            yield return new WaitForSeconds(0.5f);


            JackpotInitInfo jpData = SlotDllAlgorithmG152Manager.Instance.GetJackpotGame(_contentBB.Instance.totalBet);

            JackpotRes info = new JackpotRes();
            _contentBB.Instance.jpGameRes = info;
            info.curJackpotGrand = (float)(jpData.jackpotout[0]/100);
            info.curJackpotMajor = (float)(jpData.jackpotout[1]/100);
            info.curJackpotMinior = (float)(jpData.jackpotout[2]/100);
            info.curJackpotMini = (float)(jpData.jackpotout[3]/100);

            SetUIJackpotGameReel();
            DebugUtils.Log("init game jackpot finish!");
        }



        #endregion

        void SetJPCurCredit(JackpotWinInfo jpWin)
        {
            switch (jpWin.id)
            {
                case 0:
                    uiJPGrandCtrl.SetData(jpWin.curCredit);
                    break;
                case 1:
                    uiJPMajorCtrl.SetData(jpWin.curCredit);
                    break;
                case 2:
                    uiJPMinorCtrl.SetData(jpWin.curCredit);
                    break;
                case 3:
                    uiJPMiniCtrl.SetData(jpWin.curCredit);
                    break;
            }
        }






        [Button]
        void StartGameOnce(Action successCallback = null, Action<string> errorCallback = null)
        {
            DoCor(COR_GAME_ONCE, GameOnce(successCallback, errorCallback));
        }

        void StartGameAuto(Action successCallback = null, Action<string> errorCallback = null)
        {
            DoCor(COR_GAME_AUTO, GameAuto(successCallback, errorCallback));
        }




        GameSenceData testlastGameSenceData;

        // 待废弃
        int? seqidMachine;


        IEnumerator RequestSlotSpinFromDll02(Action successCallback = null, Action<string> errorCallback = null)
        {
            bool isNext = false;
            bool isBreak = false;

            long totalBet = (int)_contentBB.Instance.totalBet;

            // 游戏数据
            SlotDllAlgorithmG152.LinkInfo res = SlotDllAlgorithmG152Manager.Instance.GetSlotSpin(totalBet);

            // 数据解析
            DlllDataG152Controller.Instance.ParseSlotSpin(totalBet, res);

            // 玩家分数同步
            yield return DlllDataG152Controller.Instance.SetPlayerCredit(() =>
            {

            }, (errMsg) =>
            {
                errorCallback?.Invoke(errMsg);
                isBreak = true;
            });

            if (isBreak) yield break;



            // 数据入库
            DlllDataG152Controller.Instance.Record();

            // 设置UIJP
            SetUIJackpotGameReel();

            successCallback?.Invoke();


            yield return null;
        }



        IEnumerator RequestSlotSpinFromMock03(Action successCallback = null, Action<string> errorCallback = null)
        {
            bool isNext = false;
            bool isBreak = false;

            long totalBet = (int)_contentBB.Instance.totalBet;
            JSONNode resNode = null;

            // 游戏数据
            DlllDataG152Controller.Instance.RequestSlotSpinFromMock(totalBet,
            (res) =>
            {
                resNode = res as JSONNode;
                isNext = true;
            },
            (err) =>
            {
                errorCallback?.Invoke(err.msg);
                isNext = true;
                isBreak = true;
            });


            yield return new WaitUntil(() => isNext == true);
            isNext = false;

            if (isBreak)
                yield break;


            // 数据解析
            LinkInfo resObj = JsonConvert.DeserializeObject<LinkInfo>(resNode.ToString());
            DlllDataG152Controller.Instance.ParseSlotSpin(totalBet, resObj);

            // 玩家分数同步
            yield return DlllDataG152Controller.Instance.SetPlayerCredit(() =>
            {

            }, (errMsg) =>
            {
                errorCallback?.Invoke(errMsg);
                isBreak = true;
            });

            if (isBreak) yield break;



            // 数据入库
            DlllDataG152Controller.Instance.Record();

            // 设置UIJP
            SetUIJackpotGameReel();

            successCallback?.Invoke();

        }


        public void SetUIJackpotGameReel()
        {
            JackpotRes info = _contentBB.Instance.jpGameRes;

            _contentBB.Instance.uiGrandJP.nowCredit = uiJPGrandCtrl.nowCredit;
            _contentBB.Instance.uiMajorJP.nowCredit = uiJPMajorCtrl.nowCredit;
            _contentBB.Instance.uiMinorJP.nowCredit = uiJPMinorCtrl.nowCredit;
            _contentBB.Instance.uiMiniJP.nowCredit = uiJPMiniCtrl.nowCredit;

            _contentBB.Instance.uiGrandJP.curCredit = info.curJackpotGrand;
            _contentBB.Instance.uiMajorJP.curCredit = info.curJackpotMajor;
            _contentBB.Instance.uiMinorJP.curCredit = info.curJackpotMinior;
            _contentBB.Instance.uiMiniJP.curCredit = info.curJackpotMini;

            // 游戏滚轮显示
            uiJPGrandCtrl.SetData(_contentBB.Instance.jpGameWhenCreditLst[0]);
            uiJPMajorCtrl.SetData(_contentBB.Instance.jpGameWhenCreditLst[1]);
            uiJPMinorCtrl.SetData(_contentBB.Instance.jpGameWhenCreditLst[2]);
            uiJPMiniCtrl.SetData(_contentBB.Instance.jpGameWhenCreditLst[3]);
        }










        /// <summary>
        /// 大厅彩金
        /// </summary>
        /// <param name="res"></param>
        void OnRpcJackpotHall(string res)
        {
            //DebugUtil.Log($"{winJackpotInfo.macId}号机  {winJackpotInfo.seat}分机 彩金id: {winJackpotInfo.jackpotId}  中奖金额：{winJackpotInfo.win}");

            var winJPInfo = JsonConvert.DeserializeObject<WinJackpotInfo>(res);
            _contentBB.Instance.dataJackpotHall.Add(winJPInfo);

            long creditBefore = _consoleBB.Instance.myCredit;
            long winCredit = _consoleBB.Instance.coinInScale * winJPInfo.win;

            //插入数据库
            MachineDeviceCommonBiz.Instance.ConInWhenHitJackpotOnline(winJPInfo.win);

            string jpName = "jp_online";
            switch (winJPInfo.jackpotId)
            {
                case 0:
                    jpName = "jp_online_grand";
                    break;
                case 1:
                    jpName = "jp_online_major";
                    break;
                case 2:
                    jpName = "jp_online_minor";
                    break;
                case 3:
                    jpName = "jp_online_mini";
                    break;
            }
            TableJackpotRecordAsyncManager.Instance.AddJackpotRecord(winJPInfo.jackpotId, jpName, 
                winCredit, creditBefore, creditBefore + winCredit, 
                _contentBB.Instance.curGameGuid, winJPInfo.time);
        }



        void OnGameReset()
        {
            ClearCor(COR_GAME_IDLE);
            slotMachineCtrl.isStopImmediately = false;
            goSlotCover?.SetActive(false);

            
            ClearCor(COR_EFFECT_SLOW_MOTION);
            isEffectSlowMotion = false;
            goExpectation.SetActive(false);


            isStoppedSlotMachine = false;

            slotMachineCtrl.SkipWinLine(true);

            //_contentBB.Instance.isRequestToRealCreditWhenStop = false;
        }

        ReelsTurnType reelsTurnType;


        /// <summary>
        /// 加钱动画
        /// </summary>
        /// <remarks>
        /// * 本剧被立马按停，或使用了即中即退时，不播放加钱动画。
        /// </remarks>
        bool isAddCreditAnim => slotMachineCtrl.isStopImmediately == false && !_consoleBB.Instance.isCoinOutImmediately;
        IEnumerator GameOnce(Action successCallback, Action<string> errorCallback)
        {

            if (!_consoleBB.Instance.isMachineActive)
            {
                errorCallback?.Invoke("<size=24>Machine not activated!</size>");
                yield break;
            }


            long totalBet = _contentBB.Instance.totalBet; 
            long myCredit = _consoleBB.Instance.myCredit;

            if (myCredit < totalBet)
            {
                tipCoinIn = true;
                errorCallback?.Invoke("<size=15>Balance is insufficient, please recharge first</size>");
                yield break;
            }

            #region reset
            OnGameReset();
            #endregion

            _contentBB.Instance.gameState = GameState.Spin; //BlackboardUtils.SetValue("./gameState", GameState.Spin); 

            // #创建 Turn
            slotMachineCtrl.BeginTurn();


            bool isNext = false;
            bool isBreak = false;
            string errMsg = "";


            if (false && ApplicationSettings.Instance.isMock)
            {
                yield return RequestSlotSpinFromMock03(() =>
                {
                    isNext = true;
                }, (err) =>
                {
                    errMsg = err;
                    isNext = true;
                    isBreak = true;
                });
            }
            else if (true)
            {
                yield return RequestSlotSpinFromDll02(() =>
                {
                    isNext = true;
                }, (err) =>
                {
                    errMsg = err;
                    isNext = true;
                    isBreak = true;
                });
            }

            yield return new WaitUntil(() => isNext == true);
            isNext = false;


            if (isBreak)
            {
                if (errorCallback != null)
                    errorCallback.Invoke(errMsg);
                yield break;
            }

            /*
            // 这里测试用之后要去掉！
            if (ApplicationSettings.Instance.isRelease == false) 
            {
                slotMachineCtrl.isStopImmediately = true;  // 去停止游戏  
            }
            */


            // 大厅彩金
            /*seqidJpHall = JackpotHallManager.Instance.RequestsJackpotHallData(
                new JackBetInfo
                {
                    seat = 1,
                    bet = 40000,
                    betPercent = 100,
                    scoreRate = 10000,
                    JPPercent = 5
                },
                (res) =>
                {
                    DebugUtil.Log($"==@ 大厅彩金 = {res}");
                   // var winJackpotInfo = JsonConvert.DeserializeObject<WinJackpotInfo>((string)res);

                    isNext = true;
                },
                (err) =>
                {
                    DebugUtil.LogWarning($"大厅彩金失败 = {err}");
                    isNext = true;
                }
            );
            
            yield return new WaitUntil(() => isNext == true);
            isNext = false;    
            
            seqidJpHall = null;
            */


            if (_consoleBB.Instance.isJackpotOnLine)
            {
                if (ApplicationSettings.Instance.isMock)
                {   
                    // 模拟在线彩金中奖数据
                    MachineDataManager.Instance.RequestJackpotOnLineWhenMock();
                }
                else
                {
                    JackpotOnLineManager.Instance.RequestsJackpotOnLineData(
                        new JackBetInfo
                        {
                            seat = 1,  // 固定死
                            bet = (int)_contentBB.Instance.totalBet,  // 总压注
                            betPercent = 100, // 固定死
                            scoreRate =  _consoleBB.Instance.jackpotScoreRate,      //10000,  // 1 除以 币值 乘以 1000 整形   （联网彩金分值比 ：只能该币值）
                            JPPercent =  _consoleBB.Instance.jackpotPercent,    //5  // 千分之几（1 - 100 可调 ；名称： 联网彩金比（千分）  ）
                        },
                        null, null
                    );
                }
            }





            #region 选择滚轮数据
            /*if (_contentBB.Instance.isReelsSlowMotion)
                BlackboardUtils.Invoke("@customData/SelectData", new object[] { "CustomData Reels Slow Motion" });
            else
                BlackboardUtils.Invoke("@customData/SelectData", new object[] { "CustomData Reels Regular" });*/

            // BlackboardUtils.Invoke("@customData/SelectData", new object[] { _contentBB.Instance.customDataName") });
            /*
            if (_contentBB.Instance.isReelsSlowMotion)
            {
                _reelSetBB.Instance.SelectData(REEL_SETTING_SLOW_MOTION);
            }*/
            //_reelSetBB.Instance.SelectData(_contentBB.Instance.customDataName);
            #endregion


            slotMachineCtrl.BeginSpin();


            if (_contentBB.Instance.isReelsSlowMotion)
            {
                DoCor(COR_EFFECT_SLOW_MOTION, ShowEffectReelsSlowMotion());

                slotMachineCtrl.ShowSymbolAppearEffectAfterReelStop(true);
            }
            else
            {
                slotMachineCtrl.ShowSymbolAppearEffectAfterReelStop(_contentBB.Instance.winList.Count == 0);
            }



            if (slotMachineCtrl.isStopImmediately)
            {
                reelsTurnType = ReelsTurnType.Once;

                DoCor(COR_REELS_TURN, slotMachineCtrl.TurnReelsOnce(_contentBB.Instance.strDeckRowCol, () =>
                {
                    isNext = true;
                }));

                isNext = false;
                yield return new WaitUntil(() => isNext == true);
            }
            else
            {
                reelsTurnType = ReelsTurnType.Normal;

                DoCor(COR_REELS_TURN, slotMachineCtrl.TurnReelsNormal(_contentBB.Instance.strDeckRowCol, () =>
                {
                    isNext = true;
                }));

                isNext = false;
                yield return new WaitUntil(() => isNext == true || slotMachineCtrl.isStopImmediately == true);

                // 等待移动结束
                if (slotMachineCtrl.isStopImmediately && isNext == false)
                {
                    DoCor(COR_REELS_TURN, slotMachineCtrl.ReelsToStopOrTurnOnce(() =>
                    {
                        isNext = true;
                    }));

                    isNext = false;
                    yield return new WaitUntil(() => isNext == true);
                }

            }

            List<SymbolWin> winList = _contentBB.Instance.winList;
            long allWinCredit = 0;
            if (winList.Count > 0)
            {

                long totalWinLineCredit = slotMachineCtrl.GetTotalWinCredit(winList);
                allWinCredit = totalWinLineCredit;

                WinLevelType winLevelType = GetBigWinType();

                yield return ShowWinListOnceAtNormalSpin009(winList);



                if (winLevelType != WinLevelType.None)
                {
                    //goSlotCover?.SetActive(true);
                    //slotMachineCtrl.ShowTotalWinListOnce(winList,false);

                    slotMachineCtrl.ShowSymbolWinDeck( slotMachineCtrl.GetTotalSymbolWin(winList), true);

                    // 大奖弹窗
                    yield return WinPopup();

                    slotMachineCtrl.CloseSlotCover(); //goSlotCover?.SetActive(false);

                    slotMachineCtrl.SkipWinLine(false);
     
                }
                else {
                    // 总线赢分（同步？？）
                    bool isAddToCredit = totalWinLineCredit > _contentBB.Instance.totalBet * 4;
                    slotMachineCtrl.SendPrepareTotalWinCreditEvent(totalWinLineCredit, isAddToCredit);
                }

                // 总线赢分（同步？？）
                //slotMachineCtrl.SendTotalWinCreditEvent(totalWinLineCredit);
                slotMachineCtrl.SendTotalWinCreditEvent(allWinCredit);

                //加钱动画
                MainBlackboardController.Instance.AddMyTempCredit(totalWinLineCredit, true, isAddCreditAnim);
            }



            #region 中游戏彩金

            List<JackpotWinInfo> jpRes = _contentBB.Instance.jpWinLst;


            if (jpRes.Count > 0)
            {
                // 中彩金动画
                slotMachineCtrl.SkipWinLine(true);
                slotMachineCtrl.OpenSlotCover();
                slotMachineCtrl.ShowSymbolEffect(SymbolEffectType.Hit, new List<int>() { 12 }, true, 12, true);
                yield return new WaitForSeconds(1.5f);
            }

            while (jpRes.Count > 0)
            {
                JackpotWinInfo  jpWin = jpRes[0];
                jpRes.RemoveAt(0);

                Action onJPPoolSubCredit = () => {
                    SetJPCurCredit(jpWin);
                };

                allWinCredit += (long)jpWin.winCredit;
                PageManager.Instance.OpenPageAsync(PageName.PO152PopupJackpot1080,
                    new EventData<Dictionary<string, object>>("", new Dictionary<string, object>
                    {
                        ["jackpotType"] = jpWin.name,
                        ["totalEarnCredit"] = jpWin.winCredit,
                        ["onJPPoolSubCredit"] = onJPPoolSubCredit,
                    }),
                    (res) =>
                    {
                        isNext = true;
                    });

                isNext = false;
                yield return new WaitUntil(() => isNext == true);

                // 总线赢分（同步？？）
                slotMachineCtrl.SendTotalWinCreditEvent(allWinCredit);

                MainBlackboardController.Instance.AddMyTempCredit((long)jpWin.winCredit, true, isAddCreditAnim);
            }

            #endregion



            #region 中大厅彩金
            while (_contentBB.Instance.dataJackpotHall.Count > 0)
            {
                WinJackpotInfo data = _contentBB.Instance.dataJackpotHall[0];
                _contentBB.Instance.dataJackpotHall.RemoveAt(0);

                //long fromCredit = data.win < 1000 ? data.win : data.win - 1000;

                long winCredit = _consoleBB.Instance.coinInScale * data.win;
                allWinCredit += winCredit;

                PageManager.Instance.OpenPageAsync(PageName.PopupJackpotOnLine002,
                    new EventData<Dictionary<string, object>>("", new Dictionary<string, object>
                    {
                        ["toCredit"] = winCredit,
                        ["jackpotType"] = data.jackpotId,
                        //["fromCredit"] = (long)fromCredit
                    }),
                    (res) =>
                    {
                        isNext = true;
                    });
                isNext = false;
                yield return new WaitUntil(() => isNext == true);


                // 总线赢分（同步？？）
                slotMachineCtrl.SendTotalWinCreditEvent(allWinCredit);

                MainBlackboardController.Instance.AddMyTempCredit(winCredit, true, isAddCreditAnim);
            }

            #endregion


    
            // 本剧同步玩家金钱
            MainBlackboardController.Instance.SyncMyTempCreditToReal(false);




           // 即中即退
           yield return CoinOutImmediately(allWinCredit);


            // Free Spin
            if (_contentBB.Instance.isFreeSpinTrigger)
            {

                // 财神动画
                slotMachineCtrl.SkipWinLine(true);
                slotMachineCtrl.OpenSlotCover();
                slotMachineCtrl.ShowSymbolEffect(SymbolEffectType.Hit, new List<int>() { 11 }, true, 11, true);

                yield return new WaitForSeconds(3f);

                InputStackContextFreeSpin((context) =>
                {
                    goBgRegular.SetActive(false);
                    goBgFreeSpin.SetActive(true);

                    if (goPanelFreeSpin != null)
                        goPanelFreeSpin.SetActive(true);
                    // Transform tfm = goAnchorMain.GetComponent<Transform>();
                    // tfm.localPosition = new Vector3(0, 75, 0);
                });


                PageManager.Instance.OpenPageAsync(PageName.PO152PopupFreeSpinTrigger1080,
                    new EventData<Dictionary<string, object>>("",
                        new Dictionary<string, object>()
                        {
                            ["count"] = _contentBB.Instance.freeSpinTotalTimes,
                            ["autoCloseTimeS"] = 3f,
                        }),
                    (ed) =>
                    {
                        isNext = true;
                    });

                isNext = false;
                yield return new WaitUntil(() => isNext == true);


                slotMachineCtrl.BeginBonusFreeSpin();


                yield return GameFreeSpin(null, errorCallback);


                OutputStackContextFreeSpin(
                (context) =>
                {
                    //BlackboardUtils.Invoke("@customData/SelectData", new object[] { (string)context["./customDataName"] });
                    //_reelSetBB.Instance.SelectData(_contentBB.Instance.customDataName);

                    SlotGameEffectManager.Instance.SetEffect(SlotGameEffect.Default);

                    slotMachineCtrl.SetReelsDeck((string)context["./strDeckRowCol"]);

                    _spinWEBB.Instance.SelectData(_spinWEBB.SPIN_WIN_EFFECT_FREE_SPIN_TRIGGER);

                    goSlotCover.SetActive(true);
                    goBgRegular.SetActive(true);
                    goBgFreeSpin.SetActive(false);


                    // slotMachineCtrl.ShowSymbolWinDeck((SymbolWin)context["./winFreeSpinTriggerOrAddCopy"],true);
                    slotMachineCtrl.ShowSymbolEffect(SymbolEffectType.Hit, new List<int>() { 11 }, true, 11, true);



                    if (goPanelFreeSpin != null)
                        goPanelFreeSpin.SetActive(false);
                    // Transform tfm = goAnchorMain.GetComponent<Transform>();
                    // tfm.localPosition = new Vector3(0, 0, 0);
                });

                slotMachineCtrl.EndBonusFreeSpin();


                PageManager.Instance.OpenPageAsync(PageName.PO152PopupFreeSpinResult1080,
                new EventData<double>("", _contentBB.Instance.freeSpinTotalWinCredit),
                (ed) =>
                {
                    DebugUtils.LogWarning($"msg: {(string)ed.value}");
                    isNext = true;
                });
                isNext = false;
                yield return new WaitUntil(() => isNext == true);

                yield return slotMachineCtrl.SlotWaitForSeconds(1.5f);

                // 大奖弹窗？？
            }


            // 进入空闲模式
            _contentBB.Instance.gameState = GameState.Idle;
            if (winList.Count > 0 && !_contentBB.Instance.isAuto && !_contentBB.Instance.isFreeSpinTrigger)
            {
                DoCor(COR_GAME_IDLE, GameIdle(winList));
            }


            if (successCallback != null)
                successCallback.Invoke(); 


            // 游戏过程中进行上分 或 投币
            if (_contentBB.Instance.isRequestToRealCreditWhenStop)
            {
                MainBlackboardController.Instance.SyncMyTempCreditToReal(true);
                _contentBB.Instance.isRequestToRealCreditWhenStop = false;
            }
        }



        IEnumerator GameFreeSpinOnce(Action successCallback, Action<string> errorCallback)
        {

            #region reset
            OnGameReset();
            #endregion

            _contentBB.Instance.gameState = GameState.FreeSpin;


            bool isNext = false;
            bool isBreak = false;
            string errMsg = "";



            if (false && ApplicationSettings.Instance.isMock)
            {
                yield return RequestSlotSpinFromMock03(() =>
                {
                    isNext = true;
                }, (err) =>
                {
                    errMsg = err;
                    isNext = true;
                    isBreak = true;
                });
            }
            else
            {
                yield return RequestSlotSpinFromDll02(() =>
                {
                    isNext = true;
                }, (err) =>
                {
                    errMsg = err;
                    isNext = true;
                    isBreak = true;
                });
            }



            yield return new WaitUntil(() => isNext == true);

            if (isBreak)
            {

                if (errorCallback != null)
                    errorCallback.Invoke(errMsg);
                yield break;
            }



            #region 选择滚轮数据
            //BlackboardUtils.Invoke("@customData/SelectData", new object[] { _contentBB.Instance.customDataName") });
            //_reelSetBB.Instance.SelectData(_contentBB.Instance.customDataName);
            #endregion


            slotMachineCtrl.BeginSpin();


            if (_contentBB.Instance.isReelsSlowMotion)
            {
                DoCor(COR_EFFECT_SLOW_MOTION, ShowEffectReelsSlowMotion());

                slotMachineCtrl.ShowSymbolAppearEffectAfterReelStop(true);
            }
            else
            {
                slotMachineCtrl.ShowSymbolAppearEffectAfterReelStop(_contentBB.Instance.winList.Count == 0);
            }


            if (slotMachineCtrl.isStopImmediately)
            {
                reelsTurnType = ReelsTurnType.Once;

                DoCor(COR_REELS_TURN, slotMachineCtrl.TurnReelsOnce(_contentBB.Instance.strDeckRowCol, () =>
                {
                    isNext = true;
                }));

                isNext = false;
                yield return new WaitUntil(() => isNext == true);
            }
            else
            {
                reelsTurnType = ReelsTurnType.Normal;

                DoCor(COR_REELS_TURN, slotMachineCtrl.TurnReelsNormal(_contentBB.Instance.strDeckRowCol, () =>
                {
                    isNext = true;
                }));

                isNext = false;
                yield return new WaitUntil(() => isNext == true || slotMachineCtrl.isStopImmediately == true);

                // 等待移动结束
                if (slotMachineCtrl.isStopImmediately && isNext == false)
                {
                    DoCor(COR_REELS_TURN, slotMachineCtrl.ReelsToStopOrTurnOnce(() =>
                    {
                        isNext = true;
                    }));
                    isNext = false;
                    yield return new WaitUntil(() => isNext == true);
                }
            }



            List<SymbolWin> winList = _contentBB.Instance.winList; // BlackboardUtils.GetValue<List<SymbolWin>>("./winList");
            long allWinCredit = 0;
            #region Win
            if (winList.Count > 0)
            {

                long totalWinLineCredit = slotMachineCtrl.GetTotalWinCredit(winList);
                allWinCredit = totalWinLineCredit;

                WinLevelType winLevelType = GetBigWinType();


                //yield return ShowSingleWinListOnceAtFreeSpin(winList,0.8f, timeTotalWin);

                yield return ShowSingleWinListOnceAtFreeSpin009(winList);

                // 播大奖弹窗
                if (winLevelType != WinLevelType.None)
                {
                    //goSlotCover?.SetActive(true);
                    //slotMachineCtrl.ShowTotalWinListOnce(winList, false);

                    slotMachineCtrl.ShowSymbolWinDeck(slotMachineCtrl.GetTotalSymbolWin(winList), true);

                    // 大奖弹窗
                    yield return WinPopup();

                    slotMachineCtrl.CloseSlotCover();

                    slotMachineCtrl.SkipWinLine(false);

                }
                else
                {
                    // 总线赢分（同步？？）
                    bool isAddToCredit = totalWinLineCredit > _contentBB.Instance.totalBet * 4;
                    slotMachineCtrl.SendPrepareTotalWinCreditEvent(totalWinLineCredit, isAddToCredit);
                }

                // 总线赢分事件
                slotMachineCtrl.SendTotalWinCreditEvent(totalWinLineCredit);

                //加钱动画
                MainBlackboardController.Instance.AddMyTempCredit(totalWinLineCredit, true, isAddCreditAnim);

            }
            #endregion



            /* 先结算“免费游戏”或“小游戏”再回主游戏结算主游戏，则每局不能同步玩家真实金钱金额
            MainBlackboardController.Instance.SyncMyCreditToReal(false);*/

            /*
             if (_contentBB.Instance.isFreeSpinTrigger"))
             {
                 //增加免费游戏？？
             }*/



            #region 中游戏彩金

            List<JackpotWinInfo> jpRes = _contentBB.Instance.jpWinLst;
            while (jpRes.Count > 0)
            {
                JackpotWinInfo jpWin = jpRes[0];
                jpRes.RemoveAt(0);


                Action onJPPoolSubCredit = () => {
                    SetJPCurCredit(jpWin);
                };


                allWinCredit += (long)jpWin.winCredit;

                PageManager.Instance.OpenPageAsync(PageName.PO152PopupJackpot1080,
                    new EventData<Dictionary<string, object>>("", new Dictionary<string, object>
                    {
                        ["jackpotType"] = jpWin.name,
                        ["totalEarnCredit"] = jpWin.winCredit,
                        ["onJPPoolSubCredit"] = onJPPoolSubCredit,
                    }),
                    (res) =>
                    {
                        isNext = true;
                    });

                isNext = false;
                yield return new WaitUntil(() => isNext == true);

                // 总线赢分事件
                slotMachineCtrl.SendTotalWinCreditEvent(allWinCredit);

                MainBlackboardController.Instance.AddMyTempCredit((long)jpWin.winCredit, true, isAddCreditAnim);
            }
            #endregion


            // 本剧同步玩家金钱
            MainBlackboardController.Instance.SyncMyTempCreditToReal(false);


            // 即中即退
            yield return CoinOutImmediately(allWinCredit);


            #region 添加额外7场免费游戏
            if (_contentBB.Instance.isFreeSpinAdd)
            {
                /* 测试代码
                CommonPopupHandler.Instance.OpenPopupSingle(new CommonPopupInfo()
                {
                    text = I18nMgr.T("[测试]： 添加额外7场免费游戏"),
                    type = CommonPopupType.OK,
                    buttonText1 = I18nMgr.T("OK"),
                    buttonAutoClose1 = true,
                    callback1 = delegate
                    {
                        isNext = true;
                    },
                    isUseXButton = false,
                });
                isNext = false;
                yield return new WaitUntil(() => isNext == true);
                 */

                slotMachineCtrl.BeginBonusFreeSpinAdd();

                PageManager.Instance.OpenPageAsync(PageName.PO152PopupFreeSpinTrigger1080,
                    new EventData<Dictionary<string, object>>("",
                        new Dictionary<string, object>()
                        {
                            //["autoCloseTimeS"] = 3f,
                            ["isAddFreeGame"] = true,
                        }),
                    (ed) =>
                    {
                        isNext = true;
                    });

                isNext = false;
                yield return new WaitUntil(() => isNext == true);


                // 重置剩余的局数
                _contentBB.Instance.showFreeSpinRemainTime = _contentBB.Instance.showFreeSpinRemainTime + 7;


                yield return slotMachineCtrl.SlotWaitForSeconds(1.5f);
                slotMachineCtrl.EndBonusFreeSpinAdd();


            }
            #endregion




            _contentBB.Instance.gameState = GameState.Idle;
            // 先结算主游戏，再进入“免费游戏”或“小游戏”，则每局都可以同步玩家真实金钱金额

            if (successCallback != null)
                successCallback.Invoke();
        }





        WinLevelType GetBigWinType()
        {
            long baseGameWinCredit = _contentBB.Instance.baseGameWinCredit;
            List<WinMultiple> winMultipleList = _contentBB.Instance.winMultipleList;
            long totalBet = _contentBB.Instance.totalBet;
            WinLevelType winLevelType = WinLevelType.None;
            for (int i = 0; i < winMultipleList.Count; i++)
            {
                if (baseGameWinCredit > totalBet * winMultipleList[i].multiple)
                {
                    winLevelType = winMultipleList[i].winLevelType;
                }
            }
            return winLevelType;
        }





        IEnumerator WinPopup()
        {

            WinLevelType winLevelType = GetBigWinType();

            if (winLevelType != WinLevelType.None)
            {
                bool isNext = false;


                PageManager.Instance.OpenPageAsync(PageName.PO152PopupBigWin1080,
                new EventData<Dictionary<string, object>>("WinLevelType",
                    new Dictionary<string, object>()
                    {
                        ["winLevelType"] = winLevelType,
                        ["totalEarnCredit"] = _contentBB.Instance.baseGameWinCredit,
                        ["totalBet"] =_contentBB.Instance.totalBet,
                    }
                ),
                (ed) =>
                {
                    isNext = true;
                });
                isNext = false;
                yield return new WaitUntil(() => isNext == true);


                int indx = UnityEngine.Random.Range(1, 3);
                GameSoundHelper.Instance.PlaySound(indx == 1 ? SoundKey.PopupWinAfterCloseCongratulate01 : SoundKey.PopupWinAfterCloseCongratulate02);
            }

        }



        List<Dictionary<string,object>> stackContext = new List<Dictionary<string, object>>();
        void InputStackContextFreeSpin(Action<Dictionary<string, object>> inputStackCallBack)
        {
            Dictionary<string, object> context = new Dictionary<string, object>()
            {
                ["name"] = "FreeSpinTrigger",
                ["modifyTime"] = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),

                ["./gameState"] = _contentBB.Instance.gameState,


                ["./winList"] = _contentBB.Instance.winList,
                ["./response"] = _contentBB.Instance.response,
                ["./winFreeSpinTriggerOrAddCopy"] = _contentBB.Instance.winFreeSpinTriggerOrAddCopy,
                //["./win5Kind"] = _contentBB.Instance.win5Kind,
                //["./isWin5Kind"] = _contentBB.Instance.isWin5Kind,
                ["./strDeckRowCol"] = _contentBB.Instance.strDeckRowCol,
                ["./middleIndexList"] = _contentBB.Instance.middleIndexList,
                ["./curReelStripsIndex"] = _contentBB.Instance.curReelStripsIndex,
                ["./nextReelStripsIndex"] = _contentBB.Instance.nextReelStripsIndex,
                ["./totalEarnCredit"] = _contentBB.Instance.totalEarnCredit,
                ["./isReelsSlowMotion"] = _contentBB.Instance.isReelsSlowMotion,
                ["./isFreeSpinTrigger"] = _contentBB.Instance.isFreeSpinTrigger,
                //["./customDataName"] = _contentBB.Instance.customDataName,
                ["./shufflingList"] = _contentBB.Instance.shufflingList,

                ["./curGameNumber"] = _contentBB.Instance.curGameNumber,
                ["./curGameCreatTimeMS"] = _contentBB.Instance.curGameCreatTimeMS,
                ["./curGameGuid"] =  _contentBB.Instance.curGameGuid,
            };
            stackContext.Insert(0, context);

            //=====================
            inputStackCallBack?.Invoke(context);
        }
        void OutputStackContextFreeSpin(Action<Dictionary<string, object>> outputStackCallBack)
        {
            Dictionary<string, object> context = stackContext[0];
            stackContext.RemoveAt(0);

            _contentBB.Instance.gameState = (string)context["./gameState"];


            _contentBB.Instance.winList = (List<SymbolWin>)context["./winList"];
            _contentBB.Instance.response = (string)context["./response"];
            _contentBB.Instance.winFreeSpinTriggerOrAddCopy = (SymbolWin)context["./winFreeSpinTriggerOrAddCopy"];
           // _contentBB.Instance.win5Kind = (SymbolWin)context["./win5Kind"];
            _contentBB.Instance.strDeckRowCol = (string)context["./strDeckRowCol"];
            _contentBB.Instance.middleIndexList = (List<int>)context["./middleIndexList"];
            _contentBB.Instance.curReelStripsIndex = (string)context["./curReelStripsIndex"];
            _contentBB.Instance.nextReelStripsIndex = (string)context["./nextReelStripsIndex"];
            _contentBB.Instance.totalEarnCredit = (long)context["./totalEarnCredit"];
            //_contentBB.Instance.isWin5Kind = (bool)context["./isWin5Kind"];
            _contentBB.Instance.isReelsSlowMotion = (bool)context["./isReelsSlowMotion"];
            _contentBB.Instance.isFreeSpinTrigger = (bool)context["./isFreeSpinTrigger"];
            //_contentBB.Instance.customDataName = (string)context["./customDataName"];
            _contentBB.Instance.shufflingList = (List<List<int>>)context["./shufflingList"];


            _contentBB.Instance.curGameNumber = (long)context["./curGameNumber"];
            _contentBB.Instance.curGameCreatTimeMS = (long)context["./curGameCreatTimeMS"];
            _contentBB.Instance.curGameGuid = (string)context["./curGameGuid"];


            //=====================
            outputStackCallBack?.Invoke(context);
        }


        IEnumerator GameAuto(Action successCallback, Action<string> errorCallback)
        {
            //yield return GameOnce(null, errorCallback);

            bool isErr = false;
            Action<string> errFunc = (err) =>
            {
                isErr = true;
                errorCallback?.Invoke(err);
            };


            while (_contentBB.Instance.isAuto && !_contentBB.Instance.isRequestToStop)
            {
                yield return GameOnce(null, errFunc);

                if (isErr)
                    yield break;

                //yield return new WaitForSeconds(3);
                float time = Time.time; //获取游戏已运行时间(单位：秒)
                while (Time.time - time < 1f)
                {
                    yield return new WaitForSeconds(0.1f);
                    if (!_contentBB.Instance.isAuto)
                        break;
                }
            }

            if (_contentBB.Instance.isRequestToStop)
            {
                _contentBB.Instance.isRequestToStop = false;
                _contentBB.Instance.isAuto = false;
            }

            if (successCallback != null)
                successCallback.Invoke();
        }

        // context
        IEnumerator GameFreeSpin(Action successCallback, Action<string> errorCallback)
        {

            while (_contentBB.Instance.nextReelStripsIndex == "FS")
            {

                yield return GameFreeSpinOnce(null, errorCallback);

                yield return slotMachineCtrl.SlotWaitForSeconds(1);
            }

            if (successCallback != null)
                successCallback.Invoke();
        }


        /// <summary>
        /// 进入游戏Idle状态
        /// </summary>
        /// <param name="winList"></param>
        /// <returns></returns>
        private IEnumerator GameIdle(List<SymbolWin> winList)
        {
            if (winList.Count == 0)
            {
                yield break;
            }

            SlotGameEffectManager.Instance.SetEffect(SlotGameEffect.GameIdle);

            yield return new WaitForSeconds(3f);

            /*
            goSlotCover?.SetActive(true);

            yield return slotMachineCtrl.ShowSingleWinListAwayDuringIdle(winList);
            */

            yield return slotMachineCtrl.ShowWinListAwayDuringIdle009(winList);
        }


        #region 即中即退

        bool isCoinOutImmediatelyFinish = false;
        private IEnumerator CoinOutImmediately(long totalWinCredit)
        {
            if (_consoleBB.Instance.isCoinOutImmediately && totalWinCredit > 0)
            {
                int coinOutCount = DeviceUtils.GetCoinOutNum((int)totalWinCredit);
                if(coinOutCount > 0) //退票个数大于0
                {
                    isCoinOutImmediatelyFinish = false;
                    MachineDeviceCommonBiz.Instance.DoCoinOutImmediately((int)totalWinCredit);
                    yield return new WaitUntil(() => isCoinOutImmediatelyFinish == true);
                }
            }
        }

        void OnCoinInOutEvent(EventData res)
        {
            if (res.name == GlobalEvent.CoinOutSuccess)
            {
                isCoinOutImmediatelyFinish = true;
            }else if (res.name == GlobalEvent.CoinOutError)
            {
                isCoinOutImmediatelyFinish = true;
            }
        }

        #endregion
    }

    public partial class PageGameMain : PageCorBase //BasePageSingleton<GamePage>
    {

        IEnumerator ShowWinListOnceAtNormalSpin009(List<SymbolWin> winList)
        {

            if (_spinWEBB.Instance.isTotalWin)
            {

                yield return slotMachineCtrl.ShowSymbolWinBySetting(
                   slotMachineCtrl.GetTotalSymbolWin(winList), true, SpinWinEvent.TotalWinLine);

                // goSlotCover?.SetActive(true);
                // yield return slotMachineCtrl.ShowTotalWinListOnce(winList, timeTotalWin, timeTotalWin);
                // goSlotCover?.SetActive(false);
            }
            else
            {
                //停止特效显示
                slotMachineCtrl.SkipWinLine(false);

                int idx = 0;
                while (idx < winList.Count)
                {
                    SymbolWin curSymbolWin = winList[idx];
                    //是否是五连线
                    if (slotMachineCtrl.Check5kind(curSymbolWin) && !slotMachineCtrl.isStopImmediately)
                    {
                        yield return Show5KindPoup(curSymbolWin);
                    }

                    yield return slotMachineCtrl.ShowSymbolWinBySetting(curSymbolWin, true, SpinWinEvent.SingleWinLine);

                    ++idx;
                }
            }

            //停止特效显示
            slotMachineCtrl.SkipWinLine(false);

            slotMachineCtrl.CloseSlotCover();

        }

        IEnumerator ShowSingleWinListOnceAtFreeSpin009(List<SymbolWin> winList)
        {
            int idx = 0;
            while (idx < winList.Count)
            {
                SymbolWin curSymbolWin = winList[idx];
                bool isUserMyselfSymbolIndex = true;

                //停止特效显示
                slotMachineCtrl.SkipWinLine(false);


                //是否改变图标
                if (curSymbolWin.customData == "change" || slotMachineCtrl.CheckHasSymbolChange(curSymbolWin))
                {
                    _spinWEBB.Instance.SelectData(_spinWEBB.SPIN_WIN_EFFECT_CHANGE_SYMBOL);

                    GameSoundHelper.Instance.PlaySound(SoundKey.FreeSpinChangeSymbol);
                    yield return slotMachineCtrl.ShowSymbolChangeBySetting(curSymbolWin, "Symbol Change");
                    isUserMyselfSymbolIndex = false;

                    _spinWEBB.Instance.SelectData(_spinWEBB.SPIN_WIN_EFFECT_FREE_SPIN);
                }

                //是否是五连线
                if (slotMachineCtrl.Check5kind(curSymbolWin))
                {
                    yield return Show5KindPoup(curSymbolWin);
                }

                yield return slotMachineCtrl.ShowSymbolWinBySetting(curSymbolWin, isUserMyselfSymbolIndex, SpinWinEvent.SingleWinLine);

                ++idx;
            }

            slotMachineCtrl.CloseSlotCover();

            //停止特效显示
            slotMachineCtrl.SkipWinLine(false);

        }



        #region 特效



        public IEnumerator Show5KindPoup(SymbolWin win5Kind)
        {

            slotMachineCtrl.SkipWinLine(false);
            slotMachineCtrl.CloseSlotCover();

            GameSoundHelper.Instance.PlaySound(SoundKey.FiveLine);

            goEffect5Kind.SetActive(true);
            yield return new WaitForSeconds(1f);
            goEffect5Kind.SetActive(false);

            yield return slotMachineCtrl.SlotWaitForSeconds(0.3f);

        }

        /*
        public IEnumerator Show5KindPoup01(SymbolWin win5Kind)
        {

            GameSoundHelper.Instance.PlaySound(SoundKey.FiveLine);

            goEffect5Kind.SetActive(true);         
            yield return new WaitForSeconds(1f);
            goEffect5Kind.SetActive(false);

            yield return slotMachineCtrl.SlotWaitForSeconds(0.3f);

            goSlotCover?.SetActive(true);
            slotMachineCtrl.ShowSymbolWinAnim(win5Kind);

            yield return slotMachineCtrl.SlotWaitForSeconds(0.5f,0.5f);

            goSlotCover?.SetActive(false);
        }
        */

        public IEnumerator ShowEffectReelsSlowMotion()
        {

            yield return new WaitUntil(() => isEffectSlowMotion == true);

            goExpectation.SetActive(true);
            GameSoundHelper.Instance.PlaySound(SoundKey.SlowMotion);
            GameSoundHelper.Instance.PlaySound(SoundKey.SlowMotionCongratulate);
            //切换滚轮配置数据
            //12行显示gold结束显示gold
            //第三列长滚动

            yield return new WaitUntil(() => isStoppedSlotMachine == true);
            goExpectation.SetActive(false);


            if (_contentBB.Instance.isFreeSpinTrigger)
            {
                GameSoundHelper.Instance.PlaySound(SoundKey.SlowMotionReal123HasGod);
                //goSlotCover.SetActive(true);
                //slotMachineCtrl.ShowSymbolWinAnim(_contentBB.Instance.winFreeSpinTrigger);

                _spinWEBB.Instance.SelectData(_spinWEBB.SPIN_WIN_EFFECT_FREE_SPIN_TRIGGER);
                slotMachineCtrl.ShowSymbolWinDeck(_contentBB.Instance.winFreeSpinTriggerOrAddCopy, true);
            }

        }

        #endregion



    }
}









