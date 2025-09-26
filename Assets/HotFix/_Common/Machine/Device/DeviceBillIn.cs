#define SQLITE_ASYNC
using Game;
using GameMaker;
using PssOn00152;
using SBoxApi;
using SimpleJSON;
using System;
using System.Collections.Generic;
using UnityEngine;
using _consoleBB = PssOn00152.ConsoleBlackboard02;


/// <summary>
/// 纸钞机
/// </summary>
/// <remarks>
/// * 纸钞机使用投币比例。
/// * 投一张钞票（如台币1000），收到数字20.则等于投入20个币
/// * 纸钞机收到多少数值，等于投入多少个币。
/// </remarks>
public partial class DeviceBillIn : CorBehaviour
{

    const string COR_INIT_BILL = "COR_INIT_BILL";
    const string DEVICE_Bill_IN_ORDER = "device_bill_in_order";
    const string COR_CHECK_BILLER_CONNECT = "COR_CHECK_BILLER_CONNECT";

    const string MARK_POP_BILLER_NOT_LINK = "MARK_POP_BILLER_NOT_LINK";

    //bool isBillerInit = false;

    //int deviceNumber;
    JSONNode _cacheBillInOrder = null;
    string orderIdBillIn;
    JSONNode cacheBillInOrder
    {
        get
        {
            if (_cacheBillInOrder == null)
                _cacheBillInOrder = JSONNode.Parse(SQLitePlayerPrefs03.Instance.GetString(DEVICE_Bill_IN_ORDER, "{}"));
            return _cacheBillInOrder;
        }
        //set => _cacheBillInOrder = value;
    }

    private void OnEnable()
    {
        //if (!ApplicationSettings.Instance.isMachine)  return;

        EventCenter.Instance.AddEventListener<int>(SBoxSanboxEventHandle.BILL_IN, OnHardwareBillIn);
        EventCenter.Instance.AddEventListener(SBoxSanboxEventHandle.BILL_STACKED, OnHardwareBillStacked);

        RepeatInitBiller();
    }
    private void OnDisable()
    {
        MachineDataManager.Instance?.RemoveRequestAt(RPC_MARK_DEVICE_BILLER_IN);

        EventCenter.Instance.RemoveEventListener<int>(SBoxSanboxEventHandle.BILL_IN, OnHardwareBillIn);
        EventCenter.Instance.RemoveEventListener(SBoxSanboxEventHandle.BILL_STACKED, OnHardwareBillStacked);

    }


    /// <summary>
    /// 重复复位打印机，直到复位成功
    /// </summary>
    void RepeatInitBiller()
    {
        DoCor(COR_INIT_BILL, DoTaskRepeat(()=>InitBiller(null,null), 5000));
    }

    void FirstOrRepeatInitBiller()
    {
        InitBiller();
        DoCor(COR_INIT_BILL, DoTaskRepeat(() => InitBiller(null, null), 5000));
    }
    const string RPC_MARK_DEVICE_BILLER_IN = "RPC_MARK_DEVICE_BILLER_IN";
    public void InitBiller(Action successCallback = null, Action<string> errorCallback = null)
    {
        _consoleBB.Instance.isInitBiller = false;
        _consoleBB.Instance.isConnectBiller = false;

        ClearCor(COR_CHECK_BILLER_CONNECT);


        MachineDataManager.Instance.RequestGetBillerList(
            (res) => {
                List<string> billerList = (List<string>)res;

                _consoleBB.Instance.sboxBillerList = billerList;

                if (_consoleBB.Instance.selectBillerNumber > billerList.Count - 1)
                    _consoleBB.Instance.selectBillerNumber = 0;


                if (!_consoleBB.Instance.isUseBiller)
                {
                    ClearCor(COR_INIT_BILL);
                    return;
                }



                MachineDataManager.Instance.RequestSelectBiller(_consoleBB.Instance.selectBillerNumber, (res) => {

                    bool isOk = (int)res == 0;
                    if (isOk)
                    {
                        DebugUtils.LogWarning("【biller】: 纸钞机初始化成功");
                        ClearCor(COR_INIT_BILL);
                        _consoleBB.Instance.isInitBiller = true;

                        CheckIsBillerConnect();

                        successCallback?.Invoke();
                    }
                    else
                    {
                        DebugUtils.LogWarning("【biller】: 纸钞机初始化失败");
                        errorCallback?.Invoke("纸钞机初始化失败");
                    }

                }, (err) => {
                    DebugUtils.LogError(err.msg);
                    errorCallback?.Invoke(err.msg);
                }, RPC_MARK_DEVICE_BILLER_IN);
            },
            (err) =>
            {
                DebugUtils.LogError(err.msg);
                errorCallback?.Invoke(err.msg);
            }, RPC_MARK_DEVICE_BILLER_IN
        );
    }


