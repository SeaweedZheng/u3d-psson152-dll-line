using Game;
using SBoxApi;
using System.Collections.Generic;
using GameMaker;
using UnityEngine;
using _consoleBB = PssOn00152.ConsoleBlackboard02;
using Newtonsoft.Json;
using System.Collections;


public class DeviceCoder : CorBehaviour
{
    const string COR_CHECK_CODER_REPEAT = "COR_CHECK_CODER_REPEAT";
    void OnEnable()
    {
        /*
        if (!ApplicationSettings.Instance.isMachine)
            return;
        */

        //UI--显示Licenen
        EventCenter.Instance.AddEventListener<EventData>(MachineUIEvent.ON_MACHINE_UI_EVENT, OnMachineUIEvent);


        DoCor(COR_CHECK_CODER_REPEAT, DoTaskRepeat(CheckSBoxNeedActivated, 10000));

    }


    private void OnDisable()
    {
        ClearCor(COR_CHECK_CODER_REPEAT);

        EventCenter.Instance.RemoveEventListener<EventData>(MachineUIEvent.ON_MACHINE_UI_EVENT, OnMachineUIEvent);

        if (seqIdActive != null && MachineDataManager.Instance != null)
            MachineDataManager.Instance.RemoveRequestAt((int)seqIdActive);
        seqIdActive = null;

        if(seqIdCode != null && MachineDataManager.Instance != null)
            MachineDataManager.Instance.RemoveRequestAt((int)seqIdCode);
        seqIdCode = null;
    }


    const string MARK_IS_POPUP_CHECK_ACTIVE = "MARK_IS_POPUP_CHECK_ACTIVE";

    /*
    1:运行时间到(时间打码相关)
    2:盈余档机
    3:历史数据过大
    4:全台爆机(总输分爆机)
    5:本轮爆机
    */

    /*
    //错误码:
    //0:按键档机 
    //1:本轮爆机
    //2：全台爆机（总输分爆机）
    //3:盈余档机
    //4:保单箱未连接 
    //5:打印机缺纸 
    //6:非法开箱1 
    //7:非法开箱2
    //8：运行时间到（时间打码相关）
    //9:历史数据过大
    //10:正在打印
    //11：打印机切刀故障
    //12：打印机门未关闭
    */


    /// <summary>
    /// 打码
    /// </summary>
    /// <remarks>
    /// 【返回值意义】<br/>
    /// * 0:
    /// * 1: 
    /// * 2: 盈利余额用完宕机
    /// 
    /// 
    /// 
    /// </remarks>
    private void CheckSBoxNeedActivated()
    {
        //假数据
        seqIdActive = MachineDataManager.Instance.RequestIsCodingActive((res) =>
        {
            int code = (int)res;
            bool isActive = code == 0;

            DebugUtils.Log($"check code; isActive = {isActive}");

            _consoleBB.Instance.isMachineActive = isActive;
            //BlackboardUtils.SetValue<bool>("@console/isMachineActive", isActive);

            if (!(bool)isActive)
            {
                //控台没有打开
                if (PageManager.Instance.IndexOf(PageName.Console001PageConsoleMain) == -1)
                {

                    if (!CommonPopupHandler.Instance.isOpen(MARK_IS_POPUP_CHECK_ACTIVE)
                       //&& ErrorPopupHandler.Instance.curPopupType != ErrorPopupType.SystemTextOnly
                       )
                        CommonPopupHandler.Instance.OpenPopupSingle(new CommonPopupInfo()
                        {
                            text = string.Format(I18nMgr.T("<size=24>Please activate : {0}</size>"), code),
                            type = CommonPopupType.SystemTextOnly,
                            buttonAutoClose1 = false,
                            buttonAutoClose2 = false,
                            isUseXButton = false,
                            mark = MARK_IS_POPUP_CHECK_ACTIVE,
                        });
                }
            }
        });
    }

    int? seqIdActive;
    int? seqIdCode;
    private void OnMachineUIEvent(EventData evt)
    {
        if (evt.name == MachineUIEvent.ShowCoding)
        {
            seqIdCode = MachineDataManager.Instance.RequestMachineCodingInfo((object res) =>
            {
                OnResponseShowCoder(res as SBoxCoderData);

            }, (BagelCodeError err) =>
            {

            });
        }else if (evt.name == MachineUIEvent.CheckCodeActive)
        {
            CheckSBoxNeedActivated();
        }
    }



    /// <summary>
    /// 返回打码数据
    /// </summary>
    /// <param name="data"></param>
    private async void OnResponseShowCoder(SBoxCoderData data)
    {

        //long day = data.RemainMinute / (60 * 24);
        //long hour = data.RemainMinute % (60 * 24) /60

        long totalBets = data.Bets != 0 ? data.Bets : TableBusniessTotalRecordAsyncManager.Instance.historyTotalBet;
        long totalWins = data.Wins != 0 ? data.Wins : TableBusniessTotalRecordAsyncManager.Instance.historyTotalWin;

        DebugUtils.LogWarning($"获取打码数据 ： {JSONNodeUtil.ObjectToJsonStr(data)}");
        Dictionary<string, object> req = new Dictionary<string, object>()
        {
            ["A"] = $"{totalBets}", //data.Bets.ToString(),
            ["B"] = $"{totalWins}", //data.Wins.ToString(),
            ["C"] = data.MachineId.ToString(),
            ["D"] = data.CoderCount.ToString(),
            ["E"] = data.CheckValue.ToString(),
           // ["Day"] = (data.RemainMinute  / 60 /24).ToString(),//多少天
            //["Hour"] = ((data.RemainMinute / 60) % 24).ToString(),//多少小时
            //["Minute"] = (data.RemainMinute % 60).ToString(),//多少分钟
            ["Day"] = (data.RemainMinute / (60 * 24)).ToString(),//多少天
            ["Hour"] = ((data.RemainMinute % (60 * 24) / 60)).ToString(),//多少小时
            ["Minute"] = (data.RemainMinute % 60).ToString(),//多少分钟


            //((data.RemainMinute / 60) % 24).ToString();
        };

        EventData res = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleCoder, 
            new EventData<Dictionary<string, object>>("", req));

