#define SQLITE_ASYNC
using GameMaker;
using Newtonsoft.Json;
using SBoxApi;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _consoleBB = PssOn00152.ConsoleBlackboard02;
using System.Threading.Tasks;
using SlotDllAlgorithmG152;

namespace PssOn00152
{

    #region 创建默认表
    public partial class ConsoleBlackboardController02 : MonoBehaviour
    {

        MessageDelegates onPropertyChangedEventDelegates;

        IEnumerator Start()
        {
            // InitParam();

            yield return OnInitParam();

            onPropertyChangedEventDelegates = new MessageDelegates
                (
                    new Dictionary<string, EventDelegate>
                    {
                        { "@console/tableBet", OnPropertyChangeTableBet},
                        { "@console/tableSysSetting", OnPropertyChangeTableSysSetting},
                        //{ "@console/tableMachine", OnPropertyChangeTableMachine},
                        { "@console/shift", OnPropertyChangeShift},
                        { "@console/admin", OnPropertyChangeAdmin},
                        { "@console/manager", OnPropertyChangeManager},

                        { "@console/betAllowList", OnPropertyChangeBetAllowList},//betAllowList
                                                                                //{ "@console/miniJP",  OnPropertyChangeMiniJP},

                        //{ "@console/sboxPlayerInfo",OnPropertyChangeSboxPlayerInfo},
                        //{ "@console/sboxConfData",OnPropertyChangeSBoxConfData},
                    }
                );
            //yield return new WaitForSeconds(2f);
            EventCenter.Instance.AddEventListener<EventData>(ObservableObject.ON_PROPERTY_CHANGED_EVENT, onPropertyChangedEventDelegates.Delegate);

        }





        private void OnDestroy()
        {
            EventCenter.Instance.RemoveEventListener<EventData>(ObservableObject.ON_PROPERTY_CHANGED_EVENT, onPropertyChangedEventDelegates.Delegate);
        }


        void LoadingPageAddSetingCount(int count) => EventCenter.Instance.EventTrigger<EventData>(GlobalEvent.ON_INIT_SETTINGS_EVENT,
                new EventData<int>(GlobalEvent.AddSettingsCount, count));
        void LoadingPageInitSeting(string msg) => EventCenter.Instance.EventTrigger<EventData>(GlobalEvent.ON_INIT_SETTINGS_EVENT,
                new EventData<string>(GlobalEvent.InitSettings, msg));