    void CheckIsBillerConnect()
    {
        _CheckIsBillerConnect();
        DoCor(COR_INIT_BILL, DoTaskRepeat(() => _CheckIsBillerConnect(), 8000));
    }


    void _CheckIsBillerConnect()
    {
        MachineDataManager.Instance.RequestIsBillerConnect((res) =>
        {
            int data = (int)res;
            _consoleBB.Instance.isConnectBiller = data == _consoleBB.Instance.selectBillerNumber;
            //Debug.LogError($"算法卡 纸钞机编号{data} ， 已选纸钞机编号： {_consoleBB.Instance.selectBillerNumber}");

            if (!_consoleBB.Instance.isConnectBiller)
            {
                if(PageManager.Instance.IndexOf(PageName.Console001PageConsoleMain) == -1 
                && PageManager.Instance.IndexOf(PageName.PO152PageGameMain1080) != -1 
                && _consoleBB.Instance.isMachineActive == true)
                {
                    //TipPopupHandler.Instance.OpenPopupOnce(I18nMgr.T("Biller not linked."));
                    CommonPopupHandler.Instance.OpenPopupSingle(new CommonPopupInfo()
                    {
                        text = I18nMgr.T("Biller not linked."),
                        type = CommonPopupType.OK,
                        buttonText1 = I18nMgr.T("OK"),
                        buttonAutoClose1 = true,
                        isUseXButton = false,
                        mark = MARK_POP_BILLER_NOT_LINK,
                    });
                    //Debug.LogError("纸钞机没有连接");
                }

            }else
            {
                //Debug.LogWarning("纸钞机已连接");
                CommonPopupHandler.Instance.ClosePopup(MARK_POP_BILLER_NOT_LINK);
            }
        }, RPC_MARK_DEVICE_BILLER_IN);
    }




    void RejectBill()
    {
        if (Application.isEditor)
        {
            money = 0;
            MatchDebugManager.Instance.SendUdpMessage(SBoxEventHandle.SBOX_SADNBOX_BILL_REJECT);
        }
        else
        {
            SBoxSandbox.BillReject();
        }
    }


   
    long cashSeq;
    int money;


    /// <summary>
    /// 收到钞票
    /// </summary>
    /// <param name="mny"></param>
    private void OnHardwareBillIn(int mny)
    {
        if (DeviceUtils.IsCurSasBiller())
            return;

        Debug.LogWarning($"OnBillIn  money: {mny}");

        // 普通打印机
        if (mny <= 0)
            return;

        if (!_consoleBB.Instance.isMachineActive)
        {
            RejectBill();
            DebugUtils.LogWarning("Machine not activated");
            return;
        }

        if (BlackboardUtils.GetValue<bool>("./isSpin"))
        {
            TipPopupHandler.Instance.OpenPopupOnce(I18nMgr.T("<size=26>Cannot recharge during the game period</size>"));
            RejectBill();
            return;
        }


        if (PlayerPrefsUtils.isUseSas)
        {

            MachineDataManager.Instance.RequestSasCashSeqScoreUp((res) =>
            {
                int[] data = res as int[];

                if (data[0] == 0)
                {
                    cashSeq = ((long)data[1] << 32) | (uint)data[2];

                    SasCommand.Instance.PushGeneralBillInDetails(mny , cashSeq); 
                    SasCommand.Instance.SetMeterBillInCash(mny, cashSeq);

                    DoReceiveCash(mny);
                }
                else
                {
                    RejectBill();
                    return;
                }
            });

        }
        else
        {
            DoReceiveCash(mny);
        }

        
    }


