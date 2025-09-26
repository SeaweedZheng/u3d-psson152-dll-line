#define SQLITE_ASYNC
using GameMaker;
using System;
using UnityEngine;
using _consoleBB = PssOn00152.ConsoleBlackboard02;


// 上下分短按: 按 投币比例
// 上分长按 ： 1000；
// 下分长按 请0


public class DeviceCreditUpDown : MonoSingleton<DeviceCreditUpDown>
{
    public  void CreditUp(bool isLongClick = false)
    {
        if (!_consoleBB.Instance.isMachineActive)
        {
            DebugUtils.LogWarning("Machine not activated");
            return;
        }

        /*if (BlackboardUtils.GetValue<bool>("./isSpin"))
        {
            TipPopupHandler.Instance.OpenPopupOnce(I18nMgr.T("<size=26>Cannot recharge during the game period</size>"));
            return;
        }*/

        int scoreUpCredit = 
            1 * (isLongClick? _consoleBB.Instance.scoreUpScaleLongClick: _consoleBB.Instance.scoreUpDownScale);

        MachineDataManager.Instance.RequestScoreUp(scoreUpCredit, (Action<object>)((object res) =>
        {
            int credit = (int)res;
            string orderId = OrderIdManager.Instance.CreatAndUseOrderId("score_up");

            TableCoinInOutRecordItem record = new TableCoinInOutRecordItem()
            {
                device_type = "score_up",
                device_number = -1,
                order_id = orderId,// Guid.NewGuid().ToString(),
                count = 1,
                credit_before = _consoleBB.Instance.myCredit,
                in_out = 1,
                created_at = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            };


            //long myCredit = _consoleBB.Instance.myCredit * 1;
            _consoleBB.Instance.myCredit += credit;
            //EventCenter.Instance.EventTrigger<EventData>(MetaUIEvent.ON_CREDIT_EVENT, new EventData<bool>(MetaUIEvent.UpdateNaviCredit, true));
            MainBlackboardController.Instance.AddOrSyncMyCreditToReal(credit);


            GlobalSoundHelper.Instance.PlaySound(GameMaker.SoundKey.MachineCoinIn);


            record.credit_after = _consoleBB.Instance.myCredit;
            record.credit = record.credit_after - record.credit_before;


#if !SQLITE_ASYNC
            string sql = SQLiteHelper01.SQLInsertTableData<TableCoinInOutRecordItem>(
                ConsoleTableName.TABLE_COIN_IN_OUT_RECORD, record);
            SQLiteHelper01.Instance.ExecuteNonQueryAfterOpenDB(sql);

#else
            string sql = SQLiteAsyncHelper.SQLInsertTableData<TableCoinInOutRecordItem>(
                ConsoleTableName.TABLE_COIN_IN_OUT_RECORD, record);
            SQLiteAsyncHelper.Instance.ExecuteNonQueryAsync(sql);
#endif


            OrderIdManager.Instance.CompleteOrderId(orderId);

            //每日统计
            TableBusniessDayRecordAsyncManager.Instance.AddTotalScoreUp(record.credit, record.credit_after);

        }));

        /*
        TableCoinInOutRecordItem record = new TableCoinInOutRecordItem()
        {
            device_type = "score_up",
            device_number = -1,
            order_id = Guid.NewGuid().ToString(),
            count = 1,
            credit_before = _consoleBB.Instance.myCredit,
            in_out = 1,
            created_at = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
        };

        int scoreUpCredit = _consoleBB.Instance.scoreUpDownScale * 1;
        //long myCredit = _consoleBB.Instance.myCredit * 1;
        _consoleBB.Instance.myCredit += scoreUpCredit;
        EventCenter.Instance.EventTrigger<EventData>(MetaUIEvent.ON_CREDIT_EVENT, new EventData<bool>(MetaUIEvent.UpdateNaviCredit, true));
        GlobalSoundHelper.Instance.PlaySound(GameMaker.SoundKey.MachineCoinIn);

        record.credit_after = _consoleBB.Instance.myCredit;
        record.credit = record.credit_after - record.credit_before;
        string sql = SQLiteHelper02.SQLInsertTableData<TableCoinInOutRecordItem>(
            ConsoleTableName.TABLE_COIN_IN_OUT_RECORD, record);
        SQLiteHelper02.Instance.ExecuteNonQueryAfterOpenDB(sql);
        */

    }