        IEnumerator OnInitParam()
        {

            MainBlackboard.Instance.gameID = 152;

            bool isNext = false;

            LoadingPageAddSetingCount(7);

            LoadingPageInitSeting("open db ...");

            if (!SQLiteAsyncHelper.Instance.isConnect)
            {
                DebugUtils.LogError($"【Check Record】{SQLiteAsyncHelper.databaseName} is close");
                yield break;
            }


            SQLiteHelper.Instance.OpenDB(ConsoleTableName.DB_NAME, (connect) =>
            {
                isNext = true;
            });

            yield return new WaitUntil(() => isNext == true);
            isNext = false;


            // 【添加】打开数据库


            ConsoleTableUtils.DeleteTables();




            ConsoleTableUtils.CheckOrCreatTableSysSetting();
            ConsoleTableUtils.CheckOrCreatTableCoinInOutRecord();
            ConsoleTableUtils.CheckOrCreatTableJackpotRecord();
            ConsoleTableUtils.CheckOrCreatTableGameRecord();
            ConsoleTableUtils.CheckOrCreatTableLogEventRecord();
            ConsoleTableUtils.CheckOrCreatTableLogErrorRecord();
            ConsoleTableUtils.CheckOrCreatTableBussinessDayRecord();
            ConsoleTableUtils.CheckOrCreatTableBet();

            //ConsoleTableUtils.CheckOrCreatTableUsers();
            //ConsoleTableUtils.CheckOrCreatTableGame();
            ConsoleTableUtils.CheckOrCreatTableOrderId();

            DebugUtils.Log($"获取表数据");


            //ConsoleTableUtils.GetTableGame();
            ConsoleTableUtils.GetTableBet();
            //ConsoleTableUtils.GetTableUsers();


            TableBusniessTotalRecordAsyncManager.Instance.GetTotalBusniess();


            yield return null;

            LoadingPageInitSeting("get table sys settings");
            ConsoleTableUtils.GetTableSysSetting((TableSysSettingItem res) =>
            {
                DebugUtils.Log($"获取表数据成功 ： SysSetting");

                _music = res.music;
                _sound = res.sound;
                SoundManager.Instance.SetBGMVolumScale(_music);
                SoundManager.Instance.SetEFFVolumScale(_sound);


                //I18nMgr.Add("i18n_po152");
                //I18nMgr.Add("i18n_console001");
                foreach (string fileName in  ConfigUtils.i18nLoadFile)
                {
                    I18nMgr.Add(fileName);
                }

                I18nLang curLanguage = (I18nLang)Enum.Parse(typeof(I18nLang), res.language_number);
                I18nMgr.ChangeLanguage(curLanguage);

                //Debug.LogWarning($"【Log】Platform:{ApplicationSettings.Instance.platformName}; Software Ver:{ApplicationSettings.Instance.appVersion}; Is Machine:{ApplicationSettings.Instance.isMachine}; Hotfix Ver:{"--"}");

                // 是否显示调试日志
                EventCenter.Instance.EventTrigger<EventData>(ObservableObject.ON_PROPERTY_CHANGED_EVENT,
                    new EventData<object>("@console/isDebug", _consoleBB.Instance.isDebug));

                isNext = true;
            });

            yield return new WaitUntil(() => isNext == true);
            isNext = false;


            LoadingPageInitSeting("get SBOX_READ_CONF");
            MachineDataManager.Instance.RequestReadConf((data) =>
            {
                SBoxConfData res = (SBoxConfData)data;
                _consoleBB.Instance.sboxConfData = res;


                Debug.Log("【SBoxConfData】 sboxConfData: " + JsonConvert.SerializeObject(res));
                Debug.Log("【SBoxConfData】 投币比例: " + res.CoinValue); // 投币比例
                Debug.Log("【SBoxConfData】 1分对应几票: " + res.scoreTicket);  // 1分对应几票
                Debug.Log("【SBoxConfData】 1票对应几分: " + res.TicketValue); // 1票对应几分（彩票比例）
                Debug.Log("【SBoxConfData】 机台编号: " + res.MachineId); // 机台编号
                Debug.Log("【SBoxConfData】 线号: " + res.LineId); // 线号
                Debug.Log("【SBoxConfData】 上下分: " + res.ScoreUpUnit); // 上下分

                isNext = true;

            }, (BagelCodeError err) =>
            {
                DebugUtils.LogError(err.msg);
            });

            yield return new WaitUntil(() => isNext == true);
            isNext = false;



            LoadingPageInitSeting("get SBOX_GET_ACCOUNT");
            MachineDataManager.Instance.RequestGetPlayerInfo((res) =>
            {
                DebugUtils.LogWarning($"@@##  SBoxAccount == {JSONNodeUtil.ObjectToJsonStr(res)} ");
                SBoxAccount data = (SBoxAccount)res;
                /*
                if (data.result == 0)
                {

                }*/

                int pid = _consoleBB.Instance.pid;

                bool isOK = false;
                List<SBoxPlayerAccount> playerAccountList = data.PlayerAccountList;
                for (int i = 0; i < playerAccountList.Count; i++)
                {
                    if (playerAccountList[i].PlayerId == pid)
                    {
                        _consoleBB.Instance.sboxPlayerInfo = playerAccountList[i];

                        MainBlackboardController.Instance.SyncMyTempCreditToReal(false); //同步玩家金币

                        isOK = true;
                        break;
                    }
                }
                if (!isOK)
                    DebugUtils.LogError($" SBoxPlayerAccount is null , Where PlayerId = {pid}");

                isNext = true;

            }, (BagelCodeError err) =>
            {
                DebugUtils.LogError(err.msg);
            });

            yield return new WaitUntil(() => isNext == true);
            isNext = false;


            LoadingPageInitSeting("get hardware version");
            MachineDataManager.Instance.RequestGetHardwareVersion((res) => {

                _consoleBB.Instance.hardwareVer = (string)res;
                isNext = true;
            }, (err) => {
                DebugUtils.LogError(err.msg);
            });
            yield return new WaitUntil(() => isNext == true);
            isNext = false;

            LoadingPageInitSeting("get algorithm version");

            /*
            MachineDataManager.Instance.RequestGetAlgorithmVersion((res) => {

                _consoleBB.Instance.algorithmVer = (string)res;
                isNext = true;
            }, (err) => {
                DebugUtil.LogError($"{SBoxEventHandle.SBOX_IDEA_VERSION} : {err.msg}");
            });
            yield return new WaitUntil(() => isNext == true);
            isNext = false;
            */

            string ver = SlotDllAlgorithmG152Manager.Instance.GetVersion();
            _consoleBB.Instance.algorithmVer = ver;

            LoadingPageInitSeting("get table finish");
        }