    private void DoReceiveCash(int mny)
    {

        money = mny;

        DebugUtils.LogWarning($"@ 收到金钱 {money}");

        // 创建记录
        //orderIdBillIn = Guid.NewGuid().ToString();

        orderIdBillIn = OrderIdCreater.Instance.CreatOrderId(OrderIdCreater.CoinIn);

        if (true)  //钞票是否允许收入
        {
            if (Application.isEditor)
            {
                MatchDebugManager.Instance.SendUdpMessage(SBoxEventHandle.SBOX_SADNBOX_BILL_APPROVE);
            }
            else
            {
                SBoxSandbox.BillApprove();
            }
        }
        else
        {
            RejectBill();
        }
    }






    /// <summary>
    /// 钞票进入钱箱
    /// </summary>
    private void OnHardwareBillStacked()
    {

        if (DeviceUtils.IsCurSasBiller())
            return;


        if(_consoleBB.Instance.isUseBiller == false)
        {
            RejectBill();
            TipPopupHandler.Instance.OpenPopupOnce(I18nMgr.T("Biller function not enabled."));
            return;
        }


        // 普通进票
        if (string.IsNullOrEmpty(orderIdBillIn))
            return;

        if(PlayerPrefsUtils.isUseSas)
            SasCommand.Instance.SetMeterBillInCash(money, cashSeq,100);


        //保存订单到缓存
        JSONNode nodeOrder = JSONNode.Parse("{}");
        nodeOrder["type"] = "bill_in";
        nodeOrder["order_id"] = orderIdBillIn;
        nodeOrder["device_number"] = _consoleBB.Instance.selectBillerNumber;
        nodeOrder["money"] = money;
        nodeOrder["timestamp"] = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        nodeOrder["credit_before"] = ConsoleBlackboard02.Instance.myCredit;


        cacheBillInOrder[orderIdBillIn] = nodeOrder;
        SQLitePlayerPrefs03.Instance.SetString(DEVICE_Bill_IN_ORDER, cacheBillInOrder.ToString());

        RequestBillIn(orderIdBillIn, () =>
        {
            //订单入库

            JSONNode nodeOrder = cacheBillInOrder[orderIdBillIn];
#if !SQLITE_ASYNC
            string sql = SQLiteHelper01.SQLInsertTableData<TableCoinInOutRecordItem>(
            ConsoleTableName.TABLE_COIN_IN_OUT_RECORD,
            new TableCoinInOutRecordItem()
            {
                device_type = nodeOrder["type"],
                device_number = nodeOrder["device_number"],
                order_id = nodeOrder["order_id"],
                count = 1, //nodeOrder["count"],
                credit = (long)nodeOrder["credit"],
                credit_after = nodeOrder["credit_after"],
                credit_before = nodeOrder["credit_before"],
                as_money = nodeOrder["money"],
                in_out = 1,
                created_at = nodeOrder["timestamp"],
            });
            SQLiteHelper01.Instance.ExecuteNonQueryAfterOpenDB(sql);

#else
            string sql = SQLiteAsyncHelper.SQLInsertTableData<TableCoinInOutRecordItem>(
            ConsoleTableName.TABLE_COIN_IN_OUT_RECORD,
            new TableCoinInOutRecordItem()
            {
                device_type = nodeOrder["type"],
                device_number = nodeOrder["device_number"],
                order_id = nodeOrder["order_id"],
                count = 1, //nodeOrder["count"],
                credit = (long)nodeOrder["credit"],
                credit_after = nodeOrder["credit_after"],
                credit_before = nodeOrder["credit_before"],
                as_money = nodeOrder["money"],
                in_out = 1,
                created_at = nodeOrder["timestamp"],
            });
            SQLiteAsyncHelper.Instance.ExecuteNonQueryAsync(sql);
#endif


            //TableBusniessDayRecordManager.Instance.AddTotalBillIn(nodeOrder["credit"], nodeOrder["money"], nodeOrder["credit_after"]);
            TableBusniessDayRecordAsyncManager.Instance.AddTotalCoinIn(nodeOrder["credit"], nodeOrder["credit_after"]);

            //删除缓存
            cacheBillInOrder.Remove(orderIdBillIn);

            money = 0;
            orderIdBillIn = null;
        });
    }



