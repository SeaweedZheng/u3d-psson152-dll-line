#define SQLITE_ASYNC
using GameMaker;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _consoleBB = PssOn00152.ConsoleBlackboard02;
/// <summary>
/// Ͷ��
/// </summary>
public partial class DeviceCoinIn : MonoSingleton<DeviceCoinIn>
{

    const string COR_COIN_IN_OUT_TIME = "COR_COIN_IN_OUT_TIME";
    const string DEVICE_COIN_IN_ORDER = "device_coin_in_order";
    const string DEVICE_COIN_IN_NUM = "device_coin_in_num";

    string orderIdCoinIn;
    int lastCoinInId = -1;


    private CorController _corCtrl;
    private CorController corCtrl
    {
        get
        {
            if (_corCtrl == null)
                _corCtrl = new CorController(this);
            return _corCtrl;
        }
    }

    public void ClearCor(string name) => corCtrl.ClearCor(name);

    public void DoCor(string name, IEnumerator routine) => corCtrl.DoCor(name, routine);

    public bool IsCor(string name) => corCtrl.IsCor(name);

    public IEnumerator DoTask(Action cb, int ms) => corCtrl.DoTask(cb, ms);


    JSONNode _cacheCoinInOrder = null;
    JSONNode cacheCoinInOrder
    {
        get
        {
            if (_cacheCoinInOrder == null)
                _cacheCoinInOrder = JSONNode.Parse(SQLitePlayerPrefs03.Instance.GetString(DEVICE_COIN_IN_ORDER, "{}"));
            return _cacheCoinInOrder;
        }
        //set => _cacheCoinInOrder = value;
    }

    JSONNode _cacheCoinInNum = null;
    JSONNode cacheCoinInNum
    {
        get
        {
            if (_cacheCoinInNum == null)
                _cacheCoinInNum = JSONNode.Parse(SQLitePlayerPrefs03.Instance.GetString(DEVICE_COIN_IN_NUM, "{}"));
            return _cacheCoinInNum;
        }
    }