        void OnPropertyChangeAdmin(EventData receivedEvent) => OnPropertyChangeUserPassword("admin", receivedEvent);
        void OnPropertyChangeManager(EventData receivedEvent) => OnPropertyChangeUserPassword("manager", receivedEvent);
        void OnPropertyChangeShift(EventData receivedEvent) => OnPropertyChangeUserPassword("shift", receivedEvent);

        void OnPropertyChangeUserPassword(string userName , EventData receivedEvent)
        {
#if !SQLITE_ASYNC

            TableUsersItem data = (TableUsersItem)receivedEvent.value;
            string sql = $"UPDATE users SET password = '{data.password}' WHERE user_name = '{userName}'"; //shift
            SQLiteHelper01.Instance.ExecuteNonQueryAfterOpenDB(sql);
#else
            TableUsersItem data = (TableUsersItem)receivedEvent.value;
            string sql = $"UPDATE users SET password = '{data.password}' WHERE user_name = '{userName}'"; //shift
            SQLiteAsyncHelper.Instance.ExecuteNonQueryAsync(sql);
#endif


        }

        /*
        void OnPropertyChangeSboxPlayerInfo(EventData receivedEvent)
        {
            DebugUtil.Log("【测试-待确认】：是否可以直接修改玩家金币？？");
            SBoxPlayerAccount data = (SBoxPlayerAccount)receivedEvent.value;
            MachineDataManager.Instance.RequestSetPlayerInfo(data,null);
        }
        void OnPropertyChangeSBoxConfData(EventData receivedEvent)
        {
            SBoxConfData data = (SBoxConfData)receivedEvent.value;
            MachineDataManager.Instance.RequestWriteConf(data, null, null);
        }*/


        void OnPropertyChangeTableBet(EventData receivedEvent)
        {
#if !SQLITE_ASYNC

            string sql = SQLiteHelper01.SQLUpdateTableData<TableBetItem>(ConsoleTableName.TABLE_BET, (TableBetItem)receivedEvent.value);
            SQLiteHelper01.Instance.ExecuteNonQuery(sql);
#else
            string sql = SQLiteAsyncHelper.SQLUpdateTableData<TableBetItem>(ConsoleTableName.TABLE_BET, (TableBetItem)receivedEvent.value);
            SQLiteAsyncHelper.Instance.ExecuteNonQueryAsync(sql);
#endif
        }

        void OnPropertyChangeTableSysSetting(EventData receivedEvent)
        {
            TableSysSettingItem sysSettingItem = (TableSysSettingItem)receivedEvent.value;

            if (sysSettingItem.music != _music)
            {
                _music = sysSettingItem.music;
                SoundManager.Instance.SetBGMVolumScale(_music);
            }

            if (sysSettingItem.sound != _sound)
            {
                _sound = sysSettingItem.sound;
                SoundManager.Instance.SetEFFVolumScale(_sound);
            }

            if(sysSettingItem.language_number != I18nMgr.language.ToString())
            {
                I18nLang curLanguage = (I18nLang)Enum.Parse(typeof(I18nLang), sysSettingItem.language_number);
                I18nMgr.ChangeLanguage(curLanguage);
            }

#if !SQLITE_ASYNC

            string sql = SQLiteHelper01.SQLUpdateTableData<TableSysSettingItem>(ConsoleTableName.TABLE_SYS_SETTING, sysSettingItem);
            SQLiteHelper01.Instance.ExecuteNonQuery(sql);

#else
            string sql = SQLiteAsyncHelper.SQLUpdateTableData<TableSysSettingItem>(ConsoleTableName.TABLE_SYS_SETTING, sysSettingItem);
            SQLiteAsyncHelper.Instance.ExecuteNonQueryAsync(sql);
#endif
        }

        float _sound = 0;
        float _music = 0;




        void OnPropertyChangeBetAllowList(EventData receivedEvent)
        {
            List<BetAllow> betAllowList = (List<BetAllow>)receivedEvent.value;

            TableBetItem tableBet = _consoleBB.Instance.tableBet;
            tableBet.bet_list = JSONNodeUtil.ObjectToJsonStr(betAllowList);
            //string sql = SQLiteHelper02.SQLUpdateTableData<TableBetItem>(TABLE_BET, tableBet);
            //SQLiteHelper02.Instance.ExecuteNonQuery(sql);

            List<long> betList = new List<long>();
            foreach (BetAllow nd in betAllowList)
            {
                if (nd.allowed == 1)
                    betList.Add(nd.value);
            }
            _consoleBB.Instance.betList = betList;

        }

    }

#endregion

}




