        if (res.value != null)
        {

            seqIdCode = MachineDataManager.Instance.RequestSetCoding(ulong.Parse((string)res.value), (res) =>
            {
                OnCoder(res as SBoxPermissionsData);
            }, (err) =>
            {
                OnCoder(err.response as SBoxPermissionsData);
            });
        }

    }

    //const string COR_CLEAR_SQL_TABLE = "COR_CLEAR_SQL_TABLE";


    const string COR_CODE = "COR_CODE"; 
    /// <summary>
    /// 输入激活码后：
    /// </summary>
    /// <param name="sBoxPermissionsData"></param>
    private void OnCoder(SBoxPermissionsData sBoxPermissionsData)
    {
        DoCor(COR_CODE, _OnCode(sBoxPermissionsData));
    }

    IEnumerator _OnCode(SBoxPermissionsData sBoxPermissionsData)
    {
        //DebugUtil.Log("打码结果");
        //DebugUtil.Log(sBoxPermissionsData.result);
        //DebugUtil.Log(sBoxPermissionsData.permissions);

        bool isNext = false;

        bool isSuccess = sBoxPermissionsData.result == 0;
        if (isSuccess) // 成功
        {
            if (sBoxPermissionsData.permissions / 1000 > 0)//2001
            {
                if (sBoxPermissionsData.permissions % 10 > 0) //清帐
                {

                    //清除“游戏记录”，“投退币记录”数据。

                    MaskPopupHandler.Instance.OpenPopup();

                    MachineRestoreManager.Instance.ClearRecordWhenCoding();

                    yield return new WaitForSeconds(5f);
                }
                else
                {
                    //不清帐
                    Debug.Log("不清帐");
                }
            }

            // 同步玩家金额
            SyncPlayerCredit();

            // 已激活
            _consoleBB.Instance.isMachineActive = true;

            //关掉锁死弹窗
            CommonPopupHandler.Instance.ClosePopup(MARK_IS_POPUP_CHECK_ACTIVE);

            // 【新加】重新获取配置
            MachineDataManager.Instance.RequestReadConf((data) =>
            {
                SBoxConfData res = (SBoxConfData)data;
                _consoleBB.Instance.sboxConfData = res;
                isNext = true;
            }, (BagelCodeError err) =>
            {
                DebugUtils.LogError(err.msg);
                isNext = true;
            });


            yield return new WaitUntil(() => isNext == true);
            isNext = false;


            if (_consoleBB.Instance.betList.Count >0)
            {
                //20012 失败要重发
                BlackboardUtils.SetValue<int>("./betIndex", 0);
                long totalBet = _consoleBB.Instance.betList[0];
                BlackboardUtils.SetValue<long>("./totalBet", totalBet);

                bool isBreak = false;
                do 
                {
                    Debug.LogWarning($"【Test】 设置压注： {totalBet}");
                    MachineDataManager.Instance.RequestSetPlayerBets(0, totalBet, (res) =>
                    {
                        int result = (int)res;

                        if (result ==0)
                        {
                            isBreak = true;
                        }
                        else
                        {
                            DebugUtils.LogError($"set total bet for machine is err :{result}");
                        }
   
                        isNext = true;
                    });

                    yield return new WaitUntil(() => isNext == true);
                    isNext = false;

                } while (!isBreak);
            }

            MaskPopupHandler.Instance.ClosePopup();

            // 通知重新获取彩金值
            EventCenter.Instance.EventTrigger<EventData>(GlobalEvent.ON_DEVICE_EVENT, new EventData(GlobalEvent.CodeCompleted));
        }

        // 延时打开？？
        TipPopupHandler.Instance.OpenPopup(I18nMgr.T(isSuccess ? "Coding activation successful" : "Coding activation failed"));
    }





    void SyncPlayerCredit()
    {
        // 同步玩家金额
        MachineDataManager.Instance.RequestGetPlayerInfo((res) =>
        {
            SBoxAccount data = (SBoxAccount)res;
            int pid = _consoleBB.Instance.pid;
            List<SBoxPlayerAccount> playerAccountList = data.PlayerAccountList;
            for (int i = 0; i < playerAccountList.Count; i++)
            {
                if (playerAccountList[i].PlayerId == pid)
                {
                    _consoleBB.Instance.sboxPlayerInfo = playerAccountList[i];

                    MainBlackboardController.Instance.SyncMyTempCreditToReal(true); //同步玩家金币
                    return;
                }
            }
        }, (BagelCodeError err) =>
        {
            DebugUtils.Log(err.msg);
        });
    }


}