    public bool isRegularCoinIning
    {
        get => IsCor(COR_COIN_IN_OUT_TIME);
    }
    private void OnEnable()
    {
        //if (!ApplicationSettings.Instance.isMachine) return;

        EventCenter.Instance.AddEventListener<CoinInData>(SBoxSanboxEventHandle.COIN_IN, OnHardwareCoinIn);
        EventCenter.Instance.AddEventListener<EventData>(GlobalEvent.ON_COIN_IN_OUT_EVENT, OnCLearAllOrderCache);
    }
    private void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener<CoinInData>(SBoxSanboxEventHandle.COIN_IN, OnHardwareCoinIn);
        EventCenter.Instance.RemoveEventListener<EventData>(GlobalEvent.ON_COIN_IN_OUT_EVENT, OnCLearAllOrderCache);
    }

    private void OnCLearAllOrderCache(EventData res)
    {
        if (res.name == GlobalEvent.ClearAllOrderCache)
        {
            _cacheCoinInOrder = null;
            _cacheCoinInNum = null;
            SQLitePlayerPrefs03.Instance.SetString(DEVICE_COIN_IN_ORDER, "{}");
            SQLitePlayerPrefs03.Instance.SetString(DEVICE_COIN_IN_NUM, "{}");
        }
    }

    
    private void OnHardwareCoinIn(CoinInData coinInData)
    {

        DebugUtils.LogWarning($"CoinIn id = {coinInData.id} value = {coinInData.coinNum}");

        if (coinInData.coinNum <= 0)
        {
            return;
        }

        if (BlackboardUtils.IsBlackboard("./") && BlackboardUtils.GetValue<bool>("./isSpin"))
        {
            BlackboardUtils.SetValue<bool>("./isRequestToRealCreditWhenStop",true);
        }




        MachineDataManager.Instance.RequestCounter(0, coinInData.coinNum, 2, (res) =>
        {
            int resault = (int)res;
            if (resault < 0)
                DebugUtils.LogError($"Ͷ����� : ����״̬��{resault}  Ͷ�Ҹ�����{coinInData.coinNum}");
            else
                DebugUtils.Log($"Ͷ����� : ����״̬��{resault}  Ͷ�Ҹ�����{coinInData.coinNum}");
        });


        DeviceOrderReship.Instance.DelayReshipOrderRepeat();


        string deviceId = $"{coinInData.id}";
        /*if (!cacheCoinInNum.HasKey(deviceId))
        {
            JSONNode nodeOrder = JSONNode.Parse("{}");
            nodeOrder["count"] = coinInData.coinNum;
            nodeOrder["timestamp"] = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            cacheCoinInNum.Add(deviceId, nodeOrder);
        }
        else
        {
            cacheCoinInNum[deviceId]["count"] += coinInData.coinNum;
            cacheCoinInNum[deviceId]["timestamp"] = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }*/

        cacheCoinInNum[deviceId]["count"] += coinInData.coinNum;
        cacheCoinInNum[deviceId]["scale"] = _consoleBB.Instance.coinInScale;
        cacheCoinInNum[deviceId]["timestamp"] = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        GlobalSoundHelper.Instance.PlaySound(GameMaker.SoundKey.MachineCoinIn);

        SQLitePlayerPrefs03.Instance.SetString(DEVICE_COIN_IN_NUM, cacheCoinInNum.ToString());


        // ��Ǯ���� 
        int addCredit = coinInData.coinNum * _consoleBB.Instance.coinInScale;
        Debug.Log($"��ǰ��{MainBlackboard.Instance.myCredit}  ��ӽ�{addCredit} = {MainBlackboard.Instance.myCredit + addCredit}");
        MainBlackboardController.Instance.AddMyTempCredit(addCredit, true, true);


        if (lastCoinInId != -1 && lastCoinInId != coinInData.id)
        {
            ClearCor(COR_COIN_IN_OUT_TIME);
            AddCoin(lastCoinInId);
            lastCoinInId = coinInData.id;
        }

        // ��Ǯ���� 
        //int addCredit = coinInData.coinNum * _consoleBB.Instance.coinInScale;
        //Debug.Log($"��ǰ��{MainBlackboard.Instance.myCredit}  ��ӽ�{addCredit} = { MainBlackboard.Instance.myCredit + addCredit}");
        //MainBlackboardController.Instance.AddMyUICredit(addCredit, true, true);


        lastCoinInId = coinInData.id;
        DoCor(COR_COIN_IN_OUT_TIME, DoTask(() =>
        {
            AddCoin(lastCoinInId);
            lastCoinInId = -1;
        }, 301)); //��ʱ�����ظ�����
    }



    void AddCoin(int deviceNumber)
    {

        /* �������Ʊ�У���Ͷ�Ҳ��������㷨�����ˣ�ֻ����ui�����Ͷ�Ҽ�Ǯ����
         * �ȶ�������ʱ�Ž������˴���
         */
        if (DeviceCoinOut.Instance.isRegularCoinOuting)
        {
            return;
        }
       

        int count = cacheCoinInNum[$"{deviceNumber}"]["count"];
        int scale = cacheCoinInNum[$"{deviceNumber}"]["scale"];
        cacheCoinInNum[$"{deviceNumber}"]["count"] = 0;
        cacheCoinInNum[$"{deviceNumber}"]["timestamp"] = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        SQLitePlayerPrefs03.Instance.SetString(DEVICE_COIN_IN_NUM, cacheCoinInNum.ToString());

        //JSONNode nd = SimpleJSON.JSONNode.Parse(string.Format("{{\"timestamp\":{0},\"id\":{1},\"count\":{2}}}", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), id, count));

        int coinInCredit = scale * count;

        //���������뻺��:
        orderIdCoinIn = OrderIdCreater.Instance.CreatOrderId(OrderIdCreater.CoinIn); //Guid.NewGuid().ToString();
        JSONNode nodeOrder = JSONNode.Parse("{}");
        nodeOrder["type"] = "coin_in";
        nodeOrder["order_id"] = orderIdCoinIn;
        nodeOrder["count"] = count;
        nodeOrder["scale"] = scale;
        nodeOrder["credit"] = coinInCredit;
        nodeOrder["device_number"] = deviceNumber;
        nodeOrder["timestamp"] = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        cacheCoinInOrder[orderIdCoinIn] = nodeOrder;
        SQLitePlayerPrefs03.Instance.SetString(DEVICE_COIN_IN_ORDER, cacheCoinInOrder.ToString());

        // Ӳ���ӿڣ�
        RequestAddCoin(orderIdCoinIn);

    }

    void RequestAddCoin(string orderIdCoinIn, Action onFinishCallback = null)
    {


        JSONNode nodeOrder = cacheCoinInOrder[orderIdCoinIn];

        int coinInCredit = (int)nodeOrder["credit"];
        int coinInNum = (int)nodeOrder["count"];

        MachineDataManager.Instance.RequestCoinIn(coinInNum, (Action<object>)((res) =>
        {

            long creditBefore = _consoleBB.Instance.myCredit;
            long creditAfter = _consoleBB.Instance.myCredit + coinInCredit;

            // ������涩��
            cacheCoinInOrder.Remove(orderIdCoinIn);
            SQLitePlayerPrefs03.Instance.SetString(DEVICE_COIN_IN_ORDER, cacheCoinInOrder.ToString());


#if !SQLITE_ASYNC

            string sql = SQLiteHelper01.SQLInsertTableData<TableCoinInOutRecordItem>(
                ConsoleTableName.TABLE_COIN_IN_OUT_RECORD,
                new TableCoinInOutRecordItem()
                {
                    device_type = nodeOrder["type"],
                    device_number = nodeOrder["device_number"],
                    order_id = nodeOrder["order_id"],
                    count = coinInNum,
                    credit = coinInCredit,
                    credit_after = creditAfter,
                    credit_before = creditBefore,
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
                    count = coinInNum,
                    credit = coinInCredit,
                    credit_after = creditAfter,
                    credit_before = creditBefore,
                    in_out = 1,
                    created_at = nodeOrder["timestamp"],
                });
            SQLiteAsyncHelper.Instance.ExecuteNonQueryAsync(sql);
#endif





            //ÿ��ͳ��
            TableBusniessDayRecordAsyncManager.Instance.AddTotalCoinIn((long)coinInCredit, creditAfter);

            //��Ǯ����
            _consoleBB.Instance.myCredit = creditAfter;
            //Debug.LogError($"��ǰ��{MainBlackboard.Instance.myCredit}  ͬ����{_consoleBB.Instance.myCredit}");

            // �����ڶ����У�������ͬ�����ս��
            // MainBlackboardController.Instance.SyncMyTempCreditToReal(true);  

            // �����Ѿ��ӹ�Ǯ
            MainBlackboardController.Instance.TrySyncMyCreditToReel();

            //EventCenter.Instance.EventTrigger<EventData>(MetaUIEvent.ON_CREDIT_EVENT, new EventData<bool>(MetaUIEvent.UpdateNaviCredit, true));

            onFinishCallback?.Invoke();
        }));
    }



    /// <summary>
    /// ��������
    /// </summary>
    public IEnumerator ReshipOrde()
    {

        bool isNext = false;

        JSONNode tmpCoinInNum = JSONNode.Parse(cacheCoinInNum.ToString());

        foreach (KeyValuePair<string, JSONNode> item in tmpCoinInNum)
        {

            string deviceNumber = item.Key;
            int count = cacheCoinInNum[deviceNumber]["count"];
            int scale = cacheCoinInNum[deviceNumber]["scale"];
            if (count > 0)
            {
                cacheCoinInNum[deviceNumber]["count"] = 0;
                cacheCoinInNum[deviceNumber]["timestamp"] = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                SQLitePlayerPrefs03.Instance.SetString(DEVICE_COIN_IN_NUM, cacheCoinInNum.ToString());

                //���������뻺��:
                orderIdCoinIn = OrderIdCreater.Instance.CreatOrderId(OrderIdCreater.CoinIn); //Guid.NewGuid().ToString();
                JSONNode nodeOrder = JSONNode.Parse("{}");
                nodeOrder["type"] = "coin_in";
                nodeOrder["order_id"] = orderIdCoinIn;
                nodeOrder["count"] = count;
                nodeOrder["scale"] = scale;
                nodeOrder["credit"] = count * scale;
                nodeOrder["device_number"] = deviceNumber;
                nodeOrder["timestamp"] = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                cacheCoinInOrder[orderIdCoinIn] = nodeOrder;
                SQLitePlayerPrefs03.Instance.SetString(DEVICE_COIN_IN_ORDER, cacheCoinInOrder.ToString());

            }
        }

        JSONNode tmpCoinInOrder = JSONNode.Parse(cacheCoinInOrder.ToString());

        foreach (KeyValuePair<string, JSONNode> item in tmpCoinInOrder)
        {

            DebugUtils.Log($"��OrderReship - CoinIn����order id = {item.Key}  credit = {(int)item.Value["credit"]}");
            RequestAddCoin(item.Key, () =>
            {
                isNext = true;
            });

            yield return new WaitUntil(() => isNext == true);
            isNext = false;
        }
    }

}