    void RequestBillIn(string orderId, Action successCallback)
    {

        long coinInNum = (long)cacheBillInOrder[orderId]["money"];
        long billInCredit = ConsoleBlackboard02.Instance.billInScale * coinInNum;

        MachineDataManager.Instance.RequestCoinIn((int)coinInNum, (res) =>
        {
            //long billInCredit = ConsoleBlackboard02.Instance.billInScale * (long)cacheBillInOrder[orderId]["money"];
            ConsoleBlackboard02.Instance.myCredit += billInCredit;

            MainBlackboardController.Instance.AddOrSyncMyCreditToReal(billInCredit);

            //GlobalSoundHelper.Instance.PlaySound(GameMaker.SoundKey.MachineCoinIn);

            cacheBillInOrder[orderId]["credit"] = billInCredit;
            cacheBillInOrder[orderId]["credit_after"] = ConsoleBlackboard02.Instance.myCredit;

            successCallback?.Invoke();

        });


        /*
            long billInCredit = ConsoleBlackboard02.Instance.billInScale * (long)cacheBillInOrder[orderId]["money"];
            ConsoleBlackboard02.Instance.myCredit += billInCredit;

            MainBlackboardController.Instance.AddOrSyncMyCreditToReal(billInCredit);

            //GlobalSoundHelper.Instance.PlaySound(GameMaker.SoundKey.MachineCoinIn);


            cacheBillInOrder[orderId]["credit"] = billInCredit;
            cacheBillInOrder[orderId]["credit_after"] = ConsoleBlackboard02.Instance.myCredit;

            successCallback?.Invoke();
         */
    }


}

public partial class DeviceBillIn : CorBehaviour
{


    private void OnEnable01()
    {

        EventCenter.Instance.AddEventListener<List<string>>(SBoxEventHandle.SBOX_SADNBOX_BILL_LIST_GET, OnSboxSandBoxBillListGet);
        EventCenter.Instance.AddEventListener<int>(SBoxEventHandle.SBOX_SADNBOX_BILL_SELECT, OnSboxSandBoxBillSelect);



        DoCor(COR_INIT_BILL, DoTaskRepeat(GetBillLst, 5000));
    }
    private void OnDisable01()
    {
        EventCenter.Instance.RemoveEventListener<List<string>>(SBoxEventHandle.SBOX_SADNBOX_BILL_LIST_GET, OnSboxSandBoxBillListGet);
        EventCenter.Instance.RemoveEventListener<int>(SBoxEventHandle.SBOX_SADNBOX_BILL_SELECT, OnSboxSandBoxBillSelect);
    }

    public void GetBillLst()
    {
        _consoleBB.Instance.isInitBiller = false;
        if (Application.isEditor)
        {
            MatchDebugManager.Instance.SendUdpMessage(SBoxEventHandle.SBOX_SADNBOX_BILL_LIST_GET);
        }
        else
        {
            SBoxSandbox.BillListGet();
        }
    }

    private void OnSboxSandBoxBillListGet(List<string> sandBoxBillList)
    {
        _consoleBB.Instance.sboxBillerList = sandBoxBillList;

        /*for (int j = 0; j< sandBoxBillList.Count; j++)
        {
            Debug.Log($"纸钞机列表 ： {j}  {sandBoxBillList[j]}");
        }
        int i = 0;
        foreach (var item in sandBoxBillList)
        {
            DebugUtil.Log($"【BillLst】sandBoxBillList: idx = {i}  val = {item}");
            i++;
        }*/

        //deviceNumber = 3; //存本地

        SBoxSandbox.BillSelect(_consoleBB.Instance.selectBillerNumber);
    }


    private void OnSboxSandBoxBillSelect(int res)
    {
        if (res == 0)
        {
            _consoleBB.Instance.isInitBiller = true;
            ClearCor(COR_INIT_BILL);
            DebugUtils.LogWarning($"【Biller】biller select success  res = {res}");
        }
        else
        {
            DebugUtils.LogWarning($"【Biller】biller select error  res = {res}");
        }

    }
}