    /// <summary>
    /// 长按下分清0
    /// </summary>
    public void CreditAllDown()
    {
        if (!_consoleBB.Instance.isMachineActive)
        {
            DebugUtils.LogWarning("Machine not activated");
            return;
        }

        if (_consoleBB.Instance.myCredit <= 0)
            return;

        if (BlackboardUtils.GetValue<bool>("./isSpin"))
        {
            TipPopupHandler.Instance.OpenPopupOnce(I18nMgr.T("Cannot score down during the game period"));
            return;
        }


        int scoreDownCredit = (int)_consoleBB.Instance.myCredit;


        MachineDataManager.Instance.RequestScoreDown(scoreDownCredit, (Action<object>)((res) =>
        {
            int credit = (int)res;

            string orderId = OrderIdManager.Instance.CreatAndUseOrderId("score_down");

            TableCoinInOutRecordItem record = new TableCoinInOutRecordItem()
            {
                device_type = "score_down",
                device_number = -1,
                //order_id = Guid.NewGuid().ToString(),
                order_id = orderId,
                count = 1,
                credit_before = _consoleBB.Instance.myCredit,
                in_out = 0,
                created_at = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            };



            long myCredit = _consoleBB.Instance.myCredit;
            if (credit > myCredit)
                _consoleBB.Instance.myCredit = 0;
            else
                _consoleBB.Instance.myCredit = myCredit - credit;

            //EventCenter.Instance.EventTrigger<EventData>(MetaUIEvent.ON_CREDIT_EVENT, new EventData<bool>(MetaUIEvent.UpdateNaviCredit, true));

            MainBlackboardController.Instance.MinusOrSyncMyCreditToReal(credit);

            GlobalSoundHelper.Instance.PlaySound(GameMaker.SoundKey.MachineCoinIn);

            record.credit_after = _consoleBB.Instance.myCredit;
            record.credit = record.credit_before - record.credit_after;


#if !SQLITE_ASYNC

            string sql = SQLiteHelper01.SQLInsertTableData<TableCoinInOutRecordItem>(
                ConsoleTableName.TABLE_COIN_IN_OUT_RECORD, record);
            SQLiteHelper01.Instance.ExecuteNonQueryAfterOpenDB(sql);
#else
            string sql = SQLiteAsyncHelper.SQLInsertTableData<TableCoinInOutRecordItem>(
                ConsoleTableName.TABLE_COIN_IN_OUT_RECORD, record);
            SQLiteAsyncHelper.Instance.ExecuteNonQueryAsync(sql);
#endif


            OrderIdManager.Instance.CompleteOrderId(orderId);

            //每日统计
            TableBusniessDayRecordAsyncManager.Instance.AddTotalScoreDown(record.credit, record.credit_after);
        }));

    }
    public void CreditDown()
    {
        if (!_consoleBB.Instance.isMachineActive) {
            DebugUtils.LogWarning("Machine not activated");
            return;
        }

        if (_consoleBB.Instance.myCredit <= 0)
            return;

        if (BlackboardUtils.GetValue<bool>("./isSpin"))
        {
            TipPopupHandler.Instance.OpenPopupOnce(I18nMgr.T("Cannot score down during the game period"));
            return;
        }

        int scoreDownCredit = _consoleBB.Instance.scoreUpDownScale * 1;

        if (scoreDownCredit > _consoleBB.Instance.myCredit)
            scoreDownCredit = (int)_consoleBB.Instance.myCredit;


        MachineDataManager.Instance.RequestScoreDown(scoreDownCredit, (Action<object>)((res) =>
        {
            int credit = (int)res;

            string orderId = OrderIdManager.Instance.CreatAndUseOrderId("score_down");

            TableCoinInOutRecordItem record = new TableCoinInOutRecordItem()
            {
                device_type = "score_down",
                device_number = -1,
                order_id = orderId,// Guid.NewGuid().ToString(),
                count = 1,
                credit_before = _consoleBB.Instance.myCredit,
                in_out = 0,
                created_at = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            };


            long myCredit = _consoleBB.Instance.myCredit;
            if (credit > myCredit)
                _consoleBB.Instance.myCredit = 0;
            else
                _consoleBB.Instance.myCredit = myCredit - credit;

            //EventCenter.Instance.EventTrigger<EventData>(MetaUIEvent.ON_CREDIT_EVENT, new EventData<bool>(MetaUIEvent.UpdateNaviCredit, true));

            MainBlackboardController.Instance.MinusOrSyncMyCreditToReal(credit);

            GlobalSoundHelper.Instance.PlaySound(GameMaker.SoundKey.MachineCoinIn);

            record.credit_after = _consoleBB.Instance.myCredit;
            record.credit = record.credit_before - record.credit_after;


#if !SQLITE_ASYNC
            string sql = SQLiteHelper01.SQLInsertTableData<TableCoinInOutRecordItem>(
                ConsoleTableName.TABLE_COIN_IN_OUT_RECORD, record);
            SQLiteHelper01.Instance.ExecuteNonQueryAfterOpenDB(sql);

#else
            string sql = SQLiteAsyncHelper.SQLInsertTableData<TableCoinInOutRecordItem>(
                ConsoleTableName.TABLE_COIN_IN_OUT_RECORD, record);
            SQLiteAsyncHelper.Instance.ExecuteNonQueryAsync(sql);
#endif



            OrderIdManager.Instance.CompleteOrderId(orderId);

            //每日统计
            TableBusniessDayRecordAsyncManager.Instance.AddTotalScoreDown(record.credit, record.credit_after);
        }));


    }
}