public partial class DeviceCoinIn : MonoSingleton<DeviceCoinIn>
{
    /// <summary>
    /// �����ʽ��н�Ͷ�Ҵ���
    /// </summary>
    /// <param name="count"></param>
    public void OnJackpotOnlineCoinIn(int count)
    {
        string orderId  = OrderIdCreater.Instance.CreatOrderId(OrderIdCreater.CoinIn); //Guid.NewGuid().ToString();
        int coinInCredit = _consoleBB.Instance.coinInScale * count;
        int coinInNum = count;

        MachineDataManager.Instance.RequestCoinIn(coinInNum, (Action<object>)((res) =>
        {
            long creditBefore = _consoleBB.Instance.myCredit;
            long creditAfter = creditBefore + coinInCredit;


#if !SQLITE_ASYNC

            string sql = SQLiteHelper01.SQLInsertTableData<TableCoinInOutRecordItem>(
                ConsoleTableName.TABLE_COIN_IN_OUT_RECORD,
                new TableCoinInOutRecordItem()
                {
                    device_type = "coin_in",
                    device_number = 0,
                    order_id = orderId,
                    count = coinInNum,
                    credit = coinInCredit,
                    credit_after = creditAfter,
                    credit_before = creditBefore,
                    in_out = 1,
                    created_at = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                });
            SQLiteHelper01.Instance.ExecuteNonQueryAfterOpenDB(sql);

#else
            string sql = SQLiteAsyncHelper.SQLInsertTableData<TableCoinInOutRecordItem>(
                ConsoleTableName.TABLE_COIN_IN_OUT_RECORD,
                new TableCoinInOutRecordItem()
                {
                    device_type = "coin_in",
                    device_number = 0,
                    order_id = orderId,
                    count = coinInNum,
                    credit = coinInCredit,
                    credit_after = creditAfter,
                    credit_before = creditBefore,
                    in_out = 1,
                    created_at = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                });
            SQLiteAsyncHelper.Instance.ExecuteNonQueryAsync(sql);

#endif




            //ÿ��ͳ��
            TableBusniessDayRecordAsyncManager.Instance.AddTotalCoinIn((long)coinInCredit, creditAfter);

            //�޸���ʵ���
            _consoleBB.Instance.myCredit += coinInCredit;
        }));

    }
}
public partial class DeviceCoinIn : MonoSingleton<DeviceCoinIn>
{

    /// <summary>
    /// Զ�̿���Ͷ��
    /// </summary>
    /// <param name="coinInCount"></param>
    /// <param name="onCallback"></param>
    public void DoCmdCoinIn( int coinInCount , Action<object> onCallback)
    {
        CoinInData data = new CoinInData();
        data.id = 0;
        data.coinNum = coinInCount;
        OnHardwareCoinIn(data);
        onCallback?.Invoke(coinInCount > 0);
    }
